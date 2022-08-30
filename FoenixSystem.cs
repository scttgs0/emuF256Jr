using System.Collections.Generic;

using FoenixCore.MemoryLocations;
using FoenixCore.Simulator.Devices;
using FoenixCore.Simulator.FileFormat;


namespace FoenixCore
{
    public class FoenixSystem
    {
        public Processor.wdc65c02.CentralProcessingUnit CoreCpu = null;
        public MemoryManager MemMgr = null;

        public ResourceChecker Resources;
        public Processor.Generic.Breakpoints Breakpoints = new();
        public ListFile lstFile;
        private BoardVersion boardVersion;
        public SortedList<int, WatchedMemory> WatchList = new();
        private string LoadedKernel;

        public FoenixSystem(BoardVersion version, string DefaultKernel)
        {
            boardVersion = version;

            CodecRAM codec = null;
            SDCardDevice sdcard = null;
            byte SystemStat = 0; // FMX
            int keyboardAddress = MemoryMap.KBD_DATA_BUF_FMX; // FMX

            switch (boardVersion)
            {
                case BoardVersion.RevJr:
                    SystemStat = 1;
                    keyboardAddress = MemoryMap.KBD_DATA_BUF_U;
                    break;
            }

            codec = new CodecRAM(MemoryMap.CODEC.BASE, 2);      // This register is only a single byte but we allow writing a word
            sdcard = new GabeSDController(MemoryMap.GABE_SDC_CTRL_START, MemoryMap.GABE_SDC_CTRL_SIZE);

            MemMgr = new MemoryManager
            {
                RAM = new MemoryRAM(MemoryMap.RAM_START, MemoryMap.RAM_SIZE),                        // RAM: 2MB Rev B & U, 4MB Rev C & U+
                VICKY = new MemoryRAM(MemoryMap.VICKY_START, MemoryMap.VICKY_SIZE),       // 60K
                VIDEO = new MemoryRAM(MemoryMap.VIDEO_START, MemoryMap.VIDEO_SIZE),       // 4MB Video
                FLASH = new MemoryRAM(MemoryMap.FLASH_START, MemoryMap.FLASH_SIZE),       // 8MB RAM
                GABE = new GabeRAM(MemoryMap.GABE_START, MemoryMap.GABE_SIZE),            // 4K 

                // Special devices
                MATH = new MathCoproRegister(MemoryMap.MATH_START, MemoryMap.MATH_END - MemoryMap.MATH_START + 1), // 48 bytes
                KEYBOARD = new KeyboardRegister(keyboardAddress, 5),
                SDCARD = sdcard,
                INTERRUPT = new InterruptController(MemoryMap.INT_PENDING_REG0, 4),
                UART = new UART(MemoryMap.UART_REGISTERS, 8),
                OPL2 = new OPL2(MemoryMap.OPL2_S_BASE, 256),
                FLOAT = new MathFloatRegister(MemoryMap.FLOAT_START, MemoryMap.FLOAT_END - MemoryMap.FLOAT_START + 1),
                MPU401 = new MPU401(MemoryMap.MPU401_REGISTERS, 2),
                VDMA = new VDMA(MemoryMap.VDMA_START, MemoryMap.VDMA_SIZE),
                TIMER0 = new TimerRegister(MemoryMap.TIMER0_CTRL_REG, 8),
                TIMER1 = new TimerRegister(MemoryMap.TIMER1_CTRL_REG, 8)
            };

            MemMgr.CODEC = codec;
            MemMgr.KEYBOARD.SetKernel(this);

            // Assign memory variables used by other processes
            CoreCpu = new Processor.wdc65c02.CentralProcessingUnit(MemMgr);

            MemMgr.VDMA.SetVideoRam(MemMgr.VIDEO);
            MemMgr.VDMA.SetSystemRam(MemMgr.RAM);
            MemMgr.VDMA.SetVickyRam(MemMgr.VICKY);
            MemMgr.GABE.WriteByte(MemoryMap.GABE_SYS_STAT - MemoryMap.GABE_START, SystemStat);

            // Load the kernel.hex if present
            ResetCPU(DefaultKernel);

            // Write bytes $9F in the joystick registers to mean that they are not installed.
            MemMgr.WriteWord(0xAFE800, 0x9F9F);
            MemMgr.WriteWord(0xAFE802, 0x9F9F);

            MemMgr.TIMER0.TimerInterruptDelegate += TimerEvent0;
            MemMgr.TIMER1.TimerInterruptDelegate += TimerEvent1;

            // Set the Vicky rev and subrev
            MemMgr.VICKY.WriteWord(0x1C, 0x7654);
            MemMgr.VICKY.WriteWord(0x1E, 0x3456);
            MemMgr.VICKY.WriteByte(MemoryMap.GAMMA_CTRL_REG - MemoryMap.VICKY_BASE_ADDR, 0x11); // Gamma and hi-res are off

            // set the date
            // MemMgr.VICKY.WriteByte(MemoryMap.FPGA_DOR - MemoryMap.VICKY_BASE_ADDR, 0x1);
            // MemMgr.VICKY.WriteByte(MemoryMap.FPGA_MOR - MemoryMap.VICKY_BASE_ADDR, 0x2);
            // MemMgr.VICKY.WriteByte(MemoryMap.FPGA_YOR - MemoryMap.VICKY_BASE_ADDR, 0x21);

            // Set board revision
            MemMgr.GABE.WriteByte(MemoryMap.REVOFPCB_C - MemoryMap.GABE_START, (byte)'E');
            MemMgr.GABE.WriteByte(MemoryMap.REVOFPCB_4 - MemoryMap.GABE_START, (byte)'M');
            MemMgr.GABE.WriteByte(MemoryMap.REVOFPCB_A - MemoryMap.GABE_START, (byte)'U');
        }

        private void TimerEvent0()
        {
            byte mask = MemMgr.ReadByte(MemoryLocations.MemoryMap.INT_MASK_REG0);

            if (!CoreCpu.DebugPause && !CoreCpu.Flags.IrqDisable && ((~mask & (byte)Register0.INT04_TMR0) == (byte)Register0.INT04_TMR0))
            {
                // Set the Timer0 Interrupt
                byte IRQ0 = MemMgr.ReadByte(MemoryLocations.MemoryMap.INT_PENDING_REG0);
                IRQ0 |= (byte)Register0.INT04_TMR0;

                MemMgr.INTERRUPT.WriteFromGabe(0, IRQ0);

                CoreCpu.Pins.IRQ = true;
            }
        }

        private void TimerEvent1()
        {
            byte mask = MemMgr.ReadByte(MemoryLocations.MemoryMap.INT_MASK_REG0);

            if (!CoreCpu.DebugPause && !CoreCpu.Flags.IrqDisable && ((~mask & (byte)Register0.INT05_TMR1) == (byte)Register0.INT05_TMR1))
            {
                // Set the Timer1 Interrupt
                byte IRQ0 = MemMgr.ReadByte(MemoryLocations.MemoryMap.INT_PENDING_REG0);
                IRQ0 |= (byte)Register0.INT05_TMR1;

                MemMgr.INTERRUPT.WriteFromGabe(0, IRQ0);

                CoreCpu.Pins.IRQ = true;
            }
        }

        public BoardVersion GetVersion()
        {
            return boardVersion;
        }

        public void SetVersion(BoardVersion rev)
        {
            boardVersion = rev;
        }

        // return true if the CPU was reset and the program was loaded
        public bool ResetCPU(string kernelFilename)
        {
            CoreCpu.DebugPause = true;

            if (kernelFilename != null)
                LoadedKernel = kernelFilename;

            // If the reset vector is not set in Bank 0, but it is set in Bank 18, then copy bank 18 into bank 0.
            int BasePageAddress = 0x18_0000;

            if (LoadedKernel.EndsWith(".fnxml", true, null))
            {
                ResetMemory();

                FoenixXmlFile fnxml = new(this, Resources);
                fnxml.Load(LoadedKernel);

                boardVersion = fnxml.Version;
            }
            else
            {
                LoadedKernel = HexFile.Load(MemMgr.RAM, LoadedKernel, BasePageAddress, out _, out _);
                if (LoadedKernel == null)
                    return false;

                if (lstFile == null)
                    lstFile = new ListFile(LoadedKernel);
                else
                {
                    // TODO: This results in lines of code to be shown in incorrect order - Fix
                    ListFile tempList = new(LoadedKernel);

                    foreach (DebugLine line in tempList.Lines.Values)
                    {
                        if (lstFile.Lines.ContainsKey(line.PC))
                            lstFile.Lines.Remove(line.PC);

                        lstFile.Lines.Add(line.PC, line);

                        for (int i = 1; i < line.commandLength; ++i)
                            if (lstFile.Lines.ContainsKey(line.PC + i))
                                lstFile.Lines.Remove(line.PC + i);
                    }
                }
            }

            // See if lines of code exist in the 0x18_0000 to 0x18_FFFF block for RevB or 0x38_0000 to 0x38_FFFF block for Rev C
            List<DebugLine> copiedLines = new();

            if (lstFile.Lines.Count > 0)
            {
                foreach (DebugLine line in lstFile.Lines.Values)
                {
                    if (line != null && line.PC >= BasePageAddress && line.PC < BasePageAddress + 0x1_0000)
                    {
                        DebugLine dl = (DebugLine)line.Clone();
                        dl.PC -= BasePageAddress;
                        copiedLines.Add(dl);
                    }
                }
            }

            if (copiedLines.Count > 0)
            {
                foreach (DebugLine line in copiedLines)
                {
                    if (lstFile.Lines.ContainsKey(line.PC))
                        lstFile.Lines.Remove(line.PC);

                    lstFile.Lines.Add(line.PC, line);
                }
            }

            CoreCpu.Reset();

            // Reset the keyboard
            MemMgr.KEYBOARD.WriteByte(0, 0);
            MemMgr.KEYBOARD.WriteByte(4, 0);

            return true;
        }

        public void ResetMemory()
        {
            MemMgr.RAM.Zero();
            MemMgr.VICKY.Zero();
            MemMgr.VDMA.Zero();
        }
    }
}
