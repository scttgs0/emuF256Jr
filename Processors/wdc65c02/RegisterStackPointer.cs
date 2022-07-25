namespace FoenixCore.Processor.wdc65c02
{
    public class RegisterStackPointer : Processor.Generic.Register16
    {
        public const int DefaultStackValue = 0x01ff;
        public int TopOfStack = DefaultStackValue;

        public int Value16
        {
            get => _value;
            set => _value = value;
        }

        public override void Reset()
        {
            base.Reset();

            TopOfStack = DefaultStackValue;
            Value = DefaultStackValue;
        }
    }
}
