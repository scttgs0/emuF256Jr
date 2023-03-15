
using System;
using System.Collections.Generic;
using System.Xml;

using FoenixCore.Processor;
using FoenixCore.Simulator.Devices;
using static FoenixCore.Simulator.FileFormat.ResourceChecker;


namespace FoenixCore.Simulator.FileFormat
{
    class FoenixXmlFile
    {
        private readonly FoenixSystem _kernel;
        private ResourceChecker _resources;

        private const int PHRASE_LENGTH = 16;

        private readonly Processor.Generic.Breakpoints BreakPoints;
        private readonly SortedList<int, DebugLine> codeList;
        private readonly SortedList<int, WatchedMemory> watchList;
        public BoardVersion Version;

        readonly string tabs = "\t\t\t\t\t\t\t\t";

        private FoenixXmlFile() { }

        public FoenixXmlFile(FoenixSystem kernel, ResourceChecker resources)
        {
            _kernel = kernel;
            _resources = resources;

            codeList = _kernel.lstFile.Lines;
            BreakPoints = _kernel.Breakpoints;
            watchList = _kernel.WatchList;
        }

        public void Write(String filename, bool compact)
        {
            if (_kernel == null)
                throw new InvalidOperationException("Kernel is undefined");
            if (_resources == null)
                throw new InvalidOperationException("Resources is undefined");

            XmlWriter xmlWriter = XmlWriter.Create(filename);

            xmlWriter.WriteStartDocument();
            xmlWriter.WriteRaw("\r");
            xmlWriter.WriteComment("Export of FoenixIDE for C256.  All values are in hexadecimal form");
            xmlWriter.WriteRaw("\r");

            xmlWriter.WriteStartElement("project");
            xmlWriter.WriteAttributeString("version", _kernel.GetVersion().ToString());
            xmlWriter.WriteRaw("\r");

            // Write resources
            xmlWriter.WriteRaw(tabs[..1]);
            xmlWriter.WriteStartElement("resources");
            xmlWriter.WriteRaw("\r");

            foreach (ResourceChecker.Resource res in _resources.Items)
            {
                xmlWriter.WriteRaw(tabs[..2]);
                xmlWriter.WriteStartElement("resource");
                xmlWriter.WriteAttributeString("name", res.Name);
                xmlWriter.WriteAttributeString("source", res.SourceFile);
                xmlWriter.WriteAttributeString("start-address", res.StartAddress.ToString("X6"));
                xmlWriter.WriteAttributeString("length", res.Length.ToString("X"));
                xmlWriter.WriteAttributeString("resource-type", res.FileType.ToString());
                xmlWriter.WriteEndElement();  // end resource
                xmlWriter.WriteRaw("\r");
            }

            xmlWriter.WriteRaw(tabs[..1]);
            xmlWriter.WriteEndElement(); // end resources
            xmlWriter.WriteRaw("\r");

            if (BreakPoints != null)
            {
                // Write breakpoints
                xmlWriter.WriteRaw(tabs[..1]);
                xmlWriter.WriteStartElement("breakpoints");
                xmlWriter.WriteRaw("\r");

                foreach (string bp in BreakPoints.Values)
                {
                    xmlWriter.WriteRaw(tabs[..2]);
                    xmlWriter.WriteStartElement("breakpoint");
                    xmlWriter.WriteAttributeString("address", bp);
                    xmlWriter.WriteEndElement();  // end resource
                    xmlWriter.WriteRaw("\r");
                }

                xmlWriter.WriteRaw(tabs[..1]);
                xmlWriter.WriteEndElement(); // end breakpoints
                xmlWriter.WriteRaw("\r");
            }

            // Write code listing
            xmlWriter.WriteRaw(tabs[..1]);
            xmlWriter.WriteStartElement("listing");
            xmlWriter.WriteRaw("\r");

            foreach (DebugLine code in codeList.Values)
            {
                xmlWriter.WriteRaw(tabs[..2]);
                xmlWriter.WriteStartElement("code");
                xmlWriter.WriteAttributeString("address", "$" + code.PC.ToString("X6"));
                xmlWriter.WriteAttributeString("command", code.GetOpcodes());
                xmlWriter.WriteAttributeString("source", code.GetSource());

                if (code.label != null)
                    xmlWriter.WriteAttributeString("label", code.label);

                xmlWriter.WriteEndElement();  // end resource
                xmlWriter.WriteRaw("\r");
            }

            xmlWriter.WriteRaw(tabs[..1]);
            xmlWriter.WriteEndElement(); // end code listing
            xmlWriter.WriteRaw("\r");

            if (watchList != null)
            {
                // Write watch list
                xmlWriter.WriteRaw(tabs[..1]);
                xmlWriter.WriteStartElement("watches");
                xmlWriter.WriteRaw("\r");

                foreach (KeyValuePair<int, WatchedMemory> nvp in watchList)
                {
                    xmlWriter.WriteRaw(tabs[..2]);
                    xmlWriter.WriteStartElement("watch");
                    xmlWriter.WriteAttributeString("address", nvp.Key.ToString("X6"));
                    xmlWriter.WriteAttributeString("label", nvp.Value.name);
                    xmlWriter.WriteEndElement();  // end resource
                    xmlWriter.WriteRaw("\r");
                }

                xmlWriter.WriteRaw(tabs[..1]);
                xmlWriter.WriteEndElement(); // end watch list
                xmlWriter.WriteRaw("\r");
            }

            // Write pages
            xmlWriter.WriteRaw(tabs[..1]);
            xmlWriter.WriteStartElement("pages");

            if (compact)
                xmlWriter.WriteAttributeString("format", "compact");
            else
                xmlWriter.WriteAttributeString("format", "full");

            xmlWriter.WriteRaw("\r");

            // We don't need to scan $FFFF pages, only scan the ones we know are gettings used
            // Scan each of the banks and pages and save to an XML file
            // If a page is blank, don't export it.
            int RamLength = _kernel.MemMgr.RAM.Length;

            for (int i = 0; i < RamLength; i += 256)
                if (PageChecksum(i) != 0)
                    WriteData(i, xmlWriter, compact);

            xmlWriter.WriteRaw(tabs[..1]);
            xmlWriter.WriteEndElement(); // end pages
            xmlWriter.WriteRaw("\r");

            xmlWriter.WriteRaw(tabs[..1]);
            xmlWriter.WriteStartElement("vicky");
            xmlWriter.WriteRaw("\r");

            for (int i = 0xAF_0000; i < 0xB0_0000; i += 256)
                if (PageChecksum(i) != 0)
                    WriteData(i, xmlWriter, compact);

            xmlWriter.WriteRaw(tabs[..1]);
            xmlWriter.WriteEndElement(); // end vicky

            // Video RAM for resources
            xmlWriter.WriteRaw(tabs[..1]);
            xmlWriter.WriteStartElement("video");
            xmlWriter.WriteRaw("\r");

            foreach (ResourceChecker.Resource res in _kernel.Resources.Items)
                // If the assets were loaded in RAM, they would have been saved already
                if (res.StartAddress >= 0xB0_0000)
                    for (int i = res.StartAddress; i < res.StartAddress + res.Length; i += 256)
                        if (PageChecksum(i) != 0)
                            WriteData(i, xmlWriter, compact);

            xmlWriter.WriteRaw(tabs[..1]);
            xmlWriter.WriteEndElement(); // end video

            xmlWriter.WriteRaw("\r");
            xmlWriter.WriteEndElement(); // end project

            xmlWriter.WriteEndDocument();
            xmlWriter.Close();
        }

        public void WriteWatches(string filename)
        {
            XmlWriter xmlWriter = XmlWriter.Create(filename);

            xmlWriter.WriteStartDocument();
            xmlWriter.WriteRaw("\r");
            xmlWriter.WriteComment("Export of Watch List for C256 Foenix IDE");
            xmlWriter.WriteRaw("\r");

            if (watchList != null)
            {
                // Write watch list
                xmlWriter.WriteStartElement("watches");
                xmlWriter.WriteRaw("\r");

                foreach (KeyValuePair<int, WatchedMemory> nvp in watchList)
                {
                    xmlWriter.WriteRaw(tabs[..1]);
                    xmlWriter.WriteStartElement("watch");
                    xmlWriter.WriteAttributeString("address", nvp.Key.ToString("X6"));
                    xmlWriter.WriteAttributeString("label", nvp.Value.name);
                    xmlWriter.WriteEndElement();  // end resource
                    xmlWriter.WriteRaw("\r");
                }

                xmlWriter.WriteEndElement(); // end watch list
                xmlWriter.WriteRaw("\r");
            }

            xmlWriter.WriteEndDocument();
            xmlWriter.Close();
        }

        public void ReadWatches(string filename)
        {
            XmlReader reader = XmlReader.Create(filename);

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    if (reader.Name.Equals("watch"))
                    {
                        int address = Convert.ToInt32(reader.GetAttribute("address"), 16);
                        string name = reader.GetAttribute("label");
                        WatchedMemory mem = new(name, address, 0, 0);

                        if (watchList.ContainsKey(address))
                            watchList.Remove(address);

                        watchList.Add(address, mem);
                    }
                }
            }
        }

        private void WriteData(int startAddress, XmlWriter writer, bool compact)
        {
            writer.WriteRaw(tabs[..1]);
            writer.WriteStartElement("page");
            writer.WriteAttributeString("start-address", "$" + startAddress.ToString("X6"));
            writer.WriteAttributeString("bank", "$" + startAddress.ToString("X6").Substring(0, 2));
            writer.WriteRaw("\r");

            // Write PHRASE_LENGTH bytes per data line
            for (int i = 0; i < 256; i += PHRASE_LENGTH)
                WritePhrase(startAddress + i, writer, compact);

            writer.WriteRaw(tabs[..1]);
            writer.WriteEndElement();
            writer.WriteRaw("\r");
        }

        // Only write a phrase if the bytes are non-zero
        private void WritePhrase(int startAddress, XmlWriter writer, bool compact)
        {
            if (_kernel == null)
                throw new InvalidOperationException("Kernel is undefined");

            if (PhraseChecksum(startAddress) == 0 && !compact || PhraseChecksum(startAddress) != 0)
            {
                writer.WriteRaw(tabs[..2]);
                writer.WriteStartElement("data");
                writer.WriteAttributeString("address", "$" + (startAddress).ToString("X6"));

                for (int i = 0; i < PHRASE_LENGTH; ++i)
                    writer.WriteString(_kernel.MemMgr.ReadByte(startAddress + i).ToString("X2") + " ");

                writer.WriteEndElement();
                writer.WriteRaw("\r");
            }
        }

        // Sum 256 bytes
        private int PageChecksum(int startAddress)
        {
            if (_kernel == null)
                throw new InvalidOperationException("Kernel is undefined");

            int sum = 0;

            for (int i = 0; i < 255; ++i)
                sum += _kernel.MemMgr.ReadByte(startAddress + i);

            return sum;
        }

        // Sum PHRASE_LENGTH bytes
        private int PhraseChecksum(int startAddress)
        {
            if (_kernel == null)
                throw new InvalidOperationException("Kernel is undefined");

            int sum = 0;
            for (int i = 0; i < PHRASE_LENGTH; ++i)
                sum += _kernel.MemMgr.ReadByte(startAddress + i);

            return sum;
        }

        /*
         * Read a file into memory
         */
        public void Load(String filename)
        {
            XmlReader reader = XmlReader.Create(filename);
            Version = BoardVersion.RevJr;

            if (_resources == null)
                _resources = new ResourceChecker();
            else
                _resources.Clear();

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    if (reader.Name.Equals("data"))
                    {
                        LoadMemory(reader.GetAttribute("address"), reader.ReadElementContentAsString());
                        continue;
                    }

                    if (reader.Name.Equals("resource"))
                    {
                        ResourceChecker.Resource res = new()
                        {
                            Name = reader.GetAttribute("name"),
                            SourceFile = reader.GetAttribute("source"),
                            StartAddress = Convert.ToInt32(reader.GetAttribute("start-address"), 16),
                            Length = Convert.ToInt32(reader.GetAttribute("length"), 16)
                        };

                        string resType = reader.GetAttribute("resource-type");
                        if (resType != null)
                            res.FileType = (ResourceType)Enum.Parse(typeof(ResourceType), resType);
                        else
                            res.FileType = ResourceType.raw;

                        _resources.Add(res);
                        continue;
                    }

                    if (reader.Name.Equals("code"))
                    {
                        string addrStr = reader.GetAttribute("address");

                        if (addrStr.Length > 0)
                        {
                            int address = Convert.ToInt32(addrStr.Replace("$", ""), 16);
                            string label = reader.GetAttribute("label");
                            string source = reader.GetAttribute("source");
                            string command = reader.GetAttribute("command");

                            DebugLine code = new(address);
                            code.SetLabel(label);
                            code.SetMnemonic(source);
                            code.SetOpcodes(command);

                            if (codeList.ContainsKey(address))
                                codeList.Remove(address);

                            codeList.Add(address, code);
                        }
                        continue;
                    }

                    if (reader.Name.Equals("breakpoint"))
                    {
                        string address = reader.GetAttribute("address");
                        BreakPoints.Add(address);
                        continue;
                    }

                    if (reader.Name.Equals("project"))
                    {
                        String version = reader.GetAttribute("version");

                        if (version != null)
                            Enum.TryParse<BoardVersion>(version, out Version);
                        continue;
                    }

                    if (reader.Name.Equals("watch"))
                    {
                        int address = Convert.ToInt32(reader.GetAttribute("address"), 16);
                        string name = reader.GetAttribute("label");

                        WatchedMemory mem = new(name, address, 0, 0);
                        if (watchList.ContainsKey(address))
                            watchList.Remove(address);

                        watchList.Add(address, mem);
                    }
                }
            }

            reader.Close();
        }

        public void LoadMemory(String address, String values)
        {
            if (_kernel == null)
                throw new InvalidOperationException("Kernel is undefined");

            int addr = Convert.ToInt32(address.Replace("$", ""), 16);

            // Each byte is written as 3 characters (2 Hex and a space)
            if (values.Length % 3 == 0)
                for (int i = 0; i < values.Length / 3; ++i)
                    _kernel.MemMgr.WriteByte(addr++, Convert.ToByte(values.Substring(i * 3, 2), 16));
        }
    }
}
