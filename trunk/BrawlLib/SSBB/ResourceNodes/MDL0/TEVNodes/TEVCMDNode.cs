using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.SSBBTypes;
using System.ComponentModel;
using BrawlLib.Wii.Graphics;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class TEVCMDNode : TEVNode
    {
        int bt, fmt, bias, bs, m, sw, tw, pad;
        bool lb, fb;

        [Category("TEV CMD"), Browsable(true)]
        public IndTexStageID TexStage { get { return (IndTexStageID)bt; } set { bt = (int)value; getRawValue(); } }
        [Category("TEV CMD"), Browsable(true)]
        public IndTexFormat TexFormat { get { return (IndTexFormat)fmt; } set { fmt = (int)value; getRawValue(); } }
        [Category("TEV CMD"), Browsable(true)]
        public IndTexBiasSel Bias { get { return (IndTexBiasSel)bias; } set { bias = (int)value; getRawValue(); } }
        [Category("TEV CMD"), Browsable(true)]
        public IndTexAlphaSel Alpha { get { return (IndTexAlphaSel)bs; } set { bs = (int)value; getRawValue(); } }
        [Category("TEV CMD"), Browsable(true)]
        public IndTexMtxID Matrix { get { return (IndTexMtxID)m; } set { m = (int)value; getRawValue(); } }
        
        [Category("TEV CMD"), Browsable(true)]
        public IndTexWrap S_Wrap { get { return (IndTexWrap)sw; } set { sw = (int)value; getRawValue(); } }
        [Category("TEV CMD"), Browsable(true)]
        public IndTexWrap T_Wrap { get { return (IndTexWrap)tw; } set { tw = (int)value; getRawValue(); } }
        [Category("TEV CMD"), Browsable(true)]
        public bool UsePrevStage { get { return lb; } set { lb = value; getRawValue(); } }
        [Category("TEV CMD"), Browsable(true)]
        public bool UnmodifiedLOD { get { return fb; } set { fb = value; getRawValue(); } }
        [Category("TEV CMD"), Browsable(true)]
        public int Pad { get { return pad; } }//set { pad = value; getRawValue(); } }

        public override void NameChanged() { Name = String.Format("CMD{0}", _stage); }

        protected override bool OnInitialize()
        {
            Int24* data = Data;
            _name = String.Format("CMD{0}", _stage);
            _rawValue = data->Value;
            getValues();

            if (_rawValue != 0)
                Console.WriteLine("Shader" + Parent.Parent.Index + " - Struct" + Parent.Index + " - CMD" + _stage + "'s raw value is not 0! ");
            return false;
        }

        public override void getValues()
        {
            CMD data = new CMD(_rawValue);
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

        private void getRawValue()
        {
            _rawValue = CMD.Shift(bt, fmt, bias, bs, m, sw, tw, lb ? 1 : 0, fb ? 1 : 0);
            SignalPropertyChange();
        }
    }

    /*---------------------------------------------------------------------------*/
    //  Name:         GDSetTevIndirect
    //
    //  Desc:         Compile the state of an Indirect texture object in to HW 
    //                dependent values.
    //
    //  Arguments:    tev_stage:  	TEV stage name.
    //                ind_stage:  	Index of the Indirect texture being bound       
    //                format:     	format of indirect texture offsets.
    //                bias_sel:   	Bias added to the texture offsets.
    //                matrix_sel: 	Selects texture offset matrix.
    //                wrap_s:     	Wrap value of Direct S coordinate.
    //                wrap_t:     	Wrap value of Direct T coordinate
    //                add_prev:   	Add output from previous stage to texture coords.
    //                utc_lod:    	Use the unmodified texture coordinates for LOD.
    //                alpha_sel:  	Selects indirect texture alpha output.
    //                
    //  Returns:      none
    //
    /*---------------------------------------------------------------------------*/
}
