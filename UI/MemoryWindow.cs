using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Timers;

using Gtk;
using GUI = Gtk.Builder.ObjectAttribute;

using FoenixCore.MemoryLocations;


namespace FoenixToolkit.UI
{
    class MemoryWindow : Window
    {
        Timer _updateDisplayTimer = null;

        public static MemoryWindow Instance = null;
        public IMappable Memory = null;
        Point mem = new(-1, -1);
        public int StartAddress = 0x00;
        public int EndAddress = 0xFF;
        const int PageSize = 0xFF;
        public delegate void ButtonClicked(bool value);
        public ButtonClicked SetGamma;
        public ButtonClicked SetHiRes;

#pragma warning disable CS0649  // never assigned
        [GUI] ActionBar pnlFooter;
        [GUI] Button btnBack;
        [GUI] Button btnForward;
        [GUI] Button btnRefresh;
        [GUI] Button btnSave;
        [GUI] ComboBoxText cboAddress;
        [GUI] Entry txtStartAddress;
        [GUI] Entry txtEndAddress;
        [GUI] Entry HighlightPanel;
        [GUI] Fixed fixMain;
        [GUI] Label lblPosition;
        //--[GUI] TextBuffer textbufferMemory;
        [GUI] TextView tvwMemory;
        [GUI] ToggleButton btnMCRBit0;
        [GUI] ToggleButton btnMCRBit1;
        [GUI] ToggleButton btnMCRBit2;
        [GUI] ToggleButton btnMCRBit3;
        [GUI] ToggleButton btnMCRBit4;
        [GUI] ToggleButton btnMCRBit5;
        [GUI] ToggleButton btnMCRBit6;
        [GUI] ToggleButton btnMCRBit7;
        [GUI] ToggleButton btnMCRBit8;
        [GUI] ToggleButton btnMCRBit9;

#pragma warning restore CS0649

        public MemoryWindow() : this(new Builder("MemoryWindow.ui")) { }

        private MemoryWindow(Builder builder) : base(builder.GetRawOwnedObject("MemoryWindow"))
        {
            builder.Autoconnect(this);
        }

        public void AllowSave()
        {
            btnSave.Visible = true;
        }

        public void GotoAddress(int requestedAddress)
        {
            if (Memory is MemoryRAM)
            {
                int newAddress = requestedAddress - Memory.StartAddress;
                if (newAddress >= 0 && (newAddress) < Memory.Length)
                {
                    StartAddress = newAddress;

                    EndAddress = newAddress + PageSize;
                    if (EndAddress > Memory.Length)
                        EndAddress = Memory.Length;
                }

                txtStartAddress.Text = (StartAddress + Memory.StartAddress).ToString("X6");
                txtEndAddress.Text = (EndAddress + Memory.StartAddress).ToString("X6");
            }
            else
            {
                StartAddress = requestedAddress;
                EndAddress = requestedAddress + PageSize;
                txtStartAddress.Text = requestedAddress.ToString("X6");
                txtEndAddress.Text = EndAddress.ToString("X6");
            }

            HighlightPanel.Visible = false;
            lblPosition.Text = "";

            RefreshMemoryView();

            if (!(Memory is MemoryRAM) && txtStartAddress.Text.StartsWith("AF00"))
                UpdateMCRButtons();
        }

        public void RefreshMemoryView()
        {
            StringBuilder s = new();

            if (Memory == null)
                return;

            tvwMemory.Buffer.Clear();

            // Display 16 bytes per line
            for (int i = StartAddress; i < EndAddress; i += 0x10)
            {
                s.Append(">");

                if (Memory is MemoryRAM) {
                    int bank = ((i + Memory.StartAddress) & 0xFF0000) >> 16;
                    int addr = (i + Memory.StartAddress) & 0xFFFF;
                    s.Append(bank.ToString("X2") + ":" + addr.ToString("X4"));
                }
                else {
                    int bank = (i & 0xFF0000) >> 16;
                    int addr = i  & 0xFFFF;
                    s.Append(bank.ToString("X2") + ":" + addr.ToString("X4"));
                }

                s.Append("  ");

                StringBuilder text = new();

                for (int j = 0; j < 16; ++j)
                {
                    if (i + j < Memory.Length)
                    {
                        int c = Memory.ReadByte(i + j);
                        s.Append(c.ToString("X2"));

                        // Character data
                        if (c < 32 || c > 127)
                            text.Append(".");
                        else
                            text.Append((char)c);
                    }
                    else
                    {
                        s.Append("--");
                        text.Append("-");
                    }

                    s.Append(" ");

                    // Group 8 bytes together
                    if (j == 7 || j == 15)
                    {
                        s.Append(" ");
                        text.Append(" ");
                    }
                }

                s.Append(text);

                if ((i - StartAddress) < 256)
                    s.AppendLine();
            }

            tvwMemory.Buffer.Text = s.ToString();
        }

        private void FindMatchedDropDownEntry(int address)
        { 
            // find the address in the dropdown list
            bool matched = false;
            string defaultVal = "FFFFFF";

            TreeIter iter;
			ListStore store = (ListStore) cboAddress.Model;
			store.IterChildren(out iter);

            do {
                string item = store.GetValue(iter, 1).ToString();

                int dropdownAddress = 0;
                try {
                    dropdownAddress = Convert.ToInt32(item, 16);
                }
                catch (System.FormatException) {
                    return;
                }

                if (((address & 0xFF0000) == 0xAF0000) && ((address & 0xFFFF) != 0x0000))
                    defaultVal = "AF0000";  // continue to look for a more specific region
                else if ((dropdownAddress == address) || (dropdownAddress == (address & 0xFF0000)))
                {
                    cboAddress.ActiveId = item;
                    matched = true;
                }
            } while (!matched && store.IterNext(ref iter));

            if (!matched)
                cboAddress.ActiveId = defaultVal;
        }

        public void UpdateMCRButtons()
        {
            byte value = Memory.ReadByte(0x0000);

            // Determine if Gamma was changed, if so, toggle the dip switch
            bool oldGamma = btnMCRBit6.Active;
            bool newGamma = (value & 0x40) != 0;

            btnMCRBit7.Active = (value & 0x80) != 0;
            btnMCRBit6.Active = newGamma;

            if (oldGamma != newGamma)
                SetGamma?.Invoke(newGamma);

            btnMCRBit5.Active = (value & 0x20) != 0;
            btnMCRBit4.Active = (value & 0x10) != 0;
            btnMCRBit3.Active = (value & 0x08) != 0;
            btnMCRBit2.Active = (value & 0x04) != 0;
            btnMCRBit1.Active = (value & 0x02) != 0;
            btnMCRBit0.Active = (value & 0x01) != 0;

            // High-res and double-pixels
            value = Memory.ReadByte(0x0001);    // TODO: wrong address

            // Determine if the Hi-Res was changed, if so toggle the dip switch
            bool oldHires = btnMCRBit8.Active;
            bool newHires = (value & 0x01) != 0;

            btnMCRBit8.Active = newHires;

            if (oldHires != newHires)
                SetHiRes?.Invoke(newHires);

            btnMCRBit9.Active = (value & 0x02) != 0;
        }

        // Retrieve the memory location of the mouse location
        private void GetAddressPosition(Point mouse)
        {
            int line = mouse.Y / 17;
            int col = -1;
            int colWidth = 24;
            int addr = -1;
            int value = 0;
            int offset = 0;

            if (mouse.X > 75 && mouse.X < 267)
            {
                col = 1 + (mouse.X - 75) / colWidth;
                offset = col - 1;
            }

            if (mouse.X > 275 && mouse.X < 467)
            {
                col = 10 + (mouse.X - 275) / colWidth;
                offset = col - 2;
            }

            if (line < 16 && col != -1)
            {
                // Determine the address
                addr = 0;
                if (!string.IsNullOrEmpty(txtStartAddress.Text))
                    addr = Convert.ToInt32(txtStartAddress.Text, 16);

                addr += line * 16 + offset;

                if (Memory != null) {
                    if (Memory is MemoryRAM)
                    {
                        if (addr - Memory.StartAddress < Memory.Length)
                            value = Memory.ReadByte(addr - Memory.StartAddress);
                        else
                            value = -1;
                    }
                    else
                    {
                        value = Memory.ReadByte(addr);
                    }
                }

                if (value != -1)
                {
                    int left = col < 10 ? (col - 1) * colWidth + 84 : (col - 10) * colWidth + 284;
                    int top = line * 17 + 52;
                    fixMain.Move(HighlightPanel, left, top);

                    HighlightPanel.Text = value.ToString("X2");
                    HighlightPanel.Visible = true;
                }
                else
                {
                    HighlightPanel.Visible = false;
                }
            }
            else
            {
                HighlightPanel.Visible = false;
            }

            mem.X = addr;
            mem.Y = value;
        }

        private void on_MemoryWindow_map(object sender, EventArgs e)
        {
            // Set the MCR
            btnMCRBit0.Active = false;
            btnMCRBit0.Active = false;
            btnMCRBit1.Active = false;
            btnMCRBit2.Active = false;
            btnMCRBit3.Active = false;
            btnMCRBit4.Active = false;
            btnMCRBit5.Active = false;
            btnMCRBit6.Active = false;
            btnMCRBit7.Active = false;
            btnMCRBit8.Active = false;
            btnMCRBit9.Active = false;

            txtStartAddress.Text = StartAddress.ToString("X6");
            txtEndAddress.Text = EndAddress.ToString("X6");

#pragma warning disable CS0612  // deprecated warning
            HighlightPanel.ModifyFg(StateType.Normal, new Gdk.Color(255, 255, 255));
            HighlightPanel.ModifyBg(StateType.Normal, new Gdk.Color(0, 0, 192));

            _updateDisplayTimer = new()
            {
               Interval = 10000,
               Enabled = false,
               AutoReset = true
            };

            _updateDisplayTimer.Elapsed += on_UpdateDisplayTimer_tick;

            // Set the Address to Bank $00
            if (Memory is MemoryRAM)
            {
                cboAddress.RemoveAll();
                cboAddress.AppendText("Custom Memory " + Memory.StartAddress.ToString("X6"));
                cboAddress.CanFocus = false;

                HighlightPanel.IsEditable = false;
                HighlightPanel.ModifyBg(StateType.Normal, new Gdk.Color(255, 0, 0));
                HighlightPanel.Visible = true;

                pnlFooter.Visible = false;
            }
            else
            {
                cboAddress.ActiveId = "000000";

                HighlightPanel.IsEditable = true;

                _updateDisplayTimer.Enabled = true;
            }
#pragma warning restore CS0612
        }

        private void on_MemoryWindow_unmap(object sender, EventArgs e)
        {
            if (_updateDisplayTimer != null)
            {
               _updateDisplayTimer.Stop();
               _updateDisplayTimer.Dispose();
               _updateDisplayTimer = null;
            }
        }

        private void on_MemoryWindow_key_release_event(object sender, KeyReleaseEventArgs e)
        {
            if (e.Event.Key == Gdk.Key.Page_Down)
                btnForward.Activate();
            else if (e.Event.Key == Gdk.Key.Page_Up)
                btnBack.Activate();
        }

        private void on_btnRefresh_clicked(object sender, EventArgs e)
        {
            RefreshMemoryView();
        }

        private void on_btnBack_clicked(object sender, EventArgs e)
        {
            int desiredStart = Convert.ToInt32(txtStartAddress.Text, 16) - 256;
            if (desiredStart >= 0)
            {
                GotoAddress(desiredStart);
                FindMatchedDropDownEntry(desiredStart);
            }
        }

        private void on_btnForward_clicked(object sender, EventArgs e)
        {
            // Move Down by one page
            int desiredStart = Convert.ToInt32(txtStartAddress.Text, 16) + 256;
            if (desiredStart < MemoryMap.FLASH_END)
            {
                GotoAddress(desiredStart);
                FindMatchedDropDownEntry(desiredStart);
            }
        }

        /*
         * Change the Master Control Register (MCR).
         * This allows for displaying text, overlay on top of graphics.
         */
        private void on_btnMCR_clicked(object sender, EventArgs e)
        {
            // toggle the button tag 0 or 1
            ToggleButton btn = sender as ToggleButton;

            // Save the value of all buttons to the Master Control Memory Location
            if (Memory != null) {
                int value = btnMCRBit0.Active ? 1 : 0;
                value |= (btnMCRBit1.Active ? 1 : 0) << 1;
                value |= (btnMCRBit2.Active ? 1 : 0) << 2;
                value |= (btnMCRBit3.Active ? 1 : 0) << 3;
                value |= (btnMCRBit4.Active ? 1 : 0) << 4;
                value |= (btnMCRBit5.Active ? 1 : 0) << 5;
                value |= (btnMCRBit6.Active ? 1 : 0) << 6;
                value |= (btnMCRBit7.Active ? 1 : 0) << 7;
                Memory.WriteByte(0x0000, (byte)value);

                value = btnMCRBit8.Active ? 1 : 0;
                value |= (btnMCRBit9.Active ? 1 : 0) << 1;
                Memory.WriteByte(0x0001, (byte)value);      // TODO: wrong address
            }

            if (txtStartAddress.Text.StartsWith("AF", false, null))
                RefreshMemoryView();

            if (btn == btnMCRBit6)
                SetGamma?.Invoke(btnMCRBit6.Active);

            if (btn == btnMCRBit8)
                SetHiRes?.Invoke(btnMCRBit8.Active);
        }

        private void on_cboAddress_changed(object sender, EventArgs e)
        {
            string value = cboAddress.ActiveText;
            int startAddress;

            if (value.StartsWith("Bank"))
            {
                // Read two characters and pad with '0000' to get a 24 bit address
                int start = value.IndexOf('$');
                startAddress = Convert.ToInt32(value.Substring(start + 1, 2) + "0000", 16);
            }
            else if (value.StartsWith("Address"))
            {
                // Read all 6 characters, but omit the ':'
                int start = value.IndexOf('$');
                startAddress = Convert.ToInt32(value.Replace(":", "").Substring(start + 1, 6), 16);
            }
            else
                return;

            GotoAddress(startAddress);
            btnRefresh.Activate();
        }

        private void on_txtStartAddress_activate(object sender, EventArgs e)
        {
            try
            {
                int requestedAddress = Convert.ToInt32(txtStartAddress.Text, 16) & 0xFFFF00;
                GotoAddress(requestedAddress);
                FindMatchedDropDownEntry(requestedAddress);
            }
            catch (FormatException ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void on_HighlightPanel_key_release_event(object sender, KeyReleaseEventArgs e)
        {
            if (mem.X == -1)
                return;

            string rawAddress = mem.X.ToString("X6");
            string address = "$" + rawAddress.Substring(0, 2) + ":" + rawAddress[2..];

            if (HighlightPanel.Text != "")
            {
                // The result may be a hexadecimal value
                byte intResult = Convert.ToByte(HighlightPanel.Text, 16);

                // Check that the value was changed
                if (intResult != mem.Y)
                {
                    Memory.WriteByte(mem.X, intResult);

                    RefreshMemoryView();
                }
            }
        }

        private void on_tvwMemory_leave_notify_event(object sender, LeaveNotifyEventArgs e)
        {
            HighlightPanel.Visible = false;
        }

        private void on_tvwMemory_motion_notify_event(object sender, MotionNotifyEventArgs e)
        {
            HighlightPanel.Visible = true;

            var point = new Point((int)e.Event.X, (int)e.Event.Y);
            GetAddressPosition(point);

            if (mem.X != -1 && mem.Y != -1)
            {
                string val = mem.Y.ToString("X2");
                string address = mem.X.ToString("X6");
                lblPosition.Text = $"Address: ${address[..2]}:{address[2..]}, Value: {val}";
            }
            else
                lblPosition.Text = "";
        }

        /**
         * Save the content of memory to a binary file.
         */
        private void on_btnSave_clicked(object sender, EventArgs e)
        {
            using (FileChooserDialog filechooser =
                new("Save File", this,
                FileChooserAction.Save,
                "Cancel", ResponseType.Cancel,
                "Open", ResponseType.Accept))
            {
                if (filechooser.Run() == (int)ResponseType.Accept)
                {
                    using (FileStream outputFile = File.Create(filechooser.Filename))
                    {
                        MemoryRAM ram = (MemoryRAM)Memory;
                        byte[] buffer = new byte[ram.Length];

                        ram.CopyIntoBuffer(0, ram.Length, buffer);

                        outputFile.Write(buffer, 0, buffer.Length);
                        outputFile.Flush();
                        outputFile.Close();
                    }
                }
            }
        }

        private void on_UpdateDisplayTimer_tick(object sender, ElapsedEventArgs e)
        {
            if (!(Memory is MemoryRAM))
            {
                RefreshMemoryView();
                UpdateMCRButtons();
            }
        }
    }
}
