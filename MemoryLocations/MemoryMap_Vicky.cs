namespace FoenixCore.MemoryLocations
{
    public static partial class MemoryMap
    {
        #region Vicky Memory Map

        public const ushort VICKY_BASE_ADDR = 0xD000;
        public const ushort GAMMA_CTRL_REG = 0xD002;

        public const ushort BORDER_CTRL_REG = 0xD004;          // border is enabled if bit 0 is 1.
        public const ushort BORDER_COLOR_B = 0xD005;
        public const ushort BORDER_COLOR_G = 0xD006;
        public const ushort BORDER_COLOR_R = 0xD007;
        public const ushort BORDER_X_SIZE = 0xD008;            // X-  Values: 0 - 32 (Default: 32)
        public const ushort BORDER_Y_SIZE = 0xD009;            // Y- Values 0 -32 (Default: 32)

        public const ushort BACKGROUND_COLOR_B = 0xD00D;       // When in Graphic Mode, if a pixel is "0" then the Background pixel is chosen
        public const ushort BACKGROUND_COLOR_G = 0xD00E;
        public const ushort BACKGROUND_COLOR_R = 0xD00F;

        public const int VKY_TXT_CURSOR_CTRL_REG = 0xAF_0010;
        public const int VKY_TXT_CURSOR_CHAR_REG = 0xAF_0012;

        // Line Interrupt Registers
        public const int VKY_LINE_IRQ_CTRL_REG = 0xAF001B; // [0] - Enable Line 0, [1] -Enable Line 1
        public const int VKY_LINE0_CMP_VALUE_LO = 0xAF001C; // Write Only[7:0]
        public const int VKY_LINE0_CMP_VALUE_HI = 0xAF001D; // Write Only[3:0]
        public const int VKY_LINE1_CMP_VALUE_LO = 0xAF001E; // Write Only[7:0]
        public const int VKY_LINE1_CMP_VALUE_HI = 0xAF001F; // Write Only[3:0]

        public const int BITMAP_CONTROL_REGISTER_ADDR = 0xAF_0100; // 2 layers - 8 bytes
        public const int TILE_CONTROL_REGISTER_ADDR = 0xAF_0200; // 12 bytes for each tile layer
        public const int SPRITE_CONTROL_REG = 0xD900;           // 8 bytes for each sprite

        public const int VDMA_START = 0xAF_0400;
        public const int VDMA_SIZE = 0x31; // from $af:0400 to $af:0430

        public const int C256F_MODEL_MAJOR = 0xAF_070B;
        public const int C256F_MODEL_MINOR = 0xAF_070C;
        public const int FPGA_DOR = 0xAF_070D;
        public const int FPGA_MOR = 0xAF_070E;
        public const int FPGA_YOR = 0xAF_070F;

        public const int KBD_DATA_BUF_FMX = 0xAF_1060;     // FMX Keyboard input, output buffer
        public const int KBD_STATUS_PORT_FMX = 0xAF_1064;  // FMX keyboard status port
        public const int KBD_DATA_BUF_U = 0xAF_1803;       // U Keyboard input, output buffer
        public const int KBD_STATUS_PORT_U = 0xAF_1807;    // U keyboard status port

        // FDC  - $AF:13F0
        // LPT1 - $AF:1378

        public const int UART_REGISTERS = 0xD630;

        // KBD  - $AF:1060
        // GAME - $AF:1200 - Not Connected
        // MPU  - $AF:1330

        public const int MPU401_REGISTERS = 0xAF_1330;  // 2 bytes
        public const int MPU401_DATA_REG = 0xAF_1330;
        public const int MPU401_STATUS_REG = 0xAF_1331;

        public const ushort FG_CHAR_LUT_PTR = 0xC040;       // 15 color lookup table
        public const ushort BG_CHAR_LUT_PTR = 0xC080;       // 15 color lookup table
        public const int GRP_LUT_BASE_ADDR = 0x1_D000;      // room for 8 LUTs at 1024 bytes each (256 * 4 bytes per colour)

        public const ushort GAMMA_BASE_ADDR = 0xC000;    // each 256 byte for B, G, R

        public const int TILESET_BASE_ADDR = 0xAF_0280; // 8 tileset addresses, 4 bytes - 3 bytes address of tileset, 1 byte configuration

        public const int FONT_MEMORY_BANK_START = 0x1_C000;     // TODO: Memory Bank-01

        public const int SCREEN_PAGE0 = 0xAF_A000; // 8192 Bytes First page of display RAM. This is used at boot time to display the welcome screen and the BASIC or MONITOR command screens. 
        public const int SCREEN_PAGE1 = 0xAF_C000; // 8192 Bytes Additional page of display RAM. This can be used for page flipping or to handle multiple edit buffers. 

        public const int REVOFPCB_C = 0xAF_E805;
        public const int REVOFPCB_4 = 0xAF_E806;
        public const int REVOFPCB_A = 0xAF_E807;

        #endregion
    }
}
