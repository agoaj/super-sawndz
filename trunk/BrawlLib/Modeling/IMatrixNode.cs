using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.Wii.Models;

namespace BrawlLib.Modeling
{
    public interface IMatrixNode
    {
        int ReferenceCount { get; set; }
        int NodeIndex { get; }
        Matrix Matrix { get; }
        Matrix InverseBindMatrix { get; }
        bool IsPrimaryNode { get; }
        BoneWeight[] Weights { get; }
        int PermanentID { get; }
    }
}
