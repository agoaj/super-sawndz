using System;
using System.Runtime.InteropServices;

namespace BrawlLib.Wii.Animations
{
    //I = indexed/interpolated, L = linear
    public enum AnimDataFormat : byte
    {
        None = 0,
        I4 = 1,
        I6 = 2,
        I12 = 3,
        L1 = 4,
        L4 = 6
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct AnimationCode
    {
        public static AnimationCode Default = new AnimationCode() { _data = 0x3FE07F };

        public uint _data;

        //0000 0000 0000 0000 0000 0000 0000 0001       Unknown, always present.

        //0000 0000 0000 0000 0000 0000 0000 0010       Default/ignore translation? (must be present when there are keyframes)
        //0000 0000 0000 0000 0000 0000 0000 0100       Default/ignore rotation? (must be present when there are keyframes)
        //0000 0000 0000 0000 0000 0000 0000 1000       Default/ignore scale

        //0000 0000 0000 0000 0000 0000 0001 0000		Scale isotropic
        //0000 0000 0000 0000 0000 0000 0010 0000		Rotation isotropic
        //0000 0000 0000 0000 0000 0000 0100 0000		Translation isotropic

        //0000 0000 0000 0000 1110 0000 0000 0000		Scale fixed
        //0000 0000 0000 0111 0000 0000 0000 0000		Rotation fixed
        //0000 0000 0011 1000 0000 0000 0000 0000		Translation fixed

        //0000 0000 0100 0000 0000 0000 0000 0000		Scale exists
        //0000 0000 1000 0000 0000 0000 0000 0000		Rotation exists
        //0000 0001 0000 0000 0000 0000 0000 0000		Translation exists

        //0000 0110 0000 0000 0000 0000 0000 0000		Scale format
        //0011 1000 0000 0000 0000 0000 0000 0000		Rotation format
        //1100 0000 0000 0000 0000 0000 0000 0000		Translation format

        public bool HasScale { get { return (_data & 0x400000) != 0; } set { _data = (_data & 0xFFBFFFFF) | (value ? (uint)0x400000 : 0); } }
        public bool IsScaleIsotropic { get { return (_data & 0x10) != 0; } set { _data = (_data & 0xFFFFFFEF) | (value ? (uint)0x10 : 0); } }
        //public bool IsIsotropicFixed { get { return (_data & 0xE000) != 0; } }
        public bool IsScaleXFixed { get { return (_data & 0x2000) != 0; } set { _data = (_data & 0xFFFFDFFF) | ((value) ? (uint)0x2000 : 0); } }
        public bool IsScaleYFixed { get { return (_data & 0x4000) != 0; } set { _data = (_data & 0xFFFFBFFF) | ((value) ? (uint)0x4000 : 0); } }
        public bool IsScaleZFixed { get { return (_data & 0x8000) != 0; } set { _data = (_data & 0xFFFF7FFF) | ((value) ? (uint)0x8000 : 0); } }
        public AnimDataFormat ScaleDataFormat { get { return (AnimDataFormat)((_data >> 25) & 3); } set { _data = (_data & 0xF9FFFFFF) | ((uint)value << 25); } }
        public bool IgnoreScale { get { return (_data & 0x8) != 0; } set { _data = (_data & 0xFFFFFFF7) | (value ? (uint)8 : 0); } }

        public bool HasRotation { get { return (_data & 0x800000) != 0; } set { _data = (_data & 0xFF7FFFFF) | (value ? (uint)0x800000 : 0); } }
        public bool IsRotationIsotropic { get { return (_data & 0x20) != 0; } set { _data = (_data & 0xFFFFFFDF) | (value ? (uint)0x20 : 0); } }
        public bool IsRotationXFixed { get { return (_data & 0x10000) != 0; } set { _data = (_data & 0xFFFEFFFF) | ((value) ? (uint)0x10000 : 0); } }
        public bool IsRotationYFixed { get { return (_data & 0x20000) != 0; } set { _data = (_data & 0xFFFDFFFF) | ((value) ? (uint)0x20000 : 0); } }
        public bool IsRotationZFixed { get { return (_data & 0x40000) != 0; } set { _data = (_data & 0xFFFBFFFF) | ((value) ? (uint)0x40000 : 0); } }
        public AnimDataFormat RotationDataFormat { get { return (AnimDataFormat)((_data >> 27) & 7); } set { _data = (_data & 0xC7FFFFFF) | ((uint)value << 27); } }

        public bool HasTranslation { get { return (_data & 0x1000000) != 0; } set { _data = (_data & 0xFEFFFFFF) | (value ? (uint)0x1000000 : 0); } }
        public bool IsTranslationIsotropic { get { return (_data & 0x40) != 0; } set { _data = (_data & 0xFFFFFFBF) | (value ? (uint)0x40 : 0); } }
        public bool IsTranslationXFixed { get { return (_data & 0x080000) != 0; } set { _data = (_data & 0xFFF7FFFF) | ((value) ? (uint)0x080000 : 0); } }
        public bool IsTranslationYFixed { get { return (_data & 0x100000) != 0; } set { _data = (_data & 0xFFEFFFFF) | ((value) ? (uint)0x100000 : 0); } }
        public bool IsTranslationZFixed { get { return (_data & 0x200000) != 0; } set { _data = (_data & 0xFFDFFFFF) | ((value) ? (uint)0x200000 : 0); } }
        public AnimDataFormat TranslationDataFormat { get { return (AnimDataFormat)(_data >> 30); } set { _data = (_data & 0x3FFFFFFF) | ((uint)value << 30); } }

        public int ExtraData { get { return (int)(_data & 0x6); } set { _data = (_data & 0xFFFFFFF9) | (uint)(value << 1); } }

        public bool ExtBit { get { return (_data & 1) != 0; } set { _data = (_data & 0xFFFFFFFE) | ((value) ? (uint)1 : 0); } }

        public static implicit operator AnimationCode(uint data) { return new AnimationCode() { _data = data }; }
        public static implicit operator uint(AnimationCode code) { return code._data; }

        public bool GetIsFixed(int i) { return (_data & ((uint)1 << (13 + i))) != 0; }
        public void SetIsFixed(int i, bool p)
        {
            uint mask = (uint)1 << (13 + i);
            _data = (_data & ~mask) | (p ? mask : 0);
        }

        public bool GetIsIsotropic(int i) { return (_data & ((uint)1 << (4 + i))) != 0; }
        public void SetIsIsotropic(int i, bool p)
        {
            uint mask = (uint)1 << (4 + i);
            _data = (_data & ~mask) | (p ? mask : 0);
        }

        public bool GetExists(int i) { return (_data & ((uint)1 << (22 + i))) != 0; }
        public void SetExists(int i, bool p)
        {
            uint mask = (uint)1 << (22 + i);
            _data = (_data & ~mask) | (p ? mask : 0);
        }

        public bool GetIgnore(int i) { return (_data & ((uint)1 << (3 - i))) != 0; }
        public void SetIgnore(int i, bool p)
        {
            uint mask = (uint)1 << (3 - i);
            _data = (_data & ~mask) | (p ? mask : 0);
        }

        public AnimDataFormat GetFormat(int index)
        {
            if (index == 0)
                return (AnimDataFormat)((_data >> 25) & 3);
            else if (index == 1)
                return (AnimDataFormat)((_data >> 27) & 7);
            else
                return (AnimDataFormat)((_data >> 30) & 3);
        }
        public void SetFormat(int index, AnimDataFormat format)
        {
            if (index == 0)
                _data = (_data & 0xF9FFFFFF) | ((uint)format << 25);
            else if (index == 1)
                _data = (_data & 0xC7FFFFFF) | ((uint)format << 27);
            else
                _data = (_data & 0x3FFFFFFF) | ((uint)format << 30);
        }

        public unsafe override string ToString()
        {
            sbyte* buffer = stackalloc sbyte[39];

            uint data = _data;
            for (int i = 38; i >= 0; i--)
            {
                if (((i + 1) % 5) == 0)
                    buffer[i] = 0x20;
                else
                {
                    buffer[i] = (sbyte)(((data & 1) == 0) ? 0x30 : 0x31);
                    data >>= 1;
                }
            }

            return new string(buffer, 0, 39);
        }

        internal void GetEntrySize()
        {
            throw new NotImplementedException();
        }
    }
}
