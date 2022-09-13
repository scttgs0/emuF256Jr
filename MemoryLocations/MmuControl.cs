using FoenixCore.Processor.GenericNew;


namespace FoenixCore.MemoryLocations
{
    public class MmuControl : Register<byte>
    {
        private byte _lutActive = 0b00;
        private byte _lutEdit = 0b00;
        private bool _isEditMode = false;

        public override byte Value
        {
            get
            {
                return _getFlags(
                    _isEditMode,
                    false,
                    (_lutEdit & 0b10) == 0b10,
                    (_lutEdit & 0b01) == 0b01,
                    false,
                    false,
                    (_lutActive & 0b10) == 0b10,
                    (_lutActive & 0b01) == 0b01);
            }
            set => _setFlags(value);
        }

        private byte _getFlags(params bool[] flags)
        {
            byte bits = 0;

            for (int i = 0; i < flags.Length; ++i)
            {
                bits = (byte)(bits << 1);

                if (flags[i])
                    bits = (byte)(bits | 1);
            }

            return bits;
        }

        private void _setFlags(int value)
        {
            _isEditMode = (value & 0x80) != 0;
            _lutEdit = (byte)((value >> 4) & 3);
            _lutActive = (byte)(value & 3);
        }

        public void Reset()
        {
            _lutActive = 0b00;
            _lutEdit = 0b00;
            _isEditMode = false;
        }
    }
}
