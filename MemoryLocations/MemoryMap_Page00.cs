namespace FoenixCore.MemoryLocations
{
    public static partial class MemoryMap
    {
        #region Direct page

        public const int CURSORX = 0x001A;              // 2 Bytes
                                                        // This is where the blinking cursor sits.
                                                        // Do not edit this directly.
                                                        // Call LOCATE to update the location and handle moving the cursor correctly. 
        public const int CURSORY = 0x001C;              // 2 Bytes
                                                        // This is where the blinking cursor sits.
                                                        // Do not edit this directly.
                                                        // Call LOCATE to update the location and handle moving the cursor correctly. 

        public const int VECTOR_COP = 0xFFF4;           // 2-Byte interrupt handler
        //public const int VECTOR_BRK = 0xFFF6;         // 2-Byte interrupt handler
        public const int VECTOR_ABORT = 0xFFF8;         // 2-Byte interrupt handler
        public const int VECTOR_NMI = 0xFFFA;           // 2-Byte interrupt handler
        public const int VECTOR_RESET = 0xFFFC;         // 2-Byte interrupt handler
        public const int VECTOR_IRQ_BRK = 0xFFFE;       // 2-Byte interrupt handler

        #endregion
    }
}
