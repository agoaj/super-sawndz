using System;
using BrawlLib.SSBBTypes;
using System.Audio;
using BrawlLib.Wii.Audio;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class RSEQLabelNode : RSEQEntryNode
    {
        internal RSEQ_LABLEntry* Header { get { return (RSEQ_LABLEntry*)WorkingUncompressed.Address; } }

        int _id;

        [Category("RSEQ Label")]
        public int Id { get { return _id; } set { _id = value; SignalPropertyChange(); } }
        
        protected override bool OnInitialize()
        {
            if (_name == null)
                if (Header->_stringLength > 0)
                    _name = Header->Name;
                else
                    _name = string.Format("Label[{0:X2}]", Index);

            _id = Header->_id;

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
