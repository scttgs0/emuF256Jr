namespace FoenixCore.MemoryLocations
{
    public static partial class MemoryMap
    {
        public static class IRQ_CTRL
        {
            public const ushort BASE            = 0xD660;

            public const ushort PENDING         = 0xD660;       // [word]
            public const ushort POLARITY        = 0xD662;       // [word]
            public const ushort EDGE            = 0xD664;       // [word]
            public const ushort MASK            = 0xD666;       // [word]
        }
    }
}
