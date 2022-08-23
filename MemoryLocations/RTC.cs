namespace FoenixCore.MemoryLocations
{
    public static partial class MemoryMap
    {
        #region Vicky Memory Map

        public const int RTC_SEC_ALARM      = 0xD691; // Alarm Seconds
        public const int RTC_MIN_ALARM      = 0xD693; // Alarm Minutes
        public const int RTC_HRS_ALARM      = 0xD695; // Alarm Hours
        public const int RTC_DAY_ALARM      = 0xD697; // Alarm Day

        public const int RTC_SEC            = 0xD690; // Seconds
        public const int RTC_MIN            = 0xD692; // Minutes
        public const int RTC_HRS            = 0xD694; // Hours

        public const int RTC_DAY            = 0xD696; // Day
        public const int RTC_DOW            = 0xD698; // Day of Week
        public const int RTC_MONTH          = 0xD699; // Month
        public const int RTC_YEAR           = 0xD69A; // Year
        public const int RTC_CENTURY        = 0xD69F; // Century

        public const int RTC_RATES          = 0xD69B; // Rates
        public const int RTC_ENABLE         = 0xD69C; // Enable
        public const int RTC_FLAGS          = 0xD69D; // Flags
        public const int RTC_CTRL           = 0xD69E; // Control

        #endregion
    }
}
