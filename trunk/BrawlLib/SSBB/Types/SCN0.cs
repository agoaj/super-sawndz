using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using BrawlLib.Imaging;
using BrawlLib.Wii.Graphics;

namespace BrawlLib.SSBBTypes
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct SCN0
    {
        public const uint Tag = 0x304E4353;
        public const int Size = 0x44;

        public BRESCommonHeader _header;
        public bint _dataOffset;
        public bint _lightSetOffset;
        public bint _ambLightOffset;
        public bint _lightOffset;
        public bint _fogOffset;
        public bint _cameraOffset;
        public bint _stringOffset;
        public bint _unk1; //v5 stringOffset
        public bshort _frameCount;
        public bshort _unk3;
        public bint _unk4;
        public bshort _lightSetCount;
        public bshort _ambientCount;
        public bshort _lightCount;
        public bshort _fogCount;
        public bshort _cameraCount;
        public bshort _unk10;

        public void Set(int groupLen, int lightSetLen, int ambLightLen, int lightLen, int fogLen, int cameraLen)
        {
            _dataOffset = Size;

            _header._tag = Tag;
            _header._version = 4;
            _header._bresOffset = 0;

            _lightSetOffset = _dataOffset + groupLen;
            _ambLightOffset = _lightSetOffset + lightSetLen;
            _lightOffset = _ambLightOffset + ambLightLen;
            _fogOffset = _lightOffset + lightLen;
            _cameraOffset = _fogOffset + fogLen;
            _header._size = _cameraOffset + cameraLen;

            if (lightSetLen == 0) _lightSetOffset = 0;
            if (ambLightLen == 0) _ambLightOffset = 0;
            if (lightLen == 0) _lightOffset = 0;
            if (fogLen == 0) _fogOffset = 0;
            if (cameraLen == 0) _cameraOffset = 0;
        }

        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }

        public ResourceGroup* Group { get { return (ResourceGroup*)(Address + _dataOffset); } }

        public SCN0LightSet* LightSets { get { return (SCN0LightSet*)(Address + _lightSetOffset); } }
        public SCN0AmbientLight* AmbientLights { get { return (SCN0AmbientLight*)(Address + _ambLightOffset); } }
        public SCN0Light* Lights { get { return (SCN0Light*)(Address + _lightOffset); } }
        public SCN0Fog* Fogs { get { return (SCN0Fog*)(Address + _fogOffset); } }
        public SCN0Camera* Cameras { get { return (SCN0Camera*)(Address + _cameraOffset); } }

        public string ResourceString { get { return new String((sbyte*)this.ResourceStringAddress); } }
        public VoidPtr ResourceStringAddress
        {
            get { return (VoidPtr)this.Address + _stringOffset; }
            set { _stringOffset = (int)value - (int)Address; }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct SCN0CommonHeader
    {
        public const int Size = 0x14;

        public bint _length;
        public bint _scn0Offset;
        public bint _stringOffset;
        public bint _nodeIndex;
        public bint _realIndex;

        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }

        public string ResourceString { get { return new String((sbyte*)ResourceStringAddress); } }
        public VoidPtr ResourceStringAddress
        {
            get { return (VoidPtr)Address + _stringOffset; }
            set { _stringOffset = (int)value - (int)Address; }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct SCN0LightSet
    {
        public const int Size = 76;

        public SCN0CommonHeader _header;

        public bint _ambNameOffset;
        public bshort _magic; //0xFFFF
        public byte _numLights;
        public byte _unk1;
        public fixed int _entries[8]; //string offsets

        public bint _pad1, _pad2, _pad3, _pad4; //0xFFFFFFFF

        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }
        public bint* Offsets { get { fixed (void* ptr = _entries)return (bint*)ptr; } }

        public string AmbientString { get { return new String((sbyte*)AmbientStringAddress); } }
        public VoidPtr AmbientStringAddress
        {
            get { return (VoidPtr)Address + _ambNameOffset; }
            set { _ambNameOffset = (int)value - (int)Address; }
        }

        public bint* StringOffsets { get { return (bint*)(Address + 0x1C); } }
    }

    [Flags]
    public enum SCN0AmbLightFlags
    {
        None = 0,
        FixedLighting = 128,
    }

    [Flags]
    public enum SCN0AmbLightEnableFlags
    {
        None = 0,
        Enabled = 1,
        UseLightSet = 2
    }

    [Flags]
    public enum SCN0LightsKeyframes
    {
        FixedX = 0,
        FixedY = 1,
        FixedZ = 2
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct SCN0AmbientLight
    {
        public const int Size = 28;

        public SCN0CommonHeader _header;

        public byte _fixedFlags; //0x80
        public byte _unk2; //0x00
        public byte _unk3; //0x00
        public byte _unk4; //0x03, entries?

        public RGBAPixel _lighting;

        public RGBAPixel* lightEntries { get { return (RGBAPixel*)((byte*)Address + 24 + *(bint*)((byte*)Address + 24)); } }
        
        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct SCN0Light
    {
        public const int Size = 92;

        public SCN0CommonHeader _header;

        public bint _unk1;
        public bint _unk2;
        public bushort _flags1;
        public bushort _flags2;
        public bint _unk5;

        public BVec3 _vec1;
        public RGBAPixel _lighting1;
        public BVec3 _vec2;

        public bint _unk6;
        public bfloat _unk7;
        public bfloat _unk8;
        public bint _unk9; //2
        public bfloat _unk10;
        public RGBAPixel _lighting2;
        public bfloat _unk12;

        public SCN0KeyframesHeader* xEndKeyframes { get { return (SCN0KeyframesHeader*)((byte*)Address + 0x24 + *(bint*)((byte*)Address + 0x24)); } }
        public SCN0KeyframesHeader* yEndKeyframes { get { return (SCN0KeyframesHeader*)((byte*)Address + 0x28 + *(bint*)((byte*)Address + 0x28)); } }
        public SCN0KeyframesHeader* zEndKeyframes { get { return (SCN0KeyframesHeader*)((byte*)Address + 0x2C + *(bint*)((byte*)Address + 0x2C)); } }
        public RGBAPixel* light1Entries { get { return (RGBAPixel*)((byte*)Address + 0x30 + *(bint*)((byte*)Address + 0x30)); } }
        public SCN0KeyframesHeader* xStartKeyframes { get { return (SCN0KeyframesHeader*)((byte*)Address + 0x34 + *(bint*)((byte*)Address + 0x34)); } }
        public SCN0KeyframesHeader* yStartKeyframes { get { return (SCN0KeyframesHeader*)((byte*)Address + 0x38 + *(bint*)((byte*)Address + 0x38)); } }
        public SCN0KeyframesHeader* zStartKeyframes { get { return (SCN0KeyframesHeader*)((byte*)Address + 0x3C + *(bint*)((byte*)Address + 0x3C)); } }
        public RGBAPixel* light2Entries { get { return (RGBAPixel*)((byte*)Address + 0x54 + *(bint*)((byte*)Address + 0x54)); } }
        
        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct SCN0Fog
    {
        public const int Size = 40;

        public SCN0CommonHeader _header;
        
        //Flags determine if there's an offset
        public byte _flags;
        public Int24 _pad;
        public bint _density;

        //Each of these is a bint offset if not fixed
        public bfloat _start; 
        public bfloat _end; 
        public RGBAPixel _color;

        public SCN0KeyframesHeader* startKeyframes { get { return (SCN0KeyframesHeader*)((byte*)Address + 28 + *(bint*)((byte*)Address + 28)); } }
        public SCN0KeyframesHeader* endKeyframes { get { return (SCN0KeyframesHeader*)((byte*)Address + 32 + *(bint*)((byte*)Address + 32)); } }
        public RGBAPixel* colorEntries { get { return (RGBAPixel*)((byte*)Address + 36 + *(bint*)((byte*)Address + 36)); } }

        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }
    }

    [Flags]
    public enum SCN0FogFlags
    {
        None = 0,
        FixedStart = 0x20,
        FixedEnd = 0x40,
        FixedColor = 0x80
    }

    [Flags]
    public enum SCN0CameraVectorFlags
    {
        None = 0,
        Unknown = 1,
        FixedX = 2,
        FixedY = 4,
        FixedZ = 8
    }

    [Flags]
    public enum SCN0CameraFlags2
    {
        None = 0,
        UseVec4 = 2,
        UseVec3 = 1
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct SCN0Camera
    {
        public const int Size = 92;

        public SCN0CommonHeader _header;

        public bint _pad1; //0
        public bushort _flags1;
        public bushort _flags2;
        public bint _pad2; //0

        public BVec3 _vec1;
        public BVec3 _camSettings;
        public BVec3 _vec2;
        public BVec3 _vec3;
        public BVec3 _vec4;

        public SCN0KeyframesHeader* v1xKfs { get { return (SCN0KeyframesHeader*)((byte*)Address + 0x20 + *(bint*)((byte*)Address + 0x20)); } }
        public SCN0KeyframesHeader* v1yKfs { get { return (SCN0KeyframesHeader*)((byte*)Address + 0x24 + *(bint*)((byte*)Address + 0x24)); } }
        public SCN0KeyframesHeader* v1zKfs { get { return (SCN0KeyframesHeader*)((byte*)Address + 0x28 + *(bint*)((byte*)Address + 0x28)); } }

        public SCN0KeyframesHeader* v2xKfs { get { return (SCN0KeyframesHeader*)((byte*)Address + 0x38 + *(bint*)((byte*)Address + 0x38)); } }
        public SCN0KeyframesHeader* v2yKfs { get { return (SCN0KeyframesHeader*)((byte*)Address + 0x3C + *(bint*)((byte*)Address + 0x3C)); } }
        public SCN0KeyframesHeader* v2zKfs { get { return (SCN0KeyframesHeader*)((byte*)Address + 0x40 + *(bint*)((byte*)Address + 0x40)); } }
        
        public SCN0KeyframesHeader* v3xKfs { get { return (SCN0KeyframesHeader*)((byte*)Address + 0x44 + *(bint*)((byte*)Address + 0x44)); } }
        public SCN0KeyframesHeader* v3yKfs { get { return (SCN0KeyframesHeader*)((byte*)Address + 0x48 + *(bint*)((byte*)Address + 0x48)); } }
        public SCN0KeyframesHeader* v3zKfs { get { return (SCN0KeyframesHeader*)((byte*)Address + 0x4C + *(bint*)((byte*)Address + 0x4C)); } }
        
        public SCN0KeyframesHeader* v4xKfs { get { return (SCN0KeyframesHeader*)((byte*)Address + 0x50 + *(bint*)((byte*)Address + 0x50)); } }
        public SCN0KeyframesHeader* v4yKfs { get { return (SCN0KeyframesHeader*)((byte*)Address + 0x54 + *(bint*)((byte*)Address + 0x54)); } }
        public SCN0KeyframesHeader* v4zKfs { get { return (SCN0KeyframesHeader*)((byte*)Address + 0x58 + *(bint*)((byte*)Address + 0x58)); } }

        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct SCN0KeyframesHeader
    {
        public const int Size = 4;

        public bushort _numFrames;
        public bushort _unk;

        public SCN0KeyframesHeader(int entries)
        {
            _numFrames = (ushort)entries;
            _unk = 0;
        }

        private VoidPtr Address { get { fixed (void* p = &this)return p; } }
        public SCN0KeyframeStruct* Data { get { return (SCN0KeyframeStruct*)(Address + Size); } }
    }

    public struct SCN0KeyframeStruct
    {
        public bfloat _tangent, _index, _value;

        public static implicit operator SCN0Keyframe(SCN0KeyframeStruct v) { return new SCN0Keyframe(v._tangent, v._index, v._value); }
        public static implicit operator SCN0KeyframeStruct(SCN0Keyframe v) { return new SCN0KeyframeStruct(v._tangent, v._index, v._value); }

        public SCN0KeyframeStruct(float tan, float index, float value) { _index = index; _value = value; _tangent = tan; }

        public float Index { get { return _index; } set { _index = value; } }
        public float Value { get { return _value; } set { _value = value; } }
        public float Tangent { get { return _tangent; } set { _tangent = value; } }

        public override string ToString()
        {
            return String.Format("Tangent={0}, Index={1}, Value={2}", _tangent, _index, _value);
        }
    }

    public class SCN0Keyframe
    {
        public float _tangent, _index, _value;
        
        public static implicit operator SCN0Keyframe(Vector3 v) { return new SCN0Keyframe(v._x, v._y, v._z); }
        public static implicit operator Vector3(SCN0Keyframe v) { return new Vector3(v._tangent, v._index, v._value); }

        public SCN0Keyframe(float tan, float index, float value) { _index = index; _value = value; _tangent = tan; }
        public SCN0Keyframe() { }

        public float Index { get { return _index; } set { _index = value; } }
        public float Value { get { return _value; } set { _value = value; } }
        public float Tangent { get { return _tangent; } set { _tangent = value; } }

        public override string ToString()
        {
            return String.Format("Tangent={0}, Index={1}, Value={2}", _tangent, _index, _value);
        }
    }
}
