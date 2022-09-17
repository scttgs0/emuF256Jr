namespace FoenixCore.MemoryLocations
{
    public static partial class MemoryMap
    {
        public static class IEC
        {
            public const ushort BASE                = 0xD680;

            public const ushort TALKER_CMD          = 0xD680;       // write-only
            public const ushort TALKER_CMD_LAST     = 0xD681;       // write-only
            public const ushort TALKER_DATA         = 0xD682;       // write-only
            public const ushort TALKER_DATA_LAST    = 0xD683;       // write-only

            public const ushort LISTEN_DATA         = 0xD680;       // read-only
            public const ushort LISTEN_STATUS       = 0xD681;       // read-only
            public const ushort LISTEN_COUNT        = 0xD682;       // [word] read-only
        }
    }
}
