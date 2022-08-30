namespace FoenixCore.MemoryLocations
{
    public static partial class MemoryMap
    {
        public static class CODEC
        {
            public const ushort BASE            = 0xD620;

            public const ushort DATA            = 0xD620;       // write-only [word]
            public const ushort STATUS          = 0xD620;       // read-only
        }
    }
}
