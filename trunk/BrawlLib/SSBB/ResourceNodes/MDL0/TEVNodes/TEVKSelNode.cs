using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.SSBBTypes;
using System.ComponentModel;
using BrawlLib.Wii.Graphics;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class TEVKSelNode : TEVNode
    {
        int xrb, xga, kcsel0, kasel0, kcsel1, kasel1;
        
        //[Category("TEV KSel"), Browsable(true)]
        //public ColorChannel XRedBlue { get { return (ColorChannel)xrb; } set { xrb = (int)value; getRawValue(); } }
        //[Category("TEV KSel"), Browsable(true)]
        //public ColorChannel XGreenAlpha { get { return (ColorChannel)xga; } set { xga = (int)value; getRawValue(); } }
        [Category("TEV KSel"), Browsable(true)]
        public TevKColorSel KonstantColorSelection0 { get { return (TevKColorSel)kcsel0; } set { kcsel0 = (int)value; getRawValue(); } }
        [Category("TEV KSel"), Browsable(true)]
        public TevKAlphaSel KonstantAlphaSelection0 { get { return (TevKAlphaSel)kasel0; } set { kasel0 = (int)value; getRawValue(); } }
        [Category("TEV KSel"), Browsable(true)]
        public TevKColorSel KonstantColorSelection1 { get { return (TevKColorSel)kcsel1; } set { kcsel1 = (int)value; getRawValue(); } }
        [Category("TEV KSel"), Browsable(true)]
        public TevKAlphaSel KonstantAlphaSelection1 { get { return (TevKAlphaSel)kasel1; } set { kasel1 = (int)value; getRawValue(); } }

        public override void NameChanged() { Name = String.Format("KSel{0}", _stage); }

        protected override bool OnInitialize()
        {
            Int24* data = Data;
            _name = String.Format("KSel{0}", _stage);
            _rawValue = data->Value;
            getValues();
            return false;
        }

        public override void getValues()
        {
            KSel data = new KSel(_rawValue);
            xrb = 0;//data.XRB;
            xga = 0;//data.XGA;
            kcsel0 = data.KCSEL0;
            kasel0 = data.KASEL0;
            kcsel1 = data.KCSEL1;
            kasel1 = data.KASEL1;
        }

        private void getRawValue()
        {
            _rawValue = KSel.Shift(0, 0, kcsel0, kasel0, kcsel1, kasel1);
            SignalPropertyChange();
        }
    }
}
