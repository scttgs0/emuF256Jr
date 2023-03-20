
using System.Collections.Generic;


namespace FoenixCore.Processor.mc6809
{
    public class OpcodeList11 : List<OpCode>
    {
        #region constants

        public const int SWI3_Inherent = 0x3F;

        public const int CMPU_Immediate = 0x83;
        public const int CMPS_Immediate = 0x8C;

        public const int CMPU_Direct = 0x93;
        public const int CMPS_Direct = 0x9C;

        public const int CMPU_Indexed = 0xA3;
        public const int CMPS_Indexed = 0xAC;

        public const int CMPU_Extended = 0xB3;
        public const int CMPS_Extended = 0xBC;

        #endregion constants

        public OpcodeList11(Operations operations, CentralProcessingUnit cpu)
        {
            Add(new OpCode(0x3F, "SWI3", 2, AddressModes.Inherent, new OpCode.ExecuteDelegate(operations.ExecuteMisc)));

            Add(new OpCode(0x83, "CMPU", 4, cpu.D, AddressModes.Immediate, new OpCode.ExecuteDelegate(operations.ExecuteMisc)));
            Add(new OpCode(0x8C, "CMPS", 4, cpu.X, AddressModes.Immediate, new OpCode.ExecuteDelegate(operations.ExecuteMisc)));

            Add(new OpCode(0x93, "CMPU", 3, cpu.D, AddressModes.Direct, new OpCode.ExecuteDelegate(operations.ExecuteMisc)));
            Add(new OpCode(0x9C, "CMPS", 3, cpu.X, AddressModes.Direct, new OpCode.ExecuteDelegate(operations.ExecuteSTZ)));

            Add(new OpCode(0xA3, "CMPU", 3, cpu.D, AddressModes.Indexed, new OpCode.ExecuteDelegate(operations.ExecuteMisc)));
            Add(new OpCode(0xAC, "CMPS", 3, cpu.X, AddressModes.Indexed, new OpCode.ExecuteDelegate(operations.ExecuteMisc)));

            Add(new OpCode(0xB3, "CMPU", 4, cpu.D, AddressModes.Extended, new OpCode.ExecuteDelegate(operations.ExecuteMisc)));
            Add(new OpCode(0xBC, "CMPS", 4, cpu.X, AddressModes.Extended, new OpCode.ExecuteDelegate(operations.ExecuteMisc)));
        }
    }
}
