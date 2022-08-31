namespace FoenixCore.MemoryLocations
{
    public static partial class MemoryMap
    {
        public static class INTR_CTRL
        {
            public const ushort BASE            = 0xD660;

            public const ushort PENDING_L       = 0xD660;
            public const ushort PENDING_H       = 0xD661;
            public const ushort POLARITY_L      = 0xD662;
            public const ushort POLARITY_H      = 0xD663;
            public const ushort EDGE_L          = 0xD664;
            public const ushort EDGE_H          = 0xD665;
            public const ushort MASK_L          = 0xD666;
            public const ushort MASK_H          = 0xD667;
        }
    }
}
