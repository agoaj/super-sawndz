using System;
using BrawlLib.SSBBTypes;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class RSEQDataNode : RWSDEntryNode
    {
        internal RSEQ_DATAEntry* Header { get { return (RSEQ_DATAEntry*)WorkingUncompressed.Address; } }

        internal float _float;
        internal byte _unk1, _unk2, _unk3;

        [Category("RSEQ Data")]
        public float Float { get { return _float; } set { _float = value; SignalPropertyChange(); } }
        [Category("RSEQ Data")]
        public byte Unk1 { get { return _unk1; } set { _unk1 = value; SignalPropertyChange(); } }
        [Category("RSEQ Data")]
        public byte Unk2 { get { return _unk2; } set { _unk2 = value; SignalPropertyChange(); } }
        [Category("RSEQ Data")]
        public byte Unk3 { get { return _unk3; } set { _unk3 = value; SignalPropertyChange(); } }

        protected override bool OnInitialize()
        {
            if (_name == null)
                _name = String.Format("Data[{0:X2}]", Index);

            _float = Header->_float;
            _unk1 = Header->_unk1;
            _unk2 = Header->_unk2;
            _unk3 = Header->_unk3;

            return false;
        }

        protected override int OnCalculateSize(bool force)
        {
            return base.OnCalculateSize(force);
        }

        protected internal override void OnRebuild(VoidPtr address, int length, bool force)
        {
            base.OnRebuild(address, length, force);
        }
    }
}
