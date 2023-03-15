
using System;
using System.Text;

using FoenixCore.Processor.mc6809;


namespace FoenixCore.Simulator.FileFormat
{
    /// <summary>
    /// Container to hold one line of mc6809 code debugging data
    /// </summary>
    public class DebugLine : ICloneable
    {
        //public bool isBreakpoint = false;
        public int PC;
        byte[] command;
        public int commandLength = 0;
        private string source;
        public bool StepOver = false;
        public string label;
        private string evaled = null;

        private static readonly byte[] BranchJmpOpcodes = {
            OpcodeList.LBRA_Relative,
            OpcodeList.LBSR_Relative,
            OpcodeList.BRA_Relative,
            OpcodeList.BHI_Relative,
            OpcodeList.BLS_Relative,
            OpcodeList.BCC_Relative,
            OpcodeList.BCS_Relative,
            OpcodeList.BNE_Relative,
            OpcodeList.BEQ_Relative,
            OpcodeList.BVC_Relative,
            OpcodeList.BVS_Relative,
            OpcodeList.BPL_Relative,
            OpcodeList.BMI_Relative,
            OpcodeList.BGE_Relative,
            OpcodeList.BLT_Relative,
            OpcodeList.BGT_Relative,
            OpcodeList.BLE_Relative,
            OpcodeList.BSR_Relative,
            OpcodeList.JMP_Direct,
            OpcodeList.JMP_Indexed,
            OpcodeList.JMP_Extended
        };

        private static readonly byte[] NonImmediateOpcodes = {
            // OpcodeList.LDA_Absolute,
            // OpcodeList.LDA_AbsoluteIndexedWithX,
            // OpcodeList.LDA_AbsoluteIndexedWithY,
            // OpcodeList.LDA_ZeroPage,
            // OpcodeList.LDA_ZeroPageIndirect,

            // OpcodeList.STA_Absolute,
            // OpcodeList.STA_AbsoluteIndexedWithX,
            // OpcodeList.STA_AbsoluteIndexedWithY,
            // OpcodeList.STA_ZeroPage,
            // OpcodeList.STA_ZeroPageIndirect
        };

        // Only expand when it's going to be displayed
        override public string ToString()
        {
            if (evaled == null)
            {
                StringBuilder c = new();
                for (int i = 0; i < 4; ++i)
                {
                    if (i < commandLength)
                        c.Append(command[i].ToString("X2")).Append(" ");
                    else
                        c.Append("   ");
                }

                evaled = string.Format("{0}  {1} {2}  {3}", PC.ToString("X6"), c.ToString(), source, null);
            }

            return evaled;
        }

        public DebugLine(int pc)
        {
            PC = pc;
        }

        public void SetOpcodes(byte[] cmd)
        {
            commandLength = cmd.Length;
            command = cmd;
            StepOver = (Array.Exists(BranchJmpOpcodes, element => element == command[0]));
        }

        public void SetOpcodes(string cmd)
        {
            string[] ops = cmd.Split(',');
            commandLength = ops.Length - 1;
            command = new byte[commandLength];

            for (int i = 0; i < commandLength; ++i)
                command[i] = Convert.ToByte(ops[i], 16);

            if (commandLength > 0)
                StepOver = (Array.Exists(BranchJmpOpcodes, element => element == command[0]));
        }

        public string GetOpcodes()
        {
            StringBuilder c = new();

            for (int i = 0; i < commandLength; ++i)
                c.Append(command[i].ToString("X2")).Append(",");

            return c.ToString();
        }

        public string GetSource()
        {
            return source;
        }

        public void SetLabel(string value)
        {
            int idx = value.IndexOf(':');
            if (idx > -1)
                value = value[..idx];

            label = value;
        }

        public void SetMnemonic(string value)
        {
            value = value.Trim(new char[] { ' ' });

            // Detect if the lines contains a label
            string[] tokens = value.Split();
            if (tokens.Length > 0)
            {
                if (tokens[0].Length > 3 && !tokens[0].StartsWith(";"))
                {
                    label = tokens[0];
                    // Remove the first item
                    source = value[label.Length..].Trim();
                }
                else
                    source = value;
            }
            else
                source = value;
        }

        public bool CheckOpcodes(MemoryLocations.MemoryRAM ram)
        {
            for (int i = 0; i < commandLength; ++i)
                if (ram.ReadByte(PC + i) != command[i])
                    return false;

            return true;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }

        /*
         * Return true if the line has a non-immediate LDA/STA opcode
         */
        public bool HasAddress()
        {
            return commandLength > 0 && Array.Exists(NonImmediateOpcodes, element => element == command[0]);
        }

        /*
         * Return the name of the address in this line.
         * The format is STA/LDA $123456 or like it.
         */
        public string GetAddressName()
        {
            string mnemonic = source[4..];

            int colon = mnemonic.IndexOf(';');
            if (colon > -1)
                mnemonic = mnemonic.Substring(0, colon - 1).Trim();

            return mnemonic;
        }

        /*
         * Return the address of this line
         */
        public int GetAddress()
        {
            // Read the opcodes in reverse
            int address = 0;

            for (int i = 1; i < commandLength; ++i)
                address += command[i] * (int)Math.Pow(256, i - 1);

            return address;
        }
    }
}
