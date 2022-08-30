namespace FoenixCore.MemoryLocations
{
    public static partial class MemoryMap
    {
        public static class RTC
        {
            public const ushort BASE = 0xD690;

            public const ushort SEC = 0xD690;   // Seconds
            public const ushort SEC_ALARM = 0xD691;   // Alarm Seconds

            public const ushort MIN = 0xD692;   // Minutes
            public const ushort MIN_ALARM = 0xD693;   // Alarm Minutes

            public const ushort HRS = 0xD694;   // Hours
            public const ushort HRS_ALARM = 0xD695;   // Alarm Hours

            // ----------------

            public const ushort DAY = 0xD696;   // Day
            public const ushort DAY_ALARM = 0xD697;   // Alarm Day
            public const ushort DOW = 0xD698;   // Day of Week
            public const ushort MONTH = 0xD699;   // Month
            public const ushort YEAR = 0xD69A;   // Year

            public const ushort RATES = 0xD69B;   // Rates
            public const ushort ENABLE = 0xD69C;   // Enable
            public const ushort FLAGS = 0xD69D;   // Flags
            public const ushort CTRL = 0xD69E;   // Control

            public const ushort CENTURY = 0xD69F;   // Century
        }
    }
}
