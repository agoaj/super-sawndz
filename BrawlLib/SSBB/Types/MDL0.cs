using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using BrawlLib.Wii.Models;
using BrawlLib.Wii.Graphics;
using BrawlLib.Imaging;

namespace BrawlLib.SSBBTypes
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MDL0Header
    {
        public const uint Size = 16;
        public const uint Tag = 0x304C444D;

        public BRESCommonHeader _header;

        public MDL0Header(int length, int version)
        {
            _header._tag = Tag;
            _header._size = length;
            _header._version = version;
            _header._bresOffset = 0;
        }

        internal byte* Address { get { fixed (void* ptr = &this)return (byte*)ptr; } }

        public bint* Offsets { get { return (bint*)(Address + 0x10); } }

        public string ResourceString { get { return new String((sbyte*)ResourceStringAddress); } }
        public VoidPtr ResourceStringAddress
        {
            get { return (VoidPtr)Address + StringOffset; }
            set { StringOffset = (int)value - (int)Address; }
        }

        public ResourceGroup* GetEntry(int index)
        {
            int offset = Offsets[index];
            if (offset == 0)
                return null;
            return (ResourceGroup*)(Address + offset);
        }

        public Part2Data* Part2 { get { return (_part2Offset > 0) ? (Part2Data*)(Address + _part2Offset) : null; } }

        public int _part2Offset
        {
            get
            {
                switch (_header._version)
                {
                    //case 0x08:
                    //case 0x09:
                    //    return *(bint*)(Address + 0x38);
                    case 0x0A:
                        return *(bint*)(Address + 0x40);
                    case 0x0B:
                        return *(bint*)(Address + 0x44);
                    default:
                        return 0;
                }
            }
            set
            {
                switch (_header._version)
                {
                    //case 0x08:
                    //case 0x09:
                    //    *(bint*)(Address + 0x38) = value; break;
                    case 0x0A:
                        *(bint*)(Address + 0x40) = value; break;
                    case 0x0B:
                        *(bint*)(Address + 0x44) = value; break;
                }
            }
        }

        public int StringOffset
        {
            get
            {
                switch (_header._version)
                {
                    case 0x08:
                    case 0x09:
                        return *(bint*)(Address + 0x3C);
                    case 0x0A:
                        return *(bint*)(Address + 0x44);
                    case 0x0B:
                        return *(bint*)(Address + 0x48);
                    default:
                        return *(bint*)(Address + 0x3C);
                }
            }
            set
            {
                switch (_header._version)
                {
                    case 0x08:
                    case 0x09:
                        *(bint*)(Address + 0x3C) = value; break;
                    case 0x0A:
                        *(bint*)(Address + 0x44) = value; break;
                    case 0x0B:
                        *(bint*)(Address + 0x48) = value; break;
                }
            }
        }

        public MDL0Props* Properties
        {
            get
            {
                switch (_header._version)
                {
                    case 0x08:
                    case 0x09:
                        return (MDL0Props*)(Address + 0x40);
                    case 0x0A:
                        return (MDL0Props*)(Address + 0x48);
                    case 0x0B:
                        return (MDL0Props*)(Address + 0x4C);
                    default:
                        return null;
                }
            }
            set
            {
                switch (_header._version)
                {
                    case 0x08:
                    case 0x09:
                        *(MDL0Props*)(Address + 0x40) = *value; break;
                    case 0x0A:
                        *(MDL0Props*)(Address + 0x48) = *value; break;
                    case 0x0B:
                        *(MDL0Props*)(Address + 0x4C) = *value; break;
                }
            }
        }

        public void* GetResource(MDLResourceType type, int entryId)
        {
            if (entryId < 0)
                return null;

            int groupId = ModelLinker.IndexBank[_header._version].IndexOf(type);
            if (groupId < 0)
                return null;

            byte* addr;
            fixed (void* p = &this)
                addr = (byte*)p;
            int offset = *((bint*)addr + 4 + groupId);
            if (offset > 0)
            {
                ResourceGroup* pGroup = (ResourceGroup*)(addr + offset);
                return (byte*)pGroup + (&pGroup->_first)[entryId + 1]._dataOffset;
            }
            return null;
        }
    }

    //Immediately after header, separate entity
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MDL0Props
    {
        public const uint Size = 0x40;

        public buint _headerLen; //0x40
        public bint _mdl0Offset;
        public bint _unk1; //0x00 or 0x02
        public bint _unk2; //0x00
        public bint _numVertices; //Length/offset?
        public bint _numFaces; //Length/offset?
        public bint _unk3; //0x00
        public bint _numNodes;
        public byte _unk4; //0x01
        public byte _unk5; //0x01
        public bshort _unk6; //0x00
        public buint _dataOffset; //0x40
        public BVec3 _minExtents;
        public BVec3 _maxExtents;

        public MDL0Props(int version, int vertices, int faces, int nodes, int unk1, int unk2, int unk3, int unk4, int unk5, int unk6, Vector3 min, Vector3 max)
        {
            _headerLen = 0x40;
            if (version == 9 || version == 8)
                _mdl0Offset = -64;
            else
                _mdl0Offset = -76;
            _unk1 = unk1;
            _unk2 = unk2;
            _numVertices = vertices;
            _numFaces = faces;
            _unk3 = unk3;
            _numNodes = nodes;
            _unk4 = (byte)unk4;
            _unk5 = (byte)unk5;
            _unk6 = (short)unk6;
            _dataOffset = 0x40;
            _minExtents = min;
            _maxExtents = max;
        }

        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }

        public MDL0Header* MDL0 { get { return (MDL0Header*)(Address + _mdl0Offset); } }

        public MDL0NodeTable* IndexTable { get { return (MDL0NodeTable*)((VoidPtr)Address + _dataOffset); } }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MDL0CommonHeader
    {
        public bint _size;
        public bint _mdlOffset;

        internal void* Address { get { fixed (void* p = &this)return p; } }
        public MDL0Header* MDL0Header { get { return (MDL0Header*)((byte*)Address + _mdlOffset); } }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MDL0NodeTable
    {
        public bint _numEntries;

        private void* Address { get { fixed (void* ptr = &this)return ptr; } }

        public bint* First { get { return (bint*)Address + 1; } }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MDL0DefEntry
    {
        public byte _type;

        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }

        public MDL0NodeType2* Type2Data { get { return (MDL0NodeType2*)(Address + 1); } }
        public MDL0NodeType3* Type3Data { get { return (MDL0NodeType3*)(Address + 1); } }
        public MDL0NodeType4* Type4Data { get { return (MDL0NodeType4*)(Address + 1); } }
        public MDL0NodeType5* Type5Data { get { return (MDL0NodeType5*)(Address + 1); } }

        public MDL0DefEntry* Next
        {
            get
            {
                switch (_type)
                {
                    case 2: return (MDL0DefEntry*)(Type2Data + 1);
                    case 3: return (MDL0DefEntry*)(Type3Data + 1);
                    case 4: return (MDL0DefEntry*)(Type4Data + 1);
                    case 5: return (MDL0DefEntry*)(Type5Data + 1);
                }
                return null;
            }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe abstract class MDL0NodeClass
    {
        public static object Create(ref VoidPtr addr)
        {
            object n = null;
            switch (*(byte*)addr++)
            {
                case 2: { n = Marshal.PtrToStructure(addr, typeof(MDL0Node2Class)); addr += MDL0Node2Class.Size; break; }
                case 3: { n = new MDL0Node3Class((MDL0NodeType3*)addr); addr += ((MDL0Node3Class)n).GetSize(); break; }
                case 4: { n = Marshal.PtrToStructure(addr, typeof(MDL0NodeType4)); addr += MDL0NodeType4.Size; break; }
                case 5: { n = Marshal.PtrToStructure(addr, typeof(MDL0NodeType5)); addr += MDL0NodeType5.Size; break; }
            }
            return n;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe class MDL0Node2Class : MDL0NodeClass
    {
        public const uint Size = 0x04;

        public bushort _boneIndex;
        public bushort _parentNodeIndex;

        public ushort BoneIndex { get { return _boneIndex; } set { _boneIndex = value; } }
        public ushort ParentNodeIndex { get { return _parentNodeIndex; } set { _parentNodeIndex = value; } }
        
        public override string ToString()
        {
            return string.Format("Type2 (Bone Index:{0}, Parent Node Index:{1})", BoneIndex, ParentNodeIndex);
        }
    }

    public unsafe class MDL0Node3Class
    {
        public bushort _id;
        public List<MDL0NodeType3Entry> _entries = new List<MDL0NodeType3Entry>();

        public unsafe MDL0Node3Class(MDL0NodeType3* ptr)
        {
            _id = ptr->_id;
            for (int i = 0; i < ptr->_numEntries; i++)
                _entries.Add(ptr->Entries[i]);
        }

        public ushort Id { get { return _id; } set { _id = value; } }
        public MDL0NodeType3Entry[] Entries { get { return _entries.ToArray(); } }

        public int GetSize() { return 3 + (_entries.Count * MDL0NodeType3Entry.Size); }

        public override string ToString()
        {
            return string.Format("Type3 (ID:{0})", Id);
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MDL0NodeType2
    {
        public const int Size = 0x04;

        public bushort _index;
        public bushort _parentId;

        public ushort Index { get { return _index; } set { _index = value; } }
        public ushort ParentId { get { return _parentId; } set { _parentId = value; } }

        public override string ToString()
        {
            return string.Format("Type2 (Index:{0},ParentID:{1})", Index, ParentId);
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MDL0NodeType3
    {
        public const int Size = 0x03;

        public bushort _id;
        public byte _numEntries;

        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }

        public MDL0NodeType3Entry* Entries { get { return (MDL0NodeType3Entry*)(Address + 3); } }

    }
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MDL0NodeType3Entry
    {
        public const int Size = 0x06;

        public bushort _id;
        public bfloat _value;

        public ushort Id { get { return _id; } set { _id = value; } }
        public float Value { get { return _value; } set { _value = value; } }

        public override string ToString()
        {
            return String.Format("Type3Entry (ID:{0},Weight:{1})", Id, Value);
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MDL0NodeType4
    {
        public const uint Size = 0x07;

        public bushort _materialIndex;
        public bushort _polygonIndex;
        public bushort _boneIndex;
        public byte _val4;

        public ushort MaterialId { get { return _materialIndex; } set { _materialIndex = value; } }
        public ushort PolygonId { get { return _polygonIndex; } set { _polygonIndex = value; } }
        public ushort BoneIndex { get { return _boneIndex; } set { _boneIndex = value; } }
        public byte Val4 { get { return _val4; } set { _val4 = value; } }

        public override string ToString()
        {
            return string.Format("Type4 (MatID:{0},PolyID:{1},BoneIndex:{2},Unk:{3})", MaterialId, PolygonId, BoneIndex, Val4);
        }
    }

    //Links node IDs with indexes
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MDL0NodeType5
    {
        public const uint Size = 0x04;

        public bushort _id; //Node Id
        public bushort _index; //Node Index

        public int Id { get { return _id; } set { _id = (ushort)value; } }
        public int Index { get { return _index; } set { _index = (ushort)value; } }

        public override string ToString()
        {
            return string.Format("Type5 (ID:{0},Index:{1})", Id, Index);
        }
    }

    [Flags]
    public enum BoneFlags : uint
    {
        NoTransform      = 0x1,
        FixedTranslation = 0x2,
        FixedRotation    = 0x4,
        FixedScale       = 0x8,
        Common5          = 0x10,
        HasGeomParent    = 0x20,
        HasGeomChildren  = 0x40,
        Unk8             = 0x80,
        Visible          = 0x100,
        HasGeometry      = 0x200,
        Unk11            = 0x400,
        Unk12            = 0x800,
        Unk13            = 0x1000,
        Unk14            = 0x2000,
        Unk15            = 0x4000,
        Unk16            = 0x8000,
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MDL0Bone
    {
        public bint _headerLen;
        public bint _mdl0Offset;
        public bint _stringOffset;
        public bint _index;

        public bint _nodeId;
        public buint _flags;
        public buint _pad1;
        public buint _pad2;

        public BVec3 _scale;
        public BVec3 _rotation;
        public BVec3 _translation;
        public BVec3 _boxMin;
        public BVec3 _boxMax;

        public bint _parentOffset;
        public bint _firstChildOffset;
        public bint _nextOffset;
        public bint _prevOffset;
        public bint _part2Offset;

        public bMatrix43 _transform;
        public bMatrix43 _transformInv;

        public VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }

        public Part2Data* Part2 { get { return (Part2Data*)(Address + _part2Offset); } }

        public string ResourceString { get { return new String((sbyte*)ResourceStringAddress); } }
        public VoidPtr ResourceStringAddress
        {
            get { return (VoidPtr)Address + _stringOffset; }
            set { _stringOffset = (int)value - (int)Address; }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MDL0VertexData
    {
        public bint _dataLen; //including header
        public bint _mdl0Offset;
        public bint _dataOffset; //0x40
        public bint _stringOffset;
        public bint _index;
        public bint _isXYZ;
        public bint _type;
        public byte _divisor;
        public byte _entryStride;
        public bshort _numVertices;
        public BVec3 _eMin;
        public BVec3 _eMax;
        public bint _pad1;
        public bint _pad2;

        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }
        public VoidPtr Data 
        { 
            get
            {
                //if (_dataOffset != null)
                //    return Address + _dataOffset;
                //else
                    return Address + 0x40;
            } 
        }

        public WiiVertexComponentType Type { get { return (WiiVertexComponentType)(int)_type; } }

        public string ResourceString { get { return new String((sbyte*)this.ResourceStringAddress); } }
        public VoidPtr ResourceStringAddress
        {
            get { return (VoidPtr)this.Address + _stringOffset; }
            set { _stringOffset = (int)value - (int)Address; }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MDL0NormalData
    {
        public bint _dataLen; //includes header/padding
        public bint _mdl0Offset;
        public bint _dataOffset; //0x20
        public bint _stringOffset;
        public bint _index;
        public bint _isNBT;
        public bint _type;
        public byte _divisor;
        public byte _entryStride;
        public bushort _numVertices;

        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }
        public VoidPtr Data { get { return Address + _dataOffset; } }

        public WiiVertexComponentType Type { get { return (WiiVertexComponentType)(int)_type; } }

        public string ResourceString { get { return new String((sbyte*)this.ResourceStringAddress); } }
        public VoidPtr ResourceStringAddress
        {
            get { return (VoidPtr)this.Address + _stringOffset; }
            set { _stringOffset = (int)value - (int)Address; }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MDL0ColorData
    {
        public bint _dataLen; //includes header/padding
        public bint _mdl0Offset;
        public bint _dataOffset; //0x20
        public bint _stringOffset;
        public bint _index;
        public bint _isRGBA;
        public bint _format;
        public byte _entryStride;
        public byte _scale;
        public bushort _numEntries;

        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }
        public VoidPtr Data { get { return Address + _dataOffset; } }

        public WiiColorComponentType Type { get { return (WiiColorComponentType)(int)_format; } }

        public string ResourceString { get { return new String((sbyte*)this.ResourceStringAddress); } }
        public VoidPtr ResourceStringAddress
        {
            get { return (VoidPtr)this.Address + _stringOffset; }
            set { _stringOffset = (int)value - (int)Address; }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MDL0UVData
    {
        public bint _dataLen; //includes header/padding
        public bint _mdl0Offset;
        public bint _dataOffset; //0x40
        public bint _stringOffset;
        public bint _index;
        public bint _isST;
        public bint _format;
        public byte _divisor;
        public byte _entryStride;
        public bushort _numEntries;
        public BVec2 _min;
        public BVec2 _max;
        public int _pad1, _pad2, _pad3, _pad4;

        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }
        public BVec2* Entries { get { return (BVec2*)(Address + _dataOffset); } }

        public WiiVertexComponentType Type { get { return (WiiVertexComponentType)(int)_format; } }

        public string ResourceString { get { return new String((sbyte*)this.ResourceStringAddress); } }
        public VoidPtr ResourceStringAddress
        {
            get { return (VoidPtr)this.Address + _stringOffset; }
            set { _stringOffset = (int)value - (int)Address; }
        }
    }

    public enum CullMode : int
    {
        Cull_None = 0,
        Cull_Outside = 1,
        Cull_Inside = 2,
        Cull_All = 3
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TextureFlags
    {
        public BVec2 TexScale;
        public bfloat TexRotation;
        public BVec2 TexTranslation;

        public static readonly TextureFlags Default = new TextureFlags()
        {
            TexScale = new Vector2(1),
            TexRotation = 0,
            TexTranslation = new Vector2(0)
        };
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TextureMatrix
    {
        public sbyte TexUnk1;
        public sbyte TexUnk2;
        public sbyte TexUnk3;
        public sbyte TexUnk4;
        public bMatrix43 TexMtx;

        public static readonly TextureMatrix Default = new TextureMatrix()
        {
            TexUnk1 = -1,
            TexUnk2 = -1,
            TexUnk3 = 0,
            TexUnk4 = 1,
            TexMtx = Matrix43.Identity
        };
    }


    [Flags]
    public enum TexFlags
    {
        Enabled = 0x1,
        FixedScale = 0x2,
        FixedRot = 0x4,
        FixedTrans = 0x8
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MDL0MtlTexSettings
    {
        public static readonly MDL0MtlTexSettings Default = new MDL0MtlTexSettings()
        {
            Tex1Flags = TextureFlags.Default,
            Tex2Flags = TextureFlags.Default,
            Tex3Flags = TextureFlags.Default,
            Tex4Flags = TextureFlags.Default,
            Tex5Flags = TextureFlags.Default,
            Tex6Flags = TextureFlags.Default,
            Tex7Flags = TextureFlags.Default,
            Tex8Flags = TextureFlags.Default,

            Tex1Matrices = TextureMatrix.Default,
            Tex2Matrices = TextureMatrix.Default,
            Tex3Matrices = TextureMatrix.Default,
            Tex4Matrices = TextureMatrix.Default,
            Tex5Matrices = TextureMatrix.Default,
            Tex6Matrices = TextureMatrix.Default,
            Tex7Matrices = TextureMatrix.Default,
            Tex8Matrices = TextureMatrix.Default,
        };

        public buint LayerFlags;
        public buint UnkFlags;

        public TextureFlags Tex1Flags;
        public TextureFlags Tex2Flags;
        public TextureFlags Tex3Flags;
        public TextureFlags Tex4Flags;
        public TextureFlags Tex5Flags;
        public TextureFlags Tex6Flags;
        public TextureFlags Tex7Flags;
        public TextureFlags Tex8Flags;

        public TextureMatrix Tex1Matrices;
        public TextureMatrix Tex2Matrices;
        public TextureMatrix Tex3Matrices;
        public TextureMatrix Tex4Matrices;
        public TextureMatrix Tex5Matrices;
        public TextureMatrix Tex6Matrices;
        public TextureMatrix Tex7Matrices;
        public TextureMatrix Tex8Matrices;

        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }

        public TextureFlags GetTexFlags(int Index) { return *(TextureFlags*)((byte*)Address + 8 + (Index * 20)); }
        public void SetTexFlags(TextureFlags value, int Index) { *(TextureFlags*)((byte*)Address + 8 + (Index * 20)) = value; }
        
        public TextureMatrix GetTexMatrices(int Index) { return *(TextureMatrix*)((byte*)Address + 168 + (Index * 52)); }
        public void SetTexMatrices(TextureMatrix value, int Index) { *(TextureMatrix*)((byte*)Address + 168 + (Index * 52)) = value; }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MDL0MaterialLighting
    {
        public buint flags0;
        public RGBAPixel c00;
        public RGBAPixel c01;
        public short pad0;
        public byte unk00;
        public byte unk01;
        public short pad1;
        public byte unk02;
        public byte unk03;
        
        public buint flags1;
        public RGBAPixel c10;
        public RGBAPixel c11;
        public short pad2;
        public byte unk10;
        public byte unk11;
        public short pad3;
        public byte unk12;
        public byte unk13;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Bin32 //For reading flags
    {
        public uint data;

        public Bin32(uint val) { data = val; }

        public override string ToString()
        {
            int i = 0;
            string val = "";
            while (i++ < 32)
            {
                val += (data >> (32 - i)) & 1;
                if (i % 4 == 0 && i != 32)
                    val += " ";
            }
            return val;
        }

        public bool this[int index]
        {
            get { return (data >> index & 1) != 0; }
            set
            {
                if (value)
                    data |= (uint)(1 << index);
                else
                    data &= ~(uint)(1 << index);
            }
        }

        //public uint this[int shift, int mask]
        //{
        //    get { return (uint)(data >> shift & mask); }
        //    set { data = (uint)((data & ~(mask << shift)) | ((value & mask) << shift)); }
        //}

        public uint this[int shift, int bitCount]
        {
            get
            {
                int mask = 0;
                for (int i = 0; i < bitCount; i++)
                    mask += 1 << i;
                return (uint)(data >> shift & mask);
            }
            set
            {
                int mask = 0;
                for (int i = 0; i < bitCount; i++)
                    mask += 1 << i;
                data = (uint)((data & ~(mask << shift)) | ((value & mask) << shift));
            }
        }
    }
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Bin16 //For reading flags
    {
        public ushort data;

        public Bin16(ushort val) { data = val; }

        public override string ToString()
        {
            int i = 0;
            string val = "";
            while (i++ < 16)
            {
                val += (data >> (16 - i)) & 1;
                if (i % 4 == 0 && i != 16)
                    val += " ";
            }
            return val;
        }

        public bool this[int index]
        {
            get { return (data >> index & 1) != 0; } 
            set 
            {
                if (value)
                    data |= (ushort)(1 << index);
                else
                    data &= (ushort)~(1 << index);
            }
        }

        //public ushort this[int shift, int mask]
        //{
        //    get { return (ushort)(data >> shift & mask); }
        //    set { data = (ushort)((data & ~(mask << shift)) | ((value & mask) << shift)); }
        //}

        public ushort this[int shift, int bitCount]
        {
            get
            {
                int mask = 0;
                for (int i = 0; i < bitCount; i++)
                    mask += 1 << i;
                return (ushort)(data >> shift & mask); 
            }
            set
            {
                int mask = 0;
                for (int i = 0; i < bitCount; i++)
                    mask += 1 << i;
                data = (ushort)((data & ~(mask << shift)) | ((value & mask) << shift)); 
            }
        }
    }
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Bin8 //For reading flags
    {
        public byte data;

        public Bin8(byte val) { data = val; }

        public override string ToString()
        {
            int i = 0;
            string val = "";
            while (i++ < 8)
            {
                val += (data >> (8 - i)) & 1;
                if (i % 4 == 0 && i != 8)
                    val += " ";
            }
            return val;
        }

        public bool this[int index]
        {
            get { return (data >> index & 1) != 0; }
            set
            {
                if (value)
                    data |= (byte)(1 << index);
                else
                    data &= (byte)~(1 << index);
            }
        }

        //public byte this[int shift, int mask]
        //{
        //    get { return (byte)(data >> shift & mask); }
        //    set { data = (byte)((data & ~(mask << shift)) | ((value & mask) << shift)); }
        //}

        public byte this[int shift, int bitCount]
        {
            get
            {
                int mask = 0;
                for (int i = 0; i < bitCount; i++)
                    mask += 1 << i;
                return (byte)(data >> shift & mask);
            }
            set
            {
                int mask = 0;
                for (int i = 0; i < bitCount; i++)
                    mask += 1 << i;
                data = (byte)((data & ~(mask << shift)) | ((value & mask) << shift));
            }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MDL0Material
    {
        public const int Size = 64;

        public bint _dataLen;
        public bint _mdl0Offset;
        public bint _stringOffset;
        public bint _index;
        public buint _isXLU; //0x00 or 0x80000000 for XLU textures
        public byte _numTexGens;
        public byte _numLightChans;
        public byte _activeTEVStages;
        public byte _numIndTexStages;
        public bint _cull; //0x02, XLU = 0
        public byte _enableAlphaTest;
        public sbyte _lightSet;
        public sbyte _fogSet;
        public byte _unk1;
        public bint _unk2;
        public bint _unk3; //-1
        public bint _shaderOffset;
        public bint _numTextures;
        public bint _matRefOffset; //1044 for v8 & v9 or 1048 for v10 & v11 MDL0
        public bint _part2Offset;

        //Offset to display list(s).
        public bint _dlOffset_08_09; 
        public bint _dlOffset_10_11;
        
        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }
        public MDL0Header* Parent { get { return (MDL0Header*)(Address + _mdl0Offset); } }

        public MDL0TextureRef* First { get { return (_matRefOffset != 0) ? (MDL0TextureRef*)(Address + _matRefOffset) : null; } }
        public Part2Data* Part2 { get { return (_part2Offset != 0) ? (Part2Data*)(Address + _part2Offset) : null; } }

        public int DisplayListOffset(int version)
        {
            switch (version)
            {
                case 10:
                case 11:
                    return _dlOffset_10_11;
                default:
                    return _dlOffset_08_09;
            }
        }

        public MDL0MtlTexSettings* TexMatrices(int version)
        {
            switch (version)
            {
                case 10:
                case 11:
                    return (MDL0MtlTexSettings*)(Address + 0x1A8);
                default:
                    return (MDL0MtlTexSettings*)(Address + 0x1A4);
            }
        }

        public MDL0MaterialLighting* Light(int version)
        {
            switch (version)
            {
                case 10:
                case 11:
                    return (MDL0MaterialLighting*)(Address + 0x3F0);
                default:
                    return (MDL0MaterialLighting*)(Address + 0x3EC);
            }
        }

        public MatModeBlock* DisplayLists(int version) { return (MatModeBlock*)(Address + DisplayListOffset(version)); }
        public MatTevColorBlock* TevColorBlock(int version) { return (MatTevColorBlock*)(Address + DisplayListOffset(version) + MatModeBlock.Size); }
        public MatTevKonstBlock* TevKonstBlock(int version) { return (MatTevKonstBlock*)(Address + DisplayListOffset(version) + MatModeBlock.Size + MatTevColorBlock.Size); }

        public string ResourceString { get { return new String((sbyte*)ResourceStringAddress); } }
        public VoidPtr ResourceStringAddress
        {
            get { return (VoidPtr)Address + _stringOffset; }
            set { _stringOffset = (int)value - (int)Address; }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MatModeBlock
    {
        public const int Size = 32;
        public static readonly MatModeBlock Default = new MatModeBlock()
        {
            _alphafuncCmd = 0xF361,
            AlphaFunction = AlphaFunction.Default,
            _zmodeCmd = 0x4061,
            ZMode = ZMode.Default,
            _maskCmd = 0xFE61,
            _mask1 = 0xFF,
            _mask2 = 0xE3,
            _blendmodeCmd = 0x4161,
            BlendMode = BlendMode.Default,
            _constAlphaCmd = 0x4261,
            ConstantAlpha = ConstantAlpha.Default
        };

        private ushort _alphafuncCmd;
        public AlphaFunction AlphaFunction;
        private ushort _zmodeCmd;
        public ZMode ZMode;
        private ushort _maskCmd;
        private byte _mask0, _mask1, _mask2;
        private ushort _blendmodeCmd;
        public BlendMode BlendMode;
        private ushort _constAlphaCmd;
        public ConstantAlpha ConstantAlpha;
        private fixed byte _pad[7];
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MatTevColorBlock
    {
        public const int Size = 64;
        public static readonly MatTevColorBlock Default = new MatTevColorBlock()
        {
            _tr1LCmd = 0xE261,
            _tr1HCmd0 = 0xE361,
            _tr1HCmd1 = 0xE361,
            _tr1HCmd2 = 0xE361,
            _tr2LCmd = 0xE461,
            _tr2HCmd0 = 0xE561,
            _tr2HCmd1 = 0xE561,
            _tr2HCmd2 = 0xE561,
            _tr3LCmd = 0xE661,
            _tr3HCmd0 = 0xE761,
            _tr3HCmd1 = 0xE761,
            _tr3HCmd2 = 0xE761
        };

        private ushort _tr1LCmd;
        public ColorReg TevReg1Lo;
        private ushort _tr1HCmd0;
        public ColorReg TevReg1Hi0;
        private ushort _tr1HCmd1;
        public ColorReg TevReg1Hi1;
        private ushort _tr1HCmd2;
        public ColorReg TevReg1Hi2;

        private ushort _tr2LCmd;
        public ColorReg TevReg2Lo;
        private ushort _tr2HCmd0;
        public ColorReg TevReg2Hi0;
        private ushort _tr2HCmd1;
        public ColorReg TevReg2Hi1;
        private ushort _tr2HCmd2;
        public ColorReg TevReg2Hi2;

        private ushort _tr3LCmd;
        public ColorReg TevReg3Lo;
        private ushort _tr3HCmd0;
        public ColorReg TevReg3Hi0;
        private ushort _tr3HCmd1;
        public ColorReg TevReg3Hi1;
        private ushort _tr3HCmd2;
        public ColorReg TevReg3Hi2;

        private fixed byte _pad[4];
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MatTevKonstBlock
    {
        public const int Size = 64;
        public static readonly MatTevKonstBlock Default = new MatTevKonstBlock()
        {
            _tr0LoCmd = 0xE061,
            TevReg0Lo = ColorReg.Konstant,
            _tr0HiCmd = 0xE161,
            TevReg0Hi = ColorReg.Konstant,
            _tr1LoCmd = 0xE261,
            TevReg1Lo = ColorReg.Konstant,
            _tr1HiCmd = 0xE361,
            TevReg1Hi = ColorReg.Konstant,
            _tr2LoCmd = 0xE461,
            TevReg2Lo = ColorReg.Konstant,
            _tr2HiCmd = 0xE561,
            TevReg2Hi = ColorReg.Konstant,
            _tr3LoCmd = 0xE661,
            TevReg3Lo = ColorReg.Konstant,
            _tr3HiCmd = 0xE761,
            TevReg3Hi = ColorReg.Konstant,

            bpReg1 = 0x61,
            RAS1_SS0 = BPMemory.BPMEM_RAS1_SS0,
            bpReg2 = 0x61,
            RAS1_SS1 = BPMemory.BPMEM_RAS1_SS1
        };

        private ushort _tr0LoCmd;
        public ColorReg TevReg0Lo;
        private ushort _tr0HiCmd;
        public ColorReg TevReg0Hi;
        private ushort _tr1LoCmd;
        public ColorReg TevReg1Lo;
        private ushort _tr1HiCmd;
        public ColorReg TevReg1Hi;
        private ushort _tr2LoCmd;
        public ColorReg TevReg2Lo;
        private ushort _tr2HiCmd;
        public ColorReg TevReg2Hi;
        private ushort _tr3LoCmd;
        public ColorReg TevReg3Lo;
        private ushort _tr3HiCmd;
        public ColorReg TevReg3Hi;

        private fixed byte _pad1[24];

        private byte bpReg1;
        public BPMemory RAS1_SS0;
        public RAS1_SS SS0val;
        private byte bpReg2;
        public BPMemory RAS1_SS1;
        public RAS1_SS SS1val;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct KSelSwapBlock
    {
        public const int Size = 64;
        public static readonly KSelSwapBlock Default = new KSelSwapBlock()
        {
            Reg00 = 0x61,
            Reg01 = 0x61,
            Reg02 = 0x61,
            Reg03 = 0x61,
            Reg04 = 0x61,
            Reg05 = 0x61,
            Reg06 = 0x61,
            Reg07 = 0x61,
            Reg08 = 0x61,
            Reg09 = 0x61,
            Reg10 = 0x61,
            Reg11 = 0x61,
            Reg12 = 0x61,
            Reg13 = 0x61,
            Reg14 = 0x61,
            Reg15 = 0x61,
            Reg16 = 0x61,

            Mem00 = (BPMemory)0xFE,
            _Value00 = new Int24(0xF),
            Mem01 = (BPMemory)0xF6,
            _Value01 = new KSel(0x4),
            Mem02 = (BPMemory)0xFE,
            _Value02 = new Int24(0xF),
            Mem03 = (BPMemory)0xF7,
            _Value03 = new KSel(0xE),
            Mem04 = (BPMemory)0xFE,
            _Value04 = new Int24(0xF),
            Mem05 = (BPMemory)0xF8,
            _Value05 = new KSel(0x0),
            Mem06 = (BPMemory)0xFE,
            _Value06 = new Int24(0xF),
            Mem07 = (BPMemory)0xF9,
            _Value07 = new KSel(0xC),
            Mem08 = (BPMemory)0xFE,
            _Value08 = new Int24(0xF),
            Mem09 = (BPMemory)0xFA,
            _Value09 = new KSel(0x5),
            Mem10 = (BPMemory)0xFE,
            _Value10 = new Int24(0xF),
            Mem11 = (BPMemory)0xFB,
            _Value11 = new KSel(0xD),
            Mem12 = (BPMemory)0xFE,
            _Value12 = new Int24(0xF),
            Mem13 = (BPMemory)0xFC,
            _Value13 = new KSel(0xA),
            Mem14 = (BPMemory)0xFE,
            _Value14 = new Int24(0xF),
            Mem15 = (BPMemory)0xFD,
            _Value15 = new KSel(0xE),
            Mem16 = (BPMemory)0x27,
            _Value16 = new Int24(0xFF, 0xFF, 0xFF),
        };

        public byte Reg00; //0x61
        public BPMemory Mem00;
        public Int24 _Value00; //KSel Mask - Swap Mode
        
        public byte Reg01; //0x61
        public BPMemory Mem01;
        public KSel _Value01; //KSel 0 - RG

        public byte Reg02; //0x61
        public BPMemory Mem02;
        public Int24 _Value02; //KSel Mask - Swap Mode

        public byte Reg03; //0x61
        public BPMemory Mem03;
        public KSel _Value03; //KSel 1 - BA

        public byte Reg04; //0x61
        public BPMemory Mem04;
        public Int24 _Value04; //KSel Mask - Swap Mode

        public byte Reg05; //0x61
        public BPMemory Mem05;
        public KSel _Value05; //KSel 2 - RR

        public byte Reg06; //0x61
        public BPMemory Mem06;
        public Int24 _Value06; //KSel Mask - Swap Mode

        public byte Reg07; //0x61
        public BPMemory Mem07;
        public KSel _Value07; //KSel 3 - RA

        public byte Reg08; //0x61
        public BPMemory Mem08;
        public Int24 _Value08; //KSel Mask - Swap Mode

        public byte Reg09; //0x61
        public BPMemory Mem09;
        public KSel _Value09; //KSel 4 - GG

        public byte Reg10; //0x61
        public BPMemory Mem10;
        public Int24 _Value10; //KSel Mask - Swap Mode

        public byte Reg11; //0x61
        public BPMemory Mem11;
        public KSel _Value11; //KSel 5 - GA

        public byte Reg12; //0x61
        public BPMemory Mem12;
        public Int24 _Value12; //KSel Mask - Swap Mode

        public byte Reg13; //0x61
        public BPMemory Mem13;
        public KSel _Value13; //KSel 6 - BB

        public byte Reg14; //0x61
        public BPMemory Mem14;
        public Int24 _Value14; //KSel Mask - Swap Mode

        public byte Reg15; //0x61
        public BPMemory Mem15;
        public KSel _Value15; //KSel 7 - BA
        
        public byte Reg16; //0x61
        public BPMemory Mem16; //IREF
        public Int24 _Value16; 

        private fixed byte _pad[11];
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct StageGroup
    {
        //Carries an even and an odd stage.
        //TRef and KSel modify both stages.
        //The odd stage does not need to be used.

        public const int Size = 0x30;
        
        public BPCommand mask; //KSel Mask - Selection Mode (XRB = 0, XGA = 0)
        public BPCommand ksel; //KSel
        public BPCommand tref; //TRef
        public BPCommand eClrEnv; //Color Env Even
        public BPCommand oClrEnv; //Color Env Odd (Optional)
        public BPCommand eAlpEnv; //Alpha Env Even
        public BPCommand oAlpEnv; //Alpha Env Odd (Optional)
        public BPCommand eCMD; //CMD (Indirect Texture) Even
        public BPCommand oCMD; //CMD (Indirect Texture) Odd (Optional)

        public static readonly StageGroup Default = new StageGroup()
        {
            mask = new BPCommand(true) { Mem = BPMemory.BPMEM_BP_MASK, Data = new Int24(0xFFFFF0) },
            ksel = new BPCommand(true) { Mem = BPMemory.BPMEM_TEV_KSEL0 },
            tref = new BPCommand(true) { Mem = BPMemory.BPMEM_TREF0 },
            eClrEnv = new BPCommand(true) { Mem = BPMemory.BPMEM_TEV_COLOR_ENV_0 },
            oClrEnv = new BPCommand(false) { Mem = BPMemory.BPMEM_GENMODE },
            eAlpEnv = new BPCommand(true) { Mem = BPMemory.BPMEM_TEV_ALPHA_ENV_0 },
            oAlpEnv = new BPCommand(false) { Mem = BPMemory.BPMEM_GENMODE },
            eCMD = new BPCommand(true) { Mem = BPMemory.BPMEM_IND_CMD0 },
            oCMD = new BPCommand(false) { Mem = BPMemory.BPMEM_GENMODE },
        };

        public void SetGroup(int index)
        {
            ksel.Mem = (BPMemory)((int)BPMemory.BPMEM_TEV_KSEL0 + index);
            tref.Mem = (BPMemory)((int)BPMemory.BPMEM_TREF0 + index);    
        }

        public void SetStage(int index) 
        {
            if (index % 2 == 0) //Even
            {
                eClrEnv.Mem = (BPMemory)((int)BPMemory.BPMEM_TEV_COLOR_ENV_0 + index * 2);
                eAlpEnv.Mem = (BPMemory)((int)BPMemory.BPMEM_TEV_ALPHA_ENV_0 + index * 2);
                eCMD.Mem = (BPMemory)((int)BPMemory.BPMEM_IND_CMD0 + index * 2);  
            }
            else //Odd
            {
                oClrEnv.Reg = 
                oAlpEnv.Reg = 
                oCMD.Reg = 0x61;

                oClrEnv.Mem = (BPMemory)((int)BPMemory.BPMEM_TEV_COLOR_ENV_0 + index * 2);
                oAlpEnv.Mem = (BPMemory)((int)BPMemory.BPMEM_TEV_ALPHA_ENV_0 + index * 2);
                oCMD.Mem = (BPMemory)((int)BPMemory.BPMEM_IND_CMD0 + index * 2);  
            }
        }

        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }

        public StageGroup* Next { get { return (StageGroup*)(Address + 0x30); } }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MDL0TextureRef
    {
        public const int Size = 52;

        public bint _stringOffset;
        public bint _secondaryOffset;
        public bint _unk2;
        public bint _unk3;
        public bint _index1;
        public bint _index2;
        public bint _uWrap;
        public bint _vWrap;
        public bint _minFltr;
        public bint _magFltr;
        public bfloat _lodBias;
        public bint _unk10;
        public byte _unk11;
        public byte _unk12;
        public bshort _unk13;
        
        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }

        public string ResourceString { get { return new String((sbyte*)this.ResourceStringAddress); } }
        public VoidPtr ResourceStringAddress
        {
            get { return (VoidPtr)this.Address + _stringOffset; }
            set { _stringOffset = (int)value - (int)Address; }
        }

        public string SecondaryTexture { get { return (_secondaryOffset == 0) ? null : new String((sbyte*)this.SecondaryTextureAddress); } }
        public VoidPtr SecondaryTextureAddress
        {
            get { return Address + _secondaryOffset; }
            set { _secondaryOffset = (int)value - (int)Address; }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct Part2Data
    {
        public bint _totalLen; //including group + all entries
        
        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }
        public ResourceGroup* Group { get { return (ResourceGroup*)(Address + 4); } }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct Part2DataEntry
    {
        public bint _totalLen;
        public bint _unk1; //0x18, length without first four?
        public bint _unk2; //0x01, num entries?
        public bint _unk3; //0x00
        public bint _stringOffset; //same as entry
        public bint _unk4; //0x00
        public bint _unk5; //0x00

        public Part2DataEntry(int unk2)
        {
            _totalLen = 0x1C;
            _unk1 = 0x18;
            _unk2 = unk2;
            _unk3 = 0;
            _stringOffset = 0;
            _unk4 = 0;
            _unk5 = 0;
        }

        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }

        public string ResourceString { get { return new String((sbyte*)ResourceStringAddress); } }
        public VoidPtr ResourceStringAddress
        {
            get { return (VoidPtr)Address + _stringOffset; }
            set { _stringOffset = (int)value - (int)Address; }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MDL0Shader
    {
        public const int Size = 32;

        public bint _dataLength; //Always 512
        public bint _mdl0Offset;
        public bint _index;
        public byte _stages;
        public byte _res0, _res1, _res2; //Always 0. Reserved?
        public sbyte _ref0, _ref1, _ref2, _ref3, _ref4, _ref5, _ref6, _ref7;
        public int _pad0, _pad1; //Always 0
        
        public KSelSwapBlock* SwapBlock { get { return (KSelSwapBlock*)(Address + Size); } }

        //There are 8 structures max following the display list, each 0x30 in length.
        //Each structure has 9 commands.
        public StageGroup* First { get { return (StageGroup*)(Address + 0x80); } }
        
        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MDL0Polygon
    {
        public const uint Size = 0x64;

        public bint _totalLength;
        public bint _mdl0Offset;
        public bint _nodeId; //Single-bind node

        public CPVertexFormat _vertexFormat;
        public XFVertexSpecs _vertexSpecs;

        public bint _defSize; //Size of def block including padding? Always 0xE0?
        public bint _defFlags; //0x80, sometimes 0xA0
        public bint _defOffset; //Relative to defSize field

        public bint _dataLen1; //Size of primitives
        public bint _dataLen2; //Same as previous
        public bint _dataOffset; //Relative to dataLen1

        public XFArrayFlags _arrayFlags; //Used to enable element arrays

        public bint _unk3; //0
        public bint _stringOffset;
        public bint _index;

        public bint _numVertices;
        public bint _numFaces;

        public bshort _vertexId; 
        public bshort _normalId; 
        public fixed short _colorIds[2];        
        public fixed short _uids[8];

        public bint _nodeTableOffset;

        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }

        public MDL0Header* Parent { get { return (MDL0Header*)(Address + _mdl0Offset); } }
        public bshort* ColorIds { get { return (bshort*)(Address + 0x4C); } }
        public bshort* UVIds { get { return (bshort*)(Address + 0x50); } }

        public MDL0PolygonDefs* DefList { get { return (MDL0PolygonDefs*)(Address + 0x18 + _defOffset); } }
        public bushort* WeightIndices(int version)
        {
            if (version != 11)
                return (bushort*)(Address + 0x64);
            else
                return (bushort*)(Address + 0x68);
        }
        public VoidPtr PrimitiveData { get { return Address + 0x24 + _dataOffset; } }

        public string ResourceString { get { return new String((sbyte*)ResourceStringAddress); } }
        public VoidPtr ResourceStringAddress
        {
            get { return (VoidPtr)Address + _stringOffset; }
            set { _stringOffset = (int)value - (int)Address; }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MDL0PolygonDefs
    {
        private fixed byte pad[10];

        //CP Vertex Format
        private bushort CPSetFmtLo; //0x0850
        public buint VtxFmtLo;
        private bushort CPSetFmtHi; //0x0860
        public buint VtxFmtHi;
        
        //XF Vertex Specs
        public byte XFCmd; //0x10
        public bushort Length; //0x0000
        public bushort XFSetVtxSpecs; //0x1008
        public XFVertexSpecs VtxSpecs;
        public byte pad0;

        //CP UVAT Flags. Used for direct primitives only?
        public bushort CPSetUVATA; //0x0870
        public buint UVATA;
        public bushort CPSetUVATB; //0x0880
        public buint UVATB;
        public bushort CPSetUVATC; //0x0890
        public buint UVATC;

        public static readonly MDL0PolygonDefs Default = new MDL0PolygonDefs()
        {
            CPSetFmtLo = 0x0850,
            CPSetFmtHi = 0x0860,
            XFCmd = 0x10,
            XFSetVtxSpecs = 0x1008,
            CPSetUVATA = 0x0870,
            CPSetUVATB = 0x0880,
            CPSetUVATC = 0x0890
        };
    }

    public struct EntrySize
    {
        public int _extraLen;
        public int _vertexLen;
        public int _normalLen;
        public int[] _colorLen;
        public int _colorEntries;
        public int _colorTotal;
        public int _uvEntries;
        public int[] _uvLen;
        public int _uvTotal;
        public int _totalLen;

        public VertexFormats _format;
        public int _stride;

        public EntrySize(CPVertexFormat flags)
        {
            _extraLen = flags.ExtraLength;
            _vertexLen = flags.VertexEntryLength;
            _normalLen = flags.NormalEntryLength;
            _colorTotal = flags.ColorEntryLength;

            _colorEntries = _colorTotal = 0;
            _colorLen = new int[2];
            for (int i = 0; i < 2; _colorTotal += _colorLen[i++])
                if ((_colorLen[i] = flags.ColorLength(i)) != 0)
                    _colorEntries++;

            _uvEntries = _uvTotal = 0;
            _uvLen = new int[8];
            for (int i = 0; i < 8; _uvTotal += _uvLen[i++])
                if ((_uvLen[i] = flags.UVLength(i)) != 0)
                    _uvEntries++;

            _totalLen = _extraLen + _vertexLen + _normalLen + _colorTotal + _uvTotal;

            _format = VertexFormats.None;
            _stride = 0;
            if (_vertexLen != 0) { _format |= VertexFormats.Position; _stride += 12; }
            if (_normalLen != 0) { _format |= VertexFormats.Normal; _stride += 12; }
            if (_colorTotal != 0) { _format |= VertexFormats.Diffuse; _stride += 4; }
            if (_uvEntries != 0) { _format |= (VertexFormats)(_uvEntries << 8); _stride += 8 * _uvEntries; }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MDL0ElementFlags
    {
        private buint _data1;
        private buint _data2;

        public bool HasVertexData { get { return (_data1 & 0x400) != 0; } }
        public int VertexEntryLength { get { return (HasVertexData) ? (((_data1 & 0x200) != 0) ? 2 : 1) : 0; } }
        public bool HasNormalData { get { return (_data1 & 0x1000) != 0; } }
        public int NormalEntryLength { get { return (HasNormalData) ? (((_data1 & 0x800) != 0) ? 2 : 1) : 0; } }

        public bool HasColorData { get { return (_data1 & 0x4000) != 0; } }
        public int ColorEntryLength { get { return (HasColorData) ? (((_data1 & 0x2000) != 0) ? 2 : 1) : 0; } }

        public bool HasColor(int index) { return (_data1 & (0x4000 << (index * 2))) != 0; }
        public int ColorLength(int index) { return HasColor(index) ? (((_data1 & (0x2000 << (index * 2))) != 0) ? 2 : 1) : 0; }
        public int ColorTotalLength { get { int len = 0; for (int i = 0; i < 2; )len += ColorLength(i++); return len; } }

        public bool HasUV(int index) { return (_data2 & (2 << (index * 2))) != 0; }
        public int UVLength(int index) { return HasUV(index) ? (((_data2 & (1 << (index * 2))) != 0) ? 2 : 1) : 0; }
        public int UVTotalLength { get { int len = 0; for (int i = 0; i < 8; )len += UVLength(i++); return len; } }

        public bool HasExtra(int index) { return (_data1 & (1 << index)) != 0; }
        public int ExtraLength { get { int len = 0; for (int i = 0; i < 8; ) if (HasExtra(i++))len++; return len; } }

        public bool HasWeights { get { return (_data1 & 0xFF) != 0; } }
        public int WeightLength { get { return ExtraLength; } }
    }

    //Part2
    //0x0850 = bytes per data

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MDL0Texture
    {
        public bint _numEntries;

        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }
        public MDL0TextureEntry* Entries { get { return (MDL0TextureEntry*)(Address + 4); } }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MDL0TextureEntry
    {
        public bint _mat; //Material offset
        public bint _ref; //Reference offset
        
        public override string ToString()
        {
            return String.Format("(Material: 0x{0:X}, MatRef: 0x{1:X})", (int)_mat, (int)_ref);
        }
    }

    //[StructLayout(LayoutKind.Sequential, Pack = 1)]
    //public unsafe struct UVPoint
    //{
    //    public bfloat U;
    //    public bfloat V;

    //    public override string ToString()
    //    {
    //        return String.Format("U:{0}, V:{1}", (float)U, (float)V);
    //    }
    //}

    //[StructLayout(LayoutKind.Sequential, Pack = 1)]
    //public unsafe struct RGBAPixel
    //{
    //    public byte R;
    //    public byte G;
    //    public byte B;
    //    public byte A;

    //    public static explicit operator RGBAPixel(uint val) { return *((RGBAPixel*)&val); }
    //    public static explicit operator uint(RGBAPixel p) { return *((uint*)&p); }

    //    public static explicit operator RGBAPixel(ARGBPixel p) { return new RGBAPixel() { R = p.R, G = p.G, B = p.G, A = p.A }; }
    //    public static explicit operator ARGBPixel(RGBAPixel p) { return new ARGBPixel() { A = p.A, R = p.R, G = p.G, B = p.G }; }

    //    public override string ToString()
    //    {
    //        return String.Format("R:{0:X}, G:{1:X}, B:{2:X}, A:{3:X}", R, G, B, A);
    //    }
    //}
}
