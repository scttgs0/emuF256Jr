namespace FoenixCore.MemoryLocations
{
    public static partial class MemoryMap
    {
        public static class VIA
        {
            public const ushort BASE            = 0xDC00;

            public const ushort IRB             = 0xDC00;       // read-only
            public const ushort ORB             = 0xDC00;       // write-only
            public const ushort IRA             = 0xDC01;       // read-only
            public const ushort ORA             = 0xDC01;       // write-only
            public const ushort DDRB            = 0xDC02;
            public const ushort DDRA            = 0xDC03;
            public const ushort T1C             = 0xDC04;       // [word]
            public const ushort T1L             = 0xDC06;       // [word]
            public const ushort T2C             = 0xDC08;       // [word]
            public const ushort SR              = 0xDC0A;
            public const ushort ACR             = 0xDC0B;
            public const ushort PCR             = 0xDC0C;
            public const ushort IFR             = 0xDC0D;
            public const ushort IER             = 0xDC0E;
            public const ushort IRA2            = 0xDC0F;       // read-only
            public const ushort ORA2            = 0xDC0F;       // write-only
        }
    }
}
