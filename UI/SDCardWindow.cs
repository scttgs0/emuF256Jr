using System;

using Gtk;
using GUI = Gtk.Builder.ObjectAttribute;


namespace FoenixToolkit.UI
{
    class SDCardWindow : Window
    {
#pragma warning disable CS0649  // never assigned
        [GUI] CheckButton chkSdcEnable;
        [GUI] CheckButton chkIsoMode;
        [GUI] ComboBoxText cboCapacity;
        [GUI] ComboBoxText cboCluster;
        [GUI] ComboBoxText cboType;
        [GUI] FileChooserButton fcbFolder;
        [GUI] Label lblFolder;
#pragma warning restore CS0649

        public SDCardWindow() : this(new Builder("SDCardWindow.ui"))
        {
            cboCapacity.ActiveId = "64";
            cboCluster.ActiveId = "512";
            cboType.ActiveId = "FAT32";
        }

        private SDCardWindow(Builder builder) : base(builder.GetRawOwnedObject("SDCardWindow"))
        {
            builder.Autoconnect(this);
        }

        public bool GetISOMode()
        {
            return chkIsoMode.Active ? true : false;
        }

        // Virtual SD Card Path
        public void SetPath(string path)
        {
            if (path != null && path.Length > 0)
            {
                chkSdcEnable.Active = true;
                fcbFolder.SetFilename(path);
            }
            else
            {
                chkSdcEnable.Active = false;
                fcbFolder.SetFilename("");
            }
        }

        public string GetPath()
        {
            return chkSdcEnable.Active ? fcbFolder.Filename : null;
        }

        // Virtual SD Card Capacity
        public void SetCapacity(int value)
        {
            cboCapacity.ActiveId = value.ToString();
        }

        public int GetCapacity()
        {
            return Convert.ToInt32(cboCapacity.ActiveId);
        }

        // Virtual SD Card Cluster Size
        public void SetClusterSize(int value)
        {
            cboCluster.ActiveId = value.ToString();
        }

        public int GetClusterSize()
        {
            return Convert.ToInt32(cboCluster.ActiveId);
        }

        // Virtual SD Card Filesystem Type
        public void SetFSType(string fsname)
        {
            cboType.ActiveId = fsname;
        }

        public string GetFSType()
        {
            return cboType.ActiveId;
        }

        private void on_chkSdcEnable_toggled(object sender, EventArgs e)
        {
            cboCapacity.Sensitive = chkIsoMode.Active ? false : chkSdcEnable.Active;
            cboType.Sensitive = chkIsoMode.Active ? false : chkSdcEnable.Active;
            cboCluster.Sensitive = chkIsoMode.Active ? false : chkSdcEnable.Active;

            if (!chkSdcEnable.Active)
            {
                fcbFolder.SetFilename("");
                cboCapacity.ActiveId = "64";
                cboCluster.ActiveId = "512";
                cboType.ActiveId = "FAT32";
            }
        }

        private void on_chkIsoMode_toggled(object sender, EventArgs e)
        {
            lblFolder.Text = chkIsoMode.Active ? "Image" : "Folder";
            fcbFolder.Action = chkIsoMode.Active ? FileChooserAction.Open : FileChooserAction.SelectFolder;

            if (chkIsoMode.Active)
            {
                cboCapacity.ActiveId = "";
                cboCapacity.Sensitive = false;
                cboCluster.ActiveId = "";
                cboCluster.Sensitive = false;
                cboType.ActiveId = "";
                cboType.Sensitive = false;
            }
            else
            {
                cboCapacity.ActiveId = "64";
                cboCapacity.Sensitive = true;
                cboCluster.ActiveId = "512";
                cboCluster.Sensitive = true;
                cboType.ActiveId = "FAT32";
                cboType.Sensitive = true;
            }

            fcbFolder.SetFilename("");
        }

        private void on_fcbFolder_file_set(object sender, EventArgs e)
        {
            chkSdcEnable.Active = true;
        }

        private void on_cboCapacity_changed(object sender, EventArgs e)
        {
            switch (cboCapacity.ActiveId)
            {
                case "8":
                    cboCluster.ActiveId = "4096";
                    cboType.ActiveId = "FAT12";
                    break;

                case "16":
                    cboCluster.ActiveId = "8192";
                    cboType.ActiveId = "FAT12";
                    break;

                case "32":
                    cboCluster.ActiveId = "1024";
                    cboType.ActiveId = "FAT16";
                    break;

                case "64":
                    cboCluster.ActiveId = "2048";
                    cboType.ActiveId = "FAT16";
                    break;

                case "128":
                    cboCluster.ActiveId = "4096";
                    cboType.ActiveId = "FAT16";
                    break;

                case "256":
                    cboCluster.ActiveId = "512";
                    cboType.ActiveId = "FAT32";
                    break;

                case "512":
                    cboCluster.ActiveId = "1024";
                    cboType.ActiveId = "FAT32";
                    break;

                case "1024":
                    cboCluster.ActiveId = "2048";
                    cboType.ActiveId = "FAT32";
                    break;

                case "2048":
                    cboCluster.ActiveId = "4096";
                    cboType.ActiveId = "FAT32";
                    break;
            }
        }
    }
}
