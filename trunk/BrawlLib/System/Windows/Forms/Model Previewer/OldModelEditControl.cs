using System;
using BrawlLib.OpenGL;
using System.ComponentModel;
using BrawlLib.SSBB.ResourceNodes;
using System.IO;
using BrawlLib.Modeling;
using System.Drawing;
using BrawlLib.Wii.Animations;
using System.Collections.Generic;
using BrawlLib.SSBBTypes;

namespace System.Windows.Forms
{
    public class ModelEditControl : UserControl
    {
        #region Designer

        private ColorDialog dlgColor;
        public ModelPanel modelPanel1;
        private Button btnAssetToggle;
        private Button btnAnimToggle;
        private System.ComponentModel.IContainer components;
        private Button btnPlaybackToggle;
        private Timer animTimer;
        private ModelAssetPanel pnlAssets;
        private Splitter spltAssets;
        public ModelOptionPanel pnlOptions;
        private Button btnOptionToggle;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem editToolStripMenuItem;
        private ToolStripMenuItem openFileToolStripMenuItem;
        private ToolStripMenuItem kinectToolStripMenuItem;
        private ToolStripMenuItem notYetImplementedToolStripMenuItem;
        private ToolStripMenuItem newSceneToolStripMenuItem;
        private ToolStripMenuItem kinectToolStripMenuItem1;
        private ToolStripMenuItem undoToolStripMenuItem;
        private ToolStripMenuItem redoToolStripMenuItem;
        private ToolStripMenuItem backColorToolStripMenuItem;
        private ToolStripMenuItem setColorToolStripMenuItem;
        private ToolStripMenuItem loadImageToolStripMenuItem;
        private TransparentPanel KinectPanel;
        private ToolStripMenuItem startTrackingToolStripMenuItem;
        private Label label1;
        private ToolStripMenuItem syncKinectToolStripMenuItem;
        private ToolStripMenuItem targetModelToolStripMenuItem;
        private ToolStripMenuItem hideFromSceneToolStripMenuItem;
        private ToolStripMenuItem deleteToolStripMenuItem;
        private ToolStripMenuItem hideAllOtherModelsToolStripMenuItem;
        private ToolStripMenuItem deleteAllOtherModelsToolStripMenuItem;
        private ToolStripMenuItem openModelSwitherToolStripMenuItem;
        private ToolStripMenuItem modelToolStripMenuItem;
        private ToolStripMenuItem toggleBones;
        private ToolStripMenuItem togglePolygons;
        private ToolStripMenuItem toggleVertices;
        private ToolStripMenuItem movesetToolStripMenuItem1;
        private ToolStripMenuItem hitboxesOffToolStripMenuItem;
        private ToolStripMenuItem hurtboxesOffToolStripMenuItem;
        private ToolStripMenuItem modifyLightingToolStripMenuItem;
        private ModelAnimPanel pnlAnim;
        private ToolStripMenuItem toggleFloor;
        private ToolStripMenuItem resetCameraToolStripMenuItem;
        private ToolStripMenuItem editorsToolStripMenuItem;
        private ToolStripMenuItem showAssets;
        private ToolStripMenuItem showAnim;
        private ToolStripMenuItem showPlay;
        private ToolStripMenuItem showOptions;
        private ToolStripMenuItem showMoveset;
        private ModelMovesetPanel pnlMoveset;
        private Splitter spltMoveset;
        private ModelPlaybackPanel pnlPlayback;

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.dlgColor = new System.Windows.Forms.ColorDialog();
            this.btnAssetToggle = new System.Windows.Forms.Button();
            this.btnAnimToggle = new System.Windows.Forms.Button();
            this.btnPlaybackToggle = new System.Windows.Forms.Button();
            this.animTimer = new System.Windows.Forms.Timer(this.components);
            this.spltAssets = new System.Windows.Forms.Splitter();
            this.btnOptionToggle = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newSceneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.redoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.modifyLightingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.kinectToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.editorsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showAssets = new System.Windows.Forms.ToolStripMenuItem();
            this.showAnim = new System.Windows.Forms.ToolStripMenuItem();
            this.showPlay = new System.Windows.Forms.ToolStripMenuItem();
            this.showOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.showMoveset = new System.Windows.Forms.ToolStripMenuItem();
            this.backColorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setColorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toggleFloor = new System.Windows.Forms.ToolStripMenuItem();
            this.resetCameraToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.modelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toggleBones = new System.Windows.Forms.ToolStripMenuItem();
            this.togglePolygons = new System.Windows.Forms.ToolStripMenuItem();
            this.toggleVertices = new System.Windows.Forms.ToolStripMenuItem();
            this.movesetToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.hitboxesOffToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hurtboxesOffToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.targetModelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hideFromSceneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hideAllOtherModelsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteAllOtherModelsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openModelSwitherToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.kinectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.syncKinectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.notYetImplementedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startTrackingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.KinectPanel = new System.Windows.Forms.TransparentPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.modelPanel1 = new System.Windows.Forms.ModelPanel();
            this.pnlOptions = new System.Windows.Forms.ModelOptionPanel();
            this.pnlAssets = new System.Windows.Forms.ModelAssetPanel();
            this.pnlAnim = new System.Windows.Forms.ModelAnimPanel();
            this.pnlPlayback = new System.Windows.Forms.ModelPlaybackPanel();
            this.pnlMoveset = new System.Windows.Forms.ModelMovesetPanel();
            this.spltMoveset = new System.Windows.Forms.Splitter();
            this.menuStrip1.SuspendLayout();
            this.KinectPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // dlgColor
            // 
            this.dlgColor.AnyColor = true;
            this.dlgColor.FullOpen = true;
            // 
            // btnAssetToggle
            // 
            this.btnAssetToggle.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnAssetToggle.Location = new System.Drawing.Point(310, 0);
            this.btnAssetToggle.Name = "btnAssetToggle";
            this.btnAssetToggle.Size = new System.Drawing.Size(15, 546);
            this.btnAssetToggle.TabIndex = 5;
            this.btnAssetToggle.TabStop = false;
            this.btnAssetToggle.Text = ">";
            this.btnAssetToggle.UseVisualStyleBackColor = false;
            this.btnAssetToggle.Click += new System.EventHandler(this.btnAssetToggle_Click);
            // 
            // btnAnimToggle
            // 
            this.btnAnimToggle.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnAnimToggle.Location = new System.Drawing.Point(796, 0);
            this.btnAnimToggle.Name = "btnAnimToggle";
            this.btnAnimToggle.Size = new System.Drawing.Size(15, 546);
            this.btnAnimToggle.TabIndex = 6;
            this.btnAnimToggle.TabStop = false;
            this.btnAnimToggle.Text = "<";
            this.btnAnimToggle.UseVisualStyleBackColor = false;
            this.btnAnimToggle.Click += new System.EventHandler(this.btnAnimToggle_Click);
            // 
            // btnPlaybackToggle
            // 
            this.btnPlaybackToggle.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnPlaybackToggle.Location = new System.Drawing.Point(325, 483);
            this.btnPlaybackToggle.Name = "btnPlaybackToggle";
            this.btnPlaybackToggle.Size = new System.Drawing.Size(471, 15);
            this.btnPlaybackToggle.TabIndex = 8;
            this.btnPlaybackToggle.TabStop = false;
            this.btnPlaybackToggle.UseVisualStyleBackColor = false;
            this.btnPlaybackToggle.Click += new System.EventHandler(this.btnPlaybackToggle_Click);
            // 
            // animTimer
            // 
            this.animTimer.Interval = 10;
            this.animTimer.Tick += new System.EventHandler(this.animTimer_Tick);
            // 
            // spltAssets
            // 
            this.spltAssets.Location = new System.Drawing.Point(306, 0);
            this.spltAssets.Name = "spltAssets";
            this.spltAssets.Size = new System.Drawing.Size(4, 546);
            this.spltAssets.TabIndex = 9;
            this.spltAssets.TabStop = false;
            this.spltAssets.Visible = false;
            // 
            // btnOptionToggle
            // 
            this.btnOptionToggle.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnOptionToggle.Location = new System.Drawing.Point(325, 40);
            this.btnOptionToggle.Name = "btnOptionToggle";
            this.btnOptionToggle.Size = new System.Drawing.Size(471, 15);
            this.btnOptionToggle.TabIndex = 11;
            this.btnOptionToggle.TabStop = false;
            this.btnOptionToggle.UseVisualStyleBackColor = false;
            this.btnOptionToggle.Click += new System.EventHandler(this.btnOptionToggle_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.Control;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.kinectToolStripMenuItem1,
            this.targetModelToolStripMenuItem,
            this.kinectToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(984, 24);
            this.menuStrip1.TabIndex = 13;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.Visible = false;
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newSceneToolStripMenuItem,
            this.openFileToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newSceneToolStripMenuItem
            // 
            this.newSceneToolStripMenuItem.Name = "newSceneToolStripMenuItem";
            this.newSceneToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.newSceneToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.newSceneToolStripMenuItem.Text = "New Scene";
            this.newSceneToolStripMenuItem.Click += new System.EventHandler(this.newSceneToolStripMenuItem_Click);
            // 
            // openFileToolStripMenuItem
            // 
            this.openFileToolStripMenuItem.Name = "openFileToolStripMenuItem";
            this.openFileToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openFileToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.openFileToolStripMenuItem.Text = "Open File";
            this.openFileToolStripMenuItem.Click += new System.EventHandler(this.openFileToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.undoToolStripMenuItem,
            this.redoToolStripMenuItem,
            this.modifyLightingToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // undoToolStripMenuItem
            // 
            this.undoToolStripMenuItem.Enabled = false;
            this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
            this.undoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.undoToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.undoToolStripMenuItem.Text = "Undo";
            this.undoToolStripMenuItem.Click += new System.EventHandler(this.undoToolStripMenuItem_Click);
            this.undoToolStripMenuItem.EnabledChanged += new System.EventHandler(this.undoToolStripMenuItem_EnabledChanged);
            // 
            // redoToolStripMenuItem
            // 
            this.redoToolStripMenuItem.Enabled = false;
            this.redoToolStripMenuItem.Name = "redoToolStripMenuItem";
            this.redoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y)));
            this.redoToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.redoToolStripMenuItem.Text = "Redo";
            this.redoToolStripMenuItem.Click += new System.EventHandler(this.redoToolStripMenuItem_Click);
            this.redoToolStripMenuItem.EnabledChanged += new System.EventHandler(this.redoToolStripMenuItem_EnabledChanged);
            // 
            // modifyLightingToolStripMenuItem
            // 
            this.modifyLightingToolStripMenuItem.Name = "modifyLightingToolStripMenuItem";
            this.modifyLightingToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.modifyLightingToolStripMenuItem.Text = "Modify Lighting";
            this.modifyLightingToolStripMenuItem.Click += new System.EventHandler(this.modifyLightingToolStripMenuItem_Click);
            // 
            // kinectToolStripMenuItem1
            // 
            this.kinectToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editorsToolStripMenuItem,
            this.backColorToolStripMenuItem,
            this.modelToolStripMenuItem,
            this.movesetToolStripMenuItem1});
            this.kinectToolStripMenuItem1.Name = "kinectToolStripMenuItem1";
            this.kinectToolStripMenuItem1.Size = new System.Drawing.Size(44, 20);
            this.kinectToolStripMenuItem1.Text = "View";
            // 
            // editorsToolStripMenuItem
            // 
            this.editorsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showAssets,
            this.showAnim,
            this.showPlay,
            this.showOptions,
            this.showMoveset});
            this.editorsToolStripMenuItem.Name = "editorsToolStripMenuItem";
            this.editorsToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.editorsToolStripMenuItem.Text = "Editors";
            // 
            // showAssets
            // 
            this.showAssets.Name = "showAssets";
            this.showAssets.Size = new System.Drawing.Size(162, 22);
            this.showAssets.Text = "Assets Panel";
            this.showAssets.CheckedChanged += new System.EventHandler(this.showAssets_CheckedChanged);
            this.showAssets.Click += new System.EventHandler(this.showAssets_Click);
            // 
            // showAnim
            // 
            this.showAnim.Name = "showAnim";
            this.showAnim.Size = new System.Drawing.Size(162, 22);
            this.showAnim.Text = "Animation Panel";
            this.showAnim.CheckedChanged += new System.EventHandler(this.showAnim_CheckedChanged);
            this.showAnim.Click += new System.EventHandler(this.showAnim_Click);
            // 
            // showPlay
            // 
            this.showPlay.Name = "showPlay";
            this.showPlay.Size = new System.Drawing.Size(162, 22);
            this.showPlay.Text = "Playback Panel";
            this.showPlay.CheckedChanged += new System.EventHandler(this.showPlay_CheckedChanged);
            this.showPlay.Click += new System.EventHandler(this.showPlay_Click);
            // 
            // showOptions
            // 
            this.showOptions.Name = "showOptions";
            this.showOptions.Size = new System.Drawing.Size(162, 22);
            this.showOptions.Text = "Options Panel";
            this.showOptions.CheckedChanged += new System.EventHandler(this.showOptions_CheckedChanged);
            this.showOptions.Click += new System.EventHandler(this.showOptions_Click);
            // 
            // showMoveset
            // 
            this.showMoveset.Name = "showMoveset";
            this.showMoveset.Size = new System.Drawing.Size(162, 22);
            this.showMoveset.Text = "Moveset Panel";
            this.showMoveset.CheckedChanged += new System.EventHandler(this.showMoveset_CheckedChanged);
            this.showMoveset.Click += new System.EventHandler(this.showMoveset_Click);
            // 
            // backColorToolStripMenuItem
            // 
            this.backColorToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setColorToolStripMenuItem,
            this.loadImageToolStripMenuItem,
            this.toggleFloor,
            this.resetCameraToolStripMenuItem});
            this.backColorToolStripMenuItem.Name = "backColorToolStripMenuItem";
            this.backColorToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.backColorToolStripMenuItem.Text = "Model Viewer";
            // 
            // setColorToolStripMenuItem
            // 
            this.setColorToolStripMenuItem.Name = "setColorToolStripMenuItem";
            this.setColorToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.setColorToolStripMenuItem.Text = "Set Color";
            this.setColorToolStripMenuItem.Click += new System.EventHandler(this.setColorToolStripMenuItem_Click);
            // 
            // loadImageToolStripMenuItem
            // 
            this.loadImageToolStripMenuItem.Name = "loadImageToolStripMenuItem";
            this.loadImageToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.loadImageToolStripMenuItem.Text = "Load Image";
            this.loadImageToolStripMenuItem.Click += new System.EventHandler(this.loadImageToolStripMenuItem_Click);
            // 
            // toggleFloor
            // 
            this.toggleFloor.Name = "toggleFloor";
            this.toggleFloor.Size = new System.Drawing.Size(187, 22);
            this.toggleFloor.Text = "Floor (Off)";
            this.toggleFloor.Click += new System.EventHandler(this.toggleFloor_Click);
            // 
            // resetCameraToolStripMenuItem
            // 
            this.resetCameraToolStripMenuItem.Name = "resetCameraToolStripMenuItem";
            this.resetCameraToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.resetCameraToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.resetCameraToolStripMenuItem.Text = "Reset Camera";
            this.resetCameraToolStripMenuItem.Click += new System.EventHandler(this.resetCameraToolStripMenuItem_Click_1);
            // 
            // modelToolStripMenuItem
            // 
            this.modelToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toggleBones,
            this.togglePolygons,
            this.toggleVertices});
            this.modelToolStripMenuItem.Name = "modelToolStripMenuItem";
            this.modelToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.modelToolStripMenuItem.Text = "Model";
            // 
            // toggleBones
            // 
            this.toggleBones.Checked = true;
            this.toggleBones.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toggleBones.Name = "toggleBones";
            this.toggleBones.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.B)));
            this.toggleBones.Size = new System.Drawing.Size(191, 22);
            this.toggleBones.Text = "Bones (On)";
            this.toggleBones.Click += new System.EventHandler(this.toggleBonesToolStripMenuItem_Click);
            // 
            // togglePolygons
            // 
            this.togglePolygons.Checked = true;
            this.togglePolygons.CheckState = System.Windows.Forms.CheckState.Checked;
            this.togglePolygons.Name = "togglePolygons";
            this.togglePolygons.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
            this.togglePolygons.Size = new System.Drawing.Size(191, 22);
            this.togglePolygons.Text = "Polygons (On)";
            this.togglePolygons.Click += new System.EventHandler(this.togglePolygonsToolStripMenuItem_Click);
            // 
            // toggleVertices
            // 
            this.toggleVertices.Name = "toggleVertices";
            this.toggleVertices.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.toggleVertices.Size = new System.Drawing.Size(191, 22);
            this.toggleVertices.Text = "Vertices (Off)";
            this.toggleVertices.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // movesetToolStripMenuItem1
            // 
            this.movesetToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.hitboxesOffToolStripMenuItem,
            this.hurtboxesOffToolStripMenuItem});
            this.movesetToolStripMenuItem1.Name = "movesetToolStripMenuItem1";
            this.movesetToolStripMenuItem1.Size = new System.Drawing.Size(146, 22);
            this.movesetToolStripMenuItem1.Text = "Moveset";
            // 
            // hitboxesOffToolStripMenuItem
            // 
            this.hitboxesOffToolStripMenuItem.Name = "hitboxesOffToolStripMenuItem";
            this.hitboxesOffToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.hitboxesOffToolStripMenuItem.Text = "Hitboxes (Off)";
            this.hitboxesOffToolStripMenuItem.Click += new System.EventHandler(this.hitboxesOffToolStripMenuItem_Click);
            // 
            // hurtboxesOffToolStripMenuItem
            // 
            this.hurtboxesOffToolStripMenuItem.Name = "hurtboxesOffToolStripMenuItem";
            this.hurtboxesOffToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.hurtboxesOffToolStripMenuItem.Text = "Hurtboxes (Off)";
            this.hurtboxesOffToolStripMenuItem.Click += new System.EventHandler(this.hurtboxesOffToolStripMenuItem_Click);
            // 
            // targetModelToolStripMenuItem
            // 
            this.targetModelToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.hideFromSceneToolStripMenuItem,
            this.deleteToolStripMenuItem,
            this.hideAllOtherModelsToolStripMenuItem,
            this.deleteAllOtherModelsToolStripMenuItem,
            this.openModelSwitherToolStripMenuItem});
            this.targetModelToolStripMenuItem.Name = "targetModelToolStripMenuItem";
            this.targetModelToolStripMenuItem.Size = new System.Drawing.Size(90, 20);
            this.targetModelToolStripMenuItem.Text = "Target Model";
            // 
            // hideFromSceneToolStripMenuItem
            // 
            this.hideFromSceneToolStripMenuItem.Name = "hideFromSceneToolStripMenuItem";
            this.hideFromSceneToolStripMenuItem.Size = new System.Drawing.Size(231, 22);
            this.hideFromSceneToolStripMenuItem.Text = "Hide from scene";
            this.hideFromSceneToolStripMenuItem.Click += new System.EventHandler(this.hideFromSceneToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(231, 22);
            this.deleteToolStripMenuItem.Text = "Delete from scene";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // hideAllOtherModelsToolStripMenuItem
            // 
            this.hideAllOtherModelsToolStripMenuItem.Name = "hideAllOtherModelsToolStripMenuItem";
            this.hideAllOtherModelsToolStripMenuItem.Size = new System.Drawing.Size(231, 22);
            this.hideAllOtherModelsToolStripMenuItem.Text = "Hide all other models";
            this.hideAllOtherModelsToolStripMenuItem.Click += new System.EventHandler(this.hideAllOtherModelsToolStripMenuItem_Click);
            // 
            // deleteAllOtherModelsToolStripMenuItem
            // 
            this.deleteAllOtherModelsToolStripMenuItem.Name = "deleteAllOtherModelsToolStripMenuItem";
            this.deleteAllOtherModelsToolStripMenuItem.Size = new System.Drawing.Size(231, 22);
            this.deleteAllOtherModelsToolStripMenuItem.Text = "Delete all other models";
            this.deleteAllOtherModelsToolStripMenuItem.Click += new System.EventHandler(this.deleteAllOtherModelsToolStripMenuItem_Click);
            // 
            // openModelSwitherToolStripMenuItem
            // 
            this.openModelSwitherToolStripMenuItem.Name = "openModelSwitherToolStripMenuItem";
            this.openModelSwitherToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.H)));
            this.openModelSwitherToolStripMenuItem.Size = new System.Drawing.Size(231, 22);
            this.openModelSwitherToolStripMenuItem.Text = "Open Model Switcher";
            this.openModelSwitherToolStripMenuItem.Click += new System.EventHandler(this.openModelSwitherToolStripMenuItem_Click);
            // 
            // kinectToolStripMenuItem
            // 
            this.kinectToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.syncKinectToolStripMenuItem,
            this.notYetImplementedToolStripMenuItem,
            this.startTrackingToolStripMenuItem});
            this.kinectToolStripMenuItem.Enabled = false;
            this.kinectToolStripMenuItem.Name = "kinectToolStripMenuItem";
            this.kinectToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.kinectToolStripMenuItem.Text = "Kinect";
            this.kinectToolStripMenuItem.Visible = false;
            // 
            // syncKinectToolStripMenuItem
            // 
            this.syncKinectToolStripMenuItem.Enabled = false;
            this.syncKinectToolStripMenuItem.Name = "syncKinectToolStripMenuItem";
            this.syncKinectToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.syncKinectToolStripMenuItem.Text = "Sync Kinect";
            // 
            // notYetImplementedToolStripMenuItem
            // 
            this.notYetImplementedToolStripMenuItem.Enabled = false;
            this.notYetImplementedToolStripMenuItem.Name = "notYetImplementedToolStripMenuItem";
            this.notYetImplementedToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.notYetImplementedToolStripMenuItem.Text = "Register Pose";
            // 
            // startTrackingToolStripMenuItem
            // 
            this.startTrackingToolStripMenuItem.Enabled = false;
            this.startTrackingToolStripMenuItem.Name = "startTrackingToolStripMenuItem";
            this.startTrackingToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.startTrackingToolStripMenuItem.Text = "Start Tracking";
            this.startTrackingToolStripMenuItem.Click += new System.EventHandler(this.startTrackingToolStripMenuItem_Click);
            // 
            // KinectPanel
            // 
            this.KinectPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.KinectPanel.BackColor = System.Drawing.Color.Transparent;
            this.KinectPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.KinectPanel.Controls.Add(this.label1);
            this.KinectPanel.Location = new System.Drawing.Point(799, 15);
            this.KinectPanel.Name = "KinectPanel";
            this.KinectPanel.Size = new System.Drawing.Size(170, 170);
            this.KinectPanel.TabIndex = 14;
            this.KinectPanel.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 157);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Open";
            // 
            // modelPanel1
            // 
            this.modelPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.modelPanel1.InitialYFactor = 100;
            this.modelPanel1.InitialZoomFactor = 5;
            this.modelPanel1.Location = new System.Drawing.Point(325, 55);
            this.modelPanel1.Name = "modelPanel1";
            this.modelPanel1.RotationScale = 0.4F;
            this.modelPanel1.Size = new System.Drawing.Size(471, 428);
            this.modelPanel1.TabIndex = 0;
            this.modelPanel1.TranslationScale = 0.05F;
            this.modelPanel1.ZoomScale = 2.5F;
            this.modelPanel1.PreRender += new System.Windows.Forms.GLRenderEventHandler(this.modelPanel1_PreRender);
            this.modelPanel1.PostRender += new System.Windows.Forms.GLRenderEventHandler(this.modelPanel1_PostRender);
            this.modelPanel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.modelPanel1_MouseDown);
            this.modelPanel1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.modelPanel1_MouseMove);
            this.modelPanel1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.modelPanel1_MouseUp);
            // 
            // pnlOptions
            // 
            this.pnlOptions.BackColor = System.Drawing.SystemColors.Control;
            this.pnlOptions.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlOptions.Location = new System.Drawing.Point(325, 0);
            this.pnlOptions.Name = "pnlOptions";
            this.pnlOptions.Size = new System.Drawing.Size(471, 40);
            this.pnlOptions.TabIndex = 10;
            this.pnlOptions.Visible = false;
            this.pnlOptions.RenderStateChanged += new System.EventHandler(this.RenderStateChanged);
            this.pnlOptions.HtBoxesChanged += new System.EventHandler(this.HtBoxesChanged);
            this.pnlOptions.TargetModelChanged += new System.EventHandler(this.TargetChanged);
            this.pnlOptions.VertexChanged += new System.EventHandler(this.pnlOptions_FloorRenderChanged);
            this.pnlOptions.CamResetClicked += new System.EventHandler(this.pnlOptions_CamResetClicked);
            this.pnlOptions.FloorRenderChanged += new System.EventHandler(this.pnlOptions_FloorRenderChanged);
            this.pnlOptions.UndoClicked += new System.EventHandler(this.Undo);
            this.pnlOptions.RedoClicked += new System.EventHandler(this.Redo);
            // 
            // pnlAssets
            // 
            this.pnlAssets.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlAssets.Location = new System.Drawing.Point(205, 0);
            this.pnlAssets.Name = "pnlAssets";
            this.pnlAssets.Size = new System.Drawing.Size(101, 546);
            this.pnlAssets.TabIndex = 4;
            this.pnlAssets.Visible = false;
            this.pnlAssets.Vis0Changed += new System.EventHandler(this.Vis0Changed);
            this.pnlAssets.SelectedPolygonChanged += new System.EventHandler(this.SelectedPolygonChanged);
            this.pnlAssets.SelectedBoneChanged += new System.EventHandler(this.pnlAssets_SelectedBoneChanged);
            this.pnlAssets.RenderStateChanged += new System.EventHandler(this.RenderStateChanged);
            this.pnlAssets.Vis0Updated += new System.EventHandler(this.UpdateVis0);
            this.pnlAssets.Key += new System.EventHandler(this.Key);
            this.pnlAssets.Unkey += new System.EventHandler(this.Unkey);
            // 
            // pnlAnim
            // 
            this.pnlAnim.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pnlAnim.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlAnim.Location = new System.Drawing.Point(811, 0);
            this.pnlAnim.Name = "pnlAnim";
            this.pnlAnim.Size = new System.Drawing.Size(173, 546);
            this.pnlAnim.TabIndex = 12;
            this.pnlAnim.Visible = false;
            this.pnlAnim.RenderStateChanged += new System.EventHandler(this.RenderStateChanged);
            this.pnlAnim.AnimStateChanged += new System.EventHandler(this.pnlAnim_AnimStateChanged);
            this.pnlAnim.SelectedAnimationChanged += new System.EventHandler(this.pnlAnim_SelectedAnimationChanged);
            this.pnlAnim.ReferenceLoaded += new System.Windows.Forms.ModelAnimPanel.ReferenceEventHandler(this.pnlAnim_ReferenceLoaded);
            this.pnlAnim.ReferenceClosed += new System.Windows.Forms.ModelAnimPanel.ReferenceEventHandler(this.pnlAnim_ReferenceClosed);
            // 
            // pnlPlayback
            // 
            this.pnlPlayback.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlPlayback.Location = new System.Drawing.Point(325, 498);
            this.pnlPlayback.Name = "pnlPlayback";
            this.pnlPlayback.Size = new System.Drawing.Size(471, 48);
            this.pnlPlayback.TabIndex = 16;
            this.pnlPlayback.Visible = false;
            this.pnlPlayback.LoopChanged += new System.EventHandler(this.chkLoop_CheckedChanged);
            this.pnlPlayback.PlayClicked += new System.EventHandler(this.btnPlay_Click);
            this.pnlPlayback.FPSChanged += new System.EventHandler(this.numFPS_ValueChanged);
            this.pnlPlayback.FrameIndexChanged += new System.EventHandler(this.numFrameIndex_ValueChanged);
            this.pnlPlayback.TotalFramesChanged += new System.EventHandler(this.numTotalFrames_ValueChanged);
            // 
            // pnlMoveset
            // 
            this.pnlMoveset.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pnlMoveset.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlMoveset.Location = new System.Drawing.Point(0, 0);
            this.pnlMoveset.Name = "pnlMoveset";
            this.pnlMoveset.Size = new System.Drawing.Size(201, 546);
            this.pnlMoveset.TabIndex = 17;
            this.pnlMoveset.Visible = false;
            this.pnlMoveset.RenderStateChanged += new System.EventHandler(this.RenderStateChanged);
            this.pnlMoveset.FileChanged += new System.EventHandler(this.FileChanged);
            // 
            // spltMoveset
            // 
            this.spltMoveset.Location = new System.Drawing.Point(201, 0);
            this.spltMoveset.Name = "spltMoveset";
            this.spltMoveset.Size = new System.Drawing.Size(4, 546);
            this.spltMoveset.TabIndex = 18;
            this.spltMoveset.TabStop = false;
            this.spltMoveset.Visible = false;
            // 
            // ModelEditControl
            // 
            this.AllowDrop = true;
            this.Controls.Add(this.KinectPanel);
            this.Controls.Add(this.modelPanel1);
            this.Controls.Add(this.btnOptionToggle);
            this.Controls.Add(this.pnlOptions);
            this.Controls.Add(this.btnPlaybackToggle);
            this.Controls.Add(this.pnlPlayback);
            this.Controls.Add(this.btnAssetToggle);
            this.Controls.Add(this.spltAssets);
            this.Controls.Add(this.btnAnimToggle);
            this.Controls.Add(this.pnlAssets);
            this.Controls.Add(this.spltMoveset);
            this.Controls.Add(this.pnlMoveset);
            this.Controls.Add(this.pnlAnim);
            this.Controls.Add(this.menuStrip1);
            this.Name = "ModelEditControl";
            this.Size = new System.Drawing.Size(984, 546);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.ModelEditControl_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.ModelEditControl_DragEnter);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.KinectPanel.ResumeLayout(false);
            this.KinectPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private const float _orbRadius = 1.0f;
        private const float _circRadius = 1.2f;
        private const float _axisSnapRange = 7.0f;
        private const float _selectRange = 0.03f; //Selection error range for orb and circ
        private const float _selectOrbScale = _selectRange / _orbRadius;
        private const float _circOrbScale = _circRadius / _orbRadius;

        public List<SaveState2> undoSaves = new List<SaveState2>();
        public List<SaveState2> redoSaves = new List<SaveState2>();
        public int saveIndex = 0;
        public bool firstUndo = false;

        public event EventHandler TargetModelChanged;

        private delegate void DelegateOpenFile(String s);
        private DelegateOpenFile m_DelegateOpenFile;

        private int _animFrame, _maxFrame;
        //private int _animFrame { get { if (_targetModel != null) return _targetModel._animFrame; else return 0; } set { if (_targetModel != null) _targetModel._animFrame = value; } }
        private bool _updating, _loop;
        
        private CHR0Node _selectedAnim;

        private bool rotating, _translating;
        private bool _rotating { get { return rotating; } set { rotating = value; pnlAnim._rotating = rotating; } }
        private Vector3 _lastPoint;
        private Vector3 _oldAngles;
        private bool _snapX, _snapY, _snapZ, _snapCirc;
        public List<MDL0Node> _targetModels = new List<MDL0Node>();
        private MDL0Node _targetModel;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MDL0Node TargetModel
        {
            get { return _targetModel; }
            set { ModelChanged(value); }
        }
        private Color _clearColor;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Color ClearColor
        {
            get { return _clearColor; }
            set { _clearColor = value; }
        }
        private Image _bgImage;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Image BGImage
        {
            get { return _bgImage; }
            set { _bgImage = value; }
        }
        public ModelEditControl() { InitializeComponent(); }

        #region File Handling
        private void ModelEditControl_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void ModelEditControl_DragDrop(object sender, DragEventArgs e)
        {
            Array a = (Array)e.Data.GetData(DataFormats.FileDrop);
            if (a != null)
            {
                string s = null;
                for (int i = 0; i < a.Length; i++)
                {
                    s = a.GetValue(i).ToString();
                    this.BeginInvoke(m_DelegateOpenFile, new Object[] { s });
                }
            }
        }
        private void OpenFile(string file)
        {
            ResourceNode node = null;
            try
            {
                if ((node = NodeFactory.FromFile(null, file)) != null)
                {
                    if (_targetModels == null)
                        _targetModels = new List<MDL0Node>();

                    LoadModels(node, _targetModels);

                    if (TargetModel == null)
                        TargetModel = _targetModels[0];

                    pnlOptions.models.SelectedItem = TargetModel;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error loading model(s) from file.");
            }
        }

        private void LoadModels(ResourceNode node, List<MDL0Node> models)
        {
            switch (node.ResourceType)
            {
                case ResourceType.ARC:
                case ResourceType.MRG:
                case ResourceType.BRES:
                case ResourceType.BRESGroup:
                    foreach (ResourceNode n in node.Children)
                        LoadModels(n, models);
                    break;
                case ResourceType.MDL0:
                    AppendTarget((MDL0Node)node);
                    break;
            }
        }

        public void AppendTarget(MDL0Node model)
        {
            //if (_targetModels.IndexOf(model) == -1)
            _targetModels.Add(model);
            pnlOptions.models.Items.Add(model);
            modelPanel1.AddTarget(model);
            model.ApplyCHR(null, 0);
        }

        public bool CloseFiles()
        {
            return pnlAnim.CloseReferences() && pnlMoveset.CloseReferences();
        }

        public bool rstcam = true;
        public bool hide = false;
        private void ModelChanged(MDL0Node model)
        {
            if (_targetModels.IndexOf(model) == -1)
                _targetModels.Add(model);

            if (model == null)
                modelPanel1.RemoveTarget(_targetModel);

            if ((_targetModel = model) != null)
                modelPanel1.AddTarget(_targetModel);

            if (rstcam)
                modelPanel1.ResetCamera();
            else
                rstcam = true;

            pnlOptions.TargetModel = _targetModel;
            pnlAnim.TargetModel = _targetModel;
            pnlAssets.Attach(_targetModel);
            pnlMoveset.TargetModel = _targetModel;

            if (TargetModelChanged != null)
                TargetModelChanged(this, null);

            SetFrame(0);
        }
        #endregion

        #region Animation Controls
        private void AnimChanged()
        {
            if (_selectedAnim == null)
            {
                pnlPlayback.numFrameIndex.Maximum = _maxFrame = 0;
                pnlPlayback.numTotalFrames.Minimum = 0;
                pnlPlayback.numTotalFrames.Value = 0;
                pnlPlayback.numTotalFrames.Enabled = false;
                pnlPlayback.btnLast.Enabled = false;
                pnlPlayback.btnFirst.Enabled = false;
                SetFrame(0);
            }
            else
            {
                int oldMax = _maxFrame;
                _maxFrame = _selectedAnim._numFrames;

                _updating = true;
                pnlPlayback.numTotalFrames.Enabled = true;
                pnlPlayback.numTotalFrames.Value = _maxFrame;
                _updating = false;

                if (_maxFrame < oldMax)
                {
                    SetFrame(1);
                    pnlPlayback.numFrameIndex.Maximum = _maxFrame;
                }
                else
                {
                    pnlPlayback.numFrameIndex.Maximum = _maxFrame;
                    SetFrame(1);
                }
            }
            Vis0Changed(null, null);
            Pat0Changed(null, null);

        }
        public bool _playing = false;
        private void SetFrame(int index)
        {
            //if (_animFrame == index)
            //    return;

            _animFrame = _targetModel == null ? 0 : index;

            pnlPlayback.btnNextFrame.Enabled = _animFrame < _maxFrame;
            pnlPlayback.btnPrevFrame.Enabled = _animFrame > 0;

            pnlPlayback.btnLast.Enabled = _animFrame != _maxFrame;
            pnlPlayback.btnFirst.Enabled = _animFrame > 1;

            pnlPlayback.numFrameIndex.Value = _animFrame;

            pnlAnim.CurrentFrame = _animFrame;

            if (vis0 != null)
                ReadVis0();
            if (pat0 != null)
                ReadPat0();
            //if (srt0 != null)
            //    ReadSrt0();
            //if (shp0 != null)
            //    ReadShp0();

            modelPanel1.Invalidate();
        }
        private bool wasOff = false;
        private void PlayAnim()
        {
            if (_selectedAnim == null || _maxFrame == 1)
                return;

            _playing = true;

            if (pnlOptions.RenderBones == false)
                wasOff = true;

            pnlOptions.RenderBones = false;
            toggleBones.Checked = false;
            toggleBones.Text = "Bones (Off)";

            pnlAnim.EnableTransformEdit = false;

            if (_animFrame >= _maxFrame) //Reset anim
                SetFrame(1);

            if (_animFrame < _maxFrame)
            {
                animTimer.Start();
                pnlPlayback.btnPlay.Text = "Stop";
            }
            else
            {
                pnlOptions.RenderBones = true;
                toggleBones.Checked = true;
                toggleBones.Text = "Bones (On)";
                _playing = false;
            }
        }
        private void StopAnim()
        {
            animTimer.Stop();

            _playing = false;

            if (!wasOff)
            {
                pnlOptions.RenderBones = true;
                toggleBones.Checked = true;
                toggleBones.Text = "Bones (On)";
            }

            wasOff = false;

            pnlPlayback.btnPlay.Text = "Play";

            pnlAnim.EnableTransformEdit = true;
        }
        private void animTimer_Tick(object sender, EventArgs e)
        {
            if (_selectedAnim == null)
                return;

            if (_animFrame >= _maxFrame)
                if (!_loop)
                    StopAnim();
                else
                    SetFrame(1);
            else
                SetFrame(_animFrame + 1);
        }
        #endregion

        #region Button Click Functions
        private void pnlOptions_CamResetClicked(object sender, EventArgs e) { modelPanel1.ResetCamera(); }
        private void loadImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog d = new OpenFileDialog();
            d.Filter = "All Image Formats (*.png,*.tga,*.tif,*.tiff,*.bmp,*.jpg,*.jpeg,*.gif)|*.png;*.tga;*.tif;*.tiff;*.bmp;*.jpg;*.jpeg,*.gif|" +
            "Portable Network Graphics (*.png)|*.png|" +
            "Truevision TARGA (*.tga)|*.tga|" +
            "Tagged Image File Format (*.tif, *.tiff)|*.tif;*.tiff|" +
            "Bitmap (*.bmp)|*.bmp|" +
            "Jpeg (*.jpg,*.jpeg)|*.jpg;*.jpeg|" +
            "Gif (*.gif)|*.gif";
            d.Title = "Select an image to load";

            if (d.ShowDialog() == DialogResult.OK)
                modelPanel1.BackgroundImage = BGImage = Image.FromFile(d.FileName);
        }
        private void btnAssetToggle_Click(object sender, EventArgs e)
        {
            if (!showAssets.Checked)
                if (!showMoveset.Checked)
                    showAssets.Checked = true;
                else
                    showMoveset.Checked = false;
            else
                if (!showMoveset.Checked)
                    showMoveset.Checked = true;
                else
                    showAssets.Checked = false;
        }
        private void btnOptionToggle_Click(object sender, EventArgs e) 
        {
            if (menuStrip1.Visible)
                if (showOptions.Checked)
                    showOptions.Checked = menuStrip1.Visible = false;
                else
                    showOptions.Checked = true;
            else
                menuStrip1.Visible = true;

            DockKinectPanel();
        }
        private void btnPlaybackToggle_Click(object sender, EventArgs e) { showPlay.Checked = !showPlay.Checked; }
        private void btnAnimToggle_Click(object sender, EventArgs e)
        {
            if (showAnim.Checked = !showAnim.Checked)
                btnAnimToggle.Text = ">";
            else
                btnAnimToggle.Text = "<";

            DockKinectPanel();
        }
        private void btnPrevFrame_Click(object sender, EventArgs e) { pnlPlayback.numFrameIndex.Value--; }
        private void btnNextFrame_Click(object sender, EventArgs e) { pnlPlayback.numFrameIndex.Value++; }
        private void btnPlay_Click(object sender, EventArgs e)
        {
            if (animTimer.Enabled)
                StopAnim();
            else
                PlayAnim();
        }
        private void setColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dlgColor.ShowDialog(this) == DialogResult.OK)
                modelPanel1.BackColor = ClearColor = dlgColor.Color;
        }
        protected override bool ProcessKeyPreview(ref Message m)
        {
            if (m.Msg == 0x100)
            {
                Keys key = (Keys)m.WParam;
                if (key == Keys.PageUp)
                {
                    if (_selectedAnim != null)
                    {
                        if (_animFrame >= _maxFrame)
                            SetFrame(1);
                        else
                            SetFrame(_animFrame + 1);
                    }
                    return true;
                }
                else if (key == Keys.PageDown)
                {
                    if (_selectedAnim != null)
                    {
                        if (_animFrame == 0)
                            SetFrame(_maxFrame);
                        else
                            SetFrame(_animFrame - 1);
                    }
                    return true;
                }
                else if (key == Keys.Left)
                {
                    if (Control.ModifierKeys == (Keys.Control | Keys.Alt))
                    {
                        btnAssetToggle_Click(null, null);
                        return true;
                    }
                    return false;
                }
                else if (key == Keys.Right)
                {
                    if (Control.ModifierKeys == (Keys.Control | Keys.Alt))
                    {
                        btnAnimToggle_Click(null, null);
                        return true;
                    }
                    return false;
                }
                else if (key == Keys.Up)
                {
                    if (Control.ModifierKeys == (Keys.Control | Keys.Alt))
                    {
                        btnOptionToggle_Click(null, null);
                        return true;
                    }
                    return false;
                }
                else if (key == Keys.Down)
                {
                    if (Control.ModifierKeys == (Keys.Control | Keys.Alt))
                    {
                        btnPlaybackToggle_Click(null, null);
                        return true;
                    }
                    return false;
                }
                else if (key == Keys.Escape)
                {
                    if (_rotating)
                    {
                        //Undo rotation, make sure to reset keyframes
                        _rotating = false;
                        pnlAnim.numRotX.Value = _oldAngles._x;
                        pnlAnim.numRotY.Value = _oldAngles._y;
                        pnlAnim.numRotZ.Value = _oldAngles._z;
                        pnlAnim.BoxChanged(pnlAnim.numRotX, null);
                        pnlAnim.BoxChanged(pnlAnim.numRotY, null);
                        pnlAnim.BoxChanged(pnlAnim.numRotZ, null);
                    }
                    if (_translating)
                    {
                        //Undo translation, make sure to reset vertex position
                        _translating = false;
                        //pnlAssets.SelectedPolygon._manager._vertices[pnlAssets.SelectedVertex]._weightedPosition = _oldTrans;
                    }
                }
                else if (key == Keys.Space)
                {
                    btnPlay_Click(null, null);
                    //return true;
                }
                else if (Control.ModifierKeys == Keys.Control)
                {
                    if (key == Keys.Z)
                    {
                        //if (_targetModel._canUndo)
                        //pnlOptions.Undo_Click(null, null);

                        if (undoToolStripMenuItem.Enabled)
                            undoToolStripMenuItem_Click(null, null);

                        return true;
                    }
                    else if (key == Keys.Y)
                    {
                        //if (_targetModel._canRedo)
                        //pnlOptions.Redo_Click(null, null);

                        if (redoToolStripMenuItem.Enabled)
                            redoToolStripMenuItem_Click(null, null);

                        return true;
                    }
                    if (key == Keys.H)
                    {
                        ModelSwitcher switcher = new ModelSwitcher();
                        switcher.ShowDialog(this, _targetModels);
                        return true;
                    }
                    else if (key == Keys.L)
                    {
                        Unkey(null, null);
                        return true;
                    }
                    else if (key == Keys.K)
                    {
                        Key(null, null);
                        return true;
                    }
                }
            }
            return base.ProcessKeyPreview(ref m);
        }
        private void Key(object sender, EventArgs e)
        {
            if (pnlAnim.TransformObject != null && _selectedAnim != null)
            {
                CHR0EntryNode entry = _selectedAnim.FindChild(((MDL0BoneNode)pnlAnim.TransformObject).Name, false) as CHR0EntryNode;
                if (entry != null)
                    for (int i = 0x10; i < 0x19; i++)
                    {
                        entry.SetKeyframe((KeyFrameMode)i, _animFrame - 1, pnlAnim._transBoxes[i - 0x10].Value);
                        pnlAnim.BoxChanged(pnlAnim._transBoxes[i - 0x10], null);
                    }
            }
        }
        private void Unkey(object sender, EventArgs e)
        {
            if (pnlAnim.TransformObject != null && _selectedAnim != null)
            {
                CHR0EntryNode entry = _selectedAnim.FindChild(((MDL0BoneNode)pnlAnim.TransformObject).Name, false) as CHR0EntryNode;
                if (entry != null)
                    for (int i = 0x10; i < 0x19; i++)
                    {
                        entry.RemoveKeyframe((KeyFrameMode)i, _animFrame - 1);
                        pnlAnim.BoxChanged(pnlAnim._transBoxes[i - 0x10], null);
                    }
            }
        }
        private void openFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog d = new OpenFileDialog();
            d.Filter = "All Compatible Files (*.pac, *.pcs, *.brres, *.mrg, *.mdl0)|*.pac;*.pcs;*.brres;*.mrg;*.mdl0";
            d.Title = "Select a file to open";
            if (d.ShowDialog() == DialogResult.OK)
                OpenFile(d.FileName);
        }

        private void startTrackingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            KinectPanel.Visible = true;
        }

        private void newSceneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Are you sure you want to clear the current scene?\nYou will lose any unsaved data.", "Continue?", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                return;

            TargetModel.ResetSaves();
            TargetModel = null;
            for (int i = 0; i < _targetModels.Count; i++)
                if (_targetModels[i] != null)
                    _targetModels[i].ResetSaves();
            _targetModels = null;

            modelPanel1.ClearAll();

            pnlOptions.models.Items.Clear();
        }

        //private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    if (_targetModel._canUndo)
        //        pnlOptions.Undo_Click(this, null);
        //}

        //private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    if (_targetModel._canRedo)
        //        pnlOptions.Redo_Click(this, null);
        //}

        private void toggleBonesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnlOptions.RenderBones = !pnlOptions.RenderBones;
            if (pnlOptions.RenderBones == false)
            {
                toggleBones.Checked = false;
                toggleBones.Text = "Bones (Off)";
            }
            else
            {
                toggleBones.Checked = true;
                toggleBones.Text = "Bones (On)";
            }
        }

        private void togglePolygonsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (togglePolygons.Text == "Polygons (On)")
            {
                togglePolygons.Checked = false;
                pnlOptions.chkPolygons.CheckState = CheckState.Unchecked;
                togglePolygons.Text = "Polygons (Off)";
            }
            else
            {
                togglePolygons.Checked = true;
                pnlOptions.chkPolygons.CheckState = CheckState.Checked;
                togglePolygons.Text = "Polygons (On)";
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            pnlOptions.RenderVertices = !pnlOptions.RenderVertices;
            if (pnlOptions.RenderVertices == false)
            {
                toggleVertices.Checked = false;
                toggleVertices.Text = "Vertices (Off)";
            }
            else
            {
                toggleVertices.Checked = true;
                toggleVertices.Text = "Vertices (On)";
            }
        }

        private void renderWireframeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnlOptions.chkPolygons.CheckState = CheckState.Indeterminate;
        }

        private void openModelSwitherToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new ModelSwitcher().ShowDialog(this, _targetModels);
        }

        private void hideFromSceneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rstcam = false;

            modelPanel1.RemoveTarget(TargetModel);

            if (_targetModels != null && _targetModels.Count != 0)
                TargetModel = _targetModels[0];

            modelPanel1.Invalidate();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rstcam = false;

            modelPanel1.RemoveTarget(TargetModel);
            _targetModels.Remove(TargetModel);
            pnlOptions.models.Items.Remove(TargetModel);

            if (_targetModels != null && _targetModels.Count != 0)
                TargetModel = _targetModels[0];

            modelPanel1.Invalidate();
        }

        private void hideAllOtherModelsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (MDL0Node node in _targetModels)
                if (node != TargetModel)
                    modelPanel1.RemoveTarget(node);

            modelPanel1.Invalidate();
        }

        private void deleteAllOtherModelsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (MDL0Node node in _targetModels)
                if (node != TargetModel)
                {
                    _targetModels.Remove(node);
                    modelPanel1.RemoveTarget(node);
                    pnlOptions.models.Items.Remove(node);
                }

            modelPanel1.Invalidate();
        }
        private void modifyLightingToolStripMenuItem_Click(object sender, EventArgs e) { new LightEditor().ShowDialog(this); }
        private void showMoveset_Click(object sender, EventArgs e) { showMoveset.Checked = !showMoveset.Checked; }
        private void showAssets_Click(object sender, EventArgs e) { showAssets.Checked = !showAssets.Checked; }
        private void hitboxesOffToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnlOptions.chkHitboxes.Checked = !pnlOptions.chkHitboxes.Checked;
            if (pnlOptions.chkHitboxes.Checked == false)
            {
                hitboxesOffToolStripMenuItem.Checked = false;
                hitboxesOffToolStripMenuItem.Text = "Hitboxes (Off)";
            }
            else
            {
                hitboxesOffToolStripMenuItem.Checked = true;
                hitboxesOffToolStripMenuItem.Text = "Hitboxes (On)";
            }

            modelPanel1.Invalidate();
        }
        private void hurtboxesOffToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnlOptions.chkHurtboxes.Checked = !pnlOptions.chkHurtboxes.Checked;
            if (pnlOptions.chkHurtboxes.Checked == false)
            {
                hurtboxesOffToolStripMenuItem.Checked = false;
                hurtboxesOffToolStripMenuItem.Text = "Hurtboxes (Off)";
            }
            else
            {
                hurtboxesOffToolStripMenuItem.Checked = true;
                hurtboxesOffToolStripMenuItem.Text = "Hurtboxes (On)";
            }

            modelPanel1.Invalidate();
        }
        private void showAnim_Click(object sender, EventArgs e) { showAnim.Checked = !showAnim.Checked; }
        private void showPlay_Click(object sender, EventArgs e) { showPlay.Checked = !showPlay.Checked; }
        private void showOptions_Click(object sender, EventArgs e) { showOptions.Checked = !showOptions.Checked; }
        private void toggleFloor_Click(object sender, EventArgs e)
        {
            pnlOptions.RenderFloor = !pnlOptions.RenderFloor;
            if (pnlOptions.RenderFloor == false)
            {
                toggleFloor.Checked = false;
                toggleFloor.Text = "Floor (Off)";
            }
            else
            {
                toggleFloor.Checked = true;
                toggleFloor.Text = "Floor (On)";
            }
        }
        private void resetCameraToolStripMenuItem_Click_1(object sender, EventArgs e) { modelPanel1.ResetCamera(); }
        #endregion

        #region Value Change Functions
        private void pnlAnim_ReferenceLoaded(ResourceNode node) { modelPanel1.AddReference(node); }
        private void pnlAnim_ReferenceClosed(ResourceNode node) { modelPanel1.RemoveReference(node); }

        private void pnlAnim_SelectedAnimationChanged(object sender, EventArgs e) { _selectedAnim = pnlAnim.SelectedAnimation; AnimChanged(); }

        private void pnlAnim_AnimStateChanged(object sender, EventArgs e)
        {
            if (_selectedAnim == null)
                return;

            if (_animFrame < _selectedAnim.FrameCount)
                SetFrame(_animFrame);
            pnlPlayback.numTotalFrames.Value = _selectedAnim.FrameCount;
        }

        private void pnlOptions_FloorRenderChanged(object sender, EventArgs e)
        {
            if (pnlOptions.RenderVertices == false)
            {
                toggleVertices.Checked = false;
                toggleVertices.Text = "Vertices (Off)";
            }
            else
            {
                toggleVertices.Checked = true;
                toggleVertices.Text = "Vertices (On)";
            }

            if (pnlOptions.RenderFloor == false)
            {
                toggleFloor.Checked = false;
                toggleFloor.Text = "Floor (Off)";
            }
            else
            {
                toggleFloor.Checked = true;
                toggleFloor.Text = "Floor (On)";
            }

            modelPanel1.Invalidate();
        }

        private void Undo(object sender, EventArgs e)
        {
            if (undoToolStripMenuItem.Enabled)
                undoToolStripMenuItem_Click(null, null);
        }
        private void Redo(object sender, EventArgs e)
        {
            if (redoToolStripMenuItem.Enabled)
                redoToolStripMenuItem_Click(null, null);
        }
        private void ApplySave(object sender, EventArgs e)
        {
            SaveState save = pnlOptions._save;
            pnlAnim.ApplySave(save);
            SetFrame(save.frameIndex);
            modelPanel1.Invalidate();
        }
        private void numFrameIndex_ValueChanged(object sender, EventArgs e)
        {
            int val = (int)pnlPlayback.numFrameIndex.Value;
            if (val != _animFrame)
                SetFrame(val);
        }
        private void DockKinectPanel()
        {
            //Can't dock Kinect panel to top right? Dock it manually then
            KinectPanel.Location = new Point(
                this.Size.Width - ((pnlAnim.Visible ? 188 : 15) + KinectPanel.Width),
                (menuStrip1.Visible ? 24 : 0) + (pnlOptions.Visible ? 40 : 0) + 15);
        }
        private void numFPS_ValueChanged(object sender, EventArgs e) { animTimer.Interval = 1000 / (int)pnlPlayback.numFPS.Value; }
        private void chkLoop_CheckedChanged(object sender, EventArgs e) { _loop = pnlPlayback.chkLoop.Checked; }

        private void FileChanged(object sender, EventArgs e)
        {
            if (pnlMoveset._externalNode == null)
                pnlOptions.chkHurtboxes.Visible = pnlOptions.chkHitboxes.Visible = pnlOptions.chkHurtboxes.Checked = false;
            else
                pnlOptions.chkHurtboxes.Visible = pnlOptions.chkHitboxes.Visible = pnlOptions.chkHurtboxes.Checked = true;
        }

        private void HtBoxesChanged(object sender, EventArgs e)
        {
            if (pnlOptions.chkHurtboxes.Checked)
            {
                hurtboxesOffToolStripMenuItem.Checked = true;
                hurtboxesOffToolStripMenuItem.Text = "Hurtboxes (On)";
            }
            else
            {
                hurtboxesOffToolStripMenuItem.Checked = false;
                hurtboxesOffToolStripMenuItem.Text = "Hurtboxes (Off)";
            }

            if (pnlOptions.chkHitboxes.Checked)
            {
                hitboxesOffToolStripMenuItem.Checked = true;
                hitboxesOffToolStripMenuItem.Text = "Hitboxes (On)";
            }
            else
            {
                hitboxesOffToolStripMenuItem.Checked = false;
                hitboxesOffToolStripMenuItem.Text = "Hitboxes (Off)";
            }

            modelPanel1.Invalidate(); 
        }

        private void RenderStateChanged(object sender, EventArgs e) 
        { 
            if (pnlOptions.RenderBones == false)
            {
                toggleBones.Checked = false;
                toggleBones.Text = "Bones (Off)";
            }
            else
            {
                toggleBones.Checked = true;
                toggleBones.Text = "Bones (On)";
            }

            if (pnlOptions.chkPolygons.CheckState == CheckState.Unchecked)
            {
                togglePolygons.Checked = false;
                pnlOptions.chkPolygons.CheckState = CheckState.Unchecked;
                togglePolygons.Text = "Polygons (Off)";
            }
            else if (pnlOptions.chkPolygons.CheckState == CheckState.Indeterminate)
            {
                togglePolygons.CheckState = CheckState.Indeterminate;
                pnlOptions.chkPolygons.CheckState = CheckState.Indeterminate;
                togglePolygons.Text = "Polygons (Wireframe)";
            }
            else
            {
                togglePolygons.Checked = true;
                pnlOptions.chkPolygons.CheckState = CheckState.Checked;
                togglePolygons.Text = "Polygons (On)";
            }

            if (pnlOptions.RenderVertices == false)
            {
                toggleVertices.Checked = false;
                toggleVertices.Text = "Vertices (Off)";
            }
            else
            {
                toggleVertices.Checked = true;
                toggleVertices.Text = "Vertices (On)";
            }

            if (pnlOptions.RenderFloor == false)
            {
                toggleFloor.Checked = false;
                toggleFloor.Text = "Floor (Off)";
            }
            else
            {
                toggleFloor.Checked = true;
                toggleFloor.Text = "Floor (On)";
            }

            modelPanel1.Invalidate(); 
        }

        private void TargetChanged(object sender, EventArgs e) 
        {
            rstcam = false;
            TargetModel = (MDL0Node)pnlOptions.models.SelectedItem;

            undoSaves.Clear();
            redoSaves.Clear();
            saveIndex = 0;
            firstUndo = false;
        }
        
        private void SelectedPolygonChanged(object sender, EventArgs e) 
        {
            MDL0PolygonNode poly = pnlAssets.SelectedPolygon;
            _targetModel.polyIndex = _targetModel._polyList.IndexOf(poly);
            modelPanel1.Invalidate(); 
        }

        private void pnlAssets_SelectedBoneChanged(object sender, EventArgs e) { pnlAnim.TransformObject = pnlAssets.SelectedBone; }

        private void numTotalFrames_ValueChanged(object sender, EventArgs e)
        {
            if ((_selectedAnim == null) || (_updating))
                return;

            _maxFrame = (int)pnlPlayback.numTotalFrames.Value;

            _selectedAnim.FrameCount = _maxFrame;
            pnlPlayback.numFrameIndex.Maximum = _maxFrame;
        }
        private void showAssets_CheckedChanged(object sender, EventArgs e)
        {
            pnlAssets.Visible = spltAssets.Visible = showAssets.Checked;

            if (showAssets.Checked == false && showMoveset.Checked == false)
                btnAssetToggle.Text = ">";
            else if (showAssets.Checked == true && showMoveset.Checked == true)
                btnAssetToggle.Text = "<";
        }
        private void showMoveset_CheckedChanged(object sender, EventArgs e)
        {
            pnlMoveset.Visible = spltMoveset.Visible = showMoveset.Checked;

            if (showAssets.Checked == false && showMoveset.Checked == false)
                btnAssetToggle.Text = ">";
            else if (showAssets.Checked == true && showMoveset.Checked == true)
                btnAssetToggle.Text = "<";
        }
        private void showAnim_CheckedChanged(object sender, EventArgs e) { pnlAnim.Visible = showAnim.Checked; }
        private void showPlay_CheckedChanged(object sender, EventArgs e) { pnlPlayback.Visible = showPlay.Checked; }
        private void showOptions_CheckedChanged(object sender, EventArgs e) { pnlOptions.Visible = showOptions.Checked; }
        private void undoToolStripMenuItem_EnabledChanged(object sender, EventArgs e) { pnlOptions.Undo.Enabled = undoToolStripMenuItem.Enabled; }
        private void redoToolStripMenuItem_EnabledChanged(object sender, EventArgs e) { pnlOptions.Redo.Enabled = redoToolStripMenuItem.Enabled; }
        #endregion

        #region VIS0 Syncing
        public VIS0Node vis0 = null;
        private void Vis0Changed(object sender, EventArgs e)
        {
            if (pnlAssets._syncVis0)
            {
                BRESNode group = null;
                if (pnlAnim.SelectedAnimation != null && (group = pnlAnim.SelectedAnimation.Parent.Parent as BRESNode) != null)
                    if ((vis0 = (VIS0Node)group.FindChildByType(pnlAnim.SelectedAnimation.Name, true, ResourceType.VIS0)) == null)
                    {
                        vis0 = group.CreateResource<VIS0Node>(pnlAnim.SelectedAnimation.Name);
                        foreach (string n in pnlAssets.VIS0Indices.Keys)
                        {
                            VIS0EntryNode node = null;
                            if ((node = (VIS0EntryNode)vis0.FindChild(n, true)) == null && ((MDL0BoneNode)_targetModel.FindChildByType(n, true, ResourceType.MDL0Bone)).BoneIndex != 0 && n != "EyeYellowM")
                            {
                                node = vis0.CreateEntry();
                                node.Name = n;
                                node.MakeConstant(true);
                            }
                        }
                    }
                ReadVis0();
                //UpdateVis0(null, null);
            }
            else
                vis0 = null;
        }

        private void UpdateVis0(object sender, EventArgs e)
        {
            if (_animFrame == 0)
                return;

            if (vis0 != null && _selectedAnim != null)
            {
                if (vis0.FrameCount != _selectedAnim.FrameCount)
                    vis0.FrameCount = _selectedAnim.FrameCount;

                if (vis0.Loop != _selectedAnim.Loop)
                    vis0.Loop = _selectedAnim.Loop;

                int index = pnlAssets._polyIndex;
                MDL0BoneNode bone = ((MDL0PolygonNode)pnlAssets.lstPolygons.Items[index])._bone;

                VIS0EntryNode node = null;
                if ((node = (VIS0EntryNode)vis0.FindChild(bone.Name, true)) == null && bone.BoneIndex != 0 && bone.Name != "EyeYellowM")
                {
                    node = vis0.CreateEntry();
                    node.Name = bone.Name;
                    node.MakeConstant(true);
                }

                //Item is in the process of being un/checked; it's not un/checked at the given moment.
                //Use opposite of current check state.
                bool ANIMval = !pnlAssets.lstPolygons.GetItemChecked(index);

                bool nowAnimated = false, alreadyConstant = false;
            Top:
                if (node != null)
                    if (node._entryCount != 0) //Node is animated
                    {
                        bool VIS0val = node.GetEntry(_animFrame - 1);

                        if (VIS0val != ANIMval)
                            node.SetEntry(_animFrame - 1, ANIMval);
                    }
                    else //Node is constant
                    {
                        alreadyConstant = true;

                        bool VIS0val = node._flags.HasFlag(VIS0Flags.Enabled);

                        if (VIS0val != ANIMval)
                        {
                            node.MakeAnimated();
                            nowAnimated = true;
                            goto Top;
                        }
                    }

                //Check if the entry can be made constant.
                //No point if the entry has just been made animated or if the node is already constant.
                if (node != null && !alreadyConstant && !nowAnimated)
                {
                    bool constant = true;
                    for (int i = 0; i < node._entryCount; i++)
                    {
                        if (i == 0)
                            continue;

                        if (node.GetEntry(i - 1) != node.GetEntry(i))
                        {
                            constant = false;
                            break;
                        }
                    }
                    if (constant) node.MakeConstant(node.GetEntry(0));
                }
            }
        }

        public void ReadVis0()
        {
            if (_animFrame == 0)
                return;

            pnlAssets._vis0Updating = true;
            if (vis0 != null && _selectedAnim != null)
            {
                //if (vis0.FrameCount != _selectedAnim.FrameCount)
                //    UpdateVis0();

                foreach (string n in pnlAssets.VIS0Indices.Keys)
                {
                    VIS0EntryNode node = null;
                    List<int> indices = pnlAssets.VIS0Indices[n];
                    for (int i = 0; i < indices.Count; i++)
                    {
                        if ((node = (VIS0EntryNode)vis0.FindChild(((MDL0PolygonNode)pnlAssets.lstPolygons.Items[indices[i]])._bone.Name, true)) != null)
                        {
                            if (node._entryCount != 0 && _animFrame != 0)
                                pnlAssets.lstPolygons.SetItemChecked(indices[i], node.GetEntry(_animFrame - 1));
                            else
                                pnlAssets.lstPolygons.SetItemChecked(indices[i], node._flags.HasFlag(VIS0Flags.Enabled));
                        }
                    }
                }
            }
            pnlAssets._vis0Updating = false;
        }
        #endregion

        #region PAT0 Syncing
        public PAT0Node pat0 = null;
        private void Pat0Changed(object sender, EventArgs e)
        {
            if (pnlAssets._syncPat0)
            {
                BRESNode group = null;
                if (pnlAnim.SelectedAnimation != null && (group = pnlAnim.SelectedAnimation.Parent.Parent as BRESNode) != null)
                    if ((pat0 = (PAT0Node)group.FindChildByType(pnlAnim.SelectedAnimation.Name, true, ResourceType.PAT0)) == null)
                    {
                        pat0 = group.CreateResource<PAT0Node>(pnlAnim.SelectedAnimation.Name);
                        
                    }
                ReadPat0();
                //UpdatePat0(null, null);
            }
            else
                pat0 = null;
        }

        private void UpdatePat0(object sender, EventArgs e)
        {
            if (_animFrame == 0)
                return;

            if (pat0 != null && _selectedAnim != null)
            {

            }
        }

        public void ReadPat0()
        {
            if (_animFrame == 0)
                return;

            pnlAssets._vis0Updating = true;
            if (pat0 != null && _selectedAnim != null)
            {
                //if (pat0.FrameCount != _selectedAnim.FrameCount)
                //    UpdatePat0();
                
                
            }
            pnlAssets._pat0Updating = false;
        }
        #endregion

        #region Rendering
        private unsafe void modelPanel1_PreRender(object sender, GLContext context)
        {
            if (pnlOptions.RenderFloor)
            {
                GLTexture _bgTex = context.FindOrCreate<GLTexture>("TexBG", GLTexturePanel.CreateBG);

                float s = 10.0f, t = 10.0f;
                float e = 30.0f;

                context.glDisable((int)GLEnableCap.TEXTURE_GEN_S);
                context.glDisable((int)GLEnableCap.TEXTURE_GEN_T);
                context.glDisable((uint)GLEnableCap.CullFace);
                context.glDisable((uint)GLEnableCap.Lighting);
                context.glEnable(GLEnableCap.DepthTest);
                context.glPolygonMode(GLFace.Front, GLPolygonMode.Line);
                context.glPolygonMode(GLFace.Back, GLPolygonMode.Fill);

                context.glEnable(GLEnableCap.Texture2D);
                _bgTex.Bind();

                context.glColor(0.5f, 0.5f, 0.75f, 1.0f);
                context.glBegin(GLPrimitiveType.Quads);

                context.glTexCoord(0.0f, 0.0f);
                context.glVertex(-e, 0.0f, -e);
                context.glTexCoord(s, 0.0f);
                context.glVertex(e, 0.0f, -e);
                context.glTexCoord(s, t);
                context.glVertex(e, 0.0f, e);
                context.glTexCoord(0, t);
                context.glVertex(-e, 0.0f, e);

                context.glEnd();

                context.glDisable((uint)GLEnableCap.Texture2D);
            }
        }
        private unsafe void modelPanel1_PostRender(object sender, GLContext context)
        {
            //Render hurtboxes
            if (hurtboxesOffToolStripMenuItem.Checked)
                for (int i = 0; i < pnlMoveset.lstHurtboxes.Items.Count; i++)
                    if (pnlMoveset.lstHurtboxes.GetItemChecked(i))
                        ((MoveDefHurtBoxNode)pnlMoveset.lstHurtboxes.Items[i]).Render(context, pnlMoveset._selectedHurtboxIndex == i);

            context.glClear(GLClearMask.DepthBuffer);
            context.glEnable(GLEnableCap.DepthTest);

            if (pnlAssets.SelectedBone != null && !_playing) //Render rotation orb
            {
                context.glDisable((uint)GLEnableCap.Lighting);

                MDL0BoneNode bone = pnlAssets.SelectedBone;
                GLDisplayList circle = context.GetRingList();
                GLDisplayList sphere = context.GetCircleList();
                Matrix m;

                //Prepare camera-facing matrix
                Vector3 center = bone._frameMatrix.GetPoint();
                Vector3 cam = modelPanel1._camera.GetPoint();
                float radius = center.TrueDistance(cam) / _orbRadius * 0.1f;

                m = Matrix.TransformMatrix(new Vector3(radius), center.LookatAngles(cam) * Maths._rad2degf, center);
                context.glPushMatrix();
                context.glMultMatrix((float*)&m);

                //Orb
                context.glColor(0.7f, 0.7f, 0.7f, 0.15f);
                sphere.Call();

                context.glDisable((uint)GLEnableCap.DepthTest);

                //Container
                context.glColor(0.4f, 0.4f, 0.4f, 1.0f);
                circle.Call();

                //Circ
                if (_snapCirc)
                    context.glColor(1.0f, 1.0f, 0.0f, 1.0f);
                else
                    context.glColor(1.0f, 0.8f, 0.5f, 1.0f);
                context.glScale(_circOrbScale, _circOrbScale, _circOrbScale);
                circle.Call();

                //Pop
                context.glPopMatrix();

                context.glEnable(GLEnableCap.DepthTest);

                //Enter local space
                m = Matrix.TransformMatrix(new Vector3(radius), bone._frameMatrix.GetAngles(), center);
                context.glPushMatrix();
                context.glMultMatrix((float*)&m);

                //Z
                if (_snapZ)
                    context.glColor(1.0f, 1.0f, 0.0f, 1.0f);
                else
                    context.glColor(0.0f, 0.0f, 1.0f, 1.0f);

                circle.Call();
                context.glRotate(90.0f, 0.0f, 1.0f, 0.0f);

                //context.glBegin(GLPrimitiveType.Lines);
                //context.glVertex(0.0f, 0.0f, 0.0f);
                //context.glVertex(0.0f, 0.0f, 1 * 2);
                //context.glEnd();

                //X
                if (_snapX)
                    context.glColor(1.0f, 1.0f, 0.0f, 1.0f);
                else
                    context.glColor(1.0f, 0.0f, 0.0f, 1.0f);

                circle.Call();
                context.glRotate(90.0f, 1.0f, 0.0f, 0.0f);

                //context.glBegin(GLPrimitiveType.Lines);
                //context.glVertex(0.0f, 0.0f, 0.0f);
                //context.glVertex(1 * 2, 0.0f, 0.0f);
                //context.glEnd();

                //Y
                if (_snapY)
                    context.glColor(1.0f, 1.0f, 0.0f, 1.0f);
                else
                    context.glColor(0.0f, 1.0f, 0.0f, 1.0f);

                circle.Call();

                //context.glBegin(GLPrimitiveType.Lines);
                //context.glVertex(0.0f, 0.0f, 0.0f);
                //context.glVertex(0.0f, 1 * 2, 0.0f);
                //context.glEnd();

                //Pop
                context.glPopMatrix();

                //Clear depth buffer for next operation
                context.glClear(GLClearMask.DepthBuffer);
            }

            if (pnlOptions.RenderBones && !modelPanel1._grabbing && !modelPanel1._scrolling && !_playing) 
            {
                //Render invisible depth orbs
                context.glColorMask(false, false, false, false);

                if (_targetModel != null && _targetModel._boneList != null)
                {
                    GLDisplayList list = context.GetSphereList();
                    foreach (MDL0BoneNode bone in _targetModel._boneList)
                        RenderOrbRecursive(bone, context, list);
                }
                context.glColorMask(true, true, true, true);
            }

            //if (_targetModel != null)
            //{
            //    pnlOptions.Undo.Enabled = _targetModel._canUndo;
            //    pnlOptions.Redo.Enabled = _targetModel._canRedo;
            //}
        }

        private unsafe void RenderOrbRecursive(MDL0BoneNode bone, GLContext ctx, GLDisplayList list)
        {
            Matrix m = Matrix.TransformMatrix(new Vector3(MDL0BoneNode._nodeRadius), new Vector3(), bone._frameMatrix.GetPoint());
            ctx.glPushMatrix();
            ctx.glMultMatrix((float*)&m);

            list.Call();
            ctx.glPopMatrix();

            foreach (MDL0BoneNode b in bone.Children)
                RenderOrbRecursive(b, ctx, list);
        }

        private void modelPanel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == Forms.MouseButtons.Left)
            {
                //Reset snap flags
                _snapX = _snapY = _snapZ = _snapCirc = false;

                MDL0BoneNode bone = pnlAssets.SelectedBone;
                MDL0PolygonNode poly = pnlAssets.SelectedPolygon;

                //Re-target selected bone
                if (bone != null)
                {
                    //Try to re-target selected node
                    Vector3 center = bone._frameMatrix.GetPoint();

                    //Standard radius scaling snippet. This is used for orb scaling depending on camera distance.
                    float radius = center.TrueDistance(modelPanel1._camera.GetPoint()) / _orbRadius * 0.1f;

                    //Get point projected onto our orb.
                    Vector3 point = modelPanel1.ProjectCameraSphere(new Vector2(e.X, e.Y), center, radius, false);

                    //Check distances
                    float distance = point.TrueDistance(center);

                    if (Math.Abs(distance - radius) < (radius * _selectOrbScale)) //Point lies within orb radius
                    {
                        //Determine axis snapping
                        Vector3 angles = (bone._inverseFrameMatrix * point).GetAngles() * Maths._rad2degf;
                        angles._x = (float)Math.Abs(angles._x);
                        angles._y = (float)Math.Abs(angles._y);
                        angles._z = (float)Math.Abs(angles._z);

                        if (Math.Abs(angles._y - 90.0f) <= _axisSnapRange)
                            _snapX = true;
                        else if (angles._x >= (180 - _axisSnapRange) || angles._x <= _axisSnapRange)
                            _snapY = true;
                        else if (angles._y >= (180 - _axisSnapRange) || angles._y <= _axisSnapRange)
                            _snapZ = true;
                    }
                    else if (Math.Abs(distance - (radius * _circOrbScale)) < (radius * _selectOrbScale)) //Point lies on circ line
                        _snapCirc = true;
                    else
                    {
                        //Orb selection missed. Assign bone and move to next step.
                        pnlAssets.SelectedBone = bone = null;
                        goto Next;
                    }

                    //Bone re-targeted. Get angles and local point (aligned to snapping plane).
                    if (GetOrbPoint(new Vector2(e.X, e.Y), out point))
                    {
                        _rotating = true;
                        _oldAngles = bone._frameState._rotate;
                        _lastPoint = bone._inverseFrameMatrix * point;

                        CreateUndo(pnlAssets.SelectedBone);
                    }

                    //Ensure a redraw so the snapping indicators are correct
                    modelPanel1.Invalidate();
                }

            Next:

                //Try selecting new bone
                if (bone == null)
                {
                    float depth = modelPanel1.GetDepth(e.X, e.Y);
                    if ((depth < 1.0f) && (_targetModel != null) && (_targetModel._boneList != null))
                    {
                        Vector3 point = modelPanel1.UnProject(e.X, e.Y, depth);

                        //Find orb near chosen point
                        foreach (MDL0BoneNode b in _targetModel._boneList)
                            if (CompareDistanceRecursive(b, point, ref bone))
                                break;

                        //Assign new bone
                        if (bone != null)
                            pnlAssets.SelectedBone = bone;

                        //No need to redraw.
                    }
                }
            }
        }

        //bool rotateFirst = true;
        private void modelPanel1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == Forms.MouseButtons.Left)
            {
                if (_rotating) undoToolStripMenuItem.Enabled = true;

                _rotating = _translating = false;
            }
        }

        private unsafe void modelPanel1_MouseMove(object sender, MouseEventArgs e)
        {
            MDL0BoneNode bone = pnlAssets.SelectedBone;

            if (_rotating && bone != null)
            {
                Vector3 point;
                if (GetOrbPoint(new Vector2(e.X, e.Y), out point))
                {
                    //Convert to local point
                    Vector3 lPoint = bone._inverseFrameMatrix * point;

                    //Check for change in selection.
                    if (_lastPoint != lPoint)
                    {
                        //Get matrix with new rotation applied
                        Matrix m = bone._frameState._transform * Matrix.AxisAngleMatrix(_lastPoint, lPoint);

                        //Derive angles from matrices, get difference
                        Vector3 angles = m.GetAngles() - bone._frameState._transform.GetAngles();

                        //Truncate (allows winding)
                        if (angles._x > 180.0f) angles._x -= 360.0f;
                        if (angles._y > 180.0f) angles._y -= 360.0f;
                        if (angles._z > 180.0f) angles._z -= 360.0f;
                        if (angles._x < -180.0f) angles._x += 360.0f;
                        if (angles._y < -180.0f) angles._y += 360.0f;
                        if (angles._z < -180.0f) angles._z += 360.0f;

                        //Apply difference to axes that have changed (pnlAnim should handle this so keyframes are created)
                        if (angles._x != 0.0f) ApplyAngle(0, angles._x);
                        if (angles._y != 0.0f) ApplyAngle(1, angles._y);
                        if (angles._z != 0.0f) ApplyAngle(2, angles._z);

                        //Find new local mouse-point (should be the same)
                        _lastPoint = bone._inverseFrameMatrix * point;

                        //Redraw (taken care of by pnlAnim)
                        //modelPanel1.Invalidate();
                    }
                }
            }
        }

        //Updates specified angle by applying an offset.
        //Allows pnlAnim to handle the changes so keyframes are updated.
        private unsafe void ApplyAngle(int index, float offset)
        {
            NumericInputBox box = pnlAnim._transBoxes[index + 3];
            box.Value = (float)Math.Round(box._value + offset, 3);
            pnlAnim.BoxChanged(box, null);
        }

        //Gets world-point of specified mouse point projected onto the selected bone's local space.
        //Intersects the projected ray with the appropriate plane using the snap flags.
        private bool GetOrbPoint(Vector2 mousePoint, out Vector3 point)
        {
            MDL0BoneNode bone = pnlAssets.SelectedBone;
            if (bone == null)
            {
                point = new Vector3();
                return false;
            }

            Vector3 lineStart = modelPanel1.UnProject(mousePoint._x, mousePoint._y, 0.0f);
            Vector3 lineEnd = modelPanel1.UnProject(mousePoint._x, mousePoint._y, 1.0f);
            Vector3 center = bone._frameMatrix.GetPoint();
            Vector3 camera = modelPanel1._camera.GetPoint();
            Vector3 normal;
            float radius = center.TrueDistance(camera) / _orbRadius * 0.1f;

            if (_snapX)
                normal = (bone._frameMatrix * new Vector3(1.0f, 0.0f, 0.0f)).Normalize(center);
            else if (_snapY)
                normal = (bone._frameMatrix * new Vector3(0.0f, 1.0f, 0.0f)).Normalize(center);
            else if (_snapZ)
                normal = (bone._frameMatrix * new Vector3(0.0f, 0.0f, 1.0f)).Normalize(center);
            else if (_snapCirc)
            {
                radius *= _circOrbScale;
                normal = camera.Normalize(center);
            }
            else if (Maths.LineSphereIntersect(lineStart, lineEnd, center, radius, out point))
                return true;
            else
                normal = camera.Normalize(center);

            if (Maths.LinePlaneIntersect(lineStart, lineEnd, center, normal, out point))
            {
                point = Maths.PointAtLineDistance(center, point, radius);
                return true;
            }

            point = new Vector3();
            return false;
        }

        private bool CompareDistanceRecursive(MDL0BoneNode bone, Vector3 point, ref MDL0BoneNode match)
        {
            Vector3 center = bone._frameMatrix.GetPoint();
            float dist = center.TrueDistance(point);

            if (Math.Abs(dist - MDL0BoneNode._nodeRadius) < 0.01)
            {
                match = bone;
                return true;
            }

            foreach (MDL0BoneNode b in bone.Children)
                if (CompareDistanceRecursive(b, point, ref match))
                    return true;

            return false;
        }
        #endregion

        #region Undo & Redo
        private void CreateUndo(object sender, EventArgs e)
        {
            CreateUndo(pnlAssets.SelectedBone);
        }

        private void CreateUndo(MDL0BoneNode bone)
        {
            SaveState2 save = new SaveState2();
            save.bone = bone;
            save.frameState = bone._frameState;

            //If there are Redos, remove them.
            if (undoSaves.Count > saveIndex)
            {
                undoSaves.RemoveRange(saveIndex, undoSaves.Count - saveIndex);
                redoSaves.Clear();
                redoToolStripMenuItem.Enabled = false;
            }

            undoSaves.Add(save);
            saveIndex++;

            //Undo Buffer Limit
            if (undoSaves.Count > 10)
            {
                undoSaves.RemoveAt(0);
                saveIndex--;
            }

            undoToolStripMenuItem.Enabled = true;
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (undoToolStripMenuItem.Enabled)
            {
                if (firstUndo) { firstUndo = false; saveIndex++; }

                SaveState2 redoSave = new SaveState2(); //Create Redo before applying Undo
                pnlAssets.SelectedBone = undoSaves[saveIndex - 1].bone;
                redoSave.bone = pnlAssets.SelectedBone;
                redoSave.frameState = pnlAssets.SelectedBone._frameState;
                redoSaves.Add(redoSave);

                pnlAnim.Undo(undoSaves[--saveIndex]); //Apply Undo

                //if (saveIndex == saves.Count - 1) //Only on first Undo
                if (saveIndex <= 0) //Unenable with no Undos
                {
                    saveIndex = 0;
                    undoToolStripMenuItem.Enabled = false;
                }

                redoToolStripMenuItem.Enabled = true;
            }
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (redoToolStripMenuItem.Enabled)
            {
                pnlAssets.SelectedBone = redoSaves[undoSaves.Count - saveIndex - 1].bone;
                pnlAnim.Undo(redoSaves[undoSaves.Count - saveIndex++ - 1]);
                redoSaves.RemoveAt(redoSaves.Count - 1);

                if (saveIndex >= undoSaves.Count) { redoToolStripMenuItem.Enabled = false; saveIndex--; }
                if (!firstUndo) firstUndo = true;

                undoToolStripMenuItem.Enabled = true;
            }
        }
        #endregion
    }

    public class TransparentPanel : Panel
    {
        public TransparentPanel() { }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams createParams = base.CreateParams;
                createParams.ExStyle |= 0x00000020; // WS_EX_TRANSPARENT
                return createParams;
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // Do not paint background.
        }
    }
}
