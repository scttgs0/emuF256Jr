
using System;
using System.IO;

using Gtk;

using FoenixCore.MemoryLocations;


namespace FoenixCore.Simulator.FileFormat
{
    public class HexFile
    {
        static public string Load(MemoryRAM ram, string Filename, int gabeAddressBank, out int startAddress, out int length)
        {
            int bank = 0;
            int address = 0;
            string processedFileName = Filename;

            startAddress = -1;
            length = -1;

            if (!File.Exists(Filename))
            {
                FileChooserDialog filechooser =
                    new("Select a kernel file", null,
                        FileChooserAction.Open,
                        "Cancel", ResponseType.Cancel,
                        "Open", ResponseType.Accept);

                using (FileFilter ff1 = new())
                {
                    ff1.Name = "Hex Files";
                    ff1.AddPattern("*.hex");
                    filechooser.AddFilter(ff1);
                }
                using (FileFilter ff2 = new())
                {
                    ff2.Name = "All Files";
                    ff2.AddPattern("*.*");
                    filechooser.AddFilter(ff2);
                }

                if (filechooser.Run() == (int)ResponseType.Accept)
                    processedFileName = filechooser.Filename;
                else
                {
                    filechooser.Destroy();
                    return null;
                }

                filechooser.Destroy();
            }

            string[] lines = File.ReadAllLines(processedFileName);

            foreach (string l in lines)
            {
                if (l.StartsWith(":"))
                {
                    string mark = l.Substring(0, 1);
                    string reclen = l.Substring(1, 2);
                    string offset = l.Substring(3, 4);
                    string rectype = l.Substring(7, 2);
                    string data = l.Substring(9, l.Length - 11);
                    string checksum = l[^2..];

                    switch (rectype)
                    {
                        // data row. The next n bytes are data to be loaded into memory
                        case "00":
                            address = GetByte(offset, 0, 2);

                            if (startAddress == -1 && ((address & 0xFF00) != 0xFF00))
                                startAddress = bank + address;

                            if (bank <= ram.Length)
                            {
                                for (int i = 0; i < data.Length; i += 2)
                                {
                                    int b = GetByte(data, i, 1);
                                    ram.WriteByte(bank + address, (byte)b);
                                    // Copy bank $38 or $18 to page 0

                                    if (bank == gabeAddressBank)
                                        ram.WriteByte(address, (byte)b);

                                    address++;
                                }
                            }

                            break;

                        // end of file - just ignore
                        case "01":
                            length = address;
                            break;

                        case "02":
                            bank = GetByte(data, 0, 2) * 16;
                            break;

                        // extended linear address
                        // lower byte will populate the bank number.
                        case "04":
                            bank = GetByte(data, 0, 2) << 16;
                            break;

                        // extended linear start address
                        // set the initial bank register value. Not used in the simulator.
                        case "05":
                            break;

                        default:
                            throw new NotImplementedException("Record type not implemented: " + rectype);
                    }
                }
                else
                {
                    using (var md = new MessageDialog(null, DialogFlags.Modal | DialogFlags.DestroyWithParent,
                            MessageType.Error, ButtonsType.Ok, "This doesn't appear to be an Intel Hex file.")) {
                        md.Title = "Error Loading Hex File";
                        md.Run();
                    }
                    break;
                }
            }

            return processedFileName;
        }

        // Read a two-character hex string into a byte
        static public int GetByte(string data, int startPos, int bytes)
        {
            return Convert.ToInt32(data.Substring(startPos, bytes * 2), 16);
        }
    }
}
