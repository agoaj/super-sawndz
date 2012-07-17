using System;
using BrawlLib.SSBBTypes;
using System.IO;
using BrawlLib.IO;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class RSARFileNode : RSAREntryNode
    {
        internal INFOFileHeader* Header { get { return (INFOFileHeader*)WorkingUncompressed.Address; } }
        internal DataSource _audioSource;

        internal string _extPath;
        internal int _fileIndex;
        //internal string[] _labels;
        internal LabelItem[] _labels;

        [Browsable(true), Category("File Node")]
        public int FileNodeIndex { get { return _fileIndex; } }
        [Browsable(false)]
        public string ExtPath { get { return _extPath; } set { _extPath = value; SignalPropertyChange(); } }

        protected virtual void GetStrings(LabelBuilder builder) { }

        protected override bool OnInitialize()
        {
            if (_name == null)
            {
                if (_parent == null)
                    _name = Path.GetFileNameWithoutExtension(_origPath);
                else
                    _name = String.Format("[{0:X3}] {1}", _fileIndex, ResourceType.ToString());
            }
            return false;
        }

        public override unsafe void Export(string outPath)
        {
            LabelBuilder labl;
            int lablLen, size;
            VoidPtr addr;

            if (_audioSource != DataSource.Empty)
            {
                //Get strings
                labl = new LabelBuilder();
                GetStrings(labl);
                lablLen = (labl.Count == 0) ? 0 : labl.GetSize();
                size = WorkingUncompressed.Length + lablLen + _audioSource.Length;

                using (FileStream stream = new FileStream(outPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
                {
                    stream.SetLength(size);
                    using (FileMap map = FileMap.FromStreamInternal(stream, FileMapProtect.ReadWrite, 0, size))
                    {
                        addr = map.Address;

                        //Write header
                        Memory.Move(addr, WorkingUncompressed.Address, (uint)WorkingUncompressed.Length);

                        //Write strings
                        addr += WorkingUncompressed.Length;
                        if (lablLen > 0)
                            labl.Write(addr);
                        addr += lablLen;

                        //Write data
                        Memory.Move(addr, _audioSource.Address, (uint)_audioSource.Length);
                    }
                }
            }
            else
                base.Export(outPath);
        }

        //protected override void OnPopulate()
        //{
        //    INFOHeader* info;
        //    VoidPtr offset;
        //    RuintList* list;
        //    INFOSoundEntry* data;
        //    int count;
        //    RSARNode n = RSARNode;

        //    if (n != null)
        //    {
        //        info = n.Header->INFOBlock;
        //        offset = &info->_collection;
        //        list = info->Sounds;
        //        count = list->_numEntries;

        //        for (int i = 0; i < count; i++)
        //            if ((data = (INFOSoundEntry*)list->Get(offset, i))->_fileId == _fileIndex)
        //                new RSARSoundNode().Initialize(this, new DataSource(data, 0));
        //    }
        //}
    }
}
