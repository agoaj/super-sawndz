using System;
using BrawlLib.SSBBTypes;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using BrawlLib.IO;
using BrawlLib.Wii.Compression;
using System.Windows;
using System.Windows.Forms;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class MRGNode : ResourceNode
    {
        internal MRGHeader* Header { get { return (MRGHeader*)WorkingUncompressed.Address; } }
        public override ResourceType ResourceType { get { return ResourceType.MRG; } }

        protected override void OnPopulate()
        {
            uint numFiles = 0;
            MRGFileHeader* entry = Header->First;
            for (int i = 0; i < (numFiles = Header->_numFiles); i++, entry = entry->Next)      
                if (NodeFactory.FromAddress(this, (VoidPtr)Header + entry->Data, entry->Length) == null)
                    new ARCEntryNode().Initialize(this, (VoidPtr)Header + entry->Data, entry->Length);
        }

        internal override void Initialize(ResourceNode parent, DataSource origSource, DataSource uncompSource)
        {
            base.Initialize(parent, origSource, uncompSource);
        }

        protected override bool OnInitialize()
        {
            base.OnInitialize();
            _name = Path.GetFileNameWithoutExtension(_origPath);
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
                {
                    ((BRESNode)entry).ReplaceFromFolder(inFolder);
                }

                //Find file name for entry
                files = dir.GetFiles(entry.Name + ".*");
                if (files.Length > 0)
                {
                    entry.Replace(files[0].FullName);
                    continue;
                }
            }
        }
        private int offset = 0;
        protected override int OnCalculateSize(bool force)
        {
            int size = offset = 0x20 + (Children.Count * 0x20);
            foreach (ResourceNode node in Children)
                size += node.CalculateSize(force);
            return size;
        }

        internal protected override void OnRebuild(VoidPtr address, int size, bool force)
        {
            MRGHeader* header = (MRGHeader*)address;
            *header = new MRGHeader((uint)Children.Count);

            MRGFileHeader* entry = header->First;
            foreach (ARCEntryNode node in Children)
            {
                *entry = new MRGFileHeader(node._calcSize, offset);
                node.Rebuild((VoidPtr)header + entry->Data, node._calcSize, force);
                offset += node._calcSize;
                entry = entry->Next;
            }
        }

        public void ExportAsARC(string path)
        {
            ARCNode node = new ARCNode();
            node._children = _children;
            node.Name = _name;
            node.Export(path);
        }

        public override unsafe void Export(string outPath)
        {
            if (outPath.EndsWith(".pac", StringComparison.OrdinalIgnoreCase) || 
                outPath.EndsWith(".pcs", StringComparison.OrdinalIgnoreCase) || 
                outPath.EndsWith(".pair", StringComparison.OrdinalIgnoreCase))
                ExportAsARC(outPath);
            else
                base.Export(outPath);
        }

        //MRG has no tag...
    }
}
