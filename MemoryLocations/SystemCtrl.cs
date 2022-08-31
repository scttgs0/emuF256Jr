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

            public const ushort RAND_L          = 0xD6A4;       // read
            public const ushort RAND_H          = 0xD6A5;       // read
            public const ushort STATUS          = 0xD6A6;       // read

            public const ushort SEED_L          = 0xD6A4;       // write
            public const ushort SEED_H          = 0xD6A5;       // write
            public const ushort RAND_CTRL       = 0xD6A6;       // write

            public const ushort MACHINE_ID      = 0xD6A7;       // read-only
            public const ushort PCB_VER_MAJOR   = 0xD6A8;       // read-only
            public const ushort PCB_VER_MINOR   = 0xD6A9;       // read-only
            public const ushort CHIP_VER_SUB_L  = 0xD6AA;       // read-only
            public const ushort CHIP_VER_SUB_H  = 0xD6AB;       // read-only
            public const ushort CHIP_VER_L      = 0xD6AC;       // read-only
            public const ushort CHIP_VER_H      = 0xD6AD;       // read-only
            public const ushort CHIP_NUMB_L     = 0xD6AE;       // read-only
            public const ushort CHIP_NUMB_H     = 0xD6AF;       // read-only
        }
    }
}
