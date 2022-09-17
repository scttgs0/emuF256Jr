namespace FoenixCore.MemoryLocations
{
    public static partial class MemoryMap
    {
        public static class SYSTEM_CTRL
        {
            public const ushort BASE            = 0xD6A0;

            public const ushort CTRL0           = 0xD6A0;
            public const ushort CTRL1           = 0xD6A1;
            public const ushort CTRL2           = 0xD6A2;
            public const ushort CTRL3           = 0xD6A3;

            public const ushort RANDOM          = 0xD6A4;       // [word] read-only
            public const ushort STATUS          = 0xD6A6;       // read-only

            public const ushort SEED            = 0xD6A4;       // [word] write-only
            public const ushort RANDOM_CTRL     = 0xD6A6;       // write-only

            public const ushort MACHINE_ID      = 0xD6A7;       // read-only
            public const ushort PCB_VER_MAJOR   = 0xD6A8;       // read-only
            public const ushort PCB_VER_MINOR   = 0xD6A9;       // read-only
            public const ushort CHIP_VER_SUB    = 0xD6AA;       // [word] read-only
            public const ushort CHIP_VERSION    = 0xD6AC;       // [word] read-only
            public const ushort CHIP_NUMBER     = 0xD6AE;       // [word] read-only
        }
    }
}
