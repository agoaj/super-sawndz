using System;
using BrawlLib.SSBBTypes;
using System.Audio;
using BrawlLib.Wii.Audio;
using System.IO;
using BrawlLib.IO;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class RWSDSoundNode : RWSDEntryNode, IAudioSource
    {
        internal RWSD_WAVEEntry* Header { get { return (RWSD_WAVEEntry*)WorkingUncompressed.Address; } }

        internal VoidPtr _dataAddr;

        protected override bool OnInitialize()
        {
            if (_parent._parent is RWSDNode)
                _dataAddr = ((RWSDNode)_parent._parent)._audioSource.Address + Header->_offset;
            else
                _dataAddr = ((RBNKNode)_parent._parent)._audioSource.Address + Header->_offset;

            if (_name == null)
                _name = string.Format("Audio[{0:X2}]", Index);

            return false;
        }

        IAudioStream stream;
        public IAudioStream CreateStream()
        {
            if (stream != null)
                return stream;
            return stream = new ADPCMStream(Header, _dataAddr);
        }

        public override unsafe void Replace(string fileName)
        {
            stream = null;

            if (fileName.EndsWith(".wav"))
                stream = WAV.FromFile(fileName);
            else
                base.Replace(fileName);

            if (stream != null)
                try { ReplaceRaw(RSTMConverter.Encode(stream, null)); }
                finally { stream.Dispose(); }
        }

        public override unsafe void Export(string outPath)
        {
            if (outPath.EndsWith(".wav"))
                WAV.ToFile(CreateStream(), outPath);
            else
                base.Export(outPath);
        }

        protected override int OnCalculateSize(bool force)
        {
            return base.OnCalculateSize(force);
        }

        protected internal override void OnRebuild(VoidPtr address, int length, bool force)
        {
            base.OnRebuild(address, length, force);
        }
    }
}
