using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Timers;

using Cairo;
using Gtk;
using GUI = Gtk.Builder.ObjectAttribute;

using FoenixCore.Display;
using FoenixCore.MemoryLocations;


namespace FoenixToolkit.Display
{
    public class GpuControl : Box
    {
        const int ROW_HEIGHT = 14;
        const int BASELINE_OFFSET = ROW_HEIGHT - 2;

        private const int REGISTER_BLOCK_SIZE = 256;
        const int MAX_TEXT_COLS = 100;
        const int MAX_TEXT_LINES = 75;
        const int SCREEN_PAGE_SIZE = 128 * 64;

        const int CHAR_WIDTH = 8;
        const int CHAR_HEIGHT = 8;
        const byte TILE_SIZE = 16;
        const int SPRITE_SIZE = 32;

        public MemoryRAM VRAM = null;
        public MemoryRAM RAM = null;
        public MemoryRAM VICKY = null;
        public int paintCycle = 0;

        public delegate void StartOfFramEvent();
        public StartOfFramEvent StartOfFrame;
        public delegate void StartOfLineEvent();
        public StartOfLineEvent StartOfLine;

        public delegate void GpuUpdateFunction();
        public GpuUpdateFunction GpuUpdated;

        /// <summary>
        /// number of frames to wait to refresh the screen.
        /// One frame = 1/60 second.
        /// </summary>
        public int BlinkingCounter = BLINK_RATE;
        public const int BLINK_RATE = 30;

        // To provide a better contrast when writing on top of bitmaps
        private Cairo.Color lightBlue = new(System.Drawing.Color.LightBlue.R / 255.0, System.Drawing.Color.LightBlue.G / 255.0, System.Drawing.Color.LightBlue.B / 255.0);
        private Cairo.Color blue = new(System.Drawing.Color.Blue.R / 255.0, System.Drawing.Color.Blue.G / 255.0, System.Drawing.Color.Blue.B / 255.0);
        private Cairo.Color black = new(System.Drawing.Color.Black.R / 255.0, System.Drawing.Color.Black.G / 255.0, System.Drawing.Color.Black.B / 255.0);

        const int STRIDE = 800;
        readonly Bitmap frameBuffer = new(STRIDE, 600, PixelFormat.Format32bppArgb);
        private bool drawing = false;
        byte[] pixVals = null;

        int[] FGTextLUT;
        int[] BGTextLUT;
        bool CursorState = true;

        Timer gpuRefreshTimer = null;

        private static readonly int[] vs = new int[256 * 8];
        private int[] lutCache = vs;

#pragma warning disable CS0649  // never assigned
        [GUI] DrawingArea daGpu;
#pragma warning restore CS0649

        public GpuControl() : this(new Builder("GpuControl.ui")) { }

        private GpuControl(Builder builder) : base(builder.GetRawOwnedObject("GpuControl"))
        {
            builder.Autoconnect(this);
        }

        public System.Drawing.Point GetScreenSize()
        {
            byte MCRHigh = (byte)(VICKY.ReadByte(1) & 3); // Reading address $AF:0001

            System.Drawing.Point p = new(640, 480);

            switch (MCRHigh)
            {
                case 1:
                    p.X = 800;
                    p.Y = 600;
                    break;

                case 2:
                    p.X = 320;
                    p.Y = 240;
                    break;

                case 3:
                    p.X = 400;
                    p.Y = 300;
                    break;
            }

            return p;
        }

        // We only cache items that are requested, instead of precomputing all 1024 colors.
        private int GetLUTValue(in byte lutIndex, in byte color, in bool gamma)
        {
            var lc = lutCache;
            int value = lc[lutIndex * 256 + color];

            if (value == 0)
            {
                int lutAddress = MemoryMap.GRP_LUT_BASE_ADDR - MemoryMap.VICKY_BASE_ADDR + (lutIndex * 256 + color) * 4;

                byte red = VICKY.ReadByte(lutAddress);
                byte green = VICKY.ReadByte(lutAddress + 1);
                byte blue = VICKY.ReadByte(lutAddress + 2);

                if (gamma)
                {
                    int baseAddr = MemoryMap.GAMMA_BASE_ADDR - MemoryMap.VICKY_BASE_ADDR;
                    blue = VICKY.ReadByte(baseAddr + blue);           // gammaCorrection[fgValueBlue];
                    green = VICKY.ReadByte(baseAddr + 0x100 + green); // gammaCorrection[0x100 + fgValueGreen];
                    red = VICKY.ReadByte(baseAddr + 0x200 + red);     // gammaCorrection[0x200 + fgValueRed];
                }

                value = (int)((blue << 16) + (green << 8) + red + 0xFF000000);
                lc[lutIndex * 256 + color] = value;
            }

            return value;
        }

        private int[] GetTextLUT(byte fg, byte bg, bool gamma)
        {
            int[] values = new int[2];
            var fgt = FGTextLUT;

            if (fgt[fg] == 0)
            {
                // Read the color lookup tables
                int fgLUTAddress = MemoryMap.FG_CHAR_LUT_PTR - VICKY.StartAddress;

                // In order to reduce the load of applying Gamma correction, load single bytes
                byte fgValueRed = VICKY.ReadByte(fgLUTAddress + fg * 4);
                byte fgValueGreen = VICKY.ReadByte(fgLUTAddress + fg * 4 + 1);
                byte fgValueBlue = VICKY.ReadByte(fgLUTAddress + fg * 4 + 2);

                if (gamma)
                {
                    int baseAddr = MemoryMap.GAMMA_BASE_ADDR - MemoryMap.VICKY_BASE_ADDR;
                    fgValueBlue = VICKY.ReadByte(baseAddr + fgValueBlue); //gammaCorrection[fgValueBlue];
                    fgValueGreen = VICKY.ReadByte(baseAddr + 0x100 + fgValueGreen);//gammaCorrection[0x100 + fgValueGreen];
                    fgValueRed = VICKY.ReadByte(baseAddr + 0x200 + fgValueRed);//gammaCorrection[0x200 + fgValueRed];
                }

                values[0] = (int)((fgValueBlue << 16) + (fgValueGreen << 8) + fgValueRed + 0xFF000000);
                fgt[fg] = values[0];
            }
            else
                values[0] = fgt[fg];

            var bgt = BGTextLUT;
            if (bgt[bg] == 0)
            {
                // Read the color lookup tables
                int bgLUTAddress = MemoryMap.BG_CHAR_LUT_PTR - VICKY.StartAddress;

                byte bgValueRed = VICKY.ReadByte(bgLUTAddress + bg * 4);
                byte bgValueGreen = VICKY.ReadByte(bgLUTAddress + bg * 4 + 1);
                byte bgValueBlue = VICKY.ReadByte(bgLUTAddress + bg * 4 + 2);

                if (gamma)
                {
                    int baseAddr = MemoryMap.GAMMA_BASE_ADDR - MemoryMap.VICKY_BASE_ADDR;
                    bgValueBlue = VICKY.ReadByte(baseAddr + bgValueBlue); //gammaCorrection[bgValueBlue];
                    bgValueGreen = VICKY.ReadByte(baseAddr + 0x100 + bgValueGreen); //gammaCorrection[0x100 + bgValueGreen];
                    bgValueRed = VICKY.ReadByte(baseAddr + 0x200 + bgValueRed); //gammaCorrection[0x200 + bgValueRed];
                }

                values[1] = (int)((bgValueBlue << 16) + (bgValueGreen << 8) + bgValueRed + 0xFF000000);
                bgt[bg] = values[1];
            }
            else
                values[1] = bgt[bg];

            return values;
        }

        /*
        * Display the text with a colored background. This should make the text more visible against bitmaps.
        */
        private void DrawTextWithBackground(string text, Cairo.Context cr, int x, int y)
        {
            cr.SelectFontFace("Consolas", Cairo.FontSlant.Normal, Cairo.FontWeight.Normal);
            cr.SetFontSize(12);

            cr.SetSourceColor(black);
            cr.MoveTo(x, y + BASELINE_OFFSET);
            cr.ShowText(text);

            cr.MoveTo(x + 2, y + BASELINE_OFFSET);
            cr.ShowText(text);

            cr.MoveTo(x, y + 2 + BASELINE_OFFSET);
            cr.ShowText(text);

            cr.MoveTo(x + 2, y + 2 + BASELINE_OFFSET);
            cr.ShowText(text);

            cr.SetSourceColor(lightBlue);
            cr.MoveTo(x + 1, y + 1 + BASELINE_OFFSET);
            cr.ShowText(text);
        }

        private void DrawBitmapText(ref byte[] buffer, int MCR, bool gammaCorrection, byte TextColumns, byte TextRows, int colOffset, int rowOffset, int line, int width, int height)
        {
            bool overlayBitSet = (MCR & 0x02) == 0x02;

            int lineStartAddress = MemoryMap.SCREEN_PAGE0 - VICKY.StartAddress;
            int colorStartAddress = MemoryMap.SCREEN_PAGE1 - VICKY.StartAddress;
            int fontBaseAddress = MemoryMap.FONT_MEMORY_BANK_START - VICKY.StartAddress;

            // Find which line of characters to display
            int txtline = (line - rowOffset) / CHAR_HEIGHT;

            // Initialize the LUTs
            FGTextLUT = new int[16];
            BGTextLUT = new int[16];

            // Cursor Values
            byte CursorY = RAM.ReadByte(MemoryMap.CURSORY);
            byte CursorX = RAM.ReadByte(MemoryMap.CURSORX);
            bool CursorEnabled = (VICKY.ReadByte(MemoryMap.VKY_TXT_CURSOR_CTRL_REG - MemoryMap.VICKY_START) & 1) != 0;

            // Each character is defined by 8 bytes
            int fontRaster = (line - rowOffset) % CHAR_HEIGHT;

            uint ptr = (uint)(line * (width << 2) + (colOffset << 2));

            for (int col = 0; col < width / CHAR_WIDTH; ++col)
            {
                int x = col * CHAR_WIDTH;
                if (x > width - 1 - (colOffset << 1))
                    continue;

                int offset = 0;
                if (col < TextColumns)
                    offset = TextColumns * txtline + col;

                int textAddr = lineStartAddress + offset;
                int colorAddr = colorStartAddress + offset;

                // Each character will have foreground and background colors
                byte character = VICKY.ReadByte(textAddr);
                byte color = VICKY.ReadByte(colorAddr);

                // Display the cursor
                if (CursorX == col && CursorY == txtline && CursorState && CursorEnabled)
                    character = VICKY.ReadByte(MemoryMap.VKY_TXT_CURSOR_CHAR_REG - VICKY.StartAddress);

                byte fgColor = (byte)((color & 0xF0) >> 4);
                byte bgColor = (byte)(color & 0x0F);

                int[] textColors = GetTextLUT(fgColor, bgColor, gammaCorrection);
                // System.Console.WriteLine($"fg: {textColors[0].ToString("X4")}  bg: {textColors[1].ToString("X4")}");

                byte fontRasterBits = VICKY.ReadByte(fontBaseAddress + character * 8 + fontRaster);

                // For each bit in the font, set the foreground color - if the bit is 0 and overlay is set, skip it (keep the background)
                for (int b = 0x80; b > 0; b >>= 1)
                {
                    if ((fontRasterBits & b) != 0)
                    {
                        // foreground color; bit is set
                        buffer[ptr++] = (byte)(textColors[0] & 0xFF);           // blue
                        buffer[ptr++] = (byte)((textColors[0] >> 8) & 0xFF);    // green
                        buffer[ptr++] = (byte)((textColors[0] >> 16) & 0xFF);   // red
                        buffer[ptr++] = (byte)((textColors[0] >> 24) & 0xFF);   // alpha
                    }
                    else if (!overlayBitSet)
                    {
                        // background color; bit is clear
                        buffer[ptr++] = (byte)(textColors[1] & 0xFF);           // blue
                        buffer[ptr++] = (byte)((textColors[1] >> 8) & 0xFF);    // green
                        buffer[ptr++] = (byte)((textColors[1] >> 16) & 0xFF);   // red
                        buffer[ptr++] = (byte)((textColors[1] >> 24) & 0xFF);   // alpha
                    }
                }
            }
        }

        private void DrawBitmap(ref byte[] buffer, bool gammaCorrection, int layer, bool bkgrnd, int bgndColor, int borderXSize, int borderYSize, int line, int width, int height)
        {
            // Bitmap Controller is located at $AF:0100 and $AF:0108
            int regAddr = MemoryMap.BITMAP_CONTROL_REGISTER_ADDR - MemoryMap.VICKY_BASE_ADDR + layer * 8;

            byte reg = VICKY.ReadByte(regAddr);
            if ((reg & 0x01) == 00)
                return;

            byte lutIndex = (byte)((reg >> 1) & 7);  // 8 possible LUTs

            int bitmapAddress = VICKY.ReadLong(regAddr + 1);
            int xOffset = VICKY.ReadWord(regAddr + 4);
            int yOffset = VICKY.ReadWord(regAddr + 6);

            int colorVal = 0;
            int offsetAddress = bitmapAddress + line * width;
            int pixelOffset = line * (width << 2);

            uint ptr = (uint)(line * (width << 2));

            byte pixVal = 0;

            VRAM.CopyIntoBuffer(offsetAddress, width, pixVals);

            int lutAddress = MemoryMap.GRP_LUT_BASE_ADDR - MemoryMap.VICKY_BASE_ADDR + lutIndex * 1024;

            for (uint col = (uint)borderXSize; col < width - borderXSize; ++col)
            {
                colorVal = bgndColor;
                pixVal = pixVals[col];
                uint bitplane = ptr + col;

                if (pixVal != 0)
                {
                    colorVal = GetLUTValue(lutIndex, pixVal, gammaCorrection);
                    buffer[bitplane++] = (byte)(colorVal & 0xFF);           // blue
                    buffer[bitplane++] = (byte)((colorVal >> 8) & 0xFF);    // green
                    buffer[bitplane++] = (byte)((colorVal >> 16) & 0xFF);   // red
                    buffer[bitplane++] = (byte)((colorVal >> 24) & 0xFF);   // alpha
                }
            }
        }

        private void DrawTiles(ref byte[] buffer, bool gammaCorrection, byte TextColumns, int layer, bool bkgrnd, in int borderXSize, in int line, in int width)
        {
            // There are four possible tilemaps to choose from
            int addrTileCtrlReg = MemoryMap.TILE_CONTROL_REGISTER_ADDR - MemoryMap.VICKY_BASE_ADDR + layer * 12;
            int reg = VICKY.ReadByte(addrTileCtrlReg);

            // if the set is not enabled, we're done.
            if ((reg & 0x01) == 00)
                return;

            int tilemapWidth = VICKY.ReadWord(addrTileCtrlReg + 4) & 0x3FF;   // 10 bits
            int tilemapHeight = VICKY.ReadWord(addrTileCtrlReg + 6) & 0x3FF;  // 10 bits
            int tilemapAddress = VICKY.ReadLong(addrTileCtrlReg + 1);

            int tilemapWindowX = VICKY.ReadWord(addrTileCtrlReg + 8);
            bool dirUp = (tilemapWindowX & 0x4000) != 0;
            byte scrollX = (byte)(tilemapWindowX & 0x3C00 >> 10);
            tilemapWindowX &= 0x3FF;
            int tileXOffset = tilemapWindowX % TILE_SIZE;

            int tilemapWindowY = VICKY.ReadWord(addrTileCtrlReg + 10);
            bool dirRight = (tilemapWindowY & 0x4000) != 0;
            byte scrollY = (byte)(tilemapWindowY & 0x3C00 >> 10);
            tilemapWindowY &= 0x3FF;

            int tileRow = (line + tilemapWindowY) / TILE_SIZE;
            int tileYOffset = (line + tilemapWindowY) % TILE_SIZE;

            // we always read tiles 0 to width/TILE_SIZE + 1 - this is to ensure we can display partial tiles, with X,Y offsets
            int tilemapItemCount = width / TILE_SIZE + 1;
            byte[] tiles = new byte[tilemapItemCount * 2];
            int[] tilesetOffsets = new int[tilemapItemCount];

            VRAM.CopyIntoBuffer(tilemapAddress + (1 + tilemapWindowX / TILE_SIZE) * 2 + (tileRow + 0) * tilemapWidth * 2, tilemapItemCount * 2, tiles);

            // cache of tilesetPointers
            int[] tilesetPointers = new int[8];
            int[] strides = new int[8];

            for (int i = 0; i < 8; ++i)
            {
                tilesetPointers[i] = VICKY.ReadLong(MemoryMap.TILESET_BASE_ADDR - MemoryMap.VICKY_BASE_ADDR + i * 4);
                byte tilesetConfig = VICKY.ReadByte(MemoryMap.TILESET_BASE_ADDR - MemoryMap.VICKY_BASE_ADDR + i * 4 + 3);
                strides[i] = (tilesetConfig & 8) != 0 ? 256 : 16;
            }

            for (int i = 0; i < tilemapItemCount; ++i)
            {
                byte tile = tiles[i * 2];
                byte tilesetReg = tiles[i * 2 + 1];
                byte tileset = (byte)(tilesetReg & 7);

                // tileset
                int tilesetPointer = tilesetPointers[tileset];
                int strideX = strides[tileset];
                if (strideX == 16)
                    tilesetOffsets[i] = tilesetPointer + tile * 256 + tileYOffset * 16;
                else
                    tilesetOffsets[i] = tilesetPointer + ((tile / TILE_SIZE) * strideX * TILE_SIZE + (tile % TILE_SIZE) * TILE_SIZE) + tileYOffset * strideX;
            }

            uint ptr = (uint)(line * (width << 2));

            // alternate display style - avoids repeating the loop so often
            int startTileX = (borderXSize + tileXOffset) / TILE_SIZE;
            int endTileX = (width - borderXSize + tileXOffset) / TILE_SIZE + 1;
            int startOffset = (borderXSize + tilemapWindowX) % TILE_SIZE;
            int x = borderXSize;
            byte[] tilepix = new byte[16];
            int clrVal = 0;

            for (int t = startTileX; t < endTileX; t++)
            {
                byte tilesetReg = tiles[t * 2 + 1];
                byte lutIndex = (byte)((tilesetReg & 0x38) >> 3);
                int lutAddress = MemoryMap.GRP_LUT_BASE_ADDR - MemoryMap.VICKY_BASE_ADDR + lutIndex * 1024;
                int tilesetOffsetAddress = tilesetOffsets[t];  // + startOffset

                VRAM.CopyIntoBuffer(tilesetOffsetAddress, 16, tilepix);

                do
                {
                    byte pixVal = tilepix[startOffset];
                    if (pixVal > 0)
                    {
                        clrVal = GetLUTValue(lutIndex, pixVal, gammaCorrection);
                        //-- causing index outside the bounds of array
                        buffer[ptr + (x << 2)] = (byte)(clrVal & 0xFF);
                        buffer[ptr + (x << 2) + 1] = (byte)((clrVal >> 8) & 0xFF);
                        buffer[ptr + (x << 2) + 2] = (byte)((clrVal >> 16) & 0xFF);
                        buffer[ptr + (x << 2) + 3] = (byte)((clrVal >> 24) & 0xFF);
                    }

                    x++;
                    startOffset++;
                    tilesetOffsetAddress++;
                } while (startOffset != 16);

                startOffset = 0;
            }
        }

        private void DrawSprites(ref byte[] buffer, bool gammaCorrection, byte layer, bool bkgrnd, int borderXSize, int borderYSize, int line, int width, int height)
        {
            // There are 32 possible sprites to choose from.
            for (int s = 63; s >= 0; --s)
            {
                int addrSprite = MemoryMap.SPRITE_CONTROL_REGISTER_ADDR + s * 8 - MemoryMap.VICKY_BASE_ADDR;
                byte reg = VICKY.ReadByte(addrSprite);

                // if the set is not enabled, we're done.
                byte spriteLayer = (byte)((reg & 0x70) >> 4);
                int posY = VICKY.ReadWord(addrSprite + 6) - 32;

                if ((reg & 1) != 0 && layer == spriteLayer && (line >= posY && line < posY + 32))
                {
                    // TODO Fix this when Vicky II fixes the LUT issue
                    byte lutIndex = (byte)(((reg & 14) >> 1));  // 8 possible LUTs 
                    bool striding = (reg & 0x80) == 0x80;

                    int spriteAddress = VICKY.ReadLong(addrSprite + 1);
                    int posX = VICKY.ReadWord(addrSprite + 4) - 32;

                    if (posX >= (width - borderXSize) || posY >= (height - borderYSize) || posX < 0 || posY < 0)
                        continue;

                    int spriteWidth = 32;
                    int xOffset = 0;

                    // Check for sprite bleeding on the left-hand-side
                    if (posX < borderXSize)
                    {
                        xOffset = borderXSize - posX;
                        posX = borderXSize;
                        spriteWidth = 32 - xOffset;

                        if (spriteWidth == 0)
                            continue;
                    }

                    // Check for sprite bleeding on the right-hand side
                    if (posX + 32 > width - borderXSize)
                    {
                        spriteWidth = 640 - borderXSize - posX;
                        if (spriteWidth == 0)
                            continue;
                    }

                    int value = 0;
                    byte pixelIndex = 0;

                    // Sprites are 32 x 32
                    int sline = line - posY;
                    int lineOffset = line * (width << 2);
                    uint ptr = (uint)(line * (width << 2));

                    for (int col = xOffset; col < xOffset + spriteWidth; col++)
                    {
                        // Lookup the pixel in the tileset - if the value is 0, it's transparent
                        pixelIndex = VRAM.ReadByte(spriteAddress + col + sline * 32);

                        if (pixelIndex != 0)
                        {
                            value = GetLUTValue(lutIndex, pixelIndex, gammaCorrection);
                            buffer[ptr + col - xOffset + posX] = (byte)(value & 0xFF);
                            buffer[ptr + col - xOffset + posX + 1] = (byte)((value >> 8) & 0xFF);
                            buffer[ptr + col - xOffset + posX + 2] = (byte)((value >> 16) & 0xFF);
                            buffer[ptr + col - xOffset + posX + 3] = (byte)((value >> 24) & 0xFF);
                        }
                    }
                }
            }
        }

        private void DrawMouse(ref byte[] buffer, bool gammaCorrection, ref int drawAtX, ref int drawAtY)
        {
            byte mouseReg = VICKY.ReadByte(MemoryMap.MOUSE_PTR_REG - MemoryMap.VICKY_BASE_ADDR);

            // Verify mouse is enabled
            if ((mouseReg & 1) != 1)
            {
                drawAtX = -1;
                drawAtY = -1;
                return;
            }

            // Verify the mouse overlaps the line being drawn
            drawAtX = VICKY.ReadWord(0x702);
            drawAtY = VICKY.ReadWord(0x704);

            int pointerAddress =
                (mouseReg & 2) == 0 ?
                    MemoryMap.MOUSE_PTR_GRAP0 - MemoryMap.VICKY_BASE_ADDR :
                    MemoryMap.MOUSE_PTR_GRAP1 - MemoryMap.VICKY_BASE_ADDR;

            for (int row = 0; row < 16; ++row)
                for (int col = 0; col < 16; ++col)
                {
                    int bitplane = row * 64 + col * 4;

                    // Values are 0: transparent, 1:black, 255: white (gray scales)
                    byte pixelIndexR = VICKY.ReadByte(pointerAddress + row * 16 + col);
                    byte pixelIndexG = pixelIndexR;
                    byte pixelIndexB = pixelIndexR;
                    bool transparent = pixelIndexR == 0;

                    if (pixelIndexR != 0)
                    {
                        if (gammaCorrection)
                        {
                            pixelIndexB = VICKY.ReadByte(MemoryMap.GAMMA_BASE_ADDR - MemoryMap.VICKY_BASE_ADDR + pixelIndexR);          // gammaCorrection[pixelIndexR];
                            pixelIndexG = VICKY.ReadByte(MemoryMap.GAMMA_BASE_ADDR - MemoryMap.VICKY_BASE_ADDR + 0x100 + pixelIndexR);  // gammaCorrection[0x100 + pixelIndexR];
                            pixelIndexR = VICKY.ReadByte(MemoryMap.GAMMA_BASE_ADDR - MemoryMap.VICKY_BASE_ADDR + 0x200 + pixelIndexR);  // gammaCorrection[0x200 + pixelIndexR];
                        }

                        buffer[bitplane++] = pixelIndexB;
                        buffer[bitplane++] = pixelIndexG;
                        buffer[bitplane++] = pixelIndexR;
                        buffer[bitplane++] = transparent ? (byte)0x00 : (byte)0xFF;
                    }
                }
        }

        public bool IsMousePointerVisible()
        {
            // Read the mouse pointer register
            byte mouseReg = VICKY.ReadByte(0x700);

            return (mouseReg & 1) != 0;
        }

        /// <summary>
        /// Loads a font set into RAM and adds it to the character set table.
        /// </summary>
        /// <param name="Address"></param>
        /// <param name="Filename"></param>
        public void LoadFontSet(string Name, string Filename, int Offset, CharacterSet.CharTypeCodes CharType, CharacterSet.SizeCodes CharSize)
        {
            CharacterSet cs = new();

            // Load the data from the file into the  IO buffer - starting at address $AF8000
            cs.Load(Filename, Offset, VICKY, MemoryMap.FONT_MEMORY_BANK_START & 0xffff, CharSize);
        }

        private void on_GpuControl_map(object sender, EventArgs e)
        {
            gpuRefreshTimer = new()
            {
               Interval = 15,
               Enabled = false,
               AutoReset = true
            };

            gpuRefreshTimer.Elapsed += on_gpuRefreshTimer_tick;

            var parent = Parent;
            if (parent == null)
                 return;

            //-- int htarget = 320;
            // int topmargin = parent.AllocatedHeight - daGpu.AllocatedHeight;
            // int sidemargin = parent.AllocatedWidth - daGpu.AllocatedWidth;
            // parent.WidthRequest = (int)Math.Ceiling(htarget * 1.6) + sidemargin;
            // parent.HeightRequest = htarget + topmargin;

            gpuRefreshTimer.Enabled = true;
        }

        private void on_GpuControl_unmap(object sender, EventArgs e)
        {
            if (gpuRefreshTimer != null)
            {
               gpuRefreshTimer.Stop();
               gpuRefreshTimer.Dispose();
               gpuRefreshTimer = null;
            }
        }

        /// <summary>
        /// Draw the frame buffer to the screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        unsafe private void on_daGpu_draw(object sender, DrawnArgs e)
        {
            var cr = e.Cr;

            cr.SelectFontFace("Consolas", Cairo.FontSlant.Normal, Cairo.FontWeight.Normal);
            cr.SetFontSize(12);

            cr.SetSourceColor(black);
            cr.Rectangle(0, 0, daGpu.AllocatedWidth, daGpu.AllocatedHeight);
            cr.Fill();

            if (VICKY == null)
            {
                cr.SetSourceColor(lightBlue); 
                cr.MoveTo(20, daGpu.AllocatedHeight / 2);
                cr.ShowText("VICKY is undefined.");

                return;
            }

            paintCycle++;

            // Read the Master Control Register
            byte MCRegister = VICKY.ReadByte(0);            // Reading address $AF:0000
            byte MCRHigh = (byte)(VICKY.ReadByte(1) & 3);   // Reading address $AF:0001

            int resX = 640;
            int resY = 480;

#pragma warning disable CS0219 // The variable 'isPixelDoubled' is assigned but its value is never used
            bool isPixelDoubled = false;
#pragma warning restore CS0219 // The variable 'isPixelDoubled' is assigned but its value is never used

            switch (MCRHigh)
            {
                case 1:
                    resX = 800;
                    resY = 600;
                    break;

                case 2:
                    resX = 320;
                    resY = 240;
                    isPixelDoubled = true;
                    break;

                case 3:
                    resX = 400;
                    resY = 300;
                    isPixelDoubled = true;
                    break;
            }

            pixVals = new byte[resX];
            int top = 0; // top gets modified if error messages are displayed

            byte ColumnsVisible = (byte)(resX / CHAR_WIDTH);
            byte LinesVisible = (byte)(resY / CHAR_HEIGHT);

            if (MCRegister == 0 || (MCRegister & 0x80) == 0x80)
            {
                cr.SetSourceColor(lightBlue);
                cr.MoveTo(0, 0 + BASELINE_OFFSET);
                cr.ShowText("Graphics Mode disabled");
                return;
            }

            if ((MCRegister & 0x1) == 0x1)
            {
                if (ColumnsVisible < 1 || ColumnsVisible > MAX_TEXT_COLS)
                {
                    DrawTextWithBackground("ColumnsVisible invalid:" + ColumnsVisible.ToString(), cr, 0, top);
                    top += 12;
                }

                if (LinesVisible < 1 || LinesVisible > MAX_TEXT_LINES)
                {
                    DrawTextWithBackground("LinesVisible invalid:" + LinesVisible.ToString(), cr, 0, top);
                    top += 12;
                }
            }

            if (drawing)
            {
                // drop the frame
                System.Console.WriteLine("Skipped Frame");
                return;
            }

            drawing = true;

            // Check if SOF is enabled
            if (MCRegister != 0 && MCRegister != 0x80)
                StartOfFrame?.Invoke();

            //-- g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
            // g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighSpeed;

            // Bilinear interpolation has effect very similar to real HW 
            //-- g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            // g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;

            // Determine if we display a border
            byte border_register = VICKY.ReadByte(MemoryMap.BORDER_CTRL_REG - MemoryMap.VICKY_BASE_ADDR);
            bool displayBorder = (border_register & 1) != 0;

            int borderXSize = displayBorder ? VICKY.ReadByte(MemoryMap.BORDER_X_SIZE - MemoryMap.VICKY_BASE_ADDR) : 0;
            int borderYSize = displayBorder ? VICKY.ReadByte(MemoryMap.BORDER_Y_SIZE - MemoryMap.VICKY_BASE_ADDR) : 0;

            System.Drawing.Rectangle rect = new(0, 0, resX - 1, resY - 1);
            BitmapData bitmapData = frameBuffer.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            int* bitmapPointer = (int*)bitmapData.Scan0.ToPointer();

            uint drawBufferLength = (uint)(resX * 4 * resY);
            var drawBuffer = new byte[drawBufferLength];
            for (uint i = 0; i < drawBufferLength; ++i)
                drawBuffer[i] = 0x00;

            // Load the SOL register - a lines
            int SOLRegAddr = MemoryMap.VKY_LINE_IRQ_CTRL_REG - MemoryMap.VICKY_BASE_ADDR;
            int SOLLine0Addr = MemoryMap.VKY_LINE0_CMP_VALUE_LO - MemoryMap.VICKY_BASE_ADDR;
            int SOLLine1Addr = MemoryMap.VKY_LINE1_CMP_VALUE_LO - MemoryMap.VICKY_BASE_ADDR;

            // Reset LUT Cache
            lutCache = new int[256 * 8]; // 8 LUTs

            bool gammaCorrection = (MCRegister & 0x40) == 0x40;

            for (int line = 0; line < resY; ++line)
            {
                // Handle SOL interrupts
                byte SOLRegister = VICKY.ReadByte(SOLRegAddr);

                if ((SOLRegister & 1) != 0)
                {
                    int SOLLine0 = VICKY.ReadWord(SOLLine0Addr);
                    if (line == SOLLine0)
                        StartOfLine?.Invoke();
                }

                if ((SOLRegister & 2) != 0)
                {
                    int SOLLine1 = VICKY.ReadWord(SOLLine1Addr);
                    if (line == SOLLine1)
                        StartOfLine?.Invoke();
                }

                // Default background color to border color
                // In Text mode, the border color is stored at D005:D007.
                byte borderBlue = VICKY.ReadByte(5);
                byte borderGreen = VICKY.ReadByte(6);
                byte borderRed = VICKY.ReadByte(7);

                if (gammaCorrection)
                {
                    borderRed = VICKY.ReadByte(MemoryMap.GAMMA_BASE_ADDR - MemoryMap.VICKY_BASE_ADDR + 0x200 + borderRed);      // gammaCorrection[0x200 + borderGreen];
                    borderGreen = VICKY.ReadByte(MemoryMap.GAMMA_BASE_ADDR - MemoryMap.VICKY_BASE_ADDR + 0x100 + borderGreen);  // gammaCorrection[0x100 + borderGreen];
                    borderBlue = VICKY.ReadByte(MemoryMap.GAMMA_BASE_ADDR - MemoryMap.VICKY_BASE_ADDR + borderBlue);            // gammaCorrection[borderBlue];
                }

                int borderColor = (int)(0xFF000000 + (borderBlue << 16) + (borderGreen << 8) + borderRed);

                int* ptr = bitmapPointer + line * STRIDE;

                if (line < borderYSize || line >= resY - borderYSize)   // bug??
                    for (int x = 0; x < resX; ++x)
                        ptr[x] = borderColor;
                else
                {
                    // Graphics Mode
                    int backgroundColor = unchecked((int)0xFF000000);

                    if ((MCRegister & 0x4) == 0x4)
                    {
                        byte backRed = VICKY.ReadByte(MemoryMap.BACKGROUND_COLOR_B - MemoryMap.VICKY_BASE_ADDR);
                        byte backGreen = VICKY.ReadByte(MemoryMap.BACKGROUND_COLOR_G - MemoryMap.VICKY_BASE_ADDR);
                        byte backBlue = VICKY.ReadByte(MemoryMap.BACKGROUND_COLOR_R - MemoryMap.VICKY_BASE_ADDR);

                        if (gammaCorrection)
                        {
                            backRed = VICKY.ReadByte(MemoryMap.GAMMA_BASE_ADDR - MemoryMap.VICKY_BASE_ADDR + 0x200 + backRed);      // gammaCorrection[0x200 + backRed];
                            backGreen = VICKY.ReadByte(MemoryMap.GAMMA_BASE_ADDR - MemoryMap.VICKY_BASE_ADDR + 0x100 + backGreen);  // gammaCorrection[0x100 + backGreen];
                            backBlue = VICKY.ReadByte(MemoryMap.GAMMA_BASE_ADDR - MemoryMap.VICKY_BASE_ADDR + backBlue);            // gammaCorrection[backBlue];
                        }

                        backgroundColor = (int)(0xFF000000 + (backBlue << 16) + (backGreen << 8) + backRed);
                    }

                    for (int x = 0; x < resX; ++x)
                    {
                        int resetValue = x < borderXSize || x >= resX - borderXSize ? borderColor : backgroundColor;    // bug??
                        ptr[x] = resetValue;
                    }

                    // Bitmap Mode - draw the layers in revers order from back to front
                    if ((MCRegister & 0x4) == 0x4)
                    {
                        // Layer 12 - sprite layer 6
                        if ((MCRegister & 0x20) != 0)
                            DrawSprites(ref drawBuffer, gammaCorrection, 6, displayBorder, borderXSize, borderYSize, line, resX, resY);

                        // Layer 11 - bitmap 1
                        if ((MCRegister & 0x8) == 0x8)
                            DrawBitmap(ref drawBuffer, gammaCorrection, 1, displayBorder, backgroundColor, borderXSize, borderYSize, line, resX, resY);

                        // Layer 10 - sprite layer 5
                        if ((MCRegister & 0x20) != 0)
                            DrawSprites(ref drawBuffer, gammaCorrection, 5, displayBorder, borderXSize, borderYSize, line, resX, resY);

                        // Layer 9 - tilemap layer 3
                        if ((MCRegister & 0x10) == 0x10)
                            DrawTiles(ref drawBuffer, gammaCorrection, ColumnsVisible, 3, displayBorder, borderXSize, line, resX);

                        // Layer 8 - sprite layer 4
                        if ((MCRegister & 0x20) != 0)
                            DrawSprites(ref drawBuffer, gammaCorrection, 4, displayBorder, borderXSize, borderYSize, line, resX, resY);

                        // Layer 7 - tilemap layer 2
                        if ((MCRegister & 0x10) == 0x10)
                            DrawTiles(ref drawBuffer, gammaCorrection, ColumnsVisible, 2, displayBorder, borderXSize, line, resX);

                        // Layer 6 - sprite layer 3
                        if ((MCRegister & 0x20) != 0)
                            DrawSprites(ref drawBuffer, gammaCorrection, 3, displayBorder, borderXSize, borderYSize, line, resX, resY);

                        // Layer 5 - tilemap layer 1
                        if ((MCRegister & 0x10) == 0x10)
                            DrawTiles(ref drawBuffer, gammaCorrection, ColumnsVisible, 1, displayBorder, borderXSize, line, resX);

                        // Layer 4 - sprite layer 2
                        if ((MCRegister & 0x20) != 0)
                            DrawSprites(ref drawBuffer, gammaCorrection, 2, displayBorder, borderXSize, borderYSize, line, resX, resY);

                        // Layer 3 - tilemap layer 0
                        if ((MCRegister & 0x10) == 0x10)
                            DrawTiles(ref drawBuffer, gammaCorrection, ColumnsVisible, 0, displayBorder, borderXSize, line, resX);

                        // Layer 2 - sprite layer 1
                        if ((MCRegister & 0x20) != 0)
                            DrawSprites(ref drawBuffer, gammaCorrection, 1, displayBorder, borderXSize, borderYSize, line, resX, resY);

                        // Layer 1 - bitmap layer 0
                        if ((MCRegister & 0x8) == 0x8)
                            DrawBitmap(ref drawBuffer, gammaCorrection, 0, displayBorder, backgroundColor, borderXSize, borderYSize, line, resX, resY);

                        // Layer 0 - sprite layer 0
                        if ((MCRegister & 0x20) != 0)
                            DrawSprites(ref drawBuffer, gammaCorrection, 0, displayBorder, borderXSize, borderYSize, line, resX, resY);
                    }

                    // Draw the text
                    if ((MCRegister & 7) == 0x1 || (MCRegister & 7) == 3 || (MCRegister & 7) == 7)
                        if (top == 0) {
                            DrawBitmapText(ref drawBuffer, MCRegister, gammaCorrection, ColumnsVisible, LinesVisible, borderXSize, borderYSize, line, resX, resY);
                        }
                }
            }

            using (var imgBitmapText = new ImageSurface(drawBuffer, Format.Argb32, resX, resY, resX << 2))
            {
                cr.SetSourceSurface(imgBitmapText, 0, 0);
                cr.Paint();
            }

            {
                var buffer = new byte[1024];
                for (int i = 0; i < 1024; ++i)
                    buffer[i] = 0x00;

                int drawAtX = -1;
                int drawAtY = -1;
                DrawMouse(ref buffer, gammaCorrection, ref drawAtX, ref drawAtY);

                if (drawAtX != -1)
                    using (var imgBitmapText = new ImageSurface(buffer, Format.Argb32, 16, 16, 64))
                    {
                        cr.SetSourceSurface(imgBitmapText, drawAtX, drawAtY);
                        cr.Paint();
                    }
            }

            frameBuffer.UnlockBits(bitmapData);

            drawing = false;
        }

       void on_gpuRefreshTimer_tick(object sender, ElapsedEventArgs e)
        {
            if (BlinkingCounter-- == 0)
            {
                CursorState = !CursorState;
                BlinkingCounter = BLINK_RATE;
            }

            daGpu.QueueDraw();

            if (BlinkingCounter == 0)
                GpuUpdated?.Invoke();
        }
    }
}
