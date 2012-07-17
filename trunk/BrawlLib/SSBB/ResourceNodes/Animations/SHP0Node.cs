using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.SSBBTypes;
using System.ComponentModel;
using BrawlLib.Wii.Animations;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class SHP0Node : BRESEntryNode
    {
        internal BRESCommonHeader* Header { get { return (BRESCommonHeader*)WorkingUncompressed.Address; } }
        internal SHP0v3* Header3 { get { return (SHP0v3*)WorkingUncompressed.Address; } }
        internal SHP0v4* Header4 { get { return (SHP0v4*)WorkingUncompressed.Address; } }

        public override ResourceType ResourceType { get { return ResourceType.SHP0; } }

        public override int tFrameCount
        {
            get { return FrameCount; }
            set { FrameCount = value; }
        }
        [Browsable(false)]
        public override bool tLoop { get { return Loop; } set { Loop = value; } }

        int _version, _numFrames, _loop;

        public int ConversionBias = 0;
        public int startUpVersion = 0;

        [Category("Vertex Morph Data")]
        public int Version
        {
            get { return _version; }
            set
            {
                if (_version == value)
                    return;

                if (value == startUpVersion)
                    ConversionBias = 0;
                else if (startUpVersion == 3 && value == 4)
                    ConversionBias = 1;
                else if (startUpVersion == 4 && value == 3)
                    ConversionBias = -1;

                _version = value;
                SignalPropertyChange();
            }
        }
        [Category("Vertex Morph Data")]
        public int FrameCount
        {
            get { return _numFrames + (startUpVersion == 4 ? 1 : 0); }
            set
            {
                int bias = (startUpVersion == 4 ? 1 : 0);

                if ((_numFrames == value - bias) || (value - bias < (1 - bias)))
                    return;

                _numFrames = value - bias;

                foreach (SHP0EntryNode n in Children)
                    foreach (SHP0VertexSetNode s in n.Children)
                        s.SetSize(FrameCount);

                SignalPropertyChange();
            }
        }
        [Category("Vertex Morph Data")]
        public bool Loop { get { return _loop != 0; } set { _loop = value ? 1 : 0; SignalPropertyChange(); } }

        public void InsertKeyframe(int index)
        {
            FrameCount++;
            foreach (SHP0EntryNode e in Children)
                foreach (SHP0VertexSetNode c in e.Children)
                    c.Keyframes.Insert(KeyFrameMode.All, index);
        }
        public void DeleteKeyframe(int index)
        {
            foreach (SHP0EntryNode e in Children)
                foreach (SHP0VertexSetNode c in e.Children)
                    c.Keyframes.Delete(KeyFrameMode.All, index);
            FrameCount--;
        }

        //public string[] StringEntries { get { return _strings.ToArray(); } }
        internal List<string> _strings = new List<string>();

        internal override void GetStrings(StringTable table)
        {
            table.Add(Name);

            foreach (string s in _strings)
                table.Add(s);

            foreach (SHP0EntryNode n in Children)
                table.Add(n.Name);
        }

        protected override bool OnInitialize()
        {
            base.OnInitialize();

            startUpVersion = _version = Header->_version;
            _strings.Clear();

            if (_version == 4)
            {
                if ((_name == null) && (Header4->_stringOffset != 0))
                    _name = Header4->ResourceString;

                _numFrames = Header4->_numFrames;
                _loop = Header4->_loop;

                bint* stringOffset = Header4->StringEntries;
                for (int i = 0; i < Header4->_numEntries; i++)
                    _strings.Add(new String((sbyte*)stringOffset + stringOffset[i]));

                return Header4->Group->_numEntries > 0;
            }
            else
            {
                if ((_name == null) && (Header3->_stringOffset != 0))
                    _name = Header3->ResourceString;

                _numFrames = Header3->_numFrames;
                _loop = Header3->_loop;

                bint* stringOffset = Header3->StringEntries;
                for (int i = 0; i < Header3->_numEntries; i++)
                    _strings.Add(new String((sbyte*)stringOffset + stringOffset[i]));

                return Header3->Group->_numEntries > 0;
            }
        }

        protected override void OnPopulate()
        {
            ResourceGroup* group = Header4->Group;
            for (int i = 0; i < group->_numEntries; i++)
                new SHP0EntryNode().Initialize(this, new DataSource(group->First[i].DataAddress, 0));
        }

        protected internal override void OnRebuild(VoidPtr address, int length, bool force)
        {
            ResourceGroup* group;
            if (_version == 4)
            {
                SHP0v4* header = (SHP0v4*)address;
                *header = new SHP0v4(_loop, (short)(_numFrames - ConversionBias), (short)_strings.Count);
                group = header->Group;
            }
            else
            {
                SHP0v3* header = (SHP0v3*)address;
                *header = new SHP0v3(_loop, (short)(_numFrames - ConversionBias), (short)_strings.Count);
                group = header->Group;
            }

            *group = new ResourceGroup(Children.Count);

            VoidPtr entryAddress = group->EndAddress;
            VoidPtr dataAddress = entryAddress;

            foreach (SHP0EntryNode n in Children)
                dataAddress += n._entryLen;

            ResourceEntry* rEntry = group->First;
            foreach (SHP0EntryNode n in Children)
            {
                (rEntry++)->_dataOffset = (int)entryAddress - (int)group;

                n._dataAddr = dataAddress;
                n.Rebuild(entryAddress, n._entryLen, true);
                entryAddress += n._entryLen;
                dataAddress += n._dataLen;
            }

            ((SHP0v3*)address)->_stringListOffset = (int)dataAddress - (int)address;
        }

        protected override int OnCalculateSize(bool force)
        {
            _strings.Clear();
            int size = (Version == 4 ? SHP0v4.Size : SHP0v3.Size) + 0x18 + Children.Count * 0x10;
            foreach (SHP0EntryNode entry in Children)
            {
                _strings.Add(entry.Name);
                foreach (SHP0VertexSetNode n in entry.Children)
                    _strings.Add(n.Name);

                size += entry.CalculateSize(true);
            }
            size += _strings.Count * 4;
            return size;
        }

        protected internal override void PostProcess(VoidPtr bresAddress, VoidPtr dataAddress, int dataLength, StringTable stringTable)
        {
            base.PostProcess(bresAddress, dataAddress, dataLength, stringTable);

            SHP0v3* header = (SHP0v3*)dataAddress;

            header->ResourceStringAddress = stringTable[Name] + 4;

            bint* stringPtr = header->StringEntries;
            for (int i = 0; i < header->_numEntries; i++)
                stringPtr[i] = ((int)stringTable[_strings[i]] + 4) - (int)stringPtr;

            ResourceGroup* group = header->Group;
            group->_first = new ResourceEntry(0xFFFF, 0, 0, 0, 0);

            ResourceEntry* rEntry = group->First;

            int index = 1;
            foreach (SHP0EntryNode n in Children)
            {
                dataAddress = (VoidPtr)group + (rEntry++)->_dataOffset;
                ResourceEntry.Build(group, index++, dataAddress, (BRESString*)stringTable[n.Name]);
                n.PostProcess(dataAddress, stringTable);
            }
        }

        internal static ResourceNode TryParse(DataSource source) { return ((BRESCommonHeader*)source.Address)->_tag == SHP0v3.Tag ? new SHP0Node() : null; }

        public unsafe SHP0EntryNode FindOrCreateEntry(string name)
        {
            foreach (SHP0EntryNode t in Children)
                if (t.Name == name)
                    return t;

            SHP0EntryNode entry = new SHP0EntryNode();
            entry.Name = name;
            entry._flags = 3;
            AddChild(entry);
            SHP0VertexSetNode morph = new SHP0VertexSetNode(FindName("NewMorphTarget"));
            morph.isFixed = true;
            entry.AddChild(morph);
            return entry;
        }
    }

    public unsafe class SHP0EntryNode : ResourceNode
    {
        internal SHP0Entry* Header { get { return (SHP0Entry*)WorkingUncompressed.Address; } }
        public override ResourceType ResourceType { get { return ResourceType.SHP0Entry; } }

        List<short> _indices;
        public int _flags, indexCount, nameIndex, fixedFlags;
        
        [Category("Vertex Morph Entry")]
        public int Unknown { get { return _flags; } set { _flags = value; SignalPropertyChange(); } }
        
        protected override bool OnInitialize()
        {
            if ((_name == null) && (Header->_stringOffset != 0))
                _name = Header->ResourceString;

            _indices = new List<short>();
            for (int i = 0; i < Header->_numIndices; i++)
                _indices.Add((short)(Header->Indicies[i]));

            _flags = Header->_flags;
            indexCount = Header->_numIndices;
            nameIndex = Header->_nameIndex;
            fixedFlags = Header->_fixedFlags;

            return Header->_flags > 0;
        }

        protected override void OnPopulate()
        {
            for (int i = 0; i < indexCount; i++)
                if ((fixedFlags >> i & 1) == 0)
                    new SHP0VertexSetNode(_indices[i] < ((SHP0Node)Parent)._strings.Count ? ((SHP0Node)Parent)._strings[_indices[i]] : "Unknown").Initialize(this, new DataSource(Header->GetEntry(i), 0x14 + Header->_numIndices * 6));
                else
                {
                    SHP0VertexSetNode n = new SHP0VertexSetNode(_indices[i] < ((SHP0Node)Parent)._strings.Count ? ((SHP0Node)Parent)._strings[_indices[i]] : "Unknown") { isFixed = true };
                    n.Keyframes[KeyFrameMode.ScaleX, 0] = ((bfloat*)Header->EntryOffset)[i];
                    n._numFrames = ((SHP0Node)Parent).FrameCount;
                    //n.Parent = this;
                    AddChild(n, false);
                }
            }

        public VoidPtr _dataAddr;
        public int _dataLen, _entryLen;

        protected internal virtual void PostProcess(VoidPtr dataAddress, StringTable stringTable)
        {
            SHP0Entry* header = (SHP0Entry*)dataAddress;
            header->ResourceStringAddress = stringTable[Name] + 4;
        }

        protected override int OnCalculateSize(bool force)
        {
            _entryLen = (0x14 + Children.Count * 6).Align(4);
            _dataLen = 0;

            foreach (SHP0VertexSetNode p in Children)
                _dataLen += p.CalculateSize(true);

            return _entryLen + _dataLen;
        }

        protected internal override void OnRebuild(VoidPtr address, int length, bool force)
        {
            SHP0Entry* header = (SHP0Entry*)address;
            VoidPtr addr = _dataAddr;
            header->_numIndices = (short)Children.Count;
            header->_nameIndex = (short)((SHP0Node)Parent)._strings.IndexOf(Name);
            header->_flags = _flags;
            header->_indiciesOffset = 0x14 + Children.Count * 4;
            uint fixedflags = 0;
            foreach (SHP0VertexSetNode p in Children)
            {
                p._dataAddr = addr;
                header->Indicies[p.Index] = (short)((SHP0Node)Parent)._strings.IndexOf(p.Name);
                if (p.isFixed)
                {
                    ((bfloat*)header->EntryOffset)[p.Index] = p.Keyframes.GetKeyframe(KeyFrameMode.ScaleX, 0)._value;
                    fixedflags = (uint)(fixedflags & ((uint)0xFFFFFFFF - (uint)(1 << p.Index))) | (uint)(1 << p.Index);
                }
                else
                {
                    header->EntryOffset[p.Index] = (int)(p._dataAddr - &header->EntryOffset[p.Index]);
                    p.Rebuild(p._dataAddr, p._calcSize, true);
                }
                addr += p._dataLen;
            }
            header->_fixedFlags = (int)fixedflags;
        }

        internal unsafe SHP0VertexSetNode FindOrCreateEntry(string name)
        {
            foreach (SHP0VertexSetNode t in Children)
                if (t.Name == name)
                    return t;

            SHP0VertexSetNode entry = new SHP0VertexSetNode(name) { _numFrames = ((SHP0Node)Parent).FrameCount };
            AddChild(entry);
            return entry;
        }

        public void CreateEntry()
        {
            SHP0VertexSetNode morph = new SHP0VertexSetNode(FindName("NewMorphTarget"));
            morph.isFixed = true;
            AddChild(morph);
        }
    }

    public unsafe class SHP0VertexSetNode : ResourceNode
    {
        internal SHP0KeyframeEntries* Header { get { return (SHP0KeyframeEntries*)WorkingUncompressed.Address; } }
        public int _dataLen;
        public VoidPtr _dataAddr;
        internal void SetSize(int count)
        {
            if (_keyframes != null)
                Keyframes.FrameLimit = count;

            _numFrames = count;
            SignalPropertyChange();
        }

        internal int _numFrames;
        [Browsable(false)]
        public int FrameCount { get { return _numFrames; } }

        internal KeyframeCollection _keyframes;
        [Browsable(false)]
        public KeyframeCollection Keyframes
        {
            get
            {
                if (_keyframes == null)
                {
                    _keyframes = new KeyframeCollection(_numFrames);
                    if (Header != null)
                    {
                        int fCount = Header->_numEntries;
                        BVec3* entry = Header->Entries;
                        for (int i = 0; i < fCount; i++, entry++)
                            Keyframes.SetFrameValue(KeyFrameMode.ScaleX, (int)entry->_y, entry->_z)._tangent = entry->_x;
                    }
                }
                return _keyframes;
            }
        }

        public bool isFixed = false;
        public float fixedValue;

        public SHP0VertexSetNode(string name) { _name = name; }

        protected override bool OnInitialize()
        {
            if (_parent is SHP0EntryNode && _parent._parent is SHP0Node)
                _numFrames = ((SHP0Node)_parent._parent).FrameCount;

            return false;
        }

        protected override int OnCalculateSize(bool force)
        {
            return _dataLen = ((isFixed = Keyframes._keyCounts[0] <= 1) ? 0 : Keyframes._keyCounts[0] * 12 + 8);
        }

        protected internal override void OnRebuild(VoidPtr address, int length, bool force)
        {
            SHP0KeyframeEntries* header = (SHP0KeyframeEntries*)address;
            header->_numEntries = (short)Keyframes._keyCounts[0];
            header->_unk1 = 0;

            BVec3* entry = header->Entries;
            KeyframeEntry frame, root = Keyframes._keyRoots[0];
            for (frame = root._next; frame._index != -1; frame = frame._next)
                *entry++ = new Vector3(frame._tangent, frame._index, frame._value);
        }

        #region Keyframe Management

        public KeyframeEntry GetKeyframe(int index) { return Keyframes.GetKeyframe(KeyFrameMode.ScaleX, index); }
        public void SetKeyframe(int index, float value)
        {
            KeyframeEntry k = Keyframes.SetFrameValue(KeyFrameMode.ScaleX, index, value);
            k.GenerateTangent();
            k._prev.GenerateTangent();
            k._next.GenerateTangent();

            SignalPropertyChange();
        }
        public void RemoveKeyframe(int index)
        {
            KeyframeEntry k = Keyframes.Remove(KeyFrameMode.ScaleX, index);
            if (k != null)
            {
                k._prev.GenerateTangent();
                k._next.GenerateTangent();
                SignalPropertyChange();
            }
        }
        public AnimationFrame GetAnimFrame(int index)
        {
            return Keyframes.GetFullFrame(index);
        }

        #endregion
    }

    public class SHP0Keyframe
    {
        public float _index, _percentage, _tangent;

        public static implicit operator SHP0Keyframe(Vector3 v) { return new SHP0Keyframe(v._x, v._y, v._z); }
        public static implicit operator Vector3(SHP0Keyframe v) { return new Vector3(v._tangent, v._index, v._percentage); }
        
        public SHP0Keyframe(float tan, float index, float percent) { _index = index; _percentage = percent; _tangent = tan; }
        public SHP0Keyframe() { }
        
        public float Index { get { return _index; } set { _index = value; } }
        public float Percentage { get { return _percentage; } set { _percentage = value; } }
        public float Tangent { get { return _tangent; } set { _tangent = value; } }

        public override string ToString()
        {
            return String.Format("Index={0}, Percentage={1}, Tangent={2}", _index, String.Format("{0}%", _percentage * 100.0f), _tangent);
        }
    }
}
