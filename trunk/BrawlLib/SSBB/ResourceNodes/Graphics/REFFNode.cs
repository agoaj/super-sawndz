using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.SSBBTypes;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class REFFNode : ARCEntryNode
    {
        internal REFF* Header { get { return (REFF*)WorkingUncompressed.Address; } }
        public override ResourceType ResourceType { get { return ResourceType.REFF; } }

        private int _unk1, _unk2, _unk3, _dataLen, _dataOff;
        private int _TableLen;
        private short _TableEntries;
        private short _TableUnk1;

        [Category("REFF Data")]
        public int DataLength { get { return _dataLen; } }
        [Category("REFF Data")]
        public int DataOffset { get { return _dataOff; } }
        [Category("REFF Data")]
        public int Unknown1 { get { return _unk1; } set { _unk1 = value; SignalPropertyChange(); } }
        [Category("REFF Data")]
        public int Unknown2 { get { return _unk2; } set { _unk2 = value; SignalPropertyChange(); } }
        [Category("REFF Data")]
        public int Unknown3 { get { return _unk3; } set { _unk3 = value; SignalPropertyChange(); } }

        [Category("REFF Object Table")]
        public int Length { get { return _TableLen; } }
        [Category("REFF Object Table")]
        public short NumEntries { get { return _TableEntries; } }
        [Category("REFF Object Table")]
        public short Unk1 { get { return _TableUnk1; } set { _TableUnk1 = value; SignalPropertyChange(); } }

        protected override bool OnInitialize()
        {
            base.OnInitialize();

            REFF* header = Header;

            _name = header->IdString;
            _dataLen = header->_dataLength;
            _dataOff = header->_dataOffset;
            _unk1 = header->_unk1;
            _unk2 = header->_unk2;
            _unk3 = header->_unk3;

            REFTypeObjectTable* objTable = header->Table;
            _TableLen = (int)objTable->_length;
            _TableEntries = (short)objTable->_entries;
            _TableUnk1 = (short)objTable->_unk1;

            return header->Table->_entries > 0;
        }

        protected override void OnPopulate()
        {
            REFTypeObjectTable* table = Header->Table;
            REFTypeObjectEntry* Entry = table->First;
            for (int i = 0; i < table->_entries; i++, Entry = Entry->Next)
                new REFFEntryNode() { _name = Entry->Name, _offset = (int)Entry->DataOffset, _length = (int)Entry->DataLength }.Initialize(this, new DataSource((byte*)table->Address + Entry->DataOffset, (int)Entry->DataLength));
        }
        int tableLen = 0;
        protected override int OnCalculateSize(bool force)
        {
            int size = 0x60;
            tableLen = 0x9;
            foreach (ResourceNode n in Children)
            {
                tableLen += n.Name.Length + 11;
                size += n.CalculateSize(force);
            }
            return size + (tableLen = tableLen.Align(4));
        }
        protected internal override void OnRebuild(VoidPtr address, int length, bool force)
        {
            REFF* header = (REFF*)address;
            header->_unk1 = 0;
            header->_unk2 = 0;
            header->_unk3 = 0;
            header->_dataLength = length - 0x18;
            header->_dataOffset = 0x48;
            header->_header._tag = header->_tag = REFF.Tag;
            header->_header._endian = -2;
            header->_header._version = 0x0700;
            header->_header._firstOffset = 0x10;
            header->_header._numEntries = 1;
            header->IdString = Name;

            REFTypeObjectTable* table = (REFTypeObjectTable*)((byte*)header + header->_dataOffset + 0x18);
            table->_entries = (short)Children.Count;
            table->_unk1 = 0;
            table->_length = tableLen;

            REFTypeObjectEntry* entry = table->First;
            int offset = tableLen;
            foreach (ResourceNode n in Children)
            {
                entry->Name = n.Name;
                entry->DataOffset = offset;
                entry->DataLength = n._calcSize - 0x20;
                n.Rebuild((VoidPtr)table + offset, n._calcSize, force);
                offset += n._calcSize;
                entry = entry->Next;
            }
        }

        internal static ResourceNode TryParse(DataSource source) { return ((REFF*)source.Address)->_tag == REFF.Tag ? new REFFNode() : null; }
    }
    public unsafe class REFFEntryNode : ResourceNode
    {
        internal REFFData* Header { get { return (REFFData*)WorkingUncompressed.Address; } }

        [Category("REFF Entry")]
        public int REFFOffset { get { return _offset; } }
        [Category("REFF Entry")]
        public int DataLength { get { return _length; } }
        
        //public uint unk1;
        //public short unk2;
        //public short unk3;
        //public byte unk4;
        //public short unk5;
        //public byte unk6;
        //public byte unk7;
        //public byte unk8;
        //public byte unk9;
        //public byte unk10;

        [Flags]
        public enum REFFflags
        {
            None = 0x0,
            Unk1 = 0x1,
            Unk2 = 0x2,
            HasParent = 0x4,
            Unk4 = 0x8,
            Unk5 = 0x10,
            Unk6 = 0x20,
            IsStandalone = 0x40,
            HasChildren = 0x80,
        }

        [Category("REFF Data")]
        public uint Unk1 { get { return Header->_unk1; } }
        [Category("REFF Data")]
        public int Unk2 { get { return Header->_unk2; } }
        [Category("REFF Data")]
        public int Unk3 { get { return Header->_unk3; } }
        [Category("REFF Data")]
        public int HeaderLength { get { return Header->_headerLen; } }
        [Category("REFF Data")]
        public bool EFLSEntry { get { return Header->_control != 0; } }
        [Category("REFF Data")]
        public REFFflags Flags { get { return (REFFflags)Header->_flags; } }
        [Category("REFF Data")]
        public int Unk7 { get { return Header->_unk7; } }
        [Category("REFF Data")]
        public int Unk8 { get { return Header->_unk8; } }
        [Category("REFF Data")]
        public int Unk9 { get { return Header->_unk9; } }
        [Category("REFF Data")]
        public int Unk10 { get { return Header->_unk10; } }
        [Category("REFF Data")]
        public int Unk11 { get { return Header->_unk11; } }
        [Category("REFF Data")]
        public int Unk12 { get { return Header->_unk12; } }
        [Category("REFF Data")]
        public int Unk13a { get { return Header->_unk13a; } }
        [Category("REFF Data")]
        public int Unk13b { get { return Header->_unk13b; } }
        [Category("REFF Data")]
        public int Unk14 { get { return Header->_unk14; } }
        [Category("REFF Data")]
        public float Unk15 { get { return Header->_unk15; } }
        [Category("REFF Data")]
        public int Unk16 { get { return Header->_unk16; } }
        [Category("REFF Data")]
        public int Unk17 { get { return Header->_unk17; } }
        [Category("REFF Data")]
        public int Unk18 { get { return Header->_unk18; } }
        [Category("REFF Data")]
        public int Unk19 { get { return Header->_unk19; } }
        [Category("REFF Data")]
        public int Unk20 { get { return Header->_unk20; } }
        [Category("REFF Data")]
        public float Unk21 { get { return Header->_unk21; } }
        [Category("REFF Data")]
        public float Unk22 { get { return Header->_unk22; } }
        [Category("REFF Data")]
        public float Unk23 { get { return Header->_unk23; } }

        public int _offset;
        public int _length;
        
        protected override bool OnInitialize()
        {
            base.OnInitialize();

            //REFFData* data = Header;

            //unk1 = data->_unk1;
            //unk2 = data->_unk2;
            //unk3 = data->_unk3;
            //unk4 = data->_unk4;
            //unk5 = data->_control;
            //unk6 = data->_unk6;
            //unk7 = data->_unk7;
            //unk8 = data->_unk8;
            //unk9 = data->_unk9;
            //unk10 = data->_unk10;

            return false;
        }
    }
}
