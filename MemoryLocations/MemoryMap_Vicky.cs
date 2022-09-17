namespace FoenixCore.MemoryLocations
{
    public static partial class MemoryMap
    {
        #region Vicky Memory Map

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

        public const int TILESET_BASE_ADDR = 0xAF_0280; // 8 tileset addresses, 4 bytes - 3 bytes address of tileset, 1 byte configuration

        // public const int SCREEN_PAGE0 = 0xAF_A000; // 8192 Bytes First page of display RAM. This is used at boot time to display the welcome screen and the BASIC or MONITOR command screens. 
        // public const int SCREEN_PAGE1 = 0xAF_C000; // 8192 Bytes Additional page of display RAM. This can be used for page flipping or to handle multiple edit buffers. 

        public const int REVOFPCB_C = 0xAF_E805;
        public const int REVOFPCB_4 = 0xAF_E806;
        public const int REVOFPCB_A = 0xAF_E807;

        #endregion
    }
}
