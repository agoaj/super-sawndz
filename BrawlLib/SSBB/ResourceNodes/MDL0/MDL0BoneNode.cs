using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using BrawlLib.Wii.Models;
using BrawlLib.SSBBTypes;
using BrawlLib.Modeling;
using BrawlLib.OpenGL;
using System.Drawing;
using BrawlLib.Wii.Animations;
using BrawlLib.Wii.Compression;
using System.Windows;
using BrawlLib.IO;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class MDL0BoneNode : MDL0EntryNode, IMatrixNode
    {
        private List<string> _entries = new List<string>();

        public int _permanentID;
        [Browsable(false)]
        public int PermanentID { get { return _permanentID; } }

        internal MDL0Bone* Header { get { return (MDL0Bone*)WorkingUncompressed.Address; } }

        public bool _moved = false;
        [Browsable(false)]
        public bool Moved
        {  
            get { return _moved;  } 
            set 
            {
                _moved = true;
                Model.SignalPropertyChange();
                Model.Rebuild(false); //Bone rebuilds are forced automatically
            } 
        }

        public override ResourceType ResourceType { get { return ResourceType.MDL0Bone; } }

        public BoneFlags _flags;
        
        internal List<MDL0PolygonNode> _infPolys = new List<MDL0PolygonNode>();
        internal List<MDL0PolygonNode> _manPolys = new List<MDL0PolygonNode>();
        [Category("Bone")]
        public MDL0PolygonNode[] Objects { get { return _manPolys.ToArray(); } }
        [Category("Bone")]
        public MDL0PolygonNode[] InfluencedObjects { get { return _infPolys.ToArray(); } }
        
        internal FrameState _bindState;
        public Matrix _bindMatrix, _inverseBindMatrix;
        internal FrameState _frameState;
        public Matrix _frameMatrix, _inverseFrameMatrix;

        private Vector3 _bMin, _bMax;
        internal int _nodeIndex, _weightCount, _refCount;

        [Category("Bone"), Browsable(true)]
        public Matrix Matrix { get { return _frameMatrix; } }
        [Category("Bone"), Browsable(true)]
        public Matrix InverseMatrix { get { return _inverseFrameMatrix; } }
        [Category("Bone"), Browsable(true), TypeConverter(typeof(MatrixStringConverter))]
        public Matrix BindMatrix { get { return _bindMatrix; } set { _bindMatrix = value; SignalPropertyChange(); } }
        [Category("Bone"), Browsable(true), TypeConverter(typeof(MatrixStringConverter))]
        public Matrix InverseBindMatrix { get { return _inverseBindMatrix; } set { _inverseBindMatrix = value; SignalPropertyChange(); } }

        [Browsable(false)]
        public int NodeIndex { get { return _nodeIndex; } }
        [Browsable(false)]
        public int ReferenceCount { get { return _refCount; } set { _refCount = value; } }
        [Browsable(false)]
        public bool IsPrimaryNode { get { return true; } }

        private BoneWeight[] _weightRef;
        [Browsable(false)]
        public BoneWeight[] Weights { get { return _weightRef == null ? _weightRef = new BoneWeight[] { new BoneWeight(this, 1.0f) } : _weightRef; } }

        [Category("Bone")]
        public int HeaderLen { get { return Header->_headerLen; } }
        [Category("Bone")]
        public int MDL0Offset { get { return Header->_mdl0Offset; } }
        [Category("Bone")]
        public int StringOffset { get { return Header->_stringOffset; } }

        public int _boneIndex;
        [Category("Bone")]
        public int BoneIndex { get { return _boneIndex; } }
        //public bool OverrideBoneIndex { get { return _override; } set { _override = value; } }
        //public bool _override = false;
        [Category("Bone")]
        public int NodeId { get { return Header->_nodeId; } }
        [Category("Bone")]
        public BoneFlags Flags { get { return _flags; } set { _flags = (BoneFlags)(int)value; SignalPropertyChange(); } }
        [Category("Bone")]
        public uint Pad1 { get { return Header->_pad1; } }
        [Category("Bone")]
        public uint Pad2 { get { return Header->_pad2; } }
        
        [Category("Bone"), TypeConverter(typeof(Vector3StringConverter))]
        public Vector3 Scale { get { return _bindState._scale; } set { _bindState.Scale = value; flagsChanged = true; SignalPropertyChange(); } }
        //[Category("Bone"), TypeConverter(typeof(Vector3StringConverter))]
        //public Quaternion QuaternionRotation { get { return _bindState._quaternion; } set { _bindState.QuaternionRotate = value; flagsChanged = true; SignalPropertyChange(); } }
        [Category("Bone"), TypeConverter(typeof(Vector3StringConverter))]
        public Vector3 Rotation { get { return _bindState._rotate; } set { _bindState.Rotate = value; flagsChanged = true; SignalPropertyChange(); } }
        [Category("Bone"), TypeConverter(typeof(Vector3StringConverter))]
        public Vector3 Translation { get { return _bindState._translate; } set { _bindState.Translate = value; flagsChanged = true; SignalPropertyChange(); } }
        [Category("Bone"), TypeConverter(typeof(Vector3StringConverter))]
        public Vector3 BoxMin { get { return _bMin; } set { _bMin = value; SignalPropertyChange(); } }
        [Category("Bone"), TypeConverter(typeof(Vector3StringConverter))]
        public Vector3 BoxMax { get { return _bMax; } set { _bMax = value; SignalPropertyChange(); } }

        [Category("Bone")]
        public int ParentOffset { get { return Header->_parentOffset / 0xD0; } }
        [Category("Bone")]
        public int FirstChildOffset { get { return Header->_firstChildOffset / 0xD0; } }
        [Category("Bone")]
        public int NextOffset { get { return Header->_nextOffset / 0xD0; } }
        [Category("Bone")]
        public int PrevOffset { get { return Header->_prevOffset / 0xD0; } }
        [Category("Bone")]
        public int Part2Offset { get { return Header->_part2Offset; } }

        internal bool flagsChanged = false;

        //[Category("Kinect Settings"), Browsable(true)]
        //public SkeletonJoint Joint
        //{
        //    get { return _joint; }
        //    set { _joint = value; }
        //}
        //public SkeletonJoint _joint;

        [Category("MDL0 Bone Part 2")]
        public string[] Entries { get { return _entries.ToArray(); } set { _entries = value.ToList<string>(); SignalPropertyChange(); } }

        internal override void GetStrings(StringTable table)
        {
            table.Add(Name);

            foreach (MDL0BoneNode n in Children)
                n.GetStrings(table);

            foreach (string s in _entries)
                table.Add(s);
        }

        //Initialize should only be called from parent group during parse.
        //Bones need not be imported/exported anyways
        protected override bool OnInitialize()
        {
            MDL0Bone* header = Header;

            SetSizeInternal(header->_headerLen);

            //Assign true parent using parent header offset
            int offset = header->_parentOffset;
            //Offsets are always < 0, because parent entries are listed before children
            if (offset < 0)
            {
                //Get address of parent header
                MDL0Bone* pHeader = (MDL0Bone*)((byte*)header + offset);
                //Search bone list for matching header
                foreach (MDL0BoneNode bone in _parent._children)
                    if (pHeader == bone.Header)
                    { _parent = bone; break; } //Assign parent and break
            }

            //Conditional name assignment
            if ((_name == null) && (header->_stringOffset != 0))
                _name = header->ResourceString;

            //Assign fields
            _flags = (BoneFlags)(uint)header->_flags;
            _nodeIndex = header->_nodeId;
            _boneIndex = header->_index;

            _permanentID = header->_index;

            _bindState = _frameState = new FrameState(header->_scale, header->_rotation, header->_translation);
            (_bindState._quaternion = new Quaternion()).FromEuler(header->_rotation);
            _bindMatrix = _frameMatrix = header->_transform;
            _inverseBindMatrix = _inverseFrameMatrix = header->_transformInv;

            _bMin = header->_boxMin;
            _bMax = header->_boxMax;

            if (header->_part2Offset != 0)
            {
                Part2Data* part2 = (Part2Data*)((byte*)header + header->_part2Offset);
                ResourceGroup* group = part2->Group;
                ResourceEntry* pEntry = &group->_first + 1;
                int count = group->_numEntries;
                for (int i = 0; i < count; i++)
                    _entries.Add(new String((sbyte*)group + (pEntry++)->_stringOffset));
            }
            //We don't want to process children because not all have been parsed yet.
            //Child assigning will be handled by the parent group.
            return false;
        }

        //Use MoveRaw without processing children.
        //Prevents addresses from changing before completion.
        //internal override void MoveRaw(VoidPtr address, int length)
        //{
        //    Memory.Move(address, WorkingSource.Address, (uint)length);
        //    DataSource newsrc = new DataSource(address, length);
        //    if (_compression == CompressionType.None)
        //    {
        //        _replSrc.Close();
        //        _replUncompSrc.Close();
        //        _replSrc = _replUncompSrc = newsrc;
        //    }
        //    else
        //    {
        //        _replSrc.Close();
        //        _replSrc = newsrc;
        //    }
        //}

        protected override int OnCalculateSize(bool force)
        {
            int len = 0xD0;
            if (_entries.Count > 0)
                len += 0x1C + (_entries.Count * 0x2C);
            return len;
        }

        public override void RemoveChild(ResourceNode child)
        {
            base.RemoveChild(child);
            Moved = true;
        }

        public void RecalcOffsets(MDL0Bone* header, VoidPtr address, int length)
        {
            MDL0BoneNode bone;
            int index = 0, offset;

            //Sub-entries
            if (_entries.Count > 0)
            {
                header->_part2Offset = 0xD0;
                *(bint*)((byte*)address + 0xD0) = 0x1C + (_entries.Count * 0x2C);
                ResourceGroup* pGroup = (ResourceGroup*)((byte*)address + 0xD4);
                ResourceEntry* pEntry = &pGroup->_first + 1;
                byte* pData = (byte*)pGroup + pGroup->_totalSize;

                *pGroup = new ResourceGroup(_entries.Count);

                foreach (string s in _entries)
                {
                    (pEntry++)->_dataOffset = (int)pData - (int)pGroup;
                    Part2DataEntry* p = (Part2DataEntry*)pData;
                    *p = new Part2DataEntry(1);
                    pData += 0x1C;
                }
            }
            else
                header->_part2Offset = 0;

            //Set first child
            if (_children.Count > 0)
                header->_firstChildOffset = length;
            else
                header->_firstChildOffset = 0;

            if (_parent != null)
            {
                index = _parent._children.IndexOf(this);

                //Parent
                if (_parent is MDL0BoneNode)
                    header->_parentOffset = (int)_parent.WorkingUncompressed.Address - (int)address;
                else
                    header->_parentOffset = 0;

                //Prev
                if (index == 0)
                    header->_prevOffset = 0;
                else
                {
                    //Link to prev
                    bone = _parent._children[index - 1] as MDL0BoneNode;
                    offset = (int)bone.Header - (int)address;
                    header->_prevOffset = offset;
                    bone.Header->_nextOffset = -offset;
                }

                //Next
                if (index == (_parent._children.Count - 1))
                    header->_nextOffset = 0;
            }
        }

        public void CalcFlags()
        {
            _flags = 0; //Reset flags

            _flags += (int)BoneFlags.Common5 + (int)BoneFlags.Visible;
            if (_refCount > 0)
                _flags += (int)BoneFlags.HasGeometry;
            if (Scale == new Vector3(1))
                _flags += (int)BoneFlags.FixedScale;
            if (Rotation == new Vector3(0))
                _flags += (int)BoneFlags.FixedRotation;
            if (Translation == new Vector3(0))
                _flags += (int)BoneFlags.FixedTranslation;

            if (Parent is MDL0BoneNode)
            {
                if ((BindMatrix == ((MDL0BoneNode)Parent).BindMatrix) && (InverseBindMatrix == ((MDL0BoneNode)Parent).InverseBindMatrix))
                    _flags += (int)BoneFlags.NoTransform;
            }
            else if (BindMatrix == Matrix.Identity && InverseBindMatrix == Matrix.Identity)
                _flags += (int)BoneFlags.NoTransform;

            flagsChanged = false;
        }

        protected internal override void OnRebuild(VoidPtr address, int length, bool force)
        {
            MDL0Bone* header = (MDL0Bone*)address;

            RecalcOffsets(header, address, length);

            if (_flags == 0 || flagsChanged)
                CalcFlags();

            header->_headerLen = length;

            //if (!_override)
                header->_index = _boneIndex = _entryIndex;// - Model._overrideCount;
            //else
            //    header->_index = _boneIndex = Model._linker.BoneCache.Length - 1 + Model._overrideCount++;
            
            header->_nodeId = _nodeIndex;
            header->_flags = (uint)_flags;
            header->_pad1 = 0;
            header->_pad2 = 0;
            header->_scale = _bindState._scale;
            header->_rotation = _bindState._rotate;
            header->_translation = _bindState._translate;
            header->_boxMin = _bMin;
            header->_boxMax = _bMax;

            //if (_bindMatrix != _frameMatrix)
            //{
            //    header->_transform = (bMatrix43)_frameMatrix;
            //    header->_transformInv = (bMatrix43)_inverseFrameMatrix;
            //}
            //else
            //{
                header->_transform = (bMatrix43)_bindMatrix;
                header->_transformInv = (bMatrix43)_inverseBindMatrix;
            //}

            _moved = false;
        }

        protected internal override void PostProcess(VoidPtr mdlAddress, VoidPtr dataAddress, StringTable stringTable)
        {
            MDL0Bone* header = (MDL0Bone*)dataAddress;
            header->_mdl0Offset = (int)mdlAddress - (int)dataAddress;
            header->_stringOffset = (int)stringTable[Name] + 4 - (int)dataAddress;

            //Entry strings
            if (_entries.Count > 0)
            {
                ResourceGroup* pGroup = (ResourceGroup*)((byte*)header + header->_part2Offset + 4);
                ResourceEntry* pEntry = &pGroup->_first;
                int count = pGroup->_numEntries;
                (*pEntry++) = new ResourceEntry(0xFFFF, 0, 0, 0, 0);

                for (int i = 0; i < count; i++)
                {
                    Part2DataEntry* entry = (Part2DataEntry*)((byte*)pGroup + (pEntry++)->_dataOffset);
                    entry->_stringOffset = (int)stringTable[_entries[i]] + 4 - ((int)entry + (int)dataAddress);
                    ResourceEntry.Build(pGroup, i + 1, entry, (BRESString*)stringTable[_entries[i]]);
                }
            }
        }

        internal void GetBindState()
        {
            if (_parent is MDL0BoneNode)
            {
                _bindState._transform = _bindMatrix / ((MDL0BoneNode)_parent)._bindMatrix;
                _bindState._iTransform = _inverseBindMatrix / ((MDL0BoneNode)_parent)._inverseBindMatrix;
            }
            else
            {
                _bindState._transform = _bindMatrix;
                _bindState._iTransform = _inverseBindMatrix;
            }

            foreach (MDL0BoneNode bone in Children)
                bone.GetBindState();
        }

        //Change has been made to bind state, need to recalculate matrices
        internal void RecalcBindState()
        {
            if (_parent is MDL0BoneNode)
            {
                _bindMatrix = ((MDL0BoneNode)_parent)._bindMatrix * _bindState._transform;
                _inverseBindMatrix = _bindState._iTransform * ((MDL0BoneNode)_parent)._inverseBindMatrix;
            }
            else
            {
                _bindMatrix = _bindState._transform;
                _inverseBindMatrix = _bindState._iTransform;
            }
            
            foreach (MDL0BoneNode bone in Children)
                bone.RecalcBindState();
        }
        internal void RecalcFrameState()
        {
            if (_parent is MDL0BoneNode)
            {
                _frameMatrix = ((MDL0BoneNode)_parent)._frameMatrix * _frameState._transform;
                _inverseFrameMatrix = _frameState._iTransform * ((MDL0BoneNode)_parent)._inverseFrameMatrix;

                //_frameMatrix = ((MDL0BoneNode)_parent)._frameMatrix * _frameState._quatTransform;
                //_inverseFrameMatrix = _frameState._quatiTransform * ((MDL0BoneNode)_parent)._inverseFrameMatrix;
            }
            else
            {
                _frameMatrix = _frameState._transform;
                _inverseFrameMatrix = _frameState._iTransform;

                //_frameMatrix = _frameState._quatTransform;
                //_inverseFrameMatrix = _frameState._quatiTransform;
            }

            foreach (MDL0BoneNode bone in Children)
                bone.RecalcFrameState();
        }

        internal unsafe List<MDL0BoneNode> ChildTree(List<MDL0BoneNode> list)
        {
            list.Add(this);
            foreach (MDL0BoneNode c in _children)
                c.ChildTree(list);
            
            return list;
        }

        internal Vector3 RecursiveScale()
        {
            if (_parent is MDL0GroupNode)
                return _frameState._scale;
            
            return _frameState._scale * ((MDL0BoneNode)_parent).RecursiveScale();
        }

        #region Rendering

        public static Color DefaultBoneColor = Color.FromArgb(0, 0, 128);
        public static Color DefaultNodeColor = Color.FromArgb(0, 128, 0);

        internal Color _boneColor = Color.Transparent;
        internal Color _nodeColor = Color.Transparent;

        public const float _nodeRadius = 0.20f;

        public bool _render = true;
        internal unsafe void Render(GLContext ctx)
        {
            if (!_render)
                return;

            if (_boneColor != Color.Transparent)
                ctx.glColor(_boneColor.R, _boneColor.G, _boneColor.B, _boneColor.A);
            else
                ctx.glColor(DefaultBoneColor.R, DefaultBoneColor.G, DefaultBoneColor.B, DefaultBoneColor.A);

            Vector3 v = _frameState._translate;

            ctx.glBegin(GLPrimitiveType.Lines);

            ctx.glVertex(0.0f, 0.0f, 0.0f);
            ctx.glVertex3v((float*)&v);

            ctx.glEnd();

            ctx.glPushMatrix();

            ctx.glTranslate(v._x, v._y, v._z);

            //Render node
            GLDisplayList ndl = ctx.FindOrCreate<GLDisplayList>("BoneNodeOrb", CreateNodeOrb);
            if (_nodeColor != Color.Transparent)
                ctx.glColor(_nodeColor.R, _nodeColor.G, _nodeColor.B, _nodeColor.A);
            else
                ctx.glColor(DefaultNodeColor.R, DefaultNodeColor.G, DefaultNodeColor.B, DefaultNodeColor.A);
            
            ndl.Call();
            
            DrawNodeOrients(ctx);

            ctx.glTranslate(-v._x, -v._y, -v._z);

            //Transform Bones
            fixed (Matrix* m = &_frameState._transform)
                ctx.glMultMatrix((float*)m);

            //Render children
            foreach (MDL0BoneNode n in Children)
                n.Render(ctx);

            ctx.glPopMatrix();
        }

        internal void ApplyCHR0(CHR0Node node, int index)
        {
            CHR0EntryNode e;

            if ((node == null) || (index == 0)) //Reset to bind pose
                _frameState = _bindState;
            else if ((e = node.FindChild(Name, false) as CHR0EntryNode) != null) //Set to anim pose
                _frameState = new FrameState(e.GetAnimFrame(index - 1));
            else //Set to neutral pose
                _frameState = _bindState;

            foreach (MDL0BoneNode b in Children)
                b.ApplyCHR0(node, index);
        }

        public static GLDisplayList CreateNodeOrb(GLContext ctx)
        {
            GLDisplayList circle = ctx.GetRingList();
            GLDisplayList orb = new GLDisplayList(ctx);

            orb.Begin();
            ctx.glPushMatrix();

            ctx.glScale(_nodeRadius, _nodeRadius, _nodeRadius);
            circle.Call();
            ctx.glRotate(90.0f, 0.0f, 1.0f, 0.0f);
            circle.Call();
            ctx.glRotate(90.0f, 1.0f, 0.0f, 0.0f);
            circle.Call();

            ctx.glPopMatrix();
            orb.End();
            return orb;
        }

        public static void DrawNodeOrients(GLContext ctx)
        {
            ctx.glBegin(GLPrimitiveType.Lines);

            ctx.glColor(1.0f, 0.0f, 0.0f, 1.0f);
            ctx.glVertex(0.0f, 0.0f, 0.0f);
            ctx.glVertex(_nodeRadius * 2, 0.0f, 0.0f);

            ctx.glColor(0.0f, 1.0f, 0.0f, 1.0f);
            ctx.glVertex(0.0f, 0.0f, 0.0f);
            ctx.glVertex(0.0f, _nodeRadius * 2, 0.0f);

            ctx.glColor(0.0f, 0.0f, 1.0f, 1.0f);
            ctx.glVertex(0.0f, 0.0f, 0.0f);
            ctx.glVertex(0.0f, 0.0f, _nodeRadius * 2);

            ctx.glEnd();
        }

        internal override void Bind(GLContext ctx)
        {
            _render = true;
            _boneColor = Color.Transparent;
            _nodeColor = Color.Transparent;
        }

        #endregion
    }
}
