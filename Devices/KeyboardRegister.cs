using System;

using FoenixIDE.Basic;


namespace FoenixCore.Simulator.Devices
{
    public class KeyboardRegister : MemoryLocations.MemoryRAM
    {
        private bool mouseDevice = false;
        private byte ps2PacketCntr = 0;
        private readonly byte[] ps2packet = new byte[3];
        private FoenixSystem _kernel = null;

        public KeyboardRegister(int StartAddress, int Length) : base(StartAddress, Length)
        {
        }

        public void SetKernel(FoenixSystem kernel)
        {
            _kernel = kernel;
        }

        // This is used to simulate the Keyboard Register
        public override void WriteByte(int Address, byte Value)
        {
            // In order to avoid an infinite loop, we write to the device directly
            data[Address] = Value;

            switch (Address)
            {
                case 0:
                    switch (Value)
                    {
                        case 0x69:
                            data[4] = 1;
                            break;

                        case 0xEE: // echo command
                            data[4] = 1;
                            break;

                        case 0xF4:
                            data[0] = 0xFA;
                            data[4] = 1;
                            break;

                        case 0xF6:
                            data[4] = 1;
                            break;
                    }
                    break;

                case 4:
                    switch (Value)
                    {
                        case 0x60:
                            break;

                        case 0xA9:
                            data[0] = 0;
                            break;

                        case 0xAA: // self test
                            data[0] = 0x55;
                            data[4] = 1;
                            break;

                        case 0xAB: // keyboard test
                            data[0] = 0;
                            data[4] = 1;
                            break;

                        case 0xAD: // disable keyboard
                            data[0] = 0;
                            data[1] = 0;
                            break;

                        case 0xAE: // re-enabled sending data
                            data[4] = 1;
                            break;

                        case 0xFF:  // reset 
                            data[4] = 0xAA;
                            break;

                        case 0x20:
                            data[4] = 1;
                            break;

                        case 0xD4:
                            data[4] = 1;
                            break;
                    }
                    break;
            }
        }

        public override byte ReadByte(int Address)
        {
            // Whenever the buffer is read, set the buffer to empty.
            if (Address == 0)
            {
                if (!mouseDevice)
                    data[4] = 0;
                else if (ps2PacketCntr != 0)
                {
                    // send the next byte in the packet
                    data[4] = ps2packet[ps2PacketCntr++];

                    if (ps2PacketCntr == 3)
                        ps2PacketCntr = 0;

                    // raise interrupt
                    TriggerMouseInterrupt();
                }

                return data[0];
            }
            else if (Address == 5)
                return 0;

            return data[Address];
        }

        public void WriteKey(ScanCode key)
        {
            // Check if the Keyboard interrupt is allowed
            byte mask = _kernel.MemMgr.ReadByte(MemoryLocations.MemoryMap.IRQ_CTRL.MASK);
            if ((~mask & (byte)Register0.INT02_KBD) == (byte)Register0.INT02_KBD)
            {
                _kernel.MemMgr.KEYBOARD.WriteByte(0, (byte)key);
                _kernel.MemMgr.KEYBOARD.WriteByte(4, 0);

                // Set the Keyboard Interrupt
                byte IRQ0 = _kernel.MemMgr.INTERRUPT.ReadByte(1);
                IRQ0 |= (byte)Register0.INT02_KBD;
                _kernel.MemMgr.INTERRUPT.WriteFromGabe(1, IRQ0);

                _kernel.CoreCpu.Pins.IRQ = true;
            }
        }

        public void MousePackets(byte buttons, byte X, byte Y)
        {
            mouseDevice = true;
            data[0] = buttons;
            ps2packet[0] = buttons;
            ps2packet[1] = X;
            ps2packet[2] = Y;
            ps2PacketCntr = 1;

            TriggerMouseInterrupt();
        }

        private void TriggerMouseInterrupt()
        {
            if (_kernel == null)
                throw new InvalidOperationException("Kernel is undefined");

            // Set the Mouse Interrupt
            byte IRQ0 = _kernel.MemMgr.ReadByte(MemoryLocations.MemoryMap.IRQ_CTRL.PENDING);
            IRQ0 |= (byte)Register0.INT03_MOUSE;
            _kernel.MemMgr.INTERRUPT.WriteFromGabe(0, IRQ0);
            _kernel.CoreCpu.Pins.IRQ = true;
        }
    }
}
