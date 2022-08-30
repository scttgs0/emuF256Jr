namespace FoenixCore.MemoryLocations
{
    public static partial class MemoryMap
    {
        public static class PSG_L
        {
            public const ushort CHANNEL         = 0xD600;
        }

        public static class PSG_R
        {
            public const ushort CHANNEL         = 0xD610;
        }

        // PSG0 (MONO)
        // ----------------
        // public const ushort PSG1_TONE1_FREQ     = 0xD600;   // [word]
        // public const ushort PSG1_TONE1_ATTN     = 0xD602;   // [word]

        // public const ushort PSG1_TONE2_FREQ     = 0xD604;   // [word]
        // public const ushort PSG1_TONE2_ATTN     = 0xD606;   // [word]

        // public const ushort PSG1_TONE3_FREQ     = 0xD608;   // [word]
        // public const ushort PSG1_TONE3_ATTN     = 0xD60A;   // [word]

        // public const ushort PSG1_NOISE_CTRL     = 0xD60C;   // [word]
        // public const ushort PSG1_NOISE_ATTN     = 0xD60E;   // [word]

        // PSG1 (STEREO)
        // ----------------
        // public const ushort PSG2_TONE1_FREQ     = 0xD610;   // [word]
        // public const ushort PSG2_TONE1_ATTN     = 0xD612;   // [word]

        // public const ushort PSG2_TONE2_FREQ     = 0xD614;   // [word]
        // public const ushort PSG2_TONE2_ATTN     = 0xD616;   // [word]

        // public const ushort PSG2_TONE3_FREQ     = 0xD618;   // [word]
        // public const ushort PSG2_TONE3_ATTN     = 0xD61A;   // [word]

        // public const ushort PSG2_NOISE_CTRL     = 0xD61C;   // [word]
        // public const ushort PSG2_NOISE_ATTN     = 0xD61E;   // [word]
    }
}
