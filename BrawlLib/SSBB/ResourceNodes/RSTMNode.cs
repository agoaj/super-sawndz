using System;
using BrawlLib.SSBBTypes;
using BrawlLib.Wii.Audio;
using System.Audio;
using System.ComponentModel;
using System.IO;
using BrawlLib.IO;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class RSTMNode : ResourceNode, IAudioSource
    {
        internal RSTMHeader* Header { get { return (RSTMHeader*)WorkingUncompressed.Address; } }
        public override ResourceType ResourceType { get { return ResourceType.RSTM; } }

        int _channels;
        bool _looped;
        int _sampleRate;
        int _loopStart;
        int _numSamples;
        int _dataOffset;
        int _numBlocks;
        int _blockSize;
        int _bps;

        [Category("Audio Stream")]
        public int Channels { get { return _channels; } }
        [Category("Audio Stream")]
        public bool IsLooped { get { return _looped; } }
        [Category("Audio Stream")]
        public int SampleRate { get { return _sampleRate; } }
        [Category("Audio Stream")]
        public int LoopStartSample { get { return _loopStart; } }
        [Category("Audio Stream")]
        public int NumSamples { get { return _numSamples; } }
        [Category("Audio Stream")]
        public int DataOffset { get { return _dataOffset; } }
        [Category("Audio Stream")]
        public int NumBlocks { get { return _numBlocks; } }
        [Category("Audio Stream")]
        public int BlockSize { get { return _blockSize; } }
        [Category("Audio Stream")]
        public int BitsPerSample { get { return _bps; } }

        public IAudioStream CreateStream()
        {
            if (Header != null)
                return new ADPCMStream(Header);
            return null;
        }

        protected override bool OnInitialize()
        {
            if ((_name == null) && (_origPath != null))
                _name = Path.GetFileNameWithoutExtension(_origPath);

            HEADPart1* part1 = Header->HEADData->Part1;

            _channels = part1->_format._channels;
            _looped = part1->_format._looped != 0;
            _sampleRate = part1->_sampleRate;
            _loopStart = part1->_loopStartSample;
            _numSamples = part1->_numSamples;
            _dataOffset = part1->_dataOffset;
            _numBlocks = part1->_numBlocks;
            _blockSize = part1->_blockSize;
            _bps = part1->_bitsPerSample;

            return false;
        }

        public override unsafe void Export(string outPath)
        {
            if (outPath.EndsWith(".wav"))
            {
                ADPCMStream stream = new ADPCMStream(Header);
                WAV.ToFile(stream, outPath);
            }
            else
                base.Export(outPath);
        }

        public override unsafe void Replace(string fileName)
        {
            IAudioStream stream = null;

            if (fileName.EndsWith(".wav"))
                stream = WAV.FromFile(fileName);
            else
                base.Replace(fileName);

            if (stream != null)
                try { ReplaceRaw(RSTMConverter.Encode(stream, null)); }
                finally { stream.Dispose(); }
        }

        internal static ResourceNode TryParse(DataSource source) { return ((RSTMHeader*)source.Address)->_header._tag == RSTMHeader.Tag ? new RSTMNode() : null; }
    }
}
