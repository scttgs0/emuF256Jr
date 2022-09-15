namespace FoenixCore.MemoryLocations
{
    public static partial class MemoryMap
    {
        public static class MATH
        {
            public const ushort BASE            = 0xDE00;

            public const ushort UMULT_OPERAND_A = 0xDE00;       // [word]
            public const ushort UMULT_OPERAND_B = 0xDE02;       // [word]

            public const ushort MULT_OPERAND_A  = 0xDE04;       // [word]
            public const ushort MULT_OPERAND_B  = 0xDE06;       // [word]
        }
    }
}
