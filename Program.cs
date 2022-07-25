using System;
using System.Collections.Generic;

using Gtk;


namespace FoenixToolkit.UI
{
    class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            Dictionary<string, string> context = null;
            bool OkToContinue = true;

            if (args.Length > 0)
            {
                Console.CancelKeyPress += new ConsoleCancelEventHandler(Console_CancelKeyPress);
                context = DecodeProgramArguments(args);
                OkToContinue = "true".Equals(context["Continue"]);
            }

            if (OkToContinue)
            {
                Application.Init();

                var app = new Application("org.FoenixToolkit.FoenixToolkit", GLib.ApplicationFlags.None);
                app.Register(GLib.Cancellable.Current);

                var win = new MainWindow(context);
                app.AddWindow(win);

                win.Show();
                Application.Run();
            }
        }

        private static Dictionary<String,String> DecodeProgramArguments(string[] args)
        {
            Dictionary<string, string> context = new()
            {
                { "Continue", "true" }
            };

            for (int i = 0; i < args.Length; ++i)
            {
                switch (args[i].Trim())
                {
                    // the hex file to load is specified
                    case "-h":
                    case "--hex":
                        // a kernel file must be specified
                        if (args.Length == i + 1 || args[i + 1].Trim().StartsWith("-") || !args[i + 1].Trim().EndsWith("hex"))
                        {
                            Console.Out.WriteLine("You must specify a hex file.");
                            context["Continue"] = "false";
                            break;
                        }

                        context.Add("defaultKernel", args[i + 1]);
                        i++; // skip the next argument
                        break;

                    case "-j":
                    case "--jump":
                        // An address must be specified
                        if (args.Length == i + 1 || args[i + 1].Trim().StartsWith("-")) {
                            Console.Out.WriteLine("You must specify a jump address.");
                            context["Continue"] = "false";
                            break;
                        }

                        int value = -1;
                        try {
                            value = Convert.ToInt32(args[i + 1].Replace("$:", ""), 16);
                        }
                        catch (System.FormatException) {}

                        if (value > 0)
                        {
                            context.Add("jumpStartAddress", value.ToString());
                            i++; // skip the next argument
                        }
                        else
                        {
                            Console.Out.WriteLine("Invalid address specified: " + args[i + 1]);
                            context["Continue"] = "false";
                        }
                        break;

                    // Disable IRQs - a value is not expected for this one
                    case "-i":
                    case "--irq":
                        context.Add("disabledIRQs", "true");
                        break;

                    case "--help":
                        DisplayUsage();

                        context["Continue"] = "false";
                        break;

                    default:
                        Console.Out.WriteLine("Unknown switch used:" + args[i].Trim());

                        DisplayUsage();

                        context["Continue"] = "false";
                        break;
                }
            }

            return context;
        }

        static void DisplayUsage()
        {
            Console.Out.WriteLine("Foenix IDE Command Line Usage:");
            Console.Out.WriteLine("   -h, --hex: kernel file name");
            Console.Out.WriteLine("   -j, --jump: jump to specified address");
            Console.Out.WriteLine("   -i, --irq: disable IRQs true/false");
            Console.Out.WriteLine("   --help: show this usage");
        }

        static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            Application.Quit();
        }
    }
}
