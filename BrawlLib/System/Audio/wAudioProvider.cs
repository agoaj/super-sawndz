using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using DS = System.Win32.DirectSound;

namespace System.Audio
{
    unsafe class wAudioProvider : AudioProvider
    {
        internal Win32.DirectSound.IDirectSound8 _ds8;

        internal wAudioProvider(AudioDevice device)
        {
            _device = device == null ? wAudioDevice.DefaultPlaybackDevice : device;

            Guid guid = ((wAudioDevice)_device)._guid;
            Win32.DirectSound.DirectSoundCreate8(&guid, out _ds8, IntPtr.Zero);
        }
        public override void Dispose()
        {
            base.Dispose();
            if (_ds8 != null)
            {
                Marshal.FinalReleaseComObject(_ds8);
                _ds8 = null;
            }
        }

        public override void Attach(Control owner)
        {
            _ds8.SetCooperativeLevel(owner.Handle, Win32.DirectSound.DSCooperativeLevel.Normal);
        }


        //public override AudioBuffer CreateBuffer(IAudioStream source)
        //{
        //    WaveFormatEx fmt = new WaveFormatEx(source.Format, source.Channels, source.Frequency, source.BitsPerSample);
        //    DS.DSBufferCapsFlags flags = DS.DSBufferCapsFlags.CtrlVolume | DS.DSBufferCapsFlags.LocDefer |DS.DSBufferCapsFlags.GlobalFocus | DS.DSBufferCapsFlags.GetCurrentPosition2;
        //    DS.DSBufferDesc desc = new DS.DSBufferDesc(AudioBuffer.DefaultBufferSpan * fmt.nAvgBytesPerSec, flags, &fmt, Guid.Empty);

        //    AudioBuffer buf = CreateBuffer(ref desc);
        //    buf._source = source;
        //    return buf;
        //}

        public override AudioBuffer CreateBuffer(WaveFormatTag format, int channels, int bps, int frequency, int size)
        {
            WaveFormatEx fmt = new WaveFormatEx(format, channels, frequency, bps);
            DS.DSBufferCapsFlags flags = DS.DSBufferCapsFlags.CtrlVolume | DS.DSBufferCapsFlags.LocDefer | DS.DSBufferCapsFlags.GlobalFocus | DS.DSBufferCapsFlags.GetCurrentPosition2;
            DS.DSBufferDesc desc = new DS.DSBufferDesc((uint)size, flags, &fmt, Guid.Empty);

            return new wAudioBuffer(this, ref desc);
        }

        //private AudioBuffer CreateBuffer(ref DS.DSBufferDesc desc)
        //{
        //    DS.IDirectSoundBuffer8 buf;
        //    _ds8.CreateSoundBuffer(ref desc, out buf, IntPtr.Zero);
        //    return new wAudioBuffer(this, buf);
        //}
    }
}
