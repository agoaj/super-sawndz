using System;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class SYMBNode : ResourceNode
    {

        protected override bool OnInitialize()
        {
            _name = "SYMB";
            return true;
        }
        protected override void OnPopulate()
        {
        }
    }
}
