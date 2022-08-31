namespace FoenixCore.MemoryLocations
{
    public static partial class MemoryMap
    {
        public static class UART    // Serial Communication
        {
            public const ushort BASE            = 0xD630;

            public const ushort HOLDING         = 0xD630;
            public const ushort INTR_ENABLE     = 0xD631;
            public const ushort INTR_STATUS     = 0xD632;
            public const ushort LINE_CTRL       = 0xD633;
            public const ushort MODEM_CTRL      = 0xD634;
            public const ushort LINE_STATUS     = 0xD635;
            public const ushort MODEM_STATUS    = 0xD636;
            public const ushort SCRATCH         = 0xD637;
        }
    }
}
