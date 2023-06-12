namespace FoenixCore.Processor.Generic
{
    /// <summary>
    /// Class to encapsulate the state of the CoreCpu 'Control Pins'. This is a made
    /// up term for those pins that control the CoreCpu execution, Reset, IRQ, etc.
    /// We treat them as a single register which makes checking to see if any
    /// one set much faster than if they were individual bool objects.
    /// </summary>
    public class CpuPins
    {
        // Pins
        /// <summary>
        /// Pause the CoreCpu to allow slow I/O or memory operations. When true, the CoreCpu will not execute 
        /// the next instruction.
        /// </summary>
        public bool Ready_ = false;

        /// <summary>
        /// When high, the CoreCpu is being reset. The CoreCpu will not execute
        /// instructions while reset is high. Once reset goes low (false),
        /// the CoreCpu will read the reset interrupt vector from memory, set the 
        /// Program Counter to the address in the vector, and begin executing 
        /// instructions
        /// </summary>
        public bool RESET = false;

        /// <summary>
        /// Execute a non-maskable interrupt
        /// </summary>
        public bool NMI = false;

        /// <summary>
        /// Execute an interrupt request
        /// </summary>
        public bool IRQ = false;

        /// <summary>
        /// Aborts the current instruction. Control is shifted to the Abort vector.
        /// </summary>
        public bool ABORT = false;

        /// <summary>
        /// When high, the CoreCpu is reading interrupt/reset vectors
        /// </summary>
        public bool VectorPull = false;

        /// <summary>
        /// Helper method to let CoreCpu class know an interrupt pin is high
        /// </summary>
        public bool InterruptPinActive => RESET || NMI || IRQ || ABORT;
    }
}
