using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.SSBBTypes;
using System.ComponentModel;
using BrawlLib.Wii.Graphics;
using System.Windows.Forms;
using BrawlLib.Imaging;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class TEVAlphaEnvNode : TEVNode
    {
        GXColorS10 d;
        GXColorS10 c, b, a;

        int rswap, tswap, seld, selc, selb, sela, bias, shift, dest;
        bool sub, clamp;

        [Category("TEV Alpha Env - Alpha In"), Browsable(true)]
        public TevSwapSel RasSwap { get { return (TevSwapSel)rswap; } set { rswap = (int)value; getRawValue(); } }
        [Category("TEV Alpha Env - Alpha In"), Browsable(true)]
        public TevSwapSel TexSwap { get { return (TevSwapSel)tswap; } set { tswap = (int)value; getRawValue(); } }

        [Category("TEV Alpha Env - Alpha In"), Browsable(true)]
        public AlphaArg SelD 
        { 
            get { return (AlphaArg)seld; }
            set 
            {
                if ((int)value <= 7)
                    if ((int)value >= 0)
                    { 
                        seld = (int)value;
                        getRawValue(); 
                    }
                    else MessageBox.Show("Value cannot be less than 0!");
                else MessageBox.Show("Value cannot be greater than 7!"); 
            } 
        }

        [Category("TEV Alpha Env - Alpha In"), Browsable(true)]
        public AlphaArg SelC
        {
            get { return (AlphaArg)selc; }
            set
            {
                if ((int)value <= 7)
                    if ((int)value >= 0)
                    {
                        selc = (int)value;
                        getRawValue();
                    }
                    else MessageBox.Show("Value cannot be less than 0!");
                else MessageBox.Show("Value cannot be greater than 7!");
            }
        }

        [Category("TEV Alpha Env - Alpha In"), Browsable(true)]
        public AlphaArg SelB
        {
            get { return (AlphaArg)selb; }
            set
            {
                if ((int)value <= 7)
                    if ((int)value >= 0)
                    {
                        selb = (int)value;
                        getRawValue();
                    }
                    else MessageBox.Show("Value cannot be less than 0!");
                else MessageBox.Show("Value cannot be greater than 7!");
            }
        }
        
        [Category("TEV Alpha Env - Alpha In"), Browsable(true)]
        public AlphaArg SelA
        {
            get { return (AlphaArg)sela; }
            set
            {
                if ((int)value <= 7)
                    if ((int)value >= 0)
                    {
                        sela = (int)value;
                        getRawValue();
                    }
                    else MessageBox.Show("Value cannot be less than 0!");
                else MessageBox.Show("Value cannot be greater than 7!");
            }
        }

        [Category("TEV Alpha Env - Operation"), Browsable(true)]
        public Bias Bias { get { return (Bias)bias; } set { bias = (int)value; getRawValue(); } }

        [Category("TEV Alpha Env - Operation"), Browsable(true)]
        public bool Subtract { get { return sub; } set { sub = value; getRawValue(); } }
        [Category("TEV Alpha Env - Operation"), Browsable(true)]
        public bool Clamp { get { return clamp; } set { clamp = value; getRawValue(); } }

        [Category("TEV Alpha Env - Operation"), Browsable(true)]
        public TevScale Scale { get { return (TevScale)shift; } set { shift = (int)value; getRawValue(); } }
        [Category("TEV Alpha Env - Operation"), Browsable(true)]
        public TevRegID Register { get { return (TevRegID)dest; } set { dest = (int)value; getRawValue(); } }

        public override void NameChanged() { Name = String.Format("AlphaEnv{0}", _stage); }

        protected override bool OnInitialize()
        {
            Int24* data = Data;
            _name = String.Format("AlphaEnv{0}", _stage);
            _rawValue = data->Value;
            getValues();
            return false;
        }

        public GXColorS10 calcReg()
        {
            GXColorS10 reg;
            if (Subtract)
                reg = (d - ((1.0f - c) * a + c * b) + (bias == 1 ? 0.5f : bias == 2 ? -0.5f : 0)) * (shift == 3 ? 0.5f : shift == 0 ? 1 : shift * 2);
            else
                reg = (d + ((1.0f - c) * a + c * b) + (bias == 1 ? 0.5f : bias == 2 ? -0.5f : 0)) * (shift == 3 ? 0.5f : shift == 0 ? 1 : shift * 2);
            if (Clamp)
                reg.CutoffTo8bit(); //No conversion
            return reg;
        }

        public override void getValues()
        {
            AlphaEnv data = new AlphaEnv(_rawValue);
            rswap = data.RSwap;
            tswap = data.TSwap;
            seld = data.SelD;
            selc = data.SelC;
            selb = data.SelB;
            sela = data.SelA;
            bias = data.Bias;
            sub = data.Sub;
            clamp = data.Clamp;
            shift = data.Shift;
            dest = data.Dest;
        }

        private void getRawValue()
        {
            _rawValue = AlphaEnv.Shiftv(rswap, tswap, seld, selc, selb, sela, bias, sub ? 1 : 0, clamp ? 1 : 0, shift, dest);
            SignalPropertyChange();
        }
    }
}
