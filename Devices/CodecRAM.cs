
using System.Threading.Tasks;


namespace FoenixCore.Simulator.Devices
{
    public class CodecRAM : MemoryLocations.MemoryRAM
    {
        public CodecRAM(int StartAddress, int Length) : base(StartAddress, Length)
        {
        }

        public override async void WriteByte(int Address, byte Value)
        {
            data[Address] = Value;
            await Task.Delay(200);
            data[0] = 0;
        }
    }
}
