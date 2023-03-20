
using System.Collections.Generic;


namespace FoenixCore.Processor.mc6809
{
    public class OpcodeList10 : List<OpCode>
    {
        #region constants

        public const int LBRN_Relative = 0x21;
        public const int LBHI_Relative = 0x22;
        public const int LBLS_Relative = 0x23;
        public const int LBCC_Relative = 0x24;
        public const int LBCS_Relative = 0x25;
        public const int LBNE_Relative = 0x26;
        public const int LBEQ_Relative = 0x27;
        public const int LBVC_Relative = 0x28;
        public const int LBVS_Relative = 0x29;
        public const int LBPL_Relative = 0x2A;
        public const int LBMI_Relative = 0x2B;
        public const int LBGE_Relative = 0x2C;
        public const int LBLT_Relative = 0x2D;
        public const int LBGT_Relative = 0x2E;
        public const int LBLE_Relative = 0x2F;

        public const int SWI2_Inherent = 0x3F;

        public const int CMPD_Immediate = 0x83;
        public const int CMPY_Immediate = 0x8C;
        public const int LDY_Immediate = 0x8E;

        public const int CMPD_Direct = 0x93;
        public const int CMPY_Direct = 0x9C;
        public const int LDY_Direct = 0x9E;
        public const int STY_Direct = 0x9F;

        public const int CMPD_Indexed = 0xA3;
        public const int CMPY_Indexed = 0xAC;
        public const int LDY_Indexed = 0xAE;
        public const int STY_Indexed = 0xAF;

        public const int CMPD_Extended = 0xB3;
        public const int CMPY_Extended = 0xBC;
        public const int LDY_Extended = 0xBE;
        public const int STY_Extended = 0xBF;

        public const int LDS_Immediate = 0xCE;

        public const int LDS_Direct = 0xDE;
        public const int STS_Direct = 0xDF;

        public const int LDS_Indexed = 0xEE;
        public const int STS_Indexed = 0xEF;

        public const int LDS_Extended = 0xFE;
        public const int STS_Extended = 0xFF;

        #endregion constants

        public OpcodeList10(Operations operations, CentralProcessingUnit cpu)
        {
            Add(new OpCode(0x21, "LBRN", 4, AddressModes.Relative, new OpCode.ExecuteDelegate(operations.ExecuteFarBranch)));
            Add(new OpCode(0x22, "LBHI", 4, AddressModes.Relative, new OpCode.ExecuteDelegate(operations.ExecuteFarBranch)));
            Add(new OpCode(0x23, "LBLS", 4, AddressModes.Relative, new OpCode.ExecuteDelegate(operations.ExecuteFarBranch)));
            Add(new OpCode(0x24, "LBCC", 4, AddressModes.Relative, new OpCode.ExecuteDelegate(operations.ExecuteFarBranch)));
            Add(new OpCode(0x25, "LBCS", 4, AddressModes.Relative, new OpCode.ExecuteDelegate(operations.ExecuteFarBranch)));
            Add(new OpCode(0x26, "LBNE", 4, AddressModes.Relative, new OpCode.ExecuteDelegate(operations.ExecuteFarBranch)));
            Add(new OpCode(0x27, "LBEQ", 4, AddressModes.Relative, new OpCode.ExecuteDelegate(operations.ExecuteFarBranch)));
            Add(new OpCode(0x28, "LBVC", 4, AddressModes.Relative, new OpCode.ExecuteDelegate(operations.ExecuteFarBranch)));
            Add(new OpCode(0x29, "LBVS", 4, AddressModes.Relative, new OpCode.ExecuteDelegate(operations.ExecuteFarBranch)));
            Add(new OpCode(0x2A, "LBPL", 4, AddressModes.Relative, new OpCode.ExecuteDelegate(operations.ExecuteFarBranch)));
            Add(new OpCode(0x2B, "LBMI", 4, AddressModes.Relative, new OpCode.ExecuteDelegate(operations.ExecuteFarBranch)));
            Add(new OpCode(0x2C, "LBGE", 4, AddressModes.Relative, new OpCode.ExecuteDelegate(operations.ExecuteFarBranch)));
            Add(new OpCode(0x2D, "LBLT", 4, AddressModes.Relative, new OpCode.ExecuteDelegate(operations.ExecuteFarBranch)));
            Add(new OpCode(0x2E, "LBGT", 4, AddressModes.Relative, new OpCode.ExecuteDelegate(operations.ExecuteFarBranch)));
            Add(new OpCode(0x2F, "LBLE", 4, AddressModes.Relative, new OpCode.ExecuteDelegate(operations.ExecuteFarBranch)));

            Add(new OpCode(0x3F, "SWI2", 2, AddressModes.Inherent, new OpCode.ExecuteDelegate(operations.ExecuteMisc)));

            Add(new OpCode(0x83, "CMPD", 4, cpu.D, AddressModes.Immediate, new OpCode.ExecuteDelegate(operations.ExecuteMisc)));
            Add(new OpCode(0x8C, "CMPY", 4, cpu.X, AddressModes.Immediate, new OpCode.ExecuteDelegate(operations.ExecuteMisc)));
            Add(new OpCode(0x8E, "LDY", 4, cpu.X, AddressModes.Immediate, new OpCode.ExecuteDelegate(operations.ExecuteMisc)));

            Add(new OpCode(0x93, "CMPD", 3, cpu.D, AddressModes.Direct, new OpCode.ExecuteDelegate(operations.ExecuteMisc)));
            Add(new OpCode(0x9C, "CMPY", 3, cpu.X, AddressModes.Direct, new OpCode.ExecuteDelegate(operations.ExecuteMisc)));
            Add(new OpCode(0x9E, "LDY", 3, cpu.X, AddressModes.Direct, new OpCode.ExecuteDelegate(operations.ExecuteMisc)));
            Add(new OpCode(0x9F, "STY", 3, cpu.X, AddressModes.Direct, new OpCode.ExecuteDelegate(operations.ExecuteMisc)));

            Add(new OpCode(0xA3, "CMPD", 3, cpu.D, AddressModes.Indexed, new OpCode.ExecuteDelegate(operations.ExecuteMisc)));
            Add(new OpCode(0xAC, "CMPY", 3, cpu.X, AddressModes.Indexed, new OpCode.ExecuteDelegate(operations.ExecuteMisc)));
            Add(new OpCode(0xAE, "LDY", 3, cpu.X, AddressModes.Indexed, new OpCode.ExecuteDelegate(operations.ExecuteMisc)));
            Add(new OpCode(0xAF, "STY", 3, cpu.X, AddressModes.Indexed, new OpCode.ExecuteDelegate(operations.ExecuteMisc)));

            Add(new OpCode(0xB3, "CMPD", 4, cpu.D, AddressModes.Extended, new OpCode.ExecuteDelegate(operations.ExecuteMisc)));
            Add(new OpCode(0xBC, "CMPY", 4, cpu.X, AddressModes.Extended, new OpCode.ExecuteDelegate(operations.ExecuteMisc)));
            Add(new OpCode(0xBE, "LDY", 4, cpu.X, AddressModes.Extended, new OpCode.ExecuteDelegate(operations.ExecuteMisc)));
            Add(new OpCode(0xBF, "STY", 4, cpu.X, AddressModes.Extended, new OpCode.ExecuteDelegate(operations.ExecuteMisc)));

            Add(new OpCode(0xCE, "LDS", 4, cpu.U, AddressModes.Immediate, new OpCode.ExecuteDelegate(operations.ExecuteMisc)));

            Add(new OpCode(0xDE, "LDS", 3, cpu.U, AddressModes.Direct, new OpCode.ExecuteDelegate(operations.ExecuteMisc)));
            Add(new OpCode(0xDF, "STS", 3, cpu.U, AddressModes.Direct, new OpCode.ExecuteDelegate(operations.ExecuteMisc)));

            Add(new OpCode(0xEE, "LDS", 3, cpu.U, AddressModes.Indexed, new OpCode.ExecuteDelegate(operations.ExecuteMisc)));
            Add(new OpCode(0xEF, "STS", 3, cpu.U, AddressModes.Indexed, new OpCode.ExecuteDelegate(operations.ExecuteMisc)));

            Add(new OpCode(0xFE, "LDS", 4, cpu.U, AddressModes.Extended, new OpCode.ExecuteDelegate(operations.ExecuteMisc)));
            Add(new OpCode(0xFF, "STS", 4, cpu.U, AddressModes.Extended, new OpCode.ExecuteDelegate(operations.ExecuteMisc)));
        }
    }
}
