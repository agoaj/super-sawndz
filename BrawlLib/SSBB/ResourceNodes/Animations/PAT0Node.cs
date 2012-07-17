using System;
using BrawlLib.SSBBTypes;
using System.ComponentModel;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class PAT0Node : BRESEntryNode
    {
        internal BRESCommonHeader* Header { get { return (BRESCommonHeader*)WorkingUncompressed.Address; } }
        internal PAT0v3* Header3 { get { return (PAT0v3*)WorkingUncompressed.Address; } }
        internal PAT0v4* Header4 { get { return (PAT0v4*)WorkingUncompressed.Address; } }

        internal List<string> _textureFiles = new List<string>();
        internal List<string> _paletteFiles = new List<string>();

        internal int _loop, _version;
        internal ushort _frameCount;

        public bool texChanged = false, pltChanged = false;

        public override ResourceType ResourceType { get { return ResourceType.PAT0; } }

        [Category("Texture Pattern")]
        public int Version { get { return _version; } set { _version = value; SignalPropertyChange(); } }
        [Category("Texture Pattern"), Browsable(true)]
        public string[] Textures
        {
            get
            {
                if (texChanged)
                {
                    _textureFiles.Clear();
                    foreach (PAT0EntryNode n in Children)
                        foreach (PAT0TextureNode t in n.Children)
                            foreach (PAT0TextureEntryNode e in t.Children)
                                if (t.hasTex && !String.IsNullOrEmpty(e.tex) && !_textureFiles.Contains(e.tex))
                                    _textureFiles.Add(e.tex);
                    texChanged = false;
                }
                return _textureFiles.ToArray();
            }
        }
        [Category("Texture Pattern"), Browsable(true)]
        public string[] Palettes
        {
            get
            {
                if (pltChanged)
                {
                    _paletteFiles.Clear();
                    foreach (PAT0EntryNode n in Children)
                        foreach (PAT0TextureNode t in n.Children)
                            foreach (PAT0TextureEntryNode e in t.Children)
                                if (t.hasPlt && !String.IsNullOrEmpty(e.plt) && !_paletteFiles.Contains(e.plt))
                                    _paletteFiles.Add(e.plt);
                    pltChanged = false;
                }
                return _paletteFiles.ToArray();
            }
        }
        [Category("Texture Pattern")]
        public ushort FrameCount
        {
            get { return _frameCount; }
            set
            {
                if ((_frameCount == value) || (value < 1))
                    return; 

                _frameCount = value;
                SignalPropertyChange();
            }
        }
        [Category("Texture Pattern")]
        public bool Loop { get { return _loop != 0; } set { _loop = value ? 1 : 0; SignalPropertyChange(); } }
        
        public override int tFrameCount
        {
            get { return FrameCount; }
            set { FrameCount = (ushort)value; }
        }

        [Browsable(false)]
        public override bool tLoop { get { return Loop; } set { Loop = value; } }

        protected override bool OnInitialize()
        {
            base.OnInitialize();

            _textureFiles.Clear();
            _paletteFiles.Clear();

            _version = Header->_version;

            int texPtr, pltPtr;
            if (_version == 4)
            {
                PAT0v4* header = Header4;
                _frameCount = header->_numFrames;
                _loop = header->_loop;
                texPtr = header->_numTexPtr;
                pltPtr = header->_numPltPtr;
                if ((_name == null) && (header->_stringOffset != 0))
                    _name = header->ResourceString;
            }
            else
            {
                PAT0v3* header = Header3;
                _frameCount = header->_numFrames;
                _loop = header->_loop;
                texPtr = header->_numTexPtr;
                pltPtr = header->_numPltPtr;
                if ((_name == null) && (header->_stringOffset != 0))
                    _name = header->ResourceString;
            }

            //Get texture strings
            for (int i = 0; i < texPtr; i++)
                _textureFiles.Add(Header3->GetTexStringEntry(i));

            //Get palette strings
            for (int i = 0; i < pltPtr; i++)
                _paletteFiles.Add(Header3->GetPltStringEntry(i));

            return Header3->Group->_numEntries > 0;
        }

        protected override void OnPopulate()
        {
            ResourceGroup* group = Header3->Group;
            for (int i = 0; i < group->_numEntries; i++)
                new PAT0EntryNode().Initialize(this, new DataSource(group->First[i].DataAddress, PAT0Pattern.Size));
        }

        internal override void GetStrings(StringTable table)
        {
            table.Add(Name);

            foreach (PAT0EntryNode n in Children)
            {
                table.Add(n.Name);
                foreach (PAT0TextureNode t in n.Children)
                {
                    foreach (PAT0TextureEntryNode e in t.Children)
                        table.Add(e.Name);
                }
            }
        }

        protected override int OnCalculateSize(bool force)
        {
            _textureFiles.Clear();
            _paletteFiles.Clear();
            foreach (PAT0EntryNode n in Children)
                foreach (PAT0TextureNode t in n.Children)
                    foreach (PAT0TextureEntryNode e in t.Children)
                    {
                        if (t.hasTex && !String.IsNullOrEmpty(e.tex) && !_textureFiles.Contains(e.tex))
                            _textureFiles.Add(e.tex);
                        if (t.hasPlt && !String.IsNullOrEmpty(e.plt) && !_paletteFiles.Contains(e.plt))
                            _paletteFiles.Add(e.plt);
                    }

            int size = PAT0v3.Size + 0x18 + Children.Count * 0x10;
            size += (_textureFiles.Count + _paletteFiles.Count) * 8;
            foreach (PAT0EntryNode n in Children)
                size += n.CalculateSize(true);

            return size;
        }

        protected internal override void OnRebuild(VoidPtr address, int length, bool force)
        {
            //Set header values
            if (_version == 4)
            {
                PAT0v4* header = (PAT0v4*)address;
                header->_header._tag = PAT0v4.Tag;
                header->_header._version = 4;
                header->_dataOffset = PAT0v4.Size;
                header->_pad1 = header->_pad2 = 0;
                header->_numFrames = _frameCount;
                header->_numEntries = (ushort)Children.Count;
                header->_numTexPtr = (ushort)_textureFiles.Count;
                header->_numPltPtr = (ushort)_paletteFiles.Count;
                header->_loop = _loop;
            }
            else
            {
                PAT0v3* header = (PAT0v3*)address;
                header->_header._tag = PAT0v3.Tag;
                header->_header._version = 3;
                header->_dataOffset = PAT0v3.Size;
                header->_pad = 0;
                header->_numFrames = _frameCount;
                header->_numEntries = (ushort)Children.Count;
                header->_numTexPtr = (ushort)_textureFiles.Count;
                header->_numPltPtr = (ushort)_paletteFiles.Count;
                header->_loop = _loop;
            }

            PAT0v3* commonHeader = (PAT0v3*)address;

            //Now set header values that are in the same spot between versions

            //Set offsets
            commonHeader->_texTableOffset = length - (_textureFiles.Count + _paletteFiles.Count) * 8;
            commonHeader->_pltTableOffset = commonHeader->_texTableOffset + _textureFiles.Count * 4;

            //Set pointer offsets
            int offset = length - _textureFiles.Count * 4 - _paletteFiles.Count * 4;
            commonHeader->_texPtrTableOffset = offset;
            commonHeader->_pltPtrTableOffset = offset + _textureFiles.Count * 4;

            //Set pointers
            bint* ptr = (bint*)(commonHeader->Address + commonHeader->_texPtrTableOffset);
            for (int i = 0; i < _textureFiles.Count; i++)
                *ptr++ = 0;
            ptr = (bint*)(commonHeader->Address + commonHeader->_pltPtrTableOffset);
            for (int i = 0; i < _paletteFiles.Count; i++)
                *ptr++ = 0;

            ResourceGroup* group = commonHeader->Group;
            *group = new ResourceGroup(Children.Count);

            VoidPtr entryAddress = group->EndAddress;
            VoidPtr dataAddress = entryAddress;
            ResourceEntry* rEntry = group->First;

            foreach (PAT0EntryNode n in Children)
                dataAddress += n._entryLen;
            foreach (PAT0EntryNode n in Children)
            foreach (PAT0TextureNode t in n.Children)
            {
                n._dataAddrs[t.Index] = dataAddress;
                if (n._dataLens[t.Index] != -1)
                    dataAddress += n._dataLens[t.Index];
            }

            foreach (PAT0EntryNode n in Children)
            {
                (rEntry++)->_dataOffset = (int)entryAddress - (int)group;

                n.Rebuild(entryAddress, n._entryLen, true);
                entryAddress += n._entryLen;
            }
        }

        protected internal override void PostProcess(VoidPtr bresAddress, VoidPtr dataAddress, int dataLength, StringTable stringTable)
        {
            base.PostProcess(bresAddress, dataAddress, dataLength, stringTable);

            PAT0v3* header = (PAT0v3*)dataAddress;
            if (_version == 4)
                ((PAT0v4*)dataAddress)->ResourceStringAddress = stringTable[Name] + 4;
            else
                header->ResourceStringAddress = stringTable[Name] + 4;

            ResourceGroup* group = header->Group;
            group->_first = new ResourceEntry(0xFFFF, 0, 0, 0, 0);
            ResourceEntry* rEntry = group->First;

            int index = 1;
            foreach (PAT0EntryNode n in Children)
            {
                dataAddress = (VoidPtr)group + (rEntry++)->_dataOffset;
                ResourceEntry.Build(group, index++, dataAddress, (BRESString*)stringTable[n.Name]);
                n.PostProcess(dataAddress, stringTable);
            }

            int i = 0;
            bint* strings = header->TexFile;

            for (i = 0; i < _textureFiles.Count; i++)
                if (!String.IsNullOrEmpty(_textureFiles[i]))
                    strings[i] = (int)stringTable[_textureFiles[i]] + 4 - (int)strings;

            strings = header->PltFile;

            for (i = 0; i < _paletteFiles.Count; i++)
                if (!String.IsNullOrEmpty(_paletteFiles[i]))
                    strings[i] = (int)stringTable[_paletteFiles[i]] + 4 - (int)strings;
        }

        internal static ResourceNode TryParse(DataSource source) { return ((PAT0v3*)source.Address)->_header._tag == PAT0v3.Tag ? new PAT0Node() : null; }

        public void CreateEntry()
        {
            PAT0EntryNode n = new PAT0EntryNode();
            n.Name = FindName(null);
            AddChild(n);
            n.CreateEntry();
        }
    }

    public unsafe class PAT0EntryNode : ResourceNode
    {
        internal PAT0Pattern* Header { get { return (PAT0Pattern*)WorkingUncompressed.Address; } }
        public override ResourceType ResourceType { get { return ResourceType.PAT0Entry; } }

        internal PAT0Flags[] texFlags = new PAT0Flags[8];

        protected override bool OnInitialize()
        {
            if ((_name == null) && (Header->_stringOffset != 0))
                _name = Header->ResourceString;

            uint flags = Header->_flags;
            for (int i = 0; i < 8; i++)
                texFlags[i] = (PAT0Flags)((flags >> (i * 4)) & 0xF);

            return true;
        }

        protected override void OnPopulate()
        {
            int count = 0, index = 0;
            foreach (PAT0Flags p in texFlags)
            {
                if (p.HasFlag(PAT0Flags.Enabled))
                {
                    if (!p.HasFlag(PAT0Flags.FixedTexture))
                        new PAT0TextureNode(p, index).Initialize(this, new DataSource(Header->GetTexTable(count), PAT0Texture.Size));
                    else
                    {
                        PAT0TextureNode t = new PAT0TextureNode(p, index) { textureCount = 1 };
                        t.Parent = this;
                        PAT0TextureEntryNode entry = new PAT0TextureEntryNode();
                        entry.key = 0;
                        entry.texFileIndex = Header->GetIndex(count, false);
                        entry.pltFileIndex = Header->GetIndex(count, true);
                        entry.Parent = t;
                        entry.GetStrings();
                    }
                    count++;
                }
                index++;
            }
        }

        protected override int OnCalculateSize(bool force)
        {
            _dataLens = new int[Children.Count];
            _dataAddrs = new VoidPtr[Children.Count];

            _entryLen = PAT0Pattern.Size + Children.Count * 4;

            foreach (PAT0TextureNode table in Children)
                _dataLens[table.Index] = table.CalculateSize(true);

            //Check to see if any children can be remapped.
            foreach (PAT0TextureNode table in Children)
                table.CompareToAll();

            int size = 0;
            foreach (int i in _dataLens)
                if (i != -1)
                    size += i;

            return size + _entryLen; 
        }

        public VoidPtr[] _dataAddrs;
        public int _entryLen;
        public int[] _dataLens;
        protected internal override void OnRebuild(VoidPtr address, int length, bool force)
        {
            PAT0Pattern* header = (PAT0Pattern*)address;

            int x = 0;
            foreach (int i in _dataLens)
            {
                if (i == -1)
                    _dataAddrs[x] = ((PAT0EntryNode)Parent.Children[((PAT0TextureNode)Children[x]).matIndex])._dataAddrs[((PAT0TextureNode)Children[x]).texIndex];
                x++;
            }
            uint flags = 0;
            foreach (PAT0TextureNode table in Children)
            {
                table.texFlags |= PAT0Flags.Enabled;
                if (table.Children.Count > 1)
                    table.texFlags &= 0xF - PAT0Flags.FixedTexture;
                else
                    table.texFlags |= PAT0Flags.FixedTexture;

                bool hasTex = false, hasPlt = false;

                //foreach (PAT0TextureEntryNode e in table.Children)
                //{
                //    if (e.Texture != null)
                //        hasTex = true;
                //    if (e.Palette != null)
                //        hasPlt = true;
                //}

                hasTex = table.hasTex;
                hasPlt = table.hasPlt;

                if (!hasTex)
                    table.texFlags &= 0xF - PAT0Flags.HasTexture;
                else
                    table.texFlags |= PAT0Flags.HasTexture;
                if (!hasPlt)
                    table.texFlags &= 0xF - PAT0Flags.HasPalette;
                else
                    table.texFlags |= PAT0Flags.HasPalette;

                if (table.Children.Count > 1)
                {
                    header->SetTexTableOffset(table.Index, _dataAddrs[table.Index]);
                    if (table._rebuild)
                        table.Rebuild(_dataAddrs[table.Index], PAT0TextureTable.Size + PAT0Texture.Size * table.Children.Count, true);
                }
                else
                {
                    PAT0TextureEntryNode entry = (PAT0TextureEntryNode)table.Children[0];
                    PAT0Node node = (PAT0Node)Parent;

                    short i = 0;
                    if (table.hasTex && !String.IsNullOrEmpty(entry.Texture))
                        i = (short)node._textureFiles.IndexOf(entry.Texture);

                    if (i < 0)
                        entry.texFileIndex = 0;
                    else
                        entry.texFileIndex = (ushort)i;

                    i = 0;
                    if (table.hasPlt && !String.IsNullOrEmpty(entry.Palette))
                        i = (short)node._paletteFiles.IndexOf(entry.Palette);

                    if (i < 0)
                        entry.pltFileIndex = 0;
                    else
                        entry.pltFileIndex = (ushort)i;

                    header->SetIndex(table.Index, entry.texFileIndex, false);
                    header->SetIndex(table.Index, entry.pltFileIndex, true);
                }

                flags = flags & ~((uint)0xF << (table._textureIndex * 4)) | ((uint)table.texFlags << (table._textureIndex * 4)); 
            }

            header->_flags = flags;
        }

        protected internal virtual void PostProcess(VoidPtr dataAddress, StringTable stringTable)
        {
            PAT0Pattern* header = (PAT0Pattern*)dataAddress;
            header->ResourceStringAddress = stringTable[Name] + 4;
        }

        public void CreateEntry()
        {
            int value = 0;
            foreach (PAT0TextureNode t in Children)
                if (t._textureIndex == value)
                    value++;

            if (value == 8)
                return;

            PAT0TextureNode node = new PAT0TextureNode((PAT0Flags)7, value);
            AddChild(node);
            node.CreateEntry();
        }
    }

    public unsafe class PAT0TextureNode : ResourceNode
    {
        internal PAT0TextureTable* Header { get { return (PAT0TextureTable*)WorkingUncompressed.Address; } }

        public override ResourceType ResourceType { get { return ResourceType.PAT0Texture; } }

        public PAT0Flags texFlags;
        public int _textureIndex, textureCount;
        public ushort _texNameIndex, _pltNameIndex;
        public bool hasPlt, hasTex;
        public float frameScale;

        public bool _rebuild = true;

        //[Category("PAT0 Texture")]
        //public PAT0Flags TextureFlags { get { return texFlags; } }//set { texFlags = value; SignalPropertyChange(); } }
        //[Category("PAT0 Texture")]
        //public float FrameScale { get { return frameScale; } }
        //[Category("PAT0 Texture")]
        //public int TextureCount { get { return textureCount; } }
        [Category("PAT0 Texture")]
        public bool HasTexture { get { return hasTex; } set { hasTex = value; SignalPropertyChange(); } }
        [Category("PAT0 Texture")]
        public bool HasPalette { get { return hasPlt; } set { hasPlt = value; SignalPropertyChange(); } }
        [Category("PAT0 Texture")]
        public int TextureIndex 
        { 
            get { return _textureIndex; } 
            set 
            {
                foreach (PAT0TextureNode t in Parent.Children)
                    if (t.Index != Index && t._textureIndex == (value > 7 ? 7 : value < 0 ? 0 : value))
                        return;

                _textureIndex = value > 7 ? 7 : value < 0 ? 0 : value;

                Name = "Texture" + _textureIndex;

                CheckNext();
                CheckPrev();
            } 
        }

        public void CheckNext()
        {
            if (Index == Parent.Children.Count - 1)
                return;

            int index = Index;
            if (_textureIndex > ((PAT0TextureNode)Parent.Children[Index + 1])._textureIndex)
            {
                doMoveDown();
                if (index != Index)
                    CheckNext();
            }
        }

        public void CheckPrev()
        {
            if (Index == 0)
                return;

            int index = Index;
            if (_textureIndex < ((PAT0TextureNode)Parent.Children[Index - 1])._textureIndex)
            {
                doMoveUp();
                if (index != Index)
                    CheckPrev();
            }
        }

        public PAT0TextureEntryNode GetTextureEntry(int index)
        {
            PAT0TextureEntryNode prev = null;
            foreach (PAT0TextureEntryNode next in Children)
            {
                if (next.Index == 0)
                {
                    prev = next;
                    continue;
                }
                if (prev.key <= index && next.key > index)
                    break;
                prev = next;
            }
            return prev;
        }
        public PAT0TextureEntryNode GetTextureEntryExplicit(int index)
        {
            PAT0TextureEntryNode prev = null;
            if (Children.Count == 0)
                return null;
            foreach (PAT0TextureEntryNode next in Children)
            {
                if (next.Index == 0)
                {
                    prev = next;
                    continue;
                }
                if (prev.key <= index && next.key > index)
                    break;
                prev = next;
            }
            if ((int)prev.key == index)
                return prev;
            else
                return null;
        }
        public string GetTexture(int index, out bool kf)
        {
            PAT0TextureEntryNode prev = null;
            if (Children.Count == 0)
            {
                kf = false;
                return null;
            }
            foreach (PAT0TextureEntryNode next in Children)
            {
                if (next.Index == 0)
                {
                    prev = next;
                    continue;
                }
                if (prev.key <= index && next.key > index)
                    break;
                prev = next;
            }
            if ((int)prev.key == index)
                kf = true;
            else
                kf = false;
            return prev.Texture;
        }
        public string GetPalette(int index, out bool kf)
        {
            PAT0TextureEntryNode prev = null;
            if (Children.Count == 0)
            {
                kf = false;
                return null;
            }
            foreach (PAT0TextureEntryNode next in Children)
            {
                if (next.Index == 0)
                {
                    prev = next;
                    continue;
                }
                if (prev.key <= index && next.key > index)
                    break;
                prev = next;
            }
            if ((int)prev.key == index)
                kf = true;
            else
                kf = false;
            return prev.Palette;
        }

        public PAT0TextureNode(PAT0Flags flags, int index)
        {
            texFlags = flags;
            hasTex = texFlags.HasFlag(PAT0Flags.HasTexture);
            hasPlt = texFlags.HasFlag(PAT0Flags.HasPalette);
            _textureIndex = index;
            _name = "Texture" + _textureIndex;
        }

        protected override bool OnInitialize()
        {
            frameScale = Header->_frameScale;
            textureCount = Header->_textureCount;

            return textureCount > 0;
        }

        protected override void OnPopulate()
        {
            if (!texFlags.HasFlag(PAT0Flags.FixedTexture))
            {
                PAT0Texture* current = Header->Textures;
                for (int i = 0; i < textureCount; i++, current++)
                    new PAT0TextureEntryNode().Initialize(this, new DataSource(current, PAT0Texture.Size));
            }
        }

        protected override int OnCalculateSize(bool force)
        {
            return Children.Count > 1 ? PAT0TextureTable.Size + PAT0Texture.Size * Children.Count : 0;
        }

        protected internal override void OnRebuild(VoidPtr address, int length, bool force)
        {
            if (Children.Count > 1)
            {
                PAT0TextureTable* table = (PAT0TextureTable*)address;
                table->_textureCount = (short)Children.Count;
                table->_frameScale = 1.0f / (Children[Children.Count - 1] as PAT0TextureEntryNode).key;
                table->_pad = 0;

                PAT0Texture* entry = table->Textures;
                foreach (PAT0TextureEntryNode n in Children)
                    n.Rebuild(entry++, PAT0Texture.Size, true);
            }
        }

        public int matIndex, texIndex;
        internal void CompareToAll()
        {
            _rebuild = true;
            foreach (PAT0EntryNode e in Parent.Parent.Children)
            foreach (PAT0TextureNode table in e.Children)
            {
                if (table == this)
                    return;

                if (table != this && table.Children.Count == Children.Count)
                {
                    bool same = true;
                    for (int l = 0; l < Children.Count; l++)
                    {
                        PAT0TextureEntryNode exte = (PAT0TextureEntryNode)table.Children[l];
                        PAT0TextureEntryNode inte = (PAT0TextureEntryNode)Children[l];

                        if (exte.key != inte.key || exte.texFileIndex != inte.texFileIndex || exte.pltFileIndex != inte.pltFileIndex)
                        {
                            same = false;
                            break;
                        }
                    }
                    if (same)
                    {
                        _rebuild = false;
                        matIndex = e.Index;
                        texIndex = table.Index;
                        ((PAT0EntryNode)Parent)._dataLens[Index] = -1;
                        return;
                    }
                }
            }
        }

        public void CreateEntry()
        {
            float value = Children.Count > 0 ? ((PAT0TextureEntryNode)Children[Children.Count - 1]).key + 1 : 0;
            PAT0TextureEntryNode node = new PAT0TextureEntryNode() { key = value };
            AddChild(node);
            node.Texture = "NewTexture";
        }
    }
    
    public unsafe class PAT0TextureEntryNode : ResourceNode
    {
        internal PAT0Texture* Header { get { return (PAT0Texture*)WorkingUncompressed.Address; } }
        internal float key;
        internal ushort texFileIndex, pltFileIndex;

        public string tex = null, plt = null;

        public override ResourceType ResourceType { get { return ResourceType.PAT0TextureEntry; } }
        public override bool AllowDuplicateNames { get { return true; } }

        [Category("PAT0 Texture Entry")]
        public float Key 
        {
            get { return key; }
            set
            {
                if (Index == 0)
                {
                    if (Index == Children.Count - 1)
                        key = 0;
                    else if (value >= ((PAT0TextureEntryNode)Parent.Children[Index + 1]).key)
                    {
                        ((PAT0TextureEntryNode)Parent.Children[Index + 1]).key = 0;
                        Parent.Children[Index + 1].SignalPropertyChange();
                        CheckNext(value);
                        key = value;
                    }
                    SignalPropertyChange();
                    return;
                }

                CheckPrev(value);
                CheckNext(value);

                key = value;
                SignalPropertyChange();
            }
        }

        public void CheckNext(float value)
        {
            if (Index == Parent.Children.Count - 1)
                return;

            int index = Index;
            if (value > ((PAT0TextureEntryNode)Parent.Children[Index + 1]).key)
            {
                doMoveDown();
                if (index != Index)
                    CheckNext(value);
            }
        }

        public void CheckPrev(float value)
        {
            if (Index == 0)
                return;

            int index = Index;
            if (value < ((PAT0TextureEntryNode)Parent.Children[Index - 1]).key)
            {
                doMoveUp();
                if (index != Index)
                    CheckPrev(value);
            }
        }

        [Category("PAT0 Texture Entry"), Browsable(true), TypeConverter(typeof(DropDownListPAT0Textures))]
        public string Texture
        {
            get { return tex; }
            set 
            {
                if (((PAT0TextureNode)Parent).hasTex)
                {
                    if (!String.IsNullOrEmpty(value))
                        Name = value;
                }
                else
                {
                    tex = null;
                    MessageBox.Show("You must enable the use of textures on the texture node.");
                }
                ((PAT0Node)Parent.Parent.Parent).texChanged = true;
            }
        }
        [Category("PAT0 Texture Entry"), Browsable(true), TypeConverter(typeof(DropDownListPAT0Palettes))]
        public string Palette
        {
            get { return plt; }
            set 
            {
                if (((PAT0TextureNode)Parent).hasPlt)
                {
                    if (!String.IsNullOrEmpty(value))
                    {
                        plt = value;
                        SignalPropertyChange();
                    }
                }
                else
                {
                    plt = null;
                    MessageBox.Show("You must enable the use of palettes on the texture node.");
                }
                ((PAT0Node)Parent.Parent.Parent).pltChanged = true;
            }
        }

        //[Category("PAT0 Texture")]
        //public ushort TextureFileIndex { get { return texFileIndex; } set { texFileIndex = value; GetName(); SignalPropertyChange(); } }
        //[Category("PAT0 Texture")]
        //public ushort PaletteFileIndex { get { return pltFileIndex; } set { pltFileIndex = value; SignalPropertyChange(); } }

        [Browsable(false)]
        public override string Name
        {
            get { return base.Name; }
            set { base.Name = value; tex = value;/*checkTexture(value, true);*/ }
        }

        protected override bool OnInitialize()
        {
            key = Header->_key;
            texFileIndex = Header->_texFileIndex;
            pltFileIndex = Header->_pltFileIndex;

            GetStrings();

            return false;
        }

        public void GetStrings()
        {
            if (Parent == null)
            {
                _name = "<null>";
                return;
            }

            PAT0Node node = (PAT0Node)Parent.Parent.Parent;

            if (((PAT0TextureNode)Parent).hasPlt && pltFileIndex < node._paletteFiles.Count)
                plt = node._paletteFiles[pltFileIndex];

            if (((PAT0TextureNode)Parent).hasTex && texFileIndex < node._textureFiles.Count)
                _name = tex = node._textureFiles[texFileIndex];

            if (_name == null && plt != null)
                _name = plt;

            if (_name == null)
                _name = "<null>";
        }

        protected internal override void OnRebuild(VoidPtr address, int length, bool force)
        {
            PAT0Node node = (PAT0Node)Parent.Parent.Parent;

            PAT0Texture* header = (PAT0Texture*)address;

            header->_key = key;

            short i = 0;
            if (((PAT0TextureNode)Parent).hasTex && !String.IsNullOrEmpty(Texture))
                i = (short)node._textureFiles.IndexOf(Texture);

            if (i < 0)
                texFileIndex = 0;
            else
                texFileIndex = (ushort)i;

            header->_texFileIndex = texFileIndex;

            i = 0;
            if (((PAT0TextureNode)Parent).hasPlt && !String.IsNullOrEmpty(Palette))
                i = (short)node._paletteFiles.IndexOf(Palette);

            if (i < 0)
                pltFileIndex = 0;
            else
                pltFileIndex = (ushort)i;

            header->_pltFileIndex = pltFileIndex;
        }

        protected override int OnCalculateSize(bool force)
        {
            return PAT0Texture.Size;
        }
    }
}
