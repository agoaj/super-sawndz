using System;
using BrawlLib.SSBBTypes;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class RSARExtFileNode : RSARFileNode
    {
        internal INFOFileHeader* Header { get { return (INFOFileHeader*)WorkingUncompressed.Address; } }

        //internal string _externalPath;
        //public string ExtPath { get { return _externalPath; } set { _externalPath = value; SignalPropertyChange(); } }

        protected override bool OnInitialize()
        {
            RSARNode parent = RSARNode;
            _extPath = Header->GetPath(&RSARNode.Header->INFOBlock->_collection);
            if (_name == null)
                _name = String.Format("[{0:X3}] {1}", _fileIndex, _extPath);

            return false;
        }
    }
}
