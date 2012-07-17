using System;

namespace System.Audio
{
    public interface IAudioSource
    {
        IAudioStream CreateStream();
    }
}
