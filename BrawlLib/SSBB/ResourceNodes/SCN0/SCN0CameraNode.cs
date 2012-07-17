using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.SSBBTypes;
using System.ComponentModel;
using BrawlLib.Imaging;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class SCN0CameraNode : SCN0EntryNode
    {
        internal SCN0Camera* Data { get { return (SCN0Camera*)WorkingUncompressed.Address; } }

        private uint _flags1, _flags2;
        private Vector3 vec1, vec2, vec3, vec4, vec5;
        private List<SCN0Keyframe> v1x, v1y, v1z, v2x, v2y, v2z, v3x, v3y, v3z, v4x, v4y, v4z;

        //Flags 1
        //1110 0000 0000 0000 - Vec2 Flags
        //0000 1110 0000 0000 - Vec3 Flags
        //0000 0000 1110 0000 - Vec4 Flags
        //0000 0000 0000 1110 - Vec1 Flags

        //Flags 2
        //0000 0000 0000 0001 - Use Vec3
        //0000 0000 0000 0010 - Use Vec4

        [Category("Camera"), TypeConverter(typeof(Bin16StringConverter))]
        public Bin16 Flags1 { get { return new Bin16((ushort)_flags1); } set { _flags1 = value.data; SignalPropertyChange(); } }
        [Category("Camera")]
        public SCN0CameraFlags2 Flags2 { get { return (SCN0CameraFlags2)_flags2; } set { _flags2 = (ushort)value; SignalPropertyChange(); } }

        [Category("Camera Vector 1"), Browsable(true)]
        public SCN0CameraVectorFlags Vec1Flags { get { return (SCN0CameraVectorFlags)(_flags1 & 0xF); } set { _flags1 = (((_flags1 & ~((uint)value))) | ((uint)value & (0xF))); SignalPropertyChange(); } }
        [Category("Camera Vector 1")]
        public List<SCN0Keyframe> Vec1X { get { return v1x; } set { v1x = value; SignalPropertyChange(); } }
        [Category("Camera Vector 1")]
        public List<SCN0Keyframe> Vec1Y { get { return v1y; } set { v1y = value; SignalPropertyChange(); } }
        [Category("Camera Vector 1")]
        public List<SCN0Keyframe> Vec1Z { get { return v1z; } set { v1z = value; SignalPropertyChange(); } }

        [Category("Camera Vector 2"), Browsable(true)]
        public SCN0CameraVectorFlags Vec2Flags { get { return (SCN0CameraVectorFlags)(_flags1 >> 12 & 0xF); } set { _flags1 = (((_flags1 & ~((uint)value << 12))) | ((uint)value << 12 & (0xF << 12))); SignalPropertyChange(); } }
        [Category("Camera Vector 2")]
        public List<SCN0Keyframe> Vec2X { get { return v2x; } set { v2x = value; SignalPropertyChange(); } }
        [Category("Camera Vector 2")]
        public List<SCN0Keyframe> Vec2Y { get { return v2y; } set { v2y = value; SignalPropertyChange(); } }
        [Category("Camera Vector 2")]
        public List<SCN0Keyframe> Vec2Z { get { return v2z; } set { v2z = value; SignalPropertyChange(); } }

        [Category("Camera Vector 3"), Browsable(true)]
        public SCN0CameraVectorFlags Vec3Flags { get { return (SCN0CameraVectorFlags)(_flags1 >> 8 & 0xF); } set { _flags1 = (((_flags1 & ~((uint)value << 8))) | ((uint)value << 18 & (0xF << 8))); SignalPropertyChange(); } }
        [Category("Camera Vector 3")]
        public List<SCN0Keyframe> Vec3X { get { return v3x; } set { v3x = value; SignalPropertyChange(); } }
        [Category("Camera Vector 3")]
        public List<SCN0Keyframe> Vec3Y { get { return v3y; } set { v3y = value; SignalPropertyChange(); } }
        [Category("Camera Vector 3")]
        public List<SCN0Keyframe> Vec3Z { get { return v3z; } set { v3z = value; SignalPropertyChange(); } }

        [Category("Camera Vector 4"), Browsable(true)]
        public SCN0CameraVectorFlags Vec4Flags { get { return (SCN0CameraVectorFlags)(_flags1 >> 4 & 0xF); } set { _flags1 = (((_flags1 & ~((uint)value << 4))) | ((uint)value << 4 & (0xF << 4))); SignalPropertyChange(); } }
        [Category("Camera Vector 4")]
        public List<SCN0Keyframe> Vec4X { get { return v4x; } set { v4x = value; SignalPropertyChange(); } }
        [Category("Camera Vector 4")]
        public List<SCN0Keyframe> Vec4Y { get { return v4y; } set { v4y = value; SignalPropertyChange(); } }
        [Category("Camera Vector 4")]
        public List<SCN0Keyframe> Vec4Z { get { return v4z; } set { v4z = value; SignalPropertyChange(); } }
        
        [Category("Camera Settings")]
        public float FieldOfView { get { return vec2._x; } set { vec2._x = value; SignalPropertyChange(); } }
        [Category("Camera Settings")]
        public float NearZ { get { return vec2._y; } set { vec2._y = value; SignalPropertyChange(); } }
        [Category("Camera Settings")]
        public float FarZ { get { return vec2._z; } set { vec2._z = value; SignalPropertyChange(); } }

        //[Category("Camera Vector 1"), TypeConverter(typeof(Vector3StringConverter))]
        //public Vector3 Vec1 { get { return vec1; } set { vec1 = value; SignalPropertyChange(); } }
        //[Category("Camera Vector 2"), TypeConverter(typeof(Vector3StringConverter))]
        //public Vector3 Vec2 { get { return vec3; } set { vec3 = value; SignalPropertyChange(); } }
        //[Category("Camera Vector 3"), TypeConverter(typeof(Vector3StringConverter))]
        //public Vector3 Vec3 { get { return vec4; } set { vec4 = value; SignalPropertyChange(); } }
        //[Category("Camera Vector 4"), TypeConverter(typeof(Vector3StringConverter))]
        //public Vector3 Vec4 { get { return vec5; } set { vec5 = value; SignalPropertyChange(); } }
        
        protected override bool OnInitialize()
        {
            base.OnInitialize();

            _flags1 = Data->_flags1;
            _flags2 = Data->_flags2;

            vec1 = Data->_vec1;
            vec2 = Data->_camSettings;
            vec3 = Data->_vec2;
            vec4 = Data->_vec3;
            vec5 = Data->_vec4;

            v1x = new List<SCN0Keyframe>();
            v1y = new List<SCN0Keyframe>();
            v1z = new List<SCN0Keyframe>();
            v2x = new List<SCN0Keyframe>();
            v2y = new List<SCN0Keyframe>();
            v2z = new List<SCN0Keyframe>();
            v3x = new List<SCN0Keyframe>();
            v3y = new List<SCN0Keyframe>();
            v3z = new List<SCN0Keyframe>();
            v4x = new List<SCN0Keyframe>();
            v4y = new List<SCN0Keyframe>();
            v4z = new List<SCN0Keyframe>();

            if (Vec1Flags.HasFlag(SCN0CameraVectorFlags.FixedX))
                v1x.Add(new Vector3(0, 0, Data->_vec1._x));
            else
            {
                if (Name != "<null>")
                {
                    SCN0KeyframesHeader* keysHeader = Data->v1xKfs;
                    SCN0KeyframeStruct* addr = keysHeader->Data;
                    for (int i = 0; i < keysHeader->_numFrames; i++)
                        v1x.Add(*addr++);
                }
            }
            if (Vec1Flags.HasFlag(SCN0CameraVectorFlags.FixedY))
                v1y.Add(new Vector3(0, 0, Data->_vec1._y));
            else
            {
                if (Name != "<null>")
                {
                    SCN0KeyframesHeader* keysHeader = Data->v1yKfs;
                    SCN0KeyframeStruct* addr = keysHeader->Data;
                    for (int i = 0; i < keysHeader->_numFrames; i++)
                        v1y.Add(*addr++);
                }
            }
            if (Vec1Flags.HasFlag(SCN0CameraVectorFlags.FixedZ))
                v1z.Add(new Vector3(0, 0, Data->_vec1._z));
            else
            {
                if (Name != "<null>")
                {
                    SCN0KeyframesHeader* keysHeader = Data->v1zKfs;
                    SCN0KeyframeStruct* addr = keysHeader->Data;
                    for (int i = 0; i < keysHeader->_numFrames; i++)
                        v1z.Add(*addr++);
                }
            }
            if (Vec2Flags.HasFlag(SCN0CameraVectorFlags.FixedX))
                v2x.Add(new Vector3(0, 0, Data->_vec2._x));
            else
            {
                if (Name != "<null>")
                {
                    SCN0KeyframesHeader* keysHeader = Data->v2xKfs;
                    SCN0KeyframeStruct* addr = keysHeader->Data;
                    for (int i = 0; i < keysHeader->_numFrames; i++)
                        v2x.Add(*addr++);
                }
            }
            if (Vec2Flags.HasFlag(SCN0CameraVectorFlags.FixedY))
                v2y.Add(new Vector3(0, 0, Data->_vec2._y));
            else
            {
                if (Name != "<null>")
                {
                    SCN0KeyframesHeader* keysHeader = Data->v2yKfs;
                    SCN0KeyframeStruct* addr = keysHeader->Data;
                    for (int i = 0; i < keysHeader->_numFrames; i++)
                        v2y.Add(*addr++);
                }
            }
            if (Vec2Flags.HasFlag(SCN0CameraVectorFlags.FixedZ))
                v2z.Add(new Vector3(0, 0, Data->_vec2._z));
            else
            {
                if (Name != "<null>")
                {
                    SCN0KeyframesHeader* keysHeader = Data->v2zKfs;
                    SCN0KeyframeStruct* addr = keysHeader->Data;
                    for (int i = 0; i < keysHeader->_numFrames; i++)
                        v2z.Add(*addr++);
                }
            }
            if (Vec3Flags.HasFlag(SCN0CameraVectorFlags.FixedX))
                v3x.Add(new Vector3(0, 0, Data->_vec3._x));
            else
            {
                if (Name != "<null>")
                {
                    SCN0KeyframesHeader* keysHeader = Data->v3xKfs;
                    SCN0KeyframeStruct* addr = keysHeader->Data;
                    for (int i = 0; i < keysHeader->_numFrames; i++)
                        v3x.Add(*addr++);
                }
            }
            if (Vec3Flags.HasFlag(SCN0CameraVectorFlags.FixedY))
                v3y.Add(new Vector3(0, 0, Data->_vec3._y));
            else
            {
                if (Name != "<null>")
                {
                    SCN0KeyframesHeader* keysHeader = Data->v3yKfs;
                    SCN0KeyframeStruct* addr = keysHeader->Data;
                    for (int i = 0; i < keysHeader->_numFrames; i++)
                        v3y.Add(*addr++);
                }
            }
            if (Vec3Flags.HasFlag(SCN0CameraVectorFlags.FixedZ))
                v3z.Add(new Vector3(0, 0, Data->_vec3._z));
            else
            {
                if (Name != "<null>")
                {
                    SCN0KeyframesHeader* keysHeader = Data->v3zKfs;
                    SCN0KeyframeStruct* addr = keysHeader->Data;
                    for (int i = 0; i < keysHeader->_numFrames; i++)
                        v3z.Add(*addr++);
                }
            }
            if (Vec4Flags.HasFlag(SCN0CameraVectorFlags.FixedX))
                v4x.Add(new Vector3(0, 0, Data->_vec4._x));
            else
            {
                if (Name != "<null>")
                {
                    SCN0KeyframesHeader* keysHeader = Data->v4xKfs;
                    SCN0KeyframeStruct* addr = keysHeader->Data;
                    for (int i = 0; i < keysHeader->_numFrames; i++)
                        v4x.Add(*addr++);
                }
            }
            if (Vec4Flags.HasFlag(SCN0CameraVectorFlags.FixedZ)) //Z for Y?
                v4y.Add(new Vector3(0, 0, Data->_vec4._y));
            else
            {
                if (Name != "<null>")
                {
                    SCN0KeyframesHeader* keysHeader = Data->v4yKfs;
                    SCN0KeyframeStruct* addr = keysHeader->Data;
                    for (int i = 0; i < keysHeader->_numFrames; i++)
                        v4y.Add(*addr++);
                }
            }
            if (Vec4Flags.HasFlag(SCN0CameraVectorFlags.FixedY)) //Y for Z?
                v4z.Add(new Vector3(0, 0, Data->_vec4._z));
            else
            {
                if (Name != "<null>")
                {
                    SCN0KeyframesHeader* keysHeader = Data->v4zKfs;
                    SCN0KeyframeStruct* addr = keysHeader->Data;
                    for (int i = 0; i < keysHeader->_numFrames; i++)
                        v4z.Add(*addr++);
                }
            }
            return false;
        }

        protected override int OnCalculateSize(bool force)
        {
            lightLen = 0;
            keyLen = 0;
            if (_name != "<null>")
            {
                if (v1x.Count > 1)
                    keyLen += 4 + v1x.Count * 12;
                if (v1y.Count > 1)
                    keyLen += 4 + v1y.Count * 12;
                if (v1z.Count > 1)
                    keyLen += 4 + v1z.Count * 12;

                if (v2x.Count > 1)
                    keyLen += 4 + v2x.Count * 12;
                if (v2y.Count > 1)
                    keyLen += 4 + v2y.Count * 12;
                if (v2z.Count > 1)
                    keyLen += 4 + v2z.Count * 12;

                if (v3x.Count > 1)
                    keyLen += 4 + v3x.Count * 12;
                if (v3y.Count > 1)
                    keyLen += 4 + v3y.Count * 12;
                if (v3z.Count > 1)
                    keyLen += 4 + v3z.Count * 12;

                if (v4x.Count > 1)
                    keyLen += 4 + v4x.Count * 12;
                if (v4y.Count > 1)
                    keyLen += 4 + v4y.Count * 12;
                if (v4z.Count > 1)
                    keyLen += 4 + v4z.Count * 12;
            }
            return SCN0Camera.Size;
        }

        protected internal override void OnRebuild(VoidPtr address, int length, bool force)
        {
            base.OnRebuild(address, length, force);

            SCN0Camera* header = (SCN0Camera*)address;
            header->_pad1 = 0;
            header->_flags2 = (ushort)_flags2;
            header->_pad2 = 0;
            header->_camSettings = vec2;

            Bin16 flags = new Bin16();

            flags[0] = Vec1Flags.HasFlag(SCN0CameraVectorFlags.Unknown);
            if (v1x.Count > 1)
            {
                *((bint*)header->_vec1._x.Address) = (int)keyframeAddr - (int)header->_vec1._x.Address;
                ((SCN0KeyframesHeader*)keyframeAddr)->_numFrames = (ushort)v1x.Count;
                SCN0KeyframeStruct* addr = ((SCN0KeyframesHeader*)keyframeAddr)->Data;
                for (int i = 0; i < v1x.Count; i++)
                    *addr++ = v1x[i];
                keyframeAddr += 4 + v1x.Count * 12;
                flags[1] = false;
            }
            else
            {
                flags[1] = true;
                if (v1x.Count == 1)
                    header->_vec1._x = v1x[0]._value;
                else
                    header->_vec1._x = 0;
            }
            if (v1y.Count > 1)
            {
                *((bint*)header->_vec1._y.Address) = (int)keyframeAddr - (int)header->_vec1._y.Address;
                ((SCN0KeyframesHeader*)keyframeAddr)->_numFrames = (ushort)v1y.Count;
                SCN0KeyframeStruct* addr = ((SCN0KeyframesHeader*)keyframeAddr)->Data;
                for (int i = 0; i < v1y.Count; i++)
                    *addr++ = v1y[i];
                keyframeAddr += 4 + v1y.Count * 12;
                flags[2] = false;
            }
            else
            {
                flags[2] = true;
                if (v1y.Count == 1)
                    header->_vec1._y = v1y[0]._value;
                else
                    header->_vec1._y = 0;
            }
            if (v1z.Count > 1)
            {
                *((bint*)header->_vec1._z.Address) = (int)keyframeAddr - (int)header->_vec1._z.Address;
                ((SCN0KeyframesHeader*)keyframeAddr)->_numFrames = (ushort)v1z.Count;
                SCN0KeyframeStruct* addr = ((SCN0KeyframesHeader*)keyframeAddr)->Data;
                for (int i = 0; i < v1z.Count; i++)
                    *addr++ = v1z[i];
                keyframeAddr += 4 + v1z.Count * 12;
                flags[3] = false;
            }
            else
            {
                flags[3] = true;
                if (v1z.Count == 1)
                    header->_vec1._z = v1z[0]._value;
                else
                    header->_vec1._z = 0;
            }

            flags[4] = Vec4Flags.HasFlag(SCN0CameraVectorFlags.Unknown);
            if (v4x.Count > 1)
            {
                *((bint*)header->_vec4._x.Address) = (int)keyframeAddr - (int)header->_vec4._x.Address;
                ((SCN0KeyframesHeader*)keyframeAddr)->_numFrames = (ushort)v4x.Count;
                SCN0KeyframeStruct* addr = ((SCN0KeyframesHeader*)keyframeAddr)->Data;
                for (int i = 0; i < v4x.Count; i++)
                    *addr++ = v4x[i];
                keyframeAddr += 4 + v4x.Count * 12;
                flags[5] = false;
            }
            else
            {
                flags[5] = true;
                if (v4x.Count == 1)
                    header->_vec4._x = v4x[0]._value;
                else
                    header->_vec4._x = 0;
            }
            if (v4y.Count > 1)
            {
                *((bint*)header->_vec4._y.Address) = (int)keyframeAddr - (int)header->_vec4._y.Address;
                ((SCN0KeyframesHeader*)keyframeAddr)->_numFrames = (ushort)v4y.Count;
                SCN0KeyframeStruct* addr = ((SCN0KeyframesHeader*)keyframeAddr)->Data;
                for (int i = 0; i < v4y.Count; i++)
                    *addr++ = v4y[i];
                keyframeAddr += 4 + v4y.Count * 12;
                flags[7] = false;
            }
            else
            {
                flags[7] = true;
                if (v4y.Count == 1)
                    header->_vec4._y = v4y[0]._value;
                else
                    header->_vec4._y = 0;
            }
            if (v4z.Count > 1)
            {
                *((bint*)header->_vec4._z.Address) = (int)keyframeAddr - (int)header->_vec4._z.Address;
                ((SCN0KeyframesHeader*)keyframeAddr)->_numFrames = (ushort)v4z.Count;
                SCN0KeyframeStruct* addr = ((SCN0KeyframesHeader*)keyframeAddr)->Data;
                for (int i = 0; i < v4z.Count; i++)
                    *addr++ = v4z[i];
                keyframeAddr += 4 + v4z.Count * 12;
                flags[6] = false;
            }
            else
            {
                flags[6] = true;
                if (v4z.Count == 1)
                    header->_vec4._z = v4z[0]._value;
                else
                    header->_vec4._z = 0;
            }

            flags[8] = Vec3Flags.HasFlag(SCN0CameraVectorFlags.Unknown);
            if (v3x.Count > 1)
            {
                *((bint*)header->_vec3._x.Address) = (int)keyframeAddr - (int)header->_vec3._x.Address;
                ((SCN0KeyframesHeader*)keyframeAddr)->_numFrames = (ushort)v3x.Count;
                SCN0KeyframeStruct* addr = ((SCN0KeyframesHeader*)keyframeAddr)->Data;
                for (int i = 0; i < v3x.Count; i++)
                    *addr++ = v3x[i];
                keyframeAddr += 4 + v3x.Count * 12;
                flags[9] = false;
            }
            else
            {
                flags[9] = true;
                if (v3x.Count == 1)
                    header->_vec3._x = v3x[0]._value;
                else
                    header->_vec3._x = 0;
            }
            if (v3y.Count > 1)
            {
                *((bint*)header->_vec3._y.Address) = (int)keyframeAddr - (int)header->_vec3._y.Address;
                ((SCN0KeyframesHeader*)keyframeAddr)->_numFrames = (ushort)v3y.Count;
                SCN0KeyframeStruct* addr = ((SCN0KeyframesHeader*)keyframeAddr)->Data;
                for (int i = 0; i < v3y.Count; i++)
                    *addr++ = v3y[i];
                keyframeAddr += 4 + v3y.Count * 12;
                flags[10] = false;
            }
            else
            {
                flags[10] = true;
                if (v3y.Count == 1)
                    header->_vec3._y = v3y[0]._value;
                else
                    header->_vec3._y = 0;
            }
            if (v3z.Count > 1)
            {
                *((bint*)header->_vec3._z.Address) = (int)keyframeAddr - (int)header->_vec3._z.Address;
                ((SCN0KeyframesHeader*)keyframeAddr)->_numFrames = (ushort)v3z.Count;
                SCN0KeyframeStruct* addr = ((SCN0KeyframesHeader*)keyframeAddr)->Data;
                for (int i = 0; i < v3z.Count; i++)
                    *addr++ = v3z[i];
                keyframeAddr += 4 + v3z.Count * 12;
                flags[11] = false;
            }
            else
            {
                flags[11] = true;
                if (v3z.Count == 1)
                    header->_vec3._z = v3z[0]._value;
                else
                    header->_vec3._z = 0;
            }

            flags[12] = Vec2Flags.HasFlag(SCN0CameraVectorFlags.Unknown);
            if (v2x.Count > 1)
            {
                *((bint*)header->_vec2._x.Address) = (int)keyframeAddr - (int)header->_vec2._x.Address;
                ((SCN0KeyframesHeader*)keyframeAddr)->_numFrames = (ushort)v2x.Count;
                SCN0KeyframeStruct* addr = ((SCN0KeyframesHeader*)keyframeAddr)->Data;
                for (int i = 0; i < v2x.Count; i++)
                    *addr++ = v2x[i];
                keyframeAddr += 4 + v2x.Count * 12;
                flags[13] = false;
            }
            else
            {
                flags[13] = true;
                if (v2x.Count == 1)
                    header->_vec2._x = v2x[0]._value;
                else
                    header->_vec2._x = 0;
            }
            if (v2y.Count > 1)
            {
                *((bint*)header->_vec2._y.Address) = (int)keyframeAddr - (int)header->_vec2._y.Address;
                ((SCN0KeyframesHeader*)keyframeAddr)->_numFrames = (ushort)v2y.Count;
                SCN0KeyframeStruct* addr = ((SCN0KeyframesHeader*)keyframeAddr)->Data;
                for (int i = 0; i < v2y.Count; i++)
                    *addr++ = v2y[i];
                keyframeAddr += 4 + v2y.Count * 12;
                flags[14] = false;
            }
            else
            {
                flags[14] = true;
                if (v2y.Count == 1)
                    header->_vec2._y = v2y[0]._value;
                else
                    header->_vec2._y = 0;
            }
            if (v2z.Count > 1)
            {
                *((bint*)header->_vec2._z.Address) = (int)keyframeAddr - (int)header->_vec2._z.Address;
                ((SCN0KeyframesHeader*)keyframeAddr)->_numFrames = (ushort)v2z.Count;
                SCN0KeyframeStruct* addr = ((SCN0KeyframesHeader*)keyframeAddr)->Data;
                for (int i = 0; i < v2z.Count; i++)
                    *addr++ = v2z[i];
                keyframeAddr += 4 + v2z.Count * 12;
                flags[15] = false;
            }
            else
            {
                flags[15] = true;
                if (v2z.Count == 1)
                    header->_vec2._z = v2z[0]._value;
                else
                    header->_vec1._z = 0;
            }

            header->_flags1 = flags.data;
        }

        protected internal override void PostProcess(VoidPtr scn0Address, VoidPtr dataAddress, StringTable stringTable)
        {
            base.PostProcess(scn0Address, dataAddress, stringTable);
        }
    }
}
