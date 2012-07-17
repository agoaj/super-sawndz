using System;
using BrawlLib.SSBBTypes;
using System.ComponentModel;
using System.Audio;
using BrawlLib.Wii.Audio;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class RSARSoundNode : RSAREntryNode, IAudioSource
    {
        internal INFOSoundEntry* Header { get { return (INFOSoundEntry*)WorkingUncompressed.Address; } }
        [Category("RSAR Sound")]
        internal override int StringId { get { return Header->_stringId; } }

        public override ResourceType ResourceType { get { return ResourceType.RSARSound; } }

        INFOSoundPart1 _part1;
        INFOSoundPart2 _part2;
        
        ResourceNode _soundNode;

        //internal VoidPtr _dataAddr;

        //[Category("RSAR Sound")]
        //public int StringId { get { return Header->_stringId; } }
        [Category("RSAR Sound")]
        public int FileId { get { return Header->_fileId; } }
        [Category("RSAR Sound")]
        public int Unknown1 { get { return Header->_unk1; } }
        [Category("RSAR Sound")]
        public byte Flag1 { get { return Header->_flag1; } }
        [Category("RSAR Sound")]
        public byte Flag2 { get { return Header->_flag2; } }
        [Category("RSAR Sound")]
        public byte Flag3 { get { return Header->_flag3; } }
        [Category("RSAR Sound")]
        public byte Flag4 { get { return Header->_flag4; } }
        [Category("RSAR Sound")]
        public int Unknown2 { get { return Header->_unk2; } }
        [Category("RSAR Sound")]
        public int Unknown3 { get { return Header->_unk3; } }
        [Category("RSAR Sound")]
        public int Unknown4 { get { return Header->_unk4; } }

        [Category("RSAR Sound Part 1")]
        public int P1Unk1 { get { return _part1._unk1; } }
        [Category("RSAR Sound Part 1")]
        public int P1Unk2 { get { return _part1._unk2; } }
        [Category("RSAR Sound Part 1")]
        public int P1Unk3 { get { return _part1._unk3; } }
        
        [Category("RSAR Sound Part 2")]
        public int PackIndex { get { return _part2._soundIndex; } }
        [Category("RSAR Sound Part 2")]
        public int P2Unk1 { get { return _part2._unk1; } }
        [Category("RSAR Sound Part 2")]
        public int P2Unk2 { get { return _part2._unk2; } }
        [Category("RSAR Sound Part 2")]
        public int P2Unk3 { get { return _part2._unk3; } }

        protected override bool OnInitialize()
        {
            base.OnInitialize();

            INFOHeader* info = RSARNode.Header->INFOBlock;
            _part1 = *Header->GetPart1(&info->_collection);
            _part2 = *Header->GetPart2(&info->_collection);

            _soundNode = RSARNode.Files[FileId];

            return false;
        }

        public IAudioStream CreateStream()
        {
            if (_soundNode == null)
                return null;

            if (_soundNode is RWSDNode)
            {
                RWSDDataNode d = _soundNode.Children[0].Children[PackIndex] as RWSDDataNode;
                RWSDSoundNode s = _soundNode.Children[1].Children[d.Part3[0].Index] as RWSDSoundNode;
                return s.CreateStream();
            }
            else
                return null;
        }

        public override unsafe void Export(string outPath)
        {
            if (outPath.EndsWith(".wav"))
                WAV.ToFile(CreateStream(), outPath);
            else
                base.Export(outPath);
        }
    }
}
