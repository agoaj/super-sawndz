using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.SSBBTypes;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class MoveDefUnk17Node : MoveDefEntryNode
    {
        internal Unk17Entry* First { get { return (Unk17Entry*)WorkingUncompressed.Address; } }
        int Count = 0;

        protected override bool OnInitialize()
        {
            _extOverride = true;
            base.OnInitialize();
            if (Size % 0x1C != 0 && Size % 0x1C != 4)
                Console.WriteLine(Size % 0x1C);
            Count = WorkingUncompressed.Length / 0x1C;
            return Count > 0;
        }

        protected override void OnPopulate()
        {
            Unk17Entry* addr = First;
            for (int i = 0; i < Count; i++)
                new MoveDefUnk17EntryNode().Initialize(this, addr++, 28);
        }

        protected override int OnCalculateSize(bool force)
        {
            _lookupCount = 0;
            return Children.Count * 0x1C;
        }

        protected internal override void OnRebuild(VoidPtr address, int length, bool force)
        {
            _entryOffset = address;
            Unk17Entry* data = (Unk17Entry*)address;
            foreach (MoveDefUnk17EntryNode e in Children)
                e.Rebuild(data++, 0x1C, true);
        }
    }

    public unsafe class MoveDefUnk17EntryNode : MoveDefEntryNode
    {
        internal Unk17Entry* Header { get { return (Unk17Entry*)WorkingUncompressed.Address; } }
        public override ResourceType ResourceType { get { return ResourceType.Unknown; } }

        int boneIndex;
        float f1, f2, f3, f4, f5, f6;

        [Browsable(false)]
        public MDL0BoneNode BoneNode
        {
            get { if (Model == null) return null; if (boneIndex > Model._linker.BoneCache.Length || boneIndex < 0) return null; return (MDL0BoneNode)Model._linker.BoneCache[boneIndex]; }
            set { boneIndex = value.BoneIndex; Name = value.Name; }
        }

        [Category("Unknown Entry"), Browsable(true), TypeConverter(typeof(DropDownListBonesMDef))]
        public string Bone { get { return BoneNode == null ? boneIndex.ToString() : BoneNode.Name; } set { if (Model == null) { boneIndex = Convert.ToInt32(value); Name = boneIndex.ToString(); } else { BoneNode = String.IsNullOrEmpty(value) ? BoneNode : Model.FindBone(value); } SignalPropertyChange(); } }
        [Category("Unknown Entry")]
        public float Float1 { get { return f1; } set { f1 = value; SignalPropertyChange(); } }
        [Category("Unknown Entry")]
        public float Float2 { get { return f2; } set { f2 = value; SignalPropertyChange(); } }
        [Category("Unknown Entry")]
        public float Float3 { get { return f3; } set { f3 = value; SignalPropertyChange(); } }
        [Category("Unknown Entry")]
        public float Float4 { get { return f4; } set { f4 = value; SignalPropertyChange(); } }
        [Category("Unknown Entry")]
        public float Float5 { get { return f5; } set { f5 = value; SignalPropertyChange(); } }
        [Category("Unknown Entry")]
        public float Float6 { get { return f6; } set { f6 = value; SignalPropertyChange(); } }

        public override string Name
        {
            get { return Bone; }
            //set { base.Name = value; }
        }

        protected override bool OnInitialize()
        {
            boneIndex = Header->_boneIndex;

            f1 = Header->_unkVec1._x;
            f2 = Header->_unkVec1._y;
            f3 = Header->_unkVec1._z;
            f4 = Header->_unkVec2._x;
            f5 = Header->_unkVec2._y;
            f6 = Header->_unkVec2._z;
            return false;
        }

        protected override int OnCalculateSize(bool force)
        {
            _lookupCount = 0;
            return 0x1C;
        }

        protected internal override void OnRebuild(VoidPtr address, int length, bool force)
        {
            _entryOffset = address;
            Unk17Entry* data = (Unk17Entry*)address;
            data->_boneIndex = boneIndex;
            data->_unkVec1._x = f1;
            data->_unkVec1._y = f2;
            data->_unkVec1._z = f3;
            data->_unkVec2._x = f4;
            data->_unkVec2._y = f5;
            data->_unkVec2._z = f6;
        }
    }

    public unsafe class MoveDefUnk23Node : MoveDefEntryNode
    {
        internal DataUnk23* Header { get { return (DataUnk23*)WorkingUncompressed.Address; } }
        public override ResourceType ResourceType { get { return ResourceType.Unknown; } }

        int boneIndex;
        float f1, f2, f3, f4, f5, f6, f7;

        [Browsable(false)]
        public MDL0BoneNode BoneNode
        {
            get { if (Model == null) return null; if (boneIndex > Model._linker.BoneCache.Length || boneIndex < 0) return null; return (MDL0BoneNode)Model._linker.BoneCache[boneIndex]; }
            set { boneIndex = value.BoneIndex; Name = value.Name; }
        }

        [Category("Unknown Entry"), Browsable(true), TypeConverter(typeof(DropDownListBonesMDef))]
        public string Bone { get { return BoneNode == null ? boneIndex.ToString() : BoneNode.Name; } set { if (Model == null) { boneIndex = Convert.ToInt32(value); Name = boneIndex.ToString(); } else { BoneNode = String.IsNullOrEmpty(value) ? BoneNode : Model.FindBone(value); } SignalPropertyChange(); } }
        [Category("Unknown Entry")]
        public float Float1 { get { return f1; } set { f1 = value; SignalPropertyChange(); } }
        [Category("Unknown Entry")]
        public float Float2 { get { return f2; } set { f2 = value; SignalPropertyChange(); } }
        [Category("Unknown Entry")]
        public float Float3 { get { return f3; } set { f3 = value; SignalPropertyChange(); } }
        [Category("Unknown Entry")]
        public float Float4 { get { return f4; } set { f4 = value; SignalPropertyChange(); } }
        [Category("Unknown Entry")]
        public float Float5 { get { return f5; } set { f5 = value; SignalPropertyChange(); } }
        [Category("Unknown Entry")]
        public float Float6 { get { return f6; } set { f6 = value; SignalPropertyChange(); } }
        [Category("Unknown Entry")]
        public float Float7 { get { return f7; } set { f7 = value; SignalPropertyChange(); } }

        protected override bool OnInitialize()
        {
            base.OnInitialize();
            _name = "Unknown 23";
            boneIndex = Header->_boneIndex;
            f1 = Header->_unk1;
            f2 = Header->_unk2;
            f3 = Header->_unk3;
            f4 = Header->_unk4;
            f5 = Header->_unk5;
            f6 = Header->_unk6;
            f7 = Header->_unk7;
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
            DataUnk23* data = (DataUnk23*)address;
            data->_boneIndex = boneIndex;
            data->_unk1 = f1;
            data->_unk2 = f2;
            data->_unk3 = f3;
            data->_unk4 = f4;
            data->_unk5 = f5;
            data->_unk6 = f6;
            data->_unk7 = f7;
        }
    }
}
