
using System;
using System.Threading;

using FoenixCore.Processor.GenericNew;
using FoenixCore.MemoryLocations;


namespace FoenixCore.Processor.mc6809
{
    /// <summary>
    /// Operations. This class encompasses the CoreCpu operations and the support routines needed to execute
    /// the operations. Execute reads a single opcode from memory, along with its data bytes, then
    /// executes that single step. The virtual CoreCpu state is retained until Execute is called again.
    /// </summary>
    public partial class CentralProcessingUnit
    {
        const int BANKSIZE = 0x2000;
        const int PAGESIZE = 0x100;
        private readonly OpcodeList opcodes = null;

        public OpCode CurrentOpcode = null;
        public uint SignatureBytes = 0;

        public Processor.Generic.CpuPins Pins = new();

        /// <summary>
        /// When true, the CoreCpu will not execute the next instruction. Used by the debugger
        /// to allow the user to analyze memory and the execution trace.
        /// </summary>
        public bool DebugPause = false;

        /// <summary>
        /// Length of the currently executing opcode
        /// </summary>
        public byte OpcodeLength;

        /// <summary>
        /// Number of clock cycles used by the currently exeucting instruction
        /// </summary>
        public int OpcodeCycles;

        /// <summary>
        ///  The virtual CoreCpu speed
        /// </summary>
        private int clockSpeed = 6290000;

        /// <summary>
        /// number of cycles since the last performance checkpoint
        /// </summary>
        private int clockCyles = 0;

        /// <summary>
        /// the number of cycles to pause at until the next performance checkpoint
        /// </summary>
        private long nextCycleCheck = long.MaxValue;

        /// <summary>
        /// The last time the performance was checked.
        /// </summary>
        private DateTime checkStartTime = DateTime.Now;

        public MemoryManager MemMgr = null;
        public Thread CPUThread = null;

        public event Operations.SimulatorCommandEvent SimulatorCommand;

        public int ClockSpeed
        {
            get => clockSpeed;
            set => clockSpeed = value;
        }

        public int[] Snapshot
        {
            get
            {
                int[] snapshot = new int[6];
                snapshot[0] = PC;
                snapshot[1] = A.Value;
                snapshot[2] = X.Value;
                snapshot[3] = Y.Value;
                snapshot[4] = Stack.Value;
                snapshot[5] = Flags.Value;

                return snapshot;
            }
        }

        public CentralProcessingUnit(MemoryManager mm)
        {
            MemMgr = mm;
            clockSpeed = 6290000;
            clockCyles = 0;
            Operations operations = new(this);
            operations.SimulatorCommand += Operations_SimulatorCommand;
            opcodes = new OpcodeList(operations, this);
        }

        private void Operations_SimulatorCommand(int EventID)
        {
            switch (EventID)
            {
                case Processor.Generic.SimulatorCommands.WaitForInterrupt:
                    break;

                case Processor.Generic.SimulatorCommands.RefreshDisplay:
                    SimulatorCommand?.Invoke(EventID);
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Execute for n cycles, then return. This gives the host a chance to do other things in the meantime.
        /// </summary>
        /// <param name="Cycles"></param>
        public void ExecuteCycles(int Cycles)
        {
            ResetCounter(Cycles);

            while (clockCyles < nextCycleCheck && !DebugPause)
                ExecuteNext();
        }

        /// <summary>
        /// Execute a single
        /// return true if an interrupt is present
        /// </summary>
        public bool ExecuteNext()
        {
            if (Pins.Ready_)
                return false;

            if (Pins.InterruptPinActive)
                if (ServiceHardwareInterrupt())
                    return true;

            if (Waiting)
                return false;

            // TODO - if pc > RAM size, then throw an exception
            try {
                CurrentOpcode = opcodes[MemMgr.RAM.ReadByte(PC)];
                OpcodeLength = (byte)CurrentOpcode.Length;
                OpcodeCycles = 1;
                SignatureBytes = ReadSignature(OpcodeLength, PC);

                PC += (ushort)OpcodeLength;
                CurrentOpcode.Execute(SignatureBytes);
                clockCyles += OpcodeCycles;
            }
            catch (Exception) {
                throw new Exception("PC: " + PC.ToString("X4"));
            }

            return false;
        }

        // Simulator State management
        /// <summary>
        /// Pause the CoreCpu execution due to a STP instruction. The CoreCpu may only be restarted
        /// by the Reset pin. In the simulator, this will close the CoreCpu execution thread.
        /// Restart the CoreCpu by executing Reset() and then Run()
        /// </summary>
        public void Halt()
        {
            if (CPUThread != null)
            {
                if (CPUThread.ThreadState == ThreadState.Running)
                {
                    DebugPause = true;
                    CPUThread.Join(1000);
                }

                CPUThread = null;
            }
        }

        public void Reset()
        {
            Pins.VectorPull = true;
            MemMgr.VectorPull = true;

            Flags.Value = 0;
            A.Value = 0;
            X.Value = 0;
            Y.Value = 0;
            Stack.Reset();

            PC = MemMgr.ReadWord(MemoryMap.VECTOR_RESET);

            Flags.Irq = true;
            Pins.IRQ = false;
            Pins.VectorPull = false;
            MemMgr.VectorPull = false;
            Waiting = false;
        }

        /// <summary>
        /// Fetch and decode the next instruction for debugging purposes
        /// </summary>
        public OpCode PreFetch()
        {
            return opcodes[MemMgr[PC]];
        }

        public uint ReadSignature(byte length, int pc)
        {
            return length switch
            {
                2 => MemMgr.RAM.ReadByte(pc + 1),
                3 => (uint)MemMgr.RAM.ReadWord(pc + 1),
                _ => 0,
            };
        }

        /// <summary>
        /// Clock cycles used for performance counter. This will be periodically reset to zero
        /// as the throttling routine adjusts the system performance.
        /// </summary>
        public int CycleCounter => clockCyles;

        #region support routines
        /// <summary>
        /// Gets the address pointed to by a pointer in the data bank.
        /// </summary>
        /// <param name="baseAddress"></param>
        /// <param name="Index"></param>
        /// <returns></returns>
        private int GetPointerLocal(int baseAddress, Register<byte> Index = null)
        {
            int addr = baseAddress;

            if (Index != null)
                addr += Index.Value;

            return addr;
        }

        /// <summary>
        /// Gets the address pointed to by a pointer in Direct page.
        /// be in the Direct Page. The address returned will be DBR+Pointer.
        /// </summary>
        /// <param name="baseAddress"></param>
        /// <param name="Index"></param>
        /// <returns></returns>
        private int GetPointerDirect(int baseAddress, Register<byte> Index = null)
        {
            int addr = baseAddress;

            if (Index != null)
                addr += Index.Value;

            int pointer = MemMgr.ReadWord(addr);

            return pointer;
        }

        #endregion

        /// <summary>
        /// Change execution to anohter address in the same bank
        /// </summary>
        /// <param name="addr"></param>
        public void JumpShort(ushort addr)
        {
            PC = addr;
        }

        public void JumpVector(int VectorAddress)
        {
            PC = MemMgr.ReadWord(VectorAddress);
        }

        public static byte GetByte(int Value, int Offset)
        {
            if (Offset == 0)
                return (byte)(Value & 0xff);

            if (Offset == 1)
                return (byte)(Value >> 8 & 0xff);

            if (Offset == 2)
                return (byte)(Value >> 16 & 0xff);

            throw new Exception("Offset must be 0-2. Got " + Offset.ToString());
        }

        public void Push(int value, ushort bytes)
        {
            if (bytes < 1 || bytes > 3)
                throw new Exception("bytes must be between 1 and 3. Got " + bytes.ToString());

            Stack.Value -= bytes;
            MemMgr.Write(Stack.Value + 1, value, bytes);
        }

        public void Push(Register<byte> Reg, int Offset)
        {
            Push(Reg.Value + Offset, 1);
        }

        public void Push(Register<byte> Reg)
        {
            Push(Reg.Value, 1);
        }

        public int Pull(ushort bytes)
        {
            if (bytes < 1 || bytes > 3)
                throw new Exception("bytes must be between 1 and 3. got " + bytes.ToString());

            int ret = MemMgr.Read(Stack.Value + 1, bytes);

            Stack.Value += bytes;

            return ret;
        }

        public void PullInto(Register<byte> Register)
        {
            Register.Value = (byte)Pull(1);
        }

        /// <summary>
        /// Service highest priority interrupt
        /// </summary>
        public bool ServiceHardwareInterrupt()
        {
            if (Pins.IRQ && !Flags.Irq)
            {
                //DebugPause = true;
                Pins.IRQ = false;
                Interrupt(Processor.Generic.InterruptTypes.IRQ);

                return true;
            }
            else if (Pins.NMI)
            {
                DebugPause = true;
                Pins.NMI = false;
                Interrupt(Processor.Generic.InterruptTypes.NMI);

                return true;
            }
            else if (Pins.ABORT)
            {
                DebugPause = true;
                Pins.ABORT = false;
                Interrupt(Processor.Generic.InterruptTypes.ABORT);

                return true;
            }
            else if (Pins.RESET)
            {
                DebugPause = true;
                Pins.RESET = false;
                Interrupt(Processor.Generic.InterruptTypes.RESET);

                return true;
            }

            return false;
        }

        public void Interrupt(Processor.Generic.InterruptTypes T)
        {
            //debug
            //DebugPause = true;

            Push(PC & 0xFFFF, 2);

            Push(Flags);

            Flags.Irq = true;

            int addr;

            switch (T)
            {
                case Processor.Generic.InterruptTypes.BRK:
                    addr = MemoryMap.VECTOR_IRQ_BRK;
                    break;

                case Processor.Generic.InterruptTypes.ABORT:
                    addr = MemoryMap.VECTOR_ABORT;
                    break;

                case Processor.Generic.InterruptTypes.IRQ:
                    addr = MemoryMap.VECTOR_IRQ_BRK;
                    break;

                case Processor.Generic.InterruptTypes.NMI:
                    addr = MemoryMap.VECTOR_NMI;
                    break;

                case Processor.Generic.InterruptTypes.RESET:
                    addr = MemoryMap.VECTOR_RESET;
                    break;

                case Processor.Generic.InterruptTypes.COP:
                    addr = MemoryMap.VECTOR_COP;
                    break;

                default:
                    throw new Exception("Invalid interrupt type: " + T.ToString());
            }

            Waiting = false;

            JumpVector(addr);
        }

        public void ResetCounter(int maxCycles)
        {
            clockCyles = 0;
            nextCycleCheck = maxCycles;
            checkStartTime = DateTime.Now;
        }
    }
}
