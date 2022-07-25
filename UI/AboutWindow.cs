using System;

using Gtk;


namespace FoenixToolkit.UI
{
    class AboutWindow : AboutDialog
    {
        public AboutWindow() : this(new Builder("AboutWindow.ui")) {
        }

        private AboutWindow(Builder builder) : base(builder.GetRawOwnedObject("AboutWindow"))
        {
            builder.Autoconnect(this);
            HideOnDelete();
        }

        private void on_AboutDialog_response(object sender, ResponseArgs e)
        {
            Hide();
        }

        protected void on_AboutDialog_map(object sender, EventArgs e)
        {
            var ver = FoenixCore.AboutWindowProcess.AppVersion();
            ((AboutWindow)sender).Version = $"Version {ver}";
        }
    }
}
