using System;
using System.Collections.Generic;

using Gtk;


namespace FoenixCore.Simulator.FileFormat
{
    public class ResourceChecker
    {
        public enum ResourceType
        {
            raw,
            bitmap,
            tileset,
            tilemap,
            sprite,
            lut
        }

        public class Resource
        {
            public int StartAddress = 0;
            public int Length = 0;
            public string Name;
            public string SourceFile;
            public ResourceType FileType = ResourceType.raw;
        }

        readonly List<Resource> resources = new();

        public bool Add(Resource resource)
        {
            // Check if there is an overlap
            foreach (Resource res in resources)
            {
                int beginRange = res.StartAddress;
                int endRange = res.StartAddress + res.Length;

                if (resource.StartAddress >= beginRange && resource.StartAddress < endRange ||
                    (resource.StartAddress + resource.Length) > beginRange && (resource.StartAddress + resource.Length) < endRange)
                {
                    using (var md = new MessageDialog(null, DialogFlags.Modal | DialogFlags.DestroyWithParent,
                            MessageType.Warning, ButtonsType.YesNo,
                            string.Format(
                            "This image overlap resource {0} which starts at {1:X6} and ends at {2:X6}.\r\nDo you want to load it anyway?",
                            res.Name, res.StartAddress, res.StartAddress + res.Length))) {
                        md.Title = "Overlap Detected";
                        if (md.Run() == (int)ResponseType.No)
                            return false;
                    }
                }
            }

            resources.Add(resource);

            return true;
        }

        public void Clear()
        {
            resources.Clear();
        }

        public List<Resource> Items
        {
            get => resources;
        }

        public Resource Find(ResourceType resType, int startAddress)
        {
            foreach (Resource res in Items)
                if (res.FileType == resType && res.StartAddress == startAddress)
                    return res;

            return null;
        }
    }
}
