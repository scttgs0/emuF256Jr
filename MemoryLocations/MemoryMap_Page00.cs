namespace FoenixCore.MemoryLocations
{
    public static partial class MemoryMap
    {
        #region Direct page

        public const ushort VECTOR_COP = 0xFFF4;        // 2-Byte interrupt handler
        //public const ushort VECTOR_BRK = 0xFFF6;      // 2-Byte interrupt handler
        public const ushort VECTOR_ABORT = 0xFFF8;      // 2-Byte interrupt handler
        public const ushort VECTOR_NMI = 0xFFFA;        // 2-Byte interrupt handler
        public const ushort VECTOR_RESET = 0xFFFC;      // 2-Byte interrupt handler
        public const ushort VECTOR_IRQ_BRK = 0xFFFE;    // 2-Byte interrupt handler

        #endregion
    }
}
