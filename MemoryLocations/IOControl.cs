using FoenixCore.Processor.GenericNew;


namespace FoenixCore.MemoryLocations
{
    public class IOControl : Register<byte>
    {
        //flags
        private byte _ioPage = 0b00;
        public bool isDisabled = true;
        public bool isColorMemory = true;
        public bool isTextInIO = true;

        public override byte Value
        {
            get
            {
                return _getFlags(
                    false,
                    false,
                    isTextInIO,
                    isColorMemory,
                    false,
                    isDisabled,
                    (_ioPage & 0b10) == 0b10,
                    (_ioPage & 0b01) == 0b01);
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
            isTextInIO = (value & 0x20) != 0;
            isColorMemory = (value & 0x10) != 0;
            isDisabled = (value & 4) != 0;
            _ioPage = (byte)(value & 3);
        }

        public void Reset()
        {
            _ioPage = 0b00;
            isDisabled = true;
            isColorMemory = true;
            isTextInIO = true;
        }
    }
}
