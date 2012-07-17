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
using BrawlLib.Wii.Graphics;
using BrawlLib.Wii.Models;
using System.Windows.Forms;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class MDL0MaterialNode : MDL0EntryNode
    {
        internal MDL0Material* Header { get { return (MDL0Material*)WorkingUncompressed.Address; } }

        public override ResourceType ResourceType { get { return ResourceType.MDL0Material; } }

        public MDL0PolygonNode[] Objects { get { if (!isMetal) return _polygons.ToArray(); else return MetalMaterial != null ? MetalMaterial._polygons.ToArray() : null; } }
        internal List<MDL0PolygonNode> _polygons = new List<MDL0PolygonNode>();

        MatModeBlock* mode;

        public string[] Part2Entries { get { return _part2Entries.ToArray(); } set { _part2Entries = value.ToList<string>(); SignalPropertyChange(); } }
        internal List<string> _part2Entries = new List<string>();

        internal int _dataLen;
        internal int _index;
        internal int _matRefOffset = 1044;
        internal int _part2Offset = 0;
        internal byte _numTextures;
        internal byte _numLights;
        internal int _unk2, _unk3, _unk4;
        internal uint _isXLU;
        public byte _ssc;
        internal byte _clip;
        public byte _transp;
        internal sbyte _lSet;
        internal sbyte _fSet;
        internal byte _unk1;
        internal CullMode _cull;

        //In order of appearance in display list:
        //Mode block
        internal AlphaFunction _alphaFunc = AlphaFunction.Default;
        internal ZMode _zMode = ZMode.Default;
        //Mask, does not allow changing the dither/update bits
        internal BlendMode _blendMode = BlendMode.Default;
        internal ConstantAlpha _constantAlpha = ConstantAlpha.Default;
        //Tev Color Block
        internal MatTevColorBlock _tevColorBlock = MatTevColorBlock.Default;
        //Pad 4
        //Tev Konstant Block
        internal MatTevKonstBlock _tevKonstBlock = MatTevKonstBlock.Default;
        //Pad 24
        //Indirect texture scale for CMD stages
        //XF Texture matrix info

        internal List<XFData> XFCmds = new List<XFData>();
        public XFData[] XFCommands { get { return XFCmds.ToArray(); } }

        [Category("TEV Konstant Block"), TypeConverter(typeof(GXColorS10StringConverter))]
        public GXColorS10 KReg0Color { get { return new GXColorS10() { R = _tevKonstBlock.TevReg0Lo.A, A = _tevKonstBlock.TevReg0Lo.B, B = _tevKonstBlock.TevReg0Hi.A, G = _tevKonstBlock.TevReg0Hi.B }; } set { if (!CheckIfMetal()) { _tevKonstBlock.TevReg0Lo.A = value.R; _tevKonstBlock.TevReg0Lo.B = value.A; _tevKonstBlock.TevReg0Hi.A = value.B; _tevKonstBlock.TevReg0Hi.B = value.G; } } }
        [Category("TEV Konstant Block"), TypeConverter(typeof(GXColorS10StringConverter))]
        public GXColorS10 KReg1Color { get { return new GXColorS10() { R = _tevKonstBlock.TevReg1Lo.A, A = _tevKonstBlock.TevReg1Lo.B, B = _tevKonstBlock.TevReg1Hi.A, G = _tevKonstBlock.TevReg1Hi.B }; } set { if (!CheckIfMetal()) { _tevKonstBlock.TevReg1Lo.A = value.R; _tevKonstBlock.TevReg1Lo.B = value.A; _tevKonstBlock.TevReg1Hi.A = value.B; _tevKonstBlock.TevReg1Hi.B = value.G; } } }
        [Category("TEV Konstant Block"), TypeConverter(typeof(GXColorS10StringConverter))]
        public GXColorS10 KReg2Color { get { return new GXColorS10() { R = _tevKonstBlock.TevReg2Lo.A, A = _tevKonstBlock.TevReg2Lo.B, B = _tevKonstBlock.TevReg2Hi.A, G = _tevKonstBlock.TevReg2Hi.B }; } set { if (!CheckIfMetal()) { _tevKonstBlock.TevReg2Lo.A = value.R; _tevKonstBlock.TevReg2Lo.B = value.A; _tevKonstBlock.TevReg2Hi.A = value.B; _tevKonstBlock.TevReg2Hi.B = value.G; } } }
        [Category("TEV Konstant Block"), TypeConverter(typeof(GXColorS10StringConverter))]
        public GXColorS10 KReg3Color { get { return new GXColorS10() { R = _tevKonstBlock.TevReg3Lo.A, A = _tevKonstBlock.TevReg3Lo.B, B = _tevKonstBlock.TevReg3Hi.A, G = _tevKonstBlock.TevReg3Hi.B }; } set { if (!CheckIfMetal()) { _tevKonstBlock.TevReg3Lo.A = value.R; _tevKonstBlock.TevReg3Lo.B = value.A; _tevKonstBlock.TevReg3Hi.A = value.B; _tevKonstBlock.TevReg3Hi.B = value.G; } } }

        [Category("TEV Color Block"), TypeConverter(typeof(GXColorS10StringConverter))]
        public GXColorS10 CReg0Color 
        { 
            get { return new GXColorS10() { R = _tevColorBlock.TevReg1Lo.A, A = _tevColorBlock.TevReg1Lo.B, B = _tevColorBlock.TevReg1Hi0.A, G = _tevColorBlock.TevReg1Hi0.B }; }
            set
            {
                if (!CheckIfMetal())
                {
                    _tevColorBlock.TevReg1Lo.A = value.R;
                    _tevColorBlock.TevReg1Lo.B = value.A;

                    //Hi values are always the same...
                    _tevColorBlock.TevReg1Hi0.A =
                    _tevColorBlock.TevReg1Hi1.A =
                    _tevColorBlock.TevReg1Hi2.A = value.B;
                    _tevColorBlock.TevReg1Hi0.B =
                    _tevColorBlock.TevReg1Hi1.B =
                    _tevColorBlock.TevReg1Hi2.B = value.G;
                }
            } 
        }
        [Category("TEV Color Block"), TypeConverter(typeof(GXColorS10StringConverter))]
        public GXColorS10 CReg1Color 
        { 
            get { return new GXColorS10() { R = _tevColorBlock.TevReg2Lo.A, A = _tevColorBlock.TevReg2Lo.B, B = _tevColorBlock.TevReg2Hi0.A, G = _tevColorBlock.TevReg2Hi0.B }; }
            set
            {
                if (!CheckIfMetal())
                {
                    _tevColorBlock.TevReg2Lo.A = value.R;
                    _tevColorBlock.TevReg2Lo.B = value.A;

                    //Hi values are always the same...
                    _tevColorBlock.TevReg2Hi0.A =
                    _tevColorBlock.TevReg2Hi1.A =
                    _tevColorBlock.TevReg2Hi2.A = value.B;
                    _tevColorBlock.TevReg2Hi0.B =
                    _tevColorBlock.TevReg2Hi1.B =
                    _tevColorBlock.TevReg2Hi2.B = value.G;
                }
            } 
        }
        [Category("TEV Color Block"), TypeConverter(typeof(GXColorS10StringConverter))]
        public GXColorS10 CReg2Color 
        { 
            get { return new GXColorS10() { R = _tevColorBlock.TevReg3Lo.A, A = _tevColorBlock.TevReg3Lo.B, B = _tevColorBlock.TevReg3Hi0.A, G = _tevColorBlock.TevReg3Hi0.B }; } 
            set 
            { 
                if (!CheckIfMetal()) 
                { 
                    _tevColorBlock.TevReg3Lo.A = value.R; 
                    _tevColorBlock.TevReg3Lo.B = value.A; 

                    //Hi values are always the same...
                    _tevColorBlock.TevReg3Hi0.A =
                    _tevColorBlock.TevReg3Hi1.A =
                    _tevColorBlock.TevReg3Hi2.A = value.B; 
                    _tevColorBlock.TevReg3Hi0.B =
                    _tevColorBlock.TevReg3Hi1.B =
                    _tevColorBlock.TevReg3Hi2.B = value.G; 
                } 
            } 
        }

        //[Category("TEV Color Block")]
        //public ColorReg Reg1Lo { get { return _tevColorBlock.TevReg1Lo; } }
        //[Category("TEV Color Block")]
        //public ColorReg Reg1Hi0 { get { return _tevColorBlock.TevReg1Hi0; } }
        //[Category("TEV Color Block")]
        //public ColorReg Reg1Hi1 { get { return _tevColorBlock.TevReg1Hi1; } }
        //[Category("TEV Color Block")]
        //public ColorReg Reg1Hi2 { get { return _tevColorBlock.TevReg1Hi2; } }

        //[Category("TEV Color Block")]
        //public ColorReg Reg2Lo { get { return _tevColorBlock.TevReg2Lo; } }
        //[Category("TEV Color Block")]
        //public ColorReg Reg2Hi0 { get { return _tevColorBlock.TevReg2Hi0; } }
        //[Category("TEV Color Block")]
        //public ColorReg Reg2Hi1 { get { return _tevColorBlock.TevReg2Hi1; } }
        //[Category("TEV Color Block")]
        //public ColorReg Reg2Hi2 { get { return _tevColorBlock.TevReg2Hi2; } }

        //[Category("TEV Color Block")]
        //public ColorReg Reg3Lo { get { return _tevColorBlock.TevReg3Lo; } }
        //[Category("TEV Color Block")]
        //public ColorReg Reg3Hi0 { get { return _tevColorBlock.TevReg3Hi0; } }
        //[Category("TEV Color Block")]
        //public ColorReg Reg3Hi1 { get { return _tevColorBlock.TevReg3Hi1; } }
        //[Category("TEV Color Block")]
        //public ColorReg Reg3Hi2 { get { return _tevColorBlock.TevReg3Hi2; } }
        
        #region Shader linkage
        internal MDL0ShaderNode _shader;
        [Browsable(false)]
        public MDL0ShaderNode ShaderNode
        {
            get { return _shader; }
            set
            {
                if (_shader == value)
                    return;
                if (_shader != null)
                    _shader._materials.Remove(this);
                if ((_shader = value) != null)
                    _shader._materials.Add(this);
                if (_shader != null)
                    ActiveShaderStages = _shader.stages;
            }
        }
        [Browsable(true), TypeConverter(typeof(DropDownListShaders))]
        public string Shader
        {
            get { return _shader == null ? null : _shader._name; }
            set
            {
                if (CheckIfMetal())
                    return;

                if (String.IsNullOrEmpty(value))
                    ShaderNode = null;
                else
                {
                    MDL0ShaderNode node = Model.FindChild(String.Format("Shaders/{0}", value), false) as MDL0ShaderNode;
                    if (node != null)
                        ShaderNode = node;
                }
            }
        }
        #endregion

        [Category("Alpha Function")]
        public byte Ref0 { get { return _alphaFunc.ref0; } set { if (!CheckIfMetal()) _alphaFunc.ref0 = value; } }
        [Category("Alpha Function")]
        public AlphaCompare Comp0 { get { return _alphaFunc.Comp0; } set { if (!CheckIfMetal()) _alphaFunc.Comp0 = value;  } }
        [Category("Alpha Function")]
        public AlphaOp Logic { get { return _alphaFunc.Logic; } set { if (!CheckIfMetal()) _alphaFunc.Logic = value;  } }
        [Category("Alpha Function")]
        public byte Ref1 { get { return _alphaFunc.ref1; } set { if (!CheckIfMetal()) _alphaFunc.ref1 = value;  } }
        [Category("Alpha Function")]
        public AlphaCompare Comp1 { get { return _alphaFunc.Comp1; } set { if (!CheckIfMetal()) _alphaFunc.Comp1 = value;  } }

        [Category("Z Mode")]
        public bool EnableDepthTest { get { return _zMode.EnableDepthTest; } set { if (!CheckIfMetal()) _zMode.EnableDepthTest = value;  } }
        [Category("Z Mode")]
        public bool EnableDepthUpdate { get { return _zMode.EnableDepthUpdate; } set { if (!CheckIfMetal()) _zMode.EnableDepthUpdate = value;  } }
        [Category("Z Mode")]
        public GXCompare DepthFunction { get { return _zMode.DepthFunction; } set { if (!CheckIfMetal()) _zMode.DepthFunction = value;  } }

        [Category("Blend Mode")] //Allows textures to be opaque. Cannot be used with Alpha Function
        public bool EnableBlend { get { return _blendMode.EnableBlend; } set { if (!CheckIfMetal()) { _blendMode.EnableBlend = value; _isXLU = value ? 0x80000000 : 0; } } }
        [Category("Blend Mode")]
        public bool EnableBlendLogic { get { return _blendMode.EnableLogicOp; } set { if (!CheckIfMetal()) _blendMode.EnableLogicOp = value;  } }
        
        //These are disabled via mask
        //[Category("Blend Mode")]
        //public bool EnableDither { get { return _blendMode.EnableDither; } }
        //[Category("Blend Mode")]
        //public bool EnableColorUpdate { get { return _blendMode.EnableColorUpdate; } }
        //[Category("Blend Mode")]
        //public bool EnableAlphaUpdate { get { return _blendMode.EnableAlphaUpdate; } }

        [Category("Blend Mode")]
        public BlendFactor SrcFactor { get { return _blendMode.SrcFactor; } set { if (!CheckIfMetal()) _blendMode.SrcFactor = value;  } }
        [Category("Blend Mode")]
        public GXLogicOp BlendLogicOp { get { return _blendMode.LogicOp; } set { if (!CheckIfMetal()) _blendMode.LogicOp = value;  } }
        [Category("Blend Mode")]
        public BlendFactor DstFactor { get { return _blendMode.DstFactor; } set { if (!CheckIfMetal()) _blendMode.DstFactor = value;  } }

        [Category("Blend Mode")]
        public bool Subtract { get { return _blendMode.Subtract; } set { if (!CheckIfMetal()) _blendMode.Subtract = value;  } }

        [Category("Constant Alpha")]
        public bool Enabled { get { return _constantAlpha.Enable != 0; } set { if (!CheckIfMetal()) _constantAlpha.Enable = (byte)(value ? 1 : 0); } }
        [Category("Constant Alpha")]
        public byte Value { get { return _constantAlpha.Value; } set { if (!CheckIfMetal()) _constantAlpha.Value = value; } }

        [Category("Material")]
        public int TotalLen { get { return _dataLen; } }
        [Category("Material")]
        public int MDL0Offset { get { return Header->_mdl0Offset; } }
        [Category("Material")]
        public int StringOffset { get { return Header->_stringOffset; } }
        [Category("Material")]
        public int ID { get { return _index; } }

        [Category("Indirect Texture Scale"), Browsable(true)]
        public IndTexScale IndTexStg0_S_Scale { get { return (IndTexScale)_tevKonstBlock.SS0val.S_Scale0; } set { if (!CheckIfMetal()) _tevKonstBlock.SS0val.S_Scale0 = value; } }
        [Category("Indirect Texture Scale"), Browsable(true)]
        public IndTexScale IndTexStg0_T_Scale { get { return (IndTexScale)_tevKonstBlock.SS0val.T_Scale0; } set { if (!CheckIfMetal()) _tevKonstBlock.SS0val.T_Scale0 = value; } }
        [Category("Indirect Texture Scale"), Browsable(true)]
        public IndTexScale IndTexStg1_S_Scale { get { return (IndTexScale)_tevKonstBlock.SS0val.S_Scale1; } set { if (!CheckIfMetal()) _tevKonstBlock.SS0val.S_Scale1 = value; } }
        [Category("Indirect Texture Scale"), Browsable(true)]
        public IndTexScale IndTexStg1_T_Scale { get { return (IndTexScale)_tevKonstBlock.SS0val.T_Scale1; } set { if (!CheckIfMetal()) _tevKonstBlock.SS0val.T_Scale1 = value; } }
        
        [Category("Indirect Texture Scale"), Browsable(true)]
        public IndTexScale IndTexStg2_S_Scale { get { return (IndTexScale)_tevKonstBlock.SS1val.S_Scale0; } set { if (!CheckIfMetal()) _tevKonstBlock.SS1val.S_Scale0 = value; } }
        [Category("Indirect Texture Scale"), Browsable(true)]
        public IndTexScale IndTexStg2_T_Scale { get { return (IndTexScale)_tevKonstBlock.SS1val.T_Scale0; } set { if (!CheckIfMetal()) _tevKonstBlock.SS1val.T_Scale0 = value; } }
        [Category("Indirect Texture Scale"), Browsable(true)]
        public IndTexScale IndTexStg3_S_Scale { get { return (IndTexScale)_tevKonstBlock.SS1val.S_Scale1; } set { if (!CheckIfMetal()) _tevKonstBlock.SS1val.S_Scale1 = value; } }
        [Category("Indirect Texture Scale"), Browsable(true)]
        public IndTexScale IndTexStg3_T_Scale { get { return (IndTexScale)_tevKonstBlock.SS1val.T_Scale1; } set { if (!CheckIfMetal()) _tevKonstBlock.SS1val.T_Scale1 = value; } }
        
        //Usage flags. Each set of 4 bits represents one texture layer.
        [Category("Texture Flags")]
        public string LayerFlags { get { return _layerFlags.ToString("X"); } }//set { if (!CheckIfMetal()) _layerFlags = UInt32.Parse(value, System.Globalization.NumberStyles.HexNumber);  } }
        public uint _layerFlags;
        [Category("Texture Flags")]
        public string UnkFlags { get { return _unkFlags.ToString("X"); } set { if (!CheckIfMetal()) _unkFlags = UInt32.Parse(value, System.Globalization.NumberStyles.HexNumber);  } }
        public uint _unkFlags;
        
        [Category("Lighting")]
        public uint Flags0 { get { return flags0; } set { if (!CheckIfMetal()) flags0 = value; } }
        public uint flags0;
        [Category("Lighting"), TypeConverter(typeof(RGBAStringConverter))]
        public RGBAPixel C0Color0 { get { return c00; } set { if (!CheckIfMetal()) c00 = value; } }
        public RGBAPixel c00;
        [Category("Lighting"), TypeConverter(typeof(RGBAStringConverter))]
        public RGBAPixel C0Color1 { get { return c01; } set { if (!CheckIfMetal()) c01 = value; } }
        public RGBAPixel c01;
        [Category("Lighting")]
        public short Pad0 { get { return Header->Light(Model._version)->pad0; } }
        [Category("Lighting")]
        public short Pad1 { get { return Header->Light(Model._version)->pad1; } }
        [Category("Lighting")]
        public byte C0Enums0 { get { return e00; } set { if (!CheckIfMetal()) e00 = value; } }
        [Category("Lighting")]
        public byte C0Enums1 { get { return e01; } set { if (!CheckIfMetal()) e01 = value; } }
        [Category("Lighting")]
        public byte C0Enums2 { get { return e02; } set { if (!CheckIfMetal()) e02 = value; } }
        [Category("Lighting")]
        public byte C0Enums3 { get { return e03; } set { if (!CheckIfMetal()) e03 = value; } }
        public byte e00, e01, e02, e03;
        
        [Category("Lighting")]
        public uint Flags1 { get { return flags1; } set { if (!CheckIfMetal()) flags1 = value; } }
        public uint flags1;
        [Category("Lighting"), TypeConverter(typeof(RGBAStringConverter))]
        public RGBAPixel C1Color0 { get { return c10; } set { if (!CheckIfMetal()) c10 = value; } }
        public RGBAPixel c10;
        [Category("Lighting"), TypeConverter(typeof(RGBAStringConverter))]
        public RGBAPixel C1Color1 { get { return c11; } set { if (!CheckIfMetal()) c11 = value; } }
        public RGBAPixel c11;
        [Category("Lighting")]
        public short Pad2 { get { return Header->Light(Model._version)->pad2; } }
        [Category("Lighting")]
        public short Pad3 { get { return Header->Light(Model._version)->pad3; } }
        [Category("Lighting")]
        public byte C1Enums0 { get { return e10; } set { if (!CheckIfMetal()) e10 = value; } }
        [Category("Lighting")]
        public byte C1Enums1 { get { return e11; } set { if (!CheckIfMetal()) e11 = value; } }
        [Category("Lighting")]
        public byte C1Enums2 { get { return e12; } set { if (!CheckIfMetal()) e12 = value; } }
        [Category("Lighting")]
        public byte C1Enums3 { get { return e13; } set { if (!CheckIfMetal()) e13 = value; } }
        public byte e10, e11, e12, e13;
        
        //[Category("Material")]
        //public string Unknown1 { get { return _isXLU.ToString("X"); } set { if (!CheckIfMetal()) _isXLU = UInt32.Parse(value, System.Globalization.NumberStyles.HexNumber); } }
        [Category("Material")]
        public bool XLUMaterial { get { return _isXLU == 0x80000000; } set { if (!CheckIfMetal()) { _isXLU = value ? 0x80000000 : 0; if (value == false) _blendMode.EnableBlend = false; } } }
        
        [Category("Material")]
        public byte Texgens { get { return _numTextures; } }//set { if (!CheckIfMetal()) _numTextures = value;  } }
        [Category("Material")]
        public byte LightChannels { get { return _numLights; } set { if (!CheckIfMetal()) _numLights = (value > 2 ? (byte)2 : value < 0 ? (byte)0 : value); } }
        [Category("Material")]
        public byte ActiveShaderStages { get { return _ssc; } set { if (!CheckIfMetal()) _ssc = (value > ShaderNode.stages ? (byte)ShaderNode.stages : value < 1 ? (byte)1 : value); } }
        [Category("Material")]
        public byte IndirectTextures { get { return _clip; } set { if (!CheckIfMetal()) _clip = (value > 4 ? (byte)4 : value < 0 ? (byte)0 : value); } }
        [Category("Material")]
        public CullMode CullMode { get { return _cull; } set { if (!CheckIfMetal()) _cull = value;  } }
        [Category("Material")]
        public bool EnableAlphaFunction { get { return _transp != 1; } set { if (!CheckIfMetal()) _transp = (byte)(value ? 0 : 1); } }
        [Category("Material")]
        public sbyte LightSet { get { return _lSet; } set { if (!CheckIfMetal()) { _lSet = value; if (MetalMaterial != null) MetalMaterial.UpdateAsMetal(); } } }
        [Category("Material")]
        public sbyte FogSet { get { return _fSet; } set { if (!CheckIfMetal()) { _fSet = value; if (MetalMaterial != null) MetalMaterial.UpdateAsMetal(); } } }
        [Category("Material")]
        public byte Unknown1 { get { return _unk1; } }//set { if (!CheckIfMetal()) { _unk1 = value; if (MetalMaterial != null) MetalMaterial.UpdateAsMetal(); } } }
        
        [Category("Material")]
        public int Unknown2 { get { return _unk2; } }
        [Category("Material")]
        public int Unknown3 { get { return _unk3; } }

        [Category("Material")]
        public int ShaderOffset { get { return Header->_shaderOffset; } }
        
        [Category("Material")]
        public int NumTextures { get { return Header->_numTextures; } }
        [Category("Material")]
        public int MaterialRefOffset { get { return _matRefOffset; } }
        [Category("Material")]
        public int Part2Offset { get { return _part2Offset; } }
        [Category("Material")]
        public int DisplayListOffset_8_9 { get { return Header->_dlOffset_08_09; } }
        [Category("Material")]
        public int DisplayListOffset_10_11 { get { return Header->_dlOffset_10_11; } }

        public void Render(GLContext ctx)
        {
            #region LayerRendering

            ////Write struct variables
            //shader += "struct VS_OUTPUT {\n";
            //shader += "  float4 pos : POSITION;\n";
            //shader += "  float4 colors_0 : COLOR0;\n";
            //shader += "  float4 colors_1 : COLOR1;\n";

            ////if (xfregs.numTexGen.numTexGens < 7) {
            ////    for (unsigned int i = 0; i < xfregs.numTexGen.numTexGens; ++i)
            ////        WRITE(p, "  float3 tex%d : TEXCOORD%d;\n", i, i);
            ////    WRITE(p, "  float4 clipPos : TEXCOORD%d;\n", xfregs.numTexGen.numTexGens);
            ////    if(g_ActiveConfig.bEnablePixelLighting && g_ActiveConfig.backend_info.bSupportsPixelLighting)
            ////        WRITE(p, "  float4 Normal : TEXCOORD%d;\n", xfregs.numTexGen.numTexGens + 1);
            ////} else {
            ////    // clip position is in w of first 4 texcoords
            ////    if(g_ActiveConfig.bEnablePixelLighting && g_ActiveConfig.backend_info.bSupportsPixelLighting)
            ////    {
            ////        for (int i = 0; i < 8; ++i)
            ////            WRITE(p, "  float4 tex%d : TEXCOORD%d;\n", i, i);
            ////    }
            ////    else
            ////    {
            ////        for (unsigned int i = 0; i < xfregs.numTexGen.numTexGens; ++i)
            ////            WRITE(p, "  float%d tex%d : TEXCOORD%d;\n", i < 4 ? 4 : 3 , i, i);
            ////    }
            ////}	
            ////WRITE(p, "};\n");

            ////Write code
            //for (int i = 0; i < m.Children.Count; i++)
            //{
            //    MDL0MaterialRefNode mr = m.Children[i] as MDL0MaterialRefNode;
            //    XFTexMtxInfo texinfo = mr.TexMtxFlags;

            //    shader += "{\n";
            //    shader += "coord = float4(0.0f, 0.0f, 1.0f, 1.0f);\n";
            //    switch (texinfo.SourceRow)
            //    {
            //        case TexSourceRow.Geometry:
            //            if (texinfo.InputForm == TexInputForm.ABC1)
            //                shader += "coord = rawpos;\n"; // pos.w is 1
            //            break;
            //        case TexSourceRow.Normals:
            //            //if (components & VB_HAS_NRM0) 
            //            //{
            //            if (texinfo.InputForm == TexInputForm.ABC1)
            //                shader += "coord = float4(rawnorm0.xyz, 1.0f);\n";
            //            //}
            //            break;
            //        case TexSourceRow.Colors:
            //            if (texinfo.TexGenType == TexTexgenType.Color0 || texinfo.TexGenType == TexTexgenType.Color1) ;
            //            break;
            //        case TexSourceRow.BinormalsT:
            //            //if (components & VB_HAS_NRM1) 
            //            //{
            //            if (texinfo.InputForm == TexInputForm.ABC1)
            //                shader += "coord = float4(rawnorm1.xyz, 1.0f);\n";
            //            //}
            //            break;
            //        case TexSourceRow.BinormalsB:
            //            //if (components & VB_HAS_NRM2) 
            //            //{
            //            if (texinfo.InputForm == TexInputForm.ABC1)
            //                shader += "coord = float4(rawnorm2.xyz, 1.0f);\n";
            //            //}
            //            break;
            //        default:
            //            if (texinfo.SourceRow <= TexSourceRow.TexCoord7)
            //                //if (components & (VB_HAS_UV0 << (texinfo.SourceRow - TexSourceRow.TexCoord0)))
            //                shader += String.Format("coord = float4(tex{0}.x, tex{0}.y, 1.0f, 1.0f);\n", texinfo.SourceRow - TexSourceRow.TexCoord0);
            //            break;
            //    }

            //    // first transformation
            //    switch (texinfo.TexGenType)
            //    {
            //        case TexTexgenType.EmbossMap: //Calculate tex coords into bump map

            //            //No BT support yet
            //            //if (components & (VB_HAS_NRM1|VB_HAS_NRM2))
            //            //{
            //            // transform the light dir into tangent space
            //            //shader += "ldir = normalize("I_LIGHTS".lights[%d].pos.xyz - pos.xyz);\n", texinfo.embosslightshift);
            //            //shader += "o.tex%d.xyz = o.tex%d.xyz + float3(dot(ldir, _norm1), dot(ldir, _norm2), 0.0f);\n", i, texinfo.embosssourceshift);
            //            //}
            //            //else
            //            //{
            //            //if (0); // should have normals
            //            shader += String.Format("o.tex{0}.xyz = o.tex{1}.xyz;\n", i, texinfo.EmbossSource);
            //            //}

            //            break;
            //        case TexTexgenType.Color0:
            //            if (texinfo.SourceRow == TexSourceRow.Colors)
            //                shader += String.Format("o.tex{0}.xyz = float3(o.colors_0.x, o.colors_0.y, 1);\n", i);
            //            break;
            //        case TexTexgenType.Color1:
            //            if (texinfo.SourceRow == TexSourceRow.Colors) ;
            //            shader += String.Format("o.tex{0}.xyz = float3(o.colors_1.x, o.colors_1.y, 1);\n", i);
            //            break;
            //        case TexTexgenType.Regular:
            //        default:
            //            //if (components & (VB_HAS_TEXMTXIDX0 << i)) 
            //            {
            //                if (texinfo.Projection == TexProjection.STQ)
            //                    shader += String.Format("o.tex{0}.xyz = float3(dot(coord, " + I_TRANSFORMMATRICES + ".T[tex{0}.z].t), dot(coord, " + I_TRANSFORMMATRICES + ".T[tex{0}.z+1].t), dot(coord, " + I_TRANSFORMMATRICES + ".T[tex{0}.z+2].t));\n", i);
            //                else
            //                    shader += String.Format("o.tex{0}.xyz = float3(dot(coord, " + I_TRANSFORMMATRICES + ".T[tex{0}.z].t), dot(coord, " + I_TRANSFORMMATRICES + ".T[tex{0}.z+1].t), 1);\n", i);
            //            }
            //            //else 
            //            //{
            //            //    if (texinfo.Projection == TexProjection.STQ)
            //            //        shader += String.Format("o.tex%d.xyz = float3(dot(coord, "+I_TEXMATRICES+".T[%d].t), dot(coord, "+I_TEXMATRICES+".T[%d].t), dot(coord, "+I_TEXMATRICES+".T[%d].t));\n", i, 3*i, 3*i+1, 3*i+2);
            //            //    else
            //            //        shader += String.Format("o.tex%d.xyz = float3(dot(coord, "+I_TEXMATRICES+".T[%d].t), dot(coord, "+I_TEXMATRICES+".T[%d].t), 1);\n", i, 3*i, 3*i+1);
            //            //}
            //            break;
            //    }

            //    //if (mr.DualTexFlags.NormalEnable == 1 && texinfo.TexGenType == TexTexgenType.Regular) { // only works for regular tex gen types?
            //    //    const PostMtxInfo& postInfo = xfregs.postMtxInfo[i];

            //    //    int postidx = postInfo.index;
            //    //    shader += "float4 P0 = "I_POSTTRANSFORMMATRICES".T[%d].t;\n"
            //    //        "float4 P1 = "I_POSTTRANSFORMMATRICES".T[%d].t;\n"
            //    //        "float4 P2 = "I_POSTTRANSFORMMATRICES".T[%d].t;\n",
            //    //        postidx&0x3f, (postidx+1)&0x3f, (postidx+2)&0x3f);

            //    //    //if (texGenSpecialCase) {
            //    //    //    // no normalization
            //    //    //    // q of input is 1
            //    //    //    // q of output is unknown

            //    //    //    // multiply by postmatrix
            //    //    //    shader += "o.tex%d.xyz = float3(dot(P0.xy, o.tex%d.xy) + P0.z + P0.w, dot(P1.xy, o.tex%d.xy) + P1.z + P1.w, 0.0f);\n", i, i, i);
            //    //    }
            //    //    else
            //    //    {
            //    //        if (postInfo.normalize)
            //    //            shader += "o.tex%d.xyz = normalize(o.tex%d.xyz);\n", i, i);

            //    //        // multiply by postmatrix
            //    //        shader += "o.tex%d.xyz = float3(dot(P0.xyz, o.tex%d.xyz) + P0.w, dot(P1.xyz, o.tex%d.xyz) + P1.w, dot(P2.xyz, o.tex%d.xyz) + P2.w);\n", i, i, i, i);
            //    //    }
            //    //}

            //    shader += "}\n";
            //}

            #endregion

            //if (!ShaderNode.rendered)
                //ShaderNode.Render(ctx, this);
        }

        public bool updating = false;
        public void UpdateAsMetal()
        {
            if (!isMetal)
                return;

            updating = true;
            if (ShaderNode != null && ShaderNode._autoMetal && ShaderNode.texCount == Children.Count)
            { 
                //ShaderNode.DefaultAsMetal(Children.Count); 
            }
            else
            {
                bool found = false;
                foreach (MDL0ShaderNode s in Model._shadGroup.Children)
                {
                    if (s._autoMetal && s.texCount == Children.Count)
                    {
                        ShaderNode = s;
                        found = true;
                    }
                    else
                    {
                        if (s.stages == 4)
                        {
                            foreach (MDL0MaterialNode y in s._materials)
                                if (!y.isMetal || y.Children.Count != Children.Count)
                                    goto NotFound;
                            ShaderNode = s;
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
                    Model._shadGroup.AddChild(shader);
                    ShaderNode = shader;
                    shader.DefaultAsMetal(Children.Count);
                }
            }

            if (MetalMaterial != null)
            {
                Name = MetalMaterial.Name + "_ExtMtl";
                _ssc = 4;

                if (Children.Count - 1 != MetalMaterial.Children.Count)
                {
                    //Remove all children
                    for (int i = 0; i < Children.Count; i++)
                    {
                        ((MDL0MaterialRefNode)Children[i]).TextureNode = null;
                        ((MDL0MaterialRefNode)Children[i]).PaletteNode = null;
                        RemoveChild(Children[i--]);
                    }

                    //Start over
                    for (int i = 0; i <= MetalMaterial.Children.Count; i++)
                    {
                        MDL0MaterialRefNode mr = new MDL0MaterialRefNode();

                        AddChild(mr);
                        mr.Texture = "metal00";
                        mr._index1 = mr._index2 = i;

                        mr._texFlags.TexScale = new Vector2(1);
                        mr._bindState._scale = new Vector3(1);
                        mr._texMatrix.TexMtx = Matrix43.Identity;
                        mr._texMatrix.TexUnk1 = -1;
                        mr._texMatrix.TexUnk2 = -1;
                        mr._texMatrix.TexUnk4 = 1;

                        if (i == MetalMaterial.Children.Count)
                        {
                            mr._minFltr = 5;
                            mr._magFltr = 1;
                            mr._float = -2;
                            mr.HasTextureMatrix = true;
                            mr._projection = (int)TexProjection.STQ;
                            mr._inputForm = (int)TexInputForm.ABC1;
                            mr._sourceRow = (int)TexSourceRow.Normals;
                            mr.Normalize = true;
                            mr.TexUnk3 = 1;
                        }
                        else
                        {
                            mr._projection = (int)TexProjection.ST;
                            mr._inputForm = (int)TexInputForm.AB11;
                            mr._sourceRow = (int)TexSourceRow.TexCoord0 + i;
                            mr.Normalize = false;
                            mr.TexUnk3 = 0;
                        }

                        mr._texGenType = (int)TexTexgenType.Regular;
                        mr._embossSource = 4;
                        mr._embossLight = 2;

                        mr.getTexMtxVal();
                    }

                    flags0 = 63;
                    c00.R = c00.G = c00.B = 128; c00.A = 255;
                    c01.R = c01.G = c01.B = c01.A = 255;
                    e01 = e03 = 2; 
                    e00 = e02 = 7;
                    flags1 = 63;
                    c10.R = c10.G = c10.B = c10.A = 255;
                    e10 = e11 = e12 = 2;

                    _lSet = MetalMaterial._lSet;
                    _fSet = MetalMaterial._fSet;
                    _unk1 = MetalMaterial._unk1;

                    _cull = MetalMaterial._cull;
                    _numLights = 2;
                    EnableAlphaFunction = false;
                    _unk3 = -1;

                    SignalPropertyChange();
                }
            }
            updating = false;
        }

        public bool CheckIfMetal()
        {
            if (Model._autoMetal)
            {
                if (!updating)
                {
                    if (isMetal)
                        if (MessageBox.Show(null, "This model is currently set to automatically modify metal materials.\nYou cannot make changes unless you turn it off.\nDo you want to turn it off?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            Model._autoMetal = false;
                        else
                            return true;
                }
            }

            SignalPropertyChange();
            return false;
        }

        [Browsable(false)]
        public bool isMetal { get { return Name.EndsWith("_ExtMtl"); } }

        [Browsable(false)]
        public MDL0MaterialNode MetalMaterial
        {
            get
            {
                foreach (MDL0MaterialNode t in Model._matList)
                {
                    if (!isMetal)
                    {
                        if (t.Name.StartsWith(Name) && t.isMetal)
                            return t;
                    }
                    else if (Name.StartsWith(t.Name) && !t.isMetal) return t;
                }
                return null;
            }
        }

        protected override bool OnInitialize()
        {
            MDL0Material* header = Header;

            if ((_name == null) && (header->_stringOffset != 0))
                _name = header->ResourceString;
            
            XFCmds.Clear();

            //Get XF Commands
            byte* pData = (byte*)header->DisplayLists(Model._version) + 0xE0;
        Top:
            if (*pData++ == 0x10)
            {
                XFData dat = new XFData();
                int count = (ushort)*(bushort*)pData; pData += 2;
                dat.addr = (XFMemoryAddr)(ushort)*(bushort*)pData; pData += 2;
                dat.values = new List<uint>();
                for (int i = 0; i < count + 1; i++)
                { dat.values.Add(*(buint*)pData); pData += 4; }
                XFCmds.Add(dat);
                goto Top;
            }

            _dataLen = header->_dataLen;
            _index = header->_index;
            _numTextures = header->_numTexGens;
            _numLights = header->_numLightChans;
            _isXLU = header->_isXLU;
            _unk2 = header->_unk2;
            _unk3 = header->_unk3;
            _unk4 = header->_dlOffset_10_11;
            _ssc = header->_activeTEVStages;
            _clip = header->_numIndTexStages;
            _transp = header->_enableAlphaTest;
            _lSet = header->_lightSet;
            _fSet = header->_fogSet;
            _unk1 = header->_unk1;
            _cull = (CullMode)(int)header->_cull;

            if ((-header->_mdl0Offset + (int)header->DisplayListOffset(Model._version)) % 0x20 != 0)
            {
                Model._errors.Add("Material " + Index + " has an improper align offset.");
                SignalPropertyChange();
            }

            _matRefOffset = header->_matRefOffset;
            _part2Offset = header->_part2Offset;

            mode = header->DisplayLists(Model._version);
            _alphaFunc = mode->AlphaFunction;
            _zMode = mode->ZMode;
            _blendMode = mode->BlendMode;
            _constantAlpha = mode->ConstantAlpha;

            _tevColorBlock = *header->TevColorBlock(Model._version);
            _tevKonstBlock = *header->TevKonstBlock(Model._version);

            MDL0MtlTexSettings* TexMatrices = header->TexMatrices(Model._version);

            _layerFlags = TexMatrices->LayerFlags;
            _unkFlags = TexMatrices->UnkFlags;

            MDL0MaterialLighting* Light = header->Light(Model._version);

            c00 = Light->c00;
            c01 = Light->c01;
            flags0 = Light->flags0;
            e00 = Light->unk00;
            e01 = Light->unk01;
            e02 = Light->unk02;
            e03 = Light->unk03;
            
            c10 = Light->c10;
            c11 = Light->c11;
            flags1 = Light->flags1;
            e10 = Light->unk10;
            e11 = Light->unk11;
            e12 = Light->unk12;
            e13 = Light->unk13;

            Part2Data* part2 = header->Part2;
            if (part2 != null)
            {
                ResourceGroup* group = part2->Group;
                for (int i = 0; i < group->_numEntries; i++)
                    _part2Entries.Add(group->First[i].GetName());
            }

            Populate();
            return true;
        }

        protected override void OnPopulate()
        {
            MDL0TextureRef* first = Header->First;
            for (int i = 0; i < Header->_numTextures; i++)
                new MDL0MaterialRefNode().Initialize(this, first++, MDL0TextureRef.Size);
        }

        internal override void GetStrings(StringTable table)
        {
            table.Add(Name);

            foreach (string s in _part2Entries)
                table.Add(s);

            foreach (MDL0MaterialRefNode n in Children)
                n.GetStrings(table);
        }

        public int _dataAlign = 0, _mdlOffset = 0;
        protected override int OnCalculateSize(bool force)
        {
            int temp, size;

            //Add header and tex matrices size at start
            if (Model._version == 11 || Model._version == 10)
                size = 0x418;
            else
                size = 0x414;

            //Add children size
            size += Children.Count * MDL0TextureRef.Size;

            //Add part 2 entries, if there are any
            if (_part2Entries.Count > 0)
                size += 0x1C + _part2Entries.Count * 0x2C;
            
            temp = size; //Set temp align offset

            //Align data to an offset divisible by 0x20 using data length.
            size = size.Align(0x10) + _dataAlign;
            if ((size + _mdlOffset) % 0x20 != 0)
                if (size - 0x10 >= temp)
                    size -= 0x10;
                else
                    size += 0x10;

            //Reset data alignment
            _dataAlign = 0;

            //Add display list and XF flags
            size += 0x180;

            return size;
        }

        public bool New = false;
        protected internal override void OnRebuild(VoidPtr address, int length, bool force)
        {
            MDL0Material* header = (MDL0Material*)address;

            ushort i1 = 0x1040, i2 = 0x1050; int mtx = 0;

            //Set offsets
            header->_dataLen = _dataLen = length;

            if (Model._version == 11 || Model._version == 10)
            {
                header->_dlOffset_08_09 = 0;
                header->_dlOffset_10_11 = length - 0x180;
                if (Children.Count > 0)
                    header->_matRefOffset = 1048;
                else
                    header->_matRefOffset = 0;
            }
            else
            {
                header->_dlOffset_08_09 = length - 0x180;
                header->_dlOffset_10_11 = 0;
                if (Children.Count > 0)
                    header->_matRefOffset = 1044;
                else
                    header->_matRefOffset = 0;
            }

            //Check for part2 entries
            if (_part2Entries.Count > 0)
            {
                header->_part2Offset = _part2Offset = header->_matRefOffset + Children.Count * 0x34;
                Part2Data* part2 = header->Part2;
                if (part2 != null)
                {
                    part2->_totalLen = 0x1C + _part2Entries.Count * 0x2C;
                    ResourceGroup* pGroup = part2->Group;
                    *pGroup = new ResourceGroup(_part2Entries.Count);
                    ResourceEntry* pEntry = &pGroup->_first + 1;
                    byte* pData = (byte*)pGroup + pGroup->_totalSize;
                    foreach (string s in _part2Entries)
                    {
                        (pEntry++)->_dataOffset = (int)pData - (int)pGroup;
                        Part2DataEntry* p = (Part2DataEntry*)pData;
                        *p = new Part2DataEntry(1);
                        pData += 0x1C;
                    }
                }
            }
            else
                _part2Offset = header->_part2Offset = 0;

            //Set defaults if the model is an import or the material was created
            if (Model._isImport || New)
            {
                if (Model._importOptions._mdlType == 0 || New)
                {
                    _lSet = 20;
                    _fSet = 4;
                    _unk3 = -1;
                    _ssc = 3;

                    _cull = CullMode.Cull_Inside;
                    _numLights = 1;

                    flags0 = 63;
                    c00.R = c00.G = c00.B = c00.A = 255;
                    c01.R = c01.G = c01.B = c01.A = 255;
                    e01 = e03 = 3;
                    e00 = e02 = 7;
                    flags1 = 15; c10.A = 255;
                }
                else
                {
                    _lSet = 1;
                    _fSet = 0;
                    _unk3 = -1;
                    _ssc = 1;
                    _cull = CullMode.Cull_Inside;
                    _numLights = 1;

                    flags0 = 63;
                    c00.R = c00.G = c00.B = c00.A = 255;
                    c01.R = c01.G = c01.B = c01.A = 255;
                    e01 = 3; e03 = 1;
                    e00 = e02 = 7;
                    flags1 = 15; c10.A = 255;
                }

                //Set default texgen flags
                for (int i = 0; i < Children.Count; i++)
                {
                    MDL0MaterialRefNode node = ((MDL0MaterialRefNode)Children[i]);

                    //Tex Mtx
                    XFData dat = new XFData();
                    dat.addr = (XFMemoryAddr)i1++;
                    XFTexMtxInfo tex = new XFTexMtxInfo();
                    tex._data = (uint)(0 | 
                        ((int)TexProjection.ST << 1) |
                        ((int)TexInputForm.AB11 << 2) |
                        ((int)TexTexgenType.Regular << 4) |
                        ((int)(0x5) << 7) |
                        (4 << 10) | 
                        (2 << 13));
                    dat.values.Add(tex._data); 
                    XFCmds.Add(dat);
                    node.TexMtxFlags = tex;

                    //Dual Tex
                    dat = new XFData();
                    dat.addr = (XFMemoryAddr)i2++;
                    XFDualTex dtex = new XFDualTex(mtx, 0); mtx += 3;
                    dat.values.Add(dtex.Value);
                    XFCmds.Add(dat);
                    node.DualTexFlags = dtex;
                    node.getValues();
                    node._texFlags.TexScale = new Vector2(1);
                    node._bindState._scale = new Vector3(1);
                    node._texMatrix.TexMtx = Matrix43.Identity;
                    node._texMatrix.TexUnk1 = -1;
                    node._texMatrix.TexUnk2 = -1;
                    node._texMatrix.TexUnk3 = 0;
                    node._texMatrix.TexUnk4 = 1;
                }
            }

            //Set header values
            header->_numTextures = Children.Count;
            header->_numTexGens = _numTextures = (byte)Children.Count;
            header->_index = _index = Index;
            header->_numLightChans = _numLights;
            header->_activeTEVStages = (byte)_ssc;
            header->_numIndTexStages = _clip;
            header->_enableAlphaTest = _transp;
            header->_lightSet = _lSet;
            header->_fogSet = _fSet;
            header->_unk1 = _unk1;
            header->_cull = (int)_cull;
            header->_isXLU = _isXLU;
            header->_unk2 = _unk2;
            header->_unk3 = _unk3; //Always -1?

            //Generate layer flags and write texture matrices
            MDL0MtlTexSettings* TexSettings = header->TexMatrices(Model._version);
            *TexSettings = MDL0MtlTexSettings.Default;

            _layerFlags = 0;
            for (int i = Children.Count - 1; i >= 0; i--)
            {
                MDL0MaterialRefNode node = (MDL0MaterialRefNode)Children[i];

                node._flags |= TexFlags.Enabled;

                node._texFlags.TexScale = new Vector2(node._bindState._scale._x, node._bindState._scale._y);
                node._texFlags.TexRotation = node._bindState._rotate._x;
                node._texFlags.TexTranslation = new Vector2(node._bindState._translate._x, node._bindState._translate._y);

                //Check for non-default values
                if (node._texFlags.TexScale != new Vector2(1))
                    node._flags &= 0xF - TexFlags.FixedScale;
                else
                    node._flags |= TexFlags.FixedScale;

                if (node._texFlags.TexRotation != 0)
                    node._flags &= 0xF - TexFlags.FixedRot;
                else
                    node._flags |= TexFlags.FixedRot;

                if (node._texFlags.TexTranslation != new Vector2(0))
                    node._flags &= 0xF - TexFlags.FixedTrans;
                else
                    node._flags |= TexFlags.FixedTrans;

                TexSettings->SetTexFlags(node._texFlags, node.Index);
                TexSettings->SetTexMatrices(node._texMatrix, node.Index);

                _layerFlags = ((_layerFlags << 4) | (byte)node._flags);
            }

            TexSettings->LayerFlags = _layerFlags;
            TexSettings->UnkFlags = _unkFlags;

            //Write lighting flags
            MDL0MaterialLighting* Light = header->Light(Model._version);

            Light->c00 = c00;
            Light->c01 = c01;
            Light->flags0 = flags0;
            Light->unk00 = e00;
            Light->unk01 = e01;
            Light->unk02 = e02;
            Light->unk03 = e03;

            Light->c10 = c10;
            Light->c11 = c11;
            Light->flags1 = flags1;
            Light->unk10 = e10;
            Light->unk11 = e11;
            Light->unk12 = e12;
            Light->unk13 = e13;

            //The shader offset will be written later

            //Rebuild references
            MDL0TextureRef* mRefs = header->First;
            foreach (MDL0MaterialRefNode n in Children)
                n.Rebuild(mRefs++, 0x34, force);
            
            //Set Display Lists
            *header->TevKonstBlock(Model._version) = _tevKonstBlock;
            *header->TevColorBlock(Model._version) = _tevColorBlock;

            mode = header->DisplayLists(Model._version);
            *mode = MatModeBlock.Default;
            if (Model._isImport)
            {
                _alphaFunc = mode->AlphaFunction;
                _zMode = mode->ZMode;
                _blendMode = mode->BlendMode;
                _constantAlpha = mode->ConstantAlpha;
            }
            else
            {
                mode->AlphaFunction = _alphaFunc;
                mode->ZMode = _zMode;
                mode->BlendMode = _blendMode;
                mode->ConstantAlpha = _constantAlpha;
            }

            //Write XF flags
            byte* xfData = (byte*)header->DisplayLists(Model._version) + 0xE0;
            i1 = 0x1040; i2 = 0x1050; mtx = 0;
            foreach (MDL0MaterialRefNode mr in Children)
            {
                //Tex Mtx
                *xfData++ = 0x10;
                *(bushort*)xfData = 0; xfData += 2;
                *(bushort*)xfData = (ushort)i1++;  xfData += 2;
                *(buint*)xfData = mr.TexMtxFlags._data; xfData += 4;

                //Dual Tex
                *xfData++ = 0x10;
                *(bushort*)xfData = 0; xfData += 2;
                *(bushort*)xfData = (ushort)i2++; xfData += 2;
                *(buint*)xfData = new XFDualTex(mtx, mr.DualTexFlags.NormalEnable).Value;
                mtx += 3; xfData += 4;
            }
            
            New = false;
        }

        protected internal override void PostProcess(VoidPtr mdlAddress, VoidPtr dataAddress, StringTable stringTable)
        {
            MDL0Material* header = (MDL0Material*)dataAddress;
            header->_mdl0Offset = (int)mdlAddress - (int)dataAddress;
            header->_stringOffset = (int)stringTable[Name] + 4 - (int)dataAddress;
            header->_index = Index;

            Part2Data* part2 = header->Part2;
            if (part2 != null && _part2Entries.Count != 0)
            {
                ResourceGroup* group = part2->Group;
                group->_first = new ResourceEntry(0xFFFF, 0, 0, 0, 0);
                ResourceEntry* rEntry = group->First;

                for (int i = 0, x = 1; i < group->_numEntries; i++)
                {
                    Part2DataEntry* entry = (Part2DataEntry*)((int)group + (rEntry++)->_dataOffset);
                    ResourceEntry.Build(group, x++, entry, (BRESString*)stringTable[_part2Entries[i]]);
                    entry->ResourceStringAddress = stringTable[_part2Entries[i]] + 4;
                }
            }

            MDL0TextureRef* first = header->First;
            foreach (MDL0MaterialRefNode n in Children)
                n.PostProcess(mdlAddress, first++, stringTable);
        }
        public override void Remove()
        {
            ShaderNode = null;
            base.Remove();
        }
        internal override void Bind(GLContext ctx) 
        {
            //Polygons will bind the mat refs

            //foreach (MDL0MaterialRefNode m in Children)
            //    m.Bind(ctx);
        }
        internal override void Unbind(GLContext ctx) 
        {
            foreach (MDL0MaterialRefNode m in Children) 
                m.Unbind(ctx); 
        }

        internal void ApplySRT0(SRT0Node node, int index)
        {
            SRT0EntryNode e;

            if (node == null || index == 0)
                foreach (MDL0MaterialRefNode r in Children)
                    r.ApplySRT0Texture(null, 0);
            else if ((e = node.FindChild(Name, false) as SRT0EntryNode) != null)
            {
                foreach (SRT0TextureNode t in e.Children)
                    if (t._textureIndex < Children.Count)
                        ((MDL0MaterialRefNode)Children[t._textureIndex]).ApplySRT0Texture(t, index);
            }
            else
                foreach (MDL0MaterialRefNode r in Children)
                    r.ApplySRT0Texture(null, 0);
        }

        internal unsafe void ApplyPAT0(PAT0Node node, int index)
        {
            PAT0EntryNode e;

            if (node == null || index == 0)
                foreach (MDL0MaterialRefNode r in Children)
                    r.ApplyPAT0Texture(null, 0);
            else if ((e = node.FindChild(Name, false) as PAT0EntryNode) != null)
            {
                foreach (PAT0TextureNode t in e.Children)
                    if (t._textureIndex < Children.Count)
                        ((MDL0MaterialRefNode)Children[t._textureIndex]).ApplyPAT0Texture(t, index);
            }
            else
                foreach (MDL0MaterialRefNode r in Children)
                    r.ApplyPAT0Texture(null, 0);
        }

        public override void RemoveChild(ResourceNode child)
        {
            base.RemoveChild(child);

            if (!updating && Model._autoMetal && MetalMaterial != null && !this.isMetal)
                MetalMaterial.UpdateAsMetal();
        }
    }
}
