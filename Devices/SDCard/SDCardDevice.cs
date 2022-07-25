using FoenixCore.MemoryLocations;
using FoenixCore.Simulator.Devices.SDCard;


namespace FoenixCore.Simulator.Devices
{
    public enum FSType
    {
        FAT12, FAT16, FAT32
    };

    public abstract class SDCardDevice : MemoryRAM
    {
        public bool isPresent = false;
        private string _sdCardPath = "";
        private bool _isoMode = false;
        private int _capacity = 8; // Capacity in MB
        private int _clusterSize = 512;
        private FSType _fsType = FSType.FAT32;
        protected string sdCurrentPath = "";
        public delegate void SDCardInterruptEvent(CH376SInterrupt irq);
        public SDCardInterruptEvent sdCardIRQMethod;

        public SDCardDevice(int StartAddress, int Length) : base(StartAddress, Length)
        {
        }

        // Path
        public string SDCardPath
        {
            get => _sdCardPath;
            set => _sdCardPath = value;
        }

        // Capacity
        public int Capacity
        {
            get => _capacity;
            set => _capacity = value;
        }

        // ISO mode
        public bool IsoMode
        {
            get => _isoMode;
            set => _isoMode = value;
        }

        // Cluster Size
        public int ClusterSize
        {
            get => _clusterSize;
            set => _clusterSize = value;
        }

        public FSType FileSystemType
        {
            get => _fsType;
            set => _fsType = value;
        }

        public abstract void ResetMbrBootSector();
    }
}
