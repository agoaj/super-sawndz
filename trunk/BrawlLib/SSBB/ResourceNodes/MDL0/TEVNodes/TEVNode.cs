using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.Wii.Graphics;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class TEVNode : MDL0EntryNode
    {
        internal Int24* Data { get { return (Int24*)_origSource.Address; } }

        public int _stage; //Max stage is 16
        public TevStageID Stage { get { return (TevStageID)_stage; } }

        [Browsable(false)]
        public int Stg { get { return _stage; } set { _stage = value; NameChanged(); } }
        [Browsable(false)]
        public int RawVal { get { return _rawValue; } set { _rawValue = value; getValues(); SignalPropertyChange(); } }
        
        public int _rawValue;
        public string RawValue { get { return _rawValue.ToString("X"); ; } set { _rawValue = Int32.Parse(value, System.Globalization.NumberStyles.HexNumber); getValues(); SignalPropertyChange(); } }

        protected override int OnCalculateSize(bool force) { return 3; }

        public virtual void getValues() { }

        public virtual void NameChanged() { }

        protected internal override void OnRebuild(VoidPtr address, int length, bool force)
        {
            Int24* val = (Int24*)address;
            *val = new Int24(_rawValue);
        }

        internal override void GetStrings(StringTable table)
        { 
            //We DO NOT want to add the name to the string table! 
        }

        public override string ToString()
        { return _parent._parent._name + "_" + _parent._name + "_" + _name; }
    }
}
