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

        public short w
        {
            get => (short) Convert.ChangeType(_value, typeof (short));
            set {
                uint v = (uint) Convert.ChangeType(_value, typeof (uint));
                v &= 0xffff0000;
                v &= (uint)value & 0xffff;

                _value = (T) Convert.ChangeType(v, typeof (T));
            }
        }

        public int l
        {
            get => (int) Convert.ChangeType(_value, typeof (int));
            set {
                _value = (T) Convert.ChangeType(value, typeof (T));
            }
        }
    }
}
