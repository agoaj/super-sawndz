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
    public unsafe class TEVColorEnvNode : TEVNode
    {
        short d;
        byte c, b, a;

        GXColorS10 D = new GXColorS10();
        RGBAPixel C = new RGBAPixel();
        RGBAPixel B = new RGBAPixel();
        RGBAPixel A = new RGBAPixel();

        int seld, selc, selb, sela, bias, shift, dest;
        bool sub, clamp;

        [Category("TEV Color Env - Color In"), Browsable(true)]
        public ColorArg SelD
        {
            get { return (ColorArg)seld; }
            set
            {
                if ((int)value <= 15)
                    if ((int)value >= 0)
                    {
                        seld = (int)value;
                        getRawValue();

                        if ((seld >= 4 && seld <= 7) || seld == 9 || seld == 11)
                        {

                        }
                    }
                    else MessageBox.Show("Value cannot be less than 0!");
                else MessageBox.Show("Value cannot be greater than 15!");
            }
        }
        [Category("TEV Color Env - Color In"), Browsable(true)]
        public ColorArg SelC
        {
            get { return (ColorArg)selc; }
            set
            {
                if ((int)value <= 15)
                    if ((int)value >= 0)
                    {
                        selc = (int)value;
                        getRawValue();

                        if ((selc >= 4 && selc <= 7) || selc == 9 || selc == 11)
                        {

                        }
                    }
                    else MessageBox.Show("Value cannot be less than 0!");
                else MessageBox.Show("Value cannot be greater than 15!");
            }
        }
        [Category("TEV Color Env - Color In"), Browsable(true)]
        public ColorArg SelB
        {
            get { return (ColorArg)selb; }
            set
            {
                if ((int)value <= 15)
                    if ((int)value >= 0)
                    {
                        selb = (int)value;
                        getRawValue();

                        if ((selb >= 4 && selb <= 7) || selb == 9 || selb == 11)
                        {

                        }
                    }
                    else MessageBox.Show("Value cannot be less than 0!");
                else MessageBox.Show("Value cannot be greater than 15!");
            }
        }
        [Category("TEV Color Env - Color In"), Browsable(true)]
        public ColorArg SelA
        {
            get { return (ColorArg)sela; }
            set
            {
                if ((int)value <= 15)
                    if ((int)value >= 0)
                    {
                        sela = (int)value;
                        getRawValue();

                        if ((sela >= 4 && sela <= 7) || sela == 9 || sela == 11)
                        {

                        }
                    }
                    else MessageBox.Show("Value cannot be less than 0!");
                else MessageBox.Show("Value cannot be greater than 15!");
            }
        }
        [Category("TEV Color Env - Operation"), Browsable(true)]
        public Bias Bias { get { return (Bias)bias; } set { bias = (int)value; getRawValue(); } }

        [Category("TEV Color Env - Operation"), Browsable(true)]
        public bool Subtract { get { return sub; } set { sub = value; getRawValue(); } }
        [Category("TEV Color Env - Operation"), Browsable(true)]
        public bool Clamp { get { return clamp; } set { clamp = value; getRawValue(); } }

        [Category("TEV Color Env - Operation"), Browsable(true)]
        public TevScale Scale { get { return (TevScale)shift; } set { shift = (int)value; getRawValue(); } }
        [Category("TEV Color Env - Operation"), Browsable(true)]
        public TevRegID Register { get { return (TevRegID)dest; } set { dest = (int)value; getRawValue(); } }
        
        public override void NameChanged() { Name = String.Format("ColorEnv{0}", _stage); }

        protected override bool OnInitialize()
        {
            Int24* data = Data;
            _name = String.Format("ColorEnv{0}", _stage);
            _rawValue = data->Value;
            getValues();
            return false;
        }

        public int calcReg()
        {
            int reg;
            
            if (Subtract)
                reg = (d - ((255 - c) * a + c * b) + (bias == 1 ? (255 / 2) : bias == 2 ? -(255 / 2) : 0)) * (shift == 3 ? (1 / 2) : shift == 0 ? 1 : shift * 2);
            else
                reg = (d + ((255 - c) * a + c * b) + (bias == 1 ? (255 / 2) : bias == 2 ? -(255 / 2) : 0)) * (shift == 3 ? (1 / 2) : shift == 0 ? 1 : shift * 2);
            if (Clamp)
                reg = reg > 1 ? 1 : reg < 0 ? 0 : reg;

            return reg;
        }

        public override void getValues()
        {
            ColorEnv data = new ColorEnv(_rawValue);
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
            _rawValue = ColorEnv.Shiftv(seld, selc, selb, sela, bias, sub ? 1 : 0, clamp ? 1 : 0, shift, dest);
            SignalPropertyChange();
        }
    }
}
