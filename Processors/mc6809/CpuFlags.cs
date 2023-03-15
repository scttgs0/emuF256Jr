
using FoenixCore.Processor.GenericNew;


namespace FoenixCore.Processor.mc6809
{
    public class CpuFlags : Register<byte>
    {
        //flags
        public bool Entire;
        public bool Fastirq;
        public bool Halfcarry;
        public bool Irq;
        public bool Negative;
        public bool Zero;
        public bool oVerflow;
        public bool Carry;

        public override byte Value
        {
            get
            {
                return GetFlags(
                    Entire,
                    Fastirq,
                    Halfcarry,
                    Irq,
                    Negative,
                    Zero,
                    oVerflow,
                    Carry);
            }
            set => SetFlags(value);
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
            Entire = (value & 0x80) != 0;
            Fastirq = (value & 0x40) != 0;
            Halfcarry = (value & 0x20) != 0;
            Irq = (value & 0x10) != 0;
            Negative = (value & 8) != 0;
            Zero = (value & 4) != 0;
            oVerflow = (value & 2) != 0;
            Carry = (value & 1) != 0;
        }

        public override string ToString()
        {
            //EFHINZVC
            char[] s = new char[8];

            s[0] = Entire ? 'E' : '-';
            s[1] = Fastirq ? 'F' : '-';
            s[2] = Halfcarry ? 'H' : '-';
            s[3] = Irq ? 'I' : '-';
            s[4] = Negative ? 'N' : '-';
            s[5] = Zero ? 'Z' : '-';
            s[6] = oVerflow ? 'V' : '-';
            s[7] = Carry ? 'C' : '-';

            return new string(s);
        }

        public virtual int CarryBit
        {
            get => Carry ? 1 : 0;
        }

        public void SetZ(int Val)
        {
            Zero = Val == 0;
        }

        public void SetZ(Register<byte> R)
        {
            Zero = R.Value == 0;
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
            Entire = false;
            Fastirq = false;
            Halfcarry = false;
            Irq = false;
            Negative = false;
            Zero = false;
            oVerflow = false;
            Carry = false;
        }
    }
}
