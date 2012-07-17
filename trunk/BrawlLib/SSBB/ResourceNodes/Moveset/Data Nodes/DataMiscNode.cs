using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.SSBBTypes;
using System.ComponentModel;
using System.IO;
using BrawlLib.IO;
using BrawlLib.Wii.Animations;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.OpenGL;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class MoveDefMiscNode : MoveDefEntryNode
    {
        internal MiscSection* Header { get { return (MiscSection*)WorkingUncompressed.Address; } }

        [Category("Misc Offsets")]
        public int UnknownSection1Offset { get { return Header->UnknownSection1Offset; } }
        [Category("Misc Offsets")]
        public int UnkBoneSectionOffset { get { return Header->UnkBoneSectionOffset; } }
        [Category("Misc Offsets")]
        public int UnkBoneSectionCount { get { return Header->UnkBoneSectionCount; } }
        [Category("Misc Offsets")]
        public int HurtBoxOffset { get { return Header->HurtBoxOffset; } }
        [Category("Misc Offsets")]
        public int HurtBoxCount { get { return Header->HurtBoxCount; } }
        [Category("Misc Offsets")]
        public int LedgegrabOffset { get { return Header->LedgegrabOffset; } }
        [Category("Misc Offsets")]
        public int LedgegrabCount { get { return Header->LedgegrabCount; } }
        [Category("Misc Offsets")]
        public int UnknownSection2Offset { get { return Header->UnknownSection2Offset; } }
        [Category("Misc Offsets")]
        public int UnknownSection2Count { get { return Header->UnknownSection2Count; } }
        [Category("Misc Offsets")]
        public int BoneRefOffset { get { return Header->BoneRef2Offset; } }
        [Category("Misc Offsets")]
        public int UnknownSection3Offset { get { return Header->UnknownSection3Offset; } }
        [Category("Misc Offsets")]
        public int SoundDataOffset { get { return Header->SoundDataOffset; } }
        [Category("Misc Offsets")]
        public int UnkSection5Offset { get { return Header->UnknownSection5Offset; } }
        [Category("Misc Offsets")]
        public int MultiJumpOffset { get { return Header->MultiJumpOffset; } }
        [Category("Misc Offsets")]
        public int GlideOffset { get { return Header->GlideOffset; } }
        [Category("Misc Offsets")]
        public int CrawlOffset { get { return Header->CrawlOffset; } }
        [Category("Misc Offsets")]
        public int UnknownSection9Offset { get { return Header->UnknownSection9Offset; } }
        [Category("Misc Offsets")]
        public int TetherOffset { get { return Header->TetherOffset; } }
        [Category("Misc Offsets")]
        public int UnknownSection12Offset { get { return Header->UnknownSection12Offset; } }

        public MoveDefMiscNode(string name) { _name = name; }

        protected override bool OnInitialize()
        {
            base.OnInitialize();
            return false;
        }

        public MoveDefMiscUnkSection9Node unk9;
        public MoveDefMiscUnkSection12Node unk12;
        public MoveDefTetherNode tether;
        public UnkSection1Node unkSection1;
        public MoveDefSectionUnk1Node unkBoneSection;
        public MoveDefMiscHurtBoxesNode hurtBoxes;
        public MoveDefLedgegrabsNode ledgeGrabs;
        public UnknownSection2Node unkSection2;
        public MoveDefBoneIndicesNode boneRefs;
        public UnknownSection3Node unkSection3;
        public MoveDefSoundDatasNode soundData;
        public UnkSection5Node unkSection5;
        public MoveDefMultiJumpNode multiJump;
        public MoveDefGlideNode glide;
        public MoveDefCrawlNode crawl;

        protected override void OnPopulate()
        {
            if (UnknownSection1Offset != 0)
                (unkSection1 = new UnkSection1Node()).Initialize(this, new DataSource(BaseAddress + UnknownSection1Offset, 0));
            if (UnkBoneSectionOffset != 0)
                (unkBoneSection = new MoveDefSectionUnk1Node(Header->UnkBoneSectionCount)).Initialize(this, new DataSource(BaseAddress + UnkBoneSectionOffset, 0));
            if (HurtBoxOffset != 0)
                (hurtBoxes = new MoveDefMiscHurtBoxesNode(Header->HurtBoxCount)).Initialize(this, new DataSource(BaseAddress + HurtBoxOffset, 0));
            if (LedgegrabOffset != 0)
                (ledgeGrabs = new MoveDefLedgegrabsNode(Header->LedgegrabCount)).Initialize(this, new DataSource(BaseAddress + LedgegrabOffset, 0));
            if (UnknownSection2Offset != 0)
                (unkSection2 = new UnknownSection2Node(UnknownSection2Count)).Initialize(this, new DataSource(BaseAddress + UnknownSection2Offset, 0));
            if (BoneRefOffset != 0)
                (boneRefs = new MoveDefBoneIndicesNode("Bone References", 10)).Initialize(this, new DataSource(BaseAddress + BoneRefOffset, 0));
            if (UnknownSection3Offset != 0)
                (unkSection3 = new UnknownSection3Node()).Initialize(this, new DataSource(BaseAddress + UnknownSection3Offset, 0));
            if (SoundDataOffset != 0)
                (soundData = new MoveDefSoundDatasNode()).Initialize(this, new DataSource(BaseAddress + SoundDataOffset, 0));
            if (UnkSection5Offset != 0)
                (unkSection5 = new UnkSection5Node()).Initialize(this, new DataSource(BaseAddress + UnkSection5Offset, 0));
            if (MultiJumpOffset != 0)
                (multiJump = new MoveDefMultiJumpNode()).Initialize(this, new DataSource(BaseAddress + MultiJumpOffset, 0));
            if (GlideOffset != 0)
                (glide = new MoveDefGlideNode()).Initialize(this, new DataSource(BaseAddress + GlideOffset, 0));
            if (CrawlOffset != 0)
                (crawl = new MoveDefCrawlNode()).Initialize(this, new DataSource(BaseAddress + CrawlOffset, 0));
            if (UnknownSection9Offset != 0)
                (unk9 = new MoveDefMiscUnkSection9Node()).Initialize(this, new DataSource(BaseAddress + UnknownSection9Offset, 0));
            if (TetherOffset != 0)
                (tether = new MoveDefTetherNode()).Initialize(this, new DataSource(BaseAddress + TetherOffset, 0));
            if (UnknownSection12Offset != 0)
                (unk12 = new MoveDefMiscUnkSection12Node()).Initialize(this, new DataSource(BaseAddress + UnknownSection12Offset, 0));
        }
    }

    public unsafe class MoveDefMiscUnkSection9Node : MoveDefEntryNode
    {
        internal FDefListOffset* Header { get { return (FDefListOffset*)WorkingUncompressed.Address; } }
        internal int i = 0;

        public int DataOffset { get { return Header->_startOffset; } }
        public int Count { get { return Header->_listCount; } }

        protected override bool OnInitialize()
        {
            base.OnInitialize();
            if (_name == null)
                _name = "Unknown Section 9";
            return Count > 0;
        }

        protected override void OnPopulate()
        {
            bint* addr = (bint*)(BaseAddress + DataOffset);
            for (int i = 0; i < Count; i++)
            {
                MoveDefOffsetNode offset = new MoveDefOffsetNode() { _name = "Entry" + i };
                offset.Initialize(this, addr++, 4);
                
                if (offset.DataOffset == 0)
                    continue;

                new MoveDefUnkSection9DataNode().Initialize(offset, new DataSource(BaseAddress + offset.DataOffset, 24));
            }
        }

        protected override int OnCalculateSize(bool force)
        {
            _lookupCount = (Children.Count > 0 ? 1 : 0); //main offset

            int size = 8;
            foreach (MoveDefOffsetNode offset in Children)
            {
                size += 4;
                if (offset.Children.Count > 0)
                {
                    _lookupCount++; //offset

                    MoveDefUnkSection9DataNode data = offset.Children[0] as MoveDefUnkSection9DataNode;

                    size += 24/* + data.Children.Count * 4*/;

                    if (data.Children.Count > 0)
                        _lookupCount++; //indices offset
                }
            }
            return size;
        }

        protected internal override void OnRebuild(VoidPtr address, int length, bool force)
        {
            _lookupOffsets = new List<int>();

            int dataOff = 0, offOff = 0, mainOff = 0;
            foreach (MoveDefOffsetNode r in Children)
            {
                mainOff += 4;
                if (r.Children.Count > 0)
                {
                    offOff += 24;
                    //dataOff += r.Children[0].Children.Count * 4;
                }
            }

            //indices
            //data
            //offsets
            //header

            bint* indices = (bint*)address;
            FDefMiscSection9Data* data = (FDefMiscSection9Data*)(address + dataOff);
            bint* offsets = (bint*)((VoidPtr)data + offOff);
            FDefListOffset* header = (FDefListOffset*)((VoidPtr)offsets + mainOff);

            _entryOffset = header;

            if (Children.Count > 0)
            {
                header->_startOffset = (int)offsets - (int)_rebuildBase;
                _lookupOffsets.Add((int)header->_startOffset.Address - (int)_rebuildBase); //main offset
            }

            header->_listCount = Children.Count;

            foreach (MoveDefOffsetNode offset in Children)
            {
                if (offset.Children.Count > 0)
                {
                    *offsets = (int)data - (int)_rebuildBase;

                    _lookupOffsets.Add((int)offsets - (int)_rebuildBase); //offset

                    offsets++;

                    MoveDefUnkSection9DataNode dataNode = offset.Children[0] as MoveDefUnkSection9DataNode;

                    data->_unk1 = dataNode._unk1;
                    data->_unk2 = dataNode._unk2;
                    data->_unk3 = dataNode._unk3;
                    data->_unk4 = dataNode._unk4;

                    data->_list._listCount = dataNode.Children.Count;
                    //data->_list._startOffset = (dataNode.Children.Count > 0 ? (int)indices - (int)_rebuildBase : 0);
                    data->_list._startOffset = (dataNode.Children.Count > 0 ? (int)(dataNode.Children[0] as MoveDefEntryNode)._entryOffset - (int)_rebuildBase : 0);

                    if (data->_list._startOffset > 0)
                        _lookupOffsets.Add((int)data->_list._startOffset.Address - (int)_rebuildBase);

                    data++;

                    //foreach (MoveDefBoneIndexNode b in dataNode.Children)
                    //{
                    //    b._entryOffset = indices;
                    //    *indices++ = b.boneIndex;
                    //}
                }
                else
                    *offsets++ = 0;
            }
        }
    }

    public unsafe class MoveDefMiscUnkSection12Node : MoveDefEntryNode
    {
        internal FDefListOffset* Header { get { return (FDefListOffset*)WorkingUncompressed.Address; } }
        internal int i = 0;

        public int DataOffset { get { return Header->_startOffset; } }
        public int Count { get { return Header->_listCount; } }

        protected override bool OnInitialize()
        {
            base.OnInitialize();
            if (_name == null)
                _name = "Unknown Section 12";
            return Count > 0;
        }

        protected override void OnPopulate()
        {
            bint* addr = (bint*)(BaseAddress + DataOffset);
            for (int i = 0; i < Count; i++)
                new MoveDefIndexNode() { _name = "Entry" + i }.Initialize(this, addr++, 4);
        }

        protected override int OnCalculateSize(bool force)
        {
            _lookupCount = (Children.Count > 0 ? 1 : 0);
            return 8 + Children.Count * 4;
        }

        protected internal override void OnRebuild(VoidPtr address, int length, bool force)
        {
            bint* addr = (bint*)address;
            foreach (MoveDefIndexNode b in Children)
                *addr++ = b.ItemIndex;

            FDefListOffset* header = (FDefListOffset*)addr;

            _entryOffset = header;

            header->_listCount = Children.Count;
            header->_startOffset = (Children.Count > 0 ? (int)address - (int)_rebuildBase : 0);

            if (header->_startOffset > 0)
                _lookupOffsets.Add((int)header->_startOffset.Address - (int)_rebuildBase);
        }
    }

    public unsafe class MoveDefUnkSection9DataNode : MoveDefEntryNode
    {
        internal FDefMiscSection9Data* Header { get { return (FDefMiscSection9Data*)WorkingUncompressed.Address; } }
        public override ResourceType ResourceType { get { return ResourceType.Unknown; } }

        public int _unk1, _unk4;
        public float _unk2, _unk3;

        [Category("Misc Section 9 Data")]
        public int Unk1 { get { return _unk1; } set { _unk1 = value; SignalPropertyChange(); } }
        [Category("Misc Section 9 Data")]
        public float Unk2 { get { return _unk2; } set { _unk2 = value; SignalPropertyChange(); } }
        [Category("Misc Section 9 Data")]
        public float Unk3 { get { return _unk3; } set { _unk3 = value; SignalPropertyChange(); } }
        [Category("Misc Section 9 Data")]
        public int Unk4 { get { return _unk4; } set { _unk4 = value; SignalPropertyChange(); } }
        [Category("Misc Section 9 Data")]
        public int ListOffset { get { return Header->_list._startOffset; } }
        [Category("Misc Section 9 Data")]
        public int ListCount { get { return Header->_list._listCount; } }

        protected override bool OnInitialize()
        {
            base.OnInitialize();
            _name = "Data";
            _unk1 = Header->_unk1;
            _unk2 = Header->_unk2;
            _unk3 = Header->_unk3;
            _unk4 = Header->_unk4;
            return ListOffset > 0;
        }

        protected override void OnPopulate()
        {
            bint* addr = (bint*)(BaseAddress + ListOffset);
            for (int i = 0; i < ListCount; i++)
                new MoveDefBoneIndexNode().Initialize(this, addr++, 4);
        }

        protected override int OnCalculateSize(bool force)
        {
            _lookupCount = (Children.Count > 0 ? 1 : 0);
            return 24 + Children.Count * 4;
        }

        protected internal override void OnRebuild(VoidPtr address, int length, bool force)
        {
            _lookupOffsets = new List<int>();

            bint* addr = (bint*)address;
            foreach (MoveDefBoneIndexNode b in Children)
                *addr++ = b.boneIndex;

            FDefMiscSection9Data* header = (FDefMiscSection9Data*)addr;

            _entryOffset = header;

            header->_unk1 = _unk1;
            header->_unk2 = _unk2;
            header->_unk3 = _unk3;
            header->_unk4 = _unk4;

            header->_list._listCount = Children.Count;
            header->_list._startOffset = (Children.Count > 0 ? (int)address - (int)_rebuildBase : 0);
            
            if (header->_list._startOffset > 0)
                _lookupOffsets.Add((int)header->_list._startOffset.Address - (int)_rebuildBase);
        }
    }

    public unsafe class UnkSection5Node : MoveDefEntryNode
    {
        internal FDefMiscSection5* Header { get { return (FDefMiscSection5*)WorkingUncompressed.Address; } }
        public override ResourceType ResourceType { get { return ResourceType.Unknown; } }

        int _unk1, _unk2, _unk3, _unk4;

        [Category("Misc Section 5")]
        public int Unk1 { get { return _unk1; } set { _unk1 = value; SignalPropertyChange(); } }
        [Category("Misc Section 5")]
        public int Unk2 { get { return _unk2; } set { _unk2 = value; SignalPropertyChange(); } }
        [Category("Misc Section 5")]
        public int Unk3 { get { return _unk3; } set { _unk3 = value; SignalPropertyChange(); } }
        [Category("Misc Section 5")]
        public int Unk4 { get { return _unk4; } set { _unk4 = value; SignalPropertyChange(); } }

        protected override bool OnInitialize()
        {
            base.OnInitialize();
            _name = "Unknown Section 5";
            _unk1 = Header->_unk1;
            _unk2 = Header->_unk2;
            _unk3 = Header->_unk3;
            _unk4 = Header->_unk4;
            return false;
        }

        protected override int OnCalculateSize(bool force)
        {
            _lookupCount = 0;
            return 16;
        }

        protected internal override void OnRebuild(VoidPtr address, int length, bool force)
        {
            _entryOffset = address;
            FDefMiscSection5* header = (FDefMiscSection5*)address;
            header->_unk1 = _unk1;
            header->_unk2 = _unk2;
            header->_unk3 = _unk3;
            header->_unk4 = _unk4;
        }
    }

    public unsafe class UnkSection1Node : MoveDefEntryNode
    {
        internal FDefMiscSection1* Header { get { return (FDefMiscSection1*)WorkingUncompressed.Address; } }
        public override ResourceType ResourceType { get { return ResourceType.Unknown; } }

        int _unk1, _unk2, _unk3, _unk4, _unk5, _unk6, _unk7, _unk8;

        [Category("Misc Section 1")]
        public int Unk1 { get { return _unk1; } set { _unk1 = value; SignalPropertyChange(); } }
        [Category("Misc Section 1")]
        public int Unk2 { get { return _unk2; } set { _unk2 = value; SignalPropertyChange(); } }
        [Category("Misc Section 1")]
        public int Unk3 { get { return _unk3; } set { _unk3 = value; SignalPropertyChange(); } }
        [Category("Misc Section 1")]
        public int Unk4 { get { return _unk4; } set { _unk4 = value; SignalPropertyChange(); } }
        [Category("Misc Section 1")]
        public int Unk5 { get { return _unk5; } set { _unk5 = value; SignalPropertyChange(); } }
        [Category("Misc Section 1")]
        public int Unk6 { get { return _unk6; } set { _unk6 = value; SignalPropertyChange(); } }
        [Category("Misc Section 1")]
        public int Unk7 { get { return _unk7; } set { _unk7 = value; SignalPropertyChange(); } }
        [Category("Misc Section 1")]
        public int Unk8 { get { return _unk8; } set { _unk8 = value; SignalPropertyChange(); } }

        protected override bool OnInitialize()
        {
            base.OnInitialize();
            _name = "Misc Section 1";
            _unk1 = Header->_unk1;
            _unk2 = Header->_unk2;
            _unk3 = Header->_unk3;
            _unk4 = Header->_unk4;
            _unk5 = Header->_unk5;
            _unk6 = Header->_unk6;
            _unk7 = Header->_unk7;
            _unk8 = Header->_unk8;
            return false;
        }

        protected override int OnCalculateSize(bool force)
        {
            _lookupCount = 0;
            return 32;
        }

        protected internal override void OnRebuild(VoidPtr address, int length, bool force)
        {
            _entryOffset = address;
            FDefMiscSection1* header = (FDefMiscSection1*)address;
            header->_unk1 = _unk1;
            header->_unk2 = _unk2;
            header->_unk3 = _unk3;
            header->_unk4 = _unk4;
            header->_unk5 = _unk5;
            header->_unk6 = _unk6;
            header->_unk7 = _unk7;
            header->_unk8 = _unk8;
        }
    }

    public unsafe class UnknownSection2Node : MoveDefEntryNode
    {
        internal byte* Header { get { return (byte*)WorkingUncompressed.Address; } }

        int Count = 0;

        public UnknownSection2Node(int count) { Count = count; }

        protected override bool OnInitialize()
        {
            base.OnInitialize();
            _name = "Misc Section 2";

            return Count > 0;
        }

        protected override void OnPopulate()
        {
            byte* addr = Header;
            for (int i = 0; i < Count; i++)
                new MoveDefRawDataNode("Unk" + i).Initialize(this, addr + i * 32, 32);

            SetSizeInternal(Children.Count * 32);
        }

        protected override int OnCalculateSize(bool force)
        {
            _lookupCount = 0;
            return Children.Count * 32;
        }

        protected internal override void OnRebuild(VoidPtr address, int length, bool force)
        {
            _entryOffset = address;
            foreach (MoveDefRawDataNode d in Children)
                d.Rebuild(address + d.Index * 32, 32, true);
        }
    }

    public unsafe class UnknownSection3Node : MoveDefEntryNode
    {
        internal FDefMiscUnk3* Header { get { return (FDefMiscUnk3*)WorkingUncompressed.Address; } }

        int _haveNBoneIndex1, _haveNBoneIndex2, _haveNBoneIndex3, _throwNBoneIndex, _unkCount, _unkOffset, _pad;
        
        [Category("Bone References")]
        public int HaveNBoneIndex1 { get { return _haveNBoneIndex1; } set { _haveNBoneIndex1 = value; SignalPropertyChange(); } }
        [Category("Bone References")]
        public int HaveNBoneIndex2 { get { return _haveNBoneIndex2; } set { _haveNBoneIndex2 = value; SignalPropertyChange(); } }
        [Category("Bone References")]
        public int ThrowNBoneIndex { get { return _throwNBoneIndex; } set { _throwNBoneIndex = value; SignalPropertyChange(); } }
        [Category("Bone References")]
        public int EntryCount { get { return _unkCount; } }
        [Category("Bone References")]
        public int EntryOffset { get { return _unkOffset; } }
        [Category("Bone References")]
        public int Pad { get { return _pad; } }
        [Category("Bone References")]
        public int HaveNBoneIndex3 { get { return _haveNBoneIndex3; } set { _haveNBoneIndex3 = value; SignalPropertyChange(); } }

        protected override bool OnInitialize()
        {
            base.OnInitialize();
            _name = "Misc Item Bones";
            
            _haveNBoneIndex1 = Header->_haveNBoneIndex1;
            _haveNBoneIndex2 = Header->_haveNBoneIndex2;
            _throwNBoneIndex = Header->_throwNBoneIndex;
            _unkCount = Header->_list._startOffset;
            _unkOffset = Header->_list._listCount;
            _pad = Header->_pad;
            _haveNBoneIndex3 = Header->_haveNBoneIndex3;

            return EntryOffset > 0;
        }

        protected override void OnPopulate()
        {
            FDefMiscUnk3Entry* addr = (FDefMiscUnk3Entry*)(BaseAddress + EntryOffset);
            for (int i = EntryOffset; i < _offset; i += 16)
                new UnknownSection3EntryNode().Initialize(this, addr++, 16);
        }

        protected override int OnCalculateSize(bool force)
        {
            _lookupCount = (Children.Count > 0 ? 1 : 0);
            return 28 + Children.Count * 16;
        }

        protected internal override void OnRebuild(VoidPtr address, int length, bool force)
        {
            FDefMiscUnk3Entry* data = (FDefMiscUnk3Entry*)address;
            foreach (UnknownSection3EntryNode e in Children)
                e.Rebuild(data++, 16, true);

            _entryOffset = data;

            FDefMiscUnk3* header = (FDefMiscUnk3*)data;
            header->_haveNBoneIndex1 = _haveNBoneIndex1;
            header->_haveNBoneIndex2 = _haveNBoneIndex2;
            header->_throwNBoneIndex = _throwNBoneIndex;
            header->_pad = _pad;
            header->_haveNBoneIndex3 = _haveNBoneIndex3;

            //Values are switched on purpose
            header->_list._startOffset = Children.Count;
            header->_list._listCount = (Children.Count > 0 ? (int)address - (int)_rebuildBase : 0);

            if (header->_list._listCount > 0)
                _lookupOffsets.Add((int)header->_list._listCount.Address - (int)_rebuildBase);
        }
    }

    public unsafe class UnknownSection3EntryNode : MoveDefEntryNode
    {
        internal FDefMiscUnk3Entry* Header { get { return (FDefMiscUnk3Entry*)WorkingUncompressed.Address; } }
        public override ResourceType ResourceType { get { return ResourceType.Unknown; } }

        int _unk1, _unk2, _pad1, _pad2;
        
        [Category("Unk Section 3 Entry")]
        public int Unk1 { get { return _unk1; } set { _unk1 = value; SignalPropertyChange(); } }
        [Category("Unk Section 3 Entry")]
        public int Unk2 { get { return _unk2; } set { _unk2 = value; SignalPropertyChange(); } }
        [Category("Unk Section 3 Entry")]
        public int Pad1 { get { return _pad1; } }
        [Category("Unk Section 3 Entry")]
        public int Pad2 { get { return _pad2; } }

        protected override bool OnInitialize()
        {
            base.OnInitialize();
            _name = "Entry" + Index;

            _unk1 = Header->_unk1;
            _unk2 = Header->_unk2;
            _pad1 = Header->_pad1;
            _pad2 = Header->_pad2;

            return false;
        }

        protected override int OnCalculateSize(bool force)
        {
            _lookupCount = 0;
            return 16;
        }

        protected internal override void OnRebuild(VoidPtr address, int length, bool force)
        {
            _entryOffset = address;
            FDefMiscUnk3Entry* header = (FDefMiscUnk3Entry*)address;
            header->_unk1 = _unk1;
            header->_unk2 = _unk2;
            header->_pad1 = _pad1;
            header->_pad2 = _pad2;
        }
    }

    public unsafe class MoveDefMiscHurtBoxesNode : MoveDefEntryNode
    {
        internal FDefHurtBox* Start { get { return (FDefHurtBox*)WorkingUncompressed.Address; } }
        public override ResourceType ResourceType { get { return ResourceType.MDefHurtboxList; } }

        internal int Count = 0;

        public MoveDefMiscHurtBoxesNode(int count) { Count = count; }

        protected override bool OnInitialize()
        {
            base.OnInitialize();
            _name = "Misc HurtBoxes";
            return Count > 0;
        }

        protected override void OnPopulate()
        {
            FDefHurtBox* entry = Start;
            for (int i = 0; i < Count; i++)
                new MoveDefHurtBoxNode().Initialize(this, new DataSource((VoidPtr)(entry++), 0x20));
        }

        protected override int OnCalculateSize(bool force)
        {
            _lookupCount = 0;
            return Children.Count * 0x20;
        }

        protected internal override void OnRebuild(VoidPtr address, int length, bool force)
        {
            _entryOffset = address;
            for (int i = 0; i < Children.Count; i++)
                Children[i].Rebuild(address + i * 0x20, 0x20, force);
        }
    }

    public unsafe class MoveDefHurtBoxNode : MoveDefEntryNode
    {
        internal FDefHurtBox* Header { get { return (FDefHurtBox*)WorkingUncompressed.Address; } }
        public override ResourceType ResourceType { get { return ResourceType.Unknown; } }

        internal Vector3 _offst, _stretch;
        internal float _radius;
        internal HurtBoxFlags flags = new HurtBoxFlags();

        [Browsable(false)]
        public MDL0BoneNode BoneNode
        {
            get { if (Model == null) return null; if (flags.BoneIndex > Model._linker.BoneCache.Length || flags.BoneIndex < 0) return null; return (MDL0BoneNode)Model._linker.BoneCache[flags.BoneIndex]; }
            set { flags.BoneIndex = value.BoneIndex; Name = value.Name; }
        }
        
        [Category("HurtBox"), TypeConverter(typeof(Vector3StringConverter))]
        public Vector3 PosOffset { get { return _offst; } set { _offst = value; SignalPropertyChange(); } }
        [Category("HurtBox"), TypeConverter(typeof(Vector3StringConverter))]
        public Vector3 Stretch { get { return _stretch; } set { _stretch = value; SignalPropertyChange(); } }
        [Category("HurtBox")]
        public float Radius { get { return _radius; } set { _radius = value; SignalPropertyChange(); } }
        [Category("HurtBox"), Browsable(true), TypeConverter(typeof(DropDownListBonesMDef))]
        public string Bone { get { return BoneNode == null ? flags.BoneIndex.ToString() : BoneNode.Name; } set { if (Model == null) { flags.BoneIndex = Convert.ToInt32(value); Name = flags.BoneIndex.ToString(); } else { BoneNode = String.IsNullOrEmpty(value) ? BoneNode : Model.FindBone(value); } SignalPropertyChange(); } }
        [Category("HurtBox")]
        public bool Enabled { get { return flags.Enabled; } set { flags.Enabled = value; SignalPropertyChange(); } }
        [Category("HurtBox")]
        public HurtBoxZone Zone { get { return flags.Zone; } set { flags.Zone = value; SignalPropertyChange(); } }
        [Category("HurtBox")]
        public int Region { get { return flags.Region; } set { flags.Region = value; SignalPropertyChange(); } }
        [Category("HurtBox")]
        public int Unknown { get { return flags.Unk; } set { flags.Unk = value; SignalPropertyChange(); } }

        public override string Name
        {
            get { return Bone; }
            //set { base.Name = value; }
        }

        protected override bool OnInitialize()
        {
            base.OnInitialize();
            _offst = Header->_offset;
            _stretch = Header->_stretch;
            _radius = Header->_radius;
            flags = Header->_flags;

            _name = Bone;

            return false;
        }

        protected override int OnCalculateSize(bool force)
        {
            _lookupCount = 0;
            return 0x20;
        }

        protected internal override void OnRebuild(VoidPtr address, int length, bool force)
        {
            _entryOffset = address;
            FDefHurtBox* header = (FDefHurtBox*)address;
            header->_offset = _offst;
            header->_stretch = _stretch;
            header->_radius = _radius;
            header->_flags = flags;
        }

        #region Rendering
        public unsafe void Render(GLContext ctx, bool selected, int type) 
        {
            //Coded by Toomai
            //Modified for release v0.67

            //Disable all things that could be enabled
            ctx.glDisable((uint)GLEnableCap.TEXTURE_GEN_S);
            ctx.glDisable((uint)GLEnableCap.TEXTURE_GEN_T);
            ctx.glDisable((uint)GLEnableCap.CullFace);
            ctx.glDisable((uint)GLEnableCap.Lighting);
            ctx.glDisable((uint)GLEnableCap.DepthTest);
            ctx.glPolygonMode(GLFace.FrontAndBack, GLPolygonMode.Fill);

            switch (type)
            {
                case 0: //normal - yellow
                    switch ((int)Zone)
                    {
                        case 0:
                            ctx.glColor(selected ? 0.0f : 0.5f, 0.5f, 0.0f, 0.5f);
                            break;
                        default:
                            ctx.glColor(selected ? 0.0f : 1.0f, 1.0f, 0.0f, 0.5f);
                            break;
                        case 2:
                            ctx.glColor(selected ? 0.0f : 1.0f, 1.0f, 0.25f, 0.5f);
                            break;
                    }
                    break;
                case 1: //invincible - green
                    switch ((int)Zone)
                    {
                        case 0:
                            ctx.glColor(selected ? 0.0f : 0.0f, 0.5f, 0.0f, 0.5f);
                            break;
                        default:
                            ctx.glColor(selected ? 0.0f : 0.0f, 1.0f, 0.0f, 0.5f);
                            break;
                        case 2:
                            ctx.glColor(selected ? 0.0f : 0.0f, 1.0f, 0.25f, 0.5f);
                            break;
                    }
                    break;
                default: //intangible - blue
                    switch ((int)Zone)
                    {
                        case 0:
                            ctx.glColor(0.0f, selected ? 0.5f : 0.0f, selected ? 0.0f : 0.5f, 0.5f);
                            break;
                        default:
                            ctx.glColor(0.0f, selected ? 1.0f : 0.0f, selected ? 0.0f : 1.0f, 0.5f);
                            break;
                        case 2:
                            ctx.glColor(0.0f, selected ? 1.0f : 0.25f, selected ? 0.25f : 1.0f, 0.5f);
                            break;
                    }
                    break;
            }

            Vector3 bonepos = BoneNode._frameMatrix.GetPoint();
            Vector3 bonerot = BoneNode._frameMatrix.GetAngles();
            Vector3 bonescl = BoneNode.RecursiveScale();

            bonescl *= _radius;
            Matrix m = Matrix.TransformMatrix(bonescl, bonerot, bonepos);

            ctx.glPushMatrix();
            ctx.glMultMatrix((float*)&m);

            Vector3 stretchfac = new Vector3(_stretch._x / bonescl._x, _stretch._y / bonescl._y, _stretch._z / bonescl._z);
            ctx.glTranslate(_offst._x / bonescl._x, _offst._y / bonescl._y, _offst._z / bonescl._z);

            int res = 16;
            double angle = 360.0 / res;

            // eight corners: XYZ, XYz, XyZ, Xyz, xYZ, xYz, xyZ, xyz
            for (int quadrant = 0; quadrant < 8; quadrant++)
            {
                for (double i = 0; i < 180 / angle; i++)
                {
                    double ringang1 = (i * angle) / 180 * Math.PI;
                    double ringang2 = ((i + 1) * angle) / 180 * Math.PI;

                    for (double j = 0; j < 360 / angle; j++)
                    {
                        double ang1 = (j * angle) / 180 * Math.PI;
                        double ang2 = ((j + 1) * angle) / 180 * Math.PI;

                        int q = 0;
                        Vector3 stretch = new Vector3(0, 0, 0);

                        if (Math.Cos(ang2) >= 0) // X
                        {
                            q += 4;
                            if (_stretch._x > 0)
                                stretch._x = stretchfac._x;
                        }
                        else
                        {
                            if (_stretch._x < 0)
                                stretch._x = stretchfac._x;
                        }
                        if (Math.Sin(ang2) >= 0) // Y
                        {
                            q += 2;
                            if (_stretch._y > 0)
                                stretch._y = stretchfac._y;
                        }
                        else
                        {
                            if (_stretch._y < 0)
                                stretch._y = stretchfac._y;
                        }
                        if (Math.Cos(ringang2) >= 0) // Z
                        {
                            q += 1;
                            if (_stretch._z > 0)
                                stretch._z = stretchfac._z;
                        }
                        else
                        {
                            if (_stretch._z < 0)
                                stretch._z = stretchfac._z;
                        }
                        if (quadrant == q)
                        {
                            ctx.glTranslate(stretch._x,stretch._y,stretch._z);
                            ctx.glBegin(GLPrimitiveType.Quads);
                            ctx.glVertex(Math.Cos(ang1) * Math.Sin(ringang2), Math.Sin(ang1) * Math.Sin(ringang2), Math.Cos(ringang2));
                            ctx.glVertex(Math.Cos(ang2) * Math.Sin(ringang2), Math.Sin(ang2) * Math.Sin(ringang2), Math.Cos(ringang2));
                            ctx.glVertex(Math.Cos(ang2) * Math.Sin(ringang1), Math.Sin(ang2) * Math.Sin(ringang1), Math.Cos(ringang1));
                            ctx.glVertex(Math.Cos(ang1) * Math.Sin(ringang1), Math.Sin(ang1) * Math.Sin(ringang1), Math.Cos(ringang1));
                            ctx.glEnd();
                            ctx.glTranslate(-stretch._x,-stretch._y,-stretch._z);
                        }
                    }
                }
            }

            // twelve edges
            double x1, x2, y1, y2, z1, z2;

            // x-axis edges
            for (double i = 0; i < 360 / angle; i++)
            {
                double ang1 = (i * angle) / 180 * Math.PI;
                double ang2 = ((i + 1) * angle) / 180 * Math.PI;

                z1 = Math.Cos(ang1);
                z2 = Math.Cos(ang2);
                y1 = Math.Sin(ang1);
                y2 = Math.Sin(ang2);

                x1 = _stretch._x < 0 ? stretchfac._x : 0;
                x2 = _stretch._x > 0 ? stretchfac._x : 0;

                if (y2 >= 0 && _stretch._y > 0)
                {
                    y1 += stretchfac._y;
                    y2 += stretchfac._y;
                }
                if (y2 <= 0 && _stretch._y < 0)
                {
                    y1 += stretchfac._y;
                    y2 += stretchfac._y;
                }
                if (z2 >= 0 && _stretch._z > 0)
                {
                    z1 += stretchfac._z;
                    z2 += stretchfac._z;
                }
                if (z2 <= 0 && _stretch._z < 0)
                {
                    z1 += stretchfac._z;
                    z2 += stretchfac._z;
                }

                ctx.glBegin(GLPrimitiveType.Quads);
                ctx.glVertex(x1, y1, z1);
                ctx.glVertex(x2, y1, z1);
                ctx.glVertex(x2, y2, z2);
                ctx.glVertex(x1, y2, z2);
                ctx.glEnd();
            }

            // y-axis edges
            for (double i = 0; i < 360 / angle; i++)
            {
                double ang1 = (i * angle) / 180 * Math.PI;
                double ang2 = ((i + 1) * angle) / 180 * Math.PI;

                x1 = Math.Cos(ang1);
                x2 = Math.Cos(ang2);
                z1 = Math.Sin(ang1);
                z2 = Math.Sin(ang2);

                y1 = _stretch._y < 0 ? stretchfac._y : 0;
                y2 = _stretch._y > 0 ? stretchfac._y : 0;

                if (x2 >= 0 && _stretch._x > 0)
                {
                    x1 += stretchfac._x;
                    x2 += stretchfac._x;
                }
                if (x2 <= 0 && _stretch._x < 0)
                {
                    x1 += stretchfac._x;
                    x2 += stretchfac._x;
                }
                if (z2 >= 0 && _stretch._z > 0)
                {
                    z1 += stretchfac._z;
                    z2 += stretchfac._z;
                }
                if (z2 <= 0 && _stretch._z < 0)
                {
                    z1 += stretchfac._z;
                    z2 += stretchfac._z;
                }

                ctx.glBegin(GLPrimitiveType.Quads);
                ctx.glVertex(x1, y1, z1);
                ctx.glVertex(x1, y2, z1);
                ctx.glVertex(x2, y2, z2);
                ctx.glVertex(x2, y1, z2);
                ctx.glEnd();
            }

            // z-axis edges
            for (double i = 0; i < 360 / angle; i++)
            {
                double ang1 = (i * angle) / 180 * Math.PI;
                double ang2 = ((i + 1) * angle) / 180 * Math.PI;

                x1 = Math.Cos(ang1);
                x2 = Math.Cos(ang2);
                y1 = Math.Sin(ang1);
                y2 = Math.Sin(ang2);

                z1 = _stretch._z < 0 ? stretchfac._z : 0;
                z2 = _stretch._z > 0 ? stretchfac._z : 0;

                if (x2 >= 0 && _stretch._x > 0)
                {
                    x1 += stretchfac._x;
                    x2 += stretchfac._x;
                }
                if (x2 <= 0 && _stretch._x < 0)
                {
                    x1 += stretchfac._x;
                    x2 += stretchfac._x;
                }
                if (y2 >= 0 && _stretch._y > 0)
                {
                    y1 += stretchfac._y;
                    y2 += stretchfac._y;
                }
                if (y2 <= 0 && _stretch._y < 0)
                {
                    y1 += stretchfac._y;
                    y2 += stretchfac._y;
                }

                ctx.glBegin(GLPrimitiveType.Quads);
                ctx.glVertex(x2, y2, z1);
                ctx.glVertex(x2, y2, z2);
                ctx.glVertex(x1, y1, z2);
                ctx.glVertex(x1, y1, z1);
                ctx.glEnd();
            }

            Vector3 scale = BoneNode.RecursiveScale();

            // six faces
            ctx.glBegin(GLPrimitiveType.Quads);
            float outpos;

            // left face
            outpos = _radius / bonescl._x * scale._x;
            if (_stretch._x > 0)
                outpos = (_stretch._x + _radius) / bonescl._x;
            
            ctx.glVertex(outpos, 0, 0);
            ctx.glVertex(outpos, stretchfac._y, 0);
            ctx.glVertex(outpos, stretchfac._y, stretchfac._z);
            ctx.glVertex(outpos, 0, stretchfac._z);

            // right face
            outpos = -_radius / bonescl._x * scale._x;
            if (_stretch._x < 0)
                outpos = (_stretch._x - _radius) / bonescl._x;
            
            ctx.glVertex(outpos, 0, 0);
            ctx.glVertex(outpos, 0, stretchfac._z);
            ctx.glVertex(outpos, stretchfac._y, stretchfac._z);
            ctx.glVertex(outpos, stretchfac._y, 0);

            // top face
            outpos = _radius / bonescl._y * scale._y;
            if (_stretch._y > 0)
                outpos = (_stretch._y + _radius) / bonescl._y;
            
            ctx.glVertex(0, outpos, 0);
            ctx.glVertex(0, outpos, stretchfac._z);
            ctx.glVertex(stretchfac._x, outpos, stretchfac._z);
            ctx.glVertex(stretchfac._x, outpos, 0);

            // bottom face
            outpos = -_radius / bonescl._y * scale._y;
            if (_stretch._y < 0)
                outpos = (_stretch._y - _radius) / bonescl._y;
            
            ctx.glVertex(0, outpos, 0);
            ctx.glVertex(stretchfac._x, outpos, 0);
            ctx.glVertex(stretchfac._x, outpos, stretchfac._z);
            ctx.glVertex(0, outpos, stretchfac._z);

            // front face
            outpos = _radius / bonescl._z * scale._z;
            if (_stretch._z > 0)
                outpos = (_stretch._z + _radius) / bonescl._z;
            
            ctx.glVertex(0, 0, outpos);
            ctx.glVertex(stretchfac._x, 0, outpos);
            ctx.glVertex(stretchfac._x, stretchfac._y, outpos);
            ctx.glVertex(0, stretchfac._y, outpos);

            // right face
            outpos = -_radius / bonescl._z * scale._z;
            if (_stretch._z < 0)
                outpos = (_stretch._z - _radius) / bonescl._z;
            
            ctx.glVertex(0, 0, outpos);
            ctx.glVertex(0, stretchfac._y, outpos);
            ctx.glVertex(stretchfac._x, stretchfac._y, outpos);
            ctx.glVertex(stretchfac._x, 0, outpos);
            ctx.glEnd();

            ctx.glPopMatrix();
        }
        #endregion
    }

    public unsafe class MoveDefSectionUnk1Node : MoveDefEntryNode
    {
        internal FDefMiscUnkType1* Start { get { return (FDefMiscUnkType1*)WorkingUncompressed.Address; } }
        internal int Count = 0;

        public MoveDefSectionUnk1Node(int count) { Count = count; }

        protected override bool OnInitialize()
        {
            base.OnInitialize();
            _name = "Misc Bone Section";
            return true;
        }

        protected override void OnPopulate()
        {
            FDefMiscUnkType1* header = Start;
            for (int i = 0; i < Count; i++)
                new MoveDefSectionsUnk1NodeEntry().Initialize(this, new DataSource((VoidPtr)(header++), 0x14));
        }

        protected override int OnCalculateSize(bool force)
        {
            _lookupCount = 0;
            return Children.Count * 0x14;
        }

        protected internal override void OnRebuild(VoidPtr address, int length, bool force)
        {
            _entryOffset = address;
            for (int i = 0; i < Children.Count; i++)
                Children[i].Rebuild(address + i * 0x14, 0x14, force);
        }
    }

    public unsafe class MoveDefSectionsUnk1NodeEntry : MoveDefEntryNode
    {
        internal FDefMiscUnkType1* Header { get { return (FDefMiscUnkType1*)WorkingUncompressed.Address; } }
        public override ResourceType ResourceType { get { return ResourceType.Unknown; } }

        internal int boneIndex = 0;
        internal float x, y, width, height;

        [Browsable(false)]
        public MDL0BoneNode BoneNode
        {
            get { if (Model == null) return null; if (boneIndex > Model._linker.BoneCache.Length || boneIndex < 0) return null; return (MDL0BoneNode)Model._linker.BoneCache[boneIndex]; }
            set { boneIndex = value.BoneIndex; Name = value.Name; }
        }

        [Category("Misc Bone Entry"), Browsable(true), TypeConverter(typeof(DropDownListBonesMDef))]
        public string Bone { get { return BoneNode == null ? boneIndex.ToString() : BoneNode.Name; } set { if (Model == null) { boneIndex = Convert.ToInt32(value); Name = boneIndex.ToString(); } else { BoneNode = String.IsNullOrEmpty(value) ? BoneNode : Model.FindBone(value); } SignalPropertyChange(); } }
        [Category("Misc Bone Entry"), TypeConverter(typeof(Vector2StringConverter))]
        public Vector2 XY { get { return new Vector2(x, y); } set { x = value._x; y = value._y; SignalPropertyChange(); } }
        [Category("Misc Bone Entry")]
        public float Height { get { return width; } set { height = value; SignalPropertyChange(); } }
        [Category("Misc Bone Entry")]
        public float Width { get { return height; } set { width = value; SignalPropertyChange(); } }

        public override string Name
        {
            get { return Bone; }
            //set { base.Name = value; }
        }

        protected override bool OnInitialize()
        {
            boneIndex = Header->_boneIndex;
            x = Header->_x;
            y = Header->_y;
            width = Header->_width;
            height = Header->_height;

            //_name = Bone;

            return false;
        }

        protected override int OnCalculateSize(bool force)
        {
            _lookupCount = 0;
            return 0x14;
        }

        protected internal override void OnRebuild(VoidPtr address, int length, bool force)
        {
            _entryOffset = address;
            FDefMiscUnkType1* header = (FDefMiscUnkType1*)address;
            header->_boneIndex = boneIndex;
            header->_height = height;
            header->_width = width;
            header->_x = x;
            header->_y = y;
        }
    }

    public unsafe class MoveDefLedgegrabsNode : MoveDefEntryNode
    {
        internal FDefLedgegrab* Start { get { return (FDefLedgegrab*)WorkingUncompressed.Address; } }
        internal int Count = 0;

        public MoveDefLedgegrabsNode(int count) { Count = count; }

        protected override bool OnInitialize()
        {
            base.OnInitialize();
            _name = "Misc LedgeGrabs";
            return Count > 0;
        }

        protected override void OnPopulate()
        {
            FDefLedgegrab* entry = Start;
            for (int i = 0; i < Count; i++)
                new MoveDefLedgegrabNode().Initialize(this, new DataSource((VoidPtr)(entry++), 0x10));
        }

        protected override int OnCalculateSize(bool force)
        {
            _lookupCount = 0;
            return Children.Count * 16;
        }

        protected internal override void OnRebuild(VoidPtr address, int length, bool force)
        {
            _entryOffset = address;
            for (int i = 0; i < Children.Count; i++)
                Children[i].Rebuild(address + i * 16, 16, force);
        }
    }

    public unsafe class MoveDefLedgegrabNode : MoveDefEntryNode
    {
        internal FDefLedgegrab* Header { get { return (FDefLedgegrab*)WorkingUncompressed.Address; } }
        public override ResourceType ResourceType { get { return ResourceType.Unknown; } }

        internal float x, y, width, height;

        [Category("LedgeGrab"), TypeConverter(typeof(Vector2StringConverter))]
        public Vector2 XY { get { return new Vector2(x, y); } set { x = value._x; y = value._y; SignalPropertyChange(); } }
        [Category("LedgeGrab")]
        public float Height { get { return height; } set { height = value; SignalPropertyChange(); } }
        [Category("LedgeGrab")]
        public float Width { get { return width; } set { width = value; SignalPropertyChange(); } }

        protected override bool OnInitialize()
        {
            base.OnInitialize();
            if (_name == null)
                _name = "LedgeGrab" + Index;
            x = Header->_x;
            y = Header->_y;
            width = Header->_width;
            height = Header->_height;

            return false;
        }

        protected override int OnCalculateSize(bool force)
        {
            _lookupCount = 0;
            return 0x10;
        }

        protected internal override void OnRebuild(VoidPtr address, int length, bool force)
        {
            _entryOffset = address;
            FDefLedgegrab* header = (FDefLedgegrab*)address;
            header->_height = height;
            header->_width = width;
            header->_x = x;
            header->_y = y;
        }
    }

    public unsafe class MoveDefMultiJumpNode : MoveDefEntryNode
    {
        internal FDefMultiJump* Header { get { return (FDefMultiJump*)WorkingUncompressed.Address; } }
        public override ResourceType ResourceType { get { return ResourceType.Unknown; } }

        internal float unk1, unk2, unk3, horizontalBoost;
        internal List<float> hops, unks;
        internal float turnFrames;

        [Category("MultiJump Attribute")]
        public float Unk1 { get { return unk1; } set { unk1 = value; SignalPropertyChange(); } }
        [Category("MultiJump Attribute")]
        public float Unk2 { get { return unk2; } set { unk2 = value; SignalPropertyChange(); } }
        [Category("MultiJump Attribute")]
        public float Unk3 { get { return unk3; } set { unk3 = value; SignalPropertyChange(); } }
        [Category("MultiJump Attribute")]
        public float HorizontalBoost { get { return horizontalBoost; } set { horizontalBoost = value; SignalPropertyChange(); } }
        [Category("MultiJump Attribute")]
        public float TurnFrames { get { return turnFrames; } set { turnFrames = value; SignalPropertyChange(); } }
        [Category("MultiJump Attribute")]
        public float[] Hops { get { return hops.ToArray(); } set { hops = value.ToList<float>(); SignalPropertyChange(); } }
        [Category("MultiJump Attribute")]
        public float[] Unks { get { return unks.ToArray(); } set { unks = value.ToList<float>(); SignalPropertyChange(); } }

        protected override bool OnInitialize()
        {
            base.OnInitialize();
            _name = "Misc MultiJump";

            unks = new List<float>();
            hops = new List<float>();

            unk1 = Header->_unk1;
            unk2 = Header->_unk2;
            unk3 = Header->_unk3;
            horizontalBoost = Header->_horizontalBoost;

            if (Header->hopFixed)
                hops.Add(*(bfloat*)Header->_hopListOffset.Address);
            else
            {
                bfloat* addr = (bfloat*)(BaseAddress + Header->_hopListOffset);
                for (int i = 0; i < (_offset - Header->_hopListOffset) / 4; i++)
                    hops.Add(*addr++);
            }
            if (Header->unkFixed)
                unks.Add(*(bfloat*)Header->_unkListOffset.Address);
            else
            {
                bfloat* addr = (bfloat*)(BaseAddress + Header->_unkListOffset);
                for (int i = 0; i < ((Header->hopFixed ? _offset : (int)Header->_hopListOffset) - Header->_unkListOffset) / 4; i++)
                    unks.Add(*addr++);
            }
            if (Header->turnFixed)
                turnFrames = *(bfloat*)Header->_turnFrameOffset.Address;
            else
                turnFrames = Header->_turnFrameOffset;
            return false;
        }

        protected override int OnCalculateSize(bool force)
        {
            int size = 28;
            _lookupCount = 0;
            if (hops.Count > 1)
            {
                _lookupCount++;
                size += hops.Count * 4;
            }
            if (unks.Count > 1)
            {
                _lookupCount++;
                size += unks.Count * 4;
            }
            
            return size;
        }

        protected internal override void OnRebuild(VoidPtr address, int length, bool force)
        {
            int off = 0;
            if (hops.Count > 1)
                off += hops.Count * 4;
            if (unks.Count > 1)
                off += unks.Count * 4;

            FDefMultiJump* header = (FDefMultiJump*)(address + off);
            _entryOffset = header;

            bfloat* addr = (bfloat*)address;

            if (unks.Count > 1)
            {
                header->_unkListOffset = (int)addr - (int)_rebuildBase;
                if (header->_unkListOffset > 0)
                    _lookupOffsets.Add((int)header->_unkListOffset.Address - (int)_rebuildBase);

                foreach (float f in unks)
                    *addr++ = f;
            }
            else if (unks.Count == 1)
                *((bfloat*)header->_unkListOffset.Address) = unks[0];
            else
                *((bfloat*)header->_unkListOffset.Address) = 0;

            if (hops.Count > 1)
            {
                header->_hopListOffset = (int)addr - (int)_rebuildBase;
                if (header->_hopListOffset > 0)
                    _lookupOffsets.Add((int)header->_hopListOffset.Address - (int)_rebuildBase);
                
                foreach (float f in hops)
                    *addr++ = f;
            }
            else if (hops.Count == 1)
                *((bfloat*)header->_hopListOffset.Address) = hops[0];
            else
                *((bfloat*)header->_hopListOffset.Address) = 0;

            header->_unk1 = unk1;
            header->_unk2 = unk2;
            header->_unk3 = unk3;
            header->_horizontalBoost = horizontalBoost;

            if (header->turnFixed)
                *(bfloat*)header->_turnFrameOffset.Address = turnFrames;
            else
                header->_turnFrameOffset = (int)turnFrames;
        }
    }

    public unsafe class MoveDefGlideNode : MoveDefEntryNode
    {
        public override ResourceType ResourceType { get { return ResourceType.Unknown; } }

        internal bfloat* floatval { get { return (bfloat*)WorkingUncompressed.Address; } }
        internal bint intval1 { get { return *(bint*)(WorkingUncompressed.Address + 80); } }
        internal bint intval2 { get { return *(bint*)(WorkingUncompressed.Address + 84); } }

        internal float[] floatEntries;
        internal int intEntry1 = 0;
        internal int intEntry2 = 0;

        [Category("Glide Attribute")]
        public float[] Entries { get { return floatEntries; } }
        [Category("Glide Attribute")]
        public int Unk1 { get { return intEntry1; } }
        [Category("Glide Attribute")]
        public int Unk2 { get { return intEntry2; } }

        protected override bool OnInitialize()
        {
            base.OnInitialize();
            _name = "Misc Glide";

            floatEntries = new float[20];
            for (int i = 0; i < floatEntries.Length; i++)
                floatEntries[i] = floatval[i];
            intEntry1 = intval1;
            intEntry2 = intval2;

            return false;
        }

        protected override int OnCalculateSize(bool force)
        {
            _lookupCount = 0;
            return 88;
        }

        protected internal override void OnRebuild(VoidPtr address, int length, bool force)
        {
            _entryOffset = address;
            for (int i = 0; i < 20; i++)
                if (i < floatEntries.Length)
                    *(bfloat*)(address + i * 4) = floatEntries[i];
                else
                    *(bfloat*)(address + i * 4) = 0;
            *(bint*)(address + 80) = intval1;
            *(bint*)(address + 84) = intval2;
        }
    }

    public unsafe class MoveDefCrawlNode : MoveDefEntryNode
    {
        internal FDefCrawl* Header { get { return (FDefCrawl*)WorkingUncompressed.Address; } }
        public override ResourceType ResourceType { get { return ResourceType.Unknown; } }

        internal float forward, backward;

        [Category("Crawl Acceleration")]
        public float Forward { get { return forward; } set { forward = value; SignalPropertyChange(); } }
        [Category("Crawl Acceleration")]
        public float Backward { get { return backward; } set { backward = value; SignalPropertyChange(); } }

        protected override bool OnInitialize()
        {
            _name = "Misc Crawl";
            forward = Header->_forward;
            backward = Header->_backward;
            return false;
        }

        protected override int OnCalculateSize(bool force)
        {
            _lookupCount = 0;
            return 8;
        }

        protected internal override void OnRebuild(VoidPtr address, int length, bool force)
        {
            _entryOffset = address;
            FDefCrawl* header = (FDefCrawl*)address;
            header->_forward = forward;
            header->_backward = backward;
        }
    }

    public unsafe class MoveDefTetherNode : MoveDefEntryNode
    {
        internal FDefTether* Header { get { return (FDefTether*)WorkingUncompressed.Address; } }
        public override ResourceType ResourceType { get { return ResourceType.Unknown; } }

        internal int numHangFrame = 0;
        internal float unk1;

        [Category("Tether Entry")]
        public int HangFrameCount { get { return numHangFrame; } set { numHangFrame = value; SignalPropertyChange(); } }
        [Category("Tether Entry")]
        public float Unk1 { get { return unk1; } set { unk1 = value; SignalPropertyChange(); } }

        protected override bool OnInitialize()
        {
            base.OnInitialize();
            _name = "Misc Tether";
            numHangFrame = Header->_numHangFrame;
            unk1 = Header->_unk1;
            return false;
        }

        protected override int OnCalculateSize(bool force)
        {
            _lookupCount = 0;
            return 8;
        }

        protected internal override void OnRebuild(VoidPtr address, int length, bool force)
        {
            _entryOffset = address;
            FDefTether* header = (FDefTether*)address;
            header->_numHangFrame = numHangFrame;
            header->_unk1 = unk1;
        }
    }

    public unsafe class MoveDefSoundDatasNode : MoveDefCharSpecificNode
    {
        internal FDefListOffset* Header { get { return (FDefListOffset*)WorkingUncompressed.Address; } }
        internal int StartOffset, ListCount;

        protected override bool OnInitialize()
        {
            base.OnInitialize();
            if (_name == null)
                _name = "Misc Sound Lists";
            StartOffset = Header->_startOffset;
            ListCount = Header->_listCount;
            return true;
        }

        protected override void OnPopulate()
        {
            for (int i = 0; i < ListCount; i++)
                new MoveDefSoundDataNode().Initialize(this, new DataSource(BaseAddress + StartOffset + i * 8, 8));
        }

        protected override int OnCalculateSize(bool force)
        {
            int size = 8;
            _lookupCount = (Children.Count > 0 ? 1 : 0);
            foreach (MoveDefSoundDataNode r in Children)
            {
                _lookupCount += (r.Children.Count > 0 ? 1 : 0);
                size += 8/* + r.Children.Count * 4*/;
            }
            return size;
        }

        protected internal override void OnRebuild(VoidPtr address, int length, bool force)
        {
            int sndOff = 0, mainOff = 0;
            foreach (MoveDefSoundDataNode r in Children)
            {
                mainOff += 8;
                //sndOff += r.Children.Count * 4;
            }

            //indices
            //sound list offsets
            //header
            
            bint* indices = (bint*)address;
            FDefListOffset* sndLists = (FDefListOffset*)(address + sndOff);
            FDefListOffset* header = (FDefListOffset*)((VoidPtr)sndLists + mainOff);

            _entryOffset = header;

            if (Children.Count > 0)
            {
                header->_startOffset = (int)sndLists - (int)_rebuildBase;
               _lookupOffsets.Add((int)header->_startOffset.Address - (int)_rebuildBase);
            }

            header->_listCount = Children.Count;

            foreach (MoveDefSoundDataNode r in Children)
            {
                if (r.Children.Count > 0)
                {
                    //sndLists->_startOffset = (int)indices - (int)_rebuildBase;
                    sndLists->_startOffset = (int)(r.Children[0] as MoveDefEntryNode)._entryOffset - (int)_rebuildBase;
                    _lookupOffsets.Add((int)sndLists->_startOffset.Address - (int)_rebuildBase);
                }

                (sndLists++)->_listCount = r.Children.Count;
                //foreach (MoveDefIndexNode b in r.Children)
                //{
                //    b._entryOffset = indices;
                //    *indices++ = b.ItemIndex;
                //}
            }
        }
    }

    public unsafe class MoveDefSoundDataNode : MoveDefEntryNode
    {
        internal FDefListOffset* Header { get { return (FDefListOffset*)WorkingUncompressed.Address; } }
        internal int StartOffset, ListCount;

        public MoveDefSoundDataNode() { }

        protected override bool OnInitialize()
        {
            base.OnInitialize();
            _name = "SoundData" + Index;
            StartOffset = Header->_startOffset;
            ListCount = Header->_listCount;
            return true;
        }

        protected override void OnPopulate()
        {
            for (int i = 0; i < ListCount; i++)
                new MoveDefIndexNode() { _name = ("SFX" + i) }.Initialize(this, new DataSource(BaseAddress + StartOffset + i * 4, 0x4));
        }
    }
}