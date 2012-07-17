using System;
using BrawlLib.SSBB.ResourceNodes;
using System.Drawing;
using System.ComponentModel;
using System.Collections.Generic;
using BrawlLib.Wii.Animations;
using BrawlLib.Modeling;

namespace System.Windows.Forms
{
    public class ModelOptionPanel : UserControl
    {
        #region Designer

        public CheckBox chkPolygons;
        private CheckBox chkBones;
        private Button btnCamReset;
        private CheckBox chkFloor;
        public Button Undo;
        public Button Redo;
        private GroupBox groupBox1;
        public ComboBox models;
        public CheckBox chkHurtboxes;
        public CheckBox chkHitboxes;
        private CheckBox chkVertices;
    
        private void InitializeComponent()
        {
            this.chkPolygons = new System.Windows.Forms.CheckBox();
            this.chkBones = new System.Windows.Forms.CheckBox();
            this.btnCamReset = new System.Windows.Forms.Button();
            this.chkFloor = new System.Windows.Forms.CheckBox();
            this.Undo = new System.Windows.Forms.Button();
            this.Redo = new System.Windows.Forms.Button();
            this.chkVertices = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.models = new System.Windows.Forms.ComboBox();
            this.chkHurtboxes = new System.Windows.Forms.CheckBox();
            this.chkHitboxes = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkPolygons
            // 
            this.chkPolygons.Location = new System.Drawing.Point(121, 2);
            this.chkPolygons.Name = "chkPolygons";
            this.chkPolygons.Size = new System.Drawing.Size(74, 20);
            this.chkPolygons.TabIndex = 5;
            this.chkPolygons.Text = "Polygons";
            this.chkPolygons.ThreeState = true;
            this.chkPolygons.UseVisualStyleBackColor = true;
            this.chkPolygons.CheckStateChanged += new System.EventHandler(this.chkPolygons_CheckStateChanged);
            // 
            // chkBones
            // 
            this.chkBones.Location = new System.Drawing.Point(193, 2);
            this.chkBones.Name = "chkBones";
            this.chkBones.Size = new System.Drawing.Size(61, 20);
            this.chkBones.TabIndex = 6;
            this.chkBones.Text = "Bones";
            this.chkBones.UseVisualStyleBackColor = true;
            this.chkBones.CheckedChanged += new System.EventHandler(this.chkBones_CheckedChanged);
            // 
            // btnCamReset
            // 
            this.btnCamReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCamReset.Location = new System.Drawing.Point(517, 1);
            this.btnCamReset.Name = "btnCamReset";
            this.btnCamReset.Size = new System.Drawing.Size(85, 20);
            this.btnCamReset.TabIndex = 7;
            this.btnCamReset.Text = "Reset Camera";
            this.btnCamReset.UseVisualStyleBackColor = true;
            this.btnCamReset.Click += new System.EventHandler(this.btnCamReset_Click);
            // 
            // chkFloor
            // 
            this.chkFloor.Location = new System.Drawing.Point(193, 21);
            this.chkFloor.Name = "chkFloor";
            this.chkFloor.Size = new System.Drawing.Size(61, 20);
            this.chkFloor.TabIndex = 8;
            this.chkFloor.Text = "Floor";
            this.chkFloor.UseVisualStyleBackColor = true;
            this.chkFloor.CheckedChanged += new System.EventHandler(this.chkFloor_CheckedChanged);
            // 
            // Undo
            // 
            this.Undo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Undo.Enabled = false;
            this.Undo.Location = new System.Drawing.Point(517, 20);
            this.Undo.Name = "Undo";
            this.Undo.Size = new System.Drawing.Size(43, 20);
            this.Undo.TabIndex = 9;
            this.Undo.Text = "Undo";
            this.Undo.UseVisualStyleBackColor = true;
            this.Undo.Click += new System.EventHandler(this.Undo_Click);
            // 
            // Redo
            // 
            this.Redo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Redo.Enabled = false;
            this.Redo.Location = new System.Drawing.Point(559, 20);
            this.Redo.Name = "Redo";
            this.Redo.Size = new System.Drawing.Size(43, 20);
            this.Redo.TabIndex = 10;
            this.Redo.Text = "Redo";
            this.Redo.UseVisualStyleBackColor = true;
            this.Redo.Click += new System.EventHandler(this.Redo_Click);
            // 
            // chkVertices
            // 
            this.chkVertices.Location = new System.Drawing.Point(121, 21);
            this.chkVertices.Name = "chkVertices";
            this.chkVertices.Size = new System.Drawing.Size(68, 20);
            this.chkVertices.TabIndex = 11;
            this.chkVertices.Text = "Vertices";
            this.chkVertices.UseVisualStyleBackColor = true;
            this.chkVertices.CheckedChanged += new System.EventHandler(this.chkVertices_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.models);
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(115, 41);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Target Model";
            // 
            // models
            // 
            this.models.FormattingEnabled = true;
            this.models.Location = new System.Drawing.Point(7, 14);
            this.models.Name = "models";
            this.models.Size = new System.Drawing.Size(102, 21);
            this.models.TabIndex = 0;
            this.models.SelectedValueChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // chkHurtboxes
            // 
            this.chkHurtboxes.Location = new System.Drawing.Point(248, 20);
            this.chkHurtboxes.Name = "chkHurtboxes";
            this.chkHurtboxes.Size = new System.Drawing.Size(78, 20);
            this.chkHurtboxes.TabIndex = 14;
            this.chkHurtboxes.Text = "Hurtboxes";
            this.chkHurtboxes.UseVisualStyleBackColor = true;
            this.chkHurtboxes.Visible = false;
            this.chkHurtboxes.CheckedChanged += new System.EventHandler(this.chkHurtboxes_CheckedChanged);
            // 
            // chkHitboxes
            // 
            this.chkHitboxes.Location = new System.Drawing.Point(248, 1);
            this.chkHitboxes.Name = "chkHitboxes";
            this.chkHitboxes.Size = new System.Drawing.Size(70, 20);
            this.chkHitboxes.TabIndex = 13;
            this.chkHitboxes.Text = "Hitboxes";
            this.chkHitboxes.UseVisualStyleBackColor = true;
            this.chkHitboxes.Visible = false;
            this.chkHitboxes.CheckedChanged += new System.EventHandler(this.chkHitboxes_CheckedChanged);
            // 
            // ModelOptionPanel
            // 
            this.Controls.Add(this.chkHurtboxes);
            this.Controls.Add(this.chkHitboxes);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.chkVertices);
            this.Controls.Add(this.Redo);
            this.Controls.Add(this.Undo);
            this.Controls.Add(this.chkFloor);
            this.Controls.Add(this.btnCamReset);
            this.Controls.Add(this.chkBones);
            this.Controls.Add(this.chkPolygons);
            this.Name = "ModelOptionPanel";
            this.Size = new System.Drawing.Size(602, 41);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        public ModelEditControl _mainWindow;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ModelEditControl MainWindow
        {
            get { return _mainWindow; }
            set { _mainWindow = value; }
        }

        private bool _updating = false;

        public SaveState _save;

        public event EventHandler RenderStateChanged;
        public event EventHandler HtBoxesChanged;
        public event EventHandler TargetModelChanged;
        public event EventHandler VertexChanged;
        public event EventHandler CamResetClicked;
        public event EventHandler FloorRenderChanged;
        public event EventHandler UndoClicked;
        public event EventHandler RedoClicked;
        
        private MDL0Node _targetModel;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MDL0Node TargetModel
        {
            get { return _targetModel; }
            set
            {
                if ((_targetModel = value) != null)
                {
                    Enabled = true;

                    _updating = true;
                    models.SelectedIndex = models.Items.IndexOf(_targetModel);
                    chkPolygons.CheckState = _targetModel._renderPolygons ? (_targetModel._renderPolygonsWireframe ? CheckState.Indeterminate : CheckState.Checked) : CheckState.Unchecked;
                    chkBones.Checked = _targetModel._renderBones;
                    chkVertices.Checked = _targetModel._renderVertices;
                    //Redo.Enabled = _targetModel._canRedo;
                    //Undo.Enabled = _targetModel._canUndo;
                    _updating = false;
                }
                else
                    Enabled = false;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool RenderFloor
        {
            get { return chkFloor.Checked; }
            set { chkFloor.Checked = value; }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool RenderBones
        {
            get { return chkBones.Checked; }
            set { chkBones.Checked = value; }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool RenderVertices
        {
            get { return chkVertices.Checked; }
            set { chkVertices.Checked = value; }
        }

        public ModelOptionPanel() { InitializeComponent(); }

        public void chkPolygons_CheckStateChanged(object sender, EventArgs e)
        {
            if ((_updating) || (_targetModel == null))
                return;

            _targetModel._renderPolygonsWireframe = (chkPolygons.CheckState == CheckState.Indeterminate);
            _targetModel._renderPolygons = (_targetModel._renderPolygonsWireframe) || (chkPolygons.CheckState == CheckState.Checked);

            if (RenderStateChanged != null)
                RenderStateChanged(this, null);
        }

        public void chkBones_CheckedChanged(object sender, EventArgs e)
        {
            if ((_updating) || (_targetModel == null))
                return;

            _targetModel._renderBones = chkBones.Checked;

            if (RenderStateChanged != null)
                RenderStateChanged(this, null);
        }

        public void chkVertices_CheckedChanged(object sender, EventArgs e)
        {
            if ((_updating) || (_targetModel == null))
                return;

            _targetModel._renderVertices = chkVertices.Checked;

            if (VertexChanged != null)
                VertexChanged(this, null);
        }

        public void btnCamReset_Click(object sender, EventArgs e)
        {
            if (CamResetClicked != null)
                CamResetClicked(this, null);
        }

        public void chkFloor_CheckedChanged(object sender, EventArgs e)
        {
            if (FloorRenderChanged != null)
                FloorRenderChanged(this, null);
        }

        public void Redo_Click(object sender, EventArgs e)
        {
            //_targetModel.Redo();
            //_save = _targetModel.Saves[_targetModel._saveIndex];
            //_targetModel._currentSave = _save.id;

            //if (_targetModel._saveIndex >= _targetModel.Saves.Count - 1)
            //{ _targetModel._canRedo = false; Redo.Enabled = false; }

            if (RedoClicked != null)
                RedoClicked(this, null);
        }

        public void Undo_Click(object sender, EventArgs e)
        {
            //_targetModel.Undo();
            //_save = _targetModel.Saves[_targetModel._saveIndex];
            //_targetModel._currentSave = _save.id;

            //if (_targetModel._saveIndex == 0)
            //{ _targetModel._canUndo = false; Undo.Enabled = false; }

            if (UndoClicked != null)
                UndoClicked(this, null);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((_updating) || (models.SelectedItem == null))
                return;

            if (TargetModelChanged != null)
                TargetModelChanged(this, null);
            
        }

        private void chkHitboxes_CheckedChanged(object sender, EventArgs e)
        {
            if ((_updating) || (_targetModel == null))
                return;

            if (HtBoxesChanged != null)
                HtBoxesChanged(this, null);
        }

        private void chkHurtboxes_CheckedChanged(object sender, EventArgs e)
        {
            if ((_updating) || (_targetModel == null))
                return;

            if (HtBoxesChanged != null)
                HtBoxesChanged(this, null);
        }
    }
}
