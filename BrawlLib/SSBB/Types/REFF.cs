using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBBTypes
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct REFF
    {
        //Header + string is aligned to 4 bytes

        public const uint Tag = 0x46464552;

        public SSBBCommonHeader _header;
        public uint _tag; //Same as header
        public bint _dataLength; //Size of second REFF block. (file size - 0x18)
        public bint _dataOffset; //Offset from itself. Begins first entry
        public bint _unk1; //0
        public bint _unk2; //0
        public bshort _stringLen;
        public bshort _unk3; //0

        private VoidPtr Address { get { fixed (void* p = &this)return p; } }

        public string IdString
        {
            get { return new String((sbyte*)Address + 0x28); }
            set
            {
                int len = value.Length + 1;
                _stringLen = (short)len;

                byte* dPtr = (byte*)Address + 0x28;
                fixed (char* sPtr = value)
                {
                    for (int i = 0; i < len; i++)
                        *dPtr++ = (byte)sPtr[i];
                }

                //Align to 4 bytes
                while ((len++ & 3) != 0)
                    *dPtr++ = 0;

                //Set data offset
                _dataOffset = 0x18 + len - 1;
            }
        }

        public REFTypeObjectTable* Table { get { return (REFTypeObjectTable*)(Address + 0x18 + _dataOffset); } }
    }

    public unsafe struct REFTypeObjectTable
    {
        //Table size is aligned to 4 bytes
        //All entry offsets are relative to this offset

        public bint _length;
        public bshort _entries;
        public bshort _unk1;

        public VoidPtr Address { get { fixed (void* p = &this)return p; } }

        public REFTypeObjectEntry* First { get { return (REFTypeObjectEntry*)(Address + 8); } }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct REFTypeObjectEntry
    {
        public bshort _strLen;
        public string Name
        {
            get { return new String((sbyte*)Address + 2); }
            set
            {
                int len = value.Length + 1;
                _strLen = (short)len;//.Align(4);

                byte* dPtr = (byte*)Address + 2;
                fixed (char* sPtr = value)
                {
                    for (int i = 0; i < len; i++)
                        *dPtr++ = (byte)sPtr[i];
                }

                //Align to 4 bytes
                //while ((len++ & 3) != 0)
                //    *dPtr++ = 0;
            }
        }

        public int DataOffset
        {
            get { return (int)*(buint*)((byte*)Address + 2 + _strLen); }
            set { *(buint*)((byte*)Address + 2 + _strLen) = (uint)value; }
        }

        public int DataLength
        {
            get { return (int)*(buint*)((byte*)Address + 2 + _strLen + 4); }
            set { *(buint*)((byte*)Address + 2 + _strLen + 4) = (uint)value; }
        }

        private VoidPtr Address { get { fixed (void* p = &this)return p; } }

        public REFTypeObjectEntry* Next { get { return (REFTypeObjectEntry*)(Address + 10 + _strLen); } }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct REFFData
    {
        //0x0
        public buint _unk1; //0
        public bshort _unk2; //0
        public bshort _unk3; //0x0140
        public byte _headerLen; //0x80
        public bshort _control; //If 1, located in EFLS file
        public byte _flags;
        public byte _unk7;
        public byte _unk8;
        public byte _unk9;
        public byte _unk10; //0, 7, 8, 9
        //0x10
        public bshort _unk11;
        public bshort _unk12;
        public byte _unk13a;
        public byte _unk13b;
        public bshort _unk14;
        public bfloat _unk15;
        public bshort _unk16;
        public bshort _unk17;
        //0x20
        public bshort _unk18;
        public byte _unk19;
        public byte _unk20;
        public bfloat _unk21;
        public bfloat _unk22;
        public bfloat _unk23;
        //0x30
        public bfloat _unk24;
        public bfloat _unk25;
        public bfloat _unk26;
        public bshort _unk27;
        public byte _unk28;
        public byte _unk29;
        //0x40
        public bfloat _unk30;
        public bint _unk31;
        public bint _unk32;
        public bint _unk33;
        //0x50
        public bfloat _unk34;
        public bfloat _unk35;
        public bfloat _unk36;
        public bfloat _unk37;
        //0x60
        public bfloat _unk38;
        public bfloat _unk39;
        public bfloat _unk40;
        public bfloat _unk41;
        //0x70
        public bfloat _unk42;
        public bfloat _unk43;
        public bfloat _unk44;
        public bfloat _unk45;
    }
}
