using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.SSBBTypes;
using System.ComponentModel;
using BrawlLib.Imaging;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class SCN0LightSetNode : SCN0EntryNode
    {
        internal SCN0LightSet* Data { get { return (SCN0LightSet*)WorkingUncompressed.Address; } }

        private string _ambientLight;
        private List<string> _entries = new List<string>();
        private short magic;
        private byte numLights, unk1;

        //[Category("Light Set")]
        //public short Magic { get { return magic; } set { magic = value; SignalPropertyChange(); } }
        [Category("Light Set")]
        public string Ambience { get { return _ambientLight; } set { _ambientLight = value; SignalPropertyChange(); } }
        [Category("Light Set")]
        public string[] Lights { get { return _entries.ToArray(); } set { _entries = value.ToList<string>(); SignalPropertyChange(); } }

        protected override bool OnInitialize()
        {
            base.OnInitialize();

            if (Data->_ambNameOffset != 0)
                _ambientLight = Data->AmbientString;

            magic = Data->_magic;
            numLights = Data->_numLights;
            unk1 = Data->_unk1;

            bint* strings = Data->StringOffsets;
            for (int i = 0; i < Data->_numLights; i++)
                _entries.Add(new String((sbyte*)strings + strings[i]));

            return false;
        }

        internal override void GetStrings(StringTable table)
        {
            if (Name != "<null>")
                table.Add(Name);
            else return;

            if (_ambientLight != null)
                table.Add(_ambientLight);

            foreach (string s in _entries)
                table.Add(s);
        }

        protected override int OnCalculateSize(bool force)
        {
            return SCN0LightSet.Size;
        }

        protected internal override void OnRebuild(VoidPtr address, int length, bool force)
        {
            SCN0LightSet* header = (SCN0LightSet*)address;

            header->_unk1 = 0;
            header->_magic = -1;
            header->_numLights = (byte)Lights.Length;
            header->_pad1 = header->_pad2 = header->_pad3 = header->_pad4 = -1;
        }

        protected internal override void PostProcess(VoidPtr scn0Address, VoidPtr dataAddress, StringTable stringTable)
        {
            base.PostProcess(scn0Address, dataAddress, stringTable);

            SCN0LightSet* header = (SCN0LightSet*)dataAddress;

            if (_ambientLight != null)
                header->AmbientStringAddress = stringTable[_ambientLight] + 4;
            else
                header->_ambNameOffset = 0;

            int i;
            bint* strings = header->StringOffsets;
            for (i = 0; i < _entries.Count; i++)
                strings[i] = (int)stringTable[_entries[i]] + 4 - (int)strings;
            while (i < 8)
                strings[i++] = 0;
        }
    }
}