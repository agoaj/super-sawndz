using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using BrawlLib.SSBBTypes;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.ResourceNodes
{
    public class MoveDefCharSpecificNode : MoveDefExternalNode { }
    public unsafe class MoveDefParamListNode : MoveDefCharSpecificNode
    {
        internal FDefListOffset* Header { get { return (FDefListOffset*)WorkingUncompressed.Address; } }
        internal int i = 0;

        [Category("List Offset")]
        public int DataOffset { get { return Header->_startOffset; } }
        [Category("List Offset")]
        public int Count { get { return Header->_listCount; } }

        protected override bool OnInitialize()
        {
            base.OnInitialize();
            return Count > 0;
        }

        protected override void OnPopulate()
        {
            int size = Root.GetSize(DataOffset) / Count;
            for (int i = 0; i < Count; i++)
                new MoveDefSectionParamNode() { _name = "Part" + i }.Initialize(this, BaseAddress + DataOffset + i * size, size);
        }

        protected override int OnCalculateSize(bool force)
        {
            _lookupCount = (Children.Count > 0 ? 1 : 0);
            int size = 8;
            foreach (MoveDefSectionParamNode p in Children)
                size += p.CalculateSize(true);
            return size;
        }

        protected internal override void OnRebuild(VoidPtr address, int length, bool force)
        {
            VoidPtr addr = address;
            foreach (MoveDefSectionParamNode p in Children)
            {
                p.Rebuild(addr, p._calcSize, true);
                addr += p._calcSize;
            }
            _entryOffset = addr;
            FDefListOffset* header = (FDefListOffset*)addr;
            if (Children.Count > 0)
            {
                header->_startOffset = (int)address - (int)_rebuildBase;
                _lookupOffsets.Add((int)header->_startOffset.Address - (int)_rebuildBase);
            }
            header->_listCount = Children.Count;
        }
    }
    public unsafe class DededeHitDataList : MoveDefCharSpecificNode
    {
        internal FDefListOffset* Header { get { return (FDefListOffset*)WorkingUncompressed.Address; } }
        internal int i = 0;
        
        [Category("List Offset")]
        public int DataOffset1 { get { return Header[0]._startOffset; } }
        [Category("List Offset")]
        public int Count1 { get { return Header[0]._listCount; } }
        [Category("List Offset")]
        public int DataOffset2 { get { return Header[1]._startOffset; } }
        [Category("List Offset")]
        public int Count2 { get { return Header[1]._listCount; } }
        [Category("List Offset")]
        public int DataOffset3 { get { return Header[2]._startOffset; } }
        [Category("List Offset")]
        public int Count3 { get { return Header[2]._listCount; } }

        protected override bool OnInitialize()
        {
            base.OnInitialize();
            _name = "HitDataLists";
            return DataOffset1 > 0 || DataOffset2 > 0 || DataOffset3 > 0;
        }

        protected override void OnPopulate()
        {
            if (DataOffset1 > 0)
                new MoveDefHitDataListNode() { _name = "HitDataList1", offsetID = 0 }.Initialize(this, BaseAddress + DataOffset1, 0);
            if (DataOffset2 > 0)
                new MoveDefHitDataListNode() { _name = "HitDataList2", offsetID = 1 }.Initialize(this, BaseAddress + DataOffset2, 0);
            if (DataOffset3 > 0)
                new MoveDefHitDataListNode() { _name = "HitDataList3", offsetID = 2 }.Initialize(this, BaseAddress + DataOffset3, 0);
        }

        protected override int OnCalculateSize(bool force)
        {
            _lookupCount = Children.Count;
            int size = 24;
            foreach (MoveDefHitDataListNode p in Children)
                size += p.CalculateSize(true);
            return size;
        }

        protected internal override void OnRebuild(VoidPtr address, int length, bool force)
        {
            VoidPtr addr = address;
            foreach (MoveDefHitDataListNode p in Children)
            {
                p.Rebuild(addr, p._calcSize, true);
                addr += p._calcSize;
            }
            _entryOffset = addr;
            FDefListOffset* header = (FDefListOffset*)addr;
            foreach (MoveDefHitDataListNode d in Children)
            {
                (&header[d.offsetID])->_listCount = d.Children.Count;
                (&header[d.offsetID])->_startOffset = (int)d._entryOffset - (int)_rebuildBase;
                _lookupOffsets.Add((&header[d.offsetID])->_startOffset.Address - (int)_rebuildBase);
            }
        }
    }
    public unsafe class HitDataListOffsetNode : MoveDefCharSpecificNode
    {
        internal FDefListOffset* Header { get { return (FDefListOffset*)WorkingUncompressed.Address; } }
        internal int i = 0;
        
        [Category("List Offset")]
        public int DataOffset { get { return Header->_startOffset; } }
        [Category("List Offset")]
        public int Count { get { return Header->_listCount; } }

        protected override bool OnInitialize()
        {
            base.OnInitialize();
            return DataOffset > 0;
        }

        protected override void OnPopulate()
        {
            for (int i = 0; i < Count; i++)
                new MoveDefHitDataNode() { _extOverride = true }.Initialize(this, BaseAddress + DataOffset + i * 32, 32);
        }

        protected override int OnCalculateSize(bool force)
        {
            _lookupCount = (Children.Count > 0 ? 1 : 0);
            return _entryLength = 8 + 32 * Children.Count;
        }

        protected internal override void OnRebuild(VoidPtr address, int length, bool force)
        {
            hitData* data = (hitData*)address;
            foreach (MoveDefHitDataNode h in Children)
                h.Rebuild(data++, 32, true);
            _entryOffset = data;
            FDefListOffset* header = (FDefListOffset*)data;
            if (Children.Count > 0)
            {
                header->_listCount = Children.Count;
                header->_startOffset = (int)address - (int)_rebuildBase;
                _lookupOffsets.Add(header->_startOffset.Address - (int)_rebuildBase);
            }
        }
    }
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct SZerosuitExtraParams8
    {
        public bint count;
        public bint offset;
        public fixed int pad[24];
    }
    public unsafe class SZerosuitExtraParams8Node : MoveDefCharSpecificNode
    {
        internal SZerosuitExtraParams8* Header { get { return (SZerosuitExtraParams8*)WorkingUncompressed.Address; } }
        public override ResourceType ResourceType { get { return ResourceType.Unknown; } }

        int count, offset;

        [Category("Extra Offset 8")]
        public int Count { get { return count; } }
        [Category("Extra Offset 8")]
        public int Offset { get { return offset; } }

        protected override bool OnInitialize()
        {
            _name = "Extra Offset 8";
            count = Header->count;
            offset = Header->offset;
            return false;
        }

        protected override int OnCalculateSize(bool force)
        {
            _lookupCount = 1;
            return 32;
        }

        protected internal override void OnRebuild(VoidPtr address, int length, bool force)
        {
            _entryOffset = address;
            SZerosuitExtraParams8* data = (SZerosuitExtraParams8*)address;
            data->count = 29;
            _lookupOffsets.Add((int)data->offset.Address - (int)_rebuildBase);
        }
    }
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct WarioExtraParams6
    {
        public byte _unk1;
        public byte _unk2;
        public byte _unk3;
        public byte _unk4;
        public bint _offset;
        public bint _unk5;
        public bint _unk6;
        public bfloat _unk7;
        public bint _unk8;
        public bfloat _unk9;
        public bfloat _unk10;
    }
    public unsafe class Wario6 : MoveDefCharSpecificNode
    {
        internal WarioExtraParams6* Header { get { return (WarioExtraParams6*)WorkingUncompressed.Address; } }
        public override ResourceType ResourceType { get { return ResourceType.Unknown; } }

        public byte _unk1;
        public byte _unk2;
        public byte _unk3;
        public byte _unk4;
        public int _Offset;
        public int _unk5;
        public int _unk6;
        public float _unk7;
        public int _unk8;
        public float _unk9;
        public float _unk10;

        [Category("Extra Offset 6")]
        public byte Unknown1 { get { return _unk1; } set { _unk1 = value; SignalPropertyChange(); } }
        [Category("Extra Offset 6")]
        public byte Unknown2 { get { return _unk2; } set { _unk2 = value; SignalPropertyChange(); } }
        [Category("Extra Offset 6")]
        public byte Unknown3 { get { return _unk3; } set { _unk3 = value; SignalPropertyChange(); } }
        [Category("Extra Offset 6")]
        public byte Unknown4 { get { return _unk4; } set { _unk4 = value; SignalPropertyChange(); } }
        [Category("Extra Offset 6")]
        public int Offset { get { return _Offset; } }
        [Category("Extra Offset 6")]
        public int Unknown5 { get { return _unk5; } set { _unk5 = value; SignalPropertyChange(); } }
        [Category("Extra Offset 6")]
        public int Unknown6 { get { return _unk6; } set { _unk6 = value; SignalPropertyChange(); } }
        [Category("Extra Offset 6")]
        public float Unknown7 { get { return _unk7; } set { _unk7 = value; SignalPropertyChange(); } }
        [Category("Extra Offset 6")]
        public int Unknown8 { get { return _unk8; } set { _unk8 = value; SignalPropertyChange(); } }
        [Category("Extra Offset 6")]
        public float Unknown9 { get { return _unk9; } set { _unk9 = value; SignalPropertyChange(); } }
        [Category("Extra Offset 6")]
        public float Unknown10 { get { return _unk10; } set { _unk10 = value; SignalPropertyChange(); } }

        protected override bool OnInitialize()
        {
            _name = "Extra Data 6";
            _unk1 = Header->_unk1;
            _unk2 = Header->_unk2;
            _unk3 = Header->_unk3;
            _unk4 = Header->_unk4;
            _Offset = Header->_offset;
            _unk5 = Header->_unk5;
            _unk6 = Header->_unk6;
            _unk7 = Header->_unk7;
            _unk8 = Header->_unk8;
            _unk9 = Header->_unk9;
            _unk10 = Header->_unk10;
            return false;
        }

        protected override int OnCalculateSize(bool force)
        {
            _lookupCount = 1;
            return 32;
        }

        protected internal override void OnRebuild(VoidPtr address, int length, bool force)
        {
            _entryOffset = address;
            WarioExtraParams6* header = (WarioExtraParams6*)address;
            header->_unk1 = _unk1;
            header->_unk2 = _unk2;
            header->_unk3 = _unk3;
            header->_unk4 = _unk4;
            header->_unk5 = _unk5;
            header->_unk6 = _unk6;
            header->_unk7 = _unk7;
            header->_unk8 = _unk8;
            header->_unk9 = _unk9;
            header->_unk10 = _unk10;

            _lookupOffsets.Add((int)header->_offset.Address - (int)_rebuildBase);
        }
    }
    public unsafe class Wario8 : MoveDefCharSpecificNode
    {
        internal bint* Header { get { return (bint*)WorkingUncompressed.Address; } }
        public override ResourceType ResourceType { get { return ResourceType.Unknown; } }

        [Category("Extra Offset 8")]
        public int Offset1 { get { return Header[0]; } }
        [Category("Extra Offset 8")]
        public int Offset2 { get { return Header[1]; } }
        
        protected override bool OnInitialize()
        {
            _name = "Bone Index Replacement";
            return Offset1 > 0 || Offset2 > 0;
        }

        protected override void OnPopulate()
        {
            if (Offset1 > 0)
                new MoveDefSectionParamNode() { _name = "Data1", offsetID = 0 }.Initialize(this, BaseAddress + Offset1, 0);
            if (Offset2 > 0)
                new MoveDefSectionParamNode() { _name = "Data2", offsetID = 1 }.Initialize(this, BaseAddress + Offset2, 0);
        }

        protected override int OnCalculateSize(bool force)
        {
            _lookupCount = Children.Count;
            return 8;
        }

        protected internal override void OnRebuild(VoidPtr address, int length, bool force)
        {
            VoidPtr addr = address;
            foreach (MoveDefSectionParamNode p in Children)
            {
                p.Rebuild(addr, p._calcSize, true);
                addr += p._calcSize;
            }
            _entryOffset = addr;
            bint* header = (bint*)address;
            foreach (MoveDefSectionParamNode p in Children)
            {
                header[p.offsetID] = (int)p._entryOffset - (int)p._rebuildBase;
                _lookupOffsets.Add((int)(&header[p.offsetID]) - (int)p._rebuildBase);
            }
        }
    }
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct YoshiExtraParams9
    {
        public bfloat _unk1;
        public bfloat _unk2;
        public bint _offset;
    }
    public unsafe class Yoshi9 : MoveDefCharSpecificNode
    {
        internal YoshiExtraParams9* Header { get { return (YoshiExtraParams9*)WorkingUncompressed.Address; } }
        public override ResourceType ResourceType { get { return ResourceType.Unknown; } }

        float unk1, unk2;
        int offset;
        
        [Category("Extra Offset 9")]
        public float Unknown1 { get { return unk1; } set { unk1 = value; SignalPropertyChange(); } }
        [Category("Extra Offset 9")]
        public float Unknown2 { get { return unk2; } set { unk2 = value; SignalPropertyChange(); } }
        [Category("Extra Offset 9")]
        public int Offset { get { return offset; } }

        protected override bool OnInitialize()
        {
            base.OnInitialize();
            if (_name == null)
                _name = "Extra Data " + offsetID;
            unk1 = Header->_unk1;
            unk2 = Header->_unk2;
            offset = Header->_offset;
            return Offset > 0;
        }

        protected override void OnPopulate()
        {
            new MoveDefSectionParamNode() { _name = "Data" }.Initialize(this, BaseAddress + Offset, 0);
        }

        protected override int OnCalculateSize(bool force)
        {
            _lookupCount = Children.Count;
            return 12 + Children.Count > 0 ? Children[0].CalculateSize(true) : 0;
        }

        protected internal override void OnRebuild(VoidPtr address, int length, bool force)
        {
            VoidPtr addr = address;
            if (Children.Count > 0)
            {
                Children[0].Rebuild(addr, Children[0]._calcSize, true);
                addr += Children[0]._calcSize;
            }
            _entryOffset = addr;
            YoshiExtraParams9* header = (YoshiExtraParams9*)address;
            if (Children.Count > 0)
            {
                MoveDefEntryNode p = Children[0] as MoveDefEntryNode;
                header->_offset = (int)p._entryOffset - (int)p._rebuildBase;
                _lookupOffsets.Add((int)header->_offset.Address - (int)p._rebuildBase);
            }
            header->_unk1 = unk1;
            header->_unk2 = unk2;
        }
    }
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct GWArticle6
    {
        public bfloat _unk1;
        public bint _unk2;
        public bint _unk3;
        public FDefListOffset _list;
    }
    public unsafe class GameWatchArticle6 : MoveDefCharSpecificNode
    {
        internal GWArticle6* Header { get { return (GWArticle6*)WorkingUncompressed.Address; } }
        internal int i = 0;

        float unk1;
        int unk2, unk3;

        [Category("Extra Offset 6")]
        public float Unknown1 { get { return unk1; } set { unk1 = value; SignalPropertyChange(); } }
        [Category("Extra Offset 6")]
        public int Unknown2 { get { return unk2; } set { unk2 = value; SignalPropertyChange(); } }
        [Category("Extra Offset 6")]
        public int Unknown3 { get { return unk3; } set { unk3 = value; SignalPropertyChange(); } }
        [Category("Extra Offset 6")]
        public int DataOffset { get { return Header->_list._startOffset; } }
        [Category("Extra Offset 6")]
        public int Count { get { return Header->_list._listCount; } }

        protected override bool OnInitialize()
        {
            base.OnInitialize();
            if (_name == null)
                _name = "Extra Data 6";
            unk1 = Header->_unk1;
            unk2 = Header->_unk2;
            unk3 = Header->_unk3;
            return Count > 0;
        }

        protected override void OnPopulate()
        {
            int size = Root.GetSize(DataOffset) / Count;
            for (int i = 0; i < Count; i++)
                new MoveDefSectionParamNode() { _name = "Part" + i }.Initialize(this, BaseAddress + DataOffset + i * size, size);
        }

        protected override int OnCalculateSize(bool force)
        {
            _lookupCount = (Children.Count > 0 ? 1 : 0);
            int size = 20;
            foreach (MoveDefSectionParamNode p in Children)
                size += p.CalculateSize(true);
            return size;
        }

        protected internal override void OnRebuild(VoidPtr address, int length, bool force)
        {
            VoidPtr addr = address;
            foreach (MoveDefSectionParamNode p in Children)
            {
                p.Rebuild(addr, p._calcSize, true);
                addr += p._calcSize;
            }
            _entryOffset = addr;
            GWArticle6* header = (GWArticle6*)addr;
            header->_unk1 = unk1;
            header->_unk2 = unk2;
            header->_unk3 = unk3;
            if (Children.Count > 0)
            {
                header->_list._startOffset = (int)address - (int)_rebuildBase;
                _lookupOffsets.Add((int)(&header->_list)->_startOffset.Address - (int)_rebuildBase);
            }
            header->_list._listCount = Children.Count;
        }
    }
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct p7r13
    {
        public FDefListOffset _list;
        public bfloat _unk1;
        public bfloat _unk2;
        public bfloat _unk3;
        public bfloat _unk4;
        public bfloat _unk5;
        public bfloat _unk6;
    }
    public unsafe class Pit7Robot13Node : MoveDefCharSpecificNode
    {
        internal p7r13* Header { get { return (p7r13*)WorkingUncompressed.Address; } }
        internal int i = 0;

        float unk1, unk2, unk3, unk4, unk5, unk6;

        [Category("Extra Offset")]
        public int DataOffset { get { return Header->_list._startOffset; } }
        [Category("Extra Offset")]
        public int Count { get { return Header->_list._listCount; } }
        [Category("Extra Offset")]
        public float Unknown1 { get { return unk1; } set { unk1 = value; SignalPropertyChange(); } }
        [Category("Extra Offset")]
        public float Unknown2 { get { return unk2; } set { unk2 = value; SignalPropertyChange(); } }
        [Category("Extra Offset")]
        public float Unknown3 { get { return unk3; } set { unk3 = value; SignalPropertyChange(); } }
        [Category("Extra Offset")]
        public float Unknown4 { get { return unk4; } set { unk4 = value; SignalPropertyChange(); } }
        [Category("Extra Offset")]
        public float Unknown5 { get { return unk5; } set { unk5 = value; SignalPropertyChange(); } }
        [Category("Extra Offset")]
        public float Unknown6 { get { return unk6; } set { unk6 = value; SignalPropertyChange(); } }

        protected override bool OnInitialize()
        {
            base.OnInitialize();
            if (_name == null)
                _name = "HitDataList" + offsetID;
            unk1 = Header->_unk1;
            unk2 = Header->_unk2;
            unk3 = Header->_unk3;
            unk4 = Header->_unk4;
            unk5 = Header->_unk5;
            unk6 = Header->_unk6;
            return Count > 0;
        }

        protected override void OnPopulate()
        {
            int size = Root.GetSize(DataOffset) / Count;
            for (int i = 0; i < Count; i++)
                new MoveDefHitDataNode() { _name = "HitData" + i }.Initialize(this, BaseAddress + DataOffset + i * size, size);
        }

        protected override int OnCalculateSize(bool force)
        {
            _lookupCount = (Children.Count > 0 ? 1 : 0);
            int size = 32;
            foreach (MoveDefHitDataNode p in Children)
                size += p.CalculateSize(true);
            return size;
        }

        protected internal override void OnRebuild(VoidPtr address, int length, bool force)
        {
            VoidPtr addr = address;
            foreach (MoveDefHitDataNode p in Children)
            {
                p.Rebuild(addr, p._calcSize, true);
                addr += p._calcSize;
            }
            _entryOffset = addr;
            p7r13* header = (p7r13*)addr;
            header->_unk1 = unk1;
            header->_unk2 = unk2;
            header->_unk3 = unk3;
            header->_unk4 = unk4;
            header->_unk5 = unk5;
            header->_unk6 = unk6;
            if (Children.Count > 0)
            {
                header->_list._startOffset = (int)address - (int)_rebuildBase;
                _lookupOffsets.Add((int)(&header->_list)->_startOffset.Address - (int)_rebuildBase);
            }
            header->_list._listCount = Children.Count;
        }
    }
    public unsafe class ActionOffsetNode : MoveDefCharSpecificNode
    {
        internal bint* Header { get { return (bint*)WorkingUncompressed.Address; } }

        [Category("Extra Offset")]
        public int DataOffset { get { return *Header; } }

        protected override bool OnInitialize()
        {
            base.OnInitialize();
            if (_name == null)
                _name = "Extra Data " + offsetID;
            return DataOffset > 0;
        }

        protected override void OnPopulate()
        {
            new MoveDefActionNode("Action", false, this).Initialize(this, BaseAddress + DataOffset, 0);
        }

        protected override int OnCalculateSize(bool force)
        {
            _lookupCount = (Children.Count > 0 ? 1 : 0);
            int size = 4;
            if (Children.Count > 0)
                size += Children[0].CalculateSize(true);
            return size;
        }

        protected internal override void OnRebuild(VoidPtr address, int length, bool force)
        {
            VoidPtr addr = address;
            if (Children.Count > 0)
            {
                Children[0].Rebuild(addr, Children[0]._calcSize, true);
                addr += Children[0]._calcSize;
            }
            _entryOffset = addr;
            bint* header = (bint*)addr;
            if (Children.Count > 0)
            {
                *header = (int)(Children[0] as MoveDefEntryNode)._entryOffset - (int)_rebuildBase;
                _lookupOffsets.Add((int)header - (int)_rebuildBase);
            }
        }
    }
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct actionOffFlags
    {
        public bint _offset;
        public bshort _unk1;
        public bshort _unk2;
    }
    public unsafe class SecondaryActionOffsetNode : MoveDefCharSpecificNode
    {
        internal actionOffFlags* Header { get { return (actionOffFlags*)WorkingUncompressed.Address; } }

        short unk1, unk2;

        [Category("Extra Offset")]
        public int DataOffset { get { return Header->_offset; } }
        [Category("Extra Offset")]
        public short Unknown1 { get { return unk1; } set { unk1 = value; SignalPropertyChange(); } }
        [Category("Extra Offset")]
        public short Unknown2 { get { return unk2; } set { unk2 = value; SignalPropertyChange(); } }
        
        protected override bool OnInitialize()
        {
            base.OnInitialize();
            if (_name == null)
                _name = "Extra Data " + offsetID;
            unk1 = Header->_unk1;
            unk2 = Header->_unk2;
            return false;
        }

        //protected override void OnPopulate()
        //{
        //    new MoveDefActionNode("Action", false, this).Initialize(this, BaseAddress + DataOffset, 0);
        //}

        protected override int OnCalculateSize(bool force)
        {
            //_lookupCount = (Children.Count > 0 ? 1 : 0);
            _lookupCount = 1;
            int size = 8;
            //if (Children.Count > 0)
            //    size += Children[0].CalculateSize(true);
            return size;
        }

        protected internal override void OnRebuild(VoidPtr address, int length, bool force)
        {
            VoidPtr addr = address;
            //if (Children.Count > 0)
            //{
            //    Children[0].Rebuild(addr, Children[0]._calcSize, true);
            //    addr += Children[0]._calcSize;
            //}
            _entryOffset = addr;
            actionOffFlags* header = (actionOffFlags*)addr;
            //if (Children.Count > 0)
            //{
                header->_offset = *(bint*)(Parent.Children[Index - 1] as ActionOffsetNode)._entryOffset;
                _lookupOffsets.Add((int)header->_offset.Address - (int)_rebuildBase);
            //}
            header->_unk1 = unk1;
            header->_unk2 = unk2;
        }
    }
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct f11f9w11a6
    {
        public bint _startOffset;
        public bint _listCount;
        public buint _flags;
    }
    public unsafe class Fox11Falco9Wolf11Article6Node : MoveDefCharSpecificNode
    {
        internal f11f9w11a6* Header { get { return (f11f9w11a6*)WorkingUncompressed.Address; } }

        [Category("Extra Offset 6")]
        public int DataOffset { get { return Header->_startOffset; } }
        [Category("Extra Offset 6")]
        public int Count { get { return Header->_listCount; } }
        [Category("Extra Offset 6"), TypeConverter(typeof(Bin32StringConverter))]
        public Bin32 Flags { get { return flags; } set { flags = value; SignalPropertyChange(); } }

        Bin32 flags;

        protected override bool OnInitialize()
        {
            base.OnInitialize();
            if (_name == null)
                _name = "Extra Data 6";
            flags = new Bin32(Header->_flags);
            return Count > 0;
        }

        protected override void OnPopulate()
        {
            int size = Root.GetSize(DataOffset) / Count;
            for (int i = 0; i < Count; i++)
                new MoveDefSectionParamNode() { _name = "Part" + i }.Initialize(this, BaseAddress + DataOffset + i * size, size);
        }

        protected override int OnCalculateSize(bool force)
        {
            _lookupCount = (Children.Count > 0 ? 1 : 0);
            int size = 8;
            foreach (MoveDefSectionParamNode p in Children)
                size += p.CalculateSize(true);
            return size;
        }

        protected internal override void OnRebuild(VoidPtr address, int length, bool force)
        {
            VoidPtr addr = address;
            foreach (MoveDefSectionParamNode p in Children)
            {
                p.Rebuild(addr, p._calcSize, true);
                addr += p._calcSize;
            }
            _entryOffset = addr;
            f11f9w11a6* header = (f11f9w11a6*)addr;
            if (Children.Count > 0)
            {
                header->_startOffset = (int)address - (int)_rebuildBase;
                _lookupOffsets.Add((int)header->_startOffset.Address - (int)_rebuildBase);
            }
            header->_listCount = Children.Count;
            header->_flags = flags.data;
        }
    }
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct f11f9w11a3
    {
        public bint _startOffset1;
        public bint _startOffset2;
        public bint _listCount;
    }
    public unsafe class Fox11Falco9Wolf11Article3Node : MoveDefCharSpecificNode
    {
        internal f11f9w11a3* Header { get { return (f11f9w11a3*)WorkingUncompressed.Address; } }
        
        [Category("Extra Offset 3")]
        public int DataOffset1 { get { return Header->_startOffset1; } }
        [Category("Extra Offset 3")]
        public int DataOffset2 { get { return Header->_startOffset2; } }
        [Category("Extra Offset 3")]
        public int Count { get { return Header->_listCount; } }

        protected override bool OnInitialize()
        {
            base.OnInitialize();
            if (_name == null)
                _name = "Extra Data 3";
            return DataOffset1 > 0 || DataOffset2 > 0;
        }

        protected override void OnPopulate()
        {
            int size = -1;

            if (DataOffset1 > 0)
                size = Root.GetSize(DataOffset1) / Count;

            if (size > 0)
            {
                MoveDefGroupNode g1 = new MoveDefGroupNode() { _name = "Data1", offsetID = 0 };
                g1.Initialize(this, BaseAddress + DataOffset1, size * Count);
                for (int i = 0; i < Count; i++)
                    new MoveDefSectionParamNode() { _name = "Part" + i }.Initialize(g1, BaseAddress + DataOffset1 + i * size, size);
            }

            size = -1;

            if (DataOffset2 > 0)
                size = Root.GetSize(DataOffset2) / Count;

            if (size > 0)
            {
                MoveDefGroupNode g2 = new MoveDefGroupNode() { _name = "Data2", offsetID = 1 };
                g2.Initialize(this, BaseAddress + DataOffset2, size * Count);
                for (int i = 0; i < Count; i++)
                    new MoveDefSectionParamNode() { _name = "Part" + i }.Initialize(g2, BaseAddress + DataOffset2 + i * size, size);
            }
        }

        protected override int OnCalculateSize(bool force)
        {
            _lookupCount = Children.Count;
            int size = 12;

            foreach (MoveDefEntryNode e in Children)
                foreach (MoveDefSectionParamNode p in e.Children)
                    size += p.CalculateSize(true);
            
            return size;
        }

        protected internal override void OnRebuild(VoidPtr address, int length, bool force)
        {
            VoidPtr addr = address;
            foreach (MoveDefEntryNode e in Children)
            {
                e._entryOffset = addr;
                foreach (MoveDefSectionParamNode p in e.Children)
                {
                    p.Rebuild(addr, p._calcSize, true);
                    addr += p._calcSize;
                }
                
            }
            _entryOffset = addr;
            bint* header = (bint*)addr;
            foreach (MoveDefEntryNode e in Children)
            {
                header[e.offsetID] = (int)e._entryOffset - (int)_rebuildBase;
                _lookupOffsets.Add((int)&header[e.offsetID] - (int)_rebuildBase);
                if (e.offsetID == 0)
                    header[2] = e.Children.Count;
            }
        }
    }
}