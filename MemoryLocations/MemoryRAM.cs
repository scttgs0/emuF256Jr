using System;


namespace FoenixCore.MemoryLocations
{
    public class MemoryRAM : IMappable
    {
        protected byte[] data = null;
        private readonly int startAddress;
        private readonly int length;
        private readonly int endAddress;

        public int StartAddress => startAddress;
        public int Length => length;
        public int EndAddress => endAddress;

        public MemoryRAM(int StartAddress, int Length)
        {
            startAddress = StartAddress;
            length = Length;
            endAddress = StartAddress + Length - 1;
            data = new byte[Length];
        }

        /// <summary>
        /// Clear all the bytes in the memory array.
        /// </summary>
        public void Zero()
        {
            Array.Clear(data, 0, Length);
        }

        /// <summary>
        /// Reads a byte from memory
        /// </summary>
        /// <param name="Address"></param>
        /// <returns></returns>
        public virtual byte ReadByte(int Address)
        {
            if (Address >= 0 && Address < Length)
                return data[Address];
            else
                return 0x40;
        }

        /// <summary>
        /// Reads a 16-bit word from memory
        /// </summary>
        /// <param name="Address"></param>
        /// <returns></returns>
        public int ReadWord(int Address)
        {
            return BitConverter.ToUInt16(data, Address);
        }

        /// <summary>
        /// Read a 24-bit long from memory
        /// </summary>
        /// <param name="Address"></param>
        /// <returns></returns>
        internal int ReadLong(int Address)
        {
            return (int)BitConverter.ToInt32(data, Address) & 0xFF_FFFF;
        }

        internal void Load(byte[] SourceData, int SrcStart, int DestStart, int copyLength)
        {
            for (int i = 0; i < copyLength; ++i)
                data[DestStart + i] = SourceData[SrcStart + i];
        }

        public virtual void WriteByte(int Address, byte Value)
        {
            data[Address] = Value;
        }

        public void WriteWord(int Address, ushort Value)
        {
            WriteByte(Address, (byte)(Value & 0xff));
            WriteByte(Address + 1, (byte)(Value >> 8 & 0xff));
        }

        // Duplicate a memory block
        internal void Duplicate(int SourceAddress, int DestAddress, int Length)
        {
            Array.Copy(data, SourceAddress, data, DestAddress, Length);
        }

        public void CopyIntoBuffer(int srcAddress, int srcLength, byte[] buffer)
        {
            Array.Copy(data, srcAddress, buffer, 0, srcLength);
        }

        // Copy data from a buffer to RAM
        public void CopyFromBuffer(byte[] src, int srcAddress, int destAddress, int length)
        {
            Array.Copy(src, srcAddress, data, destAddress, length);
        }
    }
}
