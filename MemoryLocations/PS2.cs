namespace FoenixCore.MemoryLocations
{
    public static partial class MemoryMap
    {
        public static class PS2
        {
            public const ushort BASE            = 0xD640;

            public const ushort OUTPUT_BUF      = 0xD640;       // write-only
            public const ushort INPUT_BUF       = 0xD640;       // read-only
            public const ushort DATA_BUF        = 0xD640;

            public const ushort STATUS          = 0xD644;       // read-only
            public const ushort CMD             = 0xD644;       // write-only
        }
    }
}
