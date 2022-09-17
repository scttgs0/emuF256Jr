namespace FoenixCore.MemoryLocations
{
    public static partial class MemoryMap
    {
        public static class MATH
        {
            public const ushort BASE                = 0xDE00;

            public const ushort UMULT_OPERAND_A     = 0xDE00;       // [word]
            public const ushort UMULT_OPERAND_B     = 0xDE02;       // [word]
            public const ushort UMULT_RESULT        = 0xDE04;       // [dword]  read-only

            public const ushort MULT_OPERAND_A      = 0xDE08;       // [word]
            public const ushort MULT_OPERAND_B      = 0xDE0A;       // [word]
            public const ushort MULT_RESULT         = 0xDE0C;       // [dword]  read-only

            public const ushort UDIV_DENOMINATOR    = 0xDE10;       // [word]
            public const ushort UDIV_NUMERATOR      = 0xDE12;       // [word]
            public const ushort UDIV_QUOTIENT       = 0xDE14;       // [word]  read-only
            public const ushort UDIV_REMAINDER      = 0xDE16;       // [word]  read-only

            public const ushort DIV_DENOMINATOR     = 0xDE18;       // [word]
            public const ushort DIV_NUMERATOR       = 0xDE1A;       // [word]
            public const ushort DIV_QUOTIENT        = 0xDE1C;       // [word]  read-only
            public const ushort DIV_REMAINDER       = 0xDE1E;       // [word]  read-only
        }
    }
}
