using System;
using FoenixCore.Processor.GenericNew;


namespace FoenixCore.Processor.wdc65c02
{
    public partial class CentralProcessingUnit
    {
        public ushort PC = 0;

        /// <summary>
        /// Accumulator
        /// </summary>
        public Register<byte> A = new();

        /// <summary>
        /// X Index Register
        /// </summary>
        public Register<byte> X = new();

        /// <summary>
        /// Y Index Register
        /// </summary>
        public Register<byte> Y = new();

        /// <summary>
        /// Processor Status Register
        /// </summary>
        public CpuFlags Flags = new();

        /// <summary>
        /// Stack Pointer. The stack is always in the first 64KB page.
        /// </summary>
        public RegisterStackPointer Stack = new();

        /// <summary>
        /// Wait state. When Wait is true, the CoreCpu will not execute instructions. It
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
        public Register<ushort> S => Stack;

        public TimeSpan CycleTime => DateTime.Now - checkStartTime;
    }
}
