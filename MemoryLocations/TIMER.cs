namespace FoenixCore.MemoryLocations
{
    public static partial class MemoryMap
    {
        public static class TIMER0
        {
            public const ushort BASE            = 0xD650;

            public const ushort CTRL            = 0xD650;
            public const ushort VALUE_L         = 0xD651;
            public const ushort VALUE_M         = 0xD652;
            public const ushort VALUE_H         = 0xD653;
            public const ushort COMPARE_CTRL    = 0xD654;
            public const ushort COMPARE_L       = 0xD655;
            public const ushort COMPARE_M       = 0xD656;
            public const ushort COMPARE_H       = 0xD657;
        }

        public static class TIMER1
        {
            public const ushort BASE            = 0xD658;

            public const ushort CTRL            = 0xD658;
            public const ushort VALUE_L         = 0xD659;
            public const ushort VALUE_M         = 0xD65A;
            public const ushort VALUE_H         = 0xD65B;
            public const ushort COMPARE_CTRL    = 0xD65C;
            public const ushort COMPARE_L       = 0xD65D;
            public const ushort COMPARE_M       = 0xD65E;
            public const ushort COMPARE_H       = 0xD65F;
        }
    }
}
