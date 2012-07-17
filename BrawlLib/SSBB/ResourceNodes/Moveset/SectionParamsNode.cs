using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.SSBBTypes;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class MoveDefSectionParamNode : MoveDefCharSpecificNode
    {
        internal byte* Header { get { return (byte*)WorkingUncompressed.Address; } }
        public override ResourceType ResourceType { get { return ResourceType.Unknown; } }

        public List<AttributeInfo> _info;
        public string OldName;

        public override string Name
        {
            get { return base.Name; }
            set
            {
                base.Name = value;
                if (Parent is MoveDefArticleNode)
                    Root.Params[(Parent as MoveDefArticleNode).ArticleStringID].NewName = value;
                else if (Parent is MoveDefSectionParamNode)
                    Root.Params[TreePath].NewName = value;
                else
                    Root.Params[OldName].NewName = value;
                Root._dictionaryChanged = true;
            }
        }

        private UnsafeBuffer attributeBuffer;

        [Browsable(false)]
        public UnsafeBuffer AttributeBuffer { get { if (attributeBuffer != null) return attributeBuffer; else return attributeBuffer = new UnsafeBuffer(0x2E4); } }

        public Dictionary<int, FDefListOffset> offsets;

        protected override bool OnInitialize()
        {
            offsets = new Dictionary<int, FDefListOffset>();

            base.OnInitialize();

            if (Size == 0)
                SetSizeInternal(4);

            OldName = _name;
            SectionParamInfo data = null;
            if (Parent is MoveDefArticleNode)
            {
                if (Root.Params.ContainsKey((Parent as MoveDefArticleNode).ArticleStringID + "/" + _name))
                {
                    data = Root.Params[(Parent as MoveDefArticleNode).ArticleStringID + "/" + _name];
                    _info = data.Attributes;
                    if (!String.IsNullOrEmpty(data.NewName))
                        _name = data.NewName;
                }
                else _info = new List<AttributeInfo>();
            }
            else if (Parent is MoveDefSectionParamNode)
            {
                if (Root.Params.ContainsKey(TreePath))
                {
                    data = Root.Params[TreePath];
                    _info = data.Attributes;
                    if (!String.IsNullOrEmpty(data.NewName))
                        _name = data.NewName;
                }
                else _info = new List<AttributeInfo>();
            }
            else if (Root.Params.ContainsKey(Name))
            {
                data = Root.Params[Name];
                _info = data.Attributes;
                if (!String.IsNullOrEmpty(data.NewName))
                    _name = data.NewName;
            }
            else _info = new List<AttributeInfo>();

            attributeBuffer = new UnsafeBuffer(Size);
            byte* pOut = (byte*)attributeBuffer.Address;
            byte* pIn = (byte*)Header;

            //if (String.IsNullOrEmpty(_name = new String((sbyte*)Header)))
            //    _name = OldName;

            for (int i = 0; i < Size; i++)
            {
                if (i % 4 == 0)
                {
                    if (data == null)
                    {
                        AttributeInfo info = new AttributeInfo();

                        //Guess
                        if (((((uint)*((buint*)pIn)) >> 24) & 0xFF) != 0 && *((bint*)pIn) != -1 && !float.IsNaN(((float)*((bfloat*)pIn))))
                            info._type = 0;
                        else
                        {
                            if (*((bint*)pIn) > 1480 && *((bint*)pIn) < Root.dataSize)
                                info._type = 3;
                            else
                                info._type = 1;
                        }

                        info._name = (info._type == 1 ? "*" : "" + (info._type > 3 ? "+" : "")) + "0x" + i.ToString("X");
                        info._description = "No Description Available.";

                        _info.Add(info);
                    }

                    //if (_info.Count == i / 4)
                    //    break;

                    //AttributeInfo n = _info[i / 4];
                    //if (n._type == 3)
                    //{
                    //    int id = 0;
                    //    if (!int.TryParse(n._description, out id))
                    //        id = i / 4;
                    //    if (!offsets.ContainsKey(id))
                    //        offsets.Add(id, new FDefListOffset() { _startOffset = *(bint*)pIn, _listCount = 1 });
                    //    else
                    //    {
                    //        FDefListOffset d = offsets[id];
                    //        d._startOffset = *(bint*)pIn;
                    //        offsets[id] = d;
                    //    }
                    //}
                    //else if (n._type == 4)
                    //{
                    //    int id = int.Parse(n._description);
                    //    if (!offsets.ContainsKey(id))
                    //        offsets.Add(id, new FDefListOffset() { _listCount = *(bint*)pIn });
                    //    else
                    //    {
                    //        FDefListOffset d = offsets[id];
                    //        d._listCount = *(bint*)pIn;
                    //        offsets[id] = d;
                    //    }
                    //}
                }
                *pOut++ = *pIn++;
            }

            if (Parent is MoveDefArticleNode)
            {
                string id = (Parent as MoveDefArticleNode).ArticleStringID + "/" + _name;
                if (!Root.Params.ContainsKey(id))
                {
                    Root.Params.Add(id, new SectionParamInfo());
                    Root.Params[id].Attributes = _info;
                    Root.Params[id].NewName = _name;
                    data = Root.Params[id];
                }
            }
            else if (Parent is MoveDefSectionParamNode)
            {
                if (!Root.Params.ContainsKey(TreePath))
                {
                    Root.Params.Add(TreePath, new SectionParamInfo());
                    Root.Params[TreePath].Attributes = _info;
                    Root.Params[TreePath].NewName = _name;
                    data = Root.Params[TreePath];
                }
            }

            return false;
            //return offsets.Values.Count > 0;
        }

        protected override void OnPopulate()
        {
            int x = 0;
            foreach (FDefListOffset list in offsets.Values)
            {
                if (list._startOffset <= 0)
                    continue;

                string name = null;
                int off = list._startOffset;
                int count = list._listCount;
                int size = Root.GetSize(off);
                if (size <= 0)
                {
                    MoveDefExternalNode ext = Root.IsExternal(off);
                    if (ext == null)
                        size = 4;
                    else
                    {
                        name = ext.Name;
                        size = ext.Size;
                    }
                }
                else
                    size /= count;
                VoidPtr addr = BaseAddress + list._startOffset;
                MoveDefRawDataNode data = new MoveDefRawDataNode("Data" + x);
                data.Initialize(this, addr, size * count);
                for (int i = 0; i < list._listCount; i++)
                    new MoveDefSectionParamNode() { _name = name == null ? "Part" + i : name }.Initialize(data, addr + i * size, size);
                x++;
            }
        }

        public override string TreePathAbsolute { get { return _parent == null || !(_parent is MoveDefSectionParamNode || _parent is MoveDefRawDataNode) ? OldName : _parent.TreePathAbsolute + "/" + OldName; } }

        //public static void ChildLevels(MoveDefEntryNode e, ref int levels)
        //{
        //    if (e is MoveDefSectionParamNode)
        //    if ((e as MoveDefSectionParamNode).levelIndex > levels)
        //        levels = (e as MoveDefSectionParamNode).levelIndex;
        //    foreach (MoveDefSectionParamNode r in e.Children)
        //        ChildLevels(r, ref levels);
        //}

        //public static void RebuildChildren(MoveDefEntryNode e, int levels, ref VoidPtr address)
        //{
        //    if (e is MoveDefSectionParamNode && (e as MoveDefSectionParamNode).levelIndex == levels)
        //    {
        //        (e as MoveDefSectionParamNode).notRoot = true;
        //        e.Rebuild(address, e._calcSize, true);
        //        address += e._calcSize;
        //    }
        //    else
        //        foreach (MoveDefEntryNode p in e.Children)
        //            RebuildChildren(p, levels, ref address);
        //}

        //public bool notRoot = false;
        protected internal override void OnRebuild(VoidPtr address, int length, bool force)
        {
            //Top:

            

            //if (!notRoot)
            //{
            //    int childLevels = 0;
            //    foreach (MoveDefEntryNode p in Children)
            //        ChildLevels(p, ref childLevels);

            //    VoidPtr addr = address;

            //    for (int i = childLevels; i >= 0; i--)
            //        foreach (MoveDefEntryNode p in Children)
            //            RebuildChildren(this, i, ref addr);
            //}

            _entryOffset = address;
            byte* pIn = (byte*)attributeBuffer.Address;
            byte* pOut = (byte*)address;
            for (int i = 0; i < attributeBuffer.Length; i++)
                *pOut++ = *pIn++;

            //PostProcessOffsets(this);
        }

        //public static void PostProcessOffsets(MoveDefEntryNode e)
        //{
        //    if (e is MoveDefSectionParamNode)
        //    {
        //        MoveDefSectionParamNode p = e as MoveDefSectionParamNode;

        //        int index = 0;
        //        VoidPtr addr = p._entryOffset;
        //        foreach (AttributeInfo i in p._info)
        //        {
        //            if (i._type == 3) //offset
        //            {
        //                bint* offset = (bint*)addr + index;
        //                int id;
        //                if (!int.TryParse(i._description, out id))
        //                    id = index;
        //                SectionDataGroupNode d = e.Children[id] as SectionDataGroupNode;
        //                *offset = (int)d._entryOffset - (int)d._rebuildBase;
        //            }
        //            else if (i._type == 4) //count
        //            {
        //                bint* count = (bint*)addr + index;
        //                int id = int.Parse(i._description);
        //                MoveDefRawDataNode d = e.Children[id] as MoveDefRawDataNode;
        //                *count = d.Children.Count;
        //            }
        //            index++;
        //        }
        //    }
        //    foreach (MoveDefEntryNode x in e.Children)
        //        PostProcessOffsets(x);
        //}

        protected override int OnCalculateSize(bool force)
        {
            _lookupCount = 0;
            _entryLength = attributeBuffer.Length;
            //foreach (SectionDataGroupNode p in Children)
            //    _entryLength += p.CalculateSize(true);
            return _entryLength;
        }
    }

    public class SectionDataGroupNode : MoveDefEntryNode
    {
        internal VoidPtr First { get { return (VoidPtr)WorkingUncompressed.Address; } }
        public int Count = 0, EntrySize = 0, ID = 0;

        SectionDataGroupNode(int count, int size, int id) { Count = count; EntrySize = size; ID = id; }

        [Browsable(false)]
        int levelIndex
        {
            get
            {
                int i = 1;
                ResourceNode n = _parent;
                while ((n is MoveDefSectionParamNode || n is SectionDataGroupNode) && (n != null))
                {
                    n = n._parent;
                    if (n is SectionDataGroupNode)
                        i++;
                }
                return i;
            }
        }

        protected override bool OnInitialize()
        {
            _name = "Data" + ID;
            return Count > 0;
        }

        protected override void OnPopulate()
        {
            for (int i = 0; i < Count; i++)
                new MoveDefSectionParamNode() { _name = "Part" + i }.Initialize(this, First + i * EntrySize, EntrySize);
        }

        protected override int OnCalculateSize(bool force)
        {
            int size = 0;

            foreach (MoveDefSectionParamNode p in Children)
                size += p.CalculateSize(true);

            return size;
        }

        protected internal override void OnRebuild(VoidPtr address, int length, bool force)
        {
            _entryOffset = address;
            base.OnRebuild(address, length, force);
        }
    }

    public unsafe class MoveDefHitDataListNode : MoveDefCharSpecificNode
    {
        internal hitData* First { get { return (hitData*)WorkingUncompressed.Address; } }
        
        protected override bool OnInitialize()
        {
            base.OnInitialize();
            _offsets.Add(_offset);
            return Size / 32 > 0;
        }

        protected override void OnPopulate()
        {
            for (int i = 0; i < Size / 32; i++)
                new MoveDefHitDataNode() { _extOverride = true }.Initialize(this, First + i, 32);
        }

        protected override int OnCalculateSize(bool force)
        {
            _lookupCount = 0;
            return _entryLength = 32 * Children.Count;
        }

        protected internal override void OnRebuild(VoidPtr address, int length, bool force)
        {
            _entryOffset = address;
            hitData* data = (hitData*)address;
            foreach (MoveDefHitDataNode h in Children)
                h.Rebuild(data++, 32, true);
        }
    }

    public unsafe class MoveDefHitDataNode : MoveDefEntryNode
    {
        internal hitData* Header { get { return (hitData*)WorkingUncompressed.Address; } }
        public override ResourceType ResourceType { get { return ResourceType.Unknown; } }

        public uint unk8;
        public float unk1, unk2, unk3, unk4, unk5, unk6, unk7;
        
        [Category("Hit Data")]
        public float Unknown1 { get { return unk1; } set { unk1 = value; SignalPropertyChange(); } }
        [Category("Hit Data")]
        public float Unknown2 { get { return unk2; } set { unk2 = value; SignalPropertyChange(); } }
        [Category("Hit Data")]
        public float Unknown3 { get { return unk3; } set { unk3 = value; SignalPropertyChange(); } }
        [Category("Hit Data")]
        public float Unknown4 { get { return unk4; } set { unk4 = value; SignalPropertyChange(); } }
        [Category("Hit Data")]
        public float Unknown5 { get { return unk5; } set { unk5 = value; SignalPropertyChange(); } }
        [Category("Hit Data")]
        public float Unknown6 { get { return unk6; } set { unk6 = value; SignalPropertyChange(); } }
        [Category("Hit Data")]
        public float Unknown7 { get { return unk7; } set { unk7 = value; SignalPropertyChange(); } }
        [Category("Hit Data"), TypeConverter(typeof(Bin32StringConverter))]
        public Bin32 Flags { get { return new Bin32(unk8); } set { unk8 = value.data; SignalPropertyChange(); } }

        protected override bool OnInitialize()
        {
            base.OnInitialize();

            if (_name == null)
                _name = "HitData" + Index;

            unk1 = Header->_unk1;
            unk2 = Header->_unk2;
            unk3 = Header->_unk3;
            unk4 = Header->_unk4;
            unk5 = Header->_unk5;
            unk6 = Header->_unk6;
            unk7 = Header->_unk7;
            unk8 = Header->_flags;
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
            hitData* data = (hitData*)address;
            data->_flags = unk8;
            data->_unk1 = unk1;
            data->_unk2 = unk2;
            data->_unk3 = unk3;
            data->_unk4 = unk4;
            data->_unk5 = unk5;
            data->_unk6 = unk6;
            data->_unk7 = unk7;
        }
    }
}
