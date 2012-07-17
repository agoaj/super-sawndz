using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;


namespace BrawlSoundConverter
{
	public partial class Form1 : Form
	{
		public static Form1 activeForm = null;
		string VERSION = "1.0";
		public Form1()
		{
			InitializeComponent();
		}

		private void button1_Click( object sender, EventArgs e )
		{
			textBoxOutput.Text = "Running";
			textBoxOutput.Text = Sawndz.test();
		}


		TreeNodeCollection parseMappingXML(System.Xml.XmlReader reader, TreeNodeCollection nodes)
		{
			TreeNode filler = nodes.Add("");
			MappingItem parent = (MappingItem)filler.Parent;
			nodes.Remove(filler);


			reader.Read();
			for( reader.Read(); !reader.EOF; reader.Read() )
			{
				if( !reader.IsStartElement()  || reader.Name.CompareTo( "map" ) != 0 )
					continue;

				string name = reader.GetAttribute( "name" );

				string groupID = reader.GetAttribute( "groupID" );
				string collID = reader.GetAttribute( "collectionID" );
				string wavID = reader.GetAttribute( "wavID" );
				int gID = -1;
				int cID = -1;
				int wID = -1;
				//Load defaults from parent
				if(parent != null)
				{
					gID = parent.groupID;
					cID = parent.collectionID;
					wID = parent.wavID;
				}
				if( groupID != null )
					gID = int.Parse( groupID, System.Globalization.NumberStyles.HexNumber );
				if( collID != null )
					cID = int.Parse( collID, System.Globalization.NumberStyles.HexNumber );
				if( wavID != null )
					wID = int.Parse( wavID, System.Globalization.NumberStyles.HexNumber );

				MappingItem node = new MappingItem(name, gID, cID, wID);
				nodes.Add( node );

				try
				{
					System.Xml.XmlReader subReader = reader.ReadSubtree();
					parseMappingXML( subReader, node.Nodes );
				}
				catch(System.Exception e)
				{
					//We're at the bottom of the tree
				}

			}
			return nodes;
		}


		private void loadTreeView()
		{
			try
			{
				if( treeViewMapping.Nodes.Count > 0 )
					treeViewMapping.Nodes.Clear();
				brsar.LoadTreeView( treeViewMapping );
				treeViewMapping.Invalidate();
			}
			catch( System.IO.FileNotFoundException fnf )
			{
				MessageBox.Show( "Hey, we need the smashbros_sounds.brsar to be in the same directory as the program. Go find it and put it in there!" );
				Close();
			}
			
		}

		private void Form1_Load( object sender, EventArgs e )
		{
			foreach(System.Diagnostics.Process p in System.Diagnostics.Process.GetProcesses())
			{
				if( p.ProcessName.CompareTo( "sawndz.exe" ) == 0 )
					p.Kill();
			}
			activeForm = this;
			Sawndz.consoleOutput += consoleOutput;
			try
			{
				brsar.GetRSAR();
			}
			catch( Exception ex )
			{
				//no RSAR in directory
				disableStuff();
				textBoxOutput.Text = "Select File->Open BRSAR to begin.";
				//openBRSARToolStripMenuItem_Click( this, EventArgs.Empty );
			}
			
			Control.CheckForIllegalCrossThreadCalls = false;
			audioPlaybackPanelWav.TargetSource = null;
			play.TargetSource = null;
			//play = new AudioPlaybackPanel();
			//panel1.Controls.Add( play );
			//XmlTextReader reader = new XmlTextReader( "mapping.xml" );
			//reader.WhitespaceHandling = WhitespaceHandling.None;
			//skip xml header
			//reader.Read();
			//parseMappingXML( reader, treeViewMapping.Nodes );
		}

		private void treeViewMapping_NodeMouseClick( object sender, TreeNodeMouseClickEventArgs e )
		{
			
		}

		private void treeViewMapping_NodeMouseClick( object sender, TreeViewEventArgs e )
		{
			MappingItem item = ( MappingItem ) e.Node;
			if( item.groupID > -1 )
				textBoxGroupID.Text = item.groupID.ToString();
			else
				textBoxGroupID.Text = "";
			if( item.collectionID > -1 )
				textBoxCollectionID.Text = item.collectionID.ToString();
			else
				textBoxCollectionID.Text = "";
			if( item.wavID > -1 )
				textBoxWavID.Text = item.wavID.ToString();
			else
				textBoxWavID.Text = "";

			if( item.soundPath != null )
			{
				play.TargetSource = item as System.Audio.IAudioSource;
				play.Play();
			}
			else
				play.TargetSource = null;
			
		}

		private void treeViewMapping_KeyPress( object sender, KeyPressEventArgs e )
		{
			if(e.KeyChar == '\r' && play.TargetSource != null )
				play.Play();
		}

		private void button2_Click( object sender, EventArgs e )
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Filter = "Wave File(*.wav)|*.wav|Sawndz File(*.sawnd)|*.sawnd";

			if( ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK )
			{
				textBoxInputFile.Text = ofd.FileName;
				if( Path.GetExtension( ofd.FileName ).CompareTo( ".wav" ) == 0 )
				{
					audioPlaybackPanelWav.TargetSource = new StreamSource(System.Audio.WAV.FromFile( ofd.FileName ));
				}
				else
					audioPlaybackPanelWav.TargetSource = null;

			}

		}

		private void insert()
		{
			
			try
			{
				//button3.Enabled = false;
				//this.Invalidate();
				if( Path.GetExtension( textBoxInputFile.Text ).CompareTo( ".sawnd" ) == 0 )
				{
					Sawndz.insertSawnd( textBoxInputFile.Text );
					//loadTreeView();
					//textBoxOutput.Invalidate();
				}
				else if( Path.GetExtension( textBoxInputFile.Text ).CompareTo( ".wav" ) == 0 )
				{
					int gid, cid, wid;
					if( int.TryParse( textBoxGroupID.Text, out gid ) )
					{
						if( int.TryParse( textBoxCollectionID.Text, out cid ) )
						{
							if( int.TryParse( textBoxWavID.Text, out wid ) )
							{
								if( gid > -1 && cid > -1 && wid > -1 )
								{
									//textBoxOutput.Text = "Inserting Wav file: " + textBoxInputFile.Text;
									//textBoxOutput.Invalidate();
									Sawndz.insertWav( textBoxInputFile.Text, gid, cid, wid );
									//textBoxOutput.Invalidate();
									//MappingItem node = treeViewMapping.SelectedNode as MappingItem;
									//node.updateSize();
								}
							}
						}
					}

				}
			}
			catch( Exception e )
			{
				this.Invoke(new Sawndz.ConsoleOutput(consoleOutput), new object[] {e.Message});
			}
			//loadTreeView();
			//button3.Enabled = true;
		}
		System.Threading.Thread insertThread;
		private void button3_Click( object sender, EventArgs e )
		{
			disableStuff();
			textBoxOutput.Text = "";
			backgroundWorkerInsert.RunWorkerAsync();
			//insert();
			//insertThread = new System.Threading.Thread( insert );
			
			//insertThread.Start();
		}

		private void button3_EnabledChanged( object sender, EventArgs e )
		{

		}


		void consoleOutput(string data)
		{

			textBoxOutput.Text += data;
			textBoxOutput.SelectionStart = textBoxOutput.Text.Length;
			textBoxOutput.ScrollToCaret();
			textBoxOutput.Refresh();
		}


		delegate void GenerateSoundName( TreeNode node );
		void generateSoundName( TreeNode node )
		{
			MappingItem item = node as MappingItem;
			item.generateName();
		}
		void recurseSoundNames( TreeNode node )
		{
			treeViewMapping.Invoke( new GenerateSoundName( generateSoundName ), new object[] { node } );
			foreach( TreeNode subNode in node.Nodes )
				recurseSoundNames( subNode );
			if( node.Parent == null )
				node.TreeView.Invalidate();
		}
		private void backgroundWorkerBRSAR_DoWork( object sender, DoWorkEventArgs e )
		{

		}
		private void disableStuff()
		{
			play.Stop();
			textBoxGroupID.Enabled = false;
			textBoxCollectionID.Enabled = false;
			textBoxWavID.Enabled = false;
			audioPlaybackPanelWav.Stop();
			button1.Enabled = false;
			button3.Enabled = false;
			treeViewMapping.Enabled = false;
			audioPlaybackPanelWav.Enabled = false;
			play.Enabled = false;
			brsar.CloseRSAR();
		}
		private void enableStuff()
		{
			button3.Enabled = true;
			textBoxGroupID.Enabled = true;
			textBoxCollectionID.Enabled = true;
			textBoxWavID.Enabled = true;
			treeViewMapping.Enabled = true;
			audioPlaybackPanelWav.Enabled = true;
			play.Enabled = true;

			int gid;
			button1.Enabled = int.TryParse( textBoxGroupID.Text, out gid );
		}
		private void backgroundWorkerInsert_RunWorkerCompleted( object sender, RunWorkerCompletedEventArgs e )
		{
			enableStuff();
			if( Path.GetExtension( textBoxInputFile.Text ).CompareTo( ".wav" ) == 0 )
				( treeViewMapping.SelectedNode as MappingItem ).updateSize();
			else
				loadTreeView();
		}

		private void backgroundWorkerInsert_DoWork( object sender, DoWorkEventArgs e )
		{
			insert();
		}

		private void aboutToolStripMenuItem_Click( object sender, EventArgs e )
		{
			MessageBox.Show( "Super Sawndz Version:" + VERSION +"\nCreated by Agoaj\nwww.agoaj.com\n" + 
				"Uses BrawlLib: http://code.google.com/p/brawltools2/ \n" + 
				"Based off of Sawndz 0.12\n2010-2011 Jaklub\nspecial thanks to mastkalo, ssbbtailsfan, stickman, VILE\n");
		}

		private void openBRSARToolStripMenuItem_Click( object sender, EventArgs e )
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Filter = "Sound Archive(*.brsar)|*.brsar";
			if( ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK )
			{
				brsar.CloseRSAR();
				brsar.RSAR_FileName = ofd.FileName;
				loadTreeView();
				enableStuff();
			}

		}

		private void button1_Click_1( object sender, EventArgs e )
		{
			int gid;
			if( !int.TryParse( textBoxGroupID.Text, out gid ) )
			{
				MessageBox.Show( "Group ID is not valid" );
				return;
			}
			SaveFileDialog sfd = new SaveFileDialog();
			sfd.Filter = "*Sawnd File(*.sawnd)|*.sawnd";
			if( sfd.ShowDialog() == DialogResult.OK )
			{
				disableStuff();
				textBoxOutput.Text = "";
				backgroundWorkerCreateSawnd.RunWorkerAsync(sfd.FileName);
			}
		}

		private void backgroundWorkerCreateSawnd_DoWork( object sender, DoWorkEventArgs e )
		{
			Sawndz.createSawnd( int.Parse( textBoxGroupID.Text ), e.Argument as string );
		}

		private void textBoxGroupID_TextChanged( object sender, EventArgs e )
		{
			int gid;
			button1.Enabled = int.TryParse( textBoxGroupID.Text, out gid );
		}

		private void backgroundWorkerCreateSawnd_RunWorkerCompleted( object sender, RunWorkerCompletedEventArgs e )
		{
			enableStuff();
		}
	}
}
