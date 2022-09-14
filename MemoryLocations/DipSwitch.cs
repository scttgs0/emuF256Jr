namespace FoenixCore.MemoryLocations
{
    public static partial class MemoryMap
    {
        public class DIPSWITCH
        {
            // Dip switch Port
            public const ushort BASE = 0xD670;

            private byte _bootMode = 0b1111;
            private byte _userMode = 0b111;
            public bool isGammaCorrection = true;

            public byte BootMode
            {
                get => _bootMode;
            }

            public byte UserMode
            {
                get => _bootMode;
            }

            public byte Value
            {
                get => (byte)((isGammaCorrection ? 0b10000000 : 0) + (_userMode << 4) + _bootMode);
                set
                {
                    _bootMode = (byte)(value & 0x0F);
                    _userMode = (byte)((value >> 4) & 0x07);
                    isGammaCorrection = (value & 0x80) == 0x80;
                }
            }
        }
    }
}
