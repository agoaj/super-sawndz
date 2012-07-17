using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.SSBBTypes;
using System.ComponentModel;
using BrawlLib.OpenGL;
using System.Drawing;
using System.IO;
using BrawlLib.Imaging;
using System.Drawing.Imaging;
using BrawlLib.Modeling;
using BrawlLib.Wii.Graphics;
using System.Windows.Forms;
using BrawlLib.Wii.Models;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class MDL0MaterialRefNode : MDL0EntryNode
    {
        internal MDL0TextureRef* Header { get { return (MDL0TextureRef*)_origSource.Address; } set { _origSource.Address = value; } }

        public override bool AllowDuplicateNames { get { return true; } }

        [Browsable(false)]
        public MDL0MaterialNode Material { get { return Parent as MDL0MaterialNode; } }

        public TextureFlags _texFlags;
        public TextureMatrix _texMatrix;

        [Browsable(false)]
        public int TextureCoordId 
        {
            get 
            {
                if ((int)Coordinates >= (int)TexSourceRow.TexCoord0)
                    return (int)Coordinates - (int)TexSourceRow.TexCoord0;
                else
                    return -1 - (int)Coordinates;
            } 
        }

        [Category("Texture Coordinates"), TypeConverter(typeof(Vector2StringConverter))]
        public Vector2 Scale { get { return _texFlags.TexScale; } set { if (!CheckIfMetal()) { _texFlags.TexScale = value; _bindState._scale = new Vector3(value._x, value._y, 1); } } }
        [Category("Texture Coordinates")]
        public float Rotation { get { return _texFlags.TexRotation; } set { if (!CheckIfMetal()) { _texFlags.TexRotation = value; _bindState._rotate = new Vector3(value, 0, 0); } } }
        [Category("Texture Coordinates"), TypeConverter(typeof(Vector2StringConverter))]
        public Vector2 Translation { get { return _texFlags.TexTranslation; } set { if (!CheckIfMetal()) { _texFlags.TexTranslation = value; _bindState._translate = new Vector3(value._x, value._y, 0); } } }
        [Category("Texture Coordinates")]
        public TexFlags Flags { get { return _flags; } }
        public TexFlags _flags;

        [Category("Texture Matrix")]
        public sbyte TexUnk1 { get { return _texMatrix.TexUnk1; } set { if (!CheckIfMetal()) _texMatrix.TexUnk1 = value; } }
        [Category("Texture Matrix")]
        public sbyte TexUnk2 { get { return _texMatrix.TexUnk2; } set { if (!CheckIfMetal()) _texMatrix.TexUnk2 = value; } }
        [Category("Texture Matrix")]
        public sbyte TexUnk3 { get { return _texMatrix.TexUnk3; } set { if (!CheckIfMetal()) _texMatrix.TexUnk3 = value; } }
        [Category("Texture Matrix")]
        public sbyte TexUnk4 { get { return _texMatrix.TexUnk4; } set { if (!CheckIfMetal()) _texMatrix.TexUnk4 = value; } }
        [Category("Texture Matrix"), TypeConverter(typeof(Matrix43StringConverter))]
        public Matrix43 TexMtx { get { return _texMatrix.TexMtx; } set { if (!CheckIfMetal()) _texMatrix.TexMtx = value; } }

        public XFDualTex DualTexFlags;
        public XFTexMtxInfo TexMtxFlags;

        internal int _projection; //Normal enable is true when projection is XF_TEX_STQ
        internal int _inputForm;
        internal int _texGenType;
        internal int _sourceRow;
        internal int _embossSource;
        internal int _embossLight;

        public bool _hasTexMtx = false;
        public bool HasTextureMatrix 
        {
            get
            {
                //if (_hasTexMtx == false)
                //    return false;

                bool allsinglebinds = true;
                if (((MDL0MaterialNode)Parent).Objects != null)
                {
                    foreach (MDL0PolygonNode n in ((MDL0MaterialNode)Parent).Objects)
                        if (n.Weighted)
                        {
                            allsinglebinds = false;
                            if (n._arrayFlags.GetHasTexMatrix(Index) == false)
                                return false;
                        }
                }
                else return false;

                if (allsinglebinds)
                    return false;

                return true;
            }
            set 
            {
                bool changed = false;
                foreach (MDL0PolygonNode n in ((MDL0MaterialNode)Parent).Objects)
                    if (n.Weighted)
                    {
                        n._vertexFormat.SetHasTexMatrix(Index, value);
                        n._arrayFlags.SetHasTexMatrix(Index, value);
                        n._rebuild = true;
                        Model.SignalPropertyChange();

                        if (n._vertexNode.Format != WiiVertexComponentType.Float)
                            n._vertexNode._forceRebuild = value;
                        if (n._normalNode.Format != WiiVertexComponentType.Float)
                            n._normalNode._forceRebuild = value;
                        for (int i = 4; i < 12; i++)
                            if (n._uvSet[i - 4] != null && n._uvSet[i - 4].Format != WiiVertexComponentType.Float)
                                n._uvSet[i - 4]._forceRebuild = value;

                        changed = true;
                    }
                if (changed) _hasTexMtx = value;
            }
        }
        
        [Category("XF TexGen Flags")]
        public TexProjection Projection { get { return (TexProjection)_projection; } set { if (!CheckIfMetal()) { _projection = (int)value; getTexMtxVal(); } } }
        [Category("XF TexGen Flags")]
        public TexInputForm InputForm { get { return (TexInputForm)_inputForm; } set { if (!CheckIfMetal()) { _inputForm = (int)value; getTexMtxVal(); } } }
        [Category("XF TexGen Flags")]
        public TexTexgenType Type { get { return (TexTexgenType)_texGenType; } set { if (!CheckIfMetal()) { _texGenType = (int)value; getTexMtxVal(); } } }
        [Category("XF TexGen Flags")]
        public TexSourceRow Coordinates { get { return (TexSourceRow)_sourceRow; } set { if (!CheckIfMetal()) { _sourceRow = (int)value; getTexMtxVal(); } } }
        [Category("XF TexGen Flags")]
        public int EmbossSource { get { return _embossSource; } set { if (!CheckIfMetal()) { _embossSource = value; getTexMtxVal(); } } }
        [Category("XF TexGen Flags")]
        public int EmbossLight { get { return _embossLight; } set { if (!CheckIfMetal()) { _embossLight = value; getTexMtxVal(); } } }
        [Category("XF TexGen Flags")]
        public bool Normalize { get { return DualTexFlags.NormalEnable != 0; } set { if (!CheckIfMetal()) { DualTexFlags.NormalEnable = (byte)(value ? 1 : 0); } } }
        
        public void getTexMtxVal()
        {
            TexMtxFlags._data = (uint)(0 |
            (_projection << 1) |
            (_inputForm << 2) |
            (_texGenType << 4) |
            (_sourceRow << 7) |
            (_embossSource << 10) |
            (_embossLight << 13));

            SignalPropertyChange();
        }

        public void getValues()
        {
            _projection = (int)TexMtxFlags.Projection;
            _inputForm = (int)TexMtxFlags.InputForm;
            _texGenType = (int)TexMtxFlags.TexGenType;
            _sourceRow = (int)TexMtxFlags.SourceRow;
            _embossSource = (int)TexMtxFlags.EmbossSource;
            _embossLight = (int)TexMtxFlags.EmbossLight;
        }

        internal int _unk2;
        internal int _unk3;
        internal int _index1;
        internal int _index2;
        internal int _uWrap; 
        internal int _vWrap;
        internal int _minFltr;
        internal int _magFltr;
        internal float _float;
        internal int _unk10;
        internal bool _unk11;
        internal bool _unk12;
        internal int _unk13;
        
        public enum WrapMode
        {
            Clamp,
            Repeat,
            Mirror
        }

        #region Texture linkage
        internal MDL0TextureNode _texture;
        [Browsable(false)]
        public MDL0TextureNode TextureNode
        {
            get { return _texture; }
            set
            {
                if (_texture == value)
                    return;
                if (_texture != null)
                {
                    _texture._references.Remove(this);
                    if (_texture._references.Count == 0)
                        _texture.Remove();
                }
                if ((_texture = value) != null)
                {
                    _texture._references.Add(this);

                    Name = _texture.Name;

                    if (_texture.Source == null)
                        _texture.GetSource();

                    if (_texture.Source is TEX0Node && ((TEX0Node)_texture.Source).HasPalette)
                        PaletteNode = Model.FindOrCreatePalette(_texture.Name);
                    else
                        PaletteNode = null;
                }
                SignalPropertyChange();
            }
        }
        [Browsable(true), TypeConverter(typeof(DropDownListTextures))]
        public string Texture
        {
            get { return _texture == null ? null : _texture.Name; }
            set { TextureNode = String.IsNullOrEmpty(value) ? null : Model.FindOrCreateTexture(value); }
        }
        #endregion

        #region Palette linkage
        internal MDL0TextureNode _palette;
        [Browsable(false)]
        public MDL0TextureNode PaletteNode
        {
            get { return _palette; }
            set
            {
                if (_palette == value)
                    return;
                if (_palette != null)
                {
                    _palette._references.Remove(this);
                    if (_palette._references.Count == 0)
                        _palette.Remove();
                }
                if ((_palette = value) != null)
                    _palette._references.Add(this);
                SignalPropertyChange();
            }
        }
        [Browsable(true), TypeConverter(typeof(DropDownListTextures))]
        public string Palette
        {
            get { return _palette == null ? null : _palette.Name; }
            set { PaletteNode = String.IsNullOrEmpty(value) ? null : Model.FindOrCreatePalette(value); }
        }
        #endregion

        public override string Name
        {
            get { return _texture != null ? _texture.Name : base.Name; }
            set { if (_texture != null) Texture = value; base.Name = value; }
        }

        public enum TextureMinFilter : uint
        {
            Nearest = 0,
            Linear,
            Nearest_Mipmap_Nearest,
            Linear_Mipmap_Nearest,
            Nearest_Mipmap_Linear,
            Linear_Mipmap_Linear
        }

        public enum TextureMagFilter : uint
        {
            Nearest = 0,
            Linear,
        }

        //[Category("Texture Reference")]
        //public string Palette { get { return _palette == null ? null : _palette.Name; } }//set { _secondaryTexture = value; SignalPropertyChange(); } }
        [Category("Texture Reference")]
        public int Unknown1 { get { return _unk2; } set { if (!CheckIfMetal()) _unk2 = value; } }
        [Category("Texture Reference")]
        public int Unknown2 { get { return _unk3; } set { if (!CheckIfMetal()) _unk3 = value; } }
        [Category("Texture Reference")]
        public int Index1 { get { return _index1; } set { if (!CheckIfMetal()) _index1 = value; } }
        [Category("Texture Reference")]
        public int Index2 { get { return _index2; } set { if (!CheckIfMetal()) _index2 = value; } }
        [Category("Texture Reference")]
        public WrapMode UWrapMode { get { return (WrapMode)_uWrap; } set { if (!CheckIfMetal()) _uWrap = (int)value; } }
        [Category("Texture Reference")]
        public WrapMode VWrapMode { get { return (WrapMode)_vWrap; } set { if (!CheckIfMetal()) _vWrap = (int)value; } }
        [Category("Texture Reference")]
        public TextureMinFilter MinFilter { get { return (TextureMinFilter)_minFltr; } set { if (!CheckIfMetal()) _minFltr = (int)value; } }
        [Category("Texture Reference")]
        public TextureMagFilter MagFilter { get { return (TextureMagFilter)_magFltr; } set { if (!CheckIfMetal()) _magFltr = (int)value; } }
        [Category("Texture Reference")]
        public float LODBias { get { return _float; } set { if (!CheckIfMetal()) _float = value; } }
        [Category("Texture Reference")]
        public Anisotropy MaxAnisotropy { get { return (Anisotropy)_unk10; } set { if (!CheckIfMetal()) _unk10 = (int)value; } }
        [Category("Texture Reference")]
        public bool ClampBias { get { return _unk11; } set { if (!CheckIfMetal()) _unk11 = value; } }
        [Category("Texture Reference")]
        public bool TexelInterpolate { get { return _unk12; } set { if (!CheckIfMetal()) _unk12 = value; } }
        [Category("Texture Reference")]
        public int Unknown3 { get { return _unk13; } set { if (!CheckIfMetal()) _unk13 = value; } }
        
        public enum Anisotropy
        {
            One,//GX_ANISO_1, //No anisotropic filter.
            Two,//GX_ANISO_2, //Filters a maximum of two samples.
            Four//GX_ANISO_4  //Filters a maximum of four samples.
        }

        public bool CheckIfMetal()
        {
            if (Material != null && Material.CheckIfMetal())
                return true;

            SignalPropertyChange();
            return false;
        }

        protected override bool OnInitialize()
        {
            MDL0TextureRef* header = Header;

            _unk2 = header->_unk2;
            _unk3 = header->_unk3;
            _index1 = header->_index1;
            _index2 = header->_index2;
            _uWrap = header->_uWrap;
            _vWrap = header->_vWrap;
            _minFltr = header->_minFltr;
            _magFltr = header->_magFltr;
            _float = header->_lodBias;
            _unk10 = header->_unk10;
            _unk11 = header->_unk11 == 1;
            _unk12 = header->_unk12 == 1;
            _unk13 = header->_unk13;

            if (header->_stringOffset != 0)
            {
                if (!Material._replaced)
                    _name = header->ResourceString;
                else
                    _name = Material._name + "_Texture" + Index;

                _texture = Model.FindOrCreateTexture(_name);
                _texture._references.Add(this);
            }
            if (header->_secondaryOffset != 0)
            {
                string name;
                if (!Material._replaced)
                    name = header->SecondaryTexture;
                else
                    name = Material._name + "_Texture" + Index;

                _palette = Model.FindOrCreatePalette(name);
                _palette._references.Add(this);
            }

            if (((MDL0MaterialNode)Parent).XFCommands.Length != 0)
            {
                TexMtxFlags = new XFTexMtxInfo(((MDL0MaterialNode)Parent).XFCommands[Index * 2].values[0]);
                DualTexFlags = new XFDualTex(((MDL0MaterialNode)Parent).XFCommands[Index * 2 + 1].values[0]);
                getValues();
            }

            //if (PaletteNode == null && TextureNode != null)
            //{
            //    if (TextureNode.Source == null)
            //        TextureNode.GetSource();

            //    if (TextureNode.Source is TEX0Node && ((TEX0Node)TextureNode.Source).HasPalette)
            //    {
            //        Model._errors.Add("A palette was not set to texture reference " + Index + " in material " + Parent.Index + " (" + Parent.Name + ").");
            //        PaletteNode = Model.FindOrCreatePalette(TextureNode.Name);

            //        SignalPropertyChange();
            //    }
            //}

            MDL0MtlTexSettings* TexSettings = ((MDL0MaterialNode)Parent).Header->TexMatrices(Model._version);
            
            _texFlags = TexSettings->GetTexFlags(Index);
            _texMatrix = TexSettings->GetTexMatrices(Index);

            _flags = (TexFlags)((((MDL0MaterialNode)Parent)._layerFlags >> (4 * Index)) & 0xF);

            _bindState = new FrameState(
                new Vector3(_texFlags.TexScale._x, _texFlags.TexScale._y, 1),
                new Vector3(_texFlags.TexRotation, 0, 0),
                new Vector3(_texFlags.TexTranslation._x, _texFlags.TexTranslation._y, 0));

            return false;
        }

        internal override void GetStrings(StringTable table)
        {
            table.Add(Name);
            if (_palette != null)
                table.Add(_palette.Name);
        }

        protected internal override void OnRebuild(VoidPtr address, int length, bool force)
        {
            MDL0TextureRef* header = (MDL0TextureRef*)address;
            header->_unk2 = _unk2;
            header->_unk3 = _unk3;
            header->_index1 = _index1;
            header->_index2 = _index2;
            header->_uWrap = _uWrap;
            header->_vWrap = _vWrap;
            header->_minFltr = _minFltr;
            header->_magFltr = _magFltr;
            header->_unk10 = _unk10;
            header->_unk11 = (byte)(_unk11 ? 1 : 0);
            header->_unk12 = (byte)(_unk12 ? 1 : 0);
            header->_unk13 = (short)_unk13;
            header->_lodBias = _float;
        }

        protected internal override void PostProcess(VoidPtr mdlAddress, VoidPtr dataAddress, StringTable stringTable)
        {
            MDL0TextureRef* header = (MDL0TextureRef*)dataAddress;
            header->_stringOffset = (int)stringTable[Name] + 4 - (int)dataAddress;

            if (_palette != null)
                header->_secondaryOffset = (int)stringTable[_palette.Name] + 4 - (int)dataAddress;
            else
                header->_secondaryOffset = 0;

            header->_unk2 = _unk2;
            header->_unk3 = _unk3;
            header->_index1 = _index1;
            header->_index2 = _index2;
            header->_uWrap = _uWrap;
            header->_vWrap = _vWrap;
            header->_minFltr = _minFltr;
            header->_magFltr = _magFltr;
            header->_unk10 = _unk10;
            header->_unk11 = (byte)(_unk11 ? 1 : 0);
            header->_unk12 = (byte)(_unk12 ? 1 : 0);
            header->_unk13 = (short)_unk13;
            header->_lodBias = _float;
        }

        internal override void Bind(GLContext ctx)
        {
            if (_texture != null)
                _texture.Prepare(ctx, this);

            if (PAT0Texture != null)
            {
                if (!PAT0Textures.ContainsKey(PAT0Texture))
                    PAT0Textures[PAT0Texture] = new MDL0TextureNode(PAT0Texture) { Source = null, palette = PAT0Palette != null ? RootNode.FindChildByType(PAT0Palette, true, ResourceNodes.ResourceType.PLT0) as PLT0Node : null };
                PAT0Textures[PAT0Texture].Prepare(ctx, this);
            }
        }

        internal override void Unbind(GLContext ctx)
        {
            if (_texture != null && _texture._context != null)
                _texture.Unbind();
            
            foreach (MDL0TextureNode t in PAT0Textures.Values)
                if (t._context != null)
                    t.Unbind();
        }

        public FrameState _frameState, _bindState;
        internal void ApplySRT0Texture(SRT0TextureNode node, int index)
        {
            if ((node == null) || (index == 0)) //Reset to identity
                _frameState = new FrameState() { _scale = new Vector3(1) };
            else
                _frameState = new FrameState(node.GetAnimFrame(index - 1));
        }

        public Dictionary<string, MDL0TextureNode> PAT0Textures = new Dictionary<string, MDL0TextureNode>(); 
        public string PAT0Texture, PAT0Palette;
        internal void ApplyPAT0Texture(PAT0TextureNode node, int index)
        {
            PAT0TextureEntryNode prev = null;
            if (node != null && index != 0 && node.Children.Count > 0)
            {
                foreach (PAT0TextureEntryNode next in node.Children)
                {
                    if (next.Index == 0)
                    {
                        prev = next;
                        continue;
                    }
                    if (prev.key <= index - 1 && next.key > index - 1)
                        break;
                    prev = next;
                }

                PAT0Texture = prev.Texture;
                PAT0Palette = prev.Palette;
                if (!PAT0Textures.ContainsKey(PAT0Texture))
                {
                    TEX0Node texture = RootNode.FindChildByType(PAT0Texture, true, ResourceNodes.ResourceType.TEX0) as TEX0Node;
                    if (texture != null)
                        PAT0Textures[PAT0Texture] = new MDL0TextureNode(texture.Name) { Source = texture, palette = PAT0Palette != null ? RootNode.FindChildByType(PAT0Palette, true, ResourceNodes.ResourceType.PLT0) as PLT0Node : null };
                }
                return;
            }
            else PAT0Texture = PAT0Palette = null;
        }

        public void Default()
        {
            Name = "NewRef";
            Index1 = Index;
            Index2 = Index;
            _minFltr = 1;
            _magFltr = 1;
            UWrapMode = WrapMode.Repeat;
            VWrapMode = WrapMode.Repeat;

            _flags = (TexFlags)0xF;
            _texFlags.TexScale = new Vector2(1);
            _bindState._scale = new Vector3(1);
            _texMatrix.TexMtx = Matrix43.Identity;
            _texMatrix.TexUnk1 = -1;
            _texMatrix.TexUnk2 = -1;
            _texMatrix.TexUnk4 = 1;

            _projection = (int)TexProjection.ST;
            _inputForm = (int)TexInputForm.AB11;
            _texGenType = (int)TexTexgenType.Regular;
            _sourceRow = (int)TexSourceRow.TexCoord0;
            _embossSource = 4;
            _embossLight = 2;

            getTexMtxVal();

            _texture = Model.FindOrCreateTexture(_name);
            _texture._references.Add(this);
        }

        public override void Remove()
        {
            if (_parent != null && !CheckIfMetal())
            {
                TextureNode = null;
                PaletteNode = null;
                base.Remove();
            }
        }

        public override bool MoveUp()
        {
            if (Parent == null)
                return false;

            if (CheckIfMetal())
                return false;

            int index = Index - 1;
            if (index < 0)
                return false;

            Parent.Children.Remove(this);
            Parent.Children.Insert(index, this);
            Parent._changed = true;

            _index1 = _index2 = index;

            return true;
        }

        public override bool MoveDown()
        {
            if (Parent == null)
                return false;

            if (CheckIfMetal())
                return false;

            int index = Index + 1;
            if (index >= Parent.Children.Count)
                return false;

            Parent.Children.Remove(this);
            Parent.Children.Insert(index, this);
            Parent._changed = true;

            _index1 = _index2 = index;

            return true;
        }
    }
}
