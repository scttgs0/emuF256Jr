using System;


namespace FoenixCore.Processor.GenericNew
{
    public class Register<T>
    {
        protected T _value;

        public virtual T Value
        {
            get => _value;
            set => _value = value;
        }

        public byte b
        {
            get => (byte) Convert.ChangeType(_value, typeof (byte));
            set {
                uint v = (uint) Convert.ChangeType(_value, typeof (uint));
                v &= 0xffffff00;
                v &= (uint)value & 0xff;

                _value = (T) Convert.ChangeType(v, typeof (T));
            }
        }

        public ushort w
        {
            get => (ushort) Convert.ChangeType(_value, typeof (ushort));
            set {
                uint v = (uint) Convert.ChangeType(_value, typeof (uint));
                v &= 0xffff0000;
                v &= (uint)value & 0xffff;

                _value = (T) Convert.ChangeType(v, typeof (T));
            }
        }
    }
}
