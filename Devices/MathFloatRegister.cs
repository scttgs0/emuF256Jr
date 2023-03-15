
using System;


namespace FoenixCore.Simulator.Devices
{
    /**
     * This class will automatically call other methods immediately after writes
     */
    public class MathFloatRegister : MemoryLocations.MemoryRAM
    {
        readonly byte[] inputData = new byte[16];
        readonly byte[] outputData = new byte[16];

        public MathFloatRegister(int StartAddress, int Length) : base(StartAddress, Length)
        {
        }

        public override void WriteByte(int Address, byte Value)
        {
            inputData[Address] = Value;
            ComputeValue();
        }

        public override byte ReadByte(int Address)
        {
            return outputData[Address];
        }

        private void ComputeValue()
        {
            byte CTRL0 = inputData[0];
            byte CTRL1 = (byte)(inputData[1] & 3);
            float input0 = 0, input1 = 0;

            if ((CTRL0 & 1) != 0)
            {
                uint raw = BitConverter.ToUInt32(inputData, 8);
                input0 = ConvertFixedtoFP(raw);
            }
            else
                input0 = BitConverter.ToSingle(inputData, 8);

            if ((CTRL0 & 2) != 0)
            {
                uint raw = BitConverter.ToUInt32(inputData, 12);
                input1 = ConvertFixedtoFP(raw);
            }
            else
                input1 = BitConverter.ToSingle(inputData, 12);

            // Multiplication
            float FPMult = input0 * input1;
            data[4] = (byte)((FPMult == 0) ? 8 : 0);

            // Division
            float FPDiv = 0;

            try
            {
                FPDiv = input0 / input1;
                outputData[5] = (byte)(((FPDiv == 0) ? 8 : 0) + ((FPDiv is float.NaN) ? 1 : 0));
            }
            catch (DivideByZeroException)
            {
                outputData[5] = 0x10;
            }

            // Addition
            float A = ((CTRL0 >> 4) & 3) switch
            {
                0 => input0,
                1 => input1,
                2 => FPMult,
                3 => FPDiv,
                _ => 0.0f
            };

            float B = ((CTRL0 >> 6) & 3) switch
            {
                0 => input0,
                1 => input1,
                2 => FPMult,
                3 => FPDiv,
                _ => 0.0f
            };

            float AddSubValue = (CTRL0 & 8) != 0 ? A + B : A - B;

            float result = (CTRL1) switch
            {
                0 => FPMult,
                1 => FPDiv,
                2 => AddSubValue,
                3 => 1.0f,
                _ => 0.0f
            };

            byte[] buffer = BitConverter.GetBytes(result);
            buffer.CopyTo(outputData, 8);

            uint fixedResult = ConvertFPtoFixed(result);
            buffer = BitConverter.GetBytes(fixedResult);
            buffer.CopyTo(outputData, 12);
        }

        // We're looking at the LSB 12 bits to determine the fractional portion
        private float ConvertFixedtoFP(uint value)
        {
            float fractional = 0;
            float bit = 0.5f;

            for (short mask = 0b1000_0000_0000; mask > 0; mask = (short)(mask >> 1))
            {
                fractional += (value & mask) != 0 ? bit : 0;
                bit /= 2;
            }

            return (value >> 12) + fractional;
        }

        private uint ConvertFPtoFixed(float value)
        {
            int intVal = (int)value;
            float remainder = value - intVal;
            float bit = 0.5f;
            uint result = (uint)(intVal << 12);

            for (short mask = 0b1000_0000_0000; mask > 0; mask = (short)(mask >> 1))
            {
                if (remainder > bit)
                {
                    result += (uint)mask;
                    remainder -= bit;
                }

                bit /= 2;
            }

            return result;
        }
    }
}
