
using FoenixCore.Processor.GenericNew;


namespace FoenixCore.Processor.mc6809
{
    public class RegisterStackPointer : Register<ushort>
    {
        public const int DefaultStackValue = 0x01ff;
        public int TopOfStack = DefaultStackValue;

        public override ushort Value
        {
            get => _value;
            set => _value = value;
        }

        public void Reset()
        {
            TopOfStack = DefaultStackValue;
            Value = DefaultStackValue;
        }
    }
}
