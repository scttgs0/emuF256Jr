
using System;
using System.Timers;


namespace FoenixCore.Simulator.Devices
{
    public class TimerRegister : MemoryLocations.MemoryRAM
    {
        private readonly Timer _timer = null;

        public delegate void RaiseInterruptFunction();
        public RaiseInterruptFunction TimerInterruptDelegate;
        const int CPU_FREQ = 14318000;

        public TimerRegister(int StartAddress, int Length) : base(StartAddress, Length)
        {
            _timer = new Timer(1000);
            _timer.Elapsed += on_Timer_tick;
        }

        public override void WriteByte(int Address, byte Value)
        {
            // Address 0 is control register
            data[Address] = Value;

            if (Address == 0)
            {
                bool enabled = (Value & 1) != 0;
                if (enabled)
                    _timer.Start();
                else
                    _timer.Stop();
            }
            else if (Address > 4 && Address < 8)
            {
                // Calculate interval in milliseconds
                int longInterval = data[5] + (data[6] << 8) + (data[7] << 16);
                double msInterval = (double)(longInterval) * 1000 / (double)CPU_FREQ;
                uint adjInterval = (uint)msInterval;

                if (adjInterval == 0)
                    _timer.Interval = 1;
                else
                    _timer.Interval = adjInterval;
            }
        }

        void on_Timer_tick(object sender, ElapsedEventArgs e)
        {
            TimerInterruptDelegate?.Invoke();
        }
    }
}
