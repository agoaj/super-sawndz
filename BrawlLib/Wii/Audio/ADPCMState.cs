using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace BrawlLib.Wii.Audio
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct ADPCMState
    {
        public byte* _srcPtr;
        public int _sampleIndex;
        public short _ps, _yn1, _yn2;
        public short[] _coefs;

        public ADPCMState(byte* srcPtr, short yn1, short yn2, short[] coefs)
        {
            _srcPtr = srcPtr;
            _sampleIndex = 0;
            _ps = 0;
            _yn1 = yn1;
            _yn2 = yn2;

            _coefs = coefs;
        }
        public ADPCMState(byte* srcPtr, short ps, short yn1, short yn2, short[] coefs)
        {
            _srcPtr = srcPtr;
            _sampleIndex = 0;
            _ps = ps;
            _yn1 = yn1;
            _yn2 = yn2;

            _coefs = coefs;
        }

        public short ReadSample()
        {
            int outSample, scale, cIndex;

            //if ((_sampleIndex == 0) && (_ps != 0))
            //    _srcPtr++;
            if (_sampleIndex % 14 == 0)
                _ps = *_srcPtr++;

            if ((_sampleIndex++ & 1) == 0)
                outSample = *_srcPtr >> 4;
            else
                outSample = *_srcPtr++ & 0x0F;

            if (outSample >= 8)
                outSample -= 16;

            scale = 1 << (_ps & 0x0F);
            cIndex = (_ps >> 4) << 1;

            outSample = (0x400 + (scale * outSample << 11) + (_coefs[cIndex] * _yn1) + (_coefs[cIndex + 1] * _yn2)) >> 11;

            //if (outSample > 32767)
            //    outSample = 32767;
            //if (outSample < -32768)
            //    outSample = -32768;

            _yn2 = _yn1;
            return _yn1 = (short)outSample.Clamp(-32768, 32767);
        }
    }
}
