using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BrawlSoundConverter
{
	public class StreamSource : System.Audio.IAudioSource
	{
		public System.Audio.IAudioStream stream;
		public StreamSource()
		{
			stream = null;
		}
		public StreamSource(System.Audio.IAudioStream istream)
		{
			stream = istream;
		}
		#region IAudioSource Members

		public System.Audio.IAudioStream CreateStream()
		{
			return stream;
		}

		#endregion
	}
}
