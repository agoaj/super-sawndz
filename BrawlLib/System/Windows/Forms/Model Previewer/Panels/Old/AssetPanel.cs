using System;
using BrawlLib.SSBB.ResourceNodes;
using System.Drawing;
using BrawlLib.Modeling;
using System.ComponentModel;
using BrawlLib.OpenGL;
using BrawlLib;
using System.IO;
using BrawlBox;
using System.Collections.Generic;

namespace System.Windows.Forms
{
    public class ModelAssetPanel : UserControl
    {
        #region Designer

        public CheckedListBox lstPolygons;
        private CheckBox chkAllPoly;
        private Button btnPolygons;
        private ProxySplitter spltPolygons;
        private Panel pnlTextures;
        private CheckedListBox lstTextures;
        private CheckBox chkAllTextures;
        private Button btnTextures;
        private Panel pnlBones;
        private CheckedListBox lstBones;
        private CheckBox chkAllBones;
        private Button btnBones;
        private ProxySplitter spltBones;
        private ContextMenuStrip ctxTextures;
        private ContextMenuStrip ctxBones;
        private ToolStripMenuItem sourceToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem1;
        private ToolStripMenuItem viewToolStripMenuItem;
        private ToolStripMenuItem replaceToolStripMenuItem;
        private ToolStripMenuItem sizeToolStripMenuItem;
        private ToolStripMenuItem resetToolStripMenuItem;
        private ToolStripMenuItem renameBoneToolStripMenuItem;
        private ToolStripMenuItem renameTextureToolStripMenuItem;
        private ToolStripMenuItem exportToolStripMenuItem;
        private IContainer components;
        private ToolStripMenuItem boneIndex;
        private CheckBox chkSyncVis;
        private Panel pnlPolygons;

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pnlPolygons = new System.Windows.Forms.Panel();
            this.lstPolygons = new System.Windows.Forms.CheckedListBox();
            this.chkAllPoly = new System.Windows.Forms.CheckBox();
            this.chkSyncVis = new System.Windows.Forms.CheckBox();
            this.btnPolygons = new System.Windows.Forms.Button();
            this.pnlTextures = new System.Windows.Forms.Panel();
            this.lstTextures = new System.Windows.Forms.CheckedListBox();
            this.ctxTextures = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.sourceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.replaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renameTextureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.chkAllTextures = new System.Windows.Forms.CheckBox();
            this.btnTextures = new System.Windows.Forms.Button();
            this.ctxBones = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.boneIndex = new System.Windows.Forms.ToolStripMenuItem();
            this.renameBoneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pnlBones = new System.Windows.Forms.Panel();
            this.lstBones = new System.Windows.Forms.CheckedListBox();
            this.chkAllBones = new System.Windows.Forms.CheckBox();
            this.btnBones = new System.Windows.Forms.Button();
            this.spltBones = new System.Windows.Forms.ProxySplitter();
            this.spltPolygons = new System.Windows.Forms.ProxySplitter();
            this.pnlPolygons.SuspendLayout();
            this.pnlTextures.SuspendLayout();
            this.ctxTextures.SuspendLayout();
            this.ctxBones.SuspendLayout();
            this.pnlBones.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlPolygons
            // 
            this.pnlPolygons.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlPolygons.Controls.Add(this.lstPolygons);
            this.pnlPolygons.Controls.Add(this.chkAllPoly);
            this.pnlPolygons.Controls.Add(this.chkSyncVis);
            this.pnlPolygons.Controls.Add(this.btnPolygons);
            this.pnlPolygons.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlPolygons.Location = new System.Drawing.Point(0, 0);
            this.pnlPolygons.MinimumSize = new System.Drawing.Size(0, 21);
            this.pnlPolygons.Name = "pnlPolygons";
            this.pnlPolygons.Size = new System.Drawing.Size(98, 143);
            this.pnlPolygons.TabIndex = 0;
            // 
            // lstPolygons
            // 
            this.lstPolygons.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lstPolygons.CausesValidation = false;
            this.lstPolygons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstPolygons.IntegralHeight = false;
            this.lstPolygons.Location = new System.Drawing.Point(0, 61);
            this.lstPolygons.Margin = new System.Windows.Forms.Padding(0);
            this.lstPolygons.Name = "lstPolygons";
            this.lstPolygons.Size = new System.Drawing.Size(96, 80);
            this.lstPolygons.TabIndex = 4;
            this.lstPolygons.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.lstPolygons_ItemCheck);
            this.lstPolygons.SelectedValueChanged += new System.EventHandler(this.lstPolygons_SelectedValueChanged);
            this.lstPolygons.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lstPolygons_KeyDown);
            // 
            // chkAllPoly
            // 
            this.chkAllPoly.Checked = true;
            this.chkAllPoly.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAllPoly.Dock = System.Windows.Forms.DockStyle.Top;
            this.chkAllPoly.Location = new System.Drawing.Point(0, 41);
            this.chkAllPoly.Margin = new System.Windows.Forms.Padding(0);
            this.chkAllPoly.Name = "chkAllPoly";
            this.chkAllPoly.Padding = new System.Windows.Forms.Padding(1, 0, 0, 0);
            this.chkAllPoly.Size = new System.Drawing.Size(96, 20);
            this.chkAllPoly.TabIndex = 5;
            this.chkAllPoly.Text = "All";
            this.chkAllPoly.UseVisualStyleBackColor = false;
            this.chkAllPoly.CheckStateChanged += new System.EventHandler(this.chkAllPoly_CheckStateChanged);
            // 
            // chkSyncVis
            // 
            this.chkSyncVis.Dock = System.Windows.Forms.DockStyle.Top;
            this.chkSyncVis.Location = new System.Drawing.Point(0, 21);
            this.chkSyncVis.Margin = new System.Windows.Forms.Padding(0);
            this.chkSyncVis.Name = "chkSyncVis";
            this.chkSyncVis.Padding = new System.Windows.Forms.Padding(1, 0, 0, 0);
            this.chkSyncVis.Size = new System.Drawing.Size(96, 20);
            this.chkSyncVis.TabIndex = 7;
            this.chkSyncVis.Text = "Sync VIS0";
            this.chkSyncVis.UseVisualStyleBackColor = false;
            this.chkSyncVis.CheckedChanged += new System.EventHandler(this.chkSyncVis_CheckedChanged);
            // 
            // btnPolygons
            // 
            this.btnPolygons.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnPolygons.Location = new System.Drawing.Point(0, 0);
            this.btnPolygons.Name = "btnPolygons";
            this.btnPolygons.Size = new System.Drawing.Size(96, 21);
            this.btnPolygons.TabIndex = 6;
            this.btnPolygons.Text = "Polygons";
            this.btnPolygons.UseVisualStyleBackColor = true;
            this.btnPolygons.Click += new System.EventHandler(this.btnPolygons_Click);
            // 
            // pnlTextures
            // 
            this.pnlTextures.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlTextures.Controls.Add(this.lstTextures);
            this.pnlTextures.Controls.Add(this.chkAllTextures);
            this.pnlTextures.Controls.Add(this.btnTextures);
            this.pnlTextures.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlTextures.Location = new System.Drawing.Point(0, 322);
            this.pnlTextures.MinimumSize = new System.Drawing.Size(0, 21);
            this.pnlTextures.Name = "pnlTextures";
            this.pnlTextures.Size = new System.Drawing.Size(98, 150);
            this.pnlTextures.TabIndex = 2;
            // 
            // lstTextures
            // 
            this.lstTextures.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lstTextures.ContextMenuStrip = this.ctxTextures;
            this.lstTextures.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstTextures.FormattingEnabled = true;
            this.lstTextures.Location = new System.Drawing.Point(0, 41);
            this.lstTextures.Name = "lstTextures";
            this.lstTextures.Size = new System.Drawing.Size(96, 107);
            this.lstTextures.TabIndex = 8;
            this.lstTextures.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.lstTextures_ItemCheck);
            this.lstTextures.SelectedValueChanged += new System.EventHandler(this.lstTextures_SelectedValueChanged);
            this.lstTextures.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lstTextures_KeyDown);
            this.lstTextures.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lstTextures_MouseDown);
            // 
            // ctxTextures
            // 
            this.ctxTextures.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sourceToolStripMenuItem,
            this.sizeToolStripMenuItem,
            this.toolStripMenuItem1,
            this.viewToolStripMenuItem,
            this.exportToolStripMenuItem,
            this.replaceToolStripMenuItem,
            this.renameTextureToolStripMenuItem,
            this.resetToolStripMenuItem});
            this.ctxTextures.Name = "ctxTextures";
            this.ctxTextures.Size = new System.Drawing.Size(125, 164);
            this.ctxTextures.Opening += new System.ComponentModel.CancelEventHandler(this.ctxTextures_Opening);
            // 
            // sourceToolStripMenuItem
            // 
            this.sourceToolStripMenuItem.Enabled = false;
            this.sourceToolStripMenuItem.Name = "sourceToolStripMenuItem";
            this.sourceToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.sourceToolStripMenuItem.Text = "Source";
            // 
            // sizeToolStripMenuItem
            // 
            this.sizeToolStripMenuItem.Enabled = false;
            this.sizeToolStripMenuItem.Name = "sizeToolStripMenuItem";
            this.sizeToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.sizeToolStripMenuItem.Text = "Size";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(121, 6);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.viewToolStripMenuItem.Text = "View...";
            this.viewToolStripMenuItem.Click += new System.EventHandler(this.viewToolStripMenuItem_Click);
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.exportToolStripMenuItem.Text = "Export...";
            this.exportToolStripMenuItem.Click += new System.EventHandler(this.exportToolStripMenuItem_Click);
            // 
            // replaceToolStripMenuItem
            // 
            this.replaceToolStripMenuItem.Name = "replaceToolStripMenuItem";
            this.replaceToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.replaceToolStripMenuItem.Text = "Replace...";
            this.replaceToolStripMenuItem.Click += new System.EventHandler(this.replaceToolStripMenuItem_Click);
            // 
            // renameTextureToolStripMenuItem
            // 
            this.renameTextureToolStripMenuItem.Name = "renameTextureToolStripMenuItem";
            this.renameTextureToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.renameTextureToolStripMenuItem.Text = "Rename";
            this.renameTextureToolStripMenuItem.Click += new System.EventHandler(this.renameTextureToolStripMenuItem_Click);
            // 
            // resetToolStripMenuItem
            // 
            this.resetToolStripMenuItem.Name = "resetToolStripMenuItem";
            this.resetToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.resetToolStripMenuItem.Text = "Reload";
            this.resetToolStripMenuItem.Click += new System.EventHandler(this.resetToolStripMenuItem_Click);
            // 
            // chkAllTextures
            // 
            this.chkAllTextures.Checked = true;
            this.chkAllTextures.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAllTextures.Dock = System.Windows.Forms.DockStyle.Top;
            this.chkAllTextures.Location = new System.Drawing.Point(0, 21);
            this.chkAllTextures.Margin = new System.Windows.Forms.Padding(0);
            this.chkAllTextures.Name = "chkAllTextures";
            this.chkAllTextures.Padding = new System.Windows.Forms.Padding(1, 0, 0, 0);
            this.chkAllTextures.Size = new System.Drawing.Size(96, 20);
            this.chkAllTextures.TabIndex = 9;
            this.chkAllTextures.Text = "All";
            this.chkAllTextures.UseVisualStyleBackColor = false;
            this.chkAllTextures.CheckStateChanged += new System.EventHandler(this.chkAllTextures_CheckStateChanged);
            // 
            // btnTextures
            // 
            this.btnTextures.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnTextures.Location = new System.Drawing.Point(0, 0);
            this.btnTextures.Name = "btnTextures";
            this.btnTextures.Size = new System.Drawing.Size(96, 21);
            this.btnTextures.TabIndex = 7;
            this.btnTextures.Text = "Textures";
            this.btnTextures.UseVisualStyleBackColor = true;
            this.btnTextures.Click += new System.EventHandler(this.btnTextures_Click);
            // 
            // ctxBones
            // 
            this.ctxBones.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.boneIndex,
            this.renameBoneToolStripMenuItem});
            this.ctxBones.Name = "ctxBones";
            this.ctxBones.Size = new System.Drawing.Size(133, 48);
            this.ctxBones.Opening += new System.ComponentModel.CancelEventHandler(this.ctxBones_Opening);
            // 
            // boneIndex
            // 
            this.boneIndex.Enabled = false;
            this.boneIndex.Name = "boneIndex";
            this.boneIndex.Size = new System.Drawing.Size(132, 22);
            this.boneIndex.Text = "Bone Index";
            // 
            // renameBoneToolStripMenuItem
            // 
            this.renameBoneToolStripMenuItem.Name = "renameBoneToolStripMenuItem";
            this.renameBoneToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
            this.renameBoneToolStripMenuItem.Text = "Rename";
            this.renameBoneToolStripMenuItem.Click += new System.EventHandler(this.renameBoneToolStripMenuItem_Click);
            // 
            // pnlBones
            // 
            this.pnlBones.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlBones.Controls.Add(this.lstBones);
            this.pnlBones.Controls.Add(this.chkAllBones);
            this.pnlBones.Controls.Add(this.btnBones);
            this.pnlBones.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBones.Location = new System.Drawing.Point(0, 147);
            this.pnlBones.MinimumSize = new System.Drawing.Size(0, 21);
            this.pnlBones.Name = "pnlBones";
            this.pnlBones.Size = new System.Drawing.Size(98, 171);
            this.pnlBones.TabIndex = 3;
            // 
            // lstBones
            // 
            this.lstBones.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lstBones.CausesValidation = false;
            this.lstBones.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstBones.IntegralHeight = false;
            this.lstBones.Location = new System.Drawing.Point(0, 41);
            this.lstBones.Margin = new System.Windows.Forms.Padding(0);
            this.lstBones.Name = "lstBones";
            this.lstBones.Size = new System.Drawing.Size(96, 128);
            this.lstBones.TabIndex = 7;
            this.lstBones.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.lstBones_ItemCheck);
            this.lstBones.SelectedValueChanged += new System.EventHandler(this.lstBones_SelectedValueChanged);
            this.lstBones.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lstBones_KeyDown);
            this.lstBones.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lstBones_MouseDown);
            // 
            // chkAllBones
            // 
            this.chkAllBones.Checked = true;
            this.chkAllBones.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAllBones.Dock = System.Windows.Forms.DockStyle.Top;
            this.chkAllBones.Location = new System.Drawing.Point(0, 21);
            this.chkAllBones.Margin = new System.Windows.Forms.Padding(0);
            this.chkAllBones.Name = "chkAllBones";
            this.chkAllBones.Padding = new System.Windows.Forms.Padding(1, 0, 0, 0);
            this.chkAllBones.Size = new System.Drawing.Size(96, 20);
            this.chkAllBones.TabIndex = 8;
            this.chkAllBones.Text = "All";
            this.chkAllBones.UseVisualStyleBackColor = false;
            this.chkAllBones.CheckStateChanged += new System.EventHandler(this.chkAllBones_CheckStateChanged);
            // 
            // btnBones
            // 
            this.btnBones.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnBones.Location = new System.Drawing.Point(0, 0);
            this.btnBones.Name = "btnBones";
            this.btnBones.Size = new System.Drawing.Size(96, 21);
            this.btnBones.TabIndex = 9;
            this.btnBones.Text = "Bones";
            this.btnBones.UseVisualStyleBackColor = true;
            this.btnBones.Click += new System.EventHandler(this.btnBones_Click);
            // 
            // spltBones
            // 
            this.spltBones.Cursor = System.Windows.Forms.Cursors.HSplit;
            this.spltBones.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.spltBones.Location = new System.Drawing.Point(0, 318);
            this.spltBones.Name = "spltBones";
            this.spltBones.Size = new System.Drawing.Size(98, 4);
            this.spltBones.TabIndex = 4;
            this.spltBones.Text = "proxySplitter1";
            this.spltBones.Dragged += new System.Windows.Forms.SplitterEventHandler(this.spltBones_Dragged);
            // 
            // spltPolygons
            // 
            this.spltPolygons.Cursor = System.Windows.Forms.Cursors.HSplit;
            this.spltPolygons.Dock = System.Windows.Forms.DockStyle.Top;
            this.spltPolygons.Location = new System.Drawing.Point(0, 143);
            this.spltPolygons.Name = "spltPolygons";
            this.spltPolygons.Size = new System.Drawing.Size(98, 4);
            this.spltPolygons.TabIndex = 1;
            this.spltPolygons.Text = "proxySplitter1";
            this.spltPolygons.Dragged += new System.Windows.Forms.SplitterEventHandler(this.spltPolygons_Dragged);
            // 
            // ModelAssetPanel
            // 
            this.Controls.Add(this.pnlBones);
            this.Controls.Add(this.spltBones);
            this.Controls.Add(this.pnlTextures);
            this.Controls.Add(this.spltPolygons);
            this.Controls.Add(this.pnlPolygons);
            this.Name = "ModelAssetPanel";
            this.Size = new System.Drawing.Size(98, 472);
            this.pnlPolygons.ResumeLayout(false);
            this.pnlTextures.ResumeLayout(false);
            this.ctxTextures.ResumeLayout(false);
            this.ctxBones.ResumeLayout(false);
            this.pnlBones.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        public bool _syncVis0 = false;
        public bool _syncPat0 = false;
        public bool _syncSrt0 = false;
        public bool _syncShp0 = false;
        
        private bool _updating = false;
        private object _targetObject;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object TargetObject
        {
            get { return _targetObject; }
            set { _targetObject = value; }
        }
        
        private int _selectedVertex = -1; //Vertex index in the selected polygon's vertices
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int SelectedVertex { get { return _selectedVertex; } set { _selectedVertex = value; } }

        private MDL0BoneNode _selectedBone;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MDL0BoneNode SelectedBone { get { return _selectedBone; } set { lstBones.SelectedItem = value; } }

        private MDL0PolygonNode _selectedPolygon;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MDL0PolygonNode SelectedPolygon { get { return _selectedPolygon; } set { lstPolygons.SelectedItem = value; } }

        private MDL0TextureNode _selectedTexture;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MDL0TextureNode SelectedTexture { get { return _selectedTexture; } }
        
        public event EventHandler Vis0Changed;
        public event EventHandler SelectedPolygonChanged;
        public event EventHandler SelectedBoneChanged;
        public event EventHandler SelectedTextureChanged;
        public event EventHandler RenderStateChanged;
        public event EventHandler Vis0Updated;

        //Bone Name - Attached Polygon Indices
        public Dictionary<string, List<int>> VIS0Indices = new Dictionary<string, List<int>>();

        public ModelAssetPanel()
        {
            InitializeComponent();
        }

        public void Attach(MDL0Node model)
        {
            lstPolygons.BeginUpdate();
            lstPolygons.Items.Clear();
            lstBones.BeginUpdate();
            lstBones.Items.Clear();
            lstTextures.BeginUpdate();
            lstTextures.Items.Clear();

            _selectedBone = null;
            _selectedPolygon = null;
            _targetObject = null;

            chkAllPoly.CheckState = CheckState.Checked;
            chkAllBones.CheckState = CheckState.Checked;
            chkAllTextures.CheckState = CheckState.Checked;

            if (model != null)
            {
                ResourceNode n;

                if ((n = model.FindChild("Objects", false)) != null)
                    foreach (MDL0PolygonNode poly in n.Children)
                        lstPolygons.Items.Add(poly, poly._render);

                if ((n = model.FindChild("Bones", false)) != null)
                    foreach (MDL0BoneNode bone in n.Children)
                        WrapBone(bone);

                if ((n = model.FindChild("Textures", false)) != null)
                    foreach (MDL0TextureNode tref in n.Children)
                        lstTextures.Items.Add(tref, tref.Enabled);
            }

            lstTextures.EndUpdate();
            lstPolygons.EndUpdate();
            lstBones.EndUpdate();

            VIS0Indices.Clear(); int i = 0;
            foreach (MDL0PolygonNode p in lstPolygons.Items)
            {
                if (p._bone != null && p._bone.BoneIndex != 0)
                {
                    if (VIS0Indices.ContainsKey(p._bone.Name))
                    {
                        if (!VIS0Indices[p._bone.Name].Contains(i))
                            VIS0Indices[p._bone.Name].Add(i);
                    }
                    else
                        VIS0Indices.Add(p._bone.Name, new List<int> { i });
                }
                i++;
            }
        }

        private void WrapBone(MDL0BoneNode bone)
        {
            lstBones.Items.Add(bone, bone._render);
            foreach (MDL0BoneNode b in bone.Children)
                WrapBone(b);
        }

        private void spltPolygons_Dragged(object sender, SplitterEventArgs e)
        {
            if (e.Y == 0)
                return;

            pnlPolygons.Height += e.Y;
        }

        private void spltBones_Dragged(object sender, SplitterEventArgs e)
        {
            if (e.Y == 0)
                return;

            pnlTextures.Height -= e.Y;
        }

        private void lstPolygons_SelectedValueChanged(object sender, EventArgs e)
        {
            _targetObject = _selectedPolygon = lstPolygons.SelectedItem as MDL0PolygonNode;
            if (SelectedPolygonChanged != null)
                SelectedPolygonChanged(this, null);
        }

        public bool _vis0Updating = false; //Done
        public bool _pat0Updating = false; //In Progress
        public bool _srt0Updating = false; //In Progress
        public bool _shp0Updating = false; //Not Started
        
        public int _polyIndex = -1;
        public int _boneIndex = -1;
        public int _texIndex = -1;

        private void lstPolygons_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            MDL0PolygonNode poly = lstPolygons.Items[e.Index] as MDL0PolygonNode;

            poly._render = e.NewValue == CheckState.Checked;

            if (_syncVis0 && poly._bone != null)
            {
                bool temp = false;
                if (!_vis0Updating)
                {
                    _vis0Updating = true;
                    temp = true;
                }

                if (VIS0Indices.ContainsKey(poly._bone.Name))
                    foreach (int i in VIS0Indices[poly._bone.Name])
                        if (((MDL0PolygonNode)lstPolygons.Items[i])._render != poly._render)
                            lstPolygons.SetItemChecked(i, poly._render);

                if (temp)
                {
                    _vis0Updating = false;
                    temp = false;
                }

                if (!_vis0Updating)
                {
                    _polyIndex = e.Index;
                    if (Vis0Updated != null)
                        Vis0Updated(this, null);
                }
            }

            if (!_updating)
                if (RenderStateChanged != null)
                    RenderStateChanged(this, null);
        }

        private void lstBones_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            MDL0BoneNode bone = lstBones.Items[e.Index] as MDL0BoneNode;

            bone._render = e.NewValue == CheckState.Checked;

            if (!_updating)
                if (RenderStateChanged != null)
                    RenderStateChanged(this, null);
        }

        public event EventHandler Key;
        public event EventHandler Unkey;
        protected override bool ProcessKeyPreview(ref Message m)
        {
            if (m.Msg == 0x100)
            {
                Keys key = (Keys)m.WParam;
                if (Control.ModifierKeys == Keys.Control)
                {
                    if (key == Keys.K)
                    {
                        if (Key != null)
                            Key(this, null);
                        return true;
                    }
                    else if (key == Keys.L)
                    {
                        if (Unkey != null)
                            Unkey(this, null);
                        return true;
                    }
                    return false;
                }
            }
            return base.ProcessKeyPreview(ref m);
        }

        private void lstBones_SelectedValueChanged(object sender, EventArgs e)
        {
            if (_selectedBone != null)
                _selectedBone._boneColor = _selectedBone._nodeColor = Color.Transparent;

            if ((_targetObject = _selectedBone = lstBones.SelectedItem as MDL0BoneNode) != null)
            {
                _selectedBone._boneColor = Color.FromArgb(0, 128, 255);
                _selectedBone._nodeColor = Color.FromArgb(255, 128, 0);
            }

            if (SelectedBoneChanged != null)
                SelectedBoneChanged(this, null);

            if (RenderStateChanged != null)
                RenderStateChanged(this, null);
        }

        private void lstTextures_SelectedValueChanged(object sender, EventArgs e)
        {
            if (_selectedTexture != null)
                _selectedTexture.Selected = false;

            if ((_targetObject = _selectedTexture = lstTextures.SelectedItem as MDL0TextureNode) != null)
                _selectedTexture.Selected = true;

            if (SelectedTextureChanged != null)
                SelectedTextureChanged(this, null);

            if (!_updating)
                if (RenderStateChanged != null)
                    RenderStateChanged(this, null);
        }

        private void chkAllPoly_CheckStateChanged(object sender, EventArgs e)
        {
            if (lstPolygons.Items.Count == 0)
                return;

            _updating = true;

            lstPolygons.BeginUpdate();
            for (int i = 0; i < lstPolygons.Items.Count; i++)
                lstPolygons.SetItemCheckState(i, chkAllPoly.CheckState);
            lstPolygons.EndUpdate();

            _updating = false;

            if (RenderStateChanged != null)
                RenderStateChanged(this, null);
        }

        private void chkAllBones_CheckStateChanged(object sender, EventArgs e)
        {
            if (lstBones.Items.Count == 0)
                return;

            _updating = true;

            lstBones.BeginUpdate();
            for (int i = 0; i < lstBones.Items.Count; i++)
                lstBones.SetItemCheckState(i, chkAllBones.CheckState);
            lstBones.EndUpdate();

            _updating = false;

            if (RenderStateChanged != null)
                RenderStateChanged(this, null);
        }

        private void btnPolygons_Click(object sender, EventArgs e)
        {
            if (lstPolygons.Visible)
            {
                pnlPolygons.Tag = pnlPolygons.Height;
                pnlPolygons.Height = btnPolygons.Height;
                lstPolygons.Visible = chkAllPoly.Visible = spltPolygons.Visible = false;
            }
            else
            {
                pnlPolygons.Height = (int)pnlPolygons.Tag;
                lstPolygons.Visible = chkAllPoly.Visible = spltPolygons.Visible = true;
            }
        }

        private void btnBones_Click(object sender, EventArgs e)
        {
            if (lstBones.Visible)
            {
                pnlBones.Tag = pnlBones.Height;
                if (lstPolygons.Visible)
                {
                    pnlBones.Dock = DockStyle.Bottom;
                    pnlBones.Height = btnBones.Height;
                    pnlPolygons.Dock = DockStyle.Fill;
                }
                else
                {
                    spltPolygons.Visible = false;
                    pnlBones.Dock = DockStyle.Top;
                    pnlBones.Height = btnBones.Height;
                    if (lstTextures.Visible)
                    {
                        spltBones.Visible = false;
                        pnlTextures.Dock = DockStyle.Fill;
                    }
                    else
                        pnlTextures.Dock = DockStyle.Top;
                }
                chkAllBones.Visible = lstBones.Visible = false;
            }
            else
            {
                pnlBones.Height = (int)pnlBones.Tag;
                if (lstPolygons.Visible)
                    pnlPolygons.Dock = DockStyle.Top;

                pnlTextures.Dock = DockStyle.Bottom;
                pnlBones.Dock = DockStyle.Fill;
                chkAllBones.Visible = lstBones.Visible = true;
            }
        }

        private void btnTextures_Click(object sender, EventArgs e)
        {
            if (lstTextures.Visible)
            {
                pnlTextures.Tag = pnlTextures.Height;
                pnlTextures.Height = btnTextures.Height;
                lstTextures.Visible = chkAllTextures.Visible = spltBones.Visible = false;
            }
            else
            {
                pnlTextures.Height = (int)pnlTextures.Tag;
                lstTextures.Visible = chkAllTextures.Visible = spltBones.Visible = true;
            }
        }

        private void lstTextures_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            MDL0TextureNode tref = lstTextures.Items[e.Index] as MDL0TextureNode;

            tref.Enabled = e.NewValue == CheckState.Checked;

            if (!_updating)
                if (RenderStateChanged != null)
                    RenderStateChanged(this, null);
        }

        private void chkAllTextures_CheckStateChanged(object sender, EventArgs e)
        {
            _updating = true;

            lstTextures.BeginUpdate();
            for (int i = 0; i < lstTextures.Items.Count; i++)
                lstTextures.SetItemCheckState(i, chkAllTextures.CheckState);
            lstTextures.EndUpdate();

            _updating = false;

            if (RenderStateChanged != null)
                RenderStateChanged(this, null);
        }

        private void viewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_selectedTexture != null)
                using (GLTextureWindow w = new GLTextureWindow())
                    w.ShowDialog(this, _selectedTexture.Texture);
        }

        private void replaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int index = lstTextures.SelectedIndex;
            if ((_selectedTexture != null) && (_selectedTexture.Source is TEX0Node))
            {
                TEX0Node node = _selectedTexture.Source as TEX0Node;
                using (TextureConverterDialog dlg = new TextureConverterDialog())
                    if (dlg.ShowDialog(this, node) == DialogResult.OK)
                    {
                        _updating = true;
                        _selectedTexture.Reload();
                        lstTextures.SetItemCheckState(index, CheckState.Checked);
                        lstTextures.SetSelected(index, false);
                        _updating = false;

                        if (RenderStateChanged != null)
                            RenderStateChanged(this, null);
                    }
            }
        }

        private void ctxTextures_Opening(object sender, CancelEventArgs e)
        {
            if (_selectedTexture == null)
                e.Cancel = true;
            else
            {
                if (_selectedTexture.Source is TEX0Node)
                {
                    viewToolStripMenuItem.Enabled = true;
                    replaceToolStripMenuItem.Enabled = true;
                    exportToolStripMenuItem.Enabled = true;
                    sourceToolStripMenuItem.Text = String.Format("Source: {0}", Path.GetFileName(((ResourceNode)_selectedTexture.Source).RootNode._origPath));
                }
                else if (_selectedTexture.Source is string)
                {
                    viewToolStripMenuItem.Enabled = true;
                    replaceToolStripMenuItem.Enabled = false;
                    exportToolStripMenuItem.Enabled = false;
                    sourceToolStripMenuItem.Text = String.Format("Source: {0}", (string)_selectedTexture.Source);
                }
                else
                {
                    viewToolStripMenuItem.Enabled = false;
                    replaceToolStripMenuItem.Enabled = false;
                    exportToolStripMenuItem.Enabled = false;
                    sourceToolStripMenuItem.Text = "Source: <Not Found>";
                }

                if (_selectedTexture.Texture != null)
                    sizeToolStripMenuItem.Text = String.Format("Size: {0} x {1}", _selectedTexture.Texture.Width, _selectedTexture.Texture.Height);
                else
                    sizeToolStripMenuItem.Text = "Size: <Not Found>";
            }
        }

        private void ctxBones_Opening(object sender, CancelEventArgs e)
        {

        }

        private void lstTextures_MouseDown(object sender, MouseEventArgs e)
        {
            int index = lstTextures.IndexFromPoint(e.X, e.Y);
            if (lstTextures.SelectedIndex != index)
                lstTextures.SelectedIndex = index;

            if (e.Button == MouseButtons.Right)
            {
                if (_selectedTexture != null)
                    lstTextures.ContextMenuStrip = ctxTextures;
                else
                    lstTextures.ContextMenuStrip = null;
            }
        }

        private void lstBones_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (_selectedBone != null)
                {
                    lstBones.ContextMenuStrip = ctxBones;
                    boneIndex.Text = "Bone Index: " + _selectedBone.BoneIndex.ToString();
                }
                else
                    lstBones.ContextMenuStrip = null;
            }
        }

        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_selectedTexture != null)
            {
                _selectedTexture.Reload();
                if (RenderStateChanged != null)
                    RenderStateChanged(this, null);
            }
        }

        private void lstTextures_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                lstTextures.SelectedItem = null;
        }
        private void lstBones_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                lstBones.SelectedItem = null;
        }
        private void lstPolygons_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                lstPolygons.SelectedItem = null;
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ((_selectedTexture != null) && (_selectedTexture.Source is TEX0Node))
            {
                TEX0Node node = _selectedTexture.Source as TEX0Node;
                using (SaveFileDialog dlgSave = new SaveFileDialog())
                {
                    dlgSave.FileName = node.Name;
                    dlgSave.Filter = ExportFilters.TEX0;
                    if (dlgSave.ShowDialog(this) == DialogResult.OK)
                        node.Export(dlgSave.FileName);
                }
            }
        }

        private void renameBoneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (RenameDialog dlg = new RenameDialog())
                dlg.ShowDialog(this.ParentForm, _selectedBone);
        }

        private void renameTextureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (RenameDialog dlg = new RenameDialog())
                dlg.ShowDialog(this.ParentForm, (_selectedTexture.Source as TEX0Node));
            
            _selectedTexture.Name = (_selectedTexture.Source as TEX0Node).Name;
        }

        private void chkSyncVis_CheckedChanged(object sender, EventArgs e)
        {
            _syncVis0 = chkSyncVis.Checked;

            if (Vis0Changed != null)
                Vis0Changed(this, null);
        }
    }
}
