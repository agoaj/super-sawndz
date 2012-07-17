using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.SSBBTypes;
using System.ComponentModel;
using BrawlLib.Wii.Graphics;
using BrawlLib.IO;
using BrawlLib.Imaging;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class TEVStage : MDL0EntryNode
    {
        public TEVStage(int index) { _name = String.Format("Stage{0}", index); }
        public override ResourceType ResourceType { get { return ResourceType.TEVStage; } }

        [Category("c TEV Color Env"), Browsable(true)]
        public string ColorOutput { get { return "(d " + (ColorSubtract ? "-" : "+") + " ((1 - c) * a + c * b) + " + ((int)ColorBias == 1 ? "0.5" : (int)ColorBias == 2 ? "-0.5" : "0") + ") * " + ((int)ColorScale == 3 ? "0.5" : (int)ColorScale == 0 ? "1" : ((int)ColorScale * 2).ToString()) + (ColorClamp ? ", clamped from 0 - 255" : ""); } }
        [Category("d TEV Alpha Env"), Browsable(true)]
        public string AlphaOutput { get { return "(d " + (AlphaSubtract ? "-" : "+") + " ((1 - c) * a + c * b) + " + ((int)AlphaBias == 1 ? "0.5" : (int)AlphaBias == 2 ? "-0.5" : "0") + ") * " + ((int)AlphaScale == 3 ? "0.5" : (int)AlphaScale == 0 ? "1" : ((int)AlphaScale * 2).ToString()) + (AlphaClamp ? ", clamped from 0 - 255" : ""); } }

        //Raw values. KSel and TRef control two stages, so they can't be stored here.
        public int rawColEnv, rawAlphaEnv, rawCMD;

        //KSel Values
        public int kcsel, kasel;

        //TRef Values
        public int ti, tc, cc;
        public bool te;

        //Color Env Values
        public int cseld, cselc, cselb, csela, cbias, cshift, cdest;
        public bool csub, cclamp;

        //Alpha Env Values
        public int rswap, tswap, aseld, aselc, aselb, asela, abias, ashift, adest;
        public bool asub, aclamp;

        //CMD Values
        public int bt, fmt, bias, bs, m, sw, tw, pad;
        public bool lb, fb;

        [Category("a TEV KSel"), Browsable(true)]
        public TevKColorSel KonstantColorSelection { get { return (TevKColorSel)kcsel; } set { kcsel = (int)value; SignalPropertyChange(); } }
        [Category("a TEV KSel"), Browsable(true)]
        public TevKAlphaSel KonstantAlphaSelection { get { return (TevKAlphaSel)kasel; } set { kasel = (int)value; SignalPropertyChange(); } }
        
        [Category("b TEV RAS1 TRef"), Browsable(true)]
        public TexMapID TextureMapID { get { return (TexMapID)ti; } set { ti = (int)value; SignalPropertyChange(); } }
        [Category("b TEV RAS1 TRef"), Browsable(true)]
        public TexCoordID TextureCoord { get { return (TexCoordID)tc; } set { tc = (int)value; SignalPropertyChange(); } }
        [Category("b TEV RAS1 TRef"), Browsable(true)]
        public bool TextureEnabled { get { return te; } set { te = value; SignalPropertyChange(); } }
        [Category("b TEV RAS1 TRef"), Browsable(true)]
        public ColorSelChan ColorChannel { get { return (ColorSelChan)cc; } set { cc = (int)value; SignalPropertyChange(); } }
        
        [Category("c TEV Color Env"), Browsable(true)]
        public ColorArg ColorSelectionA { get { return (ColorArg)csela; } set { csela = (int)value; getRawColEnv(); } }
        [Category("c TEV Color Env"), Browsable(true)]
        public ColorArg ColorSelectionB { get { return (ColorArg)cselb; } set { cselb = (int)value; getRawColEnv(); } }
        [Category("c TEV Color Env"), Browsable(true)]
        public ColorArg ColorSelectionC { get { return (ColorArg)cselc; } set { cselc = (int)value; getRawColEnv(); } }
        [Category("c TEV Color Env"), Browsable(true)]
        public ColorArg ColorSelectionD { get { return (ColorArg)cseld; } set { cseld = (int)value; getRawColEnv(); } }

        [Category("c TEV Color Env"), Browsable(true)]
        public Bias ColorBias { get { return (Bias)cbias; } set { cbias = (int)value; getRawColEnv(); } }

        [Category("c TEV Color Env"), Browsable(true)]
        public bool ColorSubtract { get { return csub; } set { csub = value; getRawColEnv(); } }
        [Category("c TEV Color Env"), Browsable(true)]
        public bool ColorClamp { get { return cclamp; } set { cclamp = value; getRawColEnv(); } }

        [Category("c TEV Color Env"), Browsable(true)]
        public TevScale ColorScale { get { return (TevScale)cshift; } set { cshift = (int)value; getRawColEnv(); } }
        [Category("c TEV Color Env"), Browsable(true)]
        public TevRegID ColorRegister { get { return (TevRegID)cdest; } set { cdest = (int)value; getRawColEnv(); } }
        
        [Category("d TEV Alpha Env"), Browsable(true)]
        public TevSwapSel AlphaRasterSwap { get { return (TevSwapSel)rswap; } set { rswap = (int)value; getRawAlphaEnv(); } }
        [Category("d TEV Alpha Env"), Browsable(true)]
        public TevSwapSel AlphaTextureSwap { get { return (TevSwapSel)tswap; } set { tswap = (int)value; getRawAlphaEnv(); } }
        
        [Category("d TEV Alpha Env"), Browsable(true)]
        public AlphaArg AlphaSelectionA { get { return (AlphaArg)asela; } set { asela = (int)value; getRawAlphaEnv(); } }
        [Category("d TEV Alpha Env"), Browsable(true)]
        public AlphaArg AlphaSelectionB { get { return (AlphaArg)aselb; } set { aselb = (int)value; getRawAlphaEnv(); } }
        [Category("d TEV Alpha Env"), Browsable(true)]
        public AlphaArg AlphaSelectionC { get { return (AlphaArg)aselc; } set { aselc = (int)value; getRawAlphaEnv(); } }
        [Category("d TEV Alpha Env"), Browsable(true)]
        public AlphaArg AlphaSelectionD { get { return (AlphaArg)aseld; } set { aseld = (int)value; getRawAlphaEnv(); } }

        [Category("d TEV Alpha Env"), Browsable(true)]
        public Bias AlphaBias { get { return (Bias)abias; } set { abias = (int)value; getRawAlphaEnv(); } }

        [Category("d TEV Alpha Env"), Browsable(true)]
        public bool AlphaSubtract { get { return asub; } set { asub = value; getRawAlphaEnv(); } }
        [Category("d TEV Alpha Env"), Browsable(true)]
        public bool AlphaClamp { get { return aclamp; } set { aclamp = value; getRawAlphaEnv(); } }
        
        [Category("d TEV Alpha Env"), Browsable(true)]
        public TevScale AlphaScale { get { return (TevScale)ashift; } set { ashift = (int)value; getRawAlphaEnv(); } }
        [Category("d TEV Alpha Env"), Browsable(true)]
        public TevRegID AlphaRegister { get { return (TevRegID)adest; } set { adest = (int)value; getRawAlphaEnv(); } }

        [Category("e TEV Ind CMD"), Browsable(true)]
        public IndTexStageID TexStage { get { return (IndTexStageID)bt; } set { bt = (int)value; getRawCMD(); } }
        [Category("e TEV Ind CMD"), Browsable(true)]
        public IndTexFormat TexFormat { get { return (IndTexFormat)fmt; } set { fmt = (int)value; getRawCMD(); } }
        [Category("e TEV Ind CMD"), Browsable(true)]
        public IndTexBiasSel Bias { get { return (IndTexBiasSel)bias; } set { bias = (int)value; getRawCMD(); } }
        [Category("e TEV Ind CMD"), Browsable(true)]
        public IndTexAlphaSel Alpha { get { return (IndTexAlphaSel)bs; } set { bs = (int)value; getRawCMD(); } }
        [Category("e TEV Ind CMD"), Browsable(true)]
        public IndTexMtxID Matrix { get { return (IndTexMtxID)m; } set { m = (int)value; getRawCMD(); } }

        [Category("e TEV Ind CMD"), Browsable(true)]
        public IndTexWrap S_Wrap { get { return (IndTexWrap)sw; } set { sw = (int)value; getRawCMD(); } }
        [Category("e TEV Ind CMD"), Browsable(true)]
        public IndTexWrap T_Wrap { get { return (IndTexWrap)tw; } set { tw = (int)value; getRawCMD(); } }
        [Category("e TEV Ind CMD"), Browsable(true)]
        public bool UsePrevStage { get { return lb; } set { lb = value; getRawCMD(); } }
        [Category("e TEV Ind CMD"), Browsable(true)]
        public bool UnmodifiedLOD { get { return fb; } set { fb = value; getRawCMD(); } }

        public void Default()
        {
            Name = String.Format("Stage{0}", Index);

            AlphaSelectionA = AlphaArg.Zero;
            AlphaSelectionB = AlphaArg.Zero;
            AlphaSelectionC = AlphaArg.Zero;
            AlphaSelectionD = AlphaArg.Zero;
            AlphaBias = Wii.Graphics.Bias.Zero;
            AlphaClamp = true;

            ColorSelectionA = ColorArg.Zero;
            ColorSelectionB = ColorArg.Zero;
            ColorSelectionC = ColorArg.Zero;
            ColorSelectionD = ColorArg.Zero;
            ColorBias = Wii.Graphics.Bias.Zero;
            ColorClamp = true;

            TextureMapID = TexMapID.TexMap7;
            TextureCoord = TexCoordID.TexCoord7;
            ColorChannel = ColorSelChan.Zero;

            getValues();
        }

        public void DefaultAsMetal(int texIndex)
        {
            Name = String.Format("Stage{0}", Index);

            if (Index == 0)
            {
                rawColEnv = 0x28F8AF;
                rawAlphaEnv = 0x08F2F0;
                KonstantColorSelection = TevKColorSel.KSel_0_Value;
                KonstantAlphaSelection = TevKAlphaSel.KSel_0_Alpha;
                cc = 0;
                TextureCoord = TexCoordID.TexCoord0 + texIndex;
                TextureMapID = TexMapID.TexMap0 + texIndex;
                TextureEnabled = true;
            }
            else if (Index == 1)
            {
                rawColEnv = 0x08AFF0;
                rawAlphaEnv = 0x08FF80;
                KonstantColorSelection = TevKColorSel.KSel_0_Value;
                KonstantAlphaSelection = TevKAlphaSel.KSel_0_Alpha;
                cc = 1;
                TextureCoord = TexCoordID.TexCoord7;
                TextureMapID = TexMapID.TexMap7;
                TextureEnabled = false;
            }
            else if (Index == 2)
            {
                rawColEnv = 0x08FEB0;
                rawAlphaEnv = 0x081FF0;
                KonstantColorSelection = TevKColorSel.KSel_1_Value;
                KonstantAlphaSelection = TevKAlphaSel.KSel_0_Alpha;
                cc = 0;
                TextureCoord = TexCoordID.TexCoord7;
                TextureMapID = TexMapID.TexMap7;
                TextureEnabled = false;
            }
            else if (Index == 3)
            {
                rawColEnv = 0x0806EF;
                rawAlphaEnv = 0x081FF0;
                KonstantColorSelection = TevKColorSel.KSel_0_Value;
                KonstantAlphaSelection = TevKAlphaSel.KSel_0_Alpha;
                cc = 7;
                TextureCoord = TexCoordID.TexCoord7;
                TextureMapID = TexMapID.TexMap7;
                TextureEnabled = false;
            }

            getValues();
        }

        public void getRawColEnv()
        {
            rawColEnv = ColorEnv.Shiftv(cseld, cselc, cselb, csela, cbias, csub ? 1 : 0, cclamp ? 1 : 0, cshift, cdest);
            SignalPropertyChange();
        }

        public void getRawAlphaEnv()
        {
            rawAlphaEnv = AlphaEnv.Shiftv(rswap, tswap, aseld, aselc, aselb, asela, abias, asub ? 1 : 0, aclamp ? 1 : 0, ashift, adest);
            SignalPropertyChange();
        }
        
        public void getRawCMD()
        {
            rawCMD = CMD.Shift(bt, fmt, bias, bs, m, sw, tw, lb ? 1 : 0, fb ? 1 : 0);
            SignalPropertyChange();
        }

        public void getRawValues()
        {
            getRawColEnv();
            getRawAlphaEnv();
            getRawCMD();
        }

        public void getValues()
        {
            getColEnvValues();
            getAlphaEnvValues();
            getCMDValues();
        }

        public void getColEnvValues()
        {
            ColorEnv data = new ColorEnv(rawColEnv);
            cseld = data.SelD;
            cselc = data.SelC;
            cselb = data.SelB;
            csela = data.SelA;
            cbias = data.Bias;
            csub = data.Sub;
            cclamp = data.Clamp;
            cshift = data.Shift;
            cdest = data.Dest;
        }

        public void getAlphaEnvValues()
        {
            AlphaEnv data = new AlphaEnv(rawAlphaEnv);
            rswap = data.RSwap;
            tswap = data.TSwap;
            aseld = data.SelD;
            aselc = data.SelC;
            aselb = data.SelB;
            asela = data.SelA;
            abias = data.Bias;
            asub = data.Sub;
            aclamp = data.Clamp;
            ashift = data.Shift;
            adest = data.Dest;
        }

        public void getCMDValues()
        {
            CMD data = new CMD(rawCMD);
            bt = data.BT;
            fmt = data.Format;
            bias = data.Bias;
            bs = data.BS;
            m = data.M;
            sw = data.SW;
            tw = data.TW;
            pad = data.Pad;
            lb = data.LB;
            fb = data.FB;
        }

        public override void Remove()
        {
            if (_parent == null)
                return;

            ((MDL0ShaderNode)Parent).STGs = (byte)(Parent.Children.Count - 1);
            base.Remove();
        }

        internal override void GetStrings(StringTable table)
        {
            //We DO NOT want to add the name to the string table!
        }

        //Write this stage to the shader code
        internal string Write(MDL0MaterialNode mat)
        {
            MDL0ShaderNode shader = ((MDL0ShaderNode)Parent);

            string stage = "";

            //Get the texture coordinate to use
            int texcoord = (int)TextureCoord;

            //Do we need to use the coordinates?
	        bool bHasTexCoord = texcoord < mat.Children.Count;

            //Is there an indirect stage? (CMD is not 0)
            //Is active == 
            //FB true
            //LB false
            //TW max
            //SW max
            //M max
            //0001 0111 1111 1110 0000 0000 = 0x17FE00
	        bool bHasIndStage = (rawCMD & 0x17FE00) != 0 && bt < mat.IndirectTextures;

	        // HACK to handle cases where the tex gen is not enabled
	        if (!bHasTexCoord)
		        texcoord = 0;

	        stage += String.Format("//TEV stage {0}\n", Index);

            //Add indirect support later

            //if (bHasIndStage)
            //{
            //    stage += String.Format("// indirect op\n");
            //    // perform the indirect op on the incoming regular coordinates using indtex%d as the offset coords
            //    if (bpmem.tevind[n].bs != ITBA_OFF)
            //    {
            //        stage += String.Format("alphabump = indtex%d.%s %s;\n",
            //                bpmem.tevind[n].bt,
            //                tevIndAlphaSel[bpmem.tevind[n].bs],
            //                tevIndAlphaScale[bpmem.tevind[n].fmt]);
            //    }
            //    // format
            //    stage += String.Format("float3 indtevcrd%d = indtex%d * %s;\n", n, bpmem.tevind[n].bt, tevIndFmtScale[bpmem.tevind[n].fmt]);

            //    // bias
            //    if (bpmem.tevind[n].bias != ITB_NONE )
            //        stage += String.Format("indtevcrd%d.%s += %s;\n", n, tevIndBiasField[bpmem.tevind[n].bias], tevIndBiasAdd[bpmem.tevind[n].fmt]);

            //    // multiply by offset matrix and scale
            //    if (bpmem.tevind[n].mid != 0)
            //    {
            //        if (bpmem.tevind[n].mid <= 3)
            //        {
            //            int mtxidx = 2*(bpmem.tevind[n].mid-1);
            //            stage += String.Format("float2 indtevtrans%d = float2(dot("I_INDTEXMTX"[%d].xyz, indtevcrd%d), dot("I_INDTEXMTX"[%d].xyz, indtevcrd%d));\n",
            //                n, mtxidx, n, mtxidx+1, n);
            //        }
            //        else if (bpmem.tevind[n].mid <= 7 && bHasTexCoord)
            //        { // s matrix
            //            if (bpmem.tevind[n].mid >= 5);
            //            int mtxidx = 2*(bpmem.tevind[n].mid-5);
            //            stage += String.Format("float2 indtevtrans%d = "I_INDTEXMTX"[%d].ww * uv%d.xy * indtevcrd%d.xx;\n", n, mtxidx, texcoord, n);
            //        }
            //        else if (bpmem.tevind[n].mid <= 11 && bHasTexCoord)
            //        { // t matrix
            //            if (bpmem.tevind[n].mid >= 9);
            //            int mtxidx = 2*(bpmem.tevind[n].mid-9);
            //            stage += String.Format("float2 indtevtrans%d = "I_INDTEXMTX"[%d].ww * uv%d.xy * indtevcrd%d.yy;\n", n, mtxidx, texcoord, n);
            //        }
            //        else
            //            stage += String.Format("float2 indtevtrans%d = 0;\n", n);
            //    }
            //    else
            //        stage += String.Format("float2 indtevtrans%d = 0;\n", n);

            //    // ---------
            //    // Wrapping
            //    // ---------

            //    // wrap S
            //    if (bpmem.tevind[n].sw == ITW_OFF)
            //        stage += String.Format("wrappedcoord.x = uv%d.x;\n", texcoord);
            //    else if (bpmem.tevind[n].sw == ITW_0)
            //        stage += String.Format("wrappedcoord.x = 0.0f;\n");
            //    else
            //        stage += String.Format("wrappedcoord.x = fmod( uv%d.x, %s );\n", texcoord, tevIndWrapStart[bpmem.tevind[n].sw]);

            //    // wrap T
            //    if (bpmem.tevind[n].tw == ITW_OFF)
            //        stage += String.Format("wrappedcoord.y = uv%d.y;\n", texcoord);
            //    else if (bpmem.tevind[n].tw == ITW_0)
            //        stage += String.Format("wrappedcoord.y = 0.0f;\n");
            //    else
            //        stage += String.Format("wrappedcoord.y = fmod( uv%d.y, %s );\n", texcoord, tevIndWrapStart[bpmem.tevind[n].tw]);

            //    if (bpmem.tevind[n].fb_addprev) // add previous tevcoord
            //        stage += String.Format("tevcoord.xy += wrappedcoord + indtevtrans%d;\n", n);
            //    else
            //        stage += String.Format("tevcoord.xy = wrappedcoord + indtevtrans%d;\n", n);
            //}

            //Check if we need to use Alpha
            if (ColorSelectionA == ColorArg.RasterAlpha || ColorSelectionA == ColorArg.RasterColor
             || ColorSelectionB == ColorArg.RasterAlpha || ColorSelectionB == ColorArg.RasterColor
             || ColorSelectionC == ColorArg.RasterAlpha || ColorSelectionC == ColorArg.RasterColor
             || ColorSelectionD == ColorArg.RasterAlpha || ColorSelectionD == ColorArg.RasterColor
             || AlphaSelectionA == AlphaArg.RasterAlpha || AlphaSelectionB == AlphaArg.RasterAlpha
             || AlphaSelectionC == AlphaArg.RasterAlpha || AlphaSelectionD == AlphaArg.RasterAlpha)
	        {
		        string rasswap = shader.swapModeTable[rswap];
                stage += String.Format("rastemp = {0}.{1};\n", shader.tevRasTable[(int)ColorChannel], rasswap);
		        stage += String.Format("crastemp = frac(rastemp * (255.0f/256.0f)) * (256.0f/255.0f);\n");
	        }

	        if (TextureEnabled)
	        {
		        if(!bHasIndStage) //Calculate tevcoord
			        if(bHasTexCoord)
				        stage += String.Format("tevcoord.xy = uv{0}.xy;\n", texcoord);
			        else
				        stage += String.Format("tevcoord.xy = float2(0.0f, 0.0f);\n");

                string texswap = shader.swapModeTable[tswap];
                int texmap = (int)TextureMapID;
		        //SampleTexture(p, "textemp", "tevcoord", texswap, texmap, ApiType);
                stage += String.Format("{0} = tex2D(samp{1},{2}.xy * "+shader.I_TEXDIMS+"[{3}].xy).{4};\n", "textemp", texmap, "tevcoord", texmap, texswap);
	        }
	        else
		        stage += String.Format("textemp = float4(1.0f, 1.0f, 1.0f, 1.0f);\n");

            //Check if we need to use Konstant Colors
            if (ColorSelectionA == ColorArg.KonstantColorSelection || 
                ColorSelectionB == ColorArg.KonstantColorSelection || 
                ColorSelectionC == ColorArg.KonstantColorSelection || 
                ColorSelectionD == ColorArg.KonstantColorSelection || 
                AlphaSelectionA == AlphaArg.KonstantAlphaSelection ||
                AlphaSelectionB == AlphaArg.KonstantAlphaSelection ||
                AlphaSelectionC == AlphaArg.KonstantAlphaSelection ||
                AlphaSelectionD == AlphaArg.KonstantAlphaSelection)
	        {
                int kc = (int)KonstantColorSelection;
                int ka = (int)KonstantAlphaSelection;
                stage += String.Format("konsttemp = float4({0}, {1});\n", shader.tevKSelTableC[kc], shader.tevKSelTableA[ka]);
		        if(kc > 7 || ka > 7)
			        stage += String.Format("ckonsttemp = frac(konsttemp * (255.0f/256.0f)) * (256.0f/255.0f);\n");
		        else
			        stage += String.Format("ckonsttemp = konsttemp;\n");
	        }

            if (ColorSelectionA == ColorArg.PreviousColor || ColorSelectionA == ColorArg.PreviousAlpha
             || ColorSelectionB == ColorArg.PreviousColor || ColorSelectionB == ColorArg.PreviousAlpha
             || ColorSelectionC == ColorArg.PreviousColor || ColorSelectionC == ColorArg.PreviousAlpha
             || AlphaSelectionA == AlphaArg.PreviousAlpha || AlphaSelectionB == AlphaArg.PreviousAlpha || AlphaSelectionC == AlphaArg.PreviousAlpha)
		        stage += String.Format("cprev = frac(prev * (255.0f/256.0f)) * (256.0f/255.0f);\n");

	        if (ColorSelectionA == ColorArg.Color0 || ColorSelectionA == ColorArg.Alpha0
	         || ColorSelectionB == ColorArg.Color0 || ColorSelectionB == ColorArg.Alpha0
	         || ColorSelectionC == ColorArg.Color0 || ColorSelectionC == ColorArg.Alpha0
	         || AlphaSelectionA == AlphaArg.Alpha0 || AlphaSelectionB == AlphaArg.Alpha0 || AlphaSelectionC == AlphaArg.Alpha0)
		        stage += String.Format("cc0 = frac(c0 * (255.0f/256.0f)) * (256.0f/255.0f);\n");

	        if (ColorSelectionA == ColorArg.Color1 || ColorSelectionA == ColorArg.Alpha1
	         || ColorSelectionB == ColorArg.Color1 || ColorSelectionB == ColorArg.Alpha1
	         || ColorSelectionC == ColorArg.Color1 || ColorSelectionC == ColorArg.Alpha1
	         || AlphaSelectionA == AlphaArg.Alpha1 || AlphaSelectionB == AlphaArg.Alpha1 || AlphaSelectionC == AlphaArg.Alpha1)
		        stage += String.Format("cc1 = frac(c1 * (255.0f/256.0f)) * (256.0f/255.0f);\n");

	        if (ColorSelectionA == ColorArg.Color2 || ColorSelectionA == ColorArg.Alpha2
	         || ColorSelectionB == ColorArg.Color2 || ColorSelectionB == ColorArg.Alpha2
	         || ColorSelectionC == ColorArg.Color2 || ColorSelectionC == ColorArg.Alpha2
	         || AlphaSelectionA == AlphaArg.Alpha2 || AlphaSelectionB == AlphaArg.Alpha2 || AlphaSelectionC == AlphaArg.Alpha2)
			    stage += String.Format("cc2 = frac(c2 * (255.0f/256.0f)) * (256.0f/255.0f);\n");

	        stage += String.Format("// color combine\n");
	        if (ColorClamp)
                stage += String.Format("{0} = saturate(", shader.tevCOutputTable[(int)ColorRegister]);
	        else
		        stage += String.Format("{0} = ", shader.tevCOutputTable[(int)ColorRegister]);

	        //Combine the color channel

            //There is no compare enum...
	        //if (ColorBias != TevBias_COMPARE) // if not compare
	        //{
		        //Normal color combiner goes here
		        if (ColorScale > TevScale.MultiplyBy1)
                    stage += String.Format("{0}*(", shader.tevScaleTable[(int)ColorScale]);

                if (!(ColorSelectionD == ColorArg.Zero && ColorSubtract == false))
                    stage += String.Format("{0}{1}", shader.tevCInputTable[(int)ColorSelectionD], shader.tevOpTable[ColorSubtract ? 1 : 0]);

                if (ColorSelectionA == ColorSelectionB)
                    stage += String.Format("{0}",
                        shader.tevCInputTable[(int)ColorSelectionA + 16]);
                else if (ColorSelectionC == ColorArg.Zero)
                    stage += String.Format("{0}",
                        shader.tevCInputTable[(int)ColorSelectionA + 16]);
                else if (ColorSelectionC == ColorArg.One)
                    stage += String.Format("{0}",
                        shader.tevCInputTable[(int)ColorSelectionB + 16]);
                else if (ColorSelectionA == ColorArg.Zero)
                    stage += String.Format("{0}*{1}",
                        shader.tevCInputTable[(int)ColorSelectionB + 16],
                        shader.tevCInputTable[(int)ColorSelectionC + 16]);
                else if (ColorSelectionB == ColorArg.Zero)
                    stage += String.Format("{0}*(float3(1.0f, 1.0f, 1.0f)-{1})",
                        shader.tevCInputTable[(int)ColorSelectionA + 16],
                        shader.tevCInputTable[(int)ColorSelectionC + 16]);
                else
                    stage += String.Format("lerp({0}, {1}, {2})",
                        shader.tevCInputTable[(int)ColorSelectionA + 16],
                        shader.tevCInputTable[(int)ColorSelectionB + 16],
                        shader.tevCInputTable[(int)ColorSelectionC + 16]);

                stage += shader.tevBiasTable[(int)ColorBias];

                if (ColorScale > TevScale.MultiplyBy1) stage += ")";
	        //}
	        //else
	        //{
		    //    int cmp = (cc.shift<<1)|cc.op|8; // comparemode stored here
		    //    stage += String.Format(TEVCMPColorOPTable[cmp],//lookup the function from the op table
			//	        tevCInputTable[ColorSelectionD],
			//	        tevCInputTable[ColorSelectionA + 16],
			//	        tevCInputTable[ColorSelectionB + 16],
			//	        tevCInputTable[ColorSelectionC + 16]);
	        //}
            if (ColorClamp) stage += ")";

	        stage += ";\n";

	        stage += String.Format("// alpha combine\n");
	        // combine the alpha channel
	        if (AlphaClamp)
                stage += String.Format("{0} = saturate(", shader.tevAOutputTable[(int)AlphaRegister]);
	        else
                stage += String.Format("{0} = ", shader.tevAOutputTable[(int)AlphaRegister]);

	        //if (AlphaSelectionBias != TevBias_COMPARE) // if not compare
	        //{
		        //normal alpha combiner goes here
		        if (AlphaScale > TevScale.MultiplyBy1)
			        stage += String.Format("{0}*(", shader.tevScaleTable[(int)AlphaScale]);

		        if(!(AlphaSelectionD == AlphaArg.Zero && AlphaSubtract == false))
			        stage += String.Format("{0}.a{1}", shader.tevAInputTable[(int)AlphaSelectionD], shader.tevOpTable[AlphaSubtract ? 1 : 0]);

		        if (AlphaSelectionA == AlphaSelectionB)
                    stage += String.Format("{0}.a", 
                        shader.tevAInputTable[(int)AlphaSelectionA + 8]);
		        else if (AlphaSelectionC == AlphaArg.Zero)
                    stage += String.Format("{0}.a", 
                        shader.tevAInputTable[(int)AlphaSelectionA + 8]);
                else if (AlphaSelectionA == AlphaArg.Zero)
                    stage += String.Format("{0}.a*{1}.a", 
                        shader.tevAInputTable[(int)AlphaSelectionB + 8], 
                        shader.tevAInputTable[(int)AlphaSelectionC + 8]);
                else if (AlphaSelectionB == AlphaArg.Zero)
                    stage += String.Format("{0}.a*(1.0f-{1}.a)", 
                        shader.tevAInputTable[(int)AlphaSelectionA + 8], 
                        shader.tevAInputTable[(int)AlphaSelectionC + 8]);
		        else
                    stage += String.Format("lerp({0}.a, {1}.a, {2}.a)", 
                        shader.tevAInputTable[(int)AlphaSelectionA + 8], 
                        shader.tevAInputTable[(int)AlphaSelectionB + 8],
                        shader.tevAInputTable[(int)AlphaSelectionC + 8]);

                stage += shader.tevBiasTable[(int)AlphaBias];

		        if (AlphaScale > 0)
			        stage += ")";

	        //}
	        //else
            //{
            //    //compare alpha combiner goes here
            //    int cmp = (ac.shift<<1)|ac.op|8; // comparemode stored here
            //    stage += String.Format(TEVCMPAlphaOPTable[cmp],
            //            tevAInputTable[AlphaSelectionD],
            //            tevAInputTable[AlphaSelectionA + 8],
            //            tevAInputTable[AlphaSelectionB + 8],
            //            tevAInputTable[AlphaSelectionC + 8]);
            //}
	        if (AlphaClamp)
		        stage += ")";

	        stage += String.Format(";\n\n");
	        stage += String.Format("//TEV stage done\n");

            return stage;
        }
    }
}
