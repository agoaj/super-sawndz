using System;
using BrawlLib.SSBBTypes;
using System.IO;
using System.Collections.Generic;
using BrawlLib.Wii.Audio;
using System.ComponentModel;
using System.Linq;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class RSARNode : ResourceNode
    {
        internal RSARHeader* Header { get { return (RSARHeader*)WorkingSource.Address; } }

        public override ResourceType ResourceType { get { return ResourceType.RSAR; } }

        private List<RSARFileNode> _files;
        [Browsable(false)]
        public List<RSARFileNode> Files { get { return _files; } }

        public T GetResource<T>(int index) where T : ResourceNode
        {
            ResourceNode folder = null;
            if (typeof(T) == typeof(RSARFileNode))
                folder = FindChild("Files", false);
            else if (typeof(T) == typeof(RSARTypeNode))
                folder = FindChild("Types", false);
            else if (typeof(T) == typeof(RSARGroupNode))
                folder = FindChild("Groups", false);

            if (folder != null)
                return (T)folder.Children[index];
            return null;
        }

        protected override bool OnInitialize()
        {
            if ((_name == null) && (_origPath != null))
                _name = Path.GetFileNameWithoutExtension(this._origPath);

            _files = new List<RSARFileNode>();
            _children = new List<ResourceNode>();
            OnPopulate();

            return true;
        }

        protected override void OnPopulate()
        {
            RSARFolderNode g = new RSARFolderNode();
            g.Name = "Info";
            g.Parent = this;
            g = new RSARFolderNode();
            g.Name = "Files";
            g.Parent = this;

            //Retrieve all files to attach to entries
            GetFiles();

            //Enumerate entries, attaching them to the files.
            RSARHeader* rsar = Header;
            SYMBHeader* symb = rsar->SYMBBlock;
            sbyte* offset = (sbyte*)symb + 8;
            buint* stringOffsets = symb->StringOffsets;

            VoidPtr types = (VoidPtr)rsar->INFOBlock + 8;
            ruint* typeList = (ruint*)types;

            //Iterate through group types
            for (int i = 0; i < 5; i++)
            {
                Type t = null;

                ruint* entryList = (ruint*)((uint)types + typeList[i] + 4);
                int entryCount = *((bint*)entryList - 1);
                sbyte* str, end;

                switch (i)
                {
                    case 0: t = typeof(RSARSoundNode); break;
                    case 1: t = typeof(RSARBankNode); break;
                    case 2: t = typeof(RSARTypeNode); break;
                    case 3: continue;
                    case 4: t = typeof(RSARGroupNode); break;
                }

                for (int x = 0; x < entryCount; x++)
                {
                    ResourceNode parent = Children[0];
                    RSAREntryNode n = Activator.CreateInstance(t) as RSAREntryNode;
                    n._origSource = n._uncompSource = new DataSource(types + entryList[x], 0);

                    str = offset + stringOffsets[n.StringId];

                    for (end = str; *end != 0; end++) ;
                    while ((--end > str) && (*end != '_')) ;

                    if (end > str)
                    {
                        parent = CreatePath(parent, str, (int)end - (int)str);
                        n._name = new String(end + 1);
                    }
                    else
                        n._name = new String(str);
                    n.Initialize(parent, types + entryList[x], 0);
                    //n.Populate();
                }
            }
            //Sort(true);
        }

        private void GetFiles()
        {
            INFOFileHeader* fileHeader;
            INFOFileEntry* fileEntry;
            RuintList* entryList;
            INFOGroupHeader* group;
            INFOGroupEntry* gEntry;
            RSARFileNode n;
            DataSource source;
            RSARHeader* rsar = Header;

            //SYMBHeader* symb = rsar->SYMBBlock;
            //sbyte* offset = (sbyte*)symb + 8;
            //buint* stringOffsets = symb->StringOffsets;

            //Get ruint collection from info header
            VoidPtr infoGroups = (VoidPtr)rsar->INFOBlock + 8;

            //Info has 5 groups:
            //Sounds
            //Banks
            //Types
            //Files
            //Groups

            //Convert to ruint buffer
            ruint* groups = (ruint*)infoGroups;

            //Get file ruint list at file offset and move to first entry
            ruint* fileList = (ruint*)((uint)groups + groups[3] + 4);

            //Get the count, right before the first entry
            int fileCount = *((bint*)fileList - 1);

            //Get the info list
            RuintList* groupList = rsar->INFOBlock->Groups;

            //Loop through the ruint offsets to get all files
            for (int x = 0; x < fileCount; x++)
            {
                //Get the file header for the file info
                fileHeader = (INFOFileHeader*)(infoGroups + fileList[x]);
                entryList = fileHeader->GetList(infoGroups);
                if (entryList->_numEntries == 0)
                {
                    //Must be external file.
                    n = new RSARExtFileNode();
                    source = new DataSource(fileHeader, 0);
                }
                else
                {
                    //Use first entry
                    fileEntry = (INFOFileEntry*)entryList->Get(infoGroups, 0);
                    //Find group with matching ID
                    group = (INFOGroupHeader*)groupList->Get(infoGroups, fileEntry->_groupId);
                    //Find group entry with matching index
                    gEntry = (INFOGroupEntry*)group->GetCollection(infoGroups)->Get(infoGroups, fileEntry->_index);

                    //Create node and parse
                    source = new DataSource((int)rsar + group->_headerOffset + gEntry->_headerOffset, gEntry->_headerLength);
                    if ((n = NodeFactory.GetRaw(source) as RSARFileNode) == null)
                        n = new RSARFileNode();
                    n._audioSource = new DataSource((int)rsar + group->_dataOffset + gEntry->_dataOffset, gEntry->_dataLength);
                }
                n._fileIndex = x;
                //n._parent = this; //This is so that the node won't add itself to the child list.
                n.Initialize(Children[1], source);
                _files.Add(n);
            }

            //foreach (ResourceNode r in _files)
            //    r.Populate();
        }

        internal void Sort(bool sortChildren)
        {
            if (_children != null)
            {
                _children.Sort(CompareNodes);
                if (sortChildren)
                    foreach (ResourceNode n in _children)
                        if (n is RSARFolderNode)
                            ((RSARFolderNode)n).Sort(true);
            }
        }

        internal static int CompareNodes(ResourceNode n1, ResourceNode n2)
        {
            bool is1Folder = n1 is RSARFolderNode;
            bool is2Folder = n2 is RSARFolderNode;

            if (is1Folder != is2Folder)
                return is1Folder ? -1 : 1;

            return String.Compare(n1._name, n2._name);
        }

        private ResourceNode CreatePath(ResourceNode parent, sbyte* str, int length)
        {
            ResourceNode current;

            int len;
            char* cPtr;
            sbyte* start, end;
            sbyte* ceil = str + length;
            while (str < ceil)
            {
                for (end = str; ((end < ceil) && (*end != '_')); end++) ;
                len = (int)end - (int)str;

                current = null;
                foreach (ResourceNode n in parent._children)
                {
                    if ((n._name.Length != len) || !(n is RSARFolderNode))
                        continue;

                    fixed (char* p = n._name)
                        for (cPtr = p, start = str; (start < end) && (*start == *cPtr); start++, cPtr++) ;

                    if (start == end)
                    {
                        current = n;
                        break;
                    }
                }
                if (current == null)
                {
                    current = new RSARFolderNode();
                    current._name = new String(str, 0, len);
                    current._parent = parent;
                    parent._children.Add(current);
                }

                str = end + 1;
                parent = current;
            }

            return parent;
        }

        private RSAREntryList _entryList = new RSAREntryList();
        protected override int OnCalculateSize(bool force)
        {
            _entryList.Clear();

            foreach (ResourceNode n in Children)
            {
                if (n is RSARFolderNode)
                    ((RSARFolderNode)n).GetStrings(null, 0, _entryList);
                else if (n is RSAREntryNode)
                    ((RSAREntryNode)n).GetStrings(null, 0, _entryList);
            }

            return RSARConverter.CalculateSize(_entryList);
        }

        protected internal override void OnRebuild(VoidPtr address, int length, bool force)
        {
            int symbLen, infoLen, fileLen;

            RSARHeader* rsar = (RSARHeader*)address;
            SYMBHeader* symb = (SYMBHeader*)(address + 0x40);
            INFOHeader* info;
            FILEHeader* data;

            info = (INFOHeader*)((int)symb + (symbLen = RSARConverter.EncodeSYMBBlock(symb, _entryList)));
            data = (FILEHeader*)((int)info + (infoLen = RSARConverter.EncodeINFOBlock(info, _entryList)));
            fileLen = RSARConverter.EncodeFILEBlock(data, _entryList);

            rsar->Set(symbLen, infoLen, fileLen);

            _entryList.Clear();
        }


        internal static ResourceNode TryParse(DataSource source) { return ((RSARHeader*)source.Address)->_header._tag == RSARHeader.Tag ? new RSARNode() : null; }
    }

}
