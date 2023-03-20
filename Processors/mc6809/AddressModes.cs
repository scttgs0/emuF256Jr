
namespace FoenixCore.Processor.mc6809
{
    public enum AddressModes
    {
        Inherent,               // MUL
        Immediate,              // LDA #$FF
        Extended,               // LDA CAT
        Direct,                 // LDA >CAT
        Indexed,                // LDA B,Y
        Relative,               // BRA CAT
        WORDOpcode
    }
}
