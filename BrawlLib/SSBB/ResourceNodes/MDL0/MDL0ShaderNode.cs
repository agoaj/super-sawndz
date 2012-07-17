using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.SSBBTypes;
using System.ComponentModel;
using BrawlLib.Wii.Graphics;
using BrawlLib.Imaging;
using BrawlLib.OpenGL;
using System.Windows.Forms;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class MDL0ShaderNode : MDL0EntryNode
    {
        internal MDL0Shader* Header { get { return (MDL0Shader*)WorkingUncompressed.Address; } }

        public override ResourceType ResourceType { get { return ResourceType.MDL0Shader; } }

        //Konstant Alpha Selection Swap table
        KSelSwapBlock _swapBlock = KSelSwapBlock.Default;

        [Category("Swap Mode Table"), Browsable(true)]
        public ColorChannel Swap0Red { get { return (ColorChannel)_swapBlock._Value01.XRB; } set { _swapBlock._Value01.XRB = (int)value; SignalPropertyChange(); } }
        [Category("Swap Mode Table"), Browsable(true)]
        public ColorChannel Swap0Green { get { return (ColorChannel)_swapBlock._Value01.XGA; } set { _swapBlock._Value01.XGA = (int)value; SignalPropertyChange(); } }

        [Category("Swap Mode Table"), Browsable(true)]
        public ColorChannel Swap0Blue { get { return (ColorChannel)_swapBlock._Value03.XRB; } set { _swapBlock._Value03.XRB = (int)value; SignalPropertyChange(); } }
        [Category("Swap Mode Table"), Browsable(true)]
        public ColorChannel Swap0Alpha { get { return (ColorChannel)_swapBlock._Value03.XGA; } set { _swapBlock._Value03.XGA = (int)value; SignalPropertyChange(); } }

        [Category("Swap Mode Table"), Browsable(true)]
        public ColorChannel Swap1Red { get { return (ColorChannel)_swapBlock._Value05.XRB; } set { _swapBlock._Value05.XRB = (int)value; SignalPropertyChange(); } }
        [Category("Swap Mode Table"), Browsable(true)]
        public ColorChannel Swap1Green { get { return (ColorChannel)_swapBlock._Value05.XGA; } set { _swapBlock._Value05.XGA = (int)value; SignalPropertyChange(); } }

        [Category("Swap Mode Table"), Browsable(true)]
        public ColorChannel Swap1Blue { get { return (ColorChannel)_swapBlock._Value07.XRB; } set { _swapBlock._Value07.XRB = (int)value; SignalPropertyChange(); } }
        [Category("Swap Mode Table"), Browsable(true)]
        public ColorChannel Swap1Alpha { get { return (ColorChannel)_swapBlock._Value07.XGA; } set { _swapBlock._Value07.XGA = (int)value; SignalPropertyChange(); } }

        [Category("Swap Mode Table"), Browsable(true)]
        public ColorChannel Swap2Red { get { return (ColorChannel)_swapBlock._Value09.XRB; } set { _swapBlock._Value09.XRB = (int)value; SignalPropertyChange(); } }
        [Category("Swap Mode Table"), Browsable(true)]
        public ColorChannel Swap2Green { get { return (ColorChannel)_swapBlock._Value09.XGA; } set { _swapBlock._Value09.XGA = (int)value; SignalPropertyChange(); } }

        [Category("Swap Mode Table"), Browsable(true)]
        public ColorChannel Swap2Blue { get { return (ColorChannel)_swapBlock._Value11.XRB; } set { _swapBlock._Value11.XRB = (int)value; SignalPropertyChange(); } }
        [Category("Swap Mode Table"), Browsable(true)]
        public ColorChannel Swap2Alpha { get { return (ColorChannel)_swapBlock._Value11.XGA; } set { _swapBlock._Value11.XGA = (int)value; SignalPropertyChange(); } }

        [Category("Swap Mode Table"), Browsable(true)]
        public ColorChannel Swap3Red { get { return (ColorChannel)_swapBlock._Value13.XRB; } set { _swapBlock._Value13.XRB = (int)value; SignalPropertyChange(); } }
        [Category("Swap Mode Table"), Browsable(true)]
        public ColorChannel Swap3Green { get { return (ColorChannel)_swapBlock._Value13.XGA; } set { _swapBlock._Value13.XGA = (int)value; SignalPropertyChange(); } }

        [Category("Swap Mode Table"), Browsable(true)]
        public ColorChannel Swap3Blue { get { return (ColorChannel)_swapBlock._Value15.XRB; } set { _swapBlock._Value15.XRB = (int)value; SignalPropertyChange(); } }
        [Category("Swap Mode Table"), Browsable(true)]
        public ColorChannel Swap3Alpha { get { return (ColorChannel)_swapBlock._Value15.XGA; } set { _swapBlock._Value15.XGA = (int)value; SignalPropertyChange(); } }

        //Used by Alpha Env to retrieve what values to swap
        public string[] swapModeTable = new string[4];

        private void BuildSwapModeTable()
        {
	        string swapColors = "rgba";

            //Iterate through the swaps
	        for (int i = 0; i < 4; i++)
	        {
                switch (i)
                {
                    case 0:
                        swapModeTable[i] = new string(new char[] {
                        swapColors[(int)Swap0Red],
                        swapColors[(int)Swap0Green],
                        swapColors[(int)Swap0Blue],
                        swapColors[(int)Swap0Alpha]});
                        break;
                    case 1:
                        swapModeTable[i] = new string(new char[] {
                        swapColors[(int)Swap1Red],
                        swapColors[(int)Swap1Green],
                        swapColors[(int)Swap1Blue],
                        swapColors[(int)Swap1Alpha]});
                        break;
                    case 2:
                        swapModeTable[i] = new string(new char[] {
                        swapColors[(int)Swap2Red],
                        swapColors[(int)Swap2Green],
                        swapColors[(int)Swap2Blue],
                        swapColors[(int)Swap2Alpha]});
                        break;
                    case 3:
                        swapModeTable[i] = new string(new char[] {
                        swapColors[(int)Swap3Red],
                        swapColors[(int)Swap3Green],
                        swapColors[(int)Swap3Blue],
                        swapColors[(int)Swap3Alpha]});
                        break;
                }
	        }
        }

        [Category("TEV RAS1 IRef"), Browsable(true)]
        public TexMapID IndTex0MapID { get { return (TexMapID)bi0; } set { bi0 = (int)value; getRawIRef(); } }
        [Category("TEV RAS1 IRef"), Browsable(true)]
        public TexCoordID IndTex0Coord { get { return (TexCoordID)bc0; } set { bc0 = (int)value; getRawIRef(); } }
        [Category("TEV RAS1 IRef"), Browsable(true)]
        public TexMapID IndTex1MapID { get { return (TexMapID)bi1; } set { bi1 = (int)value; getRawIRef(); } }
        [Category("TEV RAS1 IRef"), Browsable(true)]
        public TexCoordID IndTex1Coord { get { return (TexCoordID)bc1; } set { bc1 = (int)value; getRawIRef(); } }
        [Category("TEV RAS1 IRef"), Browsable(true)]
        public TexMapID IndTex2MapID { get { return (TexMapID)bi2; } set { bi2 = (int)value; getRawIRef(); } }
        [Category("TEV RAS1 IRef"), Browsable(true)]
        public TexCoordID IndTex2Coord { get { return (TexCoordID)bc2; } set { bc2 = (int)value; getRawIRef(); } }
        [Category("TEV RAS1 IRef"), Browsable(true)]
        public TexMapID IndTex3MapID { get { return (TexMapID)bi3; } set { bi3 = (int)value; getRawIRef(); } }
        [Category("TEV RAS1 IRef"), Browsable(true)]
        public TexCoordID IndTex3Coord { get { return (TexCoordID)bc3; } set { bc3 = (int)value; getRawIRef(); } }

        public int bc0, bi0, bc1, bi1, bc2, bi2, bc3, bi3;

        private void getRawIRef()
        {
            _swapBlock._Value16 = (Int24)RAS1_IRef.Shift(bi0, bc0, bi1, bc1, bi2, bc2, bi3, bc3);
            SignalPropertyChange();
        }
        public void getIRefValues()
        {
            RAS1_IRef _rawIRef = new RAS1_IRef(_swapBlock._Value16);
            bi0 = _rawIRef.TexMap0;
            bc0 = _rawIRef.TexCoord0;
            bi1 = _rawIRef.TexMap1;
            bc1 = _rawIRef.TexCoord1;
            bi2 = _rawIRef.TexMap2;
            bc2 = _rawIRef.TexCoord2;
            bi3 = _rawIRef.TexMap3;
            bc3 = _rawIRef.TexCoord3;
        }

        public MDL0MaterialNode[] Materials { get { return _materials.ToArray(); } }
        public List<MDL0MaterialNode> _materials = new List<MDL0MaterialNode>();

        public sbyte ref0, ref1, ref2, ref3, ref4, ref5, ref6, ref7;
        public byte stages, res0, res1, res2;
        int _datalen, _mdl0offset, pad0, pad1;

        [Category("Shader Data"), Browsable(true)]
        public int DataLength { get { return _datalen; } }
        [Category("Shader Data"), Browsable(true)]
        public int MDL0Offset { get { return _mdl0offset; } }
        
        [Category("Shader Data"), Browsable(true)]
        public byte Stages { get { return stages; } } //Max 16 (2 stages per group - 8 groups)
        [Browsable(false)]
        public byte STGs 
        { 
            get { return stages; } 
            set 
            { 
                stages = value; 
                SignalPropertyChange();

                foreach (MDL0MaterialNode m in Materials)
                {
                    m.updating = true;
                    m.ActiveShaderStages = value;
                    m.updating = false;
                }
            } 
        }
        
        [Category("Shader Data"), Browsable(true)]
        public byte Res0 { get { return res0; } set { res0 = value; SignalPropertyChange(); } }
        [Category("Shader Data"), Browsable(true)]
        public byte Res1 { get { return res1; } set { res1 = value; SignalPropertyChange(); } }
        [Category("Shader Data"), Browsable(true)]
        public byte Res2 { get { return res2; } set { res2 = value; SignalPropertyChange(); } }

        [Category("Shader Data"), Browsable(true)]
        public bool TextureRef0 { get { return ref0 != -1; } set { ref0 = (sbyte)(value ? 0 : -1); SignalPropertyChange(); } }
        [Category("Shader Data"), Browsable(true)]
        public bool TextureRef1 { get { return ref1 != -1; } set { ref1 = (sbyte)(value ? 1 : -1); SignalPropertyChange(); } }
        [Category("Shader Data"), Browsable(true)]
        public bool TextureRef2 { get { return ref2 != -1; } set { ref2 = (sbyte)(value ? 2 : -1); SignalPropertyChange(); } }
        [Category("Shader Data"), Browsable(true)]
        public bool TextureRef3 { get { return ref3 != -1; } set { ref3 = (sbyte)(value ? 3 : -1); SignalPropertyChange(); } }
        [Category("Shader Data"), Browsable(true)]
        public bool TextureRef4 { get { return ref4 != -1; } set { ref4 = (sbyte)(value ? 4 : -1); SignalPropertyChange(); } }
        [Category("Shader Data"), Browsable(true)]
        public bool TextureRef5 { get { return ref5 != -1; } set { ref5 = (sbyte)(value ? 5 : -1); SignalPropertyChange(); } }
        [Category("Shader Data"), Browsable(true)]
        public bool TextureRef6 { get { return ref6 != -1; } set { ref6 = (sbyte)(value ? 6 : -1); SignalPropertyChange(); } }
        [Category("Shader Data"), Browsable(true)]
        public bool TextureRef7 { get { return ref7 != -1; } set { ref7 = (sbyte)(value ? 7 : -1); SignalPropertyChange(); } }

        [Category("Shader Data"), Browsable(true)]
        public int Pad0 { get { return pad0; } }
        [Category("Shader Data"), Browsable(true)]
        public int Pad1 { get { return pad1; } }

        #region Rendering

        private bool _renderChange = true;
        public string shader;
        public bool _enabled = true;
        internal GLContext _context;
        internal uint _id;
        private void CreateGLSLShader(MDL0MaterialNode m)
        {
            //Read stages and create GLSL C++ code string
            //Color and Alpha are generated seperately

            BuildSwapModeTable();
            int numStages = Children.Count;
            int numTexgens = m.Children.Count;

            shader = "//Shader" + Index + "\n";

            shader += "uniform sampler2D";

            bool first = true;
            for (int i = 0; i < m.Children.Count; i++)
            {
                shader += String.Format("{0} samp{1}", first ? "" : ",", i);
                first = false;
            }
            shader += ";\n";

            shader += String.Format("\n");

	        shader += String.Format("uniform float4 "+I_COLORS+"[4] : register(c{0});\n", C_COLORS);
	        shader += String.Format("uniform float4 "+I_KCOLORS+"[4] : register(c{0});\n", C_KCOLORS);
	        shader += String.Format("uniform float4 "+I_ALPHA+"[1] : register(c{0});\n", C_ALPHA);
	        shader += String.Format("uniform float4 "+I_TEXDIMS+"[8] : register(c{0});\n", C_TEXDIMS);
	        shader += String.Format("uniform float4 "+I_ZBIAS+"[2] : register(c{0});\n", C_ZBIAS);
	        shader += String.Format("uniform float4 "+I_INDTEXSCALE+"[2] : register(c{0});\n", C_INDTEXSCALE);
	        shader += String.Format("uniform float4 "+I_INDTEXMTX+"[6] : register(c{0});\n", C_INDTEXMTX);
	        shader += String.Format("uniform float4 "+I_FOG+"[3] : register(c{0});\n", C_FOG);

            shader += "\n";

            //No lighting for now

            //// shader variables
            //string I_POSNORMALMATRIX      = "cpnmtx";
            //string I_PROJECTION           = "cproj";
            //string I_MATERIALS            = "cmtrl";
            //string I_LIGHTS               = "clights";
            //string I_TEXMATRICES          = "ctexmtx";
            //string I_TRANSFORMMATRICES    = "ctrmtx";
            //string I_NORMALMATRICES       = "cnmtx";
            //string I_POSTTRANSFORMMATRICES= "cpostmtx";
            //string I_DEPTHPARAMS          = "cDepth";

            //int C_POSNORMALMATRIX         =  0;
            //int C_PROJECTION              = (C_POSNORMALMATRIX + 6);
            //int C_MATERIALS               = (C_PROJECTION + 4);
            //int C_LIGHTS                  = (C_MATERIALS + 4);
            //int C_TEXMATRICES             = (C_LIGHTS + 40);
            //int C_TRANSFORMMATRICES       = (C_TEXMATRICES + 24);
            //int C_NORMALMATRICES          = (C_TRANSFORMMATRICES + 64);
            //int C_POSTTRANSFORMMATRICES   = (C_NORMALMATRICES + 32);
            //int C_DEPTHPARAMS             = (C_POSTTRANSFORMMATRICES + 64);
            //int C_VENVCONST_END		      = (C_DEPTHPARAMS + 4);

            shader += String.Format(
            "float4 c0 = " + I_COLORS + "[1],\n" +
            "  c1 = " + I_COLORS + "[2],\n" +
            "  c2 = " + I_COLORS + "[3],\n" +
            "  prev = float4(0.0f, 0.0f, 0.0f, 0.0f),\n" +
            "  textemp = float4(0.0f, 0.0f, 0.0f, 0.0f),\n" +
            "  rastemp = float4(0.0f, 0.0f, 0.0f, 0.0f),\n" +
            "  konsttemp = float4(0.0f, 0.0f, 0.0f, 0.0f);\n" + 
			"float3 comp16 = float3(1.0f, 255.0f, 0.0f),\n" +
            "  comp24 = float3(1.0f, 255.0f, 255.0f*255.0f);\n" + 
			"float4 alphabump=float4(0.0f,0.0f,0.0f,0.0f);\n" + 
			"float3 tevcoord=float3(0.0f, 0.0f, 0.0f);\n" + 
			"float2 wrappedcoord=float2(0.0f,0.0f),\n" +
            "  tempcoord=float2(0.0f,0.0f);\n" + 
			"float4 cc0=float4(0.0f,0.0f,0.0f,0.0f),\n" + 
            "  cc1=float4(0.0f,0.0f,0.0f,0.0f);\n" +
            "float4 cc2=float4(0.0f,0.0f,0.0f,0.0f),\n" + 
            "  cprev=float4(0.0f,0.0f,0.0f,0.0f);\n" + 
			"float4 crastemp=float4(0.0f,0.0f,0.0f,0.0f),\n" + 
            "  ckonsttemp=float4(0.0f,0.0f,0.0f,0.0f);\n\n");

            shader += String.Format("void main(\n");
            shader += "void";
            //shader += String.Format("  out float4 ocol0 : COLOR0,\n  in float4 rawpos : WPOS,\n");

            //    // compute window position if needed because binding semantic WPOS is not widely supported
            //if (m.Children.Count < 7)
            //{
            //    for (int i = 0; i < m.Children.Count; ++i)
            //        shader += String.Format(",\n  in float3 uv{0} : TEXCOORD{0}", i);
            //    shader += String.Format(",\n  in float4 clipPos : TEXCOORD{0}", m.Children.Count);
            //    //if(g_ActiveConfig.bEnablePixelLighting && g_ActiveConfig.backend_info.bSupportsPixelLighting)
            //        shader += String.Format(",\n  in float4 Normal : TEXCOORD{0}", m.Children.Count + 1);
            //}
            //else
            //{
            //    // wpos is in w of first 4 texcoords
            //    //if(g_ActiveConfig.bEnablePixelLighting && g_ActiveConfig.backend_info.bSupportsPixelLighting)
            //    //{
            //        for (int i = 0; i < 8; ++i)
            //            shader += String.Format(",\n  in float4 uv{0} : TEXCOORD{0}", i);
            //    //}
            //    //else
            //    //{
            //    //    for (unsigned int i = 0; i < xfregs.numTexGen.numTexGens; ++i)
            //    //        shader += String.Format(",\n  in float%d uv%d : TEXCOORD%d", i < 4 ? 4 : 3 , i, i);
            //    //}
            //}
	        shader += ") {\n";

            //Get how many stages should we apply
            int active = m.ActiveShaderStages > stages ? stages : m.ActiveShaderStages;

            //Write stages to shader code
            foreach (TEVStage s in Children)
                if (s.Index < active)
                    shader += s.Write(m);

            shader += String.Format("prev.rgb = {0};\n", tevCOutputTable[(int)((TEVStage)Children[active - 1]).ColorRegister]);
            shader += String.Format("prev.a = {0};\n", tevAOutputTable[(int)((TEVStage)Children[active - 1]).AlphaRegister]);

            //Is this really necessary?
            //shader += String.Format("prev = frac(4.0f + prev * (255.0f/256.0f)) * (256.0f/255.0f);\n");

            shader += "gl_FragColor = prev;";
            
            //if (dstAlphaMode == DSTALPHA_ALPHA_PASS)
            //    shader += String.Format("  ocol0 = float4(prev.rgb, "I_ALPHA"[0].a);\n");
            //else
            //{
            //    WriteFog(p);
			    //shader += "  ocol0 = prev;\n";
		    //}

		    // On D3D11, use dual-source color blending to perform dst alpha in a
		    // single pass
            //if (dstAlphaMode == DSTALPHA_DUAL_SOURCE_BLEND)
            //{
            //    // Colors will be blended against the alpha from ocol1...
            //    shader += String.Format("  ocol1 = ocol0;\n");
            //    // ...and the alpha from ocol0 will be written to the framebuffer.
			    //shader += "  ocol0.a = "+I_ALPHA+"[0].a;\n";
		    //}

	        shader += "}\n";

            _renderChange = false;
        }

        public void GenProgram(GLContext ctx)
        {
            _context = ctx;
        }

        public uint fragmentHandle, vertexHandle, programHandle;
        public bool written = false; public string vs;
        public void Render(GLContext ctx, MDL0MaterialNode node)
        {
            ctx.glEnable(GLEnableCap.FRAGMENT_PROGRAM_ARB);
            uint id;
            ctx.glGenProgramsARB(1, &id);
            if ((programHandle = id) == 0)
                ctx.CheckErrors();
            _context = ctx;
            int version = (int)(new string((sbyte*)ctx.glGetString(0x1F02)))[0];
            if (version < 2)
                MessageBox.Show("You need at least OpenGL 2.0 to render shaders.",
                "GLSL not supported", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            _renderChange = true;
            if (_renderChange)
            {
                //Test shader
                shader = @"uniform sampler2D samp0, samp1, samp2;
                void main(void) 
                { 
                    gl_FragColor = vec4 (1.0, 0.0, 0.0, 1.0);
                }
                ";
                vs = @"
                void main(void)
                {
                  gl_Position = ftransform(); // gl_ModelViewProjectionMatrix * gl_Vertex;
                }
                ";
                //CreateGLSLShader(node);
            }

            if (!written)
            {
                Console.WriteLine(shader);
                written = true;
            }

            //Create shader
            fragmentHandle = (uint)_context.glCreateShader(ShaderType.FragmentShader);
            vertexHandle = (uint)_context.glCreateShader(ShaderType.VertexShader);

            //Create program
            //shaderProgramHandle = _context.glCreateProgram();

            //Set the shader code
            _context.ShaderSource((int)fragmentHandle, shader);
            _context.ShaderSource((int)vertexHandle, vs);
            //_context.glProgramStringARB(AssemblyProgramTargetArb.FragmentProgram, OpenGL.etc.ArbVertexProgram.ProgramFormatAsciiArb, shader.Length, shader);

            //Compile the shader
            _context.glCompileShader(fragmentHandle);
            _context.glCompileShader(vertexHandle);
            //_context.glBindProgramARB(AssemblyProgramTargetArb.FragmentProgram, (int)shaderProgramHandle);

            //Attach the shader to the program
            //_context.glAttachObjectARB(shaderProgramHandle, fragmentShaderHandle);

            //Check to see if the shader compiled correctly
            //_context.GetShaderInfoLog(fragmentObject, out info);
            //_context.GetShader(fragmentObject, ShaderParameter.CompileStatus, out status_code);

            //if (status_code != 1)
            //    throw new ApplicationException(info);

            _context.glAttachShader(programHandle, fragmentHandle);
            _context.glAttachShader(programHandle, vertexHandle);
            _context.glLinkProgram(programHandle);
            _context.glUseProgram(programHandle);
            //_context.glUseProgramObjectARB(shaderProgramHandle);

            ctx.glDisable((uint)GLEnableCap.FRAGMENT_PROGRAM_ARB);
            ctx.glDisable((uint)GLEnableCap.VERTEX_PROGRAM_ARB);

            rendered = true;
        }

        #region Table Variables

        public static readonly string I_COLORS = "color";
        public static readonly string I_KCOLORS = "k";
        public readonly string I_ALPHA = "alphaRef";
        public readonly string I_TEXDIMS = "texdim";
        public readonly string I_ZBIAS = "czbias";
        public readonly string I_INDTEXSCALE = "cindscale";
        public readonly string I_INDTEXMTX = "cindmtx";
        public readonly string I_FOG = "cfog";
        public readonly string I_PLIGHTS = "cLights";
        public readonly string I_PMATERIALS = "cmtrl";

        public readonly int C_COLORMATRIX = 0;
        public readonly int C_COLORS = 0;
        public readonly int C_KCOLORS = 4;
        public readonly int C_ALPHA = 8;
        public readonly int C_TEXDIMS = 9;
        public readonly int C_ZBIAS = 17;
        public readonly int C_INDTEXSCALE = 19;
        public readonly int C_INDTEXMTX = 21;
        public readonly int C_FOG = 27;

        public readonly int C_PLIGHTS = 30;
        public readonly int C_PMATERIALS = 70;
        public readonly int C_PENVCONST_END = 74;
        public readonly int PIXELSHADERUID_MAX_VALUES = 70;
        public readonly int PIXELSHADERUID_MAX_VALUES_SAFE = 120;

        public readonly string[] tevKSelTableC = // KCSEL
        {
	        "1.0f,1.0f,1.0f",       // 1   = 0x00
	        "0.875f,0.875f,0.875f", // 7_8 = 0x01
	        "0.75f,0.75f,0.75f",    // 3_4 = 0x02
	        "0.625f,0.625f,0.625f", // 5_8 = 0x03
	        "0.5f,0.5f,0.5f",       // 1_2 = 0x04
	        "0.375f,0.375f,0.375f", // 3_8 = 0x05
	        "0.25f,0.25f,0.25f",    // 1_4 = 0x06
	        "0.125f,0.125f,0.125f", // 1_8 = 0x07
	        "ERROR", // 0x08
	        "ERROR", // 0x09
	        "ERROR", // 0x0a
	        "ERROR", // 0x0b
	        I_KCOLORS+"[0].rgb", // K0 = 0x0C
	        I_KCOLORS+"[1].rgb", // K1 = 0x0D
	        I_KCOLORS+"[2].rgb", // K2 = 0x0E
	        I_KCOLORS+"[3].rgb", // K3 = 0x0F
	        I_KCOLORS+"[0].rrr", // K0_R = 0x10
	        I_KCOLORS+"[1].rrr", // K1_R = 0x11
	        I_KCOLORS+"[2].rrr", // K2_R = 0x12
	        I_KCOLORS+"[3].rrr", // K3_R = 0x13
	        I_KCOLORS+"[0].ggg", // K0_G = 0x14
	        I_KCOLORS+"[1].ggg", // K1_G = 0x15
	        I_KCOLORS+"[2].ggg", // K2_G = 0x16
	        I_KCOLORS+"[3].ggg", // K3_G = 0x17
	        I_KCOLORS+"[0].bbb", // K0_B = 0x18
	        I_KCOLORS+"[1].bbb", // K1_B = 0x19
	        I_KCOLORS+"[2].bbb", // K2_B = 0x1A
	        I_KCOLORS+"[3].bbb", // K3_B = 0x1B
	        I_KCOLORS+"[0].aaa", // K0_A = 0x1C
	        I_KCOLORS+"[1].aaa", // K1_A = 0x1D
	        I_KCOLORS+"[2].aaa", // K2_A = 0x1E
	        I_KCOLORS+"[3].aaa", // K3_A = 0x1F
        };

        public readonly string[] tevKSelTableA = // KASEL
        {
	        "1.0f",  // 1   = 0x00
	        "0.875f",// 7_8 = 0x01
	        "0.75f", // 3_4 = 0x02
	        "0.625f",// 5_8 = 0x03
	        "0.5f",  // 1_2 = 0x04
	        "0.375f",// 3_8 = 0x05
	        "0.25f", // 1_4 = 0x06
	        "0.125f",// 1_8 = 0x07
	        "ERROR", // 0x08
	        "ERROR", // 0x09
	        "ERROR", // 0x0a
	        "ERROR", // 0x0b
	        "ERROR", // 0x0c
	        "ERROR", // 0x0d
	        "ERROR", // 0x0e
	        "ERROR", // 0x0f
	        I_KCOLORS+"[0].r", // K0_R = 0x10
	        I_KCOLORS+"[1].r", // K1_R = 0x11
	        I_KCOLORS+"[2].r", // K2_R = 0x12
	        I_KCOLORS+"[3].r", // K3_R = 0x13
	        I_KCOLORS+"[0].g", // K0_G = 0x14
	        I_KCOLORS+"[1].g", // K1_G = 0x15
	        I_KCOLORS+"[2].g", // K2_G = 0x16
	        I_KCOLORS+"[3].g", // K3_G = 0x17
	        I_KCOLORS+"[0].b", // K0_B = 0x18
	        I_KCOLORS+"[1].b", // K1_B = 0x19
	        I_KCOLORS+"[2].b", // K2_B = 0x1A
	        I_KCOLORS+"[3].b", // K3_B = 0x1B
	        I_KCOLORS+"[0].a", // K0_A = 0x1C
	        I_KCOLORS+"[1].a", // K1_A = 0x1D
	        I_KCOLORS+"[2].a", // K2_A = 0x1E
	        I_KCOLORS+"[3].a", // K3_A = 0x1F
        };

        public readonly string[] tevScaleTable = // CS
        {
	        "1.0f",  // SCALE_1
	        "2.0f",  // SCALE_2
	        "4.0f",  // SCALE_4
	        "0.5f",  // DIVIDE_2
        };

        public readonly string[] tevBiasTable = // TB
        {
	        "",       // ZERO,
	        "+0.5f",  // ADDHALF,
	        "-0.5f",  // SUBHALF,
	        "",
        };

        public readonly string[] tevOpTable = { // TEV
	        "+",      // TEVOP_ADD = 0,
	        "-",      // TEVOP_SUB = 1,
        };

        public readonly string[] tevCInputTable = // CC
        {
	        "(prev.rgb)",               // CPREV,
	        "(prev.aaa)",         // APREV,
	        "(c0.rgb)",                 // C0,
	        "(c0.aaa)",           // A0,
	        "(c1.rgb)",                 // C1,
	        "(c1.aaa)",           // A1,
	        "(c2.rgb)",                 // C2,
	        "(c2.aaa)",           // A2,
	        "(textemp.rgb)",            // TEXC,
	        "(textemp.aaa)",      // TEXA,
	        "(rastemp.rgb)",            // RASC,
	        "(rastemp.aaa)",      // RASA,
	        "float3(1.0f, 1.0f, 1.0f)",              // ONE
	        "float3(0.5f, 0.5f, 0.5f)",                 // HALF
	        "(konsttemp.rgb)", //"konsttemp.rgb",        // KONST
	        "float3(0.0f, 0.0f, 0.0f)",              // ZERO
	        ///added extra values to map clamped values
	        "(cprev.rgb)",               // CPREV,
	        "(cprev.aaa)",         // APREV,
	        "(cc0.rgb)",                 // C0,
	        "(cc0.aaa)",           // A0,
	        "(cc1.rgb)",                 // C1,
	        "(cc1.aaa)",           // A1,
	        "(cc2.rgb)",                 // C2,
	        "(cc2.aaa)",           // A2,
	        "(textemp.rgb)",            // TEXC,
	        "(textemp.aaa)",      // TEXA,
	        "(crastemp.rgb)",            // RASC,
	        "(crastemp.aaa)",      // RASA,
	        "float3(1.0f, 1.0f, 1.0f)",              // ONE
	        "float3(0.5f, 0.5f, 0.5f)",                 // HALF
	        "(ckonsttemp.rgb)", //"konsttemp.rgb",        // KONST
	        "float3(0.0f, 0.0f, 0.0f)",              // ZERO
	        "PADERROR", "PADERROR", "PADERROR", "PADERROR"
        };

        public readonly string[] tevAInputTable = // CA
        {
	        "prev",            // APREV,
	        "c0",              // A0,
	        "c1",              // A1,
	        "c2",              // A2,
	        "textemp",         // TEXA,
	        "rastemp",         // RASA,
	        "konsttemp",       // KONST,  (hw1 had quarter)
	        "float4(0.0f, 0.0f, 0.0f, 0.0f)", // ZERO
	        ///aded extra values to map clamped values
	        "cprev",            // APREV,
	        "cc0",              // A0,
	        "cc1",              // A1,
	        "cc2",              // A2,
	        "textemp",         // TEXA,
	        "crastemp",         // RASA,
	        "ckonsttemp",       // KONST,  (hw1 had quarter)
	        "float4(0.0f, 0.0f, 0.0f, 0.0f)", // ZERO
	        "PADERROR", "PADERROR", "PADERROR", "PADERROR",
	        "PADERROR", "PADERROR", "PADERROR", "PADERROR",
        };

        public readonly string[] tevRasTable = 
        {
	        "colors_0",
	        "colors_1",
	        "ERROR", //2
	        "ERROR", //3
	        "ERROR", //4
	        "alphabump", // use bump alpha
	        "(alphabump*(255.0f/248.0f))", //normalized
	        "float4(0.0f, 0.0f, 0.0f, 0.0f)", // zero
        };

        //static readonly string *tevTexFunc[] = { "tex2D", "texRECT" };

        public readonly string[] tevCOutputTable = { "prev.rgb", "c0.rgb", "c1.rgb", "c2.rgb" };
        public readonly string[] tevAOutputTable = { "prev.a", "c0.a", "c1.a", "c2.a" };
        public readonly string[] tevIndAlphaSel = { "", "x", "y", "z" };
        //static readonly string *tevIndAlphaScale = {"", "*32", "*16", "*8"};
        public readonly string[] tevIndAlphaScale = { "*(248.0f/255.0f)", "*(224.0f/255.0f)", "*(240.0f/255.0f)", "*(248.0f/255.0f)" };
        public readonly string[] tevIndBiasField = { "", "x", "y", "xy", "z", "xz", "yz", "xyz" }; // indexed by bias
        public readonly string[] tevIndBiasAdd = { "-128.0f", "1.0f", "1.0f", "1.0f" }; // indexed by fmt
        public readonly string[] tevIndWrapStart = { "0.0f", "256.0f", "128.0f", "64.0f", "32.0f", "16.0f", "0.001f" };
        public readonly string[] tevIndFmtScale = { "255.0f", "31.0f", "15.0f", "7.0f" };

        #endregion

        #endregion

        public bool _autoMetal = false;
        public int texCount = -1;
        public bool rendered = false;

        public void Default()
        {
            Name = String.Format("Shader{0}", Index);
            _datalen = 512;
            ref0 =
            ref1 =
            ref2 =
            ref3 =
            ref4 =
            ref5 =
            ref6 =
            ref7 = -1;

            stages = 1;

            //MDL0ShaderStructNode s = new MDL0ShaderStructNode();
            //AddChild(s, true);
            //s.Default();

            TEVStage stage = new TEVStage(Children.Count);
            AddChild(stage, true);
            stage.Default();
        }

        public void DefaultAsMetal(int texcount)
        {
            Name = String.Format("Shader{0}", Index);
            _datalen = 512;
            _autoMetal = true;

            ref0 =
            ref1 =
            ref2 =
            ref3 =
            ref4 =
            ref5 =
            ref6 =
            ref7 = -1;

            switch ((texCount = texcount) - 1)
            {
                case 0: ref0 = 0; break;
                case 1: ref1 = 1; break;
                case 2: ref2 = 2; break;
                case 3: ref3 = 3; break;
                case 4: ref4 = 4; break;
                case 5: ref5 = 5; break;
                case 6: ref6 = 6; break;
                case 7: ref7 = 7; break;
            }

            stages = 4;

            Children.Clear();

            int i = 0;
            TEVStage s;
            while (i++ < 4)
            {
                AddChild(s = new TEVStage(i));
                s.DefaultAsMetal(texcount - 1);
            }

            //MDL0ShaderStructNode s1 = new MDL0ShaderStructNode(); AddChild(s1);
            //MDL0ShaderStructNode s2 = new MDL0ShaderStructNode(); AddChild(s2);
            //s1.DefaultAsMetal(texcount - 1);
            //s2.DefaultAsMetal(texcount - 1);
        }

        internal override void GetStrings(StringTable table)
        {
            //We DO NOT want to add the name to the string table!
        }

        protected override bool OnInitialize()
        {
            MDL0Shader* header = Header;

            _datalen = header->_dataLength;
            _mdl0offset = header->_mdl0Offset;

            stages = header->_stages;

            res0 = header->_res0;
            res1 = header->_res1;
            res2 = header->_res2;

            ref0 = header->_ref0;
            ref1 = header->_ref1;
            ref2 = header->_ref2;
            ref3 = header->_ref3;
            ref4 = header->_ref4;
            ref5 = header->_ref5;
            ref6 = header->_ref6;
            ref7 = header->_ref7;

            pad0 = header->_pad0;
            pad1 = header->_pad1;
            
            if (_name == null)
                _name = String.Format("Shader{0}", Index);

            //Attach to materials
            byte* pHeader = (byte*)Header;
            if ((Model != null) && (Model._matList != null))
                foreach (MDL0MaterialNode mat in Model._matList)
                {
                    MDL0Material* mHeader = mat.Header;
                    if (((byte*)mHeader + mHeader->_shaderOffset) == pHeader)
                    {
                        mat._shader = this;
                        _materials.Add(mat);
                    }
                }

            _swapBlock = *header->SwapBlock;
            getIRefValues();

            Populate();
            return true;
        }

        protected override void OnPopulate()
        {
            StageGroup* grp = Header->First;
            int offset = 0x80; //There are 8 groups max
            for (int r = 0; r < 8; r++, grp = grp->Next, offset += 0x30)
                if (((byte*)Header)[offset] == 0x61)
                {
                    TEVStage s0 = new TEVStage(r * 2);

                    KSel KSEL = new KSel(grp->ksel.Data.Value);
                    RAS1_TRef TREF = new RAS1_TRef(grp->tref.Data.Value);

                    s0.rawColEnv = grp->eClrEnv.Data.Value;
                    s0.rawAlphaEnv = grp->eAlpEnv.Data.Value;
                    s0.rawCMD = grp->eCMD.Data.Value;

                    s0.kcsel = KSEL.KCSEL0;
                    s0.kasel = KSEL.KASEL0;

                    s0.ti = TREF.TI0;
                    s0.tc = TREF.TC0;
                    s0.cc = TREF.CC0;
                    s0.te = TREF.TE0;

                    s0.getValues();
                    AddChild(s0, false);

                    if (grp->oClrEnv.Reg == 0x61 && grp->oAlpEnv.Reg == 0x61 && grp->oCMD.Reg == 0x61)
                    {
                        TEVStage s1 = new TEVStage(r * 2 + 1);

                        s1.rawColEnv = grp->oClrEnv.Data.Value;
                        s1.rawAlphaEnv = grp->oAlpEnv.Data.Value;
                        s1.rawCMD = grp->oCMD.Data.Value;

                        s1.kcsel = KSEL.KCSEL1;
                        s1.kasel = KSEL.KASEL1;

                        s1.ti = TREF.TI1;
                        s1.tc = TREF.TC1;
                        s1.cc = TREF.CC1;
                        s1.te = TREF.TE1;

                        s1.getValues();
                        AddChild(s1, false);
                    }

                    //new MDL0ShaderStructNode().Initialize(this, grp, StageGroup.Size);
                }
        }

        protected internal override void OnRebuild(VoidPtr address, int length, bool force)
        {
            MDL0Shader* header = (MDL0Shader*)address;

            if (Model._isImport)
            {
                ref1 =
                ref2 =
                ref3 =
                ref4 =
                ref5 =
                ref6 =
                ref7 = -1;

                if (Model._importOptions._mdlType == 0)
                    stages = 3;
                else
                    stages = 1;
            }

            header->_dataLength = length;
            header->_index = Index;

            header->_stages = Model._isImport ? stages : (byte)Children.Count;

            header->_res0 = res0;
            header->_res1 = res1;
            header->_res2 = res2;

            header->_ref0 = ref0;
            header->_ref1 = ref1;
            header->_ref2 = ref2;
            header->_ref3 = ref3;
            header->_ref4 = ref4;
            header->_ref5 = ref5;
            header->_ref6 = ref6;
            header->_ref7 = ref7;

            header->_pad0 = pad0;
            header->_pad1 = pad1;

            *header->SwapBlock = _swapBlock;

            //int offset = 0x80;
            //foreach (MDL0ShaderStructNode s in Children)
            //{
            //    s.Rebuild(address + offset, 0x30, force);
            //    offset += 0x30;
            //}

            StageGroup* grp = (StageGroup*)(address + 0x80);
            for (int i = 0; i < Children.Count; i++)
            {
                TEVStage c = (TEVStage)Children[i]; //Current Stage

                if (i % 2 == 0) //Even Stage
                {
                    *grp = StageGroup.Default;

                    grp->SetGroup(i / 2);
                    grp->SetStage(i);

                    grp->eClrEnv.Data.Value = c.rawColEnv;
                    grp->eAlpEnv.Data.Value = c.rawAlphaEnv;
                    grp->eCMD.Data.Value = c.rawCMD;

                    if (i == Children.Count - 1) //Last stage is even, odd stage isn't used
                    {
                        grp->ksel.Data.Value = KSel.Shift(0, 0, c.kcsel, c.kasel, 0, 0);
                        grp->tref.Data.Value = RAS1_TRef.Shift(c.ti, c.tc, c.te ? 1 : 0, c.cc, 7, 7, 0, 7);
                    }
                }
                else //Odd Stage
                {
                    TEVStage p = (TEVStage)Children[i - 1]; //Previous Stage

                    grp->SetStage(i);

                    grp->oClrEnv.Data.Value = c.rawColEnv;
                    grp->oAlpEnv.Data.Value = c.rawAlphaEnv;
                    grp->oCMD.Data.Value = c.rawCMD;

                    grp->ksel.Data.Value = KSel.Shift(0, 0, p.kcsel, p.kasel, c.kcsel, c.kasel);
                    grp->tref.Data.Value = RAS1_TRef.Shift(p.ti, p.tc, p.te ? 1 : 0, p.cc, c.ti, c.tc, c.te ? 1 : 0, c.cc);

                    grp = grp->Next;
                }
            }

            if (Model._isImport)
            {
                StageGroup* struct0 = header->First;
                *struct0 = StageGroup.Default;
                struct0->SetGroup(0);
                switch (Model._importOptions._mdlType)
                {
                    case 0: //Character
                        
                        struct0->SetStage(0);
                        struct0->SetStage(1);

                        struct0->mask.Data.Value = 0xFFFFF0;
                        struct0->ksel.Data.Value = 0xE378C0;
                        struct0->tref.Data.Value = 0x03F040;
                        struct0->eClrEnv.Data.Value = 0x28F8AF;
                        struct0->oClrEnv.Data.Value = 0x08FEB0;
                        struct0->eAlpEnv.Data.Value = 0x08F2F0;
                        struct0->oAlpEnv.Data.Value = 0x081FF0;

                        //new MDL0ShaderStructNode().Initialize(this, header->First, StageGroup.Size);

                        StageGroup* struct1 = struct0->Next;
                        *struct1 = StageGroup.Default;

                        struct1->SetGroup(1);
                        struct1->SetStage(2);

                        struct1->mask.Data.Value = 0xFFFFF0;
                        struct1->ksel.Data.Value = 0x0038C0;
                        struct1->tref.Data.Value = 0x3BF3BF;
                        struct1->eClrEnv.Data.Value = 0x0806EF;
                        struct1->eAlpEnv.Data.Value = 0x081FF0;

                        //new MDL0ShaderStructNode().Initialize(this, struct0->Next, StageGroup.Size);
                        break;

                    case 1: //Stage/Item

                        struct0->SetStage(0);

                        struct0->mask.Data.Value = 0xFFFFF0;
                        struct0->ksel.Data.Value = 0x0038C0;
                        struct0->tref.Data.Value = 0x3BF040;
                        struct0->eClrEnv.Data.Value = 0x28F8AF;
                        struct0->eAlpEnv.Data.Value = 0x08F2F0;

                        //new MDL0ShaderStructNode().Initialize(this, header->First, StageGroup.Size);
                        break;
                }
            }
        }

        protected override int OnCalculateSize(bool force)
        {
            return 512; //Shaders are always 0x200 in length!
        }

        //public override void RemoveChild(ResourceNode child)
        //{
        //    base.RemoveChild(child);
        //    //foreach (MDL0ShaderStructNode s in Children)
        //    //    s.RecalcStages();
        //}

        //public override void Remove()
        //{
        //    //MDL0Node node = Model;
        //    base.Remove();
        //    //foreach (ResourceNode n in node._shadList)
        //    //    n.Name = "Shader" + n.Index;
        //}
    }
}
