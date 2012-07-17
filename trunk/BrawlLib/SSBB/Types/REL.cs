using System;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBBTypes
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct RELHeader
    {
        public const uint Size = 0x4C;
        
        public ModuleInfo _info;

        //0x20
        public buint _bssSize;
        public buint _relOffset;
        public buint _impOffset;
        public buint _impSize;

        //0x30
        public byte _prologSection;
        public byte _epilogSection;
        public byte _unresolvedSection;
        public byte _bssSection;
        public buint _prologOffset;
        public buint _epilogOffset;
        public buint _unresolvedOffset;

        //0x40
        public buint _moduleAlign;
        public buint _bssAlign;
        public buint _fixSize;

        private VoidPtr Address { get { fixed (void* p = &this)return p; } }

        public RELSection* SectionInfo { get { return (RELSection*)(Address + _info.sectionInfoOffset); } }
        public RELImport* Imports { get { return (RELImport*)(Address + _impOffset); } }
        
        public int ImportListCount { get { return (int)(_impSize / RELImport.Size); } }

        public string Name { get { return new String((sbyte*)Address + _info.nameOffset); } }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct RELSection
    {
        public const int Size = 8;

        public buint _offset;
        public buint _size;

        public bool IsCodeSection { get { return (_offset & 1) != 0; } set { _offset = (uint)(_offset & ~1) | (uint)(value ? 1 : 0); } }
        
        //Base is start of file
        public int Offset { get { return (int)_offset & ~1; } set { _offset = (uint)(value & ~1) | (_offset & 1); } }

        private VoidPtr Address { get { fixed (void* p = &this)return p; } }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct RELImport
    {
        public const int Size = 8;

        public buint _moduleId;

        //Base is start of file
        public buint _offset;

        private VoidPtr Address { get { fixed (void* p = &this)return p; } }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct RELLink
    {
        public const int Size = 8;

        public bushort _prevOffset; //Size of previous
        public byte _type;
        public byte _section;
        public buint _addEnd;

        public RELLinkType Type { get { return (RELLinkType)_type; } }

        private VoidPtr Address { get { fixed (void* p = &this)return p; } }
    }

    [Flags]
    public enum RELLinkType : byte
    {
        NOP1 = 0xC9, //Increment offset6
        NOP2 = 0x0,
        WriteWord = 0x1,
        SetBranchOffset = 0x2,
        WriteLowerHalf1 = 0x3,
        WriteLowerHalf2 = 0x4,
        WriteUpperHalf = 0x5,
        WriteUpperHalfandBit1 = 0x6,
        SetBranchConditionOffset1 = 0x7,
        SetBranchConditionOffset2 = 0x8,
        SetBranchConditionOffset3 = 0x9,
        SetBranchDestination = 0xA,
        SetBranchConditionDestination1 = 0xB,
        SetBranchConditionDestination2 = 0xC,
        SetBranchConditionDestination3 = 0xD,
        Section = 0xCA, //Set current section
        End = 0xCB,
        MrkRef = 0xCC
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct ModuleLink
    {
        public const int Size = 8;

        public buint _linkNext;
        public buint _linkPrev;

        public ModuleInfo* Next { get { return (ModuleInfo*)(Address + _linkNext); } }
        public ModuleInfo* Prev { get { return (ModuleInfo*)(Address + _linkPrev); } }

        private VoidPtr Address { get { fixed (void* p = &this)return p; } }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct ModuleInfo
    {
        public const int Size = 0x20;

        public buint id;                 // Unique identifier for the module
        public ModuleLink link;          // Doubly linked list of modules
        public buint numSections;        // # of sections
        public buint sectionInfoOffset;  // Offset to section info table
        public buint nameOffset;         // Offset to module name
        public buint nameSize;           // Size of module name
        public buint version;            // Version number

        private VoidPtr Address { get { fixed (void* p = &this)return p; } }
    }
}
