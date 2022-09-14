namespace FoenixCore.MemoryLocations
{
    public static partial class MemoryMap
    {
        #region Vicky Memory Map


        public const int VKY_TXT_CURSOR_CTRL_REG = 0xAF_0010;
        public const int VKY_TXT_CURSOR_CHAR_REG = 0xAF_0012;

        // Line Interrupt Registers
        public const int VKY_LINE_IRQ_CTRL_REG = 0xAF001B; // [0] - Enable Line 0, [1] -Enable Line 1
        public const int VKY_LINE0_CMP_VALUE_LO = 0xAF001C; // Write Only[7:0]
        public const int VKY_LINE0_CMP_VALUE_HI = 0xAF001D; // Write Only[3:0]
        public const int VKY_LINE1_CMP_VALUE_LO = 0xAF001E; // Write Only[7:0]
        public const int VKY_LINE1_CMP_VALUE_HI = 0xAF001F; // Write Only[3:0]


        public const int VDMA_START = 0xAF_0400;
        public const int VDMA_SIZE = 0x31; // from $af:0400 to $af:0430

        public const int KBD_DATA_BUF_FMX = 0xAF_1060;     // FMX Keyboard input, output buffer
        public const int KBD_STATUS_PORT_FMX = 0xAF_1064;  // FMX keyboard status port
        public const int KBD_DATA_BUF_U = 0xAF_1803;       // U Keyboard input, output buffer
        public const int KBD_STATUS_PORT_U = 0xAF_1807;    // U keyboard status port

        // FDC  - $AF:13F0
        // LPT1 - $AF:1378

        // KBD  - $AF:1060
        // GAME - $AF:1200 - Not Connected
        // MPU  - $AF:1330

        public const int MPU401_REGISTERS = 0xAF_1330;  // 2 bytes
        public const int MPU401_DATA_REG = 0xAF_1330;
        public const int MPU401_STATUS_REG = 0xAF_1331;


        public const int TILESET_BASE_ADDR = 0xAF_0280; // 8 tileset addresses, 4 bytes - 3 bytes address of tileset, 1 byte configuration

        // public const int SCREEN_PAGE0 = 0xAF_A000; // 8192 Bytes First page of display RAM. This is used at boot time to display the welcome screen and the BASIC or MONITOR command screens. 
        // public const int SCREEN_PAGE1 = 0xAF_C000; // 8192 Bytes Additional page of display RAM. This can be used for page flipping or to handle multiple edit buffers. 

        public const int REVOFPCB_C = 0xAF_E805;
        public const int REVOFPCB_4 = 0xAF_E806;
        public const int REVOFPCB_A = 0xAF_E807;

        #endregion
    }
}
