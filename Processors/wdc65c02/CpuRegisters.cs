using System;

namespace FoenixCore.Processor.wdc65c02
{
    public partial class CentralProcessingUnit
    {
        public int PC = 0;

        /// <summary>
        /// Accumulator
        /// </summary>
        public Processor.Generic.Register A = new();

        /// <summary>
        /// Processor Status Register
        /// </summary>
        public CpuFlags Flags = new();

        /// <summary>
        /// Stack Pointer. The stack is always in the first 64KB page.
        /// </summary>
        public RegisterStackPointer Stack = new();

        /// <summary>
        /// X Index Regiser
        /// </summary>
        public Processor.Generic.Register X = new();

        /// <summary>
        /// Y Index Register
        /// </summary>
        public Processor.Generic.Register Y = new();

        /// <summary>
        /// Wait state. When Wait is true, the CoreCpu will not exeucte instructions. It
        /// will service the IRQ, NMI, and ABORT lines. A hardware interrupt is required 
        /// to restart the CoreCpu.
        /// </summary>
        public bool Waiting;

        /// <summary>
        /// Processor status register (P)
        /// </summary>
        public CpuFlags P => Flags;

        /// <summary>
        /// Stack pointer (S)
        /// </summary>
        public Processor.Generic.Register16 S => Stack;

        public TimeSpan CycleTime => DateTime.Now - checkStartTime;
    }
}
