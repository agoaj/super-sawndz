using System;
using BrawlLib.SSBBTypes;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using BrawlLib.IO;
using BrawlLib.Wii.Compression;
using System.Windows;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class ARCNode : ARCEntryNode
    {
        private bool _isPair;

        internal ARCHeader* Header { get { return (ARCHeader*)WorkingUncompressed.Address; } }
        
        public override ResourceType ResourceType 
        {
            get
            {
                //if (Name.StartsWith("ai_"))
                //    return ResourceType.AI;
                //else
                    return ResourceType.ARC;
            } 
        }

        [Browsable(false)]
        public bool IsPair { get { return _isPair; } set { _isPair = value; } }

        #region SpecialNames
        public static List<string> SpecialName = new List<string>() {
            "FitCaptain",
            "FitDedede",
            "FitDiddy", 
            "FitDonkey",
            "FitFalco",
            "FitFox",
            "FitGameWatch",
            "FitGanon",
            "FitGKoopa",
            "FitIke",
            "FitKirby",
            "FitKoopa",
            "FitLink",
            "FitLucario", 
            "FitLucas",
            "FitLuigi",
            "FitMario", 
            "FitMarth",
            "FitMetaknight",
            "FitNess",
            "FitPeach",
            "FitPikachu",
            "FitPikmin",
            "FitPit",
            "FitPokeFushigisou",
            "FitPokeLizardon",
            "FitPokeTrainer",
            "FitPokeZenigame",
            "FitPopo",
            "FitPurin",
            "FitRobot",
            "FitSamus",
            "FitSheik",
            "FitSnake",
            "FitSonic",
            "FitSZerosuit",
            "FitToonLink",
            "FitWario",
            "FitWarioMan",
            "FitWolf",
            "FitYoshi",
            "FitZakoBall",
            "FitZakoBoy",
            "FitZakoChild",
            "FitZakoGirl",
            "FitZelda",
            "Fighter"};
#endregion

        protected override void OnPopulate()
        {
            ARCFileHeader* entry = Header->First;
            for (int i = 0; i < Header->_numFiles; i++, entry = entry->Next)
                if ((entry->_size == 0) || (NodeFactory.FromAddress(this, entry->Data, entry->Length) == null))
                    if (SpecialName.Contains(_name) && i == 0 && RootNode == this)
                        new MoveDefNode().Initialize(this, entry->Data, entry->Length);
                    else
                        new ARCEntryNode().Initialize(this, entry->Data, entry->Length);
        }

        internal override void Initialize(ResourceNode parent, DataSource origSource, DataSource uncompSource)
        {
            base.Initialize(parent, origSource, uncompSource);
            if (_origPath != null)
            {
                string path = Path.Combine(Path.GetDirectoryName(_origPath), Path.GetFileNameWithoutExtension(_origPath));
                _isPair = File.Exists(path + ".pac") && File.Exists(path + ".pcs");
            }
        }

        protected override bool OnInitialize()
        {
            base.OnInitialize();
            _name = Header->Name;
            return Header->_numFiles > 0;
        }

        public void ExtractToFolder(string outFolder)
        {
            if (!Directory.Exists(outFolder))
                Directory.CreateDirectory(outFolder);

            foreach (ARCEntryNode entry in Children)
            {
                if (entry is ARCNode)
                    ((ARCNode)entry).ExtractToFolder(Path.Combine(outFolder, entry.Name));
                else if (entry is BRESNode)
                    ((BRESNode)entry).ExportToFolder(outFolder);
            }
        }

        public void ReplaceFromFolder(string inFolder)
        {
            DirectoryInfo dir = new DirectoryInfo(inFolder);
            FileInfo[] files;
            DirectoryInfo[] dirs;
            foreach (ARCEntryNode entry in Children)
            {
                if (entry is ARCNode)
                {
                    dirs = dir.GetDirectories(entry.Name);
                    if (dirs.Length > 0)
                    {
                        ((ARCNode)entry).ReplaceFromFolder(dirs[0].FullName);
                        continue;
                    }
                }
                else if (entry is BRESNode)
                    ((BRESNode)entry).ReplaceFromFolder(inFolder);

                //Find file name for entry
                files = dir.GetFiles(entry.Name + ".*");
                if (files.Length > 0)
                {
                    entry.Replace(files[0].FullName);
                    continue;
                }
            }
        }

        //public override void Replace(string fileName)
        //{
        //    base.Replace(fileName);
        //}

        protected override int OnCalculateSize(bool force)
        {
            int size = ARCHeader.Size + (Children.Count * 0x20);
            foreach (ResourceNode node in Children)
                size += node.CalculateSize(force).Align(0x20);
            return size;
        }

        internal protected override void OnRebuild(VoidPtr address, int size, bool force)
        {
            ARCHeader* header = (ARCHeader*)address;
            *header = new ARCHeader((ushort)Children.Count, Name);

            ARCFileHeader* entry = header->First;
            foreach (ARCEntryNode node in Children)
            {
                *entry = new ARCFileHeader(node.FileType, node.FileIndex, node._calcSize, node.FileGroup, node.Unknown, node.FileId);
                if (node.IsCompressed)
                    node.MoveRaw(entry->Data, entry->Length);
                else
                    node.Rebuild(entry->Data, entry->Length, force);
                entry = entry->Next;
            }
        }

        public override unsafe void Export(string outPath)
        {
            if (outPath.EndsWith(".pair", StringComparison.OrdinalIgnoreCase))
                ExportPair(outPath);
            else if (outPath.EndsWith(".mrg", StringComparison.OrdinalIgnoreCase))
                ExportAsMRG(outPath);
            //else if (outPath.EndsWith(".pac", StringComparison.OrdinalIgnoreCase))
            //    ExportPAC(outPath);
            else
                base.Export(outPath);
        }

        public void ExportAsMRG(string path)
        {
            MRGNode node = new MRGNode();
            node._children = Children;
            node._changed = true;
            node.Export(path);
        }

        public void ExportPair(string path)
        {
            if (Path.HasExtension(path))
                path = path.Substring(0, path.LastIndexOf('.'));

            ExportPAC(path + ".pac");
            ExportPCS(path + ".pcs");
        }
        public void ExportPAC(string outPath)
        {
            Rebuild();
            ExportUncompressed(outPath);
        }
        public void ExportPCS(string outPath)
        {
            Rebuild();
            if (Compression == CompressionType.LZ77)
                Export(outPath);
            else
            {
                using (FileStream inStream = new FileStream(Path.GetTempFileName(), FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None, 0x8, FileOptions.SequentialScan | FileOptions.DeleteOnClose))
                using (FileStream outStream = new FileStream(outPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None, 8, FileOptions.SequentialScan))
                {
                    Compressor.Compact(CompressionType.LZ77, WorkingUncompressed.Address, WorkingUncompressed.Length, inStream);
                    outStream.SetLength(inStream.Length);
                    using (FileMap map = FileMap.FromStream(inStream))
                    using (FileMap outMap = FileMap.FromStream(outStream))
                        Memory.Move(outMap.Address, map.Address, (uint)map.Length);
                }
            }
        }

        internal static ResourceNode TryParse(DataSource source) { return ((ARCHeader*)source.Address)->_tag == ARCHeader.Tag ? new ARCNode() : null; }
    }

    public unsafe class ARCEntryNode : U8EntryNode
    {
        public override ResourceType ResourceType { get { return ResourceType.ARCEntry; } }

        [Browsable(true)]
        public override CompressionType Compression
        {
            get { return base.Compression; }
            set { base.Compression = value; }
        }

        internal ARCFileType _fileType;
        [Category("ARC Entry")]
        public ARCFileType FileType { get { return _fileType; } set { _fileType = value; SignalPropertyChange(); UpdateName(); } }

        internal short _fileIndex;
        [Category("ARC Entry")]
        public short FileIndex { get { return _fileIndex; } set { _fileIndex = value; SignalPropertyChange(); UpdateName(); } }
        
        internal byte _group;
        [Category("ARC Entry")]
        public byte FileGroup { get { return _group; } set { _group = value; SignalPropertyChange(); UpdateName(); } }

        internal byte _unk;
        [Category("ARC Entry")]
        public byte Unknown { get { return _unk; } set { _unk = value; SignalPropertyChange(); UpdateName(); } }

        internal short _fileId;
        [Category("ARC Entry")]
        public short FileId { get { return _fileId; } set { _fileId = value; SignalPropertyChange(); UpdateName(); } }

        protected void UpdateName()
        {
            if (!(this is ARCNode))
                Name = String.Format("{0}[{1}]", _fileType, _fileIndex);
        }

        internal override void Initialize(ResourceNode parent, DataSource origSource, DataSource uncompSource)
        {
            base.Initialize(parent, origSource, uncompSource);

            if (parent != null && (parent is MRGNode || RootNode is U8Node))
            {
                _fileType = 0;
                _fileIndex = (short)_parent._children.IndexOf(this);
                _group = 0;
                _unk = 0;
                _fileId = 0;
                if (_name == null)
                    _name = String.Format("{0}[{1}]", _fileType, _fileIndex);
            }
            else if (parent != null)
            {
                ARCFileHeader* header = (ARCFileHeader*)(origSource.Address - 0x20);
                _fileType = header->FileType;
                _fileIndex = header->_index;
                _group = header->_groupIndex;
                _unk = header->_unk1;
                _fileId = header->_id;
                if (_name == null)
                    _name = String.Format("{0}[{1}]", _fileType, _fileIndex);
            }
            else if (_name == null)
                _name = Path.GetFileName(_origPath);
        }
    }
}
