using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.SSBBTypes;
using System.ComponentModel;
using BrawlLib.Wii.Graphics;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class TEVTRefNode : TEVNode
    {
        int ti0, tc0, cc0, pad0, ti1, tc1, cc1, pad1;
        bool te0, te1;
        
        [Category("TEV RAS1 TRef"), Browsable(true)]
        public TexMapID Texture0MapID { get { return (TexMapID)ti0; } set { ti0 = (int)value; getRawValue(); } }
        [Category("TEV RAS1 TRef"), Browsable(true)]
        public TexCoordID TextureCoord0 { get { return (TexCoordID)tc0; } set { tc0 = (int)value; getRawValue(); } }
        [Category("TEV RAS1 TRef"), Browsable(true)]
        public bool Texture0Enabled { get { return te0; } set { te0 = value; getRawValue(); } }
        [Category("TEV RAS1 TRef"), Browsable(true)]
        public ColorSelChan ColorChannel0 { get { return (ColorSelChan)cc0; } set { cc0 = (int)value; getRawValue(); } }
        [Category("TEV RAS1 TRef"), Browsable(true)]
        public int Pad0 { get { return pad0; } }//set { pad0 = (int)value; getRawValue(); } }
        
        [Category("TEV RAS1 TRef"), Browsable(true)]
        public TexMapID Texture1MapID { get { return (TexMapID)ti1; } set { ti1 = (int)value; getRawValue(); } }
        [Category("TEV RAS1 TRef"), Browsable(true)]
        public TexCoordID TextureCoord1 { get { return (TexCoordID)tc1; } set { tc1 = (int)value; getRawValue(); } }
        [Category("TEV RAS1 TRef"), Browsable(true)]
        public bool Texture1Enabled { get { return te1; } set { te1 = value; getRawValue(); } }
        [Category("TEV RAS1 TRef"), Browsable(true)]
        public ColorSelChan ColorChannel1 { get { return (ColorSelChan)cc1; } set { cc1 = (int)value; getRawValue(); } }
        [Category("TEV RAS1 TRef"), Browsable(true)]
        public int Pad1 { get { return pad1; } }//set { pad1 = (int)value; getRawValue(); } }

        public override void NameChanged() { Name = String.Format("TRef{0}", _stage); }

        protected override bool OnInitialize()
        {
            Int24* data = Data;
            _name = String.Format("TRef{0}", _stage);
            _rawValue = data->Value;
            getValues();
            return false;
        }

        public override void getValues()
        {
            RAS1_TRef data = new RAS1_TRef(_rawValue);
            ti0 = data.TI0;
            tc0 = data.TC0;
            te0 = data.TE0;
            cc0 = data.CC0;
            pad0 = data.Pad0;

            ti1 = data.TI1;
            tc1 = data.TC1;
            te1 = data.TE1;
            cc1 = data.CC1;
            pad1 = data.Pad1;
        }

        private void getRawValue()
        {
            _rawValue = RAS1_TRef.Shift(ti0, tc0, te0 ? 1 : 0, cc0, ti1, tc1, te1 ? 1 : 0, cc1);
            SignalPropertyChange();
        }
    }
}
