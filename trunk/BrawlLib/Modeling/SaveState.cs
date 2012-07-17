using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.Wii.Animations;
using BrawlLib.SSBB.ResourceNodes;

namespace BrawlLib.Modeling
{
    public class SaveState2
    {
        public int frameIndex = 0;

        public CHR0Node animation;
        public MDL0BoneNode bone;
        public FrameState frameState;

        public List<CollisionLink> collisionLinks;
        public List<Vector2> vectors_2;
        public bool split;
        public bool merge;
        public bool create;
        public bool delete;

        public CollisionNode collisionNode;
        public CollisionObject collisionObject;
        public CollisionPlane collisionPlane;
    }

    public class SaveState
    {
        public int id;

        public bool undo = false;
        public bool redo = true;

        public bool primarySave = false;
        public bool keyframeSet = false;
        public bool keyframeRemoved = false;
        public bool boxChanged = false;
        public bool frameDeleted = false;
        public bool newEntry = false;
        public bool animPorted = false;
        
        public FrameState oldFrameState;
        public FrameState newFrameState;
        public CHR0Node animation; 
        public CHR0Node oldAnimation;
        public MDL0BoneNode bone;
        
        public int frameIndex = 1;
        public int boxIndex = -1;
        
        //0-2 Trans xyz, 3-5 Rot xyz, 6-8 Scale xyz
        public bool[] newBox = new bool[9];
        public float[] newBoxValues = new float[9];
        public bool[] oldBox = new bool[9];
        public float[] oldBoxValues = new float[9];

        public void SwitchType()
        {
            if (undo) { undo = false; redo = true; }
            else 
            if (redo) { undo = true; redo = false; }
        }
    }
}
