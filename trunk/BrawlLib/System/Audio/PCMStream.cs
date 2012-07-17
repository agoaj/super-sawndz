using System;
using BrawlLib.IO;

namespace System.Audio
{
    public unsafe class PCMStream : IAudioStream
    {
        private FileMap _sourceMap;

        private short* _source;

        private int _bps;
        private int _numSamples;
        private int _numChannels;
        private int _frequency;
        private int _samplePos;

        private bool _looped;
        private int _loopStart;
        private int _loopEnd;

        public WaveFormatTag Format { get { return WaveFormatTag.WAVE_FORMAT_PCM; } }
        public int BitsPerSample { get { return _bps; } }
        public int Samples { get { return _numSamples; } }
        public int Channels { get { return _numChannels; } }
        public int Frequency { get { return _frequency; } }

        public bool IsLooping { get { return _looped; } set { _looped = value; } }
        public int LoopStartSample { get { return _loopStart; } set { _loopStart = value; } }
        public int LoopEndSample { get { return _loopEnd; } set { _loopEnd = value; } }

        public int SamplePosition
        {
            get { return _samplePos; }
            set { _samplePos = Math.Max(Math.Min(value, _numSamples), 0); }
        }

        internal PCMStream(FileMap map)
        {
            _sourceMap = map;

            RIFFHeader* header = (RIFFHeader*)_sourceMap.Address;
			//May be more subchunks between RIFF and data, so search for data.
			dataChunk realDataChunk = new dataChunk();
			VoidPtr dataPtr = 0;
			for( int i = 0; i < _sourceMap.Length; i++ )
			{
				byte* dat = (byte*)_sourceMap.Address.address;
				dat += i;
				if( i == 0xa6 )
					i = 0xa6;
				if(*dat == 'd')
				{
					if( *( ++dat ) == 'a' && *( ++dat ) == 't' && *( ++dat ) == 'a' )
					{
						realDataChunk._chunkTag = *( uint* ) dat - 3;
						realDataChunk._chunkSize = *( uint* ) (dat + 1);
						dataPtr = dat+5;
						break;
					}
				}
			}
            _bps = header->_fmtChunk._bitsPerSample;
            _numChannels = header->_fmtChunk._channels;
            _frequency = (int)header->_fmtChunk._samplesSec;
			_numSamples = (int)(realDataChunk._chunkSize / header->_fmtChunk._blockAlign);

			_source = ( short* )( dataPtr);
            _samplePos = 0;
        }

        public int ReadSamples(VoidPtr destAddr, int numSamples)
        {
            short* sPtr = _source + (_samplePos * _numChannels);
            short* dPtr = (short*)destAddr;

            int max = Math.Min(numSamples, _numSamples - _samplePos);

            for (int i = 0; i < max; i++)
                for (int x = 0; x < _numChannels; x++)
                    *dPtr++ = *sPtr++;

            _samplePos += max;

            return max;
        }

        public void Wrap() 
        {
            SamplePosition = _loopStart;
        }

        public void Dispose()
        {
            if (_sourceMap != null)
            {
                _sourceMap.Dispose();
                _sourceMap = null;
            }
            GC.SuppressFinalize(this);
        }

    }
}
