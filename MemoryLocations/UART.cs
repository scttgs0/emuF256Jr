namespace FoenixCore.MemoryLocations
{
    public static partial class MemoryMap
    {
        public static class UART    // Serial Communication
        {
            public const ushort BASE            = 0xD630;

            public const ushort HOLDING         = 0xD630;
            public const ushort IRQ_ENABLE      = 0xD631;
            public const ushort IRQ_STATUS      = 0xD632;       // read-only
            public const ushort FIFO_CTRL       = 0xD632;       // write-only
            public const ushort LINE_CTRL       = 0xD633;
            public const ushort MODEM_CTRL      = 0xD634;
            public const ushort LINE_STATUS     = 0xD635;       // read-only
            public const ushort MODEM_STATUS    = 0xD636;
            public const ushort SCRATCH         = 0xD637;

            // When DLAB = 1
            public const ushort BAUD_DIVISOR    = 0xD630;       // [word]
            public const ushort PRESCALER_DIV   = 0xD632;       // write-only
        }
    }
}
