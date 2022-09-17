namespace FoenixCore.MemoryLocations
{
    public static partial class MemoryMap
    {
        // GAMMA
        // ----------------
        // public const ushort GAMMA_CTRL_REG      = 0xD002;
        public const ushort GAMMA_BASE          = 0xC000;
        public const ushort GAMMA_B_TABLE       = 0xC000;       // [C000:C3FF] 256-byte Blue Table
        public const ushort GAMMA_G_TABLE       = 0xC400;       // [C400:C7FF] 256-byte Green Table
        public const ushort GAMMA_R_TABLE       = 0xC800;       // [C800:CBFF] 256-byte Red Table

        // VICKY
        // ============================
        public const ushort VICKY_BASE_ADDR     = 0xD000;

        // MASTER
        // ----------------
        public const ushort MASTER_CTRL_0       = 0xD000;
        public const ushort MASTER_CTRL_1       = 0xD001;

        // LAYER
        // ----------------
        public const ushort LAYER_CTRL_0        = 0xD002;
        public const ushort LAYER_CTRL_1        = 0xD003;

        // BORDER
        // ----------------
        public const ushort BORDER_CTRL_REG     = 0xD004;       // border is enabled if bit 0 is 1
        public const ushort BORDER_COLOR_B      = 0xD005;
        public const ushort BORDER_COLOR_G      = 0xD006;
        public const ushort BORDER_COLOR_R      = 0xD007;
        public const ushort BORDER_X_SIZE       = 0xD008;       // X-Values: 0-32 (Default: 32)
        public const ushort BORDER_Y_SIZE       = 0xD009;       // Y-Values: 0-32 (Default: 32)

        // BACKGROUND
        // ----------------
        public const ushort BACKGROUND_COLOR_B  = 0xD00D;       // When in Graphic Mode, if a pixel is "0" then the Background pixel is chosen
        public const ushort BACKGROUND_COLOR_G  = 0xD00E;
        public const ushort BACKGROUND_COLOR_R  = 0xD00F;

        // CURSOR
        // ----------------
        public const ushort CURSOR_CTRL         = 0xD010;
        public const ushort CURSOR_CHAR         = 0xD012;
        public const ushort CURSOR_X            = 0xD014;       // 2-byte
        public const ushort CURSOR_Y            = 0xD016;       // 2-byte

        // LINE INTERRUPT
        // ----------------
        public const ushort LINE_IRQ_CTRL       = 0xD018;       // write-only
        public const ushort LINE_IRQ_LINENBR    = 0xD019;       // write-only, 2-byte
        public const ushort LINE_IRQ_POS_X      = 0xD018;       // read-only, 2-byte
        public const ushort LINE_IRQ_POS_Y      = 0xD01A;       // read-only, 2-byte

        // BITMAP
        // ----------------
        public const ushort BITMAP0_CTRL        = 0xD100;
        public const ushort BITMAP0_ADDR        = 0xD101;       // 3-byte
        public const ushort BITMAP1_CTRL        = 0xD108;
        public const ushort BITMAP1_ADDR        = 0xD109;       // 3-byte
        public const ushort BITMAP2_CTRL        = 0xD110;
        public const ushort BITMAP2_ADDR        = 0xD111;       // 3-byte

        // TILEMAP
        // ----------------
        public const ushort TILEMAP0_CTRL       = 0xD200;       // 12 bytes for each tile layer
        public const ushort TILEMAP0_ADDR       = 0xD201;       // 3-byte
        public const ushort TILEMAP0_SIZE_X     = 0xD204;       // 2-byte
        public const ushort TILEMAP0_SIZE_Y     = 0xD206;       // 2-byte
        public const ushort TILEMAP0_X          = 0xD208;       // 2-byte
        public const ushort TILEMAP0_Y          = 0xD20A;       // 2-byte
        public const ushort TILEMAP1_CTRL       = 0xD20C;       // 12 bytes for each tile layer
        public const ushort TILEMAP1_ADDR       = 0xD20D;       // 3-byte
        public const ushort TILEMAP1_SIZE_X     = 0xD210;       // 2-byte
        public const ushort TILEMAP1_SIZE_Y     = 0xD212;       // 2-byte
        public const ushort TILEMAP1_X          = 0xD214;       // 2-byte
        public const ushort TILEMAP1_Y          = 0xD216;       // 2-byte
        public const ushort TILEMAP2_CTRL       = 0xD218;       // 12 bytes for each tile layer
        public const ushort TILEMAP2_ADDR       = 0xD219;       // 3-byte
        public const ushort TILEMAP2_SIZE_X     = 0xD21C;       // 2-byte
        public const ushort TILEMAP2_SIZE_Y     = 0xD21E;       // 2-byte
        public const ushort TILEMAP2_X          = 0xD220;       // 2-byte
        public const ushort TILEMAP2_Y          = 0xD222;       // 2-byte

        // TILE SHEET
        // ----------------
        public const ushort TILESHEET0_ADDR     = 0xD280;       // 3-byte
        public const ushort TILESHEET1_ADDR     = 0xD284;       // 3-byte
        public const ushort TILESHEET2_ADDR     = 0xD288;       // 3-byte
        public const ushort TILESHEET3_ADDR     = 0xD28C;       // 3-byte
        public const ushort TILESHEET4_ADDR     = 0xD290;       // 3-byte
        public const ushort TILESHEET5_ADDR     = 0xD294;       // 3-byte
        public const ushort TILESHEET6_ADDR     = 0xD298;       // 3-byte
        public const ushort TILESHEET7_ADDR     = 0xD29C;       // 3-byte

        // MISCELLANOUS
        // ----------------
        public const ushort MISC_CTRL_REG       = 0xD300;

        // ============================

        // LOOK-UP TABLE (LUT)
        // ----------------
        public const ushort TEXT_COLOR_FG       = 0xD800;       // [D800:D83F] 16-color palette for text foreground
        public const ushort TEXT_COLOR_BG       = 0xD840;       // [D840:D87F] 16-color palette for text background

        // SPRITE
        // ----------------
        public const ushort SPRITE00_CTRL       = 0xD900;       // 64 sprites - 8 bytes for each sprite
        public const ushort SPRITE00_ADDR       = 0xD901;       // 3-byte
        public const ushort SPRITE00_X          = 0xD904;       // 2-byte
        public const ushort SPRITE00_Y          = 0xD906;       // 2-byte

        public const ushort SPRITE01_CTRL       = 0xD908;
        public const ushort SPRITE01_ADDR       = 0xD909;       // 3-byte
        public const ushort SPRITE01_X          = 0xD90C;       // 2-byte
        public const ushort SPRITE01_Y          = 0xD90E;       // 2-byte

        public const ushort SPRITE02_CTRL       = 0xD910;
        public const ushort SPRITE03_CTRL       = 0xD918;
        public const ushort SPRITE04_CTRL       = 0xD920;
        public const ushort SPRITE05_CTRL       = 0xD928;
        //...
        public const ushort SPRITE62_CTRL       = 0xDAF0;
        public const ushort SPRITE63_CTRL       = 0xDAF8;
    }
}
