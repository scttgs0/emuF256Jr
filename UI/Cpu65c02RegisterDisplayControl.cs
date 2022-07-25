using System;
using System.Timers;

using Gtk;
using GUI = Gtk.Builder.ObjectAttribute;

using FoenixCore.Processor.wdc65c02;


namespace FoenixToolkit.UI
{
    public class Cpu65c02RegisterDisplayControl : Box
    {
        Timer refreshTimer = null;

        private Cairo.Color lightBlue = new(System.Drawing.Color.LightBlue.R / 255.0, System.Drawing.Color.LightBlue.G / 255.0, System.Drawing.Color.LightBlue.B / 255.0);

#pragma warning disable CS0649  // never assigned
        [GUI] Label bit0;
        [GUI] Label bit2;
        //[GUI] ListStore liststoreRegisters;
        //[GUI] TreeView registerGrid;
        [GUI] TreeViewColumn colRegister;
        [GUI] TreeViewColumn colSymbol;
        [GUI] TreeViewColumn colValue;
#pragma warning restore CS0649

        private CentralProcessingUnit _cpu;

        public Cpu65c02RegisterDisplayControl() : this(new Builder("Cpu65c02RegisterDisplayControl.ui"))
        {
             CellRendererText registerNameCell = new();
             colRegister.PackStart(registerNameCell, true);
             colRegister.AddAttribute(registerNameCell, "text", 0);

             CellRendererText registerSymbolCell = new();
             colSymbol.PackStart(registerSymbolCell, true);
             colSymbol.AddAttribute(registerSymbolCell, "text", 1);

             CellRendererText registerValueCell = new();
             registerValueCell.SetAlignment(1.0f, 0.5f);
             colValue.PackStart(registerValueCell, true);
             colValue.AddAttribute(registerValueCell, "text", 2);
        }

        private Cpu65c02RegisterDisplayControl(Builder builder) : base(builder.GetRawOwnedObject("Cpu65c02RegisterDisplayControl"))
        {
            builder.Autoconnect(this);

            bit0.ModifyBg(StateType.Normal, new Gdk.Color(0, 192, 44));
            bit2.ModifyBg(StateType.Normal, new Gdk.Color(0, 192, 64));
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
                //-- ucRegA.Register = _cpu.A;
                // ucRegX.Register = _cpu.X;
                // ucRegY.Register = _cpu.Y;
                // ucRegS.Register = _cpu.Stack;
                // ucRegDP.Register = _cpu.DirectPage;
                // ucRegFlags.Register = _cpu.Flags;
            }
        }

        public void UpdateRegisters()
        {
            //-- ucRegPC.Value = _cpu.PC.ToString("X6");

            // foreach (object c in boxRegisters.AllChildren)
            // {
            //     if (c is UI.RegisterControl rc)
            //         rc.UpdateValue();
            //     else if (c is UI.AccumulatorControl ac)
            //         ac.UpdateValue();
            // }
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
