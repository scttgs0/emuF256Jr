using System;
using System.Collections.Generic;
using System.Timers;

using Gtk;
using GUI = Gtk.Builder.ObjectAttribute;

using FoenixCore;
using FoenixCore.Simulator.FileFormat;


namespace FoenixToolkit.UI
{
    class WatchWindow : Window
    {
        Timer _watchUpdateTimer = null;

        private FoenixSystem _kernel;
        public static WatchWindow Instance;

#pragma warning disable CS0649  // never assigned
        //--[GUI] Button btnAdd;
        [GUI] Entry txtAddress;
        [GUI] Entry txtName;
        //--[GUI] ListStore liststoreWatches;
        [GUI] TreeView WatchGrid;
        [GUI] TreeViewColumn colName;
        [GUI] TreeViewColumn colAddress;
        [GUI] TreeViewColumn col8bit;
        [GUI] TreeViewColumn col16bit;
        [GUI] TreeViewColumn colMem;
        [GUI] TreeViewColumn colDel;
#pragma warning restore CS0649

        public WatchWindow() : this(new Builder("WatchWindow.ui"))
        {
             CellRendererText watchNameCell = new();
             colName.PackStart(watchNameCell, true);
             colName.AddAttribute(watchNameCell, "text", 0);

             CellRendererText watchAddress = new();
             colAddress.PackStart(watchAddress, true);
             colAddress.AddAttribute(watchAddress, "text", 1);

             CellRendererText watch8bitCell = new();
             col8bit.PackStart(watch8bitCell, true);
             col8bit.AddAttribute(watch8bitCell, "text", 2);

             CellRendererText watch16bitCell = new();
             col16bit.PackStart(watch16bitCell, true);
             col16bit.AddAttribute(watch16bitCell, "text", 3);

             CellRendererPixbuf watchMemCell = new();
             colMem.PackStart(watchMemCell, true);
             watchMemCell.Pixbuf = new Gdk.Pixbuf("Images/memory-btn.png");

             CellRendererPixbuf watchDelCell = new();
             colDel.PackStart(watchDelCell, true);
             watchDelCell.Pixbuf = new Gdk.Pixbuf("Images/delete-btn.png");
        }

        private WatchWindow(Builder builder) : base(builder.GetRawOwnedObject("WatchWindow"))
        {
            Instance = this;
            builder.Autoconnect(this);

            HideOnDelete();
        }

        public void SetKernel(FoenixSystem kernel)
        {
            if ((_kernel = kernel) == null)
                return;

            _watchUpdateTimer.Enabled = true;
        }

        private void RefreshListStore()
        {
            //-- int index = 0;

            // TreeIter iter;
            // liststoreWatches.GetValue(iter, 0)
            // foreach (KeyHashEntry ls in liststoreWatches.Data)
            // {
            //     ls.
            // }
            // liststoreWatches.Data.Add()
            // foreach (KeyValuePair<int, WatchedMemory> kvp in _kernel.WatchList)
            // {
            //     WatchedMemory mem = kvp.Value;
            //     mem.val8bit = _kernel.MemMgr.ReadByte(mem.address);
            //     mem.val16bit = _kernel.MemMgr.ReadWord(mem.address);
            // }
        }

        private void on_WatchWindow_map(object sender, EventArgs e)
        {
            _watchUpdateTimer = new()
            {
               Interval = 1000,
               Enabled = false,
               AutoReset = true
            };

            _watchUpdateTimer.Elapsed += on_WatchUpdateTimer_tick;
        }

        private void on_WatchWindow_unmap(object sender, EventArgs e)
        {
            if (_watchUpdateTimer != null)
            {
               _watchUpdateTimer.Stop();
               _watchUpdateTimer.Dispose();
               _watchUpdateTimer = null;
            }
        }

        private void on_WatchWindow_key_press_event(object sender, KeyPressEventArgs e)
        {
            if (e.Event.Key == Gdk.Key.Escape)
                Hide();
        }

        /**
         * Update the values in the visible cells
         */
        private void on_WatchUpdateTimer_tick(object sender, ElapsedEventArgs e)
        {
            if (_kernel == null)
                throw new InvalidOperationException("Kernel is undefined");

            foreach (KeyValuePair<int, WatchedMemory> kvp in _kernel.WatchList)
            {
                WatchedMemory mem = kvp.Value;
                mem.val8bit = _kernel.MemMgr.ReadByte(mem.address);
                mem.val16bit = _kernel.MemMgr.ReadWord(mem.address);
            }

            WatchGrid.QueueDraw();
        }

        //-- private void WatchGrid_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        // {
            // if (_kernel == null)
            //     throw new InvalidOperationException("Kernel is undefined");

        //     try
        //     {
        //         KeyValuePair<int, WatchedMemory> kvp = _kernel.WatchList.ElementAt(e.RowIndex);

        //         switch (e.ColumnIndex)
        //         {
        //             case 0:
        //                 e.Value = kvp.Value.name;
        //                 break;

        //             case 1:
        //                 e.Value = kvp.Value.address.ToString("X6");
        //                 break;

        //             case 2:
        //                 e.Value = kvp.Value.val8bit.ToString("X2"); ;
        //                 break;

        //             case 3:
        //                 e.Value = kvp.Value.val16bit.ToString("X4");
        //                 break;
        //         }
        //     }
        //     catch
        //     {
        //         // whatever!
        //     }
        // }

        //-- private void WatchGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        // {
            // if (_kernel == null)
            //     throw new InvalidOperationException("Kernel is undefined");

        //     // Get the address for the RowIndex
        //     KeyValuePair<int, WatchedMemory> kvp = _kernel.WatchList.ElementAt(e.RowIndex);

        //     switch (e.ColumnIndex)
        //     {
        //         // Browse this page in the Memory Window
        //         case 4:
        //             MemoryWindow.Instance.GotoAddress(kvp.Key & 0xFFFF00);
        //             break;

        //         // Delete the row, but copy the values into our input boxes
        //         case 5:
        //             txtName.Text = kvp.Value.name;
        //             txtAddress.Text = "$" + kvp.Value.address.ToString("X6");
        //             _kernel.WatchList.Remove(kvp.Key);
        //             //-- WatchGrid.RowCount -= 1;
        //             break;
        //     }
        // }

        private void on_btnAdd_clicked(object sender, EventArgs e)
        {
            if (_kernel == null)
                throw new InvalidOperationException("Kernel is undefined");

            if (txtAddress.Text.Length > 0)
            {
                int addressVal = Convert.ToInt32(txtAddress.Text.Replace("$", "").Replace(":", ""), 16);

                if (txtName.Text.Length > 0 && addressVal >= 0)
                {
                    if (_kernel.WatchList.ContainsKey(addressVal))
                        _kernel.WatchList.Remove(addressVal);

                    WatchedMemory mem = new(txtName.Text, addressVal,
                        _kernel.MemMgr.ReadByte(addressVal),
                        _kernel.MemMgr.ReadWord(addressVal)
                    );

                    _kernel.WatchList.Add(addressVal, mem);

                    txtName.Text = "";
                    txtAddress.Text = "";
                }
            }
        }
    }
}
