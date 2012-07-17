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
    public unsafe class U8Node : ARCEntryNode
    {
        internal U8* Header { get { return (U8*)WorkingUncompressed.Address; } }
        
        public override ResourceType ResourceType { get { return ResourceType.Unknown; } }

        protected override void OnPopulate()
        {
            U8Entry* first = Header->Entries;
            uint count = first->_dataLength - 1;
            U8Entry* entry = first + 1;
            sbyte* table = (sbyte*)entry + count * 12;
            List<U8EntryNode> nodes = new List<U8EntryNode>();
            U8EntryNode e = null;
            for (int i = 0; i < count; i++)
            {
                if (entry->isFolder)
                {
                    (e = new U8FolderNode() { index = i, _name = new String(table + (int)entry->_stringOffset) }).Initialize(this, entry, 12);
                    nodes.Add(e);
                }
                else
                {
                    if ((entry->_dataLength == 0) || (e = NodeFactory.FromAddress(this, (VoidPtr)Header + entry->_dataOffset, (int)entry->_dataLength) as ARCEntryNode) == null)
                        (e = new U8EntryNode()).Initialize(this, (VoidPtr)Header + entry->_dataOffset, (int)entry->_dataLength);
                    e._name = new String(table + (int)entry->_stringOffset);
                    e.index = i;
                    e.parent = (int)entry->_dataOffset;
                    e.firstChild = (int)entry->_dataLength;
                    nodes.Add(e);
                }
                entry++;
            }
            foreach (U8EntryNode x in nodes)
            {
                if (x.type == 1)
                {
                    if (x.parent == 0)
                        x.Parent = this;
                    else if (x.parent < nodes.Count)
                        x.Parent = nodes[x.parent - 1];
                    U8EntryNode t = null;
                    if (x.index + 1 < nodes.Count)
                        t = nodes[x.index + 1];
                    while (t != null)
                    {
                        t.Parent = x;
                        if (t.index + 1 < nodes.Count && t.ChildEndIndex != nodes[t.index + 1].index)
                            t = nodes[t.index + 1];
                        else
                            t = null;
                    }
                }
            }
            IsDirty = false; //Clear up changes from parent reassignments
        }

        protected override bool OnInitialize()
        {
            return true;
        }

        int entrySize = 0, id = 0;
        U8StringTable table;
        public int GetSize(ResourceNode node, bool force)
        {
            if (node is U8EntryNode)
            {
                (node as U8EntryNode).index = id++;
                table.Add(node.Name);
                int size = node is U8FolderNode ? 0 : node.CalculateSize(force).Align(0x20);
                entrySize += 12;
                table.Add(node.Name);
                foreach (ResourceNode r in node.Children)
                    size += GetSize(r, force);
                return size;
            }
            return 0;
        }

        protected override int OnCalculateSize(bool force)
        {
            entrySize = 12;
            id = 1;
            table = new U8StringTable();
            table._table.Add("", 0);
            int childSize = 0;
            foreach (ResourceNode e in Children)
                childSize += GetSize(e, force);
            entryLength = (table.TotalSize + entrySize);
            return 0x20 + childSize + entryLength.Align(0x20);
        }

        public void RebuildNode(VoidPtr header, U8EntryNode node, ref U8Entry* entry, VoidPtr sTableStart, ref VoidPtr dataAddr, U8StringTable sTable, bool force)
        {
            entry->_type = (byte)((node is U8FolderNode) ? 1 : 0);
            entry->_stringOffset.Value = (int)sTable[node.Name] - (int)sTableStart;
            if (entry->_type == 1)
            {
                if (node.Parent != this && node.Parent != null)
                    entry->_dataOffset = (uint)(node.Parent as U8EntryNode).index;
                (entry++)->_dataLength = (uint)(node.index + node.Children.Count + 1);
                foreach (ResourceNode b in node.Children)
                    if (b is U8EntryNode)
                        RebuildNode(header, b as U8EntryNode, ref entry, sTableStart, ref dataAddr, table, force);
            }
            else
            {
                node.Rebuild(dataAddr, node._calcSize, force);
                entry->_dataOffset = (uint)dataAddr - (uint)header;
                (entry++)->_dataLength = (uint)node._calcSize.Align(0x10);
                dataAddr += node._calcSize.Align(0x20);
            }
        }

        int entryLength = 0;
        protected internal override void OnRebuild(VoidPtr address, int length, bool force)
        {
            U8* header = (U8*)address;
            header->_tag = U8.Tag;
            header->_entriesLength = (uint)entryLength;
            header->_entriesOffset = 0x20;

            VoidPtr dataAddress = address + 0x20 + entryLength.Align(0x20);
            U8Entry* entries = (U8Entry*)(address + 0x20);
            VoidPtr tableAddr = address + 0x20 + entrySize;
            table.WriteTable(tableAddr);

            header->_firstOffset = (uint)(dataAddress - address);

            entries->_dataLength = (uint)(entrySize / 12);
            entries->_type = 1;
            entries++;

            foreach (U8EntryNode b in Children)
                RebuildNode(address, b, ref entries, tableAddr, ref dataAddress, table, force);
        }

        public override unsafe void Export(string outPath)
        {
            ExportNonYaz0(outPath);
        }

        public void ExportAsMRG(string path)
        {
            MRGNode node = new MRGNode();
            node._children = Children;
            node._changed = true;
            node.Export(path);
        }

        public void ExportNonYaz0(string outPath)
        {
            //Rebuild();
            ExportUncompressed(outPath);
        }
        public void ExportCompressed(string outPath)
        {
            //Rebuild();
            if (Compression == CompressionType.RunLength)
                Export(outPath);
            else
            {
                using (FileStream inStream = new FileStream(Path.GetTempFileName(), FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None, 0x8, FileOptions.SequentialScan | FileOptions.DeleteOnClose))
                using (FileStream outStream = new FileStream(outPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None, 8, FileOptions.SequentialScan))
                {
                    Compressor.CompactYAZ0(WorkingUncompressed.Address, WorkingUncompressed.Length, inStream);
                    outStream.SetLength(inStream.Length);
                    using (FileMap map = FileMap.FromStream(inStream))
                    using (FileMap outMap = FileMap.FromStream(outStream))
                        Memory.Move(outMap.Address, map.Address, (uint)map.Length);
                }
            }
        }

        internal static ResourceNode TryParse(DataSource source) 
        {
            return ((U8*)source.Address)->_tag == U8.Tag ? new U8Node() : null;
        }
    }
    public unsafe class U8EntryNode : ResourceNode
    {
        internal U8Entry* U8EntryHeader { get { return (U8Entry*)WorkingSource.Address; } }

        public int parent, firstChild, type, index;

        [Browsable(false)]
        public int ParentIndex { get { return parent; } }
        [Browsable(false)]
        public int ChildEndIndex { get { return firstChild; } }
        [Browsable(false)]
        public int Type { get { return type; } }
        [Browsable(false)]
        public int ID { get { return index; } }

        protected override bool OnInitialize()
        {
            type = U8EntryHeader->_type;
            parent = (int)U8EntryHeader->_dataOffset;
            firstChild = (int)U8EntryHeader->_dataLength;
            return type == 1;
        }
    }
    public unsafe class U8FolderNode : U8EntryNode
    {
        public override ResourceType ResourceType { get { return ResourceType.Container; } }

        protected override bool OnInitialize()
        {
            return base.OnInitialize();
        }

        protected override int OnCalculateSize(bool force)
        {
            return 0;
        }
    }

    public unsafe class U8StringTable
    {
        public SortedList<string, VoidPtr> _table = new SortedList<string, VoidPtr>(StringComparer.OrdinalIgnoreCase);

        public void Add(string s)
        {
            if ((!String.IsNullOrEmpty(s)) && (!_table.ContainsKey(s)))
                _table.Add(s, 0);
        }

        public int TotalSize
        {
            get
            {
                int len = 0;
                foreach (string s in _table.Keys)
                    len += (s.Length + 1);
                return len;
            }
        }

        public void Clear() { _table.Clear(); }

        public VoidPtr this[string s] { get { return _table[s]; } }

        public void WriteTable(VoidPtr address)
        {
            FDefReferenceString* entry = (FDefReferenceString*)address;
            for (int i = 0; i < _table.Count; i++)
            {
                string s = _table.Keys[i];
                _table[s] = entry;
                entry->Value = s;
                entry = entry->Next;
            }
        }
    }
}
