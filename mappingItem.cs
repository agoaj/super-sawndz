using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections.Generic;
using System.ComponentModel;

namespace BrawlSoundConverter
{
	class MappingItem : TreeNode, System.Audio.IAudioSource
	{
		public int groupID, collectionID, wavID;
		public System.Audio.IAudioStream sound;
		public string groupPath;
		public string soundPath;
		public string name;
		int _fileSize;
		public int fileSize
		{
			get
			{
				return _fileSize;
				/*
				if( this.Nodes.Count == 0 )
					return _fileSize;
				else
				{
					_fileSize = 0;
					foreach( MappingItem item in Nodes )
					{
						_fileSize += item.fileSize;
					}
				}
				if( _fileSize >= 0xDDDDD )
					this.BackColor = System.Drawing.Color.Red;
				else
					this.BackColor = System.Drawing.Color.White;
				Text = name + " (" + _fileSize.ToString( "X" ) + ")";
				return _fileSize;
				 */
			}
			set
			{
				MappingItem p = Parent as MappingItem;
				if(p != null)
					p.sizeOfChildChanged( value - _fileSize );
				_fileSize = value;
				//Text = name + " (" + _fileSize.ToString( "X" ) + ")";
				if( _fileSize >= 0xDDDDD )
					this.BackColor = System.Drawing.Color.Red;
				else
					this.BackColor = System.Drawing.Color.White;
				generateName();
				//this.TreeView.Invalidate();

			}
		}
		void sizeOfChildChanged( int diff )
		{
			fileSize += diff;
		}


		void _generateName()
		{
			lock(this)
			{
				if( fileSize != _prevFileSize )
				{
					//System.Threading.Monitor.Wait( this );
					Text = name + " (" + _fileSize.ToString( "X" ) + ")";
					_prevFileSize = fileSize;
					//System.Threading.Monitor.Pulse( this );
				}
			}
		}
		int _prevFileSize = 0;
		string oldName = "";
		//Loads Text with the name and filesize, run after adding child nodes/ setting size
		public string generateName()
		{
			if( fileSize != _prevFileSize )
			{
				oldName = name + " (" + _fileSize.ToString( "X" ) + ")";
				Text = oldName;
				_prevFileSize = fileSize;
			}
			return oldName;

		}
		public void updateSize()
		{
			if( soundPath == null || wavID == -1 )
				return;

			BrawlLib.SSBB.ResourceNodes.RWSDSoundNode sound = brsar.GetNode( groupID, collectionID, wavID ) as BrawlLib.SSBB.ResourceNodes.RWSDSoundNode;

			unsafe
			{
				int samples = sound.Header->NumSamples;
				if( ( samples / 2 * 2 ) == samples )
				{
					fileSize = samples / 2;
				}
				else
				{
					fileSize = samples / 2 + 1;
				}
			}
			MappingItem p = this;
			/*
			while( p.Parent != null )
				p = p.Parent as MappingItem;
			int thatdoesit = p.fileSize;
			Text = name + " (" + _fileSize.ToString( "X" ) + ")";
			*/
			brsar.CloseRSAR();
			this.TreeView.Invalidate();
		}
		public MappingItem(string name, int group = -1, int collection = -1, int wave = -1)
		{
			this.name = name;
			this.Text = name;
			this.groupID = group;
			this.collectionID = collection;
			this.wavID = wave;
			soundPath = null;
			sound = null;
			_fileSize = 0;
		}

		#region IAudioSource Members

		static int soundBufferSize = 1024 * 2000; //Allocate 2 MB buffer for sound caching, just to be safe
		static unsafe VoidPtr soundData = Memory.Alloc( soundBufferSize );

		public unsafe System.Audio.IAudioStream CreateStream()
		{
			if( soundPath == null || wavID == -1 )
				return null;


			BrawlLib.SSBB.ResourceNodes.RWSDSoundNode sound = brsar.GetNode( groupID, collectionID, wavID ) as BrawlLib.SSBB.ResourceNodes.RWSDSoundNode;
			BrawlLib.Wii.Audio.ADPCMStream stream;
			BrawlLib.SSBBTypes.RWSD_WAVEEntry header = new BrawlLib.SSBBTypes.RWSD_WAVEEntry();
			header = *sound.Header;
			if( header.NumSamples > soundBufferSize )
			{
				int a = 324;
				System.Windows.Forms.MessageBox.Show( "Sound file is too big for playback: " + header.NumSamples.ToString() + "b / " + soundBufferSize.ToString() + "b" );
				sound.Dispose();
				//rsar.Dispose();
				return null;
			}
			Memory.Copy( sound._dataAddr, soundData, header.NumSamples );
			stream = new BrawlLib.Wii.Audio.ADPCMStream( &header, soundData );
			//sound.Dispose();
			//rsar.Dispose();
			brsar.CloseRSAR();
			return stream;
		}

		#endregion
	}
}
