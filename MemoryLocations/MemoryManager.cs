using FoenixCore.Simulator.Devices;


namespace FoenixCore.MemoryLocations
{
    /// <summary>
    /// Maps an address on the bus to a device or memory. GPU, RAM, and ROM are hard coded. Other I/O devices will be added 
    /// later.
    /// </summary>
    public class MemoryManager : IMappable
    {
        public const int MinAddress = 0x0_0000;
        public const int MaxAddress = 0xf_ffff;

        public MemoryRAM RAM = null;
        public MemoryRAM FLASH = null;
        public MemoryRAM VIDEO = null;
        public MemoryRAM VICKY = null;
        public MemoryRAM GABE = null;
        public MathCoproRegister MATH = null;
        public MathFloatRegister FLOAT = null;
        public CodecRAM CODEC = null;
        public KeyboardRegister KEYBOARD = null;
        public SDCardDevice SDCARD = null;
        public InterruptController INTERRUPT = null;
        public UART UART = null;
        public SID SID = null;
        public VDMA VDMA = null;
        public TimerRegister TIMER0 = null;
        public TimerRegister TIMER1 = null;

        public bool VectorPull = false;

        public int StartAddress => 0;
        public int Length => 0x10_0000;
        public int EndAddress => 0xF_FFFF;

        /// <summary>
        /// Determine whehter the address being read from or written to is an I/O device or a memory cell.
        /// If the location is an I/O device, return that device. Otherwise, return the memory being referenced.
        /// </summary>
        /// <param name="Address"></param>
        /// <param name="Device"></param>
        /// <param name="DeviceAddress"></param>
        public void GetDeviceAt(int Address, out IMappable Device, out int DeviceAddress)
        {
            if (Address == MemoryMap.CODEC.BASE)
            {
                Device = CODEC;
                DeviceAddress = 0;
                return;
            }

            if (Address >= MemoryMap.MATH.BASE && Address <= MemoryMap.MATH.BASE + 7)
            {
                Device = MATH;
                DeviceAddress = Address - MATH.StartAddress;
                return;
            }

            if (Address >= MemoryMap.INTR_CTRL.PENDING_L && Address <= MemoryMap.INTR_CTRL.PENDING_L + 3)
            {
                Device = INTERRUPT;
                DeviceAddress = Address - INTERRUPT.StartAddress;
                return;
            }

            if (Address >= MemoryMap.TIMER0.BASE && Address <= MemoryMap.TIMER0.BASE + 7)
            {
                Device = TIMER0;
                DeviceAddress = Address - TIMER0.StartAddress;
                return;
            }

            if (Address >= MemoryMap.TIMER1.BASE && Address <= MemoryMap.TIMER1.BASE + 7)
            {
                Device = TIMER1;
                DeviceAddress = Address - TIMER1.StartAddress;
                return;
            }

            if (Address >= RAM.StartAddress && Address <= RAM.StartAddress + RAM.Length - 1)
            {
                Device = RAM;
                DeviceAddress = Address - RAM.StartAddress;
                return;
            }

            if (Address >= KEYBOARD.StartAddress && Address <= KEYBOARD.EndAddress)
            {
                Device = KEYBOARD;
                DeviceAddress = Address - KEYBOARD.StartAddress;
                return;
            }

            if (Address >= MemoryMap.UART.BASE && Address < MemoryMap.UART.BASE + 8)
            {
                Device = UART;
                DeviceAddress = Address - MemoryMap.UART.BASE;
                return;
            }

            if (Address >= SDCARD.StartAddress && Address <= SDCARD.EndAddress)
            {
                Device = SDCARD;
                DeviceAddress = Address - SDCARD.StartAddress;
                return;
            }

            if (Address >= MemoryMap.FLOAT_START && Address <= MemoryMap.FLOAT_END)
            {
                Device = FLOAT;
                DeviceAddress = Address - FLOAT.StartAddress;
                return;
            }

            if (Address >= MemoryMap.VDMA_START && Address < MemoryMap.VDMA_START + MemoryMap.VDMA_SIZE)
            {
                Device = VDMA;
                DeviceAddress = Address - MemoryMap.VDMA_START;
                return;
            }

            if (Address >= MemoryMap.VICKY_START && Address <= MemoryMap.VICKY_END)
            {
                Device = VICKY;
                DeviceAddress = Address - VICKY.StartAddress;
                return;
            }

            if (Address >= MemoryMap.SID_S_BASE && Address <= MemoryMap.SID_S_BASE + 255)
            {
                Device = SID;
                DeviceAddress = Address - SID.StartAddress;
                return;
            }

            if (Address >= MemoryMap.GABE_START && Address <= MemoryMap.GABE_END)
            {
                Device = GABE;
                DeviceAddress = Address - GABE.StartAddress;
                return;
            }

            if (Address >= MemoryMap.VIDEO_START && Address < (MemoryMap.VIDEO_START + MemoryMap.VIDEO_SIZE))
            {
                Device = VIDEO;
                DeviceAddress = Address - VIDEO.StartAddress;
                return;
            }

            if (Address >= MemoryMap.FLASH_START && Address <= MemoryMap.FLASH_END)
            {
                Device = FLASH;
                DeviceAddress = Address - FLASH.StartAddress;
                return;
            }

            // oops, we didn't map this to anything. 
            Device = null;
            DeviceAddress = 0;
        }

        public virtual byte this[int Address]
        {
            get => ReadByte(Address);
            set => WriteByte(Address, value);
        }

        public virtual byte this[int Bank, int Address]
        {
            get => ReadByte(Bank * 0xffff + Address & 0xffff);
            set => WriteByte(Bank * 0xffff + Address & 0xffff, value);
        }

        /// <summary>
        /// Finds device mapped to 'Address' and calls it 
        /// 'Address' is offset by GetDeviceAt to device internal address range
        /// </summary>
        public virtual byte ReadByte(int Address)
        {
            GetDeviceAt(Address, out IMappable device, out int deviceAddress);

            if (device == null)
                return 0x40;

            return device.ReadByte(deviceAddress);
        }

        /// <summary>
        /// Reads a 16-bit word from memory
        /// </summary>
        /// <param name="Address"></param>
        /// <returns></returns>
        public ushort ReadWord(int Address)
        {
            GetDeviceAt(Address, out IMappable device, out int deviceAddress);

            if (device == null)
                return 0x4040;

            return (ushort)(device.ReadByte(deviceAddress) | (device.ReadByte(deviceAddress + 1) << 8));
        }

        public virtual void WriteByte(int Address, byte Value)
        {
            GetDeviceAt(Address, out IMappable device, out int deviceAddress);

            if (device != null)
                device.WriteByte(deviceAddress, Value);
        }

        public void WriteWord(int Address, int Value)
        {
            GetDeviceAt(Address, out IMappable device, out int deviceAddress);

            if (device != null)
            {
                device.WriteByte(deviceAddress, (byte)(Value & 0xff));
                device.WriteByte(deviceAddress + 1, (byte)(Value >> 8 & 0xff));
            }
        }

        public int Read(int Address, int Length)
        {
            GetDeviceAt(Address, out IMappable device, out int deviceAddress);

            int addr = deviceAddress;

            if (device == null)
                return 0x40;

            int ret = device.ReadByte(addr);

            if (Length >= 2)
                ret += device.ReadByte(addr + 1) << 8;

            if (Length >= 3)
                ret += device.ReadByte(addr + 2) << 16;

            return ret;
        }

        internal void Write(int Address, int Value, int Length)
        {
            GetDeviceAt(Address, out IMappable device, out int deviceAddress);

            if (device != null)
            {
                device.WriteByte(deviceAddress, (byte)(Value & 0xff));

                if (Length >= 2)
                    device.WriteByte(deviceAddress + 1, (byte)(Value >> 8 & 0xff));

                if (Length >= 3)
                    device.WriteByte(deviceAddress + 2, (byte)(Value >> 16 & 0xff));
            }
        }

        public void CopyFromBuffer(byte[] src, int srcAddress, int destAddress, int length)
        {
            GetDeviceAt(destAddress, out IMappable device, out int deviceAddress);

            if (device != null)
                device.CopyFromBuffer(src, srcAddress, deviceAddress, length);
        }

        public void CopyIntoBuffer(int srcAddress, int srcLength, byte[] buffer)
        {
            GetDeviceAt(srcAddress, out IMappable device, out int deviceAddress);

            if (device != null)
                device.CopyIntoBuffer(deviceAddress, srcLength, buffer);
        }
    }
}
