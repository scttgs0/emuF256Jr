using System;
using System.Diagnostics;

using FoenixCore.Processor.GenericNew;


namespace FoenixCore.Processor.wdc65c02
{
    public class OpCode
    {
        public byte Value;
        public string Mnemonic;
        public AddressModes AddressMode;
        public delegate void ExecuteDelegate(byte Instruction, AddressModes AddressMode, uint Signature);
        public event ExecuteDelegate ExecuteOp;
        public int Length8Bit;
        public Register<byte> ActionRegister = null;

        public OpCode(byte Value, string Mnemonic, int Length8Bit, Register<byte> ActionRegister, AddressModes Mode, ExecuteDelegate newDelegate)
        {
            this.Value = Value;
            this.Length8Bit = Length8Bit;
            this.ActionRegister = ActionRegister;
            this.Mnemonic = Mnemonic;
            AddressMode = Mode;
            ExecuteOp += newDelegate;

            Debug.WriteLine("public const int " + Mnemonic + "_" + Mode.ToString() + "=0x" + Value.ToString("X2") + ";");
        }

        public OpCode(byte Value, string Mnemonic, int Length, AddressModes Mode, ExecuteDelegate newDelegate)
        {
            this.Value = Value;
            Length8Bit = Length;
            this.Mnemonic = Mnemonic;
            AddressMode = Mode;
            ExecuteOp += newDelegate;

            Debug.WriteLine("public const int " + Mnemonic + "_" + Mode.ToString() + "=0x" + Value.ToString("X2") + ";");
        }

        public void Execute(uint SignatureBytes)
        {
            if (ExecuteOp == null)
                throw new NotImplementedException("Tried to execute " + Mnemonic + " but it is not implemented.");

            ExecuteOp(Value, AddressMode, SignatureBytes);
        }

        public int Length
        {
            get
            {
                return Length8Bit;
            }
        }

        public override string ToString()
        {
            return Mnemonic + " " + AddressMode.ToString();
        }

        public string ToString(uint Signature)
        {
            string sig;

            if (Length == 3)
                sig = "$" + Signature.ToString("X4");
            else if (Length == 4)
                sig = "$" + Signature.ToString("X6");
            else
                sig = "$" + Signature.ToString("X2");

            string arg = AddressMode switch
            {
                AddressModes.Immediate => "#" + sig,
                AddressModes.ZeroPage or AddressModes.Absolute => sig,
                AddressModes.ZeroPageIndirect => "(" + sig + ")",
                AddressModes.ZeroPageIndexedIndirectWithX => "(" + sig + ",X)",
                AddressModes.ZeroPageIndexedWithX or AddressModes.AbsoluteIndexedWithX => sig + ",X",
                AddressModes.ZeroPageIndexedWithY or AddressModes.AbsoluteIndexedWithY => sig + ",Y",
                AddressModes.ZeroPageIndirectIndexedWithY => "(" + sig + "),Y",
                AddressModes.ProgramCounterRelative => sig,
                _ => "",
            };

            return Mnemonic + " " + arg;
        }
    }
}
