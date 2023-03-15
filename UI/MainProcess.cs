using FoenixCore.Simulator.Devices;
using FoenixToolkit.UI;


namespace FoenixCore
{
    public class MainProcess
    {
        //--private FoenixSystem kernel;
        //--private BoardVersion version = BoardVersion.RevJr;
        //--private Cpu6809Window cpuWindow;

        public static byte BCD(int val)
        {
            return (byte)(val / 10 * 0x10 + val % 10);
        }

        public static int VersionValue(string value)
        {
            int intValue = 0;

            string[] parts = value.Split('.');
            foreach (string part in parts)
            {
                try
                {
                    int partialValue = int.Parse(part);
                    intValue = intValue * 100 + partialValue;
                }
                finally { }
            }

            return intValue;
        }
    }
}
