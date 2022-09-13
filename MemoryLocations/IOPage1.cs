namespace FoenixCore.MemoryLocations
{
    public static partial class MemoryMap
    {
        public const ushort FONT_BASE           = 0xC000;       // [C000:C7FF]

        public const ushort GRPH_LUT_BASE       = 0xD000;       // 256 * 4-bytes/color
        public const ushort GRPH_LUT0           = 0xD000;       // [D000:D3FF]
        public const ushort GRPH_LUT1           = 0xD400;       // [D400:D7FF]
        public const ushort GRPH_LUT2           = 0xD800;       // [D800:DBFF]
        public const ushort GRPH_LUT3           = 0xDC00;       // [DC00:DFFF]
    }
}
