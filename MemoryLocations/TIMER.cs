namespace FoenixCore.MemoryLocations
{
    public static partial class MemoryMap
    {
        public static class TIMER0
        {
            public const ushort BASE            = 0xD650;

            public const ushort CTRL            = 0xD650;       // write-only
            public const ushort STATUS          = 0xD650;       // read-only
            public const ushort VALUE           = 0xD651;       // 3-byte
            public const ushort COMPARE_CTRL    = 0xD654;
            public const ushort COMPARE_VALUE   = 0xD655;       // 3-byte
        }

        public static class TIMER1
        {
            public const ushort BASE            = 0xD658;

            public const ushort CTRL            = 0xD658;       // write-only
            public const ushort STATUS          = 0xD658;       // read-only
            public const ushort VALUE           = 0xD659;       // 3-byte
            public const ushort COMPARE_CTRL    = 0xD65C;
            public const ushort COMPARE_VALUE   = 0xD65D;       // 3-byte
        }
    }
}
