namespace FoenixCore.MemoryLocations
{
    public static partial class MemoryMap
    {
        // GAMMA
        // ----------------
        public const ushort GAMMA_CTRL_REG      = 0xD002;
        public const ushort GAMMA_BASE          = 0xC000;
        public const ushort GAMMA_B_TABLE       = 0xC000;       // [C000:C3FF] 256-byte Blue Table
        public const ushort GAMMA_G_TABLE       = 0xC400;       // [C400:C7FF] 256-byte Green Table
        public const ushort GAMMA_R_TABLE       = 0xC800;       // [C800:CBFF] 256-byte Red Table

        // VICKY
        // ============================
        public const ushort VICKY_BASE_ADDR = 0xD000;

        // BORDER
        // ----------------
        public const ushort BORDER_CTRL_REG = 0xD004;           // border is enabled if bit 0 is 1
        public const ushort BORDER_COLOR_B = 0xD005;
        public const ushort BORDER_COLOR_G = 0xD006;
        public const ushort BORDER_COLOR_R = 0xD007;
        public const ushort BORDER_X_SIZE = 0xD008;             // X-Values: 0-32 (Default: 32)
        public const ushort BORDER_Y_SIZE = 0xD009;             // Y-Values: 0-32 (Default: 32)

        // BACKGROUND
        // ----------------
        public const ushort BACKGROUND_COLOR_B = 0xD00D;        // When in Graphic Mode, if a pixel is "0" then the Background pixel is chosen
        public const ushort BACKGROUND_COLOR_G = 0xD00E;
        public const ushort BACKGROUND_COLOR_R = 0xD00F;

        // BITMAP
        // ----------------
        public const ushort BITMAP_CTRL_REG = 0xD100;           // 2 layers - 8 bytes

        // TILE
        // ----------------
        public const ushort TILEMAP_CTRL_REG = 0xD200;          // 12 bytes for each tile layer

        // MISCELLANOUS
        // ----------------
        public const ushort MISC_CTRL_REG = 0xD300;

        // ============================

        // LOOK-UP TABLE (LUT)
        // ----------------
        public const ushort TEXT_COLOR_FG = 0xD800;             // [D800:D83F] 16-color palette for text foreground
        public const ushort TEXT_COLOR_BG = 0xD840;             // [D840:D87F] 16-color palette for text background

        // SPRITE
        // ----------------
        public const ushort SPRITE_CTRL_REG = 0xD900;           // 64 sprites - 8 bytes for each sprite
    }
}
