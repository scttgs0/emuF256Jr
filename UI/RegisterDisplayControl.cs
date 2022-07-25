using System;
using System.Timers;

using Gtk;
using GUI = Gtk.Builder.ObjectAttribute;

using FoenixCore.Processor.wdc65c02;


namespace FoenixToolkit.UI
{
    public class RegisterDisplayControl : Box
    {
        Timer refreshTimer = null;

#pragma warning disable CS0649  // never assigned
        [GUI] Box boxRegisters;
        [GUI] RegisterControl ucRegA;
        [GUI] RegisterControl ucRegPC;
        [GUI] RegisterControl ucRegX;
        [GUI] RegisterControl ucRegY;
        [GUI] RegisterControl ucRegS;
        [GUI] RegisterControl ucRegDBR;
        [GUI] RegisterControl ucRegDP;
        [GUI] RegisterControl ucRegFlags;
#pragma warning restore CS0649

        private CentralProcessingUnit _cpu;

        public RegisterDisplayControl() : this(new Builder("RegisterDisplayControl.ui"))
        {
            ucRegPC = new() { Caption = "PC", Margin = 4 };
            boxRegisters.Add(ucRegPC);

            ucRegA = new() { Caption = "A", Margin = 4 };
            boxRegisters.Add(ucRegA);
            ucRegX = new() { Caption = "X", Margin = 4 };
            boxRegisters.Add(ucRegX);
            ucRegY = new() { Caption = "Y", Margin = 4 };
            boxRegisters.Add(ucRegY);

            ucRegS = new() { Caption = "S", Margin = 4 };
            boxRegisters.Add(ucRegS);
            ucRegDBR = new() { Caption = "DBR", Margin = 4 };
            boxRegisters.Add(ucRegDBR);
            ucRegDP = new() { Caption = "DP", Margin = 4 };
            boxRegisters.Add(ucRegDP);

            ucRegFlags = new() { Caption = "Flags", Margin = 4 };
            boxRegisters.Add(ucRegFlags);
        }

        private RegisterDisplayControl(Builder builder) : base(builder.GetRawOwnedObject("RegisterDisplayControl"))
        {
            builder.Autoconnect(this);
        }

        public CentralProcessingUnit CPU
        {
            get => _cpu;
            set
            {
                _cpu = value;
                SetRegisters();
            }
        }

        public bool AutoUpdate
        {
           get => refreshTimer != null && refreshTimer.Enabled;
           set
           {
                if (refreshTimer != null)
                   refreshTimer.Enabled = value;
           }
        }

        private void SetRegisters()
        {
            if (_cpu != null)
            {
                ucRegA.Register = _cpu.A;
                ucRegX.Register = _cpu.X;
                ucRegY.Register = _cpu.Y;
                ucRegS.Register = _cpu.Stack;
                ucRegFlags.Register = _cpu.Flags;
            }
        }

        public void UpdateRegisters()
        {
            ucRegPC.Value = _cpu.PC.ToString("X6");

            foreach (object c in boxRegisters.AllChildren)
            {
                if (c is UI.RegisterControl rc)
                    rc.UpdateValue();
            }
        }

        private void on_RegisterDisplayControl_map(object sender, EventArgs e)
        {
            refreshTimer = new()
            {
               Interval = 1000,
               Enabled = false,
               AutoReset = true
            };

            refreshTimer.Elapsed += on_refreshTimer_tick;
        }

        private void on_RegisterDisplayControl_unmap(object sender, EventArgs e)
        {
            if (refreshTimer != null)
            {
               refreshTimer.Stop();
               refreshTimer.Dispose();
               refreshTimer = null;
            }
        }

        private void on_refreshTimer_tick(object sender, ElapsedEventArgs e)
        {
            UpdateRegisters();
        }
    }
}
