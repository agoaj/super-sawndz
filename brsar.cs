using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BrawlLib;
namespace BrawlSoundConverter
{
	class brsar
	{
		public static string RSAR_FileName = "smashbros_sound.brsar";
		static BrawlLib.SSBB.ResourceNodes.RSARNode _rsar;
		public static BrawlLib.SSBB.ResourceNodes.RSARNode GetRSAR()
		{
			if( _rsar == null )
			{
				_rsar = new BrawlLib.SSBB.ResourceNodes.RSARNode();
				_rsar.Replace( RSAR_FileName );
			}
			return _rsar;
		}
		public static void CloseRSAR()
		{
			if( _rsar != null )
			{
				_rsar.Dispose();
				_rsar = null;
			}
		}
		//Returns an rsargroup, rsarfile, or rwsdsound node
		public static BrawlLib.SSBB.ResourceNodes.ResourceNode GetNode(int gid, int colid = -1, int wavid = -1)
		{
			BrawlLib.SSBB.ResourceNodes.RSARNode rsar = GetRSAR();
			BrawlLib.SSBB.ResourceNodes.RSARFolderNode folder = ( BrawlLib.SSBB.ResourceNodes.RSARFolderNode ) rsar.FindChild( "Info/snd/group", false );
			BrawlLib.SSBB.ResourceNodes.ResourceNode[] groups = folder.FindChildrenByType( "", BrawlLib.SSBB.ResourceNodes.ResourceType.RSARGroup );
			BrawlLib.SSBB.ResourceNodes.RSARGroupNode group = null;
			foreach( BrawlLib.SSBB.ResourceNodes.RSARGroupNode g in groups )
			{
				if( g.Id == gid )
				{
					group = g;
					break;
				}
			}
			if( colid == -1 )
				return group;
			BrawlLib.SSBB.ResourceNodes.RSARFileNode collection = null;
			foreach( BrawlLib.SSBB.ResourceNodes.RSARFileNode file in group.Files )
			{
				if( file.FileNodeIndex == colid )
				{
					collection = file;
					break;
				}
			}
			if( wavid == -1 )
				return collection;
			BrawlLib.SSBB.ResourceNodes.RWSDGroupNode audioFolder = ( BrawlLib.SSBB.ResourceNodes.RWSDGroupNode ) collection.FindChild( "audio", false );
			return audioFolder.Children[ wavid ];

		}
		delegate void AddNode( TreeNodeCollection nodes, TreeNode node );
		static void addNode( TreeNodeCollection nodes, TreeNode node )
		{
			nodes.Add( node );
		}
		public static void LoadTreeView( TreeView treeView )
		{
			int nodeCount = 0;
			StringBuilder sb = new StringBuilder();
			BrawlLib.SSBB.ResourceNodes.RSARNode rsar = GetRSAR();
			BrawlLib.SSBB.ResourceNodes.RSARFolderNode folder = (BrawlLib.SSBB.ResourceNodes.RSARFolderNode)rsar.FindChild("Info/snd/group",false);
			BrawlLib.SSBB.ResourceNodes.ResourceNode[] groups = folder.FindChildrenByType( "", BrawlLib.SSBB.ResourceNodes.ResourceType.RSARGroup );
			
			//Create root node and add all nodes to it.
			//Adding to the treeView collection will raise events, causing super slowdown when setting Text property.
			TreeNode root = new TreeNode();
			TreeNodeCollection nodes = root.Nodes;//treeView.Nodes;
			foreach( BrawlLib.SSBB.ResourceNodes.RSARGroupNode group in groups )
			{
				string name = group.Name;
				
				int groupID = group.Id;
				MappingItem groupMap = new MappingItem( name, groupID );

				if( treeView.InvokeRequired )
				{
					treeView.Invoke( new AddNode( addNode ), new object[] { nodes, groupMap } );
					//nodes.Add( groupMap );
				}
				else
					nodes.Add( groupMap );
				nodeCount++;
				//groupMap.fileSize = 0;
				
				foreach( BrawlLib.SSBB.ResourceNodes.RSARFileNode file in group.Files )
				{
					//int fileSize = file.CalculateSize( true);
					//groupMap.fileSize += fileSize;
					string fName = file.Name;
					int collectionID = file.FileNodeIndex;
					BrawlLib.SSBB.ResourceNodes.RWSDGroupNode audioFolder = ( BrawlLib.SSBB.ResourceNodes.RWSDGroupNode ) file.FindChild( "audio", false );
					if( audioFolder == null || audioFolder.Children.Count == 0 )
						continue;
					MappingItem colMap = new MappingItem( fName, groupID, collectionID );

					if( treeView.InvokeRequired )
					{
						treeView.Invoke( new AddNode( addNode ), new object[] { groupMap.Nodes, colMap } );
						//groupMap.Nodes.Add( colMap );
					}
					else
						groupMap.Nodes.Add( colMap );
					nodeCount++;

					if( audioFolder == null )
						continue;
					int addUpSoundSize = 0;
					//BrawlLib.SSBB.ResourceNodes.ResourceNode[] sounds = file.FindChild("audio", false).FindChildrenByType( "", BrawlLib.SSBB.ResourceNodes.ResourceType.RSARSound );
				
					for(int i = 0; i < audioFolder.Children.Count; i++)
					{
						if(!(audioFolder.Children[i] is BrawlLib.SSBB.ResourceNodes.RWSDSoundNode))
							continue;
						BrawlLib.SSBB.ResourceNodes.RWSDSoundNode sound = (BrawlLib.SSBB.ResourceNodes.RWSDSoundNode)audioFolder.Children[i];
						int soundSize = 0;//sound.CalculateSize( true );
						unsafe
						{
							int samples = sound.Header->NumSamples;
							if( ( samples / 2 * 2 ) == samples )
							{
								soundSize = samples / 2;
							}
							else
							{
								soundSize = samples / 2 + 1;
							}
						}
						addUpSoundSize += soundSize;
						//string sname = sound.Name + " (" + soundSize.ToString("X") + ")";
						
						MappingItem soundMap = new MappingItem(sound.Name, groupID, collectionID,i);
						soundMap.soundPath = sound.TreePathAbsolute.Substring(1);
						soundMap.groupPath = group.TreePathAbsolute.Substring(1);
						//soundMap.sbOffset = sb.Length;
						//sb.AppendLine( sound.Name + " (" + soundSize.ToString( "X" ) + ")" );
						
						if( treeView.InvokeRequired )
						{
							treeView.Invoke( new AddNode( addNode ), new object[] { colMap.Nodes, soundMap } );
							//colMap.Nodes.Add( soundMap );
						}
						else
							colMap.Nodes.Add( soundMap );
						nodeCount++;
						//child node must have a parent in order for size to propogate correctly.
						soundMap.fileSize = soundSize;
						soundMap.generateName();
						/*
						if( sound is System.Audio.IAudioSource)
						{
							System.Windows.Forms.AudioPlaybackPanel play = new AudioPlaybackPanel();
							play.TargetSource = sound as System.Audio.IAudioSource;
							play.Show();
						}
						*/
						
						//System.Windows.Forms.AudioPlaybackPanel play = new AudioPlaybackPanel();
						
					}
					int a = 23;
					a++;
					colMap.generateName();
					//int csize = colMap.fileSize;
					//colMap.name = file.Name + " (" + colMap.fileSize.ToString( "X" ) + ")";
				}
				if( groupMap.Nodes.Count == 0 )
				{
					nodes.Remove( groupMap );
					nodeCount -= 2;
					continue;
				}
				
				groupMap.generateName();
				int gsize = groupMap.fileSize;
				//groupMap.Text += " ("+groupMap.fileSize.ToString( "X" ) + ")";
			}
			foreach( TreeNode node in nodes )
				treeView.Nodes.Add( node );
			CloseRSAR();
		}

	}
}
