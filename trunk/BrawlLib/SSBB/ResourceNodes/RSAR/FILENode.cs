using System;
using BrawlLib.SSBBTypes;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class FILENode : ResourceNode
    {
        internal FILEHeader* Header { get { return (FILEHeader*)WorkingUncompressed.Address; } }

        protected override bool OnInitialize()
        {
            _name = "FILE";
            return true;
        }
        protected override void OnPopulate()
        {
        }
    }
}
