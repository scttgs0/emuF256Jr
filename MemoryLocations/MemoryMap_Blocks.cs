namespace FoenixCore.MemoryLocations
{
    public static partial class MemoryMap
    {
        #region Memory Blocks
        // c# Memory Blocks

        public const int RAM_START = 0x0_0000;                      // Beginning of RAM
        //public const int RAM_SIZE =  0x8_0000;                      // 512kb RAM
        public const int RAM_SIZE =  0xFF_0000;                      // 512kb RAM   HACK:
        public const int PAGE_SIZE = 0x2000;                        // 8kb

        // Beginning of Vicky Address Space
        public const int VICKY_START = VICKY_BASE_ADDR;             // Beginning of I/O Space
        public const int VICKY_END = 0xDFFF;                        // End of I/O Space
        public const int VICKY_SIZE = VICKY_END - VICKY_START + 1;  // 64KB

        public const int GABE_START = 0xAF_E000;
        public const int GABE_END = 0xAF_FFFF;
        public const int GABE_SIZE = GABE_END - GABE_START + 1;

        public const int VIDEO_START = 0xB0_0000;
        public const int VIDEO_SIZE = 0x40_0000;                    // 4MB Video RAM

        public const uint FLASH_START = 0x8_0000;       // Beginning of FLASH
        public const uint FLASH_END = 0xF_FFFF;         // End of 1MB FLASH 
        public const uint FLASH_SIZE = 0x8_0000;        // 512K

        #endregion
    }
}
