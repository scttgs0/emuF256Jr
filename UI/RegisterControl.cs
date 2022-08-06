using System;

using Gtk;
using GUI = Gtk.Builder.ObjectAttribute;

using FoenixCore.Processor.GenericNew;


namespace FoenixToolkit.UI
{
    public class RegisterControl<T> : Box
    {
        string _caption;
        string _value;
        Register<T> _register = null;

#pragma warning disable CS0649  // never assigned
        [GUI] Label lblRegister;
        [GUI] Entry txtRegister;
#pragma warning restore CS0649

        public RegisterControl() : this(new Builder("RegisterControl.ui")) { }

        private RegisterControl(Builder builder) : base(builder.GetRawOwnedObject("RegisterControl"))
        {
            builder.Autoconnect(this);
        }

        public string Caption
        {
            get => _caption;
            set
            {
                _caption = value;
                lblRegister.Text = value;
            }
        }

        public string Value
        {
            get => _value;
            set
            {
                _value = value;
                txtRegister.Text = value;
            }
        }

        public Register<T> Register
        {
            get => _register;
            set
            {
                _register = value;
                if (value != null)
                    UpdateValue();
            }
        }

        public void UpdateValue()
        {
            if (Register != null)
                Value = _register.ToString();
        }
    }
}
