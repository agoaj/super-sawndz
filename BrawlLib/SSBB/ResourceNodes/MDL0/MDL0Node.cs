using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using BrawlLib.SSBBTypes;
using BrawlLib.OpenGL;
using System.IO;
using BrawlLib.IO;
using BrawlLib.Imaging;
using BrawlLib.Modeling;
using BrawlLib.Wii.Models;
using BrawlLib.Wii.Animations;
using System.Windows.Forms;
using BrawlLib.Wii.Graphics;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class MDL0Node : BRESEntryNode, IRenderedObject
    {
        internal MDL0Header* Header { get { return (MDL0Header*)WorkingUncompressed.Address; } }

        public override ResourceType ResourceType { get { return ResourceType.MDL0; } }

        public override int DataAlign { get { return 0x20; } }

        //Changing the version will change the conversion.
        internal int _version;
        internal int _unk1, _unk2, _unk3, _unk4, _unk5, _unk6;
        internal int _numVertices, _numFaces, _numNodes;
        internal Vector3 _min, _max;

        public ModelLinker _linker;
        internal AssetStorage _assets;
        internal bool _hasTree, _hasMix, _hasOpa, _hasXlu, _isImport, _rebuildAllObj, _autoMetal, _noColors; 

        public string[] Part2Entries { get { return _part2Entries.ToArray(); } set { if (Version > 9) { _part2Entries = value.ToList<string>(); SignalPropertyChange(); } else MessageBox.Show("Versions lower than 10 do not support Part 2 entries."); } }
        internal List<string> _part2Entries = new List<string>();

        public Collada.ImportOptions _importOptions = new Collada.ImportOptions();

        public bool AutoMetalMaterials { get { return _autoMetal; } set { _autoMetal = value; CheckMetals(); } }
        [Category("MDL0 Def")]
        public int Unknown1 { get { return _unk1; } set { _unk1 = value; SignalPropertyChange(); } }
        [Category("MDL0 Def")]
        public int Unknown2 { get { return _unk2; } set { _unk2 = value; SignalPropertyChange(); } }
        [Category("MDL0 Def")]
        public int NumVertices { get { return _numVertices; } }//set { _numVertices = value; SignalPropertyChange(); } }
        [Category("MDL0 Def")]
        public int NumFaces { get { return _numFaces; } }//set { _numFaces = value; SignalPropertyChange(); } }
        [Category("MDL0 Def")]
        public int Unknown3 { get { return _unk3; } set { _unk3 = value; SignalPropertyChange(); } }
        [Category("MDL0 Def")]
        public int NumNodes { get { return _numNodes; } }// set { _numNodes = value; SignalPropertyChange(); } }
        [Category("MDL0 Def")]
        public int Version 
        { 
            get { return _version; } 
            set 
            {
                if (_version != value)
                {
                    if (((_version == 11 || _version == 10) && (value != 11 && value != 10)) ||
                        ((_version != 11 && _version != 10) && (value == 11 || value == 10)))
                        _rebuildAllObj = true;

                    if (_children == null)
                        Populate();

                    _version = value;
                    SignalPropertyChange();
                }
            } 
        }
        [Category("MDL0 Def")]
        public int Unknown4 { get { return _unk4; } set { _unk4 = value; SignalPropertyChange(); } }
        [Category("MDL0 Def")]
        public int Unknown5 { get { return _unk5; } set { _unk5 = value; SignalPropertyChange(); } }
        [Category("MDL0 Def"), TypeConverter(typeof(Vector3StringConverter))]
        public Vector3 BoxMin { get { return _min; } set { _min = value; SignalPropertyChange(); } }
        [Category("MDL0 Def"), TypeConverter(typeof(Vector3StringConverter))]
        public Vector3 BoxMax { get { return _max; } set { _max = value; SignalPropertyChange(); } }
        [Category("MDL0 Def")]
        public int Unknown6 { get { return _unk6; } set { _unk6 = value; SignalPropertyChange(); } }

        #region Immediate accessors
        
        public MDL0GroupNode _boneGroup, _matGroup, _shadGroup, _polyGroup, _texGroup, _pltGroup, _vertGroup, _normGroup, _uvGroup, _defGroup, _colorGroup;
        
        public List<ResourceNode> _boneList, _matList, _shadList, _polyList, _texList, _pltList, _vertList, _normList, _uvList, _defList, _colorList;
        
        [Browsable(false)]
        public ResourceNode DefinitionsGroup { get { return _defGroup; } }
        [Browsable(false)]
        public ResourceNode BoneGroup { get { return _boneGroup; } }
        [Browsable(false)]
        public ResourceNode MaterialGroup { get { return _matGroup; } }
        [Browsable(false)]
        public ResourceNode ShaderGroup { get { return _shadGroup; } }
        [Browsable(false)]
        public ResourceNode VertexGroup { get { return _vertGroup; } }
        [Browsable(false)]
        public ResourceNode NormalGroup { get { return _normGroup; } }
        [Browsable(false)]
        public ResourceNode UVGroup { get { return _uvGroup; } }
        [Browsable(false)]
        public ResourceNode ColorGroup { get { return _colorGroup; } }
        [Browsable(false)]
        public ResourceNode PolygonGroup { get { return _polyGroup; } }
        [Browsable(false)]
        public ResourceNode TextureGroup { get { return _texGroup; } }
        [Browsable(false)]
        public ResourceNode PaletteGroup { get { return _pltGroup; } }

        [Browsable(false)]
        public List<ResourceNode> DefinitionsList { get { return _defList; } }
        [Browsable(false)]
        public List<ResourceNode> BoneList { get { return _boneList; } }
        [Browsable(false)]
        public List<ResourceNode> MaterialList { get { return _matList; } }
        [Browsable(false)]
        public List<ResourceNode> ShaderList { get { return _shadList; } }
        [Browsable(false)]
        public List<ResourceNode> VertexList { get { return _vertList; } }
        [Browsable(false)]
        public List<ResourceNode> NormalList { get { return _normList; } }
        [Browsable(false)]
        public List<ResourceNode> UVList { get { return _uvList; } }
        [Browsable(false)]
        public List<ResourceNode> ColorList { get { return _colorList; } }
        [Browsable(false)]
        public List<ResourceNode> PolygonList { get { return _polyList; } }
        [Browsable(false)]
        public List<ResourceNode> TextureList { get { return _texList; } }
        [Browsable(false)]
        public List<ResourceNode> PaletteList { get { return _pltList; } }
        #endregion

        public List<string> _errors = new List<string>();

        public TextureManager _textures = new TextureManager();

        public List<ResourceNode> GetUsedShaders()
        {
            List<ResourceNode> shaders = new List<ResourceNode>();
            if (_shadList != null)
            foreach (MDL0ShaderNode s in _shadList)
                if (s._materials.Count > 0)
                    shaders.Add(s);
            return shaders;
        }

        public void CheckMetals()
        {
            if (_autoMetal)
            {
                if (MessageBox.Show(null, "Are you sure you want to turn this on?\nAny existing metal materials will be modified.", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    for (int x = 0; x < _matList.Count; x++)
                    {
                        MDL0MaterialNode n = (MDL0MaterialNode)_matList[x];
                        if (!n.isMetal)
                        {
                            if (n.MetalMaterial == null)
                            {
                                MDL0MaterialNode node = new MDL0MaterialNode();
                                _matGroup.AddChild(node);
                                node.updating = true;
                                node.Name = n.Name + "_ExtMtl";
                                node._ssc = 4;
                                node.New = true;

                                for (int i = 0; i <= n.Children.Count; i++)
                                {
                                    MDL0MaterialRefNode mr = new MDL0MaterialRefNode();
                                    node.AddChild(mr);
                                    mr.Texture = "metal00";
                                    mr._index1 = mr._index2 = i;
                                    if (i == n.Children.Count)
                                    {
                                        mr._minFltr = 5;
                                        mr._magFltr = 1;
                                        mr._float = -2;

                                        mr.HasTextureMatrix = true;
                                        node.Rebuild(true);

                                        mr._projection = (int)TexProjection.STQ;
                                        mr._inputForm = (int)TexInputForm.ABC1;
                                        mr._texGenType = (int)TexTexgenType.Regular;
                                        mr._sourceRow = (int)TexSourceRow.Normals;
                                        mr._embossSource = 4;
                                        mr._embossLight = 2;
                                        mr.Normalize = true;

                                        mr.TexUnk3 = 1;

                                        mr.getTexMtxVal();
                                    }
                                    mr.SignalPropertyChange();
                                }

                                node.flags0 = 63;
                                node.c00.R = node.c00.G = node.c00.B = 128; node.c00.A = 255;
                                node.c01.R = node.c01.G = node.c01.B = node.c01.A = 255;
                                node.e01 = node.e03 = 2;
                                node.e00 = node.e02 = 7;
                                node.flags1 = 63;
                                node.c10.R = node.c10.G = node.c10.B = node.c10.A = 255;
                                node.e10 = node.e11 = node.e12 = 2;

                                node._lSet = n._lSet;
                                node._fSet = n._fSet;
                                node._unk1 = n._unk1;

                                node._cull = n._cull;
                                node._numLights = 2;
                                node.EnableAlphaFunction = false;
                                node._unk3 = -1;

                                node.SignalPropertyChange();
                            }
                        }
                    }
                    foreach (MDL0MaterialNode node in _matList)
                    {
                        if (!node.isMetal)
                            continue;

                        if (node.ShaderNode != null)
                        {
                            if (node.ShaderNode._autoMetal && node.ShaderNode.texCount == node.Children.Count)
                            {
                                node.updating = false;
                                continue;
                            }
                            else
                            {
                                if (node.ShaderNode.stages == 4)
                                {
                                    foreach (MDL0MaterialNode y in node.ShaderNode._materials)
                                        if (!y.isMetal || y.Children.Count != node.Children.Count)
                                            goto Next;
                                    node.ShaderNode.DefaultAsMetal(node.Children.Count);
                                    continue;
                                }
                            }
                        }
                    Next:
                        bool found = false;
                        foreach (MDL0ShaderNode s in _shadGroup.Children)
                        {
                            if (s._autoMetal && s.texCount == node.Children.Count)
                            {
                                node.ShaderNode = s;
                                found = true;
                            }
                            else
                            {
                                if (s.stages == 4)
                                {
                                    foreach (MDL0MaterialNode y in s._materials)
                                        if (!y.isMetal || y.Children.Count != node.Children.Count)
                                            goto NotFound;
                                    node.ShaderNode = s;
                                    found = true;
                                    goto End;
                                NotFound:
                                    continue;
                                }
                            }
                        }
                    End:
                        if (!found)
                        {
                            MDL0ShaderNode shader = new MDL0ShaderNode();
                            _shadGroup.AddChild(shader);
                            node.ShaderNode = shader;
                            shader.DefaultAsMetal(node.Children.Count);
                        }
                    }
                    foreach (MDL0MaterialNode m in _matList)
                        m.updating = false;
                }
                else
                    _autoMetal = false;
            }
        }

        public void CleanTextures()
        {
            if (_texList != null)
            {
                int i = 0;
                while (i < _texList.Count)
                {
                    MDL0TextureNode texture = (MDL0TextureNode)_texList[i];

                t1:
                    foreach (MDL0MaterialRefNode r in texture._references)
                        if (_matList.IndexOf(r.Parent) == -1)
                        {
                            texture._references.Remove(r);
                            goto t1;
                        }

                    if (texture._references.Count == 0)
                        _texList.RemoveAt(i);
                    else
                        i++;
                }
            }

            if (_pltList != null)
            {
                int i = 0;
                while (i < _pltList.Count)
                {
                    MDL0TextureNode palette = (MDL0TextureNode)_pltList[i];

                t1:
                    foreach (MDL0MaterialRefNode r in palette._references)
                        if (_matList.IndexOf(r.Parent) == -1)
                        {
                            palette._references.Remove(r);
                            goto t1;
                        }

                    if (palette._references.Count == 0)
                        _pltList.RemoveAt(i);
                    else
                        i++;
                }
            }
        }

        public MDL0TextureNode FindOrCreateTexture(string name)
        {
            if (_texGroup == null)
                AddChild(_texGroup = new MDL0GroupNode(MDLResourceType.Textures), false);
            else
                foreach (MDL0TextureNode n in _texGroup.Children)
                    if (n._name == name)
                        return n;

            MDL0TextureNode node = new MDL0TextureNode(name);
            _texGroup.AddChild(node, false);

            return node;
        }
        public MDL0TextureNode FindOrCreatePalette(string name)
        {
            if (_pltGroup == null)
                AddChild(_pltGroup = new MDL0GroupNode(MDLResourceType.Palettes), false);
            else
                foreach (MDL0TextureNode n in _pltGroup.Children)
                    if (n._name == name)
                        return n;

            MDL0TextureNode node = new MDL0TextureNode(name);
            _pltGroup.AddChild(node, false);

            return node;
        }
        internal MDL0BoneNode FindBone(string name)
        {
            foreach (MDL0BoneNode b in _linker.BoneCache)
                if (b.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                    return b;
            
            //MDL0BoneNode node = new MDL0BoneNode() { _name = name };
            //_boneGroup.AddChild(node, false);

            return null;
        }
        public MDL0MaterialNode FindOrCreateMaterial(string name)
        {
            foreach (MDL0MaterialNode m in _matList)
                if (m.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                    return m;
            
            MDL0MaterialNode node = new MDL0MaterialNode() { _name = name };
            _matGroup.AddChild(node, false);

            return node;
        }
        public override void AddChild(ResourceNode child, bool change)
        {
            if (child is MDL0GroupNode)
                LinkGroup(child as MDL0GroupNode);
            base.AddChild(child, change);
        }

        #region Linking
        public void LinkGroup(MDL0GroupNode group)
        {
            switch (group._type)
            {
                case MDLResourceType.Definitions: { _defGroup = group; _defList = group._children; break; }
                case MDLResourceType.Bones: { _boneGroup = group; _boneList = group._children; break; }
                case MDLResourceType.Materials: { _matGroup = group; _matList = group._children; break; }
                case MDLResourceType.Shaders: { _shadGroup = group; _shadList = group._children; break; }
                case MDLResourceType.Vertices: { _vertGroup = group; _vertList = group._children; break; }
                case MDLResourceType.Normals: { _normGroup = group; _normList = group._children; break; }
                case MDLResourceType.UVs: { _uvGroup = group; _uvList = group._children; break; }
                case MDLResourceType.Colors: { _colorGroup = group; _colorList = group._children; break; }
                case MDLResourceType.Objects: { _polyGroup = group; _polyList = group._children; break; }
                case MDLResourceType.Textures: { _texGroup = group; _texList = group._children; break; }
                case MDLResourceType.Palettes: { _pltGroup = group; _pltList = group._children; break; }
            }
        }
        public void UnlinkGroup(MDL0GroupNode group)
        {
            if (group != null)
            switch (group._type)
            {
                case MDLResourceType.Definitions: { _defGroup = null; _defList = null; break; }
                case MDLResourceType.Bones: { _boneGroup = null; _boneList = null; break; }
                case MDLResourceType.Materials: { _matGroup = null; _matList = null; break; }
                case MDLResourceType.Shaders: { _shadGroup = null; _shadList = null; break; }
                case MDLResourceType.Vertices: { _vertGroup = null; _vertList = null; break; }
                case MDLResourceType.Normals: { _normGroup = null; _normList = null; break; }
                case MDLResourceType.UVs: { _uvGroup = null; _uvList = null; break; }
                case MDLResourceType.Colors: { _colorGroup = null; _colorList = null; break; }
                case MDLResourceType.Objects: { _polyGroup = null; _polyList = null; break; }
                case MDLResourceType.Textures: { _texGroup = null; _texList = null; break; }
                case MDLResourceType.Palettes: { _pltGroup = null; _pltList = null; break; }
            }
        }
        internal void InitGroups()
        {
            LinkGroup(new MDL0GroupNode(MDLResourceType.Definitions));
            LinkGroup(new MDL0GroupNode(MDLResourceType.Bones));
            LinkGroup(new MDL0GroupNode(MDLResourceType.Materials));
            LinkGroup(new MDL0GroupNode(MDLResourceType.Shaders));
            LinkGroup(new MDL0GroupNode(MDLResourceType.Vertices));
            LinkGroup(new MDL0GroupNode(MDLResourceType.Normals));
            LinkGroup(new MDL0GroupNode(MDLResourceType.UVs));
            LinkGroup(new MDL0GroupNode(MDLResourceType.Colors));
            LinkGroup(new MDL0GroupNode(MDLResourceType.Objects));
            LinkGroup(new MDL0GroupNode(MDLResourceType.Textures));
            LinkGroup(new MDL0GroupNode(MDLResourceType.Palettes));

            _defGroup._parent = this;
            _boneGroup._parent = this;
            _matGroup._parent = this;
            _shadGroup._parent = this;
            _vertGroup._parent = this;
            _normGroup._parent = this;
            _uvGroup._parent = this;
            _colorGroup._parent = this;
            _polyGroup._parent = this;
            _texGroup._parent = this;
            _pltGroup._parent = this;
        }
        internal void CleanGroups()
        {
            if (_defList.Count > 0)
                _children.Add(_defGroup);
            else
                UnlinkGroup(_defGroup);

            if (_boneList.Count > 0)
                _children.Add(_boneGroup);
            else
                UnlinkGroup(_boneGroup);

            if (_matList.Count > 0)
                _children.Add(_matGroup);
            else
                UnlinkGroup(_matGroup);

            if (_shadList.Count > 0)
                _children.Add(_shadGroup);
            else
                UnlinkGroup(_shadGroup);

            if (_vertList.Count > 0)
                _children.Add(_vertGroup);
            else
                UnlinkGroup(_vertGroup);

            if (_normList.Count > 0)
                _children.Add(_normGroup);
            else
                UnlinkGroup(_normGroup);

            if (_uvList.Count > 0)
                _children.Add(_uvGroup);
            else
                UnlinkGroup(_uvGroup);

            if (_colorList.Count > 0)
                _children.Add(_colorGroup);
            else
                UnlinkGroup(_colorGroup);

            if (_polyList.Count > 0)
                _children.Add(_polyGroup);
            else
                UnlinkGroup(_polyGroup);

            if (_texList.Count > 0)
                _children.Add(_texGroup);
            else
                UnlinkGroup(_texGroup);

            if (_pltList.Count > 0)
                _children.Add(_pltGroup);
            else
                UnlinkGroup(_pltGroup);
        }
        #endregion

        public override void RemoveChild(ResourceNode child)
        {
            if (child is MDL0GroupNode)
                UnlinkGroup(child as MDL0GroupNode);
            base.RemoveChild(child);
        }

        protected override bool OnInitialize()
        {
            base.OnInitialize();

            MDL0Header* header = Header;
            int offset;

            if ((_name == null) && ((offset = header->StringOffset) != 0))
                _name = new String((sbyte*)header + offset);

            MDL0Props* props = header->Properties;

            _version = header->_header._version;
            _unk1 = props->_unk1;
            _unk2 = props->_unk2;
            _numVertices = props->_numVertices;
            _numFaces = props->_numFaces;
            _unk3 = props->_unk3;
            _numNodes = props->_numNodes;
            _unk4 = props->_unk4;
            _unk5 = props->_unk5;
            _min = props->_minExtents;
            _max = props->_maxExtents;
            _unk6 = props->_unk6;

            Part2Data* part2 = header->Part2;
            if (part2 != null)
            {
                ResourceGroup* group = part2->Group;
                for (int i = 0; i < group->_numEntries; i++)
                    _part2Entries.Add(group->First[i].GetName());
            }

            return true;
        }

        protected override void OnPopulate()
        {
            InitGroups();
            _linker = new ModelLinker(Header);
            _assets = new AssetStorage(_linker);
            try
            {
                //Set def flags
                _hasMix = _hasOpa = _hasTree = _hasXlu = false;
                if (_linker.Defs != null)
                    foreach (ResourcePair p in *_linker.Defs)
                        if (p.Name == "NodeTree") _hasTree = true;
                        else if (p.Name == "NodeMix") _hasMix = true;
                        else if (p.Name == "DrawOpa") _hasOpa = true;
                        else if (p.Name == "DrawXlu") _hasXlu = true;

                //No point parsing what will be automatically regenerated with no user input.
                //_texGroup.Parse(this);
                //_pltGroup.Parse(this);

                _defGroup.Parse(this);
                _boneGroup.Parse(this);
                _matGroup.Parse(this);
                _shadGroup.Parse(this);
                _vertGroup.Parse(this);
                _normGroup.Parse(this);
                _uvGroup.Parse(this);
                _colorGroup.Parse(this);
                _polyGroup.Parse(this); //Parse objects last!
            }
            finally //Clean up!
            {
                //We'll use the linker to access the bone cache
                //_linker = null;

                //Don't dispose assets, in case an object is replaced
                //_assets.Dispose();
                //_assets = null;

                CleanGroups();

                //Check for model errors
                if (_errors.Count > 0)
                {
                    string message = _errors.Count + (_errors.Count > 1 ? " errors have" : " error has") + " been found in the model " + _name + ".\n" + (_errors.Count > 1 ? "These errors" : "This error") + " will be fixed when you save:";
                    foreach (string s in _errors)
                        message += "\n - " + s;
                    MessageBox.Show(message);
                }

                Populate();
            }
        }

        internal InfluenceManager _influences = new InfluenceManager();

        internal override void GetStrings(StringTable table)
        {
            table.Add(Name);
            foreach (MDL0GroupNode n in Children)
                n.GetStrings(table);

            _hasOpa = _hasXlu = false;

            if (_matList != null)
            foreach (MDL0MaterialNode n in _matList)
                if (n.XLUMaterial)
                    _hasXlu = true;
                else
                    _hasOpa = true;

            //Add def names
            if (_hasTree) table.Add("NodeTree");
            if (_hasMix) table.Add("NodeMix");
            if (_hasOpa) table.Add("DrawOpa");
            if (_hasXlu) table.Add("DrawXlu");

            foreach (string s in _part2Entries)
                table.Add(s);
        }

        public static MDL0Node FromFile(string path)
        {
            //string ext = Path.GetExtension(path);
            if (path.EndsWith(".mdl0", StringComparison.OrdinalIgnoreCase))
                return NodeFactory.FromFile(null, path) as MDL0Node;
            else if (path.EndsWith(".dae", StringComparison.OrdinalIgnoreCase))
                return new Collada().ShowDialog(path);
            else if (path.EndsWith(".pmd", StringComparison.OrdinalIgnoreCase))
                return PMDModel.ImportModel(path);
            //else if (string.Equals(ext, "fbx", StringComparison.OrdinalIgnoreCase))
            //{
            //}
            //else if (string.Equals(ext, "blend", StringComparison.OrdinalIgnoreCase))
            //{
            //}

            throw new NotSupportedException("The file extension specified is not of a supported model type.");
        }

        #region Saving
        public override unsafe void Export(string outPath)
        {
            if (outPath.ToUpper().EndsWith(".DAE"))
                Collada.Serialize(this, outPath);
            else if (outPath.ToUpper().EndsWith(".PMD"))
                PMDModel.Export(this, outPath);
            else
                base.Export(outPath);
        }
        public bool reopen = false;
        public void BuildFromScratch(Collada form)
        {
            _isImport = true;

            _influences.Clean();
            _influences.Sort();

            CleanTextures();

            _linker = ModelLinker.Prepare(this);
            int size = ModelEncoder.CalcSize(form, _linker);

            FileMap uncompMap = FileMap.FromTempFile(size);

            ModelEncoder.Build(form, _linker, (MDL0Header*)uncompMap.Address, size, true);

            _replSrc.Close();
            _replUncompSrc.Close();
            _replSrc = _replUncompSrc = new DataSource(uncompMap.Address, size);

            _changed = false;
            _replSrc.Map = _replUncompSrc.Map = uncompMap;

            reopen = true;
        }

        protected override int OnCalculateSize(bool force)
        {
            //Clean and sort influence list
            _influences.Clean();
            _influences.Sort();

            //Clean texture list
            CleanTextures();

            _linker = ModelLinker.Prepare(this);
            return ModelEncoder.CalcSize(_linker);
        }
        protected internal override void OnRebuild(VoidPtr address, int length, bool force)
        {
            ModelEncoder.Build(_linker, (MDL0Header*)address, length, force);
            _rebuildAllObj = false;
        }
        protected internal override void PostProcess(VoidPtr bresAddress, VoidPtr dataAddress, int dataLength, StringTable stringTable)
        {
            base.PostProcess(bresAddress, dataAddress, dataLength, stringTable);

            MDL0Header* header = (MDL0Header*)dataAddress;
            ResourceGroup* pGroup, sGroup;
            ResourceEntry* pEntry, sEntry;
            bint* offsets = header->Offsets;
            int index, sIndex;

            //Model name
            header->StringOffset = (int)((byte*)stringTable[Name] + 4 - (byte*)header);

            //Post-process groups, using linker lists
            List<MDLResourceType> gList = ModelLinker.IndexBank[_version];
            foreach (MDL0GroupNode node in Children)
            {
                MDLResourceType type = (MDLResourceType)Enum.Parse(typeof(MDLResourceType), node.Name);
                if (((index = gList.IndexOf(type)) >= 0) && (type != MDLResourceType.Shaders))
                    node.PostProcess(dataAddress, dataAddress + offsets[index], stringTable);
            }

            //Post-process definitions
            index = gList.IndexOf(MDLResourceType.Definitions);
            pGroup = (ResourceGroup*)(dataAddress + offsets[index]);
            pGroup->_first = new ResourceEntry(0xFFFF, 0, 0, 0);
            pEntry = &pGroup->_first + 1;
            index = 1;
            if (_hasTree)
                ResourceEntry.Build(pGroup, index++, (byte*)pGroup + (pEntry++)->_dataOffset, (BRESString*)stringTable["NodeTree"]);
            if (_hasMix)
                ResourceEntry.Build(pGroup, index++, (byte*)pGroup + (pEntry++)->_dataOffset, (BRESString*)stringTable["NodeMix"]);
            if (_hasOpa)
                ResourceEntry.Build(pGroup, index++, (byte*)pGroup + (pEntry++)->_dataOffset, (BRESString*)stringTable["DrawOpa"]);
            if (_hasXlu)
                ResourceEntry.Build(pGroup, index++, (byte*)pGroup + (pEntry++)->_dataOffset, (BRESString*)stringTable["DrawXlu"]);

            //Link shader names using material list
            index = offsets[gList.IndexOf(MDLResourceType.Materials)];
            sIndex = offsets[gList.IndexOf(MDLResourceType.Shaders)];
            if ((index > 0) && (sIndex > 0))
            {
                pGroup = (ResourceGroup*)(dataAddress + index);
                sGroup = (ResourceGroup*)(dataAddress + sIndex);
                pEntry = &pGroup->_first + 1;
                sEntry = &sGroup->_first + 1;

                sGroup->_first = new ResourceEntry(0xFFFF, 0, 0, 0);
                index = pGroup->_numEntries;
                for (int i = 1; i <= index; i++)
                {
                    VoidPtr dataAddr = (VoidPtr)sGroup + (sEntry++)->_dataOffset;
                    ResourceEntry.Build(sGroup, i, dataAddr, (BRESString*)((byte*)pGroup + (pEntry++)->_stringOffset - 4));
                    ((MDL0Shader*)dataAddr)->_mdl0Offset = (int)dataAddress - (int)dataAddr;
                }
            }
            
            //Write part2 entries
            if (_part2Entries.Count > 0 && Version > 9)
            {
                pGroup = (ResourceGroup*)((byte*)header + header->_part2Offset + 4);
                pEntry = &pGroup->_first;
                int count = pGroup->_numEntries;
                (*pEntry++) = new ResourceEntry(0xFFFF, 0, 0, 0, 0);

                for (int i = 0; i < count; i++)
                {
                    Part2DataEntry* entry = (Part2DataEntry*)((byte*)pGroup + (pEntry++)->_dataOffset);
                    entry->_stringOffset = (int)stringTable[_part2Entries[i]] + 4 - ((int)entry + (int)dataAddress);
                    ResourceEntry.Build(pGroup, i + 1, entry, (BRESString*)stringTable[_part2Entries[i]]);
                }
            }
        }
        #endregion

        #region Rendering

        internal bool _bound = false;
        internal bool _renderPolygons = true;
        internal bool _renderPolygonsWireframe = false;
        internal bool _renderBones = true;
        internal bool _renderVertices = false;
        internal int polyIndex = -1;
        internal bool _visible = false;
        //public int _animFrame = 0;

        public Vector4 Ambient = new Vector4(0.2f, 0.2f, 0.2f, 1);
        public Vector4 Position = new Vector4(0, 6, 6, 0);
        public Vector4 Diffuse = new Vector4(0.8f, 0.8f, 0.8f, 1);
        public Vector4 Specular = new Vector4(0.5f, 0.5f, 0.5f, 1);

        internal bool _canUndo = false;
        internal bool _canRedo = false;
        internal bool _primarySave = true;
        public List<SaveState> Saves = new List<SaveState>();
        public int _currentSave;
        public int _saveIndex = 0;
        public Dictionary<string, List<int>> VIS0Indices;

        public void ResetSaves()
        {
            Saves.Clear();
            _saveIndex = 0;
            _primarySave = true;
            _canUndo = _canRedo = false;
        }

        public void Attach(GLContext context)
        {
            _visible = true;
            ApplyCHR(null, 0);
            ApplySRT(null, 0);
            //ApplySHP(null, 0);

            foreach (MDL0GroupNode g in Children)
                g.Bind(context);

            VIS0Indices = new Dictionary<string, List<int>>(); int i = 0;
            if (_polyList != null)
            foreach (MDL0PolygonNode p in _polyList)
            {
                if (p._bone != null && p._bone.BoneIndex != 0)
                    if (VIS0Indices.ContainsKey(p._bone.Name))
                        if (!VIS0Indices[p._bone.Name].Contains(i))
                            VIS0Indices[p._bone.Name].Add(i);
                        else { }
                    else VIS0Indices.Add(p._bone.Name, new List<int> { i });
                i++;
            }
        }

        public void Detach(GLContext context)
        {
            _visible = false;
            //Unweight();
            foreach (MDL0GroupNode g in Children)
                g.Unbind(context);
        }

        public void Refesh(GLContext context)
        {
            if (_texList != null)
                foreach (MDL0TextureNode t in _texList)
                    t.Reload();
        }

        public Matrix floorShadow;
        public void Render(GLContext ctx)
        {
            if (!_visible)
                return;

            if (_renderPolygons)
            {
                //float e = 30.0f;
                //floorShadow = shadowMatrix(findPlane(new Vector3(-e, 0.0f, -e), new Vector3(e, 0.0f, -e), new Vector3(e, 0.0f, e)), Light);

                ctx.glEnable(GLEnableCap.Lighting);
                ctx.glEnable(GLEnableCap.DepthTest);
                if (_renderPolygonsWireframe)
                    ctx.glPolygonMode(GLFace.FrontAndBack, GLPolygonMode.Line);
                else
                    ctx.glPolygonMode(GLFace.FrontAndBack, GLPolygonMode.Fill);

                if (_polyList != null)
                    foreach (MDL0PolygonNode poly in _polyList)
                        poly.Render(ctx);
            }

            if (_renderBones)
            {
                ctx.glDisable((uint)GLEnableCap.Lighting);
                ctx.glDisable((uint)GLEnableCap.DepthTest);
                ctx.glPolygonMode(GLFace.FrontAndBack, GLPolygonMode.Fill);

                if (_boneList != null)
                    foreach (MDL0BoneNode bone in _boneList)
                        bone.Render(ctx);
            }

            if (_renderVertices)
            {
                ctx.glDisable((uint)GLEnableCap.Lighting);
                if (polyIndex != -1)
                {
                    ctx.glDisable((uint)GLEnableCap.DepthTest);
                    ((MDL0PolygonNode)_polyList[polyIndex])._manager.RenderVerts(ctx, ((MDL0PolygonNode)_polyList[polyIndex])._singleBind);
                }
                else
                {
                    ctx.glEnable(GLEnableCap.DepthTest);
                    foreach (MDL0PolygonNode p in _polyList)
                        p._manager.RenderVerts(ctx, p._singleBind);
                }
            }
        }

        internal void ApplyCHR(CHR0Node node, int index)
        {
            //Transform bones
            if (_boneList != null)
            {
                foreach (MDL0BoneNode b in _boneList)
                    b.ApplyCHR0(node, index);
                foreach (MDL0BoneNode b in _boneList)
                    b.RecalcFrameState();
            }
            //Transform nodes
            foreach (Influence inf in _influences._influences)
                inf.CalcMatrix();

            //Weight Vertices
            if (_polyList != null)
                foreach (MDL0PolygonNode poly in _polyList)
                    poly.WeightVertices();
        }

        internal void ApplySRT(SRT0Node node, int index)
        {
            //Transform textures
            if (_matList != null)
                foreach (MDL0MaterialNode m in _matList)
                    m.ApplySRT0(node, index);
        }

        internal void ApplyPAT(PAT0Node node, int index)
        {
            //Change textures
            if (_matList != null)
                foreach (MDL0MaterialNode m in _matList)
                    m.ApplyPAT0(node, index);
        }

        internal void ApplyVIS(VIS0Node _vis0, int _animFrame)
        {
            foreach (string n in VIS0Indices.Keys)
            {
                VIS0EntryNode node = null;
                List<int> indices = VIS0Indices[n];
                for (int i = 0; i < indices.Count; i++)
                    if ((node = (VIS0EntryNode)_vis0.FindChild(((MDL0PolygonNode)_polyList[indices[i]])._bone.Name, true)) != null)
                        if (node._entryCount != 0 && _animFrame != 0)
                            ((MDL0PolygonNode)_polyList[indices[i]])._render = node.GetEntry(_animFrame - 1);
                        else
                            ((MDL0PolygonNode)_polyList[indices[i]])._render = node._flags.HasFlag(VIS0Flags.Enabled);
            }
        }

        internal void ApplySHP(SHP0Node node, int index)
        {
            if (node == null || index == 0)
                return;

            SHP0EntryNode n;
            
            if (_polyList != null)
                foreach (MDL0PolygonNode poly in _polyList)
                    //See if the SHP0 influences this object
                    if ((n = node.FindChild(poly.VertexNode, true) as SHP0EntryNode) != null)
                        //Retrieve the influenced vertex sets referenced in the SHP0
                        foreach (SHP0VertexSetNode v in n.Children)
                        {
                            MDL0VertexNode vNode = null;
                            foreach (MDL0VertexNode vn in _vertList)
                                if (vn.Name == v.Name)
                                {
                                    vNode = vn;
                                    break;
                                }
                            if (vNode != null) 
                            {
                                //Morph vertices of this object to this vertex set

                                float percent = v.Keyframes.GetFrameValue(KeyFrameMode.ScaleX, index - 1);

                                //Clamp percentage from 0 to 1
                                percent = percent < 0 ? 0 : percent > 1 ? 1 : percent;

                                if (poly.Weighted)
                                {
                                    ushort* pMap = (ushort*)poly._manager._desc.RemapTable.Address;
                                    for (int i = 0; i < poly._manager._desc.RemapSize; i++)
                                    {
                                        Vertex3 v3 = poly._manager._vertices[i];

                                        //Create weighted vertex from the vertex set this object is being morphed to
                                        Vector3 weightedExtV = v3._influence != null ? v3._influence.Matrix * vNode.Vertices[*pMap++] : vNode.Vertices[*pMap++];
                                        
                                        //Now morph the vertex
                                        v3.Morph(weightedExtV, percent);

                                        //Skip influence
                                        pMap++;
                                    }
                                }
                                else
                                {
                                    int* pMap = (int*)poly._manager._desc.RemapTable.Address;
                                    for (int i = 0; i < poly._manager._desc.RemapSize; i++)
                                    {
                                        Vertex3 v3 = poly._manager._vertices[i];
                                        v3.Morph(vNode.Vertices[*pMap++], percent);
                                    }
                                }
                            }
                        }
        }

        internal void Unweight()
        {
            //Unweight Vertices
            if (_polyList != null)
                foreach (MDL0PolygonNode poly in _polyList)
                    poly.UnWeightVertices();
        }
        public void Undo()
        {
            Saves[_saveIndex].redo = true;
            Saves[_saveIndex].undo = false;
            _saveIndex--;
            if (_saveIndex < 0)
                _saveIndex = 0;
            Saves[_saveIndex].redo = false;
            Saves[_saveIndex].undo = true;
            _canRedo = true;
        }
        public void Redo()
        {
            Saves[_saveIndex].redo = false;
            Saves[_saveIndex].undo = true;
            _saveIndex++;
            if (_saveIndex >= Saves.Count)
                _saveIndex = Saves.Count - 1;
            Saves[_saveIndex].redo = true;
            Saves[_saveIndex].undo = false;
            _canUndo = true;
        }

        public Vector4 findPlane(Vector3 v0, Vector3 v1, Vector3 v2)
        {
            Vector4 plane = new Vector4();
            Vector3 vec0, vec1;

            //Need 2 vectors to find cross product.
            vec0._x = v1._x - v0._x;
            vec0._y = v1._y - v0._y;
            vec0._z = v1._z - v0._z;

            vec1._x = v2._x - v0._x;
            vec1._y = v2._y - v0._y;
            vec1._z = v2._y - v0._z;

            //Find cross product to get A, B, and C of plane equation
            plane._x = vec0._y * vec1._z - vec0._z * vec1._y;
            plane._y = -(vec0._x * vec1._z - vec0._z * vec1._x);
            plane._z = vec0._x * vec1._y - vec0._y * vec1._x;

            plane._w = -(plane._x * v0._x + plane._y * v0._y + plane._z * v0._z);
            return plane;
        }

        public Matrix shadowMatrix(Vector4 groundplane, Vector4 lightpos)
        {
            Matrix shadowMat = new Matrix();
            float dot = groundplane._x * lightpos._x +
                        groundplane._y * lightpos._y +
                        groundplane._z * lightpos._z +
                        groundplane._w * lightpos._w;

            shadowMat[0, 0] = dot - lightpos._x * groundplane._x;
            shadowMat[1, 0] = 0.0f - lightpos._x * groundplane._y;
            shadowMat[2, 0] = 0.0f - lightpos._x * groundplane._z;
            shadowMat[3, 0] = 0.0f - lightpos._x * groundplane._w;

            shadowMat[0, 1] = 0.0f - lightpos._y * groundplane._x;
            shadowMat[1, 1] = dot - lightpos._y * groundplane._y;
            shadowMat[2, 1] = 0.0f - lightpos._y * groundplane._z;
            shadowMat[3, 1] = 0.0f - lightpos._y * groundplane._w;

            shadowMat[0, 2] = 0.0f - lightpos._z * groundplane._x;
            shadowMat[1, 2] = 0.0f - lightpos._z * groundplane._y;
            shadowMat[2, 2] = dot - lightpos._z * groundplane._z;
            shadowMat[3, 2] = 0.0f - lightpos._z * groundplane._w;

            shadowMat[0, 3] = 0.0f - lightpos._w * groundplane._x;
            shadowMat[1, 3] = 0.0f - lightpos._w * groundplane._y;
            shadowMat[2, 3] = 0.0f - lightpos._w * groundplane._z;
            shadowMat[3, 3] = dot - lightpos._w * groundplane._w;

            return shadowMat;
        }
        #endregion

        internal static ResourceNode TryParse(DataSource source) { return ((MDL0Header*)source.Address)->_header._tag == MDL0Header.Tag ? new MDL0Node() : null; }
    }
}
