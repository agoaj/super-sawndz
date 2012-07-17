using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.SSBBTypes;
using System.ComponentModel;
using System.IO;
using BrawlLib.IO;
using BrawlLib.Wii.Animations;
using System.Windows.Forms;
using BrawlBox;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class PhysicsNode : ARCEntryNode
    {
        public PhysicsHeader* Header { get { return (PhysicsHeader*)WorkingUncompressed.Address; } }
        public override ResourceType ResourceType { get { return ResourceType.Unknown; } }

        [Category("Offsets")]
        public int Unknown2 { get { return Header->_unk2; } }
        [Category("Offsets")]
        public int Unknown3 { get { return Header->_unk3; } }
        [Category("Offsets")]
        public int Unknown4 { get { return Header->_unk4; } }
        [Category("Offsets")]
        public int Unknown5 { get { return Header->_unk5; } }
        [Category("Offsets")]
        public int Unknown6 { get { return Header->_unk6; } }
        [Category("Offsets")]
        public int Unknown7 { get { return Header->_unk7; } }
        [Category("Offsets")]
        public int Unknown8 { get { return Header->_unk8; } }
        [Category("Offsets")]
        public int Unknown9 { get { return Header->_unk9; } }
        [Category("Offsets")]
        public int Unknown10 { get { return Header->_unk10; } }
        [Category("Offsets")]
        public uint Unknown11 { get { return Header->_unk11; } }
        
        protected override bool OnInitialize()
        {
            base.OnInitialize();

            _name = Header->Name;

            return true;
        }

        protected override void OnPopulate()
        {
            new ClassNamesNode() { Header = Header->ClassNames }.Initialize(this, Header->ClassNamesData, 0);
            new DataNode() { Header = Header->Data }.Initialize(this, Header->DataData, 0);
            new DataNode() { Header = Header->Types }.Initialize(this, Header->TypesData, 0);
        }

        protected override int OnCalculateSize(bool force)
        {
            return base.OnCalculateSize(force);
        }

        protected internal override void OnRebuild(VoidPtr address, int length, bool force)
        {
            base.OnRebuild(address, length, force);
        }

        internal static ResourceNode TryParse(DataSource source) { return ((PhysicsHeader*)source.Address)->_tag1 == PhysicsHeader.Tag1 ? new PhysicsNode() : null; }
    }

    public unsafe class ClassNamesNode : ResourceNode
    {
        internal VoidPtr Base { get { return (VoidPtr)WorkingUncompressed.Address; } }
        internal PhysicsOffsetSection Header;

        public override ResourceType ResourceType { get { return ResourceType.Unknown; } }

        [Category("Offsets")]
        public int Unknown { get { return Header._unk0; } }
        [Category("Offsets")]
        public uint MainOffset { get { return Header._dataOffset; } }

        [Category("Offsets")]
        public uint Offset1 { get { return Header._dataOffset1; } }
        [Category("Offsets")]
        public uint Offset2 { get { return Header._dataOffset2; } }
        [Category("Offsets")]
        public uint Offset3 { get { return Header._dataOffset3; } }
        [Category("Offsets")]
        public uint Offset4 { get { return Header._dataOffset4; } }
        [Category("Offsets")]
        public uint Offset5 { get { return Header._dataOffset5; } }
        [Category("Offsets")]
        public uint DataLength { get { return Header._dataLength; } }

        protected override bool OnInitialize()
        {
            base.OnInitialize();

            _name = Header.Name;

            SetSizeInternal((int)DataLength);

            return true;
        }

        protected override void OnPopulate()
        {
            byte* header = (byte*)Base;
            int size = 0, len = 0;
            while (size <= DataLength && *header != 0xFF)
            {
                len = 5 + ((PhysicsClassName*)header)->_value.Length + 1;
                new ClassNameEntryNode().Initialize(this, header, len);
                header += len;
                size += len;
            }
        }

        protected override int OnCalculateSize(bool force)
        {
            return base.OnCalculateSize(force);
        }

        protected internal override void OnRebuild(VoidPtr address, int length, bool force)
        {
            base.OnRebuild(address, length, force);
        }
    }

    public unsafe class DataNode : ResourceNode
    {
        internal VoidPtr Base { get { return (VoidPtr)WorkingUncompressed.Address; } }
        internal PhysicsOffsetSection Header;
        public List<VoidPtr> _indexAddrs;
        public List<uint> _counts;
        
        public List<int>[] _indices;
        public int[] Indices1 { get { return _indices[0].ToArray(); } }
        public int[] Indices2 { get { return _indices[1].ToArray(); } }
        public int[] Indices3 { get { return _indices[2].ToArray(); } }
        public int[] Indices4 { get { return _indices[3].ToArray(); } }
        public int[] Indices5 { get { return _indices[4].ToArray(); } }

        public override ResourceType ResourceType { get { return ResourceType.Unknown; } }

        [Category("Offsets")]
        public int Unknown { get { return Header._unk0; } }
        [Category("Offsets")]
        public uint MainOffset { get { return Header._dataOffset; } }

        [Category("Offsets")]
        public uint Offset1 { get { return Header._dataOffset1; } }
        [Category("Offsets")]
        public uint Offset2 { get { return Header._dataOffset2; } }
        [Category("Offsets")]
        public uint Offset3 { get { return Header._dataOffset3; } }
        [Category("Offsets")]
        public uint Offset4 { get { return Header._dataOffset4; } }
        [Category("Offsets")]
        public uint Offset5 { get { return Header._dataOffset5; } }
        [Category("Offsets")]
        public uint DataLength { get { return Header._dataLength; } }

        protected override bool OnInitialize()
        {
            base.OnInitialize();

            _name = Header.Name;

            SetSizeInternal((int)DataLength);

            _indexAddrs = new List<VoidPtr>();
            _counts = new List<uint>();
            _indices = new List<int>[5];

            _indices[1] = new List<int>();
            _indices[2] = new List<int>();
            _indices[0] = new List<int>();
            _indices[4] = new List<int>();
            _indices[3] = new List<int>();

            if (Offset2 - Offset1 != 0)
            {
                _indexAddrs.Add(Base + Offset1);
                _counts.Add((Offset2 - Offset1) / 4);
            }
            if (Offset3 - Offset2 != 0)
            {
                _indexAddrs.Add(Base + Offset2);
                _counts.Add((Offset3 - Offset2) / 4);
            }
            if (Offset4 - Offset3 != 0)
            {
                _indexAddrs.Add(Base + Offset3);
                _counts.Add((Offset4 - Offset3) / 4);
            }
            if (Offset5 - Offset4 != 0)
            {
                _indexAddrs.Add(Base + Offset4);
                _counts.Add((Offset5 - Offset4) / 4);
            }
            if (DataLength - Offset5 != 0)
            {
                _indexAddrs.Add(Base + Offset5);
                _counts.Add((DataLength - Offset5) / 4);
            }
            int i = 0;
            foreach (VoidPtr ptr in _indexAddrs)
            {
                bint* addr = (bint*)ptr;
                for (int x = 0; x < _counts[i]; x++) 
                {
                    if (*addr != -1)
                        _indices[i].Add(*addr); 
                    addr++; 
                }
                i++;
            }
            return false;
        }

        protected override void OnPopulate()
        {

        }

        protected override int OnCalculateSize(bool force)
        {
            return base.OnCalculateSize(force);
        }

        protected internal override void OnRebuild(VoidPtr address, int length, bool force)
        {
            base.OnRebuild(address, length, force);
        }
    }

    public unsafe class ClassNameEntryNode : ResourceNode
    {
        internal PhysicsClassName* Header { get { return (PhysicsClassName*)WorkingUncompressed.Address; } }
        public override ResourceType ResourceType { get { return ResourceType.Unknown; } }

        int _value;
        byte _unk2;
        public int Unk1 { get { return _value; } set { _value = value; SignalPropertyChange(); } }
        public byte Unk2 { get { return _unk2; } set { _unk2 = value; SignalPropertyChange(); } }

        protected override bool OnInitialize()
        {
            _name = Header->_value;
            _value = Header->_unk1;
            _unk2 = Header->_unk2;
            return false;
        }
    }
}
