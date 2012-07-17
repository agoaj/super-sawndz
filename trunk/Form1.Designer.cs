namespace BrawlSoundConverter
{
	partial class Form1
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose( bool disposing )
		{
			if( disposing && ( components != null ) )
			{
				components.Dispose();
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.textBoxOutput = new System.Windows.Forms.TextBox();
			this.treeViewMapping = new System.Windows.Forms.TreeView();
			this.textBoxGroupID = new System.Windows.Forms.TextBox();
			this.textBoxCollectionID = new System.Windows.Forms.TextBox();
			this.textBoxWavID = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.textBoxInputFile = new System.Windows.Forms.TextBox();
			this.button2 = new System.Windows.Forms.Button();
			this.button3 = new System.Windows.Forms.Button();
			this.label4 = new System.Windows.Forms.Label();
			this.backgroundWorkerInsert = new System.ComponentModel.BackgroundWorker();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openBRSARToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.button1 = new System.Windows.Forms.Button();
			this.backgroundWorkerCreateSawnd = new System.ComponentModel.BackgroundWorker();
			this.play = new System.Windows.Forms.AudioPlaybackPanel();
			this.audioPlaybackPanelWav = new System.Windows.Forms.AudioPlaybackPanel();
			this.menuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// textBoxOutput
			// 
			this.textBoxOutput.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.textBoxOutput.ForeColor = System.Drawing.Color.FromArgb( ( ( int ) ( ( ( byte ) ( 128 ) ) ) ), ( ( int ) ( ( ( byte ) ( 255 ) ) ) ), ( ( int ) ( ( ( byte ) ( 128 ) ) ) ) );
			this.textBoxOutput.Location = new System.Drawing.Point( 12, 205 );
			this.textBoxOutput.Multiline = true;
			this.textBoxOutput.Name = "textBoxOutput";
			this.textBoxOutput.ReadOnly = true;
			this.textBoxOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.textBoxOutput.Size = new System.Drawing.Size( 501, 295 );
			this.textBoxOutput.TabIndex = 1;
			// 
			// treeViewMapping
			// 
			this.treeViewMapping.FullRowSelect = true;
			this.treeViewMapping.HideSelection = false;
			this.treeViewMapping.Location = new System.Drawing.Point( 519, 12 );
			this.treeViewMapping.Name = "treeViewMapping";
			this.treeViewMapping.Size = new System.Drawing.Size( 282, 381 );
			this.treeViewMapping.TabIndex = 3;
			this.treeViewMapping.AfterSelect += new System.Windows.Forms.TreeViewEventHandler( this.treeViewMapping_NodeMouseClick );
			this.treeViewMapping.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler( this.treeViewMapping_NodeMouseClick );
			this.treeViewMapping.KeyPress += new System.Windows.Forms.KeyPressEventHandler( this.treeViewMapping_KeyPress );
			// 
			// textBoxGroupID
			// 
			this.textBoxGroupID.Location = new System.Drawing.Point( 413, 13 );
			this.textBoxGroupID.Name = "textBoxGroupID";
			this.textBoxGroupID.Size = new System.Drawing.Size( 100, 20 );
			this.textBoxGroupID.TabIndex = 4;
			this.textBoxGroupID.TextChanged += new System.EventHandler( this.textBoxGroupID_TextChanged );
			// 
			// textBoxCollectionID
			// 
			this.textBoxCollectionID.Location = new System.Drawing.Point( 413, 39 );
			this.textBoxCollectionID.Name = "textBoxCollectionID";
			this.textBoxCollectionID.Size = new System.Drawing.Size( 100, 20 );
			this.textBoxCollectionID.TabIndex = 5;
			// 
			// textBoxWavID
			// 
			this.textBoxWavID.Location = new System.Drawing.Point( 413, 65 );
			this.textBoxWavID.Name = "textBoxWavID";
			this.textBoxWavID.Size = new System.Drawing.Size( 100, 20 );
			this.textBoxWavID.TabIndex = 6;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point( 357, 16 );
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size( 50, 13 );
			this.label1.TabIndex = 8;
			this.label1.Text = "Group ID";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point( 340, 42 );
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size( 67, 13 );
			this.label2.TabIndex = 9;
			this.label2.Text = "Collection ID";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point( 357, 68 );
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size( 46, 13 );
			this.label3.TabIndex = 10;
			this.label3.Text = "WAV ID";
			// 
			// textBoxInputFile
			// 
			this.textBoxInputFile.Enabled = false;
			this.textBoxInputFile.Location = new System.Drawing.Point( 12, 39 );
			this.textBoxInputFile.Name = "textBoxInputFile";
			this.textBoxInputFile.Size = new System.Drawing.Size( 291, 20 );
			this.textBoxInputFile.TabIndex = 11;
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point( 309, 39 );
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size( 25, 23 );
			this.button2.TabIndex = 12;
			this.button2.Text = "...";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler( this.button2_Click );
			// 
			// button3
			// 
			this.button3.Location = new System.Drawing.Point( 259, 65 );
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size( 75, 23 );
			this.button3.TabIndex = 13;
			this.button3.Text = "Insert";
			this.button3.UseVisualStyleBackColor = true;
			this.button3.EnabledChanged += new System.EventHandler( this.button3_EnabledChanged );
			this.button3.Click += new System.EventHandler( this.button3_Click );
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point( 12, 23 );
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size( 50, 13 );
			this.label4.TabIndex = 15;
			this.label4.Text = "Input File";
			// 
			// backgroundWorkerInsert
			// 
			this.backgroundWorkerInsert.DoWork += new System.ComponentModel.DoWorkEventHandler( this.backgroundWorkerInsert_DoWork );
			this.backgroundWorkerInsert.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler( this.backgroundWorkerInsert_RunWorkerCompleted );
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.aboutToolStripMenuItem} );
			this.menuStrip1.Location = new System.Drawing.Point( 0, 0 );
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size( 816, 24 );
			this.menuStrip1.TabIndex = 16;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.openBRSARToolStripMenuItem} );
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size( 37, 20 );
			this.fileToolStripMenuItem.Text = "File";
			// 
			// openBRSARToolStripMenuItem
			// 
			this.openBRSARToolStripMenuItem.Name = "openBRSARToolStripMenuItem";
			this.openBRSARToolStripMenuItem.Size = new System.Drawing.Size( 141, 22 );
			this.openBRSARToolStripMenuItem.Text = "Open BRSAR";
			this.openBRSARToolStripMenuItem.Click += new System.EventHandler( this.openBRSARToolStripMenuItem_Click );
			// 
			// aboutToolStripMenuItem
			// 
			this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
			this.aboutToolStripMenuItem.Size = new System.Drawing.Size( 52, 20 );
			this.aboutToolStripMenuItem.Text = "About";
			this.aboutToolStripMenuItem.Click += new System.EventHandler( this.aboutToolStripMenuItem_Click );
			// 
			// button1
			// 
			this.button1.Enabled = false;
			this.button1.Location = new System.Drawing.Point( 418, 91 );
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size( 95, 23 );
			this.button1.TabIndex = 17;
			this.button1.Text = "Create Sawnd";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler( this.button1_Click_1 );
			// 
			// backgroundWorkerCreateSawnd
			// 
			this.backgroundWorkerCreateSawnd.DoWork += new System.ComponentModel.DoWorkEventHandler( this.backgroundWorkerCreateSawnd_DoWork );
			this.backgroundWorkerCreateSawnd.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler( this.backgroundWorkerCreateSawnd_RunWorkerCompleted );
			// 
			// play
			// 
			this.play.Location = new System.Drawing.Point( 519, 399 );
			this.play.Name = "play";
			this.play.Size = new System.Drawing.Size( 282, 111 );
			this.play.TabIndex = 18;
			// 
			// audioPlaybackPanelWav
			// 
			this.audioPlaybackPanelWav.Location = new System.Drawing.Point( 12, 88 );
			this.audioPlaybackPanelWav.Name = "audioPlaybackPanelWav";
			this.audioPlaybackPanelWav.Size = new System.Drawing.Size( 291, 101 );
			this.audioPlaybackPanelWav.TabIndex = 14;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size( 816, 512 );
			this.Controls.Add( this.play );
			this.Controls.Add( this.button1 );
			this.Controls.Add( this.label4 );
			this.Controls.Add( this.audioPlaybackPanelWav );
			this.Controls.Add( this.button3 );
			this.Controls.Add( this.button2 );
			this.Controls.Add( this.textBoxInputFile );
			this.Controls.Add( this.label3 );
			this.Controls.Add( this.label2 );
			this.Controls.Add( this.label1 );
			this.Controls.Add( this.textBoxWavID );
			this.Controls.Add( this.textBoxCollectionID );
			this.Controls.Add( this.textBoxGroupID );
			this.Controls.Add( this.treeViewMapping );
			this.Controls.Add( this.textBoxOutput );
			this.Controls.Add( this.menuStrip1 );
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "Form1";
			this.Text = "Super Sawndz";
			this.Load += new System.EventHandler( this.Form1_Load );
			this.menuStrip1.ResumeLayout( false );
			this.menuStrip1.PerformLayout();
			this.ResumeLayout( false );
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox textBoxOutput;
		private System.Windows.Forms.TreeView treeViewMapping;
		private System.Windows.Forms.TextBox textBoxGroupID;
		private System.Windows.Forms.TextBox textBoxCollectionID;
		private System.Windows.Forms.TextBox textBoxWavID;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox textBoxInputFile;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.AudioPlaybackPanel audioPlaybackPanelWav;
		private System.Windows.Forms.Label label4;
		private System.ComponentModel.BackgroundWorker backgroundWorkerInsert;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem openBRSARToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.AudioPlaybackPanel play;
		private System.ComponentModel.BackgroundWorker backgroundWorkerCreateSawnd;
	}
}

