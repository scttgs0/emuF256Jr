using System;

﻿using FoenixCore.MemoryLocations;


namespace FoenixCore.Simulator.Devices
{
    public class GabeRAM : MemoryLocations.MemoryRAM
    {
        private readonly Random rng = new();

        public GabeRAM(int StartAddress, int Length) : base(StartAddress, Length)
        {
        }

        override public byte ReadByte(int Address)
        {
            var rnd = rng.Next(0xFFFF);

            if (Address == (MemoryMap.SYSTEM_CTRL.RANDOM - MemoryMap.VICKY_START))
                return (byte)(rnd & 0xFF);

            if (Address == (MemoryMap.SYSTEM_CTRL.RANDOM+1 - MemoryMap.VICKY_START))
                return (byte)((rnd >> 8) & 0xFF);

            return data[Address];
        }

        override public int ReadWord(int Address)
        {

            if (Address == (MemoryMap.SYSTEM_CTRL.RANDOM - MemoryMap.VICKY_START))
                return rng.Next(0xFFFF);

            return (data[Address+1] << 8) + data[Address];
        }
    }
}
