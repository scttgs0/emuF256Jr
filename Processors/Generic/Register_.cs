using System;


namespace FoenixCore.Processor.Generic
{
    public class Register
    {
        protected int _value;
        private int byteLength = 2;

        /// <summary>
        /// Forces the upper 8 bits to 0 when the register changes to 8 bit mode, or when writing or reading 
        /// the value in 8 bit mode. If this is false, the value is hidden, but preserved. If this is true, 
        /// the top 8 bits are destroyed when the width is set to 8 bits. 
        /// </summary>
        public bool DiscardUpper = true;

        public virtual int Value
        {
            get
            {
                return byteLength switch
                {
                    2 => _value & 0xffff,
                    1 => _value & 0xff,
                    _ => _value,
                };
            }

            set
            {
                switch(byteLength)
                {
                    case 2:
                        _value = ((int)(_value & 0xffff0000) & (value & 0xffff));
                        break;

                    case 1:
                        _value = ((int)(_value & 0xffffff00) & (value & 0xff));
                        break;

                    default:
                        _value = value;
                        break;
                }

                if (byteLength == 1)
                {
                    if (DiscardUpper)
                        _value = (int)(value & 0xff);
                    else
                        _value = (int)((value & 0xff) | (_value & 0xff00));
                }
                else
                    _value = value & 0xffff;
            }
        }

        public void Dec()
        {
            _value -= 1;
        }

        public void Inc()
        {
            _value += 1;
        }

        public virtual int Low
        {
            get => (int)(_value & 0xff);
            //set => this.Value = (int)((this.Value & 0xff00) | (value & 0xff));
        }

        /*
        public virtual int High
        {
            get => (int)((_value & 0xff00) >> 8);
            set => this.Value = (int)((this.Value & 0xff) | ((value & 0xff) << 8));
        }*/

        public virtual void Swap()
        {
            int v = _value;
            int low = (v & 0xFF) << 8;
            int high = (v & 0xFF00) >> 8;
            _value = high + low;
        }

        /// <summary>
        /// Register width in bytes. 1=8 bits, 2=16 bits
        /// </summary>
        public virtual int Width
        {
            get => byteLength;
            set => byteLength = value;
        }

        public virtual int MinUnsigned => 0;
        public virtual int MaxUnsigned => byteLength == 1 ? 0xff : 0xffff;
        public virtual int MinSigned => byteLength == 1 ? -128 : -32768;
        public virtual int MaxSigned => byteLength == 1 ? 127 : 32767;

        public virtual void SetFromVector(int v)
        {
            throw new NotImplementedException();
        }

        public virtual bool GetZeroFlag()
        {
            return Value == 0;
        }

        public override string ToString()
        {
            return byteLength switch
            {
                2 => "$" + Value.ToString("X4"),
                1 => "$" + Value.ToString("X2"),
                _ => Value.ToString(),
            };
        }

        /// <summary>
        /// Build a 24-bit address using this as a bank register
        /// </summary>
        /// <param name="Address"></param>
        /// <returns></returns>
        public virtual int GetLongAddress(int Address)
        {
            return byteLength switch
            {
                2 => (Value << 8) | Address,
                1 => (Value << 16) | Address,
                _ => Value,
            };
        }

        /// <summary>
        /// Build a 24-bit address using this as a bank register
        /// </summary>
        /// <param name="Address">Register to use as address</param>
        /// <returns></returns>
        public virtual int GetLongAddress(Register Address)
        {
            return byteLength switch
            {
                2 => (Value << 8) | Address.Value,
                1 => (Value << 16) | Address.Value,
                _ => Value,
            };
        }

        public virtual void Reset()
        {
            byteLength = 2;
        }
    }

    public class Register32 : Register
    {
        public Register32()
        {
            base.Width = 4;
        }

        public override int Width => 4;

        public override int Value
        {
            get => base.Value;
            set => _value = value;
        }

        public int Value8
        {
            get => base.Value & 0xff;
            set => base.Value = (int)((base.Value & 0xffffff00) & (value & 0xff));
        }

        public int Value16
        {
            get => base.Value & 0xffff;
            set => base.Value = (int)((base.Value & 0xffff0000) & (value & 0xffff));
        }

        public int Value32
        {
            get => (int)(base.Value & 0xffffffff);
            set => base.Value = (int)(value & 0xffffffff);
        }

    }

    /// <summary>
    /// A register that is always 16 bits, such as the Direct Page register
    /// </summary>
    public class Register16 : Register
    {
        public override int Width
        {
            get => 2;
            set => base.Width = 2;
        }

        /// <summary>
        /// Get a direct page address. Offsets the register's value by 8 bits, then adds 
        /// the supplied address.
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public int GetLongAdddress(int address)
        {
            return (Value << 8) | address;
        }

        /// <summary>
        /// Get a direct page address. Offsets the register's value by 8 bits, then adds 
        /// the supplied address.
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public int GetLongAdddress(Register index)
        {
            return (Value << 8) | index.Value;
        }
    }

    /// <summary>
    /// Defines a register that is always 16 bits, such as the program counter or the Direct Page register.
    /// </summary>
    /// 
    /// <summary>
    /// 
    /// Defines an 8 bit register.
    /// </summary>
    public class Register8 : Register
    {
        public Register8()
        {
            base.Width = 1;
        }

        public override int Width
        {
            get => 1;
            set => base.Width = 1;
        }
    }

    public class RegisterBankNumber : Register8
    {
        private int _LV = 0;
        /// <summary>
        /// Adds the 16-bit address in the register to this bank to get a 24-bit address
        /// </summary>
        /// <param name="Address"></param>
        /// <returns></returns>
        public virtual int GetLongAddress(Register16 Address)
        {
            return _LV | Address.Value;
        }

        /// <summary>
        /// Adds this bank register to a 16-bit address to form a 24-bit address
        /// </summary>
        /// <param name="Address"></param>
        /// <returns></returns>
        public override int GetLongAddress(int Address)
        {
            return _LV | Address;
        }

        public override int Value
        {
            get => base.Value;
            set
            {
                _value = value;
                _LV = value << 16;
            }
        }
    }

    /// <summary>
    /// A register that is always 16 bits, such as the Direct Page register
    /// </summary>
    public class RegisterZeroPage : Register
    {
        public override int Width
        {
            get => 2;
            set => base.Width = 2;
        }

        /// <summary>
        /// Get a direct page address. Offsets the register's value by 8 bits, then adds 
        /// the supplied address.
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public override int GetLongAddress(int address)
        {
            return Value + address;
        }
    }
}
