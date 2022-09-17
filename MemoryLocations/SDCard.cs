namespace FoenixCore.MemoryLocations
{
    public static partial class MemoryMap
    {
        public static class SDCARD
        {
            public const ushort BASE            = 0xDD00;

            public const ushort VERSION         = 0xDD00;
            public const ushort MASTER_CTRL     = 0xDD01;
            public const ushort TRANSFER_TYPE   = 0xDD02;
            public const ushort TRANSFER_CTRL   = 0xDD03;
            public const ushort TRANSFER_STATUS = 0xDD04;
            public const ushort TRANSFER_ERROR  = 0xDD05;
            public const ushort DIRECT_DATA     = 0xDD06;
            public const ushort ADDRESS         = 0xDD07;       // [dword]
            public const ushort SPI_CLK_DEL     = 0xDD0B;

            public const ushort RX_DATA         = 0xDD10;       // read-only
            public const ushort RX_DATA_COUNT   = 0xDD12;       // [word]
            public const ushort RX_CTRL         = 0xDD14;

            public const ushort TX_DATA         = 0xDD20;
            public const ushort TX_CTRL         = 0xDD24;
        }
    }
}
