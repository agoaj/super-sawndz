using System;
using System.Collections.Generic;
using BrawlLib.Wii;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class MSBinNode : ARCEntryNode
    {
        public override ResourceType ResourceType { get { return ResourceType.MSBin; } }
        internal List<string> _strings = new List<string>();

        //public List<string> Strings { get { return _strings; } set { _strings = value; } }

        protected override bool OnInitialize()
        {
            base.OnInitialize();

            _strings.Clear();

            byte* floor = (byte*)WorkingUncompressed.Address;
            int length = WorkingUncompressed.Length;
            bint* offsets = (bint*)floor;
            int index, last, current;

            for (index = 1, last = offsets[0]; last != length; index++)
            {
                current = offsets[index];
                if ((current < last) || (current > length))
                    break;

                _strings.Add(MSBinDecoder.DecodeString(floor + last, current - last));

                last = current;
            }
            return false;
        }

        protected override int OnCalculateSize(bool force)
        {
            int len = (_strings.Count + 1) << 2;
            foreach (string s in _strings)
                len += MSBinDecoder.GetStringSize(s);

            return len;
        }

        protected internal override void OnRebuild(VoidPtr address, int length, bool force)
        {
            bint* offsets = (bint*)address;
            byte* current = (byte*)(offsets + _strings.Count + 1);
            foreach (string s in _strings)
            {
                *offsets++ = (int)current - (int)address;
                //int len = MSBinDecoder.EncodeString(s, current);
                current += MSBinDecoder.EncodeString(s, current);
            }
            *offsets = (int)current - (int)address;
        }


        internal static ResourceNode TryParse(DataSource source)
        {
            int length = source.Length;
            bint* offsets = (bint*)source.Address;
            int index, last, current;

            for (index = 0, last = 0; last != length; index++)
            {
                current = offsets[index];
                if ((current < last) || (current > length))
                    return null;

                last = current;
            }

            return (offsets[0] == (index << 2)) ? new MSBinNode() : null;
        }
    }
}
