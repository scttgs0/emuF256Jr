
using System.Collections.Generic;


namespace FoenixCore.Processor.mc6809
{
    public class OpcodeList : List<OpCode>
    {
        #region constants

        public const int NEG_Direct = 0x00;
        // NOP_Inherent = 0x01;
        // NOP_Inherent = 0x02;
        public const int COM_Direct = 0x03;
        public const int LSR_Direct = 0x04;
        // NOP_Inherent = 0x05;
        public const int ROR_Direct = 0x06;
        public const int ASR_Direct = 0x07;
        public const int ASL_Direct = 0x08;         // LSL
        public const int ROL_Direct = 0x09;
        public const int DEC_Direct = 0x0A;
        // NOP_Inherent = 0x0B;
        public const int INC_Direct = 0x0C;
        public const int TST_Direct = 0x0D;
        public const int JMP_Direct = 0x0E;
        public const int CLR_Direct = 0x0F;

        // NOP_Inherent = 0x10;
        // NOP_Inherent = 0x11;
        public const int NOP_Inherent = 0x12;
        public const int SYNC_Immediate = 0x13;
        // NOP_Inherent = 0x14;
        // NOP_Inherent = 0x15;
        public const int LBRA_Relative = 0x16;
        public const int LBSR_Relative = 0x17;
        // NOP_Inherent = 0x18;
        public const int DAA_Inherent = 0x19;
        public const int ORCC_Immediate = 0x1A;
        // NOP_Inherent = 0x1B;
        public const int ANDCC_Immediate = 0x1C;
        public const int SEX_Inherent = 0x1D;
        public const int EXG_Immediate = 0x1E;
        public const int TFR_Immediate = 0x1F;

        public const int BRA_Relative = 0x20;
        public const int BRN_Relative = 0x21;
        public const int BHI_Relative = 0x22;
        public const int BLS_Relative = 0x23;
        public const int BCC_Relative = 0x24;       // BHS
        public const int BCS_Relative = 0x25;       // BLO
        public const int BNE_Relative = 0x26;
        public const int BEQ_Relative = 0x27;
        public const int BVC_Relative = 0x28;
        public const int BVS_Relative = 0x29;
        public const int BPL_Relative = 0x2A;
        public const int BMI_Relative = 0x2B;
        public const int BGE_Relative = 0x2C;
        public const int BLT_Relative = 0x2D;
        public const int BGT_Relative = 0x2E;
        public const int BLE_Relative = 0x2F;

        public const int LEAX_Indexed = 0x30;
        public const int LEAY_Indexed = 0x31;
        public const int LEAS_Indexed = 0x32;
        public const int LEAU_Indexed = 0x33;
        public const int PSHS_Immediate = 0x34;
        public const int PULS_Immediate = 0x35;
        public const int PSHU_Immediate = 0x36;
        public const int PULU_Immediate = 0x37;
        // NOP_Inherent = 0x38;
        public const int RTS_Inherent = 0x39;
        public const int ABX_Inherent = 0x3A;
        public const int RTI_Inherent = 0x3B;
        public const int CWAI_Immediate = 0x3C;
        public const int MUL_Inherent = 0x3D;
        // NOP_Inherent = 0x3E;
        public const int SWI_Inherent = 0x3F;

        public const int NEGA_Inherent = 0x40;
        // NOP_Inherent = 0x41;
        // NOP_Inherent = 0x42;
        public const int COMA_Inherent = 0x43;
        public const int LSRA_Inherent = 0x44;
        // NOP_Inherent = 0x45;
        public const int RORA_Inherent = 0x46;
        public const int ASRA_Inherent = 0x47;
        public const int ASLA_Inherent = 0x48;      // LSLA
        public const int ROLA_Inherent = 0x49;
        public const int DECA_Inherent = 0x4A;
        // NOP_Inherent = 0x4B;
        public const int INCA_Inherent = 0x4C;
        public const int TSTA_Inherent = 0x4D;
        // NOP_Inherent = 0x4E;
        public const int CLRA_Inherent = 0x4F;

        public const int NEGB_Inherent = 0x50;
        // NOP_Inherent = 0x51;
        // NOP_Inherent = 0x52;
        public const int COMB_Inherent = 0x53;
        public const int LSRB_Inherent = 0x54;
        // NOP_Inherent = 0x55;
        public const int RORB_Inherent = 0x56;
        public const int ASRB_Inherent = 0x57;
        public const int ASLB_Inherent = 0x58;      // LSLB
        public const int ROLB_Inherent = 0x59;
        public const int DECB_Inherent = 0x5A;
        // NOP_Inherent = 0x5B;
        public const int INCB_Inherent = 0x5C;
        public const int TSTB_Inherent = 0x5D;
        // NOP_Inherent = 0x5E;
        public const int CLRB_Inherent = 0x5F;

        public const int NEG_Indexed = 0x60;
        // NOP_Inherent = 0x61;
        // NOP_Inherent = 0x62;
        public const int COM_Indexed = 0x63;
        public const int LSR_Indexed = 0x64;
        // NOP_Inherent = 0x65;
        public const int ROR_Indexed = 0x66;
        public const int ASR_Indexed = 0x67;
        public const int ASL_Indexed = 0x68;        // LSL
        public const int ROL_Indexed = 0x69;
        public const int DEC_Indexed = 0x6A;
        // NOP_Inherent = 0x6B;
        public const int INC_Indexed = 0x6C;
        public const int TST_Indexed = 0x6D;
        public const int JMP_Indexed = 0x6E;
        public const int CLR_Indexed = 0x6F;

        public const int NEG_Extended = 0x70;
        // NOP_Inherent = 0x71;
        // NOP_Inherent = 0x72;
        public const int COM_Extended = 0x73;
        public const int LSR_Extended = 0x74;
        // NOP_Inherent = 0x75;
        public const int ROR_Extended = 0x76;
        public const int ASR_Extended = 0x77;
        public const int ASL_Extended = 0x78;       // LSL
        public const int ROL_Extended = 0x79;
        public const int DEC_Extended = 0x7A;
        // NOP_Inherent = 0x7B;
        public const int INC_Extended = 0x7C;
        public const int TST_Extended = 0x7D;
        public const int JMP_Extended = 0x7E;
        public const int CLR_Extended = 0x7F;

        public const int SUBA_Immediate = 0x80;
        public const int CMPA_Immediate = 0x81;
        public const int SBCA_Immediate = 0x82;
        public const int SUBD_Immediate = 0x83;     // CMPD
        public const int ANDA_Immediate = 0x84;
        public const int BITA_Immediate = 0x85;
        public const int LDA_Immediate = 0x86;
        // NOP_Inherent = 0x87;
        public const int EORA_Immediate = 0x88;
        public const int ADCA_Immediate = 0x89;
        public const int ORA_Immediate = 0x8A;
        public const int ADDA_Immediate = 0x8B;
        public const int CMPX_Immediate = 0x8C;
        public const int BSR_Relative = 0x8D;
        public const int LDX_Immediate = 0x8E;
        // NOP_Inherent = 0x8F;

        public const int SUBA_Direct = 0x90;
        public const int CMPA_Direct = 0x91;
        public const int SBCA_Direct = 0x92;
        public const int SUBD_Direct = 0x93;        // CMPD
        public const int ANDA_Direct = 0x94;
        public const int BITA_Direct = 0x95;
        public const int LDA_Direct = 0x96;
        public const int STA_Direct = 0x97;
        public const int EORA_Direct = 0x98;
        public const int ADCA_Direct = 0x99;
        public const int ORA_Direct = 0x9A;
        public const int ADDA_Direct = 0x9B;
        public const int CMPX_Direct = 0x9C;
        public const int JSR_Direct = 0x9D;
        public const int LDX_Direct = 0x9E;
        public const int STX_Direct = 0x9F;

        public const int SUBA_Indexed = 0xA0;
        public const int CMPA_Indexed = 0xA1;
        public const int SBCA_Indexed = 0xA2;
        public const int SUBD_Indexed = 0xA3;       // CMPD
        public const int ANDA_Indexed = 0xA4;
        public const int BITA_Indexed = 0xA5;
        public const int LDA_Indexed = 0xA6;
        public const int STA_Indexed = 0xA7;
        public const int EORA_Indexed = 0xA8;
        public const int ADCA_Indexed = 0xA9;
        public const int ORA_Indexed = 0xAA;
        public const int ADDA_Indexed = 0xAB;
        public const int CMPX_Indexed = 0xAC;
        public const int JSR_Indexed = 0xAD;
        public const int LDX_Indexed = 0xAE;
        public const int STX_Indexed = 0xAF;

        public const int SUBA_Extended = 0xB0;
        public const int CMPA_Extended = 0xB1;
        public const int SBCA_Extended = 0xB2;
        public const int SUBD_Extended = 0xB3;      // CMPD
        public const int ANDA_Extended = 0xB4;
        public const int BITA_Extended = 0xB5;
        public const int LDA_Extended = 0xB6;
        public const int STA_Extended = 0xB7;
        public const int EORA_Extended = 0xB8;
        public const int ADCA_Extended = 0xB9;
        public const int ORA_Extended = 0xBA;
        public const int ADDA_Extended = 0xBB;
        public const int CMPX_Extended = 0xBC;
        public const int JSR_Extended = 0xBD;
        public const int LDX_Extended = 0xBE;
        public const int STX_Extended = 0xBF;

        public const int SUBB_Immediate = 0xC0;
        public const int CMPB_Immediate = 0xC1;
        public const int SBCB_Immediate = 0xC2;
        public const int ADDD_Immediate = 0xC3;
        public const int ANDB_Immediate = 0xC4;
        public const int BITB_Immediate = 0xC5;
        public const int LDB_Immediate = 0xC6;
        // NOP_Inherent = 0xC7;
        public const int EORB_Immediate = 0xC8;
        public const int ADCB_Immediate = 0xC9;
        public const int ORB_Immediate = 0xCA;
        public const int ADDB_Immediate = 0xCB;
        public const int LDD_Immediate = 0xCC;
        // NOP_Inherent = 0xCD;
        public const int LDU_Immediate = 0xCE;
        // NOP_Inherent = 0xCF;

        public const int SUBB_Direct = 0xD0;
        public const int CMPB_Direct = 0xD1;
        public const int SBCB_Direct = 0xD2;
        public const int ADDD_Direct = 0xD3;
        public const int ANDB_Direct = 0xD4;
        public const int BITB_Direct = 0xD5;
        public const int LDB_Direct = 0xD6;
        public const int STB_Direct = 0xD7;
        public const int EORB_Direct = 0xD8;
        public const int ADCB_Direct = 0xD9;
        public const int ORB_Direct = 0xDA;
        public const int ADDB_Direct = 0xDB;
        public const int LDD_Direct = 0xDC;
        public const int STD_Direct = 0xDD;
        public const int LDU_Direct = 0xDE;
        public const int STU_Direct = 0xDF;

        public const int SUBB_Indexed = 0xE0;
        public const int CMPB_Indexed = 0xE1;
        public const int SBCB_Indexed = 0xE2;
        public const int ADDD_Indexed = 0xE3;
        public const int ANDB_Indexed = 0xE4;
        public const int BITB_Indexed = 0xE5;
        public const int LDB_Indexed = 0xE6;
        public const int STB_Indexed = 0xE7;
        public const int EORB_Indexed = 0xE8;
        public const int ADCB_Indexed = 0xE9;
        public const int ORB_Indexed = 0xEA;
        public const int ADDB_Indexed = 0xEB;
        public const int LDD_Indexed = 0xEC;
        public const int STD_Indexed = 0xED;
        public const int LDU_Indexed = 0xEE;
        public const int STU_Indexed = 0xEF;

        public const int SUBB_Extended = 0xF0;
        public const int CMPB_Extended = 0xF1;
        public const int SBCB_Extended = 0xF2;
        public const int ADDD_Extended = 0xF3;
        public const int ANDB_Extended = 0xF4;
        public const int BITB_Extended = 0xF5;
        public const int LDB_Extended = 0xF6;
        public const int STB_Extended = 0xF7;
        public const int EORB_Extended = 0xF8;
        public const int ADCB_Extended = 0xF9;
        public const int ORB_Extended = 0xFA;
        public const int ADDB_Extended = 0xFB;
        public const int LDD_Extended = 0xFC;
        public const int STD_Extended = 0xFD;
        public const int LDU_Extended = 0xFE;
        public const int STU_Extended = 0xFF;

        #endregion constants

        public OpcodeList(Operations operations, CentralProcessingUnit cpu)
        {
            Add(new OpCode(0x00, "NEG", 2, AddressModes.Direct, new OpCode.ExecuteDelegate(operations.ExecuteInterrupt)));
            Add(new OpCode(0x03, "COM", 2, AddressModes.Direct, new OpCode.ExecuteDelegate(operations.ExecuteInterrupt)));
            Add(new OpCode(0x04, "LSR", 2, AddressModes.Direct, new OpCode.ExecuteDelegate(operations.ExecuteTSBTRB)));
            Add(new OpCode(0x06, "ROR", 2, AddressModes.Direct, new OpCode.ExecuteDelegate(operations.ExecuteShift)));
            Add(new OpCode(0x07, "ASR", 2, AddressModes.Direct, new OpCode.ExecuteDelegate(operations.ExecuteBranch)));
            Add(new OpCode(0x08, "ASL", 2, AddressModes.Direct, new OpCode.ExecuteDelegate(operations.ExecuteStack)));
            Add(new OpCode(0x09, "ROL", 2, AddressModes.Direct, new OpCode.ExecuteDelegate(operations.ExecuteORA)));
            Add(new OpCode(0x0A, "DEC", 2, AddressModes.Direct, new OpCode.ExecuteDelegate(operations.ExecuteShift)));
            Add(new OpCode(0x0C, "INC", 2, AddressModes.Direct, new OpCode.ExecuteDelegate(operations.ExecuteTSBTRB)));
            Add(new OpCode(0x0D, "TST", 2, AddressModes.Direct, new OpCode.ExecuteDelegate(operations.ExecuteORA)));
            Add(new OpCode(0x0E, "JMP", 2, AddressModes.Direct, new OpCode.ExecuteDelegate(operations.ExecuteJumpReturn)));
            Add(new OpCode(0x0F, "CLR", 2, AddressModes.Direct, new OpCode.ExecuteDelegate(operations.ExecuteMisc)));

            Add(new OpCode(0x12, "NOP", 1, AddressModes.Inherent, new OpCode.ExecuteDelegate(operations.ExecuteORA)));
            Add(new OpCode(0x13, "SYNC", 1, AddressModes.Immediate, new OpCode.ExecuteDelegate(operations.ExecuteORA)));
            Add(new OpCode(0x16, "LBRA", 3, AddressModes.Relative, new OpCode.ExecuteDelegate(operations.ExecuteShift)));
            Add(new OpCode(0x17, "LBSR", 3, AddressModes.Relative, new OpCode.ExecuteDelegate(operations.ExecuteBranch)));
            Add(new OpCode(0x19, "DAA", 1, AddressModes.Inherent, new OpCode.ExecuteDelegate(operations.ExecuteORA)));
            Add(new OpCode(0x1A, "ORCC", 2, AddressModes.Immediate, new OpCode.ExecuteDelegate(operations.ExecuteINCDEC)));
            Add(new OpCode(0x1C, "ANDCC", 2, AddressModes.Immediate, new OpCode.ExecuteDelegate(operations.ExecuteTSBTRB)));
            Add(new OpCode(0x1D, "SEX", 1, AddressModes.Inherent, new OpCode.ExecuteDelegate(operations.ExecuteORA)));
            Add(new OpCode(0x1E, "EXG", 2, AddressModes.Immediate, new OpCode.ExecuteDelegate(operations.ExecuteShift)));
            Add(new OpCode(0x1F, "TFR", 2, AddressModes.Immediate, new OpCode.ExecuteDelegate(operations.ExecuteMisc)));

            Add(new OpCode(0x20, "BRA", 2, AddressModes.Relative, new OpCode.ExecuteDelegate(operations.ExecuteBranch)));
            Add(new OpCode(0x21, "BRN", 2, AddressModes.Relative, new OpCode.ExecuteDelegate(operations.ExecuteMisc)));
            Add(new OpCode(0x22, "BHI", 2, AddressModes.Relative, new OpCode.ExecuteDelegate(operations.ExecuteBranch)));
            Add(new OpCode(0x23, "BLS", 2, AddressModes.Relative, new OpCode.ExecuteDelegate(operations.ExecuteBranch)));
            Add(new OpCode(0x24, "BCC", 2, AddressModes.Relative, new OpCode.ExecuteDelegate(operations.ExecuteBranch)));
            Add(new OpCode(0x25, "BCS", 2, AddressModes.Relative, new OpCode.ExecuteDelegate(operations.ExecuteBranch)));
            Add(new OpCode(0x26, "BNE", 2, AddressModes.Relative, new OpCode.ExecuteDelegate(operations.ExecuteBranch)));
            Add(new OpCode(0x27, "BEQ", 2, AddressModes.Relative, new OpCode.ExecuteDelegate(operations.ExecuteBranch)));
            Add(new OpCode(0x28, "BVC", 2, AddressModes.Relative, new OpCode.ExecuteDelegate(operations.ExecuteBranch)));
            Add(new OpCode(0x29, "BVS", 2, AddressModes.Relative, new OpCode.ExecuteDelegate(operations.ExecuteBranch)));
            Add(new OpCode(0x2A, "BPL", 2, AddressModes.Relative, new OpCode.ExecuteDelegate(operations.ExecuteBranch)));
            Add(new OpCode(0x2B, "BMI", 2, AddressModes.Relative, new OpCode.ExecuteDelegate(operations.ExecuteBranch)));
            Add(new OpCode(0x2C, "BGE", 2, AddressModes.Relative, new OpCode.ExecuteDelegate(operations.ExecuteBranch)));
            Add(new OpCode(0x2D, "BLT", 2, AddressModes.Relative, new OpCode.ExecuteDelegate(operations.ExecuteBranch)));
            Add(new OpCode(0x2E, "BGT", 2, AddressModes.Relative, new OpCode.ExecuteDelegate(operations.ExecuteBranch)));
            Add(new OpCode(0x2F, "BLE", 2, AddressModes.Relative, new OpCode.ExecuteDelegate(operations.ExecuteBranch)));

            Add(new OpCode(0x30, "LEAX", 2, cpu.X, AddressModes.Indexed, new OpCode.ExecuteDelegate(operations.ExecuteBranch)));
            Add(new OpCode(0x31, "LEAY", 2, cpu.Y, AddressModes.Indexed, new OpCode.ExecuteDelegate(operations.ExecuteAND)));
            Add(new OpCode(0x32, "LEAS", 2, cpu.S, AddressModes.Indexed, new OpCode.ExecuteDelegate(operations.ExecuteAND)));
            Add(new OpCode(0x33, "LEAU", 2, cpu.U, AddressModes.Indexed, new OpCode.ExecuteDelegate(operations.ExecuteAND)));
            Add(new OpCode(0x34, "PSHS", 2, cpu.S, AddressModes.Immediate, new OpCode.ExecuteDelegate(operations.ExecuteBIT)));
            Add(new OpCode(0x35, "PULS", 2, cpu.S, AddressModes.Immediate, new OpCode.ExecuteDelegate(operations.ExecuteAND)));
            Add(new OpCode(0x36, "PSHU", 2, cpu.U, AddressModes.Immediate, new OpCode.ExecuteDelegate(operations.ExecuteShift)));
            Add(new OpCode(0x37, "PULU", 2, cpu.U, AddressModes.Immediate, new OpCode.ExecuteDelegate(operations.ExecuteBranch)));
            Add(new OpCode(0x39, "RTS", 1, AddressModes.Inherent, new OpCode.ExecuteDelegate(operations.ExecuteJumpReturn)));
            Add(new OpCode(0x3A, "ABX", 1, AddressModes.Inherent, new OpCode.ExecuteDelegate(operations.ExecuteINCDEC)));
            Add(new OpCode(0x3B, "RTI", 1, AddressModes.Inherent, new OpCode.ExecuteDelegate(operations.ExecuteJumpReturn)));
            Add(new OpCode(0x3C, "CWAI", 2, AddressModes.Immediate, new OpCode.ExecuteDelegate(operations.ExecuteBIT)));
            Add(new OpCode(0x3D, "MUL", 1, AddressModes.Inherent, new OpCode.ExecuteDelegate(operations.ExecuteAND)));
            Add(new OpCode(0x3F, "SWI", 1, AddressModes.Inherent, new OpCode.ExecuteDelegate(operations.ExecuteMisc)));

            Add(new OpCode(0x40, "NEGA", 1, cpu.A, AddressModes.Inherent, new OpCode.ExecuteDelegate(operations.ExecuteJumpReturn)));
            Add(new OpCode(0x43, "COMA", 1, cpu.A, AddressModes.Inherent, new OpCode.ExecuteDelegate(operations.ExecuteEOR)));
            Add(new OpCode(0x44, "LSRA", 1, cpu.A, AddressModes.Inherent, new OpCode.ExecuteDelegate(operations.ExecuteEOR)));
            Add(new OpCode(0x46, "RORA", 1, cpu.A, AddressModes.Inherent, new OpCode.ExecuteDelegate(operations.ExecuteShift)));
            Add(new OpCode(0x47, "ASRA", 1, cpu.A, AddressModes.Inherent, new OpCode.ExecuteDelegate(operations.ExecuteBranch)));
            Add(new OpCode(0x48, "ASLA", 1, cpu.A, AddressModes.Inherent, new OpCode.ExecuteDelegate(operations.ExecuteStack)));
            Add(new OpCode(0x49, "ROLA", 1, cpu.A, AddressModes.Inherent, new OpCode.ExecuteDelegate(operations.ExecuteEOR)));
            Add(new OpCode(0x4A, "DECA", 1, cpu.A, AddressModes.Inherent, new OpCode.ExecuteDelegate(operations.ExecuteShift)));
            Add(new OpCode(0x4C, "INCA", 1, cpu.A, AddressModes.Inherent, new OpCode.ExecuteDelegate(operations.ExecuteJumpReturn)));
            Add(new OpCode(0x4D, "TSTA", 1, cpu.A, AddressModes.Inherent, new OpCode.ExecuteDelegate(operations.ExecuteEOR)));
            Add(new OpCode(0x4F, "CLRA", 1, cpu.A, AddressModes.Inherent, new OpCode.ExecuteDelegate(operations.ExecuteMisc)));

            Add(new OpCode(0x50, "NEGB", 1, cpu.B, AddressModes.Inherent, new OpCode.ExecuteDelegate(operations.ExecuteBranch)));
            Add(new OpCode(0x53, "COMB", 1, cpu.B, AddressModes.Inherent, new OpCode.ExecuteDelegate(operations.ExecuteEOR)));
            Add(new OpCode(0x54, "LSRB", 1, cpu.B, AddressModes.Inherent, new OpCode.ExecuteDelegate(operations.ExecuteEOR)));
            Add(new OpCode(0x56, "RORB", 1, cpu.B, AddressModes.Inherent, new OpCode.ExecuteDelegate(operations.ExecuteShift)));
            Add(new OpCode(0x57, "ASRB", 1, cpu.B, AddressModes.Inherent, new OpCode.ExecuteDelegate(operations.ExecuteBranch)));
            Add(new OpCode(0x58, "ASLB", 1, cpu.B, AddressModes.Inherent, new OpCode.ExecuteDelegate(operations.ExecuteStatusReg)));
            Add(new OpCode(0x59, "ROLB", 1, cpu.B, AddressModes.Inherent, new OpCode.ExecuteDelegate(operations.ExecuteEOR)));
            Add(new OpCode(0x5A, "DECB", 1, cpu.B, AddressModes.Inherent, new OpCode.ExecuteDelegate(operations.ExecuteStack)));
            Add(new OpCode(0x5C, "INCB", 1, cpu.B, AddressModes.Inherent, new OpCode.ExecuteDelegate(operations.ExecuteStack)));
            Add(new OpCode(0x5D, "TSTB", 1, cpu.B, AddressModes.Inherent, new OpCode.ExecuteDelegate(operations.ExecuteEOR)));
            Add(new OpCode(0x5F, "CLRB", 1, cpu.B, AddressModes.Inherent, new OpCode.ExecuteDelegate(operations.ExecuteMisc)));

            Add(new OpCode(0x60, "NEG", 2, AddressModes.Indexed, new OpCode.ExecuteDelegate(operations.ExecuteJumpReturn)));
            Add(new OpCode(0x63, "COM", 2, AddressModes.Indexed, new OpCode.ExecuteDelegate(operations.ExecuteADC)));
            Add(new OpCode(0x64, "LSR", 2, AddressModes.Indexed, new OpCode.ExecuteDelegate(operations.ExecuteSTZ)));
            Add(new OpCode(0x66, "ROR", 2, AddressModes.Indexed, new OpCode.ExecuteDelegate(operations.ExecuteShift)));
            Add(new OpCode(0x67, "ASR", 2, AddressModes.Indexed, new OpCode.ExecuteDelegate(operations.ExecuteBranch)));
            Add(new OpCode(0x68, "ASL", 2, AddressModes.Indexed, new OpCode.ExecuteDelegate(operations.ExecuteStack)));
            Add(new OpCode(0x69, "ROL", 2, AddressModes.Indexed, new OpCode.ExecuteDelegate(operations.ExecuteADC)));
            Add(new OpCode(0x6A, "DEC", 2, AddressModes.Indexed, new OpCode.ExecuteDelegate(operations.ExecuteShift)));
            Add(new OpCode(0x6C, "INC", 2, AddressModes.Indexed, new OpCode.ExecuteDelegate(operations.ExecuteJumpReturn)));
            Add(new OpCode(0x6D, "TST", 2, AddressModes.Indexed, new OpCode.ExecuteDelegate(operations.ExecuteADC)));
            Add(new OpCode(0x6E, "JMP", 2, AddressModes.Indexed, new OpCode.ExecuteDelegate(operations.ExecuteJumpReturn)));
            Add(new OpCode(0x6F, "CLR", 2, AddressModes.Indexed, new OpCode.ExecuteDelegate(operations.ExecuteMisc)));

            Add(new OpCode(0x70, "NEG", 3, AddressModes.Extended, new OpCode.ExecuteDelegate(operations.ExecuteBranch)));
            Add(new OpCode(0x73, "COM", 3, AddressModes.Extended, new OpCode.ExecuteDelegate(operations.ExecuteADC)));
            Add(new OpCode(0x74, "LSR", 3, AddressModes.Extended, new OpCode.ExecuteDelegate(operations.ExecuteSTZ)));
            Add(new OpCode(0x76, "ROR", 3, AddressModes.Extended, new OpCode.ExecuteDelegate(operations.ExecuteShift)));
            Add(new OpCode(0x77, "ASR", 3, AddressModes.Extended, new OpCode.ExecuteDelegate(operations.ExecuteBranch)));
            Add(new OpCode(0x78, "ASL", 3, AddressModes.Extended, new OpCode.ExecuteDelegate(operations.ExecuteStatusReg)));
            Add(new OpCode(0x79, "ROL", 3, AddressModes.Extended, new OpCode.ExecuteDelegate(operations.ExecuteADC)));
            Add(new OpCode(0x7A, "DEC", 3, AddressModes.Extended, new OpCode.ExecuteDelegate(operations.ExecuteStack)));
            Add(new OpCode(0x7C, "INC", 3, AddressModes.Extended, new OpCode.ExecuteDelegate(operations.ExecuteJumpReturn)));
            Add(new OpCode(0x7D, "TST", 3, AddressModes.Extended, new OpCode.ExecuteDelegate(operations.ExecuteADC)));
            Add(new OpCode(0x7E, "JMP", 3, AddressModes.Extended, new OpCode.ExecuteDelegate(operations.ExecuteJumpReturn)));
            Add(new OpCode(0x7F, "CLR", 3, AddressModes.Extended, new OpCode.ExecuteDelegate(operations.ExecuteMisc)));

            Add(new OpCode(0x80, "SUBA", 2, cpu.A, AddressModes.Immediate, new OpCode.ExecuteDelegate(operations.ExecuteBranch)));
            Add(new OpCode(0x81, "CMPA", 2, cpu.A, AddressModes.Immediate, new OpCode.ExecuteDelegate(operations.ExecuteSTA)));
            Add(new OpCode(0x82, "SBCA", 2, cpu.A, AddressModes.Immediate, new OpCode.ExecuteDelegate(operations.ExecuteSTA)));
            Add(new OpCode(0x83, "SUBD", 3, cpu.D, AddressModes.Immediate, new OpCode.ExecuteDelegate(operations.ExecuteSTA)));
            Add(new OpCode(0x84, "ANDA", 2, cpu.A, AddressModes.Immediate, new OpCode.ExecuteDelegate(operations.ExecuteSTY)));
            Add(new OpCode(0x85, "BITA", 2, cpu.A, AddressModes.Immediate, new OpCode.ExecuteDelegate(operations.ExecuteBIT)));
            Add(new OpCode(0x86, "LDA", 2, cpu.A, AddressModes.Immediate, new OpCode.ExecuteDelegate(operations.ExecuteSTX)));
            Add(new OpCode(0x88, "EORA", 2, cpu.A, AddressModes.Immediate, new OpCode.ExecuteDelegate(operations.ExecuteINCDEC)));
            Add(new OpCode(0x89, "ADCA", 2, cpu.A, AddressModes.Immediate, new OpCode.ExecuteDelegate(operations.ExecuteBIT)));
            Add(new OpCode(0x8A, "ORA", 2, cpu.A, AddressModes.Immediate, new OpCode.ExecuteDelegate(operations.ExecuteTransfer)));
            Add(new OpCode(0x8B, "ADDA", 2, cpu.A, AddressModes.Immediate, new OpCode.ExecuteDelegate(operations.ExecuteTransfer)));
            Add(new OpCode(0x8C, "CMPX", 3, cpu.X, AddressModes.Immediate, new OpCode.ExecuteDelegate(operations.ExecuteSTY)));
            Add(new OpCode(0x8D, "BSR", 2, AddressModes.Relative, new OpCode.ExecuteDelegate(operations.ExecuteBranch)));
            Add(new OpCode(0x8E, "LDX", 3, cpu.X, AddressModes.Immediate, new OpCode.ExecuteDelegate(operations.ExecuteSTX)));

            Add(new OpCode(0x90, "SUBA", 2, cpu.A, AddressModes.Direct, new OpCode.ExecuteDelegate(operations.ExecuteBranch)));
            Add(new OpCode(0x91, "CMPA", 2, cpu.A, AddressModes.Direct, new OpCode.ExecuteDelegate(operations.ExecuteSTA)));
            Add(new OpCode(0x92, "SBCA", 2, cpu.A, AddressModes.Direct, new OpCode.ExecuteDelegate(operations.ExecuteSTA)));
            Add(new OpCode(0x93, "SUBD", 2, cpu.D, AddressModes.Direct, new OpCode.ExecuteDelegate(operations.ExecuteSTA)));
            Add(new OpCode(0x94, "ANDA", 2, cpu.A, AddressModes.Direct, new OpCode.ExecuteDelegate(operations.ExecuteSTY)));
            Add(new OpCode(0x95, "BITA", 2, cpu.A, AddressModes.Direct, new OpCode.ExecuteDelegate(operations.ExecuteSTA)));
            Add(new OpCode(0x96, "LDA", 2, cpu.A, AddressModes.Direct, new OpCode.ExecuteDelegate(operations.ExecuteSTX)));
            Add(new OpCode(0x97, "STA", 2, cpu.A, AddressModes.Direct, new OpCode.ExecuteDelegate(operations.ExecuteMisc)));
            Add(new OpCode(0x98, "EORA", 2, cpu.A, AddressModes.Direct, new OpCode.ExecuteDelegate(operations.ExecuteTransfer)));
            Add(new OpCode(0x99, "ADCA", 2, cpu.A, AddressModes.Direct, new OpCode.ExecuteDelegate(operations.ExecuteSTA)));
            Add(new OpCode(0x9A, "ORA", 2, cpu.A, AddressModes.Direct, new OpCode.ExecuteDelegate(operations.ExecuteTransfer)));
            Add(new OpCode(0x9B, "ADDA", 2, cpu.A, AddressModes.Direct, new OpCode.ExecuteDelegate(operations.ExecuteTransfer)));
            Add(new OpCode(0x9C, "CMPX", 2, cpu.X, AddressModes.Direct, new OpCode.ExecuteDelegate(operations.ExecuteSTZ)));
            Add(new OpCode(0x9D, "JSR", 2, AddressModes.Direct, new OpCode.ExecuteDelegate(operations.ExecuteSTA)));
            Add(new OpCode(0x9E, "LDX", 2, cpu.X, AddressModes.Direct, new OpCode.ExecuteDelegate(operations.ExecuteSTZ)));
            Add(new OpCode(0x9F, "STX", 2, cpu.X, AddressModes.Direct, new OpCode.ExecuteDelegate(operations.ExecuteMisc)));

            Add(new OpCode(0xA0, "SUBA", 2, cpu.A, AddressModes.Indexed, new OpCode.ExecuteDelegate(operations.ExecuteLDY)));
            Add(new OpCode(0xA1, "CMPA", 2, cpu.A, AddressModes.Indexed, new OpCode.ExecuteDelegate(operations.ExecuteLDA)));
            Add(new OpCode(0xA2, "SBCA", 2, cpu.A, AddressModes.Indexed, new OpCode.ExecuteDelegate(operations.ExecuteLDX)));
            Add(new OpCode(0xA3, "SUBD", 2, cpu.D, AddressModes.Indexed, new OpCode.ExecuteDelegate(operations.ExecuteLDX)));
            Add(new OpCode(0xA4, "ANDA", 2, cpu.A, AddressModes.Indexed, new OpCode.ExecuteDelegate(operations.ExecuteLDY)));
            Add(new OpCode(0xA5, "BITA", 2, cpu.A, AddressModes.Indexed, new OpCode.ExecuteDelegate(operations.ExecuteLDA)));
            Add(new OpCode(0xA6, "LDA", 2, cpu.A, AddressModes.Indexed, new OpCode.ExecuteDelegate(operations.ExecuteLDX)));
            Add(new OpCode(0xA7, "STA", 2, cpu.A, AddressModes.Indexed, new OpCode.ExecuteDelegate(operations.ExecuteMisc)));
            Add(new OpCode(0xA8, "EORA", 2, cpu.A, AddressModes.Indexed, new OpCode.ExecuteDelegate(operations.ExecuteTransfer)));
            Add(new OpCode(0xA9, "ADCA", 2, cpu.A, AddressModes.Indexed, new OpCode.ExecuteDelegate(operations.ExecuteLDA)));
            Add(new OpCode(0xAA, "ORA", 2, cpu.A, AddressModes.Indexed, new OpCode.ExecuteDelegate(operations.ExecuteTransfer)));
            Add(new OpCode(0xAB, "ADDA", 2, cpu.A, AddressModes.Indexed, new OpCode.ExecuteDelegate(operations.ExecuteTransfer)));
            Add(new OpCode(0xAC, "CMPX", 2, cpu.X, AddressModes.Indexed, new OpCode.ExecuteDelegate(operations.ExecuteLDY)));
            Add(new OpCode(0xAD, "JSR", 2, AddressModes.Indexed, new OpCode.ExecuteDelegate(operations.ExecuteLDA)));
            Add(new OpCode(0xAE, "LDX", 2, cpu.X, AddressModes.Indexed, new OpCode.ExecuteDelegate(operations.ExecuteLDX)));
            Add(new OpCode(0xAF, "STX", 2, cpu.X, AddressModes.Indexed, new OpCode.ExecuteDelegate(operations.ExecuteMisc)));

            Add(new OpCode(0xB0, "SUBA", 3, cpu.A, AddressModes.Extended, new OpCode.ExecuteDelegate(operations.ExecuteLDY)));
            Add(new OpCode(0xB1, "CMPA", 3, cpu.A, AddressModes.Extended, new OpCode.ExecuteDelegate(operations.ExecuteLDA)));
            Add(new OpCode(0xB2, "SBCA", 3, cpu.A, AddressModes.Extended, new OpCode.ExecuteDelegate(operations.ExecuteLDX)));
            Add(new OpCode(0xB3, "SUBD", 3, cpu.D, AddressModes.Extended, new OpCode.ExecuteDelegate(operations.ExecuteLDX)));
            Add(new OpCode(0xB4, "ANDA", 3, cpu.A, AddressModes.Extended, new OpCode.ExecuteDelegate(operations.ExecuteLDY)));
            Add(new OpCode(0xB5, "BITA", 3, cpu.A, AddressModes.Extended, new OpCode.ExecuteDelegate(operations.ExecuteLDA)));
            Add(new OpCode(0xB6, "LDA", 3, cpu.A, AddressModes.Extended, new OpCode.ExecuteDelegate(operations.ExecuteLDX)));
            Add(new OpCode(0xB7, "STA", 3, cpu.A, AddressModes.Extended, new OpCode.ExecuteDelegate(operations.ExecuteMisc)));
            Add(new OpCode(0xB8, "EORA", 3, cpu.A, AddressModes.Extended, new OpCode.ExecuteDelegate(operations.ExecuteTransfer)));
            Add(new OpCode(0xB9, "ADCA", 3, cpu.A, AddressModes.Extended, new OpCode.ExecuteDelegate(operations.ExecuteLDA)));
            Add(new OpCode(0xBA, "ORA", 3, cpu.A, AddressModes.Extended, new OpCode.ExecuteDelegate(operations.ExecuteTransfer)));
            Add(new OpCode(0xBB, "ADDA", 3, cpu.A, AddressModes.Extended, new OpCode.ExecuteDelegate(operations.ExecuteTransfer)));
            Add(new OpCode(0xBC, "CMPX", 3, cpu.X, AddressModes.Extended, new OpCode.ExecuteDelegate(operations.ExecuteLDY)));
            Add(new OpCode(0xBD, "JSR", 3, AddressModes.Extended, new OpCode.ExecuteDelegate(operations.ExecuteLDA)));
            Add(new OpCode(0xBE, "LDX", 3, cpu.X, AddressModes.Extended, new OpCode.ExecuteDelegate(operations.ExecuteLDX)));
            Add(new OpCode(0xBF, "STX", 3, cpu.X, AddressModes.Extended, new OpCode.ExecuteDelegate(operations.ExecuteMisc)));

            Add(new OpCode(0xC0, "SUBB", 2, cpu.B, AddressModes.Immediate, new OpCode.ExecuteDelegate(operations.ExecuteBranch)));
            Add(new OpCode(0xC1, "CMPB", 2, cpu.B, AddressModes.Immediate, new OpCode.ExecuteDelegate(operations.ExecuteSTA)));
            Add(new OpCode(0xC2, "SBCB", 2, cpu.B, AddressModes.Immediate, new OpCode.ExecuteDelegate(operations.ExecuteSTA)));
            Add(new OpCode(0xC3, "ADDD", 3, cpu.D, AddressModes.Immediate, new OpCode.ExecuteDelegate(operations.ExecuteSTA)));
            Add(new OpCode(0xC4, "ANDB", 2, cpu.B, AddressModes.Immediate, new OpCode.ExecuteDelegate(operations.ExecuteSTY)));
            Add(new OpCode(0xC5, "BITB", 2, cpu.B, AddressModes.Immediate, new OpCode.ExecuteDelegate(operations.ExecuteBIT)));
            Add(new OpCode(0xC6, "LDB", 2, cpu.B, AddressModes.Immediate, new OpCode.ExecuteDelegate(operations.ExecuteSTX)));
            Add(new OpCode(0xC8, "EORB", 2, cpu.B, AddressModes.Immediate, new OpCode.ExecuteDelegate(operations.ExecuteINCDEC)));
            Add(new OpCode(0xC9, "ADCB", 2, cpu.B, AddressModes.Immediate, new OpCode.ExecuteDelegate(operations.ExecuteBIT)));
            Add(new OpCode(0xCA, "ORB", 2, cpu.B, AddressModes.Immediate, new OpCode.ExecuteDelegate(operations.ExecuteTransfer)));
            Add(new OpCode(0xCB, "ADDB", 2, cpu.B, AddressModes.Immediate, new OpCode.ExecuteDelegate(operations.ExecuteTransfer)));
            Add(new OpCode(0xCC, "LDD", 3, cpu.D, AddressModes.Immediate, new OpCode.ExecuteDelegate(operations.ExecuteSTY)));
            Add(new OpCode(0xCE, "LDU", 3, cpu.U, AddressModes.Immediate, new OpCode.ExecuteDelegate(operations.ExecuteSTX)));

            Add(new OpCode(0xD0, "SUBB", 2, cpu.B, AddressModes.Direct, new OpCode.ExecuteDelegate(operations.ExecuteBranch)));
            Add(new OpCode(0xD1, "CMPB", 2, cpu.B, AddressModes.Direct, new OpCode.ExecuteDelegate(operations.ExecuteSTA)));
            Add(new OpCode(0xD2, "SBCB", 2, cpu.B, AddressModes.Direct, new OpCode.ExecuteDelegate(operations.ExecuteSTA)));
            Add(new OpCode(0xD3, "ADDD", 2, cpu.D, AddressModes.Direct, new OpCode.ExecuteDelegate(operations.ExecuteSTA)));
            Add(new OpCode(0xD4, "ANDB", 2, cpu.B, AddressModes.Direct, new OpCode.ExecuteDelegate(operations.ExecuteSTY)));
            Add(new OpCode(0xD5, "BITB", 2, cpu.B, AddressModes.Direct, new OpCode.ExecuteDelegate(operations.ExecuteSTA)));
            Add(new OpCode(0xD6, "LDB", 2, cpu.B, AddressModes.Direct, new OpCode.ExecuteDelegate(operations.ExecuteSTX)));
            Add(new OpCode(0xD7, "STB", 2, cpu.B, AddressModes.Direct, new OpCode.ExecuteDelegate(operations.ExecuteMisc)));
            Add(new OpCode(0xD8, "EORB", 2, cpu.B, AddressModes.Direct, new OpCode.ExecuteDelegate(operations.ExecuteTransfer)));
            Add(new OpCode(0xD9, "ADCB", 2, cpu.B, AddressModes.Direct, new OpCode.ExecuteDelegate(operations.ExecuteSTA)));
            Add(new OpCode(0xDA, "ORB", 2, cpu.B, AddressModes.Direct, new OpCode.ExecuteDelegate(operations.ExecuteTransfer)));
            Add(new OpCode(0xDB, "ADDB", 2, cpu.B, AddressModes.Direct, new OpCode.ExecuteDelegate(operations.ExecuteTransfer)));
            Add(new OpCode(0xDC, "LDD", 2, cpu.D, AddressModes.Direct, new OpCode.ExecuteDelegate(operations.ExecuteSTZ)));
            Add(new OpCode(0xDD, "STD", 2, cpu.D, AddressModes.Direct, new OpCode.ExecuteDelegate(operations.ExecuteSTA)));
            Add(new OpCode(0xDE, "LDU", 2, cpu.U, AddressModes.Direct, new OpCode.ExecuteDelegate(operations.ExecuteSTZ)));
            Add(new OpCode(0xDF, "STU", 2, cpu.U, AddressModes.Direct, new OpCode.ExecuteDelegate(operations.ExecuteMisc)));

            Add(new OpCode(0xE0, "SUBB", 2, cpu.B, AddressModes.Indexed, new OpCode.ExecuteDelegate(operations.ExecuteBranch)));
            Add(new OpCode(0xE1, "CMPB", 2, cpu.B, AddressModes.Indexed, new OpCode.ExecuteDelegate(operations.ExecuteSTA)));
            Add(new OpCode(0xE2, "SBCB", 2, cpu.B, AddressModes.Indexed, new OpCode.ExecuteDelegate(operations.ExecuteSTA)));
            Add(new OpCode(0xE3, "ADDD", 2, cpu.D, AddressModes.Indexed, new OpCode.ExecuteDelegate(operations.ExecuteSTA)));
            Add(new OpCode(0xE4, "ANDB", 2, cpu.B, AddressModes.Indexed, new OpCode.ExecuteDelegate(operations.ExecuteSTY)));
            Add(new OpCode(0xE5, "BITB", 2, cpu.B, AddressModes.Indexed, new OpCode.ExecuteDelegate(operations.ExecuteSTA)));
            Add(new OpCode(0xE6, "LDB", 2, cpu.B, AddressModes.Indexed, new OpCode.ExecuteDelegate(operations.ExecuteSTX)));
            Add(new OpCode(0xE7, "STB", 2, cpu.B, AddressModes.Indexed, new OpCode.ExecuteDelegate(operations.ExecuteMisc)));
            Add(new OpCode(0xE8, "EORB", 2, cpu.B, AddressModes.Indexed, new OpCode.ExecuteDelegate(operations.ExecuteTransfer)));
            Add(new OpCode(0xE9, "ADCB", 2, cpu.B, AddressModes.Indexed, new OpCode.ExecuteDelegate(operations.ExecuteSTA)));
            Add(new OpCode(0xEA, "ORB", 2, cpu.B, AddressModes.Indexed, new OpCode.ExecuteDelegate(operations.ExecuteTransfer)));
            Add(new OpCode(0xEB, "ADDB", 2, cpu.B, AddressModes.Indexed, new OpCode.ExecuteDelegate(operations.ExecuteTransfer)));
            Add(new OpCode(0xEC, "LDD", 2, cpu.D, AddressModes.Indexed, new OpCode.ExecuteDelegate(operations.ExecuteSTZ)));
            Add(new OpCode(0xED, "STD", 2, cpu.D, AddressModes.Indexed, new OpCode.ExecuteDelegate(operations.ExecuteSTA)));
            Add(new OpCode(0xEE, "LDU", 2, cpu.U, AddressModes.Indexed, new OpCode.ExecuteDelegate(operations.ExecuteSTZ)));
            Add(new OpCode(0xEF, "STU", 2, cpu.U, AddressModes.Indexed, new OpCode.ExecuteDelegate(operations.ExecuteMisc)));

            Add(new OpCode(0xF0, "SUBB", 3, cpu.B, AddressModes.Extended, new OpCode.ExecuteDelegate(operations.ExecuteBranch)));
            Add(new OpCode(0xF1, "CMPB", 3, cpu.B, AddressModes.Extended, new OpCode.ExecuteDelegate(operations.ExecuteSTA)));
            Add(new OpCode(0xF2, "SBCB", 3, cpu.B, AddressModes.Extended, new OpCode.ExecuteDelegate(operations.ExecuteSTA)));
            Add(new OpCode(0xF3, "ADDD", 3, cpu.D, AddressModes.Extended, new OpCode.ExecuteDelegate(operations.ExecuteSTA)));
            Add(new OpCode(0xF4, "ANDB", 3, cpu.B, AddressModes.Extended, new OpCode.ExecuteDelegate(operations.ExecuteSTY)));
            Add(new OpCode(0xF5, "BITB", 3, cpu.B, AddressModes.Extended, new OpCode.ExecuteDelegate(operations.ExecuteSTA)));
            Add(new OpCode(0xF6, "LDB", 3, cpu.B, AddressModes.Extended, new OpCode.ExecuteDelegate(operations.ExecuteSTX)));
            Add(new OpCode(0xF7, "STB", 3, cpu.B, AddressModes.Extended, new OpCode.ExecuteDelegate(operations.ExecuteMisc)));
            Add(new OpCode(0xF8, "EORB", 3, cpu.B, AddressModes.Extended, new OpCode.ExecuteDelegate(operations.ExecuteTransfer)));
            Add(new OpCode(0xF9, "ADCB", 3, cpu.B, AddressModes.Extended, new OpCode.ExecuteDelegate(operations.ExecuteSTA)));
            Add(new OpCode(0xFA, "ORB", 3, cpu.B, AddressModes.Extended, new OpCode.ExecuteDelegate(operations.ExecuteTransfer)));
            Add(new OpCode(0xFB, "ADDB", 3, cpu.B, AddressModes.Extended, new OpCode.ExecuteDelegate(operations.ExecuteTransfer)));
            Add(new OpCode(0xFC, "LDD", 3, cpu.D, AddressModes.Extended, new OpCode.ExecuteDelegate(operations.ExecuteSTZ)));
            Add(new OpCode(0xFD, "STD", 3, cpu.D, AddressModes.Extended, new OpCode.ExecuteDelegate(operations.ExecuteSTA)));
            Add(new OpCode(0xFE, "LDU", 3, cpu.U, AddressModes.Extended, new OpCode.ExecuteDelegate(operations.ExecuteSTZ)));
            Add(new OpCode(0xFF, "STU", 3, cpu.U, AddressModes.Extended, new OpCode.ExecuteDelegate(operations.ExecuteMisc)));
        }
    }
}
