namespace FoenixCore.MemoryLocations
{
    public static partial class MemoryMap
    {
        public static class SID_L
        {
            public const ushort FREQ1      = 0xD400;       // [word]
            public const ushort PULSE1     = 0xD402;       // [word]
            public const ushort CTRL1      = 0xD404;
            public const ushort ATDCY1     = 0xD405;
            public const ushort SUREL1     = 0xD406;

            public const ushort FREQ2      = 0xD407;       // [word]
            public const ushort PULSE2     = 0xD409;       // [word]
            public const ushort CTRL2      = 0xD40B;
            public const ushort ATDCY2     = 0xD40C;
            public const ushort SUREL2     = 0xD40D;

            public const ushort FREQ3      = 0xD40E;       // [word]
            public const ushort PULSE3     = 0xD410;       // [word]
            public const ushort CTRL3      = 0xD412;
            public const ushort ATDCY3     = 0xD413;
            public const ushort SUREL3     = 0xD414;

            public const ushort CUTOFF     = 0xD415;       // [word]
            public const ushort RESON      = 0xD417;
            public const ushort SIGVOL     = 0xD418;
            public const ushort RANDOM     = 0xD41B;
            public const ushort ENV3       = 0xD41C;
        }

        public static class SID_R
        {
            public const ushort FREQ1      = 0xD500;       // [word]
            public const ushort PULSE1     = 0xD502;       // [word]
            public const ushort CTRL1      = 0xD504;
            public const ushort ATDCY1     = 0xD505;
            public const ushort SUREL1     = 0xD506;

            public const ushort FREQ2      = 0xD507;       // [word]
            public const ushort PULSE2     = 0xD509;       // [word]
            public const ushort CTRL2      = 0xD50B;
            public const ushort ATDCY2     = 0xD50C;
            public const ushort SUREL2     = 0xD50D;

            public const ushort FREQ3      = 0xD50E;       // [word]
            public const ushort PULSE3     = 0xD510;       // [word]
            public const ushort CTRL3      = 0xD512;
            public const ushort ATDCY3     = 0xD513;
            public const ushort SUREL3     = 0xD514;

            public const ushort CUTOFF     = 0xD515;       // [word]
            public const ushort RESON      = 0xD517;
            public const ushort SIGVOL     = 0xD518;
            public const ushort RANDOM     = 0xD51B;
            public const ushort ENV3       = 0xD51C;
        }

    }
}
