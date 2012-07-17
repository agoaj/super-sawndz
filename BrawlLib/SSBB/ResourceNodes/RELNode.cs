using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.SSBBTypes;
using System.ComponentModel;
using System.IO;
using System.Drawing;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class RELNode : ARCEntryNode
    {
        internal RELHeader* Header { get { return (RELHeader*)WorkingUncompressed.Address; } }
        public override ResourceType ResourceType { get { return ResourceType.REL; } }

        public RELHeader* WriteHeader { get { return (RELHeader*)_address; } }

        private VoidPtr _address;
        private int _length;

        public static RELNode File(int fileId)
        {
            foreach (RELNode file in filesOpened)
                if (file._id == fileId)
                    return file;
            return null;
        }

        public static List<RELNode> filesOpened = new List<RELNode>();

        public RELSectionNode[] _sections;
        public RELSectionNode[] Sections { get { return _sections; } }

        public uint _id;
        public uint _linkNext; //0
        public uint _linkPrev; //0
        public uint _numSections;

        public uint _infoOffset;
        public uint _nameOffset;
        public uint _nameSize;
        public uint _version;

        public uint _bssSize;
        public uint _relOffset;
        public uint _impOffset;
        public uint _impSize;

        public byte _prologSection;
        public byte _epilogSection;
        public byte _unresolvedSection;
        public byte _bssSection;

        public uint _prologOffset;
        public uint _epilogOffset;
        public uint _unresolvedOffset;

        public uint _moduleAlign;
        public uint _bssAlign;
        public uint _fixSize;

        [Category("REL")]
        public uint ID { get { return _id; } }
        [Category("REL")]
        public uint NextLink { get { return _linkNext; } }
        [Category("REL")]
        public uint PrevLink { get { return _linkPrev; } }
        [Category("REL")]
        public uint SectionCount { get { return _numSections; } }
        
        [Category("REL")]
        public uint SectionInfoOffset { get { return _infoOffset; } }
        [Category("REL")]
        public uint NameOffset { get { return _nameOffset; } }
        [Category("REL")]
        public uint NameSize { get { return _nameSize; } }
        [Category("REL")]
        public uint Version { get { return _version; } }

        [Category("REL")]
        public uint bssSize { get { return _bssSize; } }
        [Category("REL")]
        public uint relOffset { get { return _relOffset; } }
        [Category("REL")]
        public uint impOffset { get { return _impOffset; } }
        [Category("REL")]
        public uint impSize { get { return _impSize; } }

        [Category("REL")]
        public uint prologSection { get { return _prologSection; } }
        [Category("REL")]
        public uint epilogSection { get { return _epilogSection; } }
        [Category("REL")]
        public uint unresolvedSection { get { return _unresolvedSection; } }
        [Category("REL")]
        public uint bssSection { get { return _bssSection; } }

        [Category("REL")]
        public uint prologOffset { get { return _prologOffset; } }
        [Category("REL")]
        public uint epilogOffset { get { return _epilogOffset; } }
        [Category("REL")]
        public uint unresolvedOffset { get { return _unresolvedOffset; } }

        [Category("REL")]
        public uint moduleAlign { get { return _moduleAlign; } }
        [Category("REL")]
        public uint bssAlign { get { return _bssAlign; } }
        [Category("REL")]
        public uint fixSize { get { return _fixSize; } }

        protected override bool OnInitialize()
        {
            //Allocate memory to write decompiled data to
            byte[] buffer = new byte[WorkingUncompressed.Length];
            for (int i = 0; i < buffer.Length; i++) buffer[i] = ((byte*)Header)[i];
            void* address = System.Memory.Alloc((int)WorkingUncompressed.Length.RUp(sizeof(uint)));
            fixed (byte* pBuffer = buffer) System.Memory.Copy(pBuffer, address, buffer.Length);
            _address = address;
            _length = WorkingUncompressed.Length;

            //_name = Header->Name;
            _name = Path.GetFileName(_origPath);

            _id = Header->_info.id;
            _linkNext = Header->_info.link._linkNext; //0
            _linkPrev = Header->_info.link._linkPrev; //0
            _numSections = Header->_info.numSections;
            _infoOffset = Header->_info.sectionInfoOffset;
            _nameOffset = Header->_info.nameOffset;
            _nameSize = Header->_info.nameSize;
            _version = Header->_info.version;

            _bssSize = Header->_bssSize;
            _relOffset = Header->_relOffset;
            _impOffset = Header->_impOffset;
            _impSize = Header->_impSize;
            _prologSection = Header->_prologSection;
            _epilogSection = Header->_epilogSection;
            _unresolvedSection = Header->_unresolvedSection;
            _bssSection = Header->_bssSection;
            _prologOffset = Header->_prologOffset;
            _epilogOffset = Header->_epilogOffset;
            _unresolvedOffset = Header->_unresolvedOffset;

            _moduleAlign = Header->_moduleAlign;
            _bssAlign = Header->_bssAlign;
            _fixSize = Header->_fixSize;

            return true;
        }

        protected override void OnPopulate()
        {
            RELGroupNode g;
            g = new RELGroupNode() { _name = "Sections" };
            g.Parent = this;
            g = new RELGroupNode() { _name = "Imports" };
            g.Parent = this;

            _sections = new RELSectionNode[_numSections];
            for (int i = 0; i < _numSections; i++)
                (_sections[i] = new RELSectionNode() { _name = "Section" + i }).Initialize(Children[0], &Header->SectionInfo[i], RELSection.Size);

            for (int i = 0; i < Header->ImportListCount; i++)
                new RELImportNode().Initialize(Children[1], &Header->Imports[i], RELImport.Size);
        }
    }

    public class RELGroupNode : RELEntryNode
    {
        public override ResourceType ResourceType { get { return ResourceType.MDefNoEdit; } }
    }

    public unsafe class RELSectionNode : RELEntryNode
    {
        internal RELSection* Header { get { return (RELSection*)WorkingUncompressed.Address; } }
        public override ResourceType ResourceType { get { return ResourceType.Unknown; } }

        [Category("REL Section")]
        public bool isCodeSection { get { return Header->IsCodeSection; } }
        [Category("REL Section")]
        public int Offset { get { return Header->Offset; } }
        [Category("REL Section")]
        public uint Size { get { return Header->_size; } }

        public byte* WriteHeader { get { return (byte*)_address; } }

        private VoidPtr _address;
        private int _length;

        public RELSection* _data = null;
        public bool _isDynamic = false;

        public RelCommand[] RelocationBuffer;

        protected override bool OnInitialize()
        {
            _data = (RELSection*)(((RELNode)Parent.Parent).WriteHeader + ((int)Header - (int)((RELNode)Parent.Parent).Header));
            _address = ((RELNode)Parent.Parent).WriteHeader + Offset;
            if (Offset == 0 && Size != 0)
            {
                _data->Offset = (int)System.Memory.Alloc((int)Size);
                _isDynamic = true;
            }

            RelocationBuffer = new RelCommand[Size.RUp(0x4) / 0x4];
            return false;
        }
    }

    public unsafe class RELImportNode : RELEntryNode
    {
        internal RELImport* Header { get { return (RELImport*)WorkingUncompressed.Address; } }
        public override ResourceType ResourceType { get { return ResourceType.Unknown; } }

        [Category("REL Import")]
        public uint ModuleID { get { return Header->_moduleId; } }
        [Category("REL Import")]
        public uint Offset { get { return Header->_offset; } }

        public string FileOffset { get { return "0x" + ((uint)Header - (uint)Root.Header).ToString("X"); } }

        public List<RELLinkNode> _cmds;
        public List<RELLinkNode> Commands { get { return _cmds; } set { _cmds = value; } }

        protected override bool OnInitialize()
        {
            _name = "Module" + ModuleID;
            _cmds = new List<RELLinkNode>();

            RELLinkNode n;
            RELSectionNode memblock = null;
            uint offset = 0;

            RELImport* header = Header;
            RELLink* link = (RELLink*)((VoidPtr)Root.Header + (uint)header->_offset);
            while (link->Type != RELLinkType.End)
            {
                offset += link->_prevOffset;

                if (link->Type == RELLinkType.Section)
                {
                    offset = 0;
                    memblock = Root.Sections[link->_section];
                }
                else
                {
                    if (memblock != null)
                        memblock.RelocationBuffer[offset.RDown(0x4) / 0x4] = new RelCommand((int)Root._id, memblock.Index, offset, ModuleID, *link);
                    else
                        throw new Exception("Non-block oriented relocation command.");
                }
                (n = new RELLinkNode() { _section = memblock }).Initialize(null, link++, RELImport.Size);
                _cmds.Add(n);
            }
            //Add the end node
            (n = new RELLinkNode() { _section = memblock }).Initialize(null, link++, RELImport.Size);
            _cmds.Add(n);

            return false;
        }
    }

    public unsafe class RELLinkNode : RELEntryNode
    {
        internal RELLink* Header { get { return (RELLink*)WorkingUncompressed.Address; } }
        public override ResourceType ResourceType { get { return ResourceType.Unknown; } }
        
        [Category("REL Link")]
        public uint PreviousOffset { get { return Header->_prevOffset; } }
        [Category("REL Link")]
        public RELLinkType Type { get { return (RELLinkType)Header->_type; } }
        [Category("REL Link")]
        public byte Section { get { return Header->_section; } }
        [Category("REL Link")]
        public uint Operand { get { return Header->_addEnd; } }

        public RELSectionNode _section = null;

        protected override bool OnInitialize()
        {
            _name = Type.ToString();

            return false;
        }
    }

    public unsafe class RelCommand
    {
        protected int _FileId;
        public int FileId { get { return _FileId; } set { _FileId = value; } }

        protected int _Memblock;
        public int Memblock { get { return _Memblock; } set { _Memblock = value; } }

        protected uint _Offset;
        public uint Offset { get { return _Offset; } set { _Offset = value; } }

        protected int _RefId;
        public int RefId { get { return _RefId; } set { _RefId = value; } }

        protected int _RefMemblock;
        public int RefMemblock { get { return _RefMemblock; } set { _RefMemblock = value; } }

        public int _Command;
        public int Command { get { return _Command; } set { _Command = value; } }

        protected uint _Operand;
        public uint Operand { get { return _Operand + (Initialized && _RefId != 0 ? (uint)RELNode.File(_RefId).Sections[_RefMemblock].Offset : 0); } }

        private bool _Initialized;
        public bool Initialized { get { return _Initialized; } }

        [Browsable(false)]
        public bool IsBranchSet { get { return (_Command >= 0xA && _Command <= 0xD); } }

        [Browsable(false)]
        public string OperandInfo { get { return "m" + _RefId + "[" + _RefMemblock + "] + " + _Operand; } }

        static Color clrNotRelocated = Color.FromArgb(255, 255, 255);
        static Color clrRelocated = Color.FromArgb(200, 255, 200);
        static Color clrBadRelocate = Color.FromArgb(255, 200, 200);

        public RelCommand(int fileId, int memblock, uint offset, uint refId, RELLink relData)
        {
            _FileId = fileId;
            _Memblock = memblock;
            _Offset = offset;
            _RefId = (int)refId;
            _RefMemblock = relData._section;
            _Command = relData._type;
            _Operand = relData._addEnd;
            _Initialized = false;
        }

        public void Execute()
        {
            if (_Initialized == true) return;
            RELNode baseFile = RELNode.File(_FileId);
            RELNode refFile = RELNode.File(_RefId);
            VoidPtr address = 0;
            uint param = 0;
            if (baseFile == null) return;
            if (refFile == null && _RefId != 0) return;

            if (refFile == null)
                param = _Operand;
            else
                param = (uint)refFile.Sections[_RefMemblock].Offset + _Operand;

            if (_Memblock == -1)
                address = baseFile.WriteHeader + _Offset;
            else
                address = baseFile.Sections[_Memblock].WriteHeader + _Offset;

            switch (_Command)
            {
                case 0x00: //Nop
                case 0xC9:
                    break;
                case 0x01: //Write Word
                    *(buint*)address = param;
                    break;
                case 0x02: //Set Branch Offset
                    *(buint*)address &= 0xFC000003;
                    *(buint*)address |= (param & 0x03FFFFFC);
                    break;
                case 0x3: //Write Lower Half
                case 0x4:
                    *(bushort*)address = (bushort)(param & 0x0000FFFF);
                    break;
                case 0x5: //Write Upper Half
                    *(bushort*)address = (bushort)((param & 0xFFFF0000) >> 16);
                    break;
                case 0x6: //Write Upper Half + bit 1
                    *(bushort*)address = (bushort)(((param & 0xFFFF0000) >> 16) | (param & 0x1));
                    break;
                case 0x7: //Set Branch Condition Offset
                case 0x8:
                case 0x9:
                    *(buint*)address &= 0xFFFF0003;
                    *(buint*)address |= (param & 0x0000FFFC);
                    break;
                case 0xA: //Set Branch Destination
                    break;
                case 0xB: //Set Branch Condition Destination
                case 0xC:
                case 0xD:
                    break;
                default:
                    throw new Exception("Unknown Relocation Command.");
            }
            _Initialized = true;
        }
    }

    public unsafe class RELEntryNode : ResourceNode
    {
        public override ResourceType ResourceType { get { return ResourceType.Unknown; } }
        internal VoidPtr Data { get { return WorkingUncompressed.Address; } }

        public string FileOffset { get { 
            if (Root != null)
                return "0x" + ((uint)Data - (uint)Root.Header).ToString("X"); 
            else return "0x0"; } }
        
        [Browsable(false)]
        public RELNode Root
        {
            get
            {
                ResourceNode n = _parent;
                while (!(n is RELNode) && (n != null))
                    n = n._parent;
                return n as RELNode;
            }
        }
    }
}
