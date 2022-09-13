using System;

using FoenixCore.Processor.GenericNew;
using FoenixCore.MemoryLocations;


namespace FoenixCore.Processor.wdc65c02
{
    /*
     * This file contains all of the opcode routines for the Operations class.
    */
    public class Operations
    {
        private readonly CentralProcessingUnit cpu;

        /// <summary>
        /// Used for addressing modes that
        /// </summary>
        public const int ADDRESS_IMMEDIATE = 0xf000001;
        public const int ADDRESS_IMPLIED = 0xf000002;

        public delegate void SimulatorCommandEvent(int EventID);
        public event SimulatorCommandEvent SimulatorCommand;

        public Operations(CentralProcessingUnit cpu)
        {
            this.cpu = cpu;
        }

        public void Reset()
        {
            cpu.A = new Register<byte>();
            cpu.X = new Register<byte>();
            cpu.Y = new Register<byte>();
            cpu.Flags.Reset();
            //cpu.PC.Reset();

            cpu.PC = cpu.MemMgr.ReadWord(MemoryMap.VECTOR_RESET);
        }

        /// <summary>
        /// This opcode is not implemented yet.
        /// </summary>
        public void ExecuteAbort()
        {
            cpu.OpcodeLength = 1;
            cpu.OpcodeCycles = 1;

            cpu.Interrupt(Processor.Generic.InterruptTypes.ABORT);
        }

        public void OpORA(byte val)
        {
            val &= 0xff;

            cpu.A.Value |= val;
            cpu.Flags.SetNZ(cpu.A.Value, 1);
        }

        public void OpLoad(Register<byte> Dest, byte value)
        {
            Dest.Value = value;
            cpu.Flags.SetNZ(Dest.Value, 1);
        }

        /// <summary>
        /// Branch instructions take a *signed* 8-bit value. The offset is added to the address of the NEXT instruction, so
        /// branches are always PC + 2 + offset.
        /// </summary>
        /// <param name="b"></param>
        public void BranchNear(byte b)
        {
            ushort offset = (ushort)MakeSignedByte(b);
            cpu.PC += offset;
        }

        public sbyte MakeSignedByte(byte b)
        {
            return (sbyte)b;
        }

        public Int16 MakeSignedInt(UInt16 b)
        {
            return (Int16)b;
        }

        public Int16 MakeSignedWord(UInt16 b)
        {
            return (Int16)b;
        }

        /// <summary>
        /// Retrieve final data from memory, based on address mode.
        /// <para>For immediate addressing, just returns the input value</para>
        /// <para>For absolute addressing, returns data at address in signature bytes</para>
        /// <para>For indirect addressing, returns data at address pointed to by address in signature</para>
        /// <para>For indexed modes, uses appropriate index register to adjust the address</para>
        /// </summary>
        /// <param name="mode">Address mode. Direct, Absolute, Immediate, etc. Each mode determines where the data
        /// is located and how the signature bytes are interpreted.</param>
        /// <param name="signatureBytes">byte or bytes immediately following the opcode. Varies based on the opcode.</param>
        /// <param name="isCode">Assume the address is code and uses the Program Bank Register.
        /// Otherwise uses the Data Bank Register, if appropriate.</param>
        /// <returns></returns>
        public int GetValue(AddressModes mode, int signatureBytes, int width)
        {
            return mode switch
            {
                AddressModes.Accumulator => cpu.A.Value,
                AddressModes.Absolute => GetAbsolute(signatureBytes, width),
                AddressModes.AbsoluteIndexedWithX => GetIndexed(signatureBytes, cpu.X, width),// LDA $2000,X
                AddressModes.AbsoluteIndexedWithY => GetIndexed(signatureBytes, cpu.Y, width),
                AddressModes.ZeroPage => GetAbsolute(signatureBytes, width),
                AddressModes.ZeroPageIndexedWithX => GetIndexed(signatureBytes, cpu.X, width),
                AddressModes.ZeroPageIndexedWithY => GetIndexed(signatureBytes, cpu.Y, width),
                AddressModes.ZeroPageIndexedIndirectWithX => GetDirectIndexedIndirect(signatureBytes, cpu.X),//LDA(dp, X)
                AddressModes.ZeroPageIndirect => GetDirectIndirect(signatureBytes),//LDA (dp)
                AddressModes.ZeroPageIndirectIndexedWithY => GetZeroPageIndirectIndexed(signatureBytes, cpu.Y),//LDA(dp),Y
                AddressModes.ProgramCounterRelative => cpu.PC + 2 + MakeSignedByte((byte)signatureBytes),
                AddressModes.StackImplied => cpu.Stack.Value,
                _ => signatureBytes,
            };
        }

        private int GetDirectIndirect(int Address)
        {
            int addr = Address;
            int ptr = cpu.MemMgr.ReadWord(addr);
            return cpu.MemMgr.ReadWord(ptr);
        }

        private int GetDirectIndirectLong(int Address)
        {
            int addr = Address;
            int ptr = cpu.MemMgr.ReadLong(addr);
            return cpu.MemMgr.ReadWord(ptr);
        }

        /// <summary>
        /// LDA (D),Y - returns value pointed to by (D),Y, where D is in Direct page. Final value will be in Data bank.
        /// </summary>
        /// <param name="Address">Address in direct page</param>
        /// <param name="Y">Register to index</param>
        /// <returns></returns>
        private int GetZeroPageIndirectIndexed(int Address, Register<byte> Y)
        {
            // The indirect address must be in Bank 0
            int addr = Address & 0xFFFF;

            int ptr = cpu.MemMgr.ReadWord(addr) + Y.Value;
            return cpu.MemMgr.ReadByte(ptr);
        }

        /// <summary>
        /// LDA (D,X) - returns value pointed to by D,X, where D is in Direct page. Final value will be in Data bank.
        /// </summary>
        /// <param name="Address">Address in direct page</param>
        /// <param name="X">Register to index</param>
        /// <returns></returns>
        private int GetDirectIndexedIndirect(int Address, Register<byte> X)
        {
            int addr = Address + X.Value;
            int ptr = cpu.MemMgr.ReadWord(addr);
            return cpu.MemMgr.ReadByte(ptr);
        }

        /// <summary>
        /// Read memory at specified address. Optionally use bank register
        /// to select the relevant bank.
        /// </summary>
        /// <param name="Address"></param>
        /// <param name="bank"></param>
        /// <returns></returns>
        private int GetAbsolute(int Address, int width)
        {
            return (width == 1) ? cpu.MemMgr.ReadByte(Address) : cpu.MemMgr.ReadWord(Address);
        }

        /// <summary>
        /// LDA $2000,X
        /// </summary>
        /// <param name="Address"></param>
        /// <param name="bank"></param>
        /// <param name="Index">The Index register - maybe short or long.</param>
        /// <param name="width">The width of the register requesting data</param>
        /// <returns></returns>
        private int GetIndexed(int Address, Register<byte> Index, int width)
        {
            int addr = Address;
            addr += Index.Value;

            return cpu.MemMgr.ReadByte(addr);
        }

        /// <summary>
        /// Get an indirect, indexed Jump address=: JMP ($1200,X)
        /// This looks at location $1200+X in Bank 0 to get the pointer. Then returns an address
        /// in the current Program Bank (PBR + ($1200,X))
        /// </summary>
        /// <param name="Address">Address of pointer</param>
        /// <param name="Index">Offset of address</param>
        /// <returns></returns>
        private int GetJumpAbsoluteIndexedIndirect(int Address, Register<byte> Index)
        {
            int addr = Address + Index.Value;
            int ptr = cpu.MemMgr.ReadWord(addr);
            //return cpu.ProgramBank.GetLongAddress(ptr);

            return (cpu.PC & 0xFF_0000) + ptr;
        }

        /// <summary>
        /// BRK and COP instruction. Pushes the  Program Bank Register, the Program Counter, and the Flags onto the stack.
        /// Then switches to the Bank 0 addresses stored in the approriate vector.
        /// </summary>
        /// <param name="instruction"></param>
        /// <param name="addressMode"></param>
        /// <param name="signature"></param>
        public void ExecuteInterrupt(byte instruction, AddressModes addressMode, int signature)
        {
            cpu.OpcodeLength = 2;
            cpu.OpcodeCycles = 8;

            switch (instruction)
            {
                default:
                    throw new NotImplementedException("Unknown opcode for ExecuteInterrupt: " + instruction.ToString("X2"));
            }
        }

        public void ExecuteORA(byte instruction, AddressModes addressMode, int signature)
        {
            byte val = (byte)GetValue(addressMode, signature, 1);
            cpu.A.Value |= val;
            cpu.Flags.SetNZ(cpu.A.Value, 1);
        }

        /// <summary>
        /// Test memory against the Accumulator. Sets Z based on the result of an AND.
        /// Also sets or clears bits in memory based on the bitmask in the accumulator.
        /// TSB sets bits in memory based on A. TRB clears bits in memory based on A.
        /// </summary>
        /// <param name="instruction"></param>
        /// <param name="addressMode"></param>
        /// <param name="signature"></param>
        public void ExecuteTSBTRB(byte instruction, AddressModes addressMode, int signature)
        {
            int val = GetValue(addressMode, signature, 1);
            int test = val & cpu.A.Value;
            cpu.Flags.SetZ(test);

            int addr = GetAddress(addressMode, signature);

            switch (instruction)
            {
                case OpcodeList.TSB_Absolute:
                case OpcodeList.TSB_ZeroPage:
                    cpu.MemMgr.Write(addr, val | cpu.A.Value, 1);
                    break;

                case OpcodeList.TRB_Absolute:
                case OpcodeList.TRB_ZeroPage:
                    // reset bits in memory when that bit is 1 in A
                    // AND to get bits that are both 1
                    // XOR to force thoses off in memory.
                    int mask = val & cpu.A.Value;
                    cpu.MemMgr.Write(addr, val ^ mask, 1);
                    break;

                default:
                    throw new NotImplementedException("ExecuteTSBTRB() opcode not implemented: " + instruction.ToString("X2"));
            }
        }

        public void ExecuteShift(byte instruction, AddressModes addressMode, int signature)
        {
            int val = GetValue(addressMode, signature, 1);

            switch (instruction)
            {
                case OpcodeList.ASL_ZeroPage:
                case OpcodeList.ASL_Accumulator:
                case OpcodeList.ASL_Absolute:
                case OpcodeList.ASL_ZeroPageIndexedWithX:
                case OpcodeList.ASL_AbsoluteIndexedWithX:
                    val <<= 1;
                    cpu.Flags.Carry = val > 0xff;
                    val &= 0xff;

                    break;

                case OpcodeList.LSR_ZeroPage:
                case OpcodeList.LSR_Accumulator:
                case OpcodeList.LSR_Absolute:
                case OpcodeList.LSR_ZeroPageIndexedWithX:
                case OpcodeList.LSR_AbsoluteIndexedWithX:
                    cpu.Flags.Carry = (val & 1) == 1;
                    val >>= 1;
                    break;

                case OpcodeList.ROL_ZeroPage:
                case OpcodeList.ROL_Accumulator:
                case OpcodeList.ROL_Absolute:
                case OpcodeList.ROL_ZeroPageIndexedWithX:
                case OpcodeList.ROL_AbsoluteIndexedWithX:
                    val <<= 1;
                    if (cpu.Flags.Carry)
                        val += 1;

                    cpu.Flags.Carry = val > 0xff;
                    val &= 0xff;

                    break;

                case OpcodeList.ROR_ZeroPage:
                case OpcodeList.ROR_Accumulator:
                case OpcodeList.ROR_Absolute:
                case OpcodeList.ROR_ZeroPageIndexedWithX:
                case OpcodeList.ROR_AbsoluteIndexedWithX:
                    if (cpu.Flags.Carry)
                    {
                        val += 0x100;
                    }
                    cpu.Flags.Carry = (val & 1) == 1;
                    val >>= 1;
                    break;

                default:
                    throw new NotImplementedException("ExecuteASL() opcode not implemented: " + instruction.ToString("X2"));
            }

            cpu.Flags.SetNZ(val, 1);

            if (addressMode == AddressModes.Accumulator)
                cpu.A.Value = (byte)val;
            else
            {
                int addr = GetAddress(addressMode, signature);
                cpu.MemMgr.Write(addr, val, 1);
            }
        }

        public void ExecuteStack(byte instruction, AddressModes addressMode, int signature)
        {
            switch (instruction)
            {
                case OpcodeList.PHA_StackImplied:
                    cpu.Push(cpu.A);
                    break;

                case OpcodeList.PLA_StackImplied:
                    cpu.PullInto(cpu.A);
                    cpu.Flags.SetNZ(cpu.A.Value, 1);
                    break;

                case OpcodeList.PHX_StackImplied:
                    cpu.Push(cpu.X);
                    break;

                case OpcodeList.PLX_StackImplied:
                    cpu.PullInto(cpu.X);
                    cpu.Flags.SetNZ(cpu.X.Value, 1);
                    break;

                case OpcodeList.PHY_StackImplied:
                    cpu.Push(cpu.Y);
                    break;

                case OpcodeList.PLY_StackImplied:
                    cpu.PullInto(cpu.Y);
                    cpu.Flags.SetNZ(cpu.Y.Value, 1);
                    break;

                case OpcodeList.PHP_StackImplied:
                    cpu.Push(cpu.Flags);
                    break;

                case OpcodeList.PLP_StackImplied:
                    cpu.PullInto(cpu.Flags);
                    break;

                default:
                    throw new NotImplementedException("ExecuteStack() opcode not implemented: " + instruction.ToString("X2"));
            }
        }

        public void ExecuteRMB(byte instruction, AddressModes addressMode, int signature)
        {}

        public void ExecuteSMB(byte instruction, AddressModes addressMode, int signature)
        {}

        public void ExecuteBBR(byte instruction, AddressModes addressMode, int signature)
        {}

        public void ExecuteBBS(byte instruction, AddressModes addressMode, int signature)
        {}

        public void ExecuteBranch(byte instruction, AddressModes addressMode, int signature)
        {
            bool takeBranch;
            switch (instruction)
            {
                case OpcodeList.BCC_ProgramCounterRelative:
                    takeBranch = !cpu.Flags.Carry;
                    break;

                case OpcodeList.BCS_ProgramCounterRelative:
                    takeBranch = cpu.Flags.Carry;
                    break;

                case OpcodeList.BEQ_ProgramCounterRelative:
                    takeBranch = cpu.Flags.Zero;
                    break;

                case OpcodeList.BMI_ProgramCounterRelative:
                    takeBranch = cpu.Flags.Negative;
                    break;

                case OpcodeList.BNE_ProgramCounterRelative:
                    takeBranch = !cpu.Flags.Zero;
                    break;

                case OpcodeList.BPL_ProgramCounterRelative:
                    takeBranch = !cpu.Flags.Negative;
                    break;

                case OpcodeList.BRA_ProgramCounterRelative:
                    takeBranch = true;
                    break;

                case OpcodeList.BVC_ProgramCounterRelative:
                    takeBranch = !cpu.Flags.oVerflow;
                    break;

                case OpcodeList.BVS_ProgramCounterRelative:
                    takeBranch = cpu.Flags.oVerflow;
                    break;

                default:
                    throw new NotImplementedException("ExecuteBranch() opcode not implemented: " + instruction.ToString("X2"));
            }

            if (takeBranch)
                BranchNear((byte)signature);
        }

        public void ExecuteStatusReg(byte instruction, AddressModes addressMode, int signature)
        {
            switch (instruction)
            {
                case OpcodeList.CLC_Implied:
                    cpu.Flags.Carry = false;
                    break;

                case OpcodeList.SEC_Implied:
                    cpu.Flags.Carry = true;
                    break;

                case OpcodeList.CLD_Implied:
                    cpu.Flags.Decimal = false;
                    break;

                case OpcodeList.SED_Implied:
                    cpu.Flags.Decimal = true;
                    break;

                case OpcodeList.CLI_Implied:
                    cpu.Flags.IrqDisable = false;
                    break;

                case OpcodeList.SEI_Implied:
                    cpu.Flags.IrqDisable = true;
                    break;

                case OpcodeList.CLV_Implied:
                    cpu.Flags.oVerflow = false;
                    break;

                default:
                    throw new NotImplementedException("Unknown opcode for ExecuteStatusReg: " + instruction.ToString("X2"));
            }
        }

        public void ExecuteINCDEC(byte instruction, AddressModes addressMode, int signature)
        {
            int addr;
            int bval;

            switch (instruction)
            {
                case OpcodeList.DEC_Accumulator:
                    //cpu.A.Dec();
                    cpu.A.Value -= 1;
                    cpu.Flags.SetNZ(cpu.A.Value, 1);
                    break;

                case OpcodeList.DEC_ZeroPage:
                case OpcodeList.DEC_Absolute:
                case OpcodeList.DEC_ZeroPageIndexedWithX:
                case OpcodeList.DEC_AbsoluteIndexedWithX:
                    bval = GetValue(addressMode, signature, 1);
                    addr = GetAddress(addressMode, signature);
                    bval--;
                    cpu.MemMgr.WriteByte(addr, (byte)bval);
                    cpu.Flags.SetNZ(bval, 1);

                    break;

                case OpcodeList.INC_Accumulator:
                    cpu.A.Value += 1;
                    //cpu.A.Inc();
                    cpu.Flags.SetNZ(cpu.A.Value, 1);
                    break;

                case OpcodeList.INC_ZeroPage:
                case OpcodeList.INC_Absolute:
                case OpcodeList.INC_ZeroPageIndexedWithX:
                case OpcodeList.INC_AbsoluteIndexedWithX:
                    //addr = cpu.ZeroPage.GetLongAddress(addr);
                    bval = GetValue(addressMode, signature, 1);
                    addr = GetAddress(addressMode, signature);
                    bval++;

                    cpu.MemMgr.WriteByte(addr, (byte)bval);
                    cpu.Flags.SetNZ(bval, 1);

                    break;

                case OpcodeList.DEX_Implied:
                    cpu.X.Value -= 1;
                    //cpu.X.Dec();
                    cpu.Flags.SetNZ(cpu.X.Value, 1);
                    break;

                case OpcodeList.INX_Implied:
                    cpu.X.Value += 1;
                    //cpu.X.Inc();
                    cpu.Flags.SetNZ(cpu.X.Value, 1);
                    break;

                case OpcodeList.DEY_Implied:
                    cpu.Y.Value -= 1;
                    //cpu.Y.Dec();
                    cpu.Flags.SetNZ(cpu.Y.Value, 1);
                    break;

                case OpcodeList.INY_Implied:
                    cpu.Y.Value += 1;
                    //cpu.Y.Inc();
                    cpu.Flags.SetNZ(cpu.Y.Value, 1);
                    break;

                default:
                    break;
            }
        }

        private int GetAddress(AddressModes addressMode, int SignatureBytes)
        {
            int addr;
            int ptr;

            switch (addressMode)
            {
                // The address will not be used in Immediate or Implied mode, but
                case AddressModes.Immediate:
                    return ADDRESS_IMMEDIATE;

                case AddressModes.Implied:
                    return ADDRESS_IMPLIED;

                case AddressModes.Absolute:
                    return SignatureBytes;

                case AddressModes.AbsoluteIndexedWithX:
                    return SignatureBytes + cpu.X.Value;

                case AddressModes.AbsoluteIndexedWithY:
                    return SignatureBytes + cpu.Y.Value;

                case AddressModes.ZeroPage:
                    return SignatureBytes;

                case AddressModes.ZeroPageIndexedWithX:
                    return SignatureBytes + cpu.X.Value;

                case AddressModes.ZeroPageIndexedWithY:
                    return SignatureBytes + cpu.Y.Value;

                case AddressModes.ZeroPageIndexedIndirectWithX:
                    addr = SignatureBytes + cpu.X.Value;
                    ptr = cpu.MemMgr.ReadWord(addr);
                    //return cpu.ProgramBank.GetLongAddress(ptr);
                    return (cpu.PC & 0xFF_0000) + ptr;

                case AddressModes.ZeroPageIndirect:
                    addr = SignatureBytes;
                    ptr = cpu.MemMgr.ReadWord(addr);
                    return ptr;

                case AddressModes.ZeroPageIndirectIndexedWithY:
                    addr = SignatureBytes;
                    ptr = cpu.MemMgr.ReadWord(addr) + cpu.Y.Value;
                    //return cpu.ProgramBank.GetLongAddress(ptr);
                    return (cpu.PC & 0xFF_0000) + ptr;

                case AddressModes.ProgramCounterRelative:
                    ptr = MakeSignedByte((byte)SignatureBytes);
                    addr = cpu.PC + ptr;
                    return addr;

                case AddressModes.StackImplied:
                    return 0;

                case AddressModes.Accumulator:
                    return 0;

                default:
                    throw new NotImplementedException("GetAddress() Address mode not implemented: " + addressMode.ToString());
            }
        }

        public void ExecuteTransfer(byte instruction, AddressModes addressMode, int signature)
        {
            int transWidth;

            switch (instruction)
            {
                // A - X
                case OpcodeList.TXA_Implied:
                    transWidth = 1;
                    cpu.A.Value = cpu.X.Value;
                    cpu.Flags.SetNZ(cpu.A.Value, transWidth);
                    break;

                case OpcodeList.TAX_Implied:
                    transWidth = 1;
                    cpu.X.Value = cpu.A.Value;
                    cpu.Flags.SetNZ(cpu.X.Value, transWidth);
                    break;

                // A - Y
                case OpcodeList.TYA_Implied:
                    transWidth = 1;
                    cpu.A.Value = cpu.Y.Value;
                    cpu.Flags.SetNZ(cpu.A.Value, transWidth);
                    break;

                case OpcodeList.TAY_Implied:
                    transWidth = 1;
                    cpu.Y.Value = cpu.A.Value;
                    cpu.Flags.SetNZ(cpu.Y.Value, transWidth);
                    break;

                // S - X
                case OpcodeList.TSX_Implied:
                    transWidth = 1;
                    cpu.X.Value = (byte)(cpu.Stack.Value & 0xFF);
                    cpu.Flags.SetNZ(cpu.X.Value, transWidth);
                    break;

                case OpcodeList.TXS_Implied:
                    cpu.Stack.Value = cpu.X.Value;
                    cpu.Stack.TopOfStack = cpu.X.Value;
                    break;

                default:
                    throw new NotImplementedException("ExecuteTransfer() opcode not implemented: " + instruction.ToString("X2"));
            }
        }

        public void ExecuteJumpReturn(byte instruction, AddressModes addressMode, int signature)
        {
            ushort addr = (ushort)GetAddress(addressMode, signature);

            switch (instruction)
            {
                case OpcodeList.JSR_Absolute:
                    cpu.Push(cpu.PC - 1, 2);
                    cpu.JumpShort(addr);
                    return;

                case OpcodeList.JMP_Absolute:
                case OpcodeList.JMP_AbsoluteIndirect:
                case OpcodeList.JMP_AbsoluteIndexedIndirectWithX:
                    cpu.JumpShort(addr);
                    return;

                // RTS, RTL, RTI
                case OpcodeList.RTI_StackImplied:
                    cpu.Flags.SetFlags(cpu.Pull(1));

                    cpu.JumpShort((ushort)cpu.Pull(2));
                    return;

                case OpcodeList.RTS_StackImplied:
                    cpu.JumpShort((ushort)(cpu.Pull(2) + 1));
                    return;

                default:
                    throw new NotImplementedException("ExecuteJumpReturn() opcode not implemented: " + instruction.ToString("X2"));
            }
        }

        public void ExecuteAND(byte instruction, AddressModes addressMode, int signature)
        {
            byte data = (byte)GetValue(addressMode, signature, 1);
            cpu.A.Value &= data;
            cpu.Flags.SetNZ(cpu.A.Value, 1);
        }

        public void ExecuteBIT(byte instruction, AddressModes addressMode, int signature)
        {
            int data = GetValue(addressMode, signature, 1);
            int result = cpu.A.Value & data;
            cpu.Flags.SetZ(result);

            if (addressMode != AddressModes.Immediate)
            {
                cpu.Flags.oVerflow = (data & 0x40) != 0;
                cpu.Flags.Negative = (data & 0x80) != 0;
            }
        }

        public void ExecuteEOR(byte instruction, AddressModes addressMode, int signature)
        {
            byte val = (byte)GetValue(addressMode, signature, 1);
            cpu.A.Value ^= val;
            cpu.Flags.SetNZ(cpu.A.Value, 1);
        }

        public void ExecuteMisc(byte instruction, AddressModes addressMode, int signature)
        {
            switch (instruction)
            {
                case OpcodeList.NOP_Implied:
                    break;

                case OpcodeList.STP_Implied: //stop
                    cpu.Halt();
                    break;

                default:
                    throw new NotImplementedException("ExecuteMisc() opcode not implemented: " + instruction.ToString("X2"));
            }
        }

        private void OnSimulatorCommand(int signature)
        {
            if (SimulatorCommand == null)
                return;

            SimulatorCommand(signature);
        }


        public void ExecuteADC(byte instruction, AddressModes addressMode, int signature)
        {
            ushort val = (ushort)GetValue(addressMode, signature, 1);
            ushort nv;

            if (cpu.Flags.Decimal)
                nv = (ushort)HexVal(BCDVal(val) + BCDVal(cpu.A.Value) + cpu.Flags.CarryBit);
            else
                nv = (ushort)(val + cpu.A.Value + cpu.Flags.CarryBit);

            cpu.Flags.Carry = (nv < 0 || nv > 0xFFFF);

            // We need to detect a wraparound - when the sign changes but there is no overflow
            cpu.Flags.oVerflow = ((cpu.A.Value ^ nv) & (val ^ nv) & 0x80) != 0;
            cpu.Flags.SetNZ(nv, 1);

            cpu.A.Value = (byte)nv;
        }

        /// <summary>
        /// Subtract value from accumulator. Carry acts as a "borrow". When Carry is 0,
        /// subtract one more from Accumulator. Carry will be 0 if result < 0 and 1 if result >= 0.
        /// </summary>
        /// <param name="instruction"></param>
        /// <param name="addressMode"></param>
        /// <param name="signature"></param>
        public void ExecuteSBC(byte instruction, AddressModes addressMode, int signature)
        {
            ushort val = (ushort)GetValue(addressMode, signature, 1);
            ushort nv;

            if (cpu.Flags.Decimal)
                nv = (ushort)HexVal(BCDVal(cpu.A.Value) - BCDVal(val + 1) + cpu.Flags.CarryBit);
            else
                nv = (ushort)(cpu.A.Value - val - 1 + cpu.Flags.CarryBit);

            cpu.Flags.Carry = (nv >= 0 && nv <= 0xFFFF);

            cpu.Flags.oVerflow = ((cpu.A.Value ^ nv) & ((256 - val) ^ nv) & 0x80) != 0;

            cpu.Flags.SetNZ(nv, 1);

            cpu.A.Value = (byte)nv;
        }

        private int BCDVal(int value)
        {
            if (value < 256)
                return (((value & 0xF0) >> 4) * 10) + (value & 0xF);
            else
            {
                int val = (((value & 0xF000) >> 12) * 1000) + (((value & 0xF00) >> 8) * 100) + (((value & 0xF0) >> 4) * 10) + (value & 0xF);
                return val;
            }
        }

        private int HexVal(int bcd)
        {
            return bcd / 10000 * 256 * 256 + (bcd % 10000) / 1000 * 256 * 16 + ((bcd % 10000) % 1000) / 100 * 256 + (((bcd % 10000) % 1000) % 100) / 10 * 16 + (((bcd % 10000) % 1000) % 100) % 10;
        }

        public void ExecuteSTZ(byte instruction, AddressModes addressMode, int signature)
        {
            int addr = GetAddress(addressMode, signature);
            cpu.MemMgr.Write(addr, 0, 1);
        }

        public void ExecuteSTA(byte instruction, AddressModes addressMode, int signature)
        {
            int addr = GetAddress(addressMode, signature);
            cpu.MemMgr.Write(addr, cpu.A.Value, 1);
        }

        public void ExecuteSTY(byte instruction, AddressModes addressMode, int signature)
        {
            int addr = GetAddress(addressMode, signature);
            cpu.MemMgr.Write(addr, cpu.Y.Value, 1);
        }

        public void ExecuteSTX(byte instruction, AddressModes addressMode, int signature)
        {
            int addr = GetAddress(addressMode, signature);
            cpu.MemMgr.Write(addr, cpu.X.Value, 1);
        }

        public void ExecuteLDA(byte instruction, AddressModes addressMode, int signature)
        {
            byte val = (byte)GetValue(addressMode, signature, 1);
            cpu.A.Value = val;
            cpu.Flags.SetNZ(cpu.A.Value, 1);
        }

        public void ExecuteLDX(byte instruction, AddressModes addressMode, int signature)
        {
            byte val = (byte)GetValue(addressMode, signature, 1);
            cpu.X.Value = val;
            cpu.Flags.SetNZ(cpu.X.Value, 1);
        }

        public void ExecuteLDY(byte instruction, AddressModes addressMode, int signature)
        {
            byte val = (byte)GetValue(addressMode, signature, 1);
            cpu.Y.Value = val;
            cpu.Flags.SetNZ(cpu.Y.Value, 1);
        }

        public void ExecuteCPX(byte instruction, AddressModes addressMode, int signature)
        {
            Compare(addressMode, signature, cpu.X);
        }

        public void ExecuteCPY(byte instruction, AddressModes addressMode, int signature)
        {
            Compare(addressMode, signature, cpu.Y);
        }

        public void ExecuteCMP(byte instruction, AddressModes addressMode, int signature)
        {
            Compare(addressMode, signature, cpu.A);
        }

        public void Compare(AddressModes addressMode, int signature, Register<byte> Reg)
        {
            int val = GetValue(addressMode, signature, 1);
            val = val & 0xFF;

            int subResult = Reg.Value - val;
            cpu.Flags.Zero = subResult == 0;
            cpu.Flags.Carry = Reg.Value >= val;
            cpu.Flags.Negative = (subResult & 0x80) == 0x80;
        }

        public void ExecuteWAI(byte Instruction, AddressModes AddressMode, int Signature)
        {
            cpu.Waiting = true;
        }
    }
}
