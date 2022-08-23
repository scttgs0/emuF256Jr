namespace FoenixCore.Simulator.Devices
{
    enum Register0
    {
        INT00_SOF       = 0x01,     // Start of Frame @ 60FPS
        INT01_SOL       = 0x02,     // Start of Line (Programmable)
        INT02_KBD       = 0x04,     // Keyboard
        INT03_MOUSE     = 0x08,     // Mouse
        INT04_TMR0      = 0x10,     // Timer 0
        INT05_TMR1      = 0x20,     // Timer 1
        INT06_DMA       = 0x40,     // DMA
        INT07_TBD       = 0x80      // unused
    }

    enum Register1
    {
        INT00_UART      = 0x01,     // Serial Port
        INT01_COL0      = 0x02,     // Collsion Detection
        INT02_COL1      = 0x04,     // Collsion Detection
        INT03_COL2      = 0x08,     // Collsion Detection
        INT04_RTC       = 0x10,     // Real-Time Clock
        INT05_VIA       = 0x20,     // MIDI Port
        INT06_IEC       = 0x40,     // Parallel Port
        INT07_SDCARD    = 0x80      // SDCard Insert
    }
}
