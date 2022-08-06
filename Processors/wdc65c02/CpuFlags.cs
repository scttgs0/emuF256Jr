using FoenixCore.Processor.GenericNew;


namespace FoenixCore.Processor.wdc65c02
{
    public class CpuFlags : Register<byte>
    {
        //flags
        public bool Negative;
        public bool oVerflow;
        public bool Break;
        public bool Decimal;
        public bool IrqDisable;
        public bool Zero;
        public bool Carry;

        public override byte Value
        {
            get
            {
                return GetFlags(
                    Negative,
                    oVerflow,
                    true,
                    Break,
                    Decimal,
                    IrqDisable,
                    Zero,
                    Carry);
            }
            set => SetFlags(value);
        }

        public virtual int CarryBit
        {
            get => Carry ? 1 : 0;
        }

        public byte GetFlags(params bool[] flags)
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

        public void SetFlags(int value)
        {
            Negative = (value & 0x80) != 0;
            oVerflow = (value & 0x40) != 0;

            Break = (value & 0x10) != 0;
            Decimal = (value & 8) != 0;
            IrqDisable = (value & 4) != 0;
            Zero = (value & 2) != 0;
            Carry = (value & 1) != 0;
        }

        public override string ToString()
        {
            //NVMXDIZC
            char[] s = new char[8];

            s[0] = Negative ? 'N' : '-';
            s[1] = oVerflow ? 'V' : '-';
            s[3] = '-';
            s[2] = Break ? 'B' : '-';
            s[4] = Decimal ? 'D' : '-';
            s[5] = IrqDisable ? 'I' : '-';
            s[6] = Zero ? 'Z' : '-';
            s[7] = Carry ? 'C' : '-';

            return new string(s);
        }

        public void SetZ(int Val)
        {
            Zero = Val == 0;
        }

        public void SetZ(Register<byte> X)
        {
            Zero = X.Value == 0;
        }

        public void SetNZ(int Value, int Width)
        {
            Zero = (Width == 1 ? Value & 0xFF : Value & 0xFFFF) == 0;

            if (Width == 1)
                Negative = (Value & 0x80) != 0;
            else if (Width == 2)
                Negative = (Value & 0x8000) != 0;
        }

        public void Reset()
        {
            Negative = false;
            oVerflow = false;
            Break = false;
            Decimal = false;
            IrqDisable = false;
            Zero = false;
            Carry = false;
        }
    }
}
