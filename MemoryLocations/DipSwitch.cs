namespace FoenixCore.MemoryLocations
{
    public static partial class MemoryMap
    {
        public static class DIPSWITCH
        {
            // Dip switch Ports
            public const ushort BASE = 0xD670;       // (R)

            public const int USER_MODE = 0xAF_E80D;    // TODO:
            public const int BOOT_MODE = 0xAF_E80E;
        }
    }
}
