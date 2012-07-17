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
using BrawlLib.IO;

namespace System.Windows.Forms
{
    public class ModelEditControl : UserControl
    {
        public ModelAssetPanel pnlAssets;
        public ModelMovesetPanel pnlMoveset;
        public ModelPanel modelPanel1;
        public ModelAnimPanel pnlAnim;

        #region Designer

        private ColorDialog dlgColor;
        private Button btnAssetToggle;
        private Button btnAnimToggle;
        private System.ComponentModel.IContainer components;
        private Button btnPlaybackToggle;
        public Timer animTimer;
        private Splitter spltAssets;
        private Button btnOptionToggle;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem editToolStripMenuItem;
        private ToolStripMenuItem openModelsToolStripMenuItem;
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
        private ToolStripMenuItem toggleFloor;
        private ToolStripMenuItem resetCameraToolStripMenuItem;
        private ToolStripMenuItem editorsToolStripMenuItem;
        private ToolStripMenuItem showAssets;
        private ToolStripMenuItem showBones;
        private ToolStripMenuItem showAnim;
        private ToolStripMenuItem showOptions;
        private ToolStripMenuItem showMoveset;
        public CHR0Editor chr0Editor;
        public ComboBox models;
        private Panel controlPanel;
        private Splitter spltAnims;
        private Panel panel1;
        public SRT0Editor srt0Editor;
        private ToolStripMenuItem fileTypesToolStripMenuItem;
        private ToolStripMenuItem playCHR0ToolStripMenuItem;
        private ToolStripMenuItem playSRT0ToolStripMenuItem;
        private ToolStripMenuItem playSHP0ToolStripMenuItem;
        private ToolStripMenuItem playPAT0ToolStripMenuItem;
        private ToolStripMenuItem playVIS0ToolStripMenuItem;
        private ToolStripMenuItem openAnimationsToolStripMenuItem;
        private ToolStripMenuItem openMovesetToolStripMenuItem;
        private ToolStripMenuItem btnOpenClose;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem saveAsToolStripMenuItem;
        public VIS0Editor vis0Editor;
        public PAT0Editor pat0Editor;
        public SHP0Editor shp0Editor;
        private Panel animEditors;
        private FileSystemWatcher fileSystemWatcher1;
        private ToolStrip toolStrip1;
        private ToolStripButton chkHitboxes;
        private Panel panel2;
        private ToolStripButton chkHurtboxes;
        private ToolStripButton chkBones;
        private ToolStripButton chkPolygons;
        private ToolStripButton chkVertices;
        private ToolStripButton chkFloor;
        private ToolStripButton button1;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripSeparator toolStripSeparator1;
        private ModelPlaybackPanel pnlPlayback;
        private ToolStripMenuItem showPlay;
        public ToolStripMenuItem displayFrameCountDifferencesToolStripMenuItem;
        public ToolStripMenuItem syncLoopToAnimationToolStripMenuItem;
        public ToolStripMenuItem syncObjectsListToVIS0ToolStripMenuItem;
        public ToolStripMenuItem syncTexObjToolStripMenuItem;
        private ToolStripMenuItem displayBRRESRelativeAnimationsToolStripMenuItem;
        private Splitter splitter1;
        public ToolStripMenuItem disableBonesWhenPlayingToolStripMenuItem;
        public ToolStripMenuItem syncAnimationsTogetherToolStripMenuItem;
        private ToolStripMenuItem saveCurrentSettingsToolStripMenuItem;
        public ToolStripMenuItem clearSavedSettingsToolStripMenuItem;
        public ToolStripMenuItem alwaysSyncFrameCountsToolStripMenuItem;
        private Splitter splitter2;
        private Panel panel3;
        private ToolStripMenuItem toolStripMenuItem1;
        private ToolStripMenuItem loadMovesetToolStripMenuItem;
        private ToolStripMenuItem btnLoadMoveset;
        private ToolStripMenuItem btnSaveMoveset;
        private ToolStripMenuItem btnCmnMoveset;
        private ToolStripMenuItem btnLoadCmnMoveset;
        private ToolStripMenuItem btnSaveCmnMoveset;
        private ToolStripMenuItem btnSound;
        private ToolStripMenuItem btnLoadSound;
        private ToolStripMenuItem btnCmnEffects;
        private ToolStripMenuItem btnLoadCmnEffects;
        private ToolStripMenuItem specificEffectsToolStripMenuItem;
        private ToolStripMenuItem btnLoadEffects;
        private Splitter spltMoveset;

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
            this.openModelsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openAnimationsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnOpenClose = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openMovesetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveCurrentSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearSavedSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.redoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.modifyLightingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.syncAnimationsTogetherToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.alwaysSyncFrameCountsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.displayFrameCountDifferencesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.syncLoopToAnimationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.syncTexObjToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.syncObjectsListToVIS0ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.disableBonesWhenPlayingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.kinectToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.editorsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.showBones = new System.Windows.Forms.ToolStripMenuItem();
            this.showAssets = new System.Windows.Forms.ToolStripMenuItem();
            this.showMoveset = new System.Windows.Forms.ToolStripMenuItem();
            this.showAnim = new System.Windows.Forms.ToolStripMenuItem();
            this.showPlay = new System.Windows.Forms.ToolStripMenuItem();
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
            this.fileTypesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.playCHR0ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.playSRT0ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.playSHP0ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.playPAT0ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.playVIS0ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.loadMovesetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnLoadMoveset = new System.Windows.Forms.ToolStripMenuItem();
            this.btnSaveMoveset = new System.Windows.Forms.ToolStripMenuItem();
            this.btnCmnMoveset = new System.Windows.Forms.ToolStripMenuItem();
            this.btnLoadCmnMoveset = new System.Windows.Forms.ToolStripMenuItem();
            this.btnSaveCmnMoveset = new System.Windows.Forms.ToolStripMenuItem();
            this.btnSound = new System.Windows.Forms.ToolStripMenuItem();
            this.btnLoadSound = new System.Windows.Forms.ToolStripMenuItem();
            this.btnCmnEffects = new System.Windows.Forms.ToolStripMenuItem();
            this.btnLoadCmnEffects = new System.Windows.Forms.ToolStripMenuItem();
            this.specificEffectsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnLoadEffects = new System.Windows.Forms.ToolStripMenuItem();
            this.targetModelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hideFromSceneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hideAllOtherModelsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteAllOtherModelsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openModelSwitherToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.displayBRRESRelativeAnimationsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.kinectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.syncKinectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.notYetImplementedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startTrackingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.spltMoveset = new System.Windows.Forms.Splitter();
            this.models = new System.Windows.Forms.ComboBox();
            this.controlPanel = new System.Windows.Forms.Panel();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.chkHitboxes = new System.Windows.Forms.ToolStripButton();
            this.chkHurtboxes = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.chkBones = new System.Windows.Forms.ToolStripButton();
            this.chkPolygons = new System.Windows.Forms.ToolStripButton();
            this.chkVertices = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.chkFloor = new System.Windows.Forms.ToolStripButton();
            this.button1 = new System.Windows.Forms.ToolStripButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.spltAnims = new System.Windows.Forms.Splitter();
            this.panel1 = new System.Windows.Forms.Panel();
            this.KinectPanel = new System.Windows.Forms.TransparentPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.modelPanel1 = new System.Windows.Forms.ModelPanel();
            this.animEditors = new System.Windows.Forms.Panel();
            this.pnlPlayback = new System.Windows.Forms.ModelPlaybackPanel();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.panel3 = new System.Windows.Forms.Panel();
            this.vis0Editor = new System.Windows.Forms.VIS0Editor();
            this.pat0Editor = new System.Windows.Forms.PAT0Editor();
            this.shp0Editor = new System.Windows.Forms.SHP0Editor();
            this.srt0Editor = new System.Windows.Forms.SRT0Editor();
            this.chr0Editor = new System.Windows.Forms.CHR0Editor();
            this.fileSystemWatcher1 = new System.IO.FileSystemWatcher();
            this.pnlMoveset = new System.Windows.Forms.ModelMovesetPanel();
            this.pnlAnim = new System.Windows.Forms.ModelAnimPanel();
            this.pnlAssets = new System.Windows.Forms.ModelAssetPanel();
            this.menuStrip1.SuspendLayout();
            this.controlPanel.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.KinectPanel.SuspendLayout();
            this.animEditors.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).BeginInit();
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
            this.btnAssetToggle.Location = new System.Drawing.Point(249, 24);
            this.btnAssetToggle.Name = "btnAssetToggle";
            this.btnAssetToggle.Size = new System.Drawing.Size(15, 376);
            this.btnAssetToggle.TabIndex = 5;
            this.btnAssetToggle.TabStop = false;
            this.btnAssetToggle.Text = ">";
            this.btnAssetToggle.UseVisualStyleBackColor = false;
            this.btnAssetToggle.Click += new System.EventHandler(this.btnAssetToggle_Click);
            // 
            // btnAnimToggle
            // 
            this.btnAnimToggle.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnAnimToggle.Location = new System.Drawing.Point(499, 24);
            this.btnAnimToggle.Name = "btnAnimToggle";
            this.btnAnimToggle.Size = new System.Drawing.Size(15, 376);
            this.btnAnimToggle.TabIndex = 6;
            this.btnAnimToggle.TabStop = false;
            this.btnAnimToggle.Text = "<";
            this.btnAnimToggle.UseVisualStyleBackColor = false;
            this.btnAnimToggle.Click += new System.EventHandler(this.btnAnimToggle_Click);
            // 
            // btnPlaybackToggle
            // 
            this.btnPlaybackToggle.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnPlaybackToggle.Location = new System.Drawing.Point(264, 385);
            this.btnPlaybackToggle.Name = "btnPlaybackToggle";
            this.btnPlaybackToggle.Size = new System.Drawing.Size(235, 15);
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
            this.spltAssets.Location = new System.Drawing.Point(245, 24);
            this.spltAssets.Name = "spltAssets";
            this.spltAssets.Size = new System.Drawing.Size(4, 376);
            this.spltAssets.TabIndex = 9;
            this.spltAssets.TabStop = false;
            this.spltAssets.Visible = false;
            // 
            // btnOptionToggle
            // 
            this.btnOptionToggle.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnOptionToggle.Location = new System.Drawing.Point(264, 24);
            this.btnOptionToggle.Name = "btnOptionToggle";
            this.btnOptionToggle.Size = new System.Drawing.Size(235, 15);
            this.btnOptionToggle.TabIndex = 11;
            this.btnOptionToggle.TabStop = false;
            this.btnOptionToggle.UseVisualStyleBackColor = false;
            this.btnOptionToggle.Click += new System.EventHandler(this.btnOptionToggle_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.Control;
            this.menuStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.kinectToolStripMenuItem1,
            this.toolStripMenuItem1,
            this.targetModelToolStripMenuItem,
            this.kinectToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(307, 24);
            this.menuStrip1.TabIndex = 13;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newSceneToolStripMenuItem,
            this.openModelsToolStripMenuItem,
            this.openAnimationsToolStripMenuItem,
            this.openMovesetToolStripMenuItem,
            this.saveCurrentSettingsToolStripMenuItem,
            this.clearSavedSettingsToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newSceneToolStripMenuItem
            // 
            this.newSceneToolStripMenuItem.Name = "newSceneToolStripMenuItem";
            this.newSceneToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.newSceneToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.newSceneToolStripMenuItem.Text = "New Scene";
            this.newSceneToolStripMenuItem.Click += new System.EventHandler(this.newSceneToolStripMenuItem_Click);
            // 
            // openModelsToolStripMenuItem
            // 
            this.openModelsToolStripMenuItem.Name = "openModelsToolStripMenuItem";
            this.openModelsToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.openModelsToolStripMenuItem.Text = "Load Models";
            this.openModelsToolStripMenuItem.Click += new System.EventHandler(this.openFileToolStripMenuItem_Click);
            // 
            // openAnimationsToolStripMenuItem
            // 
            this.openAnimationsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnOpenClose,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem});
            this.openAnimationsToolStripMenuItem.Name = "openAnimationsToolStripMenuItem";
            this.openAnimationsToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.openAnimationsToolStripMenuItem.Text = "Animations";
            // 
            // btnOpenClose
            // 
            this.btnOpenClose.Name = "btnOpenClose";
            this.btnOpenClose.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.btnOpenClose.Size = new System.Drawing.Size(186, 22);
            this.btnOpenClose.Text = "Load";
            this.btnOpenClose.Click += new System.EventHandler(this.btnOpenClose_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.saveToolStripMenuItem.Text = "Save ";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
                        | System.Windows.Forms.Keys.S)));
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.saveAsToolStripMenuItem.Text = "Save As";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.btnSaveAs_Click);
            // 
            // openMovesetToolStripMenuItem
            // 
            this.openMovesetToolStripMenuItem.Name = "openMovesetToolStripMenuItem";
            this.openMovesetToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.openMovesetToolStripMenuItem.Text = "Load Moveset";
            this.openMovesetToolStripMenuItem.Visible = false;
            // 
            // saveCurrentSettingsToolStripMenuItem
            // 
            this.saveCurrentSettingsToolStripMenuItem.Name = "saveCurrentSettingsToolStripMenuItem";
            this.saveCurrentSettingsToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.saveCurrentSettingsToolStripMenuItem.Text = "Save Current Settings";
            this.saveCurrentSettingsToolStripMenuItem.Click += new System.EventHandler(this.saveCurrentSettingsToolStripMenuItem_Click);
            // 
            // clearSavedSettingsToolStripMenuItem
            // 
            this.clearSavedSettingsToolStripMenuItem.Name = "clearSavedSettingsToolStripMenuItem";
            this.clearSavedSettingsToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.clearSavedSettingsToolStripMenuItem.Text = "Clear Saved Settings";
            this.clearSavedSettingsToolStripMenuItem.Click += new System.EventHandler(this.clearSavedSettingsToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.undoToolStripMenuItem,
            this.redoToolStripMenuItem,
            this.modifyLightingToolStripMenuItem,
            this.syncAnimationsTogetherToolStripMenuItem,
            this.alwaysSyncFrameCountsToolStripMenuItem,
            this.displayFrameCountDifferencesToolStripMenuItem,
            this.syncLoopToAnimationToolStripMenuItem,
            this.syncTexObjToolStripMenuItem,
            this.syncObjectsListToVIS0ToolStripMenuItem,
            this.disableBonesWhenPlayingToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.editToolStripMenuItem.Text = "Options";
            // 
            // undoToolStripMenuItem
            // 
            this.undoToolStripMenuItem.Enabled = false;
            this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
            this.undoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.undoToolStripMenuItem.Size = new System.Drawing.Size(258, 22);
            this.undoToolStripMenuItem.Text = "Undo";
            this.undoToolStripMenuItem.Click += new System.EventHandler(this.undoToolStripMenuItem_Click);
            // 
            // redoToolStripMenuItem
            // 
            this.redoToolStripMenuItem.Enabled = false;
            this.redoToolStripMenuItem.Name = "redoToolStripMenuItem";
            this.redoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y)));
            this.redoToolStripMenuItem.Size = new System.Drawing.Size(258, 22);
            this.redoToolStripMenuItem.Text = "Redo";
            this.redoToolStripMenuItem.Click += new System.EventHandler(this.redoToolStripMenuItem_Click);
            // 
            // modifyLightingToolStripMenuItem
            // 
            this.modifyLightingToolStripMenuItem.Name = "modifyLightingToolStripMenuItem";
            this.modifyLightingToolStripMenuItem.Size = new System.Drawing.Size(258, 22);
            this.modifyLightingToolStripMenuItem.Text = "Viewer Settings";
            this.modifyLightingToolStripMenuItem.Click += new System.EventHandler(this.modifyLightingToolStripMenuItem_Click);
            // 
            // syncAnimationsTogetherToolStripMenuItem
            // 
            this.syncAnimationsTogetherToolStripMenuItem.Checked = true;
            this.syncAnimationsTogetherToolStripMenuItem.CheckOnClick = true;
            this.syncAnimationsTogetherToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.syncAnimationsTogetherToolStripMenuItem.Name = "syncAnimationsTogetherToolStripMenuItem";
            this.syncAnimationsTogetherToolStripMenuItem.Size = new System.Drawing.Size(258, 22);
            this.syncAnimationsTogetherToolStripMenuItem.Text = "Retrieve corresponding animations";
            this.syncAnimationsTogetherToolStripMenuItem.CheckedChanged += new System.EventHandler(this.syncAnimationsTogetherToolStripMenuItem_CheckedChanged);
            // 
            // alwaysSyncFrameCountsToolStripMenuItem
            // 
            this.alwaysSyncFrameCountsToolStripMenuItem.CheckOnClick = true;
            this.alwaysSyncFrameCountsToolStripMenuItem.Enabled = false;
            this.alwaysSyncFrameCountsToolStripMenuItem.Name = "alwaysSyncFrameCountsToolStripMenuItem";
            this.alwaysSyncFrameCountsToolStripMenuItem.Size = new System.Drawing.Size(258, 22);
            this.alwaysSyncFrameCountsToolStripMenuItem.Text = "Always sync frame counts";
            this.alwaysSyncFrameCountsToolStripMenuItem.Visible = false;
            this.alwaysSyncFrameCountsToolStripMenuItem.CheckedChanged += new System.EventHandler(this.alwaysSyncFrameCountsToolStripMenuItem_CheckedChanged);
            // 
            // displayFrameCountDifferencesToolStripMenuItem
            // 
            this.displayFrameCountDifferencesToolStripMenuItem.CheckOnClick = true;
            this.displayFrameCountDifferencesToolStripMenuItem.Enabled = false;
            this.displayFrameCountDifferencesToolStripMenuItem.Name = "displayFrameCountDifferencesToolStripMenuItem";
            this.displayFrameCountDifferencesToolStripMenuItem.Size = new System.Drawing.Size(258, 22);
            this.displayFrameCountDifferencesToolStripMenuItem.Text = "Warn if frame counts differ";
            this.displayFrameCountDifferencesToolStripMenuItem.Visible = false;
            this.displayFrameCountDifferencesToolStripMenuItem.CheckedChanged += new System.EventHandler(this.displayFrameCountDifferencesToolStripMenuItem_CheckedChanged);
            // 
            // syncLoopToAnimationToolStripMenuItem
            // 
            this.syncLoopToAnimationToolStripMenuItem.CheckOnClick = true;
            this.syncLoopToAnimationToolStripMenuItem.Name = "syncLoopToAnimationToolStripMenuItem";
            this.syncLoopToAnimationToolStripMenuItem.Size = new System.Drawing.Size(258, 22);
            this.syncLoopToAnimationToolStripMenuItem.Text = "Sync loop enable with animation";
            // 
            // syncTexObjToolStripMenuItem
            // 
            this.syncTexObjToolStripMenuItem.CheckOnClick = true;
            this.syncTexObjToolStripMenuItem.Name = "syncTexObjToolStripMenuItem";
            this.syncTexObjToolStripMenuItem.Size = new System.Drawing.Size(258, 22);
            this.syncTexObjToolStripMenuItem.Text = "Sync texture list with object list";
            this.syncTexObjToolStripMenuItem.CheckedChanged += new System.EventHandler(this.syncTexObjToolStripMenuItem_CheckedChanged);
            // 
            // syncObjectsListToVIS0ToolStripMenuItem
            // 
            this.syncObjectsListToVIS0ToolStripMenuItem.CheckOnClick = true;
            this.syncObjectsListToVIS0ToolStripMenuItem.Name = "syncObjectsListToVIS0ToolStripMenuItem";
            this.syncObjectsListToVIS0ToolStripMenuItem.Size = new System.Drawing.Size(258, 22);
            this.syncObjectsListToVIS0ToolStripMenuItem.Text = "Sync objects list edits to VIS0";
            this.syncObjectsListToVIS0ToolStripMenuItem.CheckedChanged += new System.EventHandler(this.syncObjectsListToVIS0ToolStripMenuItem_CheckedChanged);
            // 
            // disableBonesWhenPlayingToolStripMenuItem
            // 
            this.disableBonesWhenPlayingToolStripMenuItem.Checked = true;
            this.disableBonesWhenPlayingToolStripMenuItem.CheckOnClick = true;
            this.disableBonesWhenPlayingToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.disableBonesWhenPlayingToolStripMenuItem.Name = "disableBonesWhenPlayingToolStripMenuItem";
            this.disableBonesWhenPlayingToolStripMenuItem.Size = new System.Drawing.Size(258, 22);
            this.disableBonesWhenPlayingToolStripMenuItem.Text = "Disable bones when playing";
            // 
            // kinectToolStripMenuItem1
            // 
            this.kinectToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editorsToolStripMenuItem,
            this.backColorToolStripMenuItem,
            this.modelToolStripMenuItem,
            this.movesetToolStripMenuItem1,
            this.fileTypesToolStripMenuItem});
            this.kinectToolStripMenuItem1.Name = "kinectToolStripMenuItem1";
            this.kinectToolStripMenuItem1.Size = new System.Drawing.Size(44, 20);
            this.kinectToolStripMenuItem1.Text = "View";
            // 
            // editorsToolStripMenuItem
            // 
            this.editorsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showOptions,
            this.showBones,
            this.showAssets,
            this.showMoveset,
            this.showAnim,
            this.showPlay});
            this.editorsToolStripMenuItem.Name = "editorsToolStripMenuItem";
            this.editorsToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.editorsToolStripMenuItem.Text = "Panels";
            // 
            // showOptions
            // 
            this.showOptions.Name = "showOptions";
            this.showOptions.Size = new System.Drawing.Size(162, 22);
            this.showOptions.Text = "Menu Bar";
            this.showOptions.CheckedChanged += new System.EventHandler(this.showOptions_CheckedChanged);
            this.showOptions.Click += new System.EventHandler(this.showOptions_Click);
            // 
            // showBones
            // 
            this.showBones.Name = "showBones";
            this.showBones.Size = new System.Drawing.Size(162, 22);
            this.showBones.Text = "Bones Panel";
            this.showBones.CheckedChanged += new System.EventHandler(this.showAnim_CheckedChanged);
            this.showBones.Click += new System.EventHandler(this.showAnim_Click);
            // 
            // showAssets
            // 
            this.showAssets.Name = "showAssets";
            this.showAssets.Size = new System.Drawing.Size(162, 22);
            this.showAssets.Text = "Assets Panel";
            this.showAssets.CheckedChanged += new System.EventHandler(this.showAssets_CheckedChanged);
            this.showAssets.Click += new System.EventHandler(this.showAssets_Click);
            // 
            // showMoveset
            // 
            this.showMoveset.Name = "showMoveset";
            this.showMoveset.Size = new System.Drawing.Size(162, 22);
            this.showMoveset.Text = "Moveset Panel";
            this.showMoveset.CheckedChanged += new System.EventHandler(this.showMoveset_CheckedChanged);
            this.showMoveset.Click += new System.EventHandler(this.showMoveset_Click);
            // 
            // showAnim
            // 
            this.showAnim.Name = "showAnim";
            this.showAnim.Size = new System.Drawing.Size(162, 22);
            this.showAnim.Text = "Animation Panel";
            this.showAnim.CheckedChanged += new System.EventHandler(this.showPlay_CheckedChanged);
            this.showAnim.Click += new System.EventHandler(this.showPlay_Click);
            // 
            // showPlay
            // 
            this.showPlay.Checked = true;
            this.showPlay.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showPlay.Name = "showPlay";
            this.showPlay.Size = new System.Drawing.Size(162, 22);
            this.showPlay.Text = "Playback Panel";
            this.showPlay.CheckedChanged += new System.EventHandler(this.showPlay_CheckedChanged_1);
            this.showPlay.Click += new System.EventHandler(this.showPlay_Click_1);
            // 
            // backColorToolStripMenuItem
            // 
            this.backColorToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setColorToolStripMenuItem,
            this.loadImageToolStripMenuItem,
            this.toggleFloor,
            this.resetCameraToolStripMenuItem});
            this.backColorToolStripMenuItem.Name = "backColorToolStripMenuItem";
            this.backColorToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.backColorToolStripMenuItem.Text = "Viewer";
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
            this.toggleFloor.Text = "Floor";
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
            this.modelToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.modelToolStripMenuItem.Text = "Model";
            // 
            // toggleBones
            // 
            this.toggleBones.Checked = true;
            this.toggleBones.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toggleBones.Name = "toggleBones";
            this.toggleBones.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.B)));
            this.toggleBones.Size = new System.Drawing.Size(164, 22);
            this.toggleBones.Text = "Bones";
            this.toggleBones.Click += new System.EventHandler(this.toggleBonesToolStripMenuItem_Click);
            // 
            // togglePolygons
            // 
            this.togglePolygons.Checked = true;
            this.togglePolygons.CheckState = System.Windows.Forms.CheckState.Checked;
            this.togglePolygons.Name = "togglePolygons";
            this.togglePolygons.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
            this.togglePolygons.Size = new System.Drawing.Size(164, 22);
            this.togglePolygons.Text = "Polygons";
            this.togglePolygons.Click += new System.EventHandler(this.togglePolygonsToolStripMenuItem_Click);
            // 
            // toggleVertices
            // 
            this.toggleVertices.Name = "toggleVertices";
            this.toggleVertices.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.toggleVertices.Size = new System.Drawing.Size(164, 22);
            this.toggleVertices.Text = "Vertices";
            this.toggleVertices.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // movesetToolStripMenuItem1
            // 
            this.movesetToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.hitboxesOffToolStripMenuItem,
            this.hurtboxesOffToolStripMenuItem});
            this.movesetToolStripMenuItem1.Name = "movesetToolStripMenuItem1";
            this.movesetToolStripMenuItem1.Size = new System.Drawing.Size(135, 22);
            this.movesetToolStripMenuItem1.Text = "Moveset";
            this.movesetToolStripMenuItem1.Visible = false;
            // 
            // hitboxesOffToolStripMenuItem
            // 
            this.hitboxesOffToolStripMenuItem.Name = "hitboxesOffToolStripMenuItem";
            this.hitboxesOffToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.hitboxesOffToolStripMenuItem.Text = "Hitboxes";
            this.hitboxesOffToolStripMenuItem.Click += new System.EventHandler(this.hitboxesOffToolStripMenuItem_Click);
            // 
            // hurtboxesOffToolStripMenuItem
            // 
            this.hurtboxesOffToolStripMenuItem.Name = "hurtboxesOffToolStripMenuItem";
            this.hurtboxesOffToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.hurtboxesOffToolStripMenuItem.Text = "Hurtboxes";
            this.hurtboxesOffToolStripMenuItem.Click += new System.EventHandler(this.hurtboxesOffToolStripMenuItem_Click);
            // 
            // fileTypesToolStripMenuItem
            // 
            this.fileTypesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.playCHR0ToolStripMenuItem,
            this.playSRT0ToolStripMenuItem,
            this.playSHP0ToolStripMenuItem,
            this.playPAT0ToolStripMenuItem,
            this.playVIS0ToolStripMenuItem});
            this.fileTypesToolStripMenuItem.Name = "fileTypesToolStripMenuItem";
            this.fileTypesToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.fileTypesToolStripMenuItem.Text = "Animations";
            // 
            // playCHR0ToolStripMenuItem
            // 
            this.playCHR0ToolStripMenuItem.Checked = true;
            this.playCHR0ToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.playCHR0ToolStripMenuItem.Name = "playCHR0ToolStripMenuItem";
            this.playCHR0ToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.playCHR0ToolStripMenuItem.Text = "Play CHR0";
            this.playCHR0ToolStripMenuItem.Click += new System.EventHandler(this.playCHR0ToolStripMenuItem_Click);
            // 
            // playSRT0ToolStripMenuItem
            // 
            this.playSRT0ToolStripMenuItem.Checked = true;
            this.playSRT0ToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.playSRT0ToolStripMenuItem.Name = "playSRT0ToolStripMenuItem";
            this.playSRT0ToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.playSRT0ToolStripMenuItem.Text = "Play SRT0";
            this.playSRT0ToolStripMenuItem.Click += new System.EventHandler(this.playSRT0ToolStripMenuItem_Click);
            // 
            // playSHP0ToolStripMenuItem
            // 
            this.playSHP0ToolStripMenuItem.Checked = true;
            this.playSHP0ToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.playSHP0ToolStripMenuItem.Name = "playSHP0ToolStripMenuItem";
            this.playSHP0ToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.playSHP0ToolStripMenuItem.Text = "Play SHP0";
            this.playSHP0ToolStripMenuItem.Click += new System.EventHandler(this.playSHP0ToolStripMenuItem_Click);
            // 
            // playPAT0ToolStripMenuItem
            // 
            this.playPAT0ToolStripMenuItem.Checked = true;
            this.playPAT0ToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.playPAT0ToolStripMenuItem.Name = "playPAT0ToolStripMenuItem";
            this.playPAT0ToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.playPAT0ToolStripMenuItem.Text = "Play PAT0";
            this.playPAT0ToolStripMenuItem.Click += new System.EventHandler(this.playPAT0ToolStripMenuItem_Click);
            // 
            // playVIS0ToolStripMenuItem
            // 
            this.playVIS0ToolStripMenuItem.Checked = true;
            this.playVIS0ToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.playVIS0ToolStripMenuItem.Name = "playVIS0ToolStripMenuItem";
            this.playVIS0ToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.playVIS0ToolStripMenuItem.Text = "Play VIS0";
            this.playVIS0ToolStripMenuItem.Click += new System.EventHandler(this.playVIS0ToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadMovesetToolStripMenuItem,
            this.btnCmnMoveset,
            this.btnSound,
            this.btnCmnEffects,
            this.specificEffectsToolStripMenuItem});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(64, 20);
            this.toolStripMenuItem1.Text = "Moveset";
            // 
            // loadMovesetToolStripMenuItem
            // 
            this.loadMovesetToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnLoadMoveset,
            this.btnSaveMoveset});
            this.loadMovesetToolStripMenuItem.Name = "loadMovesetToolStripMenuItem";
            this.loadMovesetToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.loadMovesetToolStripMenuItem.Text = "Moveset";
            // 
            // btnLoadMoveset
            // 
            this.btnLoadMoveset.Name = "btnLoadMoveset";
            this.btnLoadMoveset.Size = new System.Drawing.Size(100, 22);
            this.btnLoadMoveset.Text = "Load";
            this.btnLoadMoveset.Click += new System.EventHandler(this.btnLoadMoveset_Click);
            // 
            // btnSaveMoveset
            // 
            this.btnSaveMoveset.Name = "btnSaveMoveset";
            this.btnSaveMoveset.Size = new System.Drawing.Size(100, 22);
            this.btnSaveMoveset.Text = "Save";
            this.btnSaveMoveset.Click += new System.EventHandler(this.btnSaveMoveset_Click);
            // 
            // btnCmnMoveset
            // 
            this.btnCmnMoveset.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnLoadCmnMoveset,
            this.btnSaveCmnMoveset});
            this.btnCmnMoveset.Name = "btnCmnMoveset";
            this.btnCmnMoveset.Size = new System.Drawing.Size(173, 22);
            this.btnCmnMoveset.Text = "Common Moveset";
            this.btnCmnMoveset.Visible = false;
            // 
            // btnLoadCmnMoveset
            // 
            this.btnLoadCmnMoveset.Name = "btnLoadCmnMoveset";
            this.btnLoadCmnMoveset.Size = new System.Drawing.Size(100, 22);
            this.btnLoadCmnMoveset.Text = "Load";
            this.btnLoadCmnMoveset.Click += new System.EventHandler(this.btnLoadCmnMoveset_Click);
            // 
            // btnSaveCmnMoveset
            // 
            this.btnSaveCmnMoveset.Name = "btnSaveCmnMoveset";
            this.btnSaveCmnMoveset.Size = new System.Drawing.Size(100, 22);
            this.btnSaveCmnMoveset.Text = "Save";
            this.btnSaveCmnMoveset.Click += new System.EventHandler(this.btnSaveCmnMoveset_Click);
            // 
            // btnSound
            // 
            this.btnSound.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnLoadSound});
            this.btnSound.Name = "btnSound";
            this.btnSound.Size = new System.Drawing.Size(173, 22);
            this.btnSound.Text = "Sound Archive";
            this.btnSound.Visible = false;
            // 
            // btnLoadSound
            // 
            this.btnLoadSound.Name = "btnLoadSound";
            this.btnLoadSound.Size = new System.Drawing.Size(100, 22);
            this.btnLoadSound.Text = "Load";
            this.btnLoadSound.Click += new System.EventHandler(this.btnLoadSound_Click);
            // 
            // btnCmnEffects
            // 
            this.btnCmnEffects.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnLoadCmnEffects});
            this.btnCmnEffects.Name = "btnCmnEffects";
            this.btnCmnEffects.Size = new System.Drawing.Size(173, 22);
            this.btnCmnEffects.Text = "Common Effects";
            this.btnCmnEffects.Visible = false;
            // 
            // btnLoadCmnEffects
            // 
            this.btnLoadCmnEffects.Name = "btnLoadCmnEffects";
            this.btnLoadCmnEffects.Size = new System.Drawing.Size(100, 22);
            this.btnLoadCmnEffects.Text = "Load";
            this.btnLoadCmnEffects.Click += new System.EventHandler(this.btnLoadCmnEffects_Click);
            // 
            // specificEffectsToolStripMenuItem
            // 
            this.specificEffectsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnLoadEffects});
            this.specificEffectsToolStripMenuItem.Name = "specificEffectsToolStripMenuItem";
            this.specificEffectsToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.specificEffectsToolStripMenuItem.Text = "Specific Effects";
            this.specificEffectsToolStripMenuItem.Visible = false;
            // 
            // btnLoadEffects
            // 
            this.btnLoadEffects.Name = "btnLoadEffects";
            this.btnLoadEffects.Size = new System.Drawing.Size(100, 22);
            this.btnLoadEffects.Text = "Load";
            this.btnLoadEffects.Click += new System.EventHandler(this.btnLoadEffects_Click);
            // 
            // targetModelToolStripMenuItem
            // 
            this.targetModelToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.hideFromSceneToolStripMenuItem,
            this.deleteToolStripMenuItem,
            this.hideAllOtherModelsToolStripMenuItem,
            this.deleteAllOtherModelsToolStripMenuItem,
            this.openModelSwitherToolStripMenuItem,
            this.displayBRRESRelativeAnimationsToolStripMenuItem});
            this.targetModelToolStripMenuItem.Name = "targetModelToolStripMenuItem";
            this.targetModelToolStripMenuItem.Size = new System.Drawing.Size(93, 20);
            this.targetModelToolStripMenuItem.Text = "Target Model:";
            // 
            // hideFromSceneToolStripMenuItem
            // 
            this.hideFromSceneToolStripMenuItem.Name = "hideFromSceneToolStripMenuItem";
            this.hideFromSceneToolStripMenuItem.Size = new System.Drawing.Size(277, 22);
            this.hideFromSceneToolStripMenuItem.Text = "Hide from scene";
            this.hideFromSceneToolStripMenuItem.Click += new System.EventHandler(this.hideFromSceneToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(277, 22);
            this.deleteToolStripMenuItem.Text = "Delete from scene";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // hideAllOtherModelsToolStripMenuItem
            // 
            this.hideAllOtherModelsToolStripMenuItem.Name = "hideAllOtherModelsToolStripMenuItem";
            this.hideAllOtherModelsToolStripMenuItem.Size = new System.Drawing.Size(277, 22);
            this.hideAllOtherModelsToolStripMenuItem.Text = "Hide all other models";
            this.hideAllOtherModelsToolStripMenuItem.Click += new System.EventHandler(this.hideAllOtherModelsToolStripMenuItem_Click);
            // 
            // deleteAllOtherModelsToolStripMenuItem
            // 
            this.deleteAllOtherModelsToolStripMenuItem.Name = "deleteAllOtherModelsToolStripMenuItem";
            this.deleteAllOtherModelsToolStripMenuItem.Size = new System.Drawing.Size(277, 22);
            this.deleteAllOtherModelsToolStripMenuItem.Text = "Delete all other models";
            this.deleteAllOtherModelsToolStripMenuItem.Click += new System.EventHandler(this.deleteAllOtherModelsToolStripMenuItem_Click);
            // 
            // openModelSwitherToolStripMenuItem
            // 
            this.openModelSwitherToolStripMenuItem.Name = "openModelSwitherToolStripMenuItem";
            this.openModelSwitherToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.H)));
            this.openModelSwitherToolStripMenuItem.Size = new System.Drawing.Size(277, 22);
            this.openModelSwitherToolStripMenuItem.Text = "Open Model Switcher";
            this.openModelSwitherToolStripMenuItem.Click += new System.EventHandler(this.openModelSwitherToolStripMenuItem_Click);
            // 
            // displayBRRESRelativeAnimationsToolStripMenuItem
            // 
            this.displayBRRESRelativeAnimationsToolStripMenuItem.Name = "displayBRRESRelativeAnimationsToolStripMenuItem";
            this.displayBRRESRelativeAnimationsToolStripMenuItem.Size = new System.Drawing.Size(277, 22);
            this.displayBRRESRelativeAnimationsToolStripMenuItem.Text = "Display BRRES relative animations only";
            this.displayBRRESRelativeAnimationsToolStripMenuItem.CheckStateChanged += new System.EventHandler(this.displayBRRESRelativeAnimationsToolStripMenuItem_CheckStateChanged);
            this.displayBRRESRelativeAnimationsToolStripMenuItem.Click += new System.EventHandler(this.displayBRRESRelativeAnimationsToolStripMenuItem_Click);
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
            // spltMoveset
            // 
            this.spltMoveset.Location = new System.Drawing.Point(138, 24);
            this.spltMoveset.Name = "spltMoveset";
            this.spltMoveset.Size = new System.Drawing.Size(4, 376);
            this.spltMoveset.TabIndex = 18;
            this.spltMoveset.TabStop = false;
            this.spltMoveset.Visible = false;
            // 
            // models
            // 
            this.models.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.models.FormattingEnabled = true;
            this.models.Items.AddRange(new object[] {
            "All"});
            this.models.Location = new System.Drawing.Point(300, 1);
            this.models.Name = "models";
            this.models.Size = new System.Drawing.Size(148, 21);
            this.models.TabIndex = 21;
            this.models.SelectedIndexChanged += new System.EventHandler(this.models_SelectedIndexChanged);
            // 
            // controlPanel
            // 
            this.controlPanel.Controls.Add(this.splitter1);
            this.controlPanel.Controls.Add(this.toolStrip1);
            this.controlPanel.Controls.Add(this.panel2);
            this.controlPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.controlPanel.Location = new System.Drawing.Point(0, 0);
            this.controlPanel.Name = "controlPanel";
            this.controlPanel.Size = new System.Drawing.Size(732, 24);
            this.controlPanel.TabIndex = 22;
            this.controlPanel.Visible = false;
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(448, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 24);
            this.splitter1.TabIndex = 31;
            this.splitter1.TabStop = false;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.chkHitboxes,
            this.chkHurtboxes,
            this.toolStripSeparator2,
            this.chkBones,
            this.chkPolygons,
            this.chkVertices,
            this.toolStripSeparator1,
            this.chkFloor,
            this.button1});
            this.toolStrip1.Location = new System.Drawing.Point(448, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(284, 24);
            this.toolStrip1.TabIndex = 30;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // chkHitboxes
            // 
            this.chkHitboxes.Checked = true;
            this.chkHitboxes.CheckOnClick = true;
            this.chkHitboxes.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkHitboxes.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.chkHitboxes.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.chkHitboxes.Name = "chkHitboxes";
            this.chkHitboxes.Size = new System.Drawing.Size(57, 21);
            this.chkHitboxes.Text = "Hitboxes";
            this.chkHitboxes.Visible = false;
            // 
            // chkHurtboxes
            // 
            this.chkHurtboxes.Checked = true;
            this.chkHurtboxes.CheckOnClick = true;
            this.chkHurtboxes.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkHurtboxes.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.chkHurtboxes.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.chkHurtboxes.Name = "chkHurtboxes";
            this.chkHurtboxes.Size = new System.Drawing.Size(65, 21);
            this.chkHurtboxes.Text = "Hurtboxes";
            this.chkHurtboxes.Visible = false;
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 24);
            this.toolStripSeparator2.Visible = false;
            // 
            // chkBones
            // 
            this.chkBones.Checked = true;
            this.chkBones.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkBones.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.chkBones.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.chkBones.Name = "chkBones";
            this.chkBones.Size = new System.Drawing.Size(43, 21);
            this.chkBones.Text = "Bones";
            this.chkBones.CheckedChanged += new System.EventHandler(this.chkBones_CheckedChanged);
            this.chkBones.Click += new System.EventHandler(this.chkBones_Click);
            // 
            // chkPolygons
            // 
            this.chkPolygons.Checked = true;
            this.chkPolygons.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPolygons.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.chkPolygons.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.chkPolygons.Name = "chkPolygons";
            this.chkPolygons.Size = new System.Drawing.Size(60, 21);
            this.chkPolygons.Text = "Polygons";
            this.chkPolygons.CheckStateChanged += new System.EventHandler(this.chkPolygons_CheckStateChanged);
            this.chkPolygons.Click += new System.EventHandler(this.chkPolygons_Click);
            // 
            // chkVertices
            // 
            this.chkVertices.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.chkVertices.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.chkVertices.Name = "chkVertices";
            this.chkVertices.Size = new System.Drawing.Size(52, 21);
            this.chkVertices.Text = "Vertices";
            this.chkVertices.CheckedChanged += new System.EventHandler(this.checkBox3_CheckedChanged);
            this.chkVertices.Click += new System.EventHandler(this.chkVertices_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 24);
            // 
            // chkFloor
            // 
            this.chkFloor.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.chkFloor.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.chkFloor.Name = "chkFloor";
            this.chkFloor.Size = new System.Drawing.Size(38, 21);
            this.chkFloor.Text = "Floor";
            this.chkFloor.CheckedChanged += new System.EventHandler(this.chkFloor_CheckedChanged);
            this.chkFloor.Click += new System.EventHandler(this.chkFloor_Click);
            // 
            // button1
            // 
            this.button1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.button1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(83, 19);
            this.button1.Text = "Reset Camera";
            this.button1.Click += new System.EventHandler(this.resetCameraToolStripMenuItem_Click_1);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.models);
            this.panel2.Controls.Add(this.menuStrip1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(448, 24);
            this.panel2.TabIndex = 29;
            // 
            // spltAnims
            // 
            this.spltAnims.Dock = System.Windows.Forms.DockStyle.Right;
            this.spltAnims.Location = new System.Drawing.Point(514, 24);
            this.spltAnims.Name = "spltAnims";
            this.spltAnims.Size = new System.Drawing.Size(4, 376);
            this.spltAnims.TabIndex = 23;
            this.spltAnims.TabStop = false;
            this.spltAnims.Visible = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.KinectPanel);
            this.panel1.Controls.Add(this.modelPanel1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(264, 39);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(235, 346);
            this.panel1.TabIndex = 25;
            // 
            // KinectPanel
            // 
            this.KinectPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.KinectPanel.BackColor = System.Drawing.Color.Transparent;
            this.KinectPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.KinectPanel.Controls.Add(this.label1);
            this.KinectPanel.Location = new System.Drawing.Point(65, 0);
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
            this.modelPanel1.Location = new System.Drawing.Point(0, 0);
            this.modelPanel1.Name = "modelPanel1";
            this.modelPanel1.RotationScale = 0.4F;
            this.modelPanel1.Size = new System.Drawing.Size(235, 346);
            this.modelPanel1.TabIndex = 0;
            this.modelPanel1.TranslationScale = 0.05F;
            this.modelPanel1.ZoomScale = 2.5F;
            this.modelPanel1.PreRender += new System.Windows.Forms.GLRenderEventHandler(this.modelPanel1_PreRender);
            this.modelPanel1.PostRender += new System.Windows.Forms.GLRenderEventHandler(this.modelPanel1_PostRender);
            this.modelPanel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.modelPanel1_MouseDown);
            this.modelPanel1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.modelPanel1_MouseMove);
            this.modelPanel1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.modelPanel1_MouseUp);
            // 
            // animEditors
            // 
            this.animEditors.AutoScroll = true;
            this.animEditors.Controls.Add(this.pnlPlayback);
            this.animEditors.Controls.Add(this.splitter2);
            this.animEditors.Controls.Add(this.panel3);
            this.animEditors.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.animEditors.Location = new System.Drawing.Point(0, 400);
            this.animEditors.Name = "animEditors";
            this.animEditors.Size = new System.Drawing.Size(732, 60);
            this.animEditors.TabIndex = 29;
            this.animEditors.Visible = false;
            // 
            // pnlPlayback
            // 
            this.pnlPlayback.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlPlayback.Enabled = false;
            this.pnlPlayback.Location = new System.Drawing.Point(267, 0);
            this.pnlPlayback.MinimumSize = new System.Drawing.Size(290, 54);
            this.pnlPlayback.Name = "pnlPlayback";
            this.pnlPlayback.Size = new System.Drawing.Size(465, 60);
            this.pnlPlayback.TabIndex = 15;
            this.pnlPlayback.Resize += new System.EventHandler(this.pnlPlayback_Resize);
            // 
            // splitter2
            // 
            this.splitter2.Location = new System.Drawing.Point(264, 0);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(3, 60);
            this.splitter2.TabIndex = 0;
            this.splitter2.TabStop = false;
            // 
            // panel3
            // 
            this.panel3.AutoScroll = true;
            this.panel3.Controls.Add(this.vis0Editor);
            this.panel3.Controls.Add(this.pat0Editor);
            this.panel3.Controls.Add(this.shp0Editor);
            this.panel3.Controls.Add(this.srt0Editor);
            this.panel3.Controls.Add(this.chr0Editor);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(264, 60);
            this.panel3.TabIndex = 29;
            // 
            // vis0Editor
            // 
            this.vis0Editor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vis0Editor.Location = new System.Drawing.Point(0, 0);
            this.vis0Editor.Name = "vis0Editor";
            this.vis0Editor.Padding = new System.Windows.Forms.Padding(4);
            this.vis0Editor.Size = new System.Drawing.Size(264, 60);
            this.vis0Editor.TabIndex = 26;
            this.vis0Editor.Visible = false;
            // 
            // pat0Editor
            // 
            this.pat0Editor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pat0Editor.Location = new System.Drawing.Point(0, 0);
            this.pat0Editor.Name = "pat0Editor";
            this.pat0Editor.Size = new System.Drawing.Size(264, 60);
            this.pat0Editor.TabIndex = 27;
            this.pat0Editor.Visible = false;
            // 
            // shp0Editor
            // 
            this.shp0Editor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.shp0Editor.Location = new System.Drawing.Point(0, 0);
            this.shp0Editor.Name = "shp0Editor";
            this.shp0Editor.Size = new System.Drawing.Size(264, 60);
            this.shp0Editor.TabIndex = 28;
            this.shp0Editor.Visible = false;
            // 
            // srt0Editor
            // 
            this.srt0Editor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.srt0Editor.Location = new System.Drawing.Point(0, 0);
            this.srt0Editor.Name = "srt0Editor";
            this.srt0Editor.Size = new System.Drawing.Size(264, 60);
            this.srt0Editor.TabIndex = 20;
            this.srt0Editor.Visible = false;
            // 
            // chr0Editor
            // 
            this.chr0Editor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chr0Editor.Location = new System.Drawing.Point(0, 0);
            this.chr0Editor.Name = "chr0Editor";
            this.chr0Editor.Size = new System.Drawing.Size(264, 60);
            this.chr0Editor.TabIndex = 19;
            this.chr0Editor.Visible = false;
            this.chr0Editor.VisibleChanged += new System.EventHandler(this.chr0Editor_VisibleChanged);
            // 
            // fileSystemWatcher1
            // 
            this.fileSystemWatcher1.EnableRaisingEvents = true;
            this.fileSystemWatcher1.SynchronizingObject = this;
            // 
            // pnlMoveset
            // 
            this.pnlMoveset.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pnlMoveset.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlMoveset.Location = new System.Drawing.Point(518, 24);
            this.pnlMoveset.Name = "pnlMoveset";
            this.pnlMoveset.Size = new System.Drawing.Size(214, 376);
            this.pnlMoveset.TabIndex = 17;
            this.pnlMoveset.Visible = false;
            this.pnlMoveset.FileChanged += new System.EventHandler(this.FileChanged);
            // 
            // pnlAnim
            // 
            this.pnlAnim.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlAnim.Location = new System.Drawing.Point(142, 24);
            this.pnlAnim.Name = "pnlAnim";
            this.pnlAnim.Size = new System.Drawing.Size(103, 376);
            this.pnlAnim.TabIndex = 20;
            this.pnlAnim.Visible = false;
            // 
            // pnlAssets
            // 
            this.pnlAssets.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlAssets.Location = new System.Drawing.Point(0, 24);
            this.pnlAssets.Name = "pnlAssets";
            this.pnlAssets.Size = new System.Drawing.Size(138, 376);
            this.pnlAssets.TabIndex = 4;
            this.pnlAssets.Visible = false;
            this.pnlAssets.Key += new System.EventHandler(this.Key);
            this.pnlAssets.Unkey += new System.EventHandler(this.Unkey);
            // 
            // ModelEditControl
            // 
            this.AllowDrop = true;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnOptionToggle);
            this.Controls.Add(this.btnPlaybackToggle);
            this.Controls.Add(this.btnAnimToggle);
            this.Controls.Add(this.spltAnims);
            this.Controls.Add(this.pnlMoveset);
            this.Controls.Add(this.btnAssetToggle);
            this.Controls.Add(this.spltAssets);
            this.Controls.Add(this.pnlAnim);
            this.Controls.Add(this.spltMoveset);
            this.Controls.Add(this.pnlAssets);
            this.Controls.Add(this.controlPanel);
            this.Controls.Add(this.animEditors);
            this.Name = "ModelEditControl";
            this.Size = new System.Drawing.Size(732, 460);
            this.SizeChanged += new System.EventHandler(this.ModelEditControl_SizeChanged);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.ModelEditControl_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.ModelEditControl_DragEnter);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.controlPanel.ResumeLayout(false);
            this.controlPanel.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.KinectPanel.ResumeLayout(false);
            this.KinectPanel.PerformLayout();
            this.animEditors.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        public ModelEditControl()
        {
            InitializeComponent();
            pnlAssets._mainWindow =
            pnlAnim._mainWindow =
            pnlMoveset._mainWindow =
            pnlPlayback._mainWindow =
            chr0Editor._mainWindow =
            srt0Editor._mainWindow =
            shp0Editor._mainWindow =
            pat0Editor._mainWindow =
            vis0Editor._mainWindow = this;

            animEditors.HorizontalScroll.Enabled = addedHeight = (!(animEditors.Width - panel3.Width - splitter2.Width >= pnlPlayback.MinimumSize.Width));
            if (pnlPlayback.Width <= pnlPlayback.MinimumSize.Width)
            {
                pnlPlayback.Dock = DockStyle.Left;
                pnlPlayback.Width = pnlPlayback.MinimumSize.Width;
            }
            else
                pnlPlayback.Dock = DockStyle.Fill;

            pnlAssets.fileType.SelectedIndex = 0;

            m_DelegateOpenFile = new DelegateOpenFile(this.OpenFile);
        }

        #region Variables
        private const float _orbRadius = 1.0f;
        private const float _circRadius = 1.2f;
        private const float _axisLength = 2.0f;
        private const float _axisSnapRange = 7.0f;
        private const float _selectRange = 0.03f; //Selection error range for orb and circ
        private const float _selectOrbScale = _selectRange / _orbRadius;
        private const float _circOrbScale = _circRadius / _orbRadius;
        private const float _axisScale = _axisLength / _orbRadius;

        public List<SaveState2> undoSaves = new List<SaveState2>();
        public List<SaveState2> redoSaves = new List<SaveState2>();
        public int saveIndex = 0;
        public bool firstUndo = false;

        public event EventHandler TargetModelChanged;

        private delegate void DelegateOpenFile(String s);
        private DelegateOpenFile m_DelegateOpenFile;

        public int _animFrame = 0, _maxFrame;
        public bool _updating, _loop;
        
        public CHR0Node _chr0;
        public SRT0Node _srt0;
        public SHP0Node _shp0;
        public SCN0Node _scn0;
        public PAT0Node _pat0;
        public VIS0Node _vis0;

        public bool _rotating, _translating, _scaling;
        private Vector3 _lastPoint;
        private Vector3 _oldAngles, _oldPosition, _oldScale;
        private bool _snapX, _snapY, _snapZ, _snapCirc, _snapXY, _snapYZ, _snapXZ;
        
        public List<MDL0Node> _targetModels = new List<MDL0Node>();
        private MDL0Node _targetModel;

        public Color _clearColor;
        public Image _bgImage;
        public MDL0BoneNode _targetBone = null;
        public MDL0MaterialRefNode _targetTexRef = null;
        public bool _enableTransform = true;
        
        public bool _renderFloor, _renderBones = true, _renderVertices, _renderHurtboxes, _renderHitboxes;
        public CheckState _renderPolygons = CheckState.Checked;

        public ResourceNode GetSelectedBRRESFile(int type)
        {
            switch (type)
            {
                case 0: return SelectedCHR0;
                case 1: return SelectedSRT0;
                case 2: return SelectedSHP0;
                case 3: return SelectedPAT0;
                case 4: return SelectedVIS0;
                case 5: return SelectedSCN0;
                default: return null;
            }
        }
        public void SetSelectedBRRESFile(int type, ResourceNode value)
        {
            switch (type)
            {
                case 0: SelectedCHR0 = value as CHR0Node; break;
                case 1: SelectedSRT0 = value as SRT0Node; break;
                case 2: SelectedSHP0 = value as SHP0Node; break;
                case 3: SelectedPAT0 = value as PAT0Node; break;
                case 4: SelectedVIS0 = value as VIS0Node; break;
                case 5: SelectedSCN0 = value as SCN0Node; break;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MDL0Node TargetModel { get { return _targetModel; } set { ModelChanged(value); } }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public CHR0Node SelectedCHR0
        { 
            get { return _chr0; } 
            set 
            {
                _chr0 = value;

                if (_updating)
                    return;

                AnimChanged(0);
                UpdatePropDisplay();
                //pnlAnim.bgLayer.Invalidate();
            } 
        }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SRT0Node SelectedSRT0 
        { 
            get { return _srt0; } 
            set 
            { 
                _srt0 = value;

                if (_updating)
                    return;

                AnimChanged(1);
                UpdatePropDisplay();
                //pnlAnim.bgLayer.Invalidate();
            } 
        }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SHP0Node SelectedSHP0
        {
            get { return _shp0; }
            set
            {
                _shp0 = value;

                if (_updating)
                    return;

                AnimChanged(2);
                UpdatePropDisplay();
                //pnlAnim.bgLayer.Invalidate(); 
            }
        }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public PAT0Node SelectedPAT0
        {
            get { return _pat0; }
            set
            {
                _pat0 = value; 
                
                if (_updating)
                    return;

                AnimChanged(3);
                UpdatePropDisplay();
            }
        }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public VIS0Node SelectedVIS0
        {
            get { return _vis0; }
            set
            {
                _vis0 = value; 
                
                if (_updating)
                    return;

                AnimChanged(4);
                UpdatePropDisplay();
            }
        }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SCN0Node SelectedSCN0
        {
            get { return _scn0; }
            set
            {
                _scn0 = value;

                if (_updating)
                    return;

                AnimChanged(5);
                UpdatePropDisplay();
            }
        }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Color ClearColor { get { return _clearColor; } set { _clearColor = value; } }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Image BGImage { get { return _bgImage; } set { _bgImage = value; } }
        
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MDL0BoneNode TargetBone { get { return _targetBone; } set { _targetBone = value; UpdatePropDisplay(); } }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MDL0MaterialRefNode TargetTexRef { get { return _targetTexRef; } set { _targetTexRef = value; UpdatePropDisplay(); } }
        
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int CurrentFrame { get { return _animFrame; } set { _animFrame = value; UpdateModel(); } }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool EnableTransformEdit
        { 
            get 
            { 
                return _enableTransform; 
            } 
            set 
            {
                if (_enableTransform == value)
                    return;

                _enableTransform = value;
                chr0Editor.EnableTransformEdit = 
                srt0Editor.EnableTransformEdit =
                shp0Editor.EnableTransformEdit =
                vis0Editor.EnableTransformEdit =
                pat0Editor.EnableTransformEdit = value; 
            } 
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool RenderFloor 
        { 
            get { return _renderFloor; } 
            set 
            { 
                _renderFloor = value;
                _updating = true;
                chkFloor.Checked = toggleFloor.Checked = _renderFloor;
                _updating = false;
                modelPanel1.Invalidate();
            } 
        }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool RenderBones
        {
            get { return _renderBones; }
            set
            {
                if (TargetModel != null)
                    TargetModel._renderBones = value;

                _renderBones = value;
                _updating = true;
                chkBones.Checked = toggleBones.Checked = _renderBones;
                _updating = false;
                modelPanel1.Invalidate();
            }
        }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool RenderVertices
        {
            get { return _renderVertices; }
            set
            {
                if (TargetModel != null)
                    TargetModel._renderVertices = value;

                _renderVertices = value;
                _updating = true;
                chkVertices.Checked = toggleVertices.Checked = _renderVertices;
                _updating = false;
                modelPanel1.Invalidate();
            }
        }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public CheckState RenderPolygons
        {
            get { return _renderPolygons; }
            set
            {
                if (TargetModel != null)
                {
                    TargetModel._renderPolygons = value == CheckState.Checked || value == CheckState.Indeterminate ? true : false;
                    TargetModel._renderPolygonsWireframe = value == CheckState.Indeterminate ? true : false;
                }

                _renderPolygons = value;
                _updating = true;
                chkPolygons.CheckState = togglePolygons.CheckState = _renderPolygons;
                _updating = false;
                modelPanel1.Invalidate();
            }
        }

        #endregion

        #region File Handling
        public ResourceNode _externalAnimationsNode;
        private SaveFileDialog dlgSave = new SaveFileDialog();
        private OpenFileDialog dlgOpen = new OpenFileDialog();
        private bool LoadExternal()
        {
            dlgOpen.Filter = "All Compatible Files (*.pac, *.pcs, *.brres, *.chr0, *.mrg)|*.pac;*.pcs;*.brres;*.chr0;*.mrg";
            if (dlgOpen.ShowDialog() == DialogResult.OK)
            {
                ResourceNode node = null;
                pnlAssets.listAnims.BeginUpdate();
                try
                {
                    if ((node = NodeFactory.FromFile(null, dlgOpen.FileName)) != null)
                    {
                        if (!CloseExternal())
                            return false;

                        if (!pnlAssets.LoadAnims(node, pnlAssets.fileType.SelectedIndex))
                            MessageBox.Show(this, "No animations could be found in external file.", "Error");
                        else
                        {
                            _externalAnimationsNode = node;
                            node = null;
                            //txtExtPath.Text = Path.GetFileName(dlgOpen.FileName);

                            modelPanel1.AddReference(_externalAnimationsNode);

                            return true;
                        }
                    }
                    else
                        MessageBox.Show(this, "Unable to recognize input file.");
                }
                catch (Exception x) { MessageBox.Show(this, x.ToString()); }
                finally
                {
                    if (node != null)
                        node.Dispose();
                    pnlAssets.listAnims.EndUpdate();
                }
            }
            return false;
        }
        private bool CloseExternal()
        {
            if (_externalAnimationsNode != null)
            {
                if (_externalAnimationsNode.IsDirty)
                {
                    DialogResult res = MessageBox.Show(this, "You have made changes to an external file. Would you like to save those changes?", "Closing external file.", MessageBoxButtons.YesNoCancel);
                    if (((res == DialogResult.Yes) && (!SaveExternal(false))) || (res == DialogResult.Cancel))
                        return false;
                }

                modelPanel1.RemoveReference(_externalAnimationsNode);
                pnlAssets.closing = true;
                pnlAssets.listAnims.Items.Clear();
                pnlAssets.closing = false;
                _externalAnimationsNode.Dispose();
                _externalAnimationsNode = null;

                pnlAssets.UpdateAnimations(pnlAssets.fileType.SelectedIndex);
                SetSelectedBRRESFile(pnlAssets.fileType.SelectedIndex, null);
                GetFiles(-1);
                UpdatePropDisplay();
                UpdateModel();

                //txtExtPath.Text = "";
            }
            return true;
        }
        private bool SaveExternal(bool As)
        {
            if ((_externalAnimationsNode == null) || ((!_externalAnimationsNode.IsDirty) && !As))
                return true;

            try
            {
                if (As)
                {
                    FolderBrowserDialog dialog = new FolderBrowserDialog();
                    dialog.Description = "Please choose a location to save this file.";
                    dialog.SelectedPath = _externalAnimationsNode._origPath;
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        _externalAnimationsNode.Merge();
                        _externalAnimationsNode.Export(dialog.SelectedPath + "\\" + Path.GetFileName(_externalAnimationsNode._origPath));
                    }
                }
                else
                {
                    _externalAnimationsNode.Merge();
                    _externalAnimationsNode.Export(_externalAnimationsNode._origPath);
                }
                return true;
            }
            catch (Exception x) { MessageBox.Show(this, x.ToString()); }
            return false;
        }

        public void btnOpenClose_Click(object sender, EventArgs e)
        {
            if (btnOpenClose.Text == "Load")
            {
                if (LoadExternal())
                    btnOpenClose.Text = pnlAssets.Load.Text = "Close";
            }
            else if (btnOpenClose.Text == "Close")
            {
                if (CloseExternal())
                    btnOpenClose.Text = pnlAssets.Load.Text = "Load";
            }
        }
        public void btnSave_Click(object sender, EventArgs e) { SaveExternal(false); }
        private void btnSaveAs_Click(object sender, EventArgs e) { SaveExternal(true); }
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

                    models.SelectedItem = TargetModel;
                }
            }
            catch (Exception ex) { MessageBox.Show(this, ex.Message, "Error loading model(s) from file."); }
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
            if (!_targetModels.Contains(model))
                _targetModels.Add(model);
            if (!models.Items.Contains(model))
                models.Items.Add(model);
            modelPanel1.AddTarget(model);
            model.ApplyCHR(null, 0);
        }

        public bool CloseFiles() { return CloseExternal() && pnlMoveset.CloseReferences(); }

        public bool resetcam = true;
        public bool hide = false;
        private void ModelChanged(MDL0Node model)
        {
            if (model != null && !_targetModels.Contains(model))
                _targetModels.Add(model);

            if (model == null)
                modelPanel1.RemoveTarget(_targetModel);

            if ((_targetModel = model) != null)
            {
                modelPanel1.AddTarget(_targetModel);
                pnlAssets.VIS0Indices = _targetModel.VIS0Indices;
            }

            if (resetcam)
            {
                modelPanel1.ResetCamera();
                SetFrame(0);
            }
            else
                resetcam = true;

            pnlAssets.Reset();
            pnlAnim.Reset();

            if (TargetModelChanged != null)
                TargetModelChanged(this, null);

            _updating = true;
            if (_targetModel != null && !_editingAll)
                models.SelectedItem = _targetModel;
            _updating = false;

            //pnlAnim.bgLayer.Invalidate();
        }
        #endregion

        #region Animations
        private Control _currentControl = null;
        public void setCurrentControl()
        {
            Control newControl = null;
            switch (pnlAssets.fileType.SelectedIndex)
            {
                case 0:
                    newControl = chr0Editor;
                    break;
                case 1:
                    newControl = srt0Editor;
                    syncTexObjToolStripMenuItem.Checked = true;
                    break;
                case 2:
                    newControl = shp0Editor;
                    break;
                case 3:
                    newControl = pat0Editor;
                    syncTexObjToolStripMenuItem.Checked = true;
                    break;
                case 4:
                    newControl = vis0Editor;
                    break;
                //case 5:
                //    newControl = scn0Editor;
                //    break;
            }
            if (_currentControl != newControl)
            {
                //bool visible = showPlay.Checked;
                if (_currentControl != null)
                {
                    //visible = _currentControl.Visible;
                    _currentControl.Visible = false;
                }
                _currentControl = newControl;
                if (!(_currentControl is SRT0Editor) && !(_currentControl is PAT0Editor))
                    syncTexObjToolStripMenuItem.Checked = false;
                if (_currentControl != null)
                {
                    //_currentControl.Visible = visible;
                    _currentControl.Visible = true;
                    if (_currentControl is CHR0Editor)
                    {
                        animEditors.Height = 
                        panel3.Height = 82;
                        panel3.Width = 732;
                    }
                    else if (_currentControl is SRT0Editor)
                    {
                        animEditors.Height =
                        panel3.Height = 82;
                        panel3.Width = 561;
                    }
                    else if (_currentControl is SHP0Editor)
                    {
                        animEditors.Height =
                        panel3.Height = 106;
                        panel3.Width = 533;
                    }
                    else if (_currentControl is PAT0Editor)
                    {
                        animEditors.Height =
                        panel3.Height = 77;
                        panel3.Width = 402;
                    }
                    else if (_currentControl is VIS0Editor)
                    {
                        animEditors.Height =
                        panel3.Height = 112;
                        panel3.Width = 507;
                    }
                    else
                        animEditors.Height = panel3.Width = 0;
                }
                else animEditors.Height = panel3.Width = 0;
                return;
            }
            checkDimensions();
            UpdatePropDisplay();
        }
        public void UpdatePropDisplay()
        {
            if (animEditors.Height == 0 || animEditors.Visible == false)
                return;

            if (chr0Editor.Visible)
            {
                chr0Editor.UpdatePropDisplay();
                chr0Editor.btnInsert.Enabled =
                chr0Editor.btnDelete.Enabled =
                chr0Editor.btnClear.Enabled = SelectedCHR0 == null ? false : true;
            }

            if (srt0Editor.Visible)
            {
                srt0Editor.UpdatePropDisplay();
                srt0Editor.btnInsert.Enabled =
                srt0Editor.btnDelete.Enabled =
                srt0Editor.btnClear.Enabled = SelectedCHR0 == null ? false : true;
            }

            if (vis0Editor.Visible)
            {
                if (vis0Editor.visEditor1.TargetNode != null && !vis0Editor.visEditor1.TargetNode.Flags.HasFlag(VIS0Flags.Constant))
                {
                    vis0Editor.visEditor1._updating = true;
                    vis0Editor.visEditor1.listBox1.SelectedIndices.Clear();
                    vis0Editor.visEditor1.listBox1.SelectedIndex = CurrentFrame - 1;
                    vis0Editor.visEditor1._updating = false;
                }
            }

            if (shp0Editor.Visible)
                shp0Editor.UpdatePropDisplay();

            if (pat0Editor.Visible)
                pat0Editor.UpdatePropDisplay();
        }

        public bool _editingAll { get { return (!(models.SelectedItem is MDL0Node) && models.SelectedItem != null && models.SelectedItem.ToString() == "All"); } }
        public void UpdateModel()
        {
            if (_updating || models == null)
                return;

            if (!_editingAll)
            {
                if (TargetModel != null)
                    UpdateModel(TargetModel);
            }
            else
                foreach (MDL0Node n in _targetModels)
                    UpdateModel(n);

            if (!_playing) 
                UpdatePropDisplay();
            modelPanel1.Invalidate();
        }

        private void UpdateModel(MDL0Node model)
        {
            if (_chr0 != null && !(pnlAssets.fileType.SelectedIndex != 0 && !playCHR0ToolStripMenuItem.Checked))
                model.ApplyCHR(_chr0, _animFrame);
            else
                model.ApplyCHR(null, 0);
            if (_srt0 != null && !(pnlAssets.fileType.SelectedIndex != 1 && !playSRT0ToolStripMenuItem.Checked))
                model.ApplySRT(_srt0, _animFrame);
            else
                model.ApplySRT(null, 0);
            if (_shp0 != null && !(pnlAssets.fileType.SelectedIndex != 2 && !playSHP0ToolStripMenuItem.Checked))
                model.ApplySHP(_shp0, _animFrame);
            else
                model.ApplySHP(null, 0);
            if (_pat0 != null && !(pnlAssets.fileType.SelectedIndex != 3 && !playPAT0ToolStripMenuItem.Checked))
                model.ApplyPAT(_pat0, _animFrame);
            else
                model.ApplyPAT(null, 0);
            if (_vis0 != null && !(pnlAssets.fileType.SelectedIndex != 4 && !playVIS0ToolStripMenuItem.Checked))
            {
                if (model == TargetModel)
                    ReadVIS0();
                else
                    model.ApplyVIS(_vis0, _animFrame);
            }
        }
        public void AnimChanged(int type)
        {
            if (type == 1)
                pnlAssets.UpdateSRT0Selection(SelectedSRT0);
            else
                pnlAssets.UpdateSRT0Selection(null);
            if (type == 2)
                shp0Editor.UpdateSHP0Indices();
            if (type == 3)
            {
                pat0Editor.UpdateBoxes();
                pnlAssets.UpdatePAT0Selection(SelectedPAT0);
            }
            else
                pnlAssets.UpdatePAT0Selection(null);
            if (type == 4)
                vis0Editor.UpdateAnimation();

            if (GetSelectedBRRESFile(type) == null)
            {
                pnlPlayback.numFrameIndex.Maximum = _maxFrame = 0;
                pnlPlayback.numTotalFrames.Minimum = 0;
                _updating = true;
                pnlPlayback.numTotalFrames.Value = 0;
                _updating = false;
                pnlPlayback.btnPlay.Enabled =
                pnlPlayback.numTotalFrames.Enabled =
                pnlPlayback.numFrameIndex.Enabled = false;
                pnlPlayback.btnLast.Enabled = false;
                pnlPlayback.btnFirst.Enabled = false;
                pnlPlayback.Enabled = false;
                SetFrame(0);
            }
            else
            {
                int oldMax = _maxFrame;

                _maxFrame = ((BRESEntryNode)GetSelectedBRRESFile(type)).tFrameCount;

                _updating = true;
                pnlPlayback.btnPlay.Enabled =
                pnlPlayback.numFrameIndex.Enabled =
                pnlPlayback.numTotalFrames.Enabled = true;
                pnlPlayback.Enabled = true;
                pnlPlayback.numTotalFrames.Value = _maxFrame;
                if (syncLoopToAnimationToolStripMenuItem.Checked)
                    pnlPlayback.chkLoop.Checked = ((BRESEntryNode)GetSelectedBRRESFile(type)).tLoop;
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
        }

        public bool _playing = false;
        public void SetFrame(int index)
        {
            //if (_animFrame == index)
            //    return;

            if (index > _maxFrame)
                return;

            CurrentFrame = TargetModel == null ? 0 : index;

            pnlPlayback.btnNextFrame.Enabled = _animFrame < _maxFrame;
            pnlPlayback.btnPrevFrame.Enabled = _animFrame > 0;

            pnlPlayback.btnLast.Enabled = _animFrame != _maxFrame;
            pnlPlayback.btnFirst.Enabled = _animFrame > 1;

            if (_animFrame <= pnlPlayback.numFrameIndex.Maximum)
                pnlPlayback.numFrameIndex.Value = _animFrame;

            //Handled by CurrentFrame
            //modelPanel1.Invalidate();
            //if (!_playing) UpdatePropDisplay();
        }
        private bool wasOff = false;
        public bool runningAction = false;
        public void PlayAnim()
        {
            if (GetSelectedBRRESFile(pnlAssets.fileType.SelectedIndex) == null || _maxFrame == 1)
                return;

            _playing = true;

            if (disableBonesWhenPlayingToolStripMenuItem.Checked)
            {
                if (RenderBones == false)
                    wasOff = true;

                RenderBones = false;
                toggleBones.Checked = false;
            }

            EnableTransformEdit = false;

            if (_animFrame >= _maxFrame) //Reset anim
                SetFrame(1);

            if (_animFrame < _maxFrame)
            {
                animTimer.Start();
                pnlPlayback.btnPlay.Text = "Stop";
            }
            else
            {
                if (disableBonesWhenPlayingToolStripMenuItem.Checked)
                    RenderBones = true;
                _playing = false;
            }
        }
        public void StopAnim()
        {
            animTimer.Stop();

            _playing = false;

            if (disableBonesWhenPlayingToolStripMenuItem.Checked)
            {
                if (!wasOff)
                    RenderBones = true;

                wasOff = false;
            }

            pnlPlayback.btnPlay.Text = "Play";
            EnableTransformEdit = true;
            UpdatePropDisplay();
        }
        private void animTimer_Tick(object sender, EventArgs e)
        {
            if (GetSelectedBRRESFile(pnlAssets.fileType.SelectedIndex) == null)
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
        private unsafe void saveCurrentSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool maximize = false;
            if (MessageBox.Show("When the viewer is opened, do you want it to automatically maximize?", "Maximize Viewer?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                maximize = true;
            using (FileStream stream = new FileStream(Application.StartupPath + "/brawlbox.settings", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite, 8, FileOptions.SequentialScan))
            {
                stream.SetLength((long)BBVS.Size);
                using (FileMap map = FileMap.FromStream(stream))
                {
                    BBVS* settings = (BBVS*)map.Address;
                    settings->_tag = BBVS.Tag;
                    settings->_version = 1;
                    settings->defaultCam = modelPanel1._defaultTranslate;
                    settings->amb = modelPanel1._ambient;
                    settings->pos = modelPanel1._position;
                    settings->diff = modelPanel1._diffuse;
                    settings->spec = modelPanel1._specular;
                    settings->yFov = modelPanel1._fovY;
                    settings->_nearZ = modelPanel1._nearZ;
                    settings->_farz = modelPanel1._farZ;
                    settings->tScale = modelPanel1.TranslationScale;
                    settings->rScale = modelPanel1.RotationScale;
                    settings->zScale = modelPanel1.ZoomScale;
                    settings->SetOptions(syncAnimationsTogetherToolStripMenuItem.Checked,
                        false,//displayFrameCountDifferencesToolStripMenuItem.Checked,
                        syncLoopToAnimationToolStripMenuItem.Checked,
                        syncTexObjToolStripMenuItem.Checked,
                        syncObjectsListToVIS0ToolStripMenuItem.Checked,
                        disableBonesWhenPlayingToolStripMenuItem.Checked, maximize, false//alwaysSyncFrameCountsToolStripMenuItem.Checked
                        );
                }
            }
            clearSavedSettingsToolStripMenuItem.Enabled = File.Exists(Application.StartupPath + "/brawlbox.settings");
        }

        private void alwaysSyncFrameCountsToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (!_updating && alwaysSyncFrameCountsToolStripMenuItem.Checked == true)
            {
                _updating = true;
                displayFrameCountDifferencesToolStripMenuItem.Checked = false;
                _updating = false;
            }
        }

        private void clearSavedSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists(Application.StartupPath + "/brawlbox.settings") == true)
                {
                    File.Delete(Application.StartupPath + "/brawlbox.settings");
                    clearSavedSettingsToolStripMenuItem.Enabled = false;
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
        }
        private void showPlay_Click_1(object sender, EventArgs e)
        {
            showPlay.Checked = !showPlay.Checked;
        }
        private void chkBones_Click(object sender, EventArgs e)
        {
            chkBones.Checked = !chkBones.Checked;
        }

        private void chkPolygons_Click(object sender, EventArgs e)
        {
            chkPolygons.CheckState = chkPolygons.CheckState == CheckState.Checked ? CheckState.Indeterminate :
                                     chkPolygons.CheckState == CheckState.Indeterminate ? CheckState.Unchecked :
                                     CheckState.Checked;
        }

        private void chkVertices_Click(object sender, EventArgs e)
        {
            chkVertices.Checked = !chkVertices.Checked;
        }

        private void chkFloor_Click(object sender, EventArgs e)
        {
            chkFloor.Checked = !chkFloor.Checked;
        }

        private void displayBRRESRelativeAnimationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            displayBRRESRelativeAnimationsToolStripMenuItem.CheckState = displayBRRESRelativeAnimationsToolStripMenuItem.CheckState == CheckState.Checked ? CheckState.Indeterminate :
                                                                         displayBRRESRelativeAnimationsToolStripMenuItem.CheckState == CheckState.Indeterminate ? CheckState.Unchecked :
                                                                         CheckState.Checked;
        }
        private void playCHR0ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            playCHR0ToolStripMenuItem.Checked = !playCHR0ToolStripMenuItem.Checked;
        }
        private void playSRT0ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            playSRT0ToolStripMenuItem.Checked = !playSRT0ToolStripMenuItem.Checked;
        }
        private void playSHP0ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            playSHP0ToolStripMenuItem.Checked = !playSHP0ToolStripMenuItem.Checked;
        }
        private void playPAT0ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            playPAT0ToolStripMenuItem.Checked = !playPAT0ToolStripMenuItem.Checked;
        }
        private void playVIS0ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            playVIS0ToolStripMenuItem.Checked = !playVIS0ToolStripMenuItem.Checked;
        }
        private void syncTexObjToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            pnlAssets._syncObjTex = syncTexObjToolStripMenuItem.Checked;
            pnlAssets.UpdateTextures();
        }
        private void syncObjectAndTexturesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            syncTexObjToolStripMenuItem.Checked = !syncTexObjToolStripMenuItem.Checked;
        }
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
            {
                panel1.BackgroundImage = BGImage = Image.FromFile(d.FileName);
                panel1.BackgroundImageLayout = ImageLayout.Stretch;
            }
        }
        private void btnAssetToggle_Click(object sender, EventArgs e)
        {
            if (!showAssets.Checked)
                if (!showBones.Checked)
                    showAssets.Checked = true;
                else
                    showBones.Checked = false;
            else
                if (!showBones.Checked)
                    showBones.Checked = true;
                else
                    showAssets.Checked = false;
        }
        private void btnOptionToggle_Click(object sender, EventArgs e) 
        {
            showOptions.Checked = !showOptions.Checked;
            //if (!showOptions.Checked)
            //    if (!showPlay.Checked)
            //        showOptions.Checked = true;
            //    else
            //        showPlay.Checked = false;
            //else
            //    if (!showPlay.Checked)
            //        showPlay.Checked = true;
            //    else
            //        showOptions.Checked = false;
        }
        private void btnPlaybackToggle_Click(object sender, EventArgs e) { showAnim.Checked = !showAnim.Checked; checkDimensions(); }
        private void btnAnimToggle_Click(object sender, EventArgs e)
        {
            showMoveset.Checked = !showMoveset.Checked;
        }
        public void btnPrevFrame_Click(object sender, EventArgs e) { pnlPlayback.numFrameIndex.Value--; }
        public void btnNextFrame_Click(object sender, EventArgs e) { pnlPlayback.numFrameIndex.Value++; }
        public void btnPlay_Click(object sender, EventArgs e)
        {
            if (pnlMoveset._mainMoveset != null && pnlMoveset.selectedActionNodes.Count > 0)
                if (pnlMoveset.animTimer.Enabled)
                    pnlMoveset.StopScript();
                else
                    pnlMoveset.RunScript();
            else
            {
                if (animTimer.Enabled)
                    StopAnim();
                else
                    PlayAnim();
            }
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
                    if (pnlMoveset._mainMoveset != null && pnlMoveset.selectedActionNodes.Count > 0)
                    {
                        if (pnlMoveset.ActionsIdling || (pnlMoveset.subactions && pnlMoveset._animFrame >= _maxFrame - 1))
                        {
                            if (pnlMoveset.subactions && pnlMoveset.selectedSubActionGrp != null)
                            {
                                if (_animFrame < _maxFrame)
                                {
                                    SetFrame(_animFrame + 1);
                                    pnlMoveset._animFrame = _animFrame - 1;
                                }
                                else
                                    pnlMoveset.SetFrame(0);
                            }
                        }
                        else
                            pnlMoveset.SetFrame(pnlMoveset._animFrame + 1);
                    }
                    else
                    if (GetSelectedBRRESFile(pnlAssets.fileType.SelectedIndex) != null)
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
                    if (pnlMoveset._mainMoveset != null && pnlMoveset.selectedActionNodes.Count > 0)
                    {
                        if (pnlMoveset._animFrame > 0)
                            pnlMoveset.SetFrame(pnlMoveset._animFrame - 1);
                        else if (pnlMoveset.subactions)
                        {
                            pnlMoveset.SetFrame(_maxFrame - 1);
                            pnlMoveset._animFrame = _animFrame - 1;
                        }
                    }
                    else
                    if (GetSelectedBRRESFile(pnlAssets.fileType.SelectedIndex) != null)
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
                else if (key == Keys.D)
                {
                    if (Control.ModifierKeys == (Keys.Control | Keys.Alt))
                    {
                        if (pnlAssets.Visible || pnlAnim.Visible || animEditors.Visible || pnlMoveset.Visible || controlPanel.Visible)
                            showAnim.Checked = showAssets.Checked = showBones.Checked = showMoveset.Checked = showOptions.Checked = false;
                        else
                            showAnim.Checked = showAssets.Checked = showBones.Checked = showMoveset.Checked = showOptions.Checked = true;
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
                        chr0Editor.numRotX.Value = _oldAngles._x;
                        chr0Editor.numRotY.Value = _oldAngles._y;
                        chr0Editor.numRotZ.Value = _oldAngles._z;
                        chr0Editor.BoxChanged(chr0Editor.numRotX, null);
                        chr0Editor.BoxChanged(chr0Editor.numRotY, null);
                        chr0Editor.BoxChanged(chr0Editor.numRotZ, null);
                    }
                    if (_translating) _translating = false;
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
                        if (undoToolStripMenuItem.Enabled)
                            undoToolStripMenuItem_Click(null, null);

                        return true;
                    }
                    else if (key == Keys.Y)
                    {
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
            if (pnlAnim.TargetBone != null && _chr0 != null)
            {
                CHR0EntryNode entry = _chr0.FindChild(((MDL0BoneNode)pnlAnim.TargetBone).Name, false) as CHR0EntryNode;
                if (entry != null)
                    for (int i = 0x10; i < 0x19; i++)
                    {
                        entry.SetKeyframe((KeyFrameMode)i, _animFrame - 1, chr0Editor._transBoxes[i - 0x10].Value);
                        chr0Editor.BoxChanged(chr0Editor._transBoxes[i - 0x10], null);
                    }
            }
        }
        private void Unkey(object sender, EventArgs e)
        {
            if (pnlAnim.TargetBone != null && _chr0 != null)
            {
                CHR0EntryNode entry = _chr0.FindChild(((MDL0BoneNode)pnlAnim.TargetBone).Name, false) as CHR0EntryNode;
                if (entry != null)
                    for (int i = 0x10; i < 0x19; i++)
                    {
                        entry.RemoveKeyframe((KeyFrameMode)i, _animFrame - 1);
                        chr0Editor.BoxChanged(chr0Editor._transBoxes[i - 0x10], null);
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

            models.Items.Clear();
        }

        //private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    if (_targetModel._canUndo)
        //        Undo_Click(this, null);
        //}

        //private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    if (_targetModel._canRedo)
        //        Redo_Click(this, null);
        //}

        private void toggleBonesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RenderBones = !RenderBones;
            if (RenderBones == false)
                toggleBones.Checked = false;
            else
                toggleBones.Checked = true;
        }

        private void togglePolygonsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (togglePolygons.CheckState == CheckState.Checked)
            {
                togglePolygons.Checked = false;
                chkPolygons.CheckState = CheckState.Unchecked;
            }
            else
            {
                togglePolygons.Checked = true;
                chkPolygons.CheckState = CheckState.Checked;
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            RenderVertices = !RenderVertices;
            if (RenderVertices == false)
                toggleVertices.Checked = false;
            else
                toggleVertices.Checked = true;
        }

        private void renderWireframeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chkPolygons.CheckState = CheckState.Indeterminate;
        }

        private void openModelSwitherToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new ModelSwitcher().ShowDialog(this, _targetModels);
        }

        private void hideFromSceneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            resetcam = false;

            modelPanel1.RemoveTarget(TargetModel);

            if (_targetModels != null && _targetModels.Count != 0)
                TargetModel = _targetModels[0];

            modelPanel1.Invalidate();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            resetcam = false;

            modelPanel1.RemoveTarget(TargetModel);
            _targetModels.Remove(TargetModel);
            models.Items.Remove(TargetModel);

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
                    models.Items.Remove(node);
                }

            modelPanel1.Invalidate();
        }
        private void modifyLightingToolStripMenuItem_Click(object sender, EventArgs e) { new LightEditor().ShowDialog(this); }
        private void showMoveset_Click(object sender, EventArgs e) { showMoveset.Checked = !showMoveset.Checked; }
        private void showAssets_Click(object sender, EventArgs e) { showAssets.Checked = !showAssets.Checked; }
        private void hitboxesOffToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chkHitboxes.Checked = !chkHitboxes.Checked;
            if (chkHitboxes.Checked == false)
                hitboxesOffToolStripMenuItem.Checked = false;
            else
                hitboxesOffToolStripMenuItem.Checked = true;

            modelPanel1.Invalidate();
        }
        private void hurtboxesOffToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chkHurtboxes.Checked = !chkHurtboxes.Checked;
            if (chkHurtboxes.Checked == false)
                hurtboxesOffToolStripMenuItem.Checked = false;
            else
                hurtboxesOffToolStripMenuItem.Checked = true;

            modelPanel1.Invalidate();
        }
        private void showAnim_Click(object sender, EventArgs e) { showBones.Checked = !showBones.Checked; }
        private void showPlay_Click(object sender, EventArgs e) { showAnim.Checked = !showAnim.Checked; }
        private void showOptions_Click(object sender, EventArgs e) { showOptions.Checked = !showOptions.Checked; }
        private void toggleFloor_Click(object sender, EventArgs e)
        {
            RenderFloor = !RenderFloor;
            if (RenderFloor == false)
                toggleFloor.Checked = false;
            else
                toggleFloor.Checked = true;
        }
        private void resetCameraToolStripMenuItem_Click_1(object sender, EventArgs e) { modelPanel1.ResetCamera(); }
        #endregion

        #region Value Change Functions

        private void displayBRRESRelativeAnimationsToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
        {
            pnlAssets.BRRESRelative = displayBRRESRelativeAnimationsToolStripMenuItem.CheckState;
            pnlAssets.UpdateAnimations(pnlAssets.fileType.SelectedIndex);
        }

        private void chkPolygons_CheckStateChanged(object sender, EventArgs e)
        {
            if (!_updating)
                RenderPolygons = chkPolygons.CheckState;
        }

        private void showPlay_CheckedChanged_1(object sender, EventArgs e)
        {
            pnlPlayback.Visible = showPlay.Checked;
        }

        private void displayFrameCountDifferencesToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
                return;

            DialogResult d;
            if (!displayFrameCountDifferencesToolStripMenuItem.Checked)
            {
                if ((d = MessageBox.Show("Do you want to sync animation frame counts by default?", "Sync Frame Counts by Default", MessageBoxButtons.YesNo)) == DialogResult.Yes && !alwaysSyncFrameCountsToolStripMenuItem.Checked)
                    alwaysSyncFrameCountsToolStripMenuItem.Checked = true;
                else if (d == DialogResult.No)
                    alwaysSyncFrameCountsToolStripMenuItem.Checked = false;
            }
            else
                alwaysSyncFrameCountsToolStripMenuItem.Checked = false;
        }

        private void syncObjectsListToVIS0ToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
                return;

            pnlAssets.chkSyncVis.Checked = syncLoopToAnimationToolStripMenuItem.Checked;
        }

        private void syncAnimationsTogetherToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (syncAnimationsTogetherToolStripMenuItem.Checked)
                GetFiles(pnlAssets.fileType.SelectedIndex);
            else
                GetFiles(-1);
        }
        public void pnlAnim_ReferenceLoaded(ResourceNode node) { modelPanel1.AddReference(node); }

        public void CHR0StateChanged(object sender, EventArgs e)
        {
            if (_chr0 == null)
                return;

            if (_animFrame < _chr0.FrameCount)
                SetFrame(_animFrame);
            pnlPlayback.numTotalFrames.Value = _chr0.FrameCount;
        }
        public void SRT0StateChanged(object sender, EventArgs e)
        {
            if (_srt0 == null)
                return;

            if (_animFrame < _srt0.FrameCount)
                SetFrame(_animFrame);
            pnlPlayback.numTotalFrames.Value = _srt0.FrameCount;
        }
        public void SHP0StateChanged(object sender, EventArgs e)
        {
            if (_shp0 == null)
                return;

            if (_animFrame < _shp0.FrameCount)
                SetFrame(_animFrame);
            pnlPlayback.numTotalFrames.Value = _shp0.FrameCount;
        }
        public void VIS0StateChanged(object sender, EventArgs e)
        {
            if (_vis0 == null)
                return;

            if (_animFrame < _vis0.FrameCount)
                SetFrame(_animFrame);
            pnlPlayback.numTotalFrames.Value = _vis0.FrameCount;
        }
        public void PAT0StateChanged(object sender, EventArgs e)
        {
            if (_pat0 == null)
                return;

            if (_animFrame < _pat0.FrameCount)
                SetFrame(_animFrame);
            pnlPlayback.numTotalFrames.Value = _pat0.FrameCount;
        }

        private void pnlOptions_FloorRenderChanged(object sender, EventArgs e)
        {
            if (RenderFloor == false)
                toggleFloor.Checked = false;
            else
                toggleFloor.Checked = true;

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
        //private void ApplySave(object sender, EventArgs e)
        //{
        //    SaveState save = _save;
        //    pnlAnim.ApplySave(save);
        //    SetFrame(save.frameIndex);
        //    modelPanel1.Invalidate();
        //}
        public void numFrameIndex_ValueChanged(object sender, EventArgs e)
        {
            int val = (int)pnlPlayback.numFrameIndex.Value;
            if (val != _animFrame)
                SetFrame(val);
        }
        public void numFPS_ValueChanged(object sender, EventArgs e) { pnlMoveset.animTimer.Interval = animTimer.Interval = 1000 / (int)pnlPlayback.numFPS.Value; }
        public void chkLoop_CheckedChanged(object sender, EventArgs e) 
        {
            _loop = pnlPlayback.chkLoop.Checked;
            if (syncLoopToAnimationToolStripMenuItem.Checked)
                ((BRESEntryNode)GetSelectedBRRESFile(pnlAssets.fileType.SelectedIndex)).tLoop = _loop;
        }

        private void FileChanged(object sender, EventArgs e)
        {
            if (pnlMoveset._mainMoveset == null)
                chkHurtboxes.Visible = chkHitboxes.Visible = chkHurtboxes.Checked = false;
            else
                chkHurtboxes.Visible = chkHitboxes.Visible = chkHurtboxes.Checked = true;
        }

        private void HtBoxesChanged(object sender, EventArgs e)
        {
            if (chkHurtboxes.Checked)
                hurtboxesOffToolStripMenuItem.Checked = true;
            else
                hurtboxesOffToolStripMenuItem.Checked = false;

            if (chkHitboxes.Checked)
                hitboxesOffToolStripMenuItem.Checked = true;
            else
                hitboxesOffToolStripMenuItem.Checked = false;
            
            modelPanel1.Invalidate(); 
        }

        public void SelectedPolygonChanged(object sender, EventArgs e) 
        {
            _targetModel.polyIndex = _targetModel._polyList.IndexOf(pnlAssets.SelectedPolygon);

            if (pnlAssets._syncObjTex)
                pnlAssets.UpdateTextures();

            modelPanel1.Invalidate(); 
        }

        public void numTotalFrames_ValueChanged(object sender, EventArgs e)
        {
            if ((GetSelectedBRRESFile(pnlAssets.fileType.SelectedIndex) == null) || (_updating))
                return;

            _maxFrame = (int)pnlPlayback.numTotalFrames.Value;

            ResourceNode n;
            if (alwaysSyncFrameCountsToolStripMenuItem.Checked)
                for (int i = 0; i < 5; i++)
                    if ((n = GetSelectedBRRESFile(i)) != null) 
                        //if (i == 5) ((BRESEntryNode)n).tFrameCount = _maxFrame - 1; else 
                        ((BRESEntryNode)n).tFrameCount = _maxFrame;
                    else { }
            else
            {
                if ((n = GetSelectedBRRESFile(pnlAssets.fileType.SelectedIndex)) != null)
                    ((BRESEntryNode)n).tFrameCount = _maxFrame;
                if (displayFrameCountDifferencesToolStripMenuItem.Checked)
                    if (MessageBox.Show("Do you want to update the frame counts of the other animation types?", "Update Frame Counts?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    for (int i = 0; i < 5; i++)
                        if (i != pnlAssets.fileType.SelectedIndex && (n = GetSelectedBRRESFile(i)) != null)
                            ((BRESEntryNode)n).tFrameCount = _maxFrame;
            }

            pnlPlayback.numFrameIndex.Maximum = _maxFrame;
        }
        private void showAssets_CheckedChanged(object sender, EventArgs e)
        {
            pnlAssets.Visible = spltMoveset.Visible = showAssets.Checked;

            if (showAssets.Checked == false && showBones.Checked == false)
                btnAssetToggle.Text = ">";
            else if (showAssets.Checked == true && showBones.Checked == true)
                btnAssetToggle.Text = "<";
        }
        private void showAnim_CheckedChanged(object sender, EventArgs e)
        {
            pnlAnim.Visible = spltAssets.Visible = showBones.Checked;

            if (showAssets.Checked == false && showBones.Checked == false)
                btnAssetToggle.Text = ">";
            else if (showAssets.Checked == true && showBones.Checked == true)
                btnAssetToggle.Text = "<";
        }
        private void showMoveset_CheckedChanged(object sender, EventArgs e)
        {
            pnlMoveset.Visible = spltAnims.Visible = showMoveset.Checked;

            if (pnlMoveset.Visible)
                btnAnimToggle.Text = ">";
            else
                btnAnimToggle.Text = "<";
        }
        private void showPlay_CheckedChanged(object sender, EventArgs e) 
        {
            animEditors.Visible = !animEditors.Visible;
            //if (_currentControl is CHR0Editor)
            //{
            //    animEditors.Height =
            //    panel3.Height = 82;
            //    panel3.Width = 732;
            //}
            //else if (_currentControl is SRT0Editor)
            //{
            //    animEditors.Height =
            //    panel3.Height = 82;
            //    panel3.Width = 561;
            //}
            //else if (_currentControl is SHP0Editor)
            //{
            //    animEditors.Height =
            //    panel3.Height = 106;
            //    panel3.Width = 533;
            //}
            //else if (_currentControl is PAT0Editor)
            //{
            //    animEditors.Height =
            //    panel3.Height = 77;
            //    panel3.Width = 402;
            //}
            //else if (_currentControl is VIS0Editor)
            //{
            //    animEditors.Height =
            //    panel3.Height = 112;
            //    panel3.Width = 507;
            //}
            //else
            //    animEditors.Height = panel3.Width = 0;
            checkDimensions();
        }
        private void showOptions_CheckedChanged(object sender, EventArgs e) { controlPanel.Visible = showOptions.Checked; }
        //private void undoToolStripMenuItem_EnabledChanged(object sender, EventArgs e) { Undo.Enabled = undoToolStripMenuItem.Enabled; }
        //private void redoToolStripMenuItem_EnabledChanged(object sender, EventArgs e) { Redo.Enabled = redoToolStripMenuItem.Enabled; }
        
        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (!_updating)
                RenderVertices = chkVertices.Checked;
        }

        private void models_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_updating)
                return;

            resetcam = false;

            if ((models.SelectedItem is MDL0Node) && models.SelectedItem.ToString() != "All")
                TargetModel = (MDL0Node)models.SelectedItem;
            else
                TargetModel = _targetModels != null && _targetModels.Count > 0 ? _targetModels[0] : null;

            undoSaves.Clear();
            redoSaves.Clear();
            saveIndex = 0;
            firstUndo = false;
        }

        private void chkBones_CheckedChanged(object sender, EventArgs e)
        {
            if (!_updating)
                RenderBones = chkBones.Checked;
        }

        private void chkFloor_CheckedChanged(object sender, EventArgs e)
        {
            if (!_updating)
                RenderFloor = chkFloor.Checked;
        }

        private void chr0Editor_VisibleChanged(object sender, EventArgs e)
        {
            //pnlEditors.Height = pnlPlayback.Height + (chr0Editor.Visible ? chr0Editor.Height : 0);
        }

        #endregion

        #region BRRES Node Functions

        #region Get
        public void GetFiles(int focusType)
        {
            if (focusType < 0)
            {
                focusType = pnlAssets.fileType.SelectedIndex;
                if (focusType != 0)
                    _chr0 = null;
                if (focusType != 1)
                    _srt0 = null;
                if (focusType != 2)
                    _shp0 = null;
                if (focusType != 3)
                    _pat0 = null;
                if (focusType != 4)
                    _vis0 = null;
            }
            else
            {
                if (focusType != 0)
                    GetCHR0(focusType);
                if (focusType != 1)
                    GetSRT0(focusType);
                if (focusType != 2)
                    GetSHP0(focusType);
                if (focusType != 3)
                    GetPAT0(focusType);
                if (focusType != 4)
                    GetVIS0(focusType);
            }
        }
        public bool GetVIS0(int focusType)
        {
            BRESEntryNode focusFile = GetSelectedBRRESFile(focusType) as BRESEntryNode;
            ResourceNode group = null;
            if (focusFile != null && (group = focusFile.RootNode) != null)
                if ((_vis0 = (VIS0Node)group.FindChildByType(focusFile.Name, true, ResourceType.VIS0)) == null)
                {
                    //Don't create new resource until the user makes a change
                }
                else
                {
                    if (_vis0.FrameCount != focusFile.tFrameCount)
                    {
                        if (displayFrameCountDifferencesToolStripMenuItem.Checked)
                            if (MessageBox.Show("The VIS0 \"" + _vis0.Name + "\" has a frame count of " + _vis0.FrameCount + ".\nThe currently selected animation has a frame count of " + ((BRESEntryNode)GetSelectedBRRESFile(pnlAssets.fileType.SelectedIndex)).tFrameCount + ".\nWould you like to update its frame count?\n(This message can be disabled under Options in the menu bar)", "VIS0 Frame Count", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                _vis0.FrameCount = focusFile.tFrameCount;
                            else { }
                        else if (alwaysSyncFrameCountsToolStripMenuItem.Checked)
                            _vis0.FrameCount = focusFile.tFrameCount;
                    }
                    return true;
                }
            else _vis0 = null;
            return false;
        }
        public bool GetPAT0(int focusType)
        {
            BRESEntryNode focusFile = GetSelectedBRRESFile(focusType) as BRESEntryNode;
            ResourceNode group = null;
            if (focusFile != null && (group = focusFile.RootNode) != null)
                if ((_pat0 = (PAT0Node)group.FindChildByType(focusFile.Name, true, ResourceType.PAT0)) == null)
                {
                    //Don't create new resource until the user makes a change
                }
                else
                {
                    if (_pat0.FrameCount != focusFile.tFrameCount)
                    {
                        if (displayFrameCountDifferencesToolStripMenuItem.Checked)
                            if (MessageBox.Show("The PAT0 \"" + _pat0.Name + "\" has a frame count of " + _pat0.FrameCount + ".\nThe currently selected animation has a frame count of " + ((BRESEntryNode)GetSelectedBRRESFile(pnlAssets.fileType.SelectedIndex)).tFrameCount + ".\nWould you like to update its frame count?\n(This message can be disabled under Options in the menu bar)", "PAT0 Frame Count", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                _pat0.FrameCount = (ushort)focusFile.tFrameCount;
                            else { }
                        else if (alwaysSyncFrameCountsToolStripMenuItem.Checked)
                            _pat0.FrameCount = (ushort)focusFile.tFrameCount;
                    }
                    return true;
                }
            else _pat0 = null;
            return false;
        }
        public bool GetSRT0(int focusType)
        {
            BRESEntryNode focusFile = GetSelectedBRRESFile(focusType) as BRESEntryNode;
            ResourceNode group = null;
            if (focusFile != null && (group = focusFile.RootNode) != null)
                if ((_srt0 = (SRT0Node)group.FindChildByType(focusFile.Name, true, ResourceType.SRT0)) == null)
                {
                    //Don't create new resource until the user makes a change
                }
                else
                {
                    if (_srt0.FrameCount != focusFile.tFrameCount)
                    {
                        if (displayFrameCountDifferencesToolStripMenuItem.Checked)
                            if (MessageBox.Show("The SRT0 \"" + _srt0.Name + "\" has a frame count of " + _srt0.FrameCount + ".\nThe currently selected animation has a frame count of " + ((BRESEntryNode)GetSelectedBRRESFile(pnlAssets.fileType.SelectedIndex)).tFrameCount + ".\nWould you like to update its frame count?\n(This message can be disabled under Options in the menu bar)", "SRT0 Frame Count", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                _srt0.FrameCount = focusFile.tFrameCount;
                            else { }
                        else if (alwaysSyncFrameCountsToolStripMenuItem.Checked)
                            _srt0.FrameCount = focusFile.tFrameCount;
                    }
                    return true;
                }
            else _srt0 = null;
            return false;
        }
        public bool GetSHP0(int focusType)
        {
            BRESEntryNode focusFile = GetSelectedBRRESFile(focusType) as BRESEntryNode;
            ResourceNode group = null;
            if (focusFile != null && (group = focusFile.RootNode) != null)
                if ((_shp0 = (SHP0Node)group.FindChildByType(focusFile.Name, true, ResourceType.SHP0)) == null)
                {
                    //Don't create new resource until the user makes a change
                }
                else
                {
                    if (_shp0.FrameCount != focusFile.tFrameCount)
                    {
                        if (displayFrameCountDifferencesToolStripMenuItem.Checked)
                            if (MessageBox.Show("The SHP0 \"" + _shp0.Name + "\" has a frame count of " + _shp0.FrameCount + ".\nThe currently selected animation has a frame count of " + ((BRESEntryNode)GetSelectedBRRESFile(pnlAssets.fileType.SelectedIndex)).tFrameCount + ".\nWould you like to update its frame count?\n(This message can be disabled under Options in the menu bar)", "SHP0 Frame Count", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                _shp0.FrameCount = focusFile.tFrameCount;
                            else { }
                        else if (alwaysSyncFrameCountsToolStripMenuItem.Checked)
                            _shp0.FrameCount = focusFile.tFrameCount;
                    }
                    return true;
                }
            else _shp0 = null;
            return false;
        }
        public bool GetCHR0(int focusType)
        {
            BRESEntryNode focusFile = GetSelectedBRRESFile(focusType) as BRESEntryNode;
            ResourceNode group = null;
            if (focusFile != null && (group = focusFile.RootNode) != null)
                if ((_chr0 = (CHR0Node)group.FindChildByType(focusFile.Name, true, ResourceType.CHR0)) == null)
                {
                    //Don't create new resource until the user makes a change
                }
                else
                {
                    if (_chr0.FrameCount != focusFile.tFrameCount)
                    {
                        if (displayFrameCountDifferencesToolStripMenuItem.Checked)
                            if (MessageBox.Show("The CHR0 \"" + _chr0.Name + "\" has a frame count of " + _chr0.FrameCount + ".\nThe currently selected animation has a frame count of " + ((BRESEntryNode)GetSelectedBRRESFile(pnlAssets.fileType.SelectedIndex)).tFrameCount + ".\nWould you like to update its frame count?\n(This message can be disabled under Options in the menu bar)", "CHR0 Frame Count", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                _chr0.FrameCount = focusFile.tFrameCount;
                            else { }
                        else if (alwaysSyncFrameCountsToolStripMenuItem.Checked)
                            _chr0.FrameCount = focusFile.tFrameCount;
                    }
                    return true;
                }
            else _chr0 = null;
            return false;
        }
        #endregion

        #region Create

        public void CreateVIS0()
        {
            BRESNode group = null;
            BRESEntryNode n = null;
            if ((n = GetSelectedBRRESFile(pnlAssets.fileType.SelectedIndex) as BRESEntryNode) != null &&
                (group = n.Parent.Parent as BRESNode) != null)
            {
                _vis0 = group.CreateResource<VIS0Node>(SelectedCHR0.Name);
                foreach (string s in pnlAssets.VIS0Indices.Keys)
                {
                    VIS0EntryNode node = null;
                    if ((node = (VIS0EntryNode)_vis0.FindChild(s, true)) == null && ((MDL0BoneNode)_targetModel.FindChildByType(s, true, ResourceType.MDL0Bone)).BoneIndex != 0 && s != "EyeYellowM")
                    {
                        node = _vis0.CreateEntry();
                        node.Name = s;
                        node.MakeConstant(true);
                    }
                }
            }
        }

        #endregion

        #region VIS0
        public void UpdateVis0(object sender, EventArgs e)
        {
            BRESEntryNode n;
            if ((n = GetSelectedBRRESFile(pnlAssets.fileType.SelectedIndex) as BRESEntryNode) == null || _animFrame == 0)
                return;

            Start:
            if (_vis0 != null)
            {
                int index = pnlAssets._polyIndex;
                if (index == -1)
                    return;

                MDL0BoneNode bone = ((MDL0PolygonNode)pnlAssets.lstObjects.Items[index])._bone;

                VIS0EntryNode node = null;
                if ((node = (VIS0EntryNode)_vis0.FindChild(bone.Name, true)) == null && bone.BoneIndex != 0 && bone.Name != "EyeYellowM")
                {
                    node = _vis0.CreateEntry();
                    node.Name = bone.Name;
                    node.MakeConstant(true);
                }

                //Item is in the process of being un/checked; it's not un/checked at the given moment.
                //Use opposite of current check state.
                bool ANIMval = !pnlAssets.lstObjects.GetItemChecked(index);

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

                if (node != null && vis0Editor.visEditor1.TargetNode.Name == node.Name)
                    vis0Editor.UpdateEntry();
            }
            else
            {
                CreateVIS0();
                if (_vis0 != null) 
                    goto Start;
            }
        }
        public void ReadVIS0()
        {
            if (_animFrame == 0 || pnlAssets.lstObjects.Items.Count == 0)
                return;

            pnlAssets._vis0Updating = true;
            if (_vis0 != null)
            {
                if (GetSelectedBRRESFile(pnlAssets.fileType.SelectedIndex) != null && _vis0.FrameCount != ((BRESEntryNode)GetSelectedBRRESFile(pnlAssets.fileType.SelectedIndex)).tFrameCount)
                    UpdateVis0(null, null);

                foreach (string n in pnlAssets.VIS0Indices.Keys)
                {
                    VIS0EntryNode node = null;
                    List<int> indices = pnlAssets.VIS0Indices[n];
                    for (int i = 0; i < indices.Count; i++)
                    {
                        if ((node = (VIS0EntryNode)_vis0.FindChild(((MDL0PolygonNode)pnlAssets.lstObjects.Items[indices[i]])._bone.Name, true)) != null)
                        {
                            if (node._entryCount != 0 && _animFrame != 0)
                                pnlAssets.lstObjects.SetItemChecked(indices[i], node.GetEntry(_animFrame - 1));
                            else
                                pnlAssets.lstObjects.SetItemChecked(indices[i], node._flags.HasFlag(VIS0Flags.Enabled));
                        }
                    }
                }
            }
            pnlAssets._vis0Updating = false;
        }
        #endregion

        #endregion

        #region Rendering
        //public float _xTranslate = 0, _prevX = 0, _yTranslate = 0, _prevY = 0;
        private unsafe void modelPanel1_PreRender(object sender, GLContext context)
        {
            if (RenderFloor)
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

                //context.glMatrixMode(GLMatrixMode.Texture);
                //context.glTranslate(_xTranslate, _yTranslate, 0);
                //_xTranslate = 0;
                //_yTranslate = 0;
                //context.glMatrixMode(GLMatrixMode.ModelView);
            }
        }
        public int hurtBoxType = 0;
        public int editType = 1; //0 Rot, 1 Trans, 2 Scale
        public List<MDL0BoneNode> boneCollisions = new List<MDL0BoneNode>();
        private unsafe void modelPanel1_PostRender(object sender, GLContext context)
        {
            //Render hurtboxes
            if (chkHurtboxes.Checked)
                for (int i = 0; i < pnlMoveset.lstHurtboxes.Items.Count; i++)
                    if (pnlMoveset.lstHurtboxes.GetItemChecked(i))
                        ((MoveDefHurtBoxNode)pnlMoveset.lstHurtboxes.Items[i]).Render(context, pnlMoveset._selectedHurtboxIndex == i, hurtBoxType);

            //Render hitboxes
            if (chkHitboxes.Checked)
            {
                context.glDisable((uint)GLEnableCap.Lighting);
                context.glDisable((uint)GLEnableCap.DepthTest);
                context.glPolygonMode(GLFace.FrontAndBack, GLPolygonMode.Fill);
                GLDisplayList c = context.GetRingList();
                GLDisplayList s = context.GetSphereList();

                foreach (MoveDefActionNode a in pnlMoveset.selectedActionNodes)
                {
                    if (a.catchCollisions != null && a.catchCollisions.Count > 0)
                        foreach (MoveDefEventNode e in a.catchCollisions)
                            e.RenderCatchCollision(TargetModel._linker.BoneCache, context, modelPanel1._camera.GetPoint(), MParams.DrawStyle.Brawl);

                    if (a.offensiveCollisions != null && a.offensiveCollisions.Count > 0)
                        foreach (MoveDefEventNode e in a.offensiveCollisions)
                            e.RenderOffensiveCollision(TargetModel._linker.BoneCache, context, modelPanel1._camera.GetPoint(), MParams.DrawStyle.Brawl);

                    if (a.specialOffensiveCollisions != null && a.specialOffensiveCollisions.Count > 0)
                        foreach (MoveDefEventNode e in a.specialOffensiveCollisions)
                            e.RenderSpecialOffensiveCollision(TargetModel._linker.BoneCache, context, modelPanel1._camera.GetPoint(), MParams.DrawStyle.Brawl);

                    if (a.subRoutine != null)
                    {
                        if (a.subRoutine.catchCollisions != null && a.subRoutine.catchCollisions.Count > 0)
                            foreach (MoveDefEventNode e in a.subRoutine.catchCollisions)
                                e.RenderCatchCollision(TargetModel._linker.BoneCache, context, modelPanel1._camera.GetPoint(), MParams.DrawStyle.Brawl);

                        if (a.subRoutine.offensiveCollisions != null && a.subRoutine.offensiveCollisions.Count > 0)
                            foreach (MoveDefEventNode e in a.subRoutine.offensiveCollisions)
                                e.RenderOffensiveCollision(TargetModel._linker.BoneCache, context, modelPanel1._camera.GetPoint(), MParams.DrawStyle.Brawl);

                        if (a.subRoutine.specialOffensiveCollisions != null && a.subRoutine.specialOffensiveCollisions.Count > 0)
                            foreach (MoveDefEventNode e in a.subRoutine.specialOffensiveCollisions)
                                e.RenderSpecialOffensiveCollision(TargetModel._linker.BoneCache, context, modelPanel1._camera.GetPoint(), MParams.DrawStyle.Brawl);
                    }
                }
            }

            context.glClear(GLClearMask.DepthBuffer);
            context.glEnable(GLEnableCap.DepthTest);

            if (pnlAnim.SelectedBone != null && !_playing) //Render drag and drop control
            {
                context.glDisable((uint)GLEnableCap.Lighting);
                MDL0BoneNode bone = pnlAnim.SelectedBone;
                GLDisplayList sphere = context.GetCircleList();
                Matrix m;

                //Prepare camera-facing matrix
                Vector3 center = bone._frameMatrix.GetPoint();
                Vector3 cam = modelPanel1._camera.GetPoint();
                float radius = center.TrueDistance(cam) / _orbRadius * 0.1f;

                m = Matrix.TransformMatrix(new Vector3(radius), center.LookatAngles(cam) * Maths._rad2degf, center);
                context.glPushMatrix();
                context.glMultMatrix((float*)&m);

                editType = 0;

                if (editType == 0)
                {
                    GLDisplayList circle = context.GetRingList();

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

                    //X
                    if (_snapX)
                        context.glColor(1.0f, 1.0f, 0.0f, 1.0f);
                    else
                        context.glColor(1.0f, 0.0f, 0.0f, 1.0f);

                    circle.Call();
                    context.glRotate(90.0f, 1.0f, 0.0f, 0.0f);

                    //Y
                    if (_snapY)
                        context.glColor(1.0f, 1.0f, 0.0f, 1.0f);
                    else
                        context.glColor(0.0f, 1.0f, 0.0f, 1.0f);

                    circle.Call();
                }
                else if (editType == 1)
                {
                    //Create the axes.
                    GLDisplayList axis = new GLDisplayList(context);
                    axis.Begin();

                    //Disable cull mode so square bases for the arrows aren't necessary to draw
                    context.glDisable((uint)GLEnableCap.CullFace);
                    context.glPolygonMode(GLFace.FrontAndBack, GLPolygonMode.Fill);

                    float apthm = 0.075f;
                    float dst = 1.5f;

                    context.glBegin(GLPrimitiveType.Lines);

                    //X
                    if (!_snapXY && !_snapXZ)
                        context.glColor(1.0f, 0.0f, 0.0f, 1.0f);
                    context.glVertex(1.0f, 0.0f, 0.0f);
                    context.glVertex(1.0f, 1.0f, 0.0f);
                    context.glVertex(1.0f, 0.0f, 0.0f);
                    context.glVertex(1.0f, 0.0f, 1.0f);

                    if (_snapX || _snapXY || _snapXZ)
                        context.glColor(1.0f, 1.0f, 0.0f, 1.0f);
                    else
                        context.glColor(1.0f, 0.0f, 0.0f, 1.0f);
                    context.glVertex(0.0f, 0.0f, 0.0f);
                    context.glVertex(2.0f, 0.0f, 0.0f);

                    context.glEnd();

                    context.glBegin(GLPrimitiveType.Triangles);

                    context.glVertex(2.0f, 0.0f, 0.0f);
                    context.glVertex(dst, apthm, -apthm);
                    context.glVertex(dst, apthm, apthm);

                    context.glVertex(2.0f, 0.0f, 0.0f);
                    context.glVertex(dst, -apthm, apthm);
                    context.glVertex(dst, -apthm, -apthm);

                    context.glVertex(2.0f, 0.0f, 0.0f);
                    context.glVertex(dst, apthm, apthm);
                    context.glVertex(dst, -apthm, apthm);

                    context.glVertex(2.0f, 0.0f, 0.0f);
                    context.glVertex(dst, -apthm, -apthm);
                    context.glVertex(dst, apthm, -apthm);

                    context.glEnd();

                    context.glBegin(GLPrimitiveType.Lines);

                    //Y
                    if (!_snapXY && !_snapYZ)
                        context.glColor(0.0f, 1.0f, 0.0f, 1.0f);
                    context.glVertex(0.0f, 1.0f, 0.0f);
                    context.glVertex(1.0f, 1.0f, 0.0f);
                    context.glVertex(0.0f, 1.0f, 0.0f);
                    context.glVertex(0.0f, 1.0f, 1.0f);
                    if (_snapY || _snapXY || _snapYZ)
                        context.glColor(1.0f, 1.0f, 0.0f, 1.0f);
                    else
                        context.glColor(0.0f, 1.0f, 0.0f, 1.0f);
                    context.glVertex(0.0f, 0.0f, 0.0f);
                    context.glVertex(0.0f, 2.0f, 0.0f);

                    context.glEnd();

                    context.glBegin(GLPrimitiveType.Triangles);

                    context.glVertex(0.0f, 2.0f, 0.0f);
                    context.glVertex(apthm, dst, -apthm);
                    context.glVertex(apthm, dst, apthm);

                    context.glVertex(0.0f, 2.0f, 0.0f);
                    context.glVertex(-apthm, dst, apthm);
                    context.glVertex(-apthm, dst, -apthm);

                    context.glVertex(0.0f, 2.0f, 0.0f);
                    context.glVertex(apthm, dst, apthm);
                    context.glVertex(-apthm, dst, apthm);

                    context.glVertex(0.0f, 2.0f, 0.0f);
                    context.glVertex(-apthm, dst, -apthm);
                    context.glVertex(apthm, dst, -apthm);

                    context.glEnd();

                    context.glBegin(GLPrimitiveType.Lines);

                    //Z
                    if (!_snapXZ && !_snapYZ)
                        context.glColor(0.0f, 0.0f, 1.0f, 1.0f);
                    context.glVertex(0.0f, 0.0f, 1.0f);
                    context.glVertex(1.0f, 0.0f, 1.0f);
                    context.glVertex(0.0f, 0.0f, 1.0f);
                    context.glVertex(0.0f, 1.0f, 1.0f);
                    if (_snapZ || _snapXZ || _snapYZ)
                        context.glColor(1.0f, 1.0f, 0.0f, 1.0f);
                    else
                        context.glColor(0.0f, 0.0f, 1.0f, 1.0f);
                    context.glVertex(0.0f, 0.0f, 0.0f);
                    context.glVertex(0.0f, 0.0f, 2.0f);

                    context.glEnd();

                    context.glBegin(GLPrimitiveType.Triangles);

                    context.glVertex(0.0f, 0.0f, 2.0f);
                    context.glVertex(apthm, -apthm, dst);
                    context.glVertex(apthm, apthm, dst);

                    context.glVertex(0.0f, 0.0f, 2.0f);
                    context.glVertex(-apthm, apthm, dst);
                    context.glVertex(-apthm, -apthm, dst);

                    context.glVertex(0.0f, 0.0f, 2.0f);
                    context.glVertex(apthm, apthm, dst);
                    context.glVertex(-apthm, apthm, dst);

                    context.glVertex(0.0f, 0.0f, 2.0f);
                    context.glVertex(-apthm, -apthm, dst);
                    context.glVertex(apthm, -apthm, dst);

                    context.glEnd();

                    axis.End();

                    //Pop
                    context.glPopMatrix();

                    //Enter local space
                    m = Matrix.TransformMatrix(new Vector3(radius), bone._frameMatrix.GetAngles(), center);
                    context.glPushMatrix();
                    context.glMultMatrix((float*)&m);

                    axis.Call();
                }
                else if (editType == 2)
                {
                    //scale
                }

                //Pop
                context.glPopMatrix();

                //Clear depth buffer for next operation
                context.glClear(GLClearMask.DepthBuffer);
            }

            if (RenderBones && !modelPanel1._grabbing && !modelPanel1._scrolling && !_playing) 
            {
                //Render invisible depth orbs
                context.glColorMask(false, false, false, false);
                if (_editingAll)
                {
                    GLDisplayList list = context.GetSphereList();
                    foreach (MDL0Node m in _targetModels)
                        foreach (MDL0BoneNode bone in m._linker.BoneCache)
                            RenderOrbRecursive(bone, context, list);
                }
                else
                if (TargetModel != null && TargetModel._linker.BoneCache != null)
                {
                    GLDisplayList list = context.GetSphereList();
                    foreach (MDL0BoneNode bone in _targetModel._linker.BoneCache)
                        RenderOrbRecursive(bone, context, list);
                }
                context.glColorMask(true, true, true, true);
            }

            //if (_targetModel != null)
            //{
            //    Undo.Enabled = _targetModel._canUndo;
            //    Redo.Enabled = _targetModel._canRedo;
            //}
        }

        private unsafe void RenderOrbRecursive(MDL0BoneNode bone, GLContext ctx, GLDisplayList list)
        {
            Matrix m = Matrix.TransformMatrix(new Vector3(MDL0BoneNode._nodeRadius), new Vector3(), bone._frameMatrix.GetPoint());
            ctx.glPushMatrix();
            ctx.glMultMatrix((float*)&m);

            list.Call();
            ctx.glPopMatrix();

            //foreach (MDL0BoneNode b in bone.Children)
            //    RenderOrbRecursive(b, ctx, list);
        }

        private void modelPanel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == Forms.MouseButtons.Left)
            {
                //Reset snap flags
                _snapX = _snapY = _snapZ = _snapXY = _snapXZ = _snapYZ = _snapCirc = false;

                MDL0BoneNode bone = pnlAnim.SelectedBone;
                MDL0PolygonNode poly = pnlAssets.SelectedPolygon;

                //Re-target selected bone
                if (bone != null)
                {
                    //Try to re-target selected node

                    //Get the location of the bone
                    Vector3 center = bone._frameMatrix.GetPoint();

                    if (editType == 0)
                    {
                        //Standard radius scaling snippet. This is used for orb scaling depending on camera distance.
                        float radius = center.TrueDistance(modelPanel1._camera.GetPoint()) / _orbRadius * 0.1f;

                        //Get point projected onto our orb.
                        Vector3 point = modelPanel1.ProjectCameraSphere(new Vector2(e.X, e.Y), center, radius, false);

                        //Get the distance of the mouse point from the bone
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
                            pnlAnim.SelectedBone = bone = null;
                            goto GetBone;
                        }

                        //Bone re-targeted. Get angles and local point (aligned to snapping plane).
                        if (GetOrbPoint(new Vector2(e.X, e.Y), out point))
                        {
                            _rotating = true;
                            _oldAngles = bone._frameState._rotate;
                            _lastPoint = bone._inverseFrameMatrix * point;

                            CreateUndo(pnlAnim.SelectedBone);
                        }

                        //Ensure a redraw so the snapping indicators are correct
                        modelPanel1.Invalidate();
                    }
                    else if (editType == 1)
                    {
                        //Standard radius scaling snippet. This is used for orb scaling depending on camera distance.
                        float radius = center.TrueDistance(modelPanel1._camera.GetPoint()) / _axisLength * 0.1f;

                        //Get point projected onto our orb.
                        Vector3 point = modelPanel1.ProjectCameraSphere(new Vector2(e.X, e.Y), center, radius, false);

                        //Get the distance of the mouse point from the bone
                        float distance = point.TrueDistance(center);

                        if (Math.Abs(distance - radius) < (radius * _axisScale)) //Point lies within orb radius
                        {
                            Vector3 vector = point - center;

                            if (vector._y < 0.03f && vector._z < 0.03f)
                                _snapX = true;
                            if (vector._x < 0.03f && vector._z < 0.03f)
                                _snapY = true;
                            if (vector._x < 0.03f && vector._y < 0.03f)
                                _snapZ = true;

                            if (_snapX && _snapY)
                            {
                                _snapXY = true;
                            }
                            else if (_snapY && _snapZ)
                            {
                                _snapYZ = true;
                            }
                            else if (_snapX && _snapZ)
                            {
                                _snapXZ = true;
                            }
                        }
                        else
                        {
                            //Orb selection missed. Assign bone and move to next step.
                            pnlAnim.SelectedBone = bone = null;
                            goto GetBone;
                        }

                        //Bone re-targeted. Get angles and local point (aligned to snapping plane).
                        if (GetOrbPoint(new Vector2(e.X, e.Y), out point))
                        {
                            _translating = true;
                            _oldPosition = bone._frameState._translate;
                            _lastPoint = bone._inverseFrameMatrix * point;

                            CreateUndo(pnlAnim.SelectedBone);
                        }

                        //Ensure a redraw so the snapping indicators are correct
                        modelPanel1.Invalidate();
                    }
                }

            GetBone:

                //Try selecting new bone
                if (bone == null)
                {
                    float depth = modelPanel1.GetDepth(e.X, e.Y);
                    if ((depth < 1.0f) && (_targetModel != null) && (_targetModel._boneList != null))
                    {
                        Vector3 point = modelPanel1.UnProject(e.X, e.Y, depth);

                        //Find orb near chosen point
                        if (_editingAll)
                        {
                            foreach (MDL0Node m in _targetModels)
                                foreach (MDL0BoneNode b in m._boneList)
                                    if (CompareDistanceRecursive(b, point, ref bone))
                                        break;
                        }
                        else
                            foreach (MDL0BoneNode b in _targetModel._boneList)
                                if (CompareDistanceRecursive(b, point, ref bone))
                                    break;

                        //Assign new bone
                        if (bone != null)
                            pnlAnim.SelectedBone = bone;
                    }
                }
                modelPanel1.Invalidate();
            }
        }

        private void modelPanel1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == Forms.MouseButtons.Left)
            {
                if (_rotating) undoToolStripMenuItem.Enabled = true;

                _rotating = _translating = _scaling = false;
            }
        }

        private unsafe void modelPanel1_MouseMove(object sender, MouseEventArgs e)
        {
            MDL0BoneNode bone = pnlAnim.SelectedBone;

            if (_rotating || _translating && bone != null)
            {
                Vector3 point;
                if (GetOrbPoint(new Vector2(e.X, e.Y), out point))
                {
                    //Convert to local point
                    Vector3 lPoint = bone._inverseFrameMatrix * point;

                    //Check for change in selection.
                    if (_lastPoint != lPoint)
                    {
                        if (editType == 0)
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
                        }
                        else if (editType == 1)
                        {
                            //Get difference
                            Vector3 translation = lPoint - _lastPoint;

                            if (!_snapY && !_snapZ)
                            if (translation._x != 0.0f) ApplyTranslation(0, translation._x);
                            if (!_snapX && !_snapZ)
                            if (translation._y != 0.0f) ApplyTranslation(1, translation._y);
                            if (!_snapX && !_snapY)
                            if (translation._z != 0.0f) ApplyTranslation(2, translation._z);
                        }
                        else if (editType == 2)
                        {
                            //Get scale factor
                            Vector3 scale = lPoint / _lastPoint;

                            if (scale._x != 0.0f) ApplyScale(0, scale._x);
                            if (scale._y != 0.0f) ApplyScale(1, scale._y);
                            if (scale._z != 0.0f) ApplyScale(2, scale._z);
                        }
                        //Find new local mouse-point (should be the same)
                        _lastPoint = bone._inverseFrameMatrix * point;
                    }
                }
            }
        }

        //Updates specified angle by applying an offset.
        //Allows pnlAnim to handle the changes so keyframes are updated.
        private unsafe void ApplyAngle(int index, float offset)
        {
            NumericInputBox box = chr0Editor._transBoxes[index + 3];
            box.Value = (float)Math.Round(box._value + offset, 3);
            chr0Editor.BoxChanged(box, null);
        }
        //Updates translation with offset.
        private unsafe void ApplyTranslation(int index, float offset)
        {
            NumericInputBox box = chr0Editor._transBoxes[index + 6];
            box.Value = (float)Math.Round(box._value + offset, 3);
            chr0Editor.BoxChanged(box, null);
        }
        //Updates scale with offset.
        private unsafe void ApplyScale(int index, float offset)
        {
            NumericInputBox box = chr0Editor._transBoxes[index];
            box.Value = (float)Math.Round(box._value + offset, 3);
            chr0Editor.BoxChanged(box, null);
        }

        //Gets world-point of specified mouse point projected onto the selected bone's local space.
        //Intersects the projected ray with the appropriate plane using the snap flags.
        private bool GetOrbPoint(Vector2 mousePoint, out Vector3 point)
        {
            MDL0BoneNode bone = pnlAnim.SelectedBone;
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

            if (editType == 0)
            {
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
            }
            else// if (editType == 1)
            {
                if (_snapX)
                    normal = (bone._frameMatrix * new Vector3(0.0f, 0.0f, 1.0f)).Normalize(center);
                else if (_snapY)
                    normal = (bone._frameMatrix * new Vector3(0.0f, 1.0f, 0.0f)).Normalize(center);
                else if (_snapZ)
                    normal = (bone._frameMatrix * new Vector3(1.0f, 0.0f, 0.0f)).Normalize(center);
                else if (_snapXY)
                    normal = (bone._frameMatrix * new Vector3(0.0f, 0.0f, 1.0f)).Normalize(center);
                else if (_snapXZ)
                    normal = (bone._frameMatrix * new Vector3(0.0f, 1.0f, 0.0f)).Normalize(center);
                else if (_snapYZ)
                    normal = (bone._frameMatrix * new Vector3(1.0f, 0.0f, 0.0f)).Normalize(center);
                else if (_snapCirc)
                {
                    radius *= _axisScale;
                    normal = camera.Normalize(center);
                }
                else 
                if (Maths.LineSphereIntersect(lineStart, lineEnd, center, radius, out point))
                    return true;
                else
                    normal = camera.Normalize(center);
            }

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
        public void CreateUndo(object sender, EventArgs e)
        {
            CreateUndo(pnlAnim.SelectedBone);
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
            if (undoSaves.Count > 50)
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
                pnlAnim.SelectedBone = undoSaves[saveIndex - 1].bone;
                redoSave.bone = pnlAnim.SelectedBone;
                redoSave.frameState = pnlAnim.SelectedBone._frameState;
                redoSaves.Add(redoSave);

                chr0Editor.Undo(undoSaves[--saveIndex]); //Apply Undo

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
                pnlAnim.SelectedBone = redoSaves[undoSaves.Count - saveIndex - 1].bone;
                chr0Editor.Undo(redoSaves[undoSaves.Count - saveIndex++ - 1]);
                redoSaves.RemoveAt(redoSaves.Count - 1);

                if (saveIndex >= undoSaves.Count) { redoToolStripMenuItem.Enabled = false; saveIndex--; }
                if (!firstUndo) firstUndo = true;

                undoToolStripMenuItem.Enabled = true;
            }
        }
        #endregion

        private void pnlPlayback_Resize(object sender, EventArgs e)
        {
            if (pnlPlayback.Width <= pnlPlayback.MinimumSize.Width)
            {
                pnlPlayback.Dock = DockStyle.Left;
                pnlPlayback.Width = pnlPlayback.MinimumSize.Width;
            }
            else
                pnlPlayback.Dock = DockStyle.Fill;
        }

        bool addedHeight = false;
        private void ModelEditControl_SizeChanged(object sender, EventArgs e)
        {
            checkDimensions();
        }

        public void checkDimensions()
        {
            if (pnlPlayback.Width <= pnlPlayback.MinimumSize.Width)
            {
                pnlPlayback.Dock = DockStyle.Left;
                pnlPlayback.Width = pnlPlayback.MinimumSize.Width;
            }
            else
                pnlPlayback.Dock = DockStyle.Fill;

            if (_updating)
                return;

            if (animEditors.Width - panel3.Width - splitter2.Width >= pnlPlayback.MinimumSize.Width)
            {
                pnlPlayback.Width += animEditors.Width - panel3.Width - splitter2.Width - pnlPlayback.MinimumSize.Width;
                pnlPlayback.Dock = DockStyle.Fill;
            }
            else pnlPlayback.Dock = DockStyle.Left;

            if (panel3.Width + splitter2.Width + pnlPlayback.Width <= animEditors.Width)
            {
                if (addedHeight)
                {
                    _updating = true;
                    animEditors.Height -= 17;
                    _updating = false;
                    animEditors.HorizontalScroll.Visible = addedHeight = false;
                }
            }
            else
            {
                if (!addedHeight)
                {
                    _updating = true;
                    animEditors.Height += 17;
                    _updating = false;
                    animEditors.HorizontalScroll.Visible = addedHeight = true;
                }
            }
        }

        private void btnLoadMoveset_Click(object sender, EventArgs e)
        {
            if (btnLoadMoveset.Text == "Load")
            {
                if (pnlMoveset.LoadMoveset())
                    btnLoadMoveset.Text = "Close";
            }
            else
            {
                if (pnlMoveset.CloseMoveset())
                    btnLoadMoveset.Text = "Load";
            }
        }

        private void btnSaveMoveset_Click(object sender, EventArgs e)
        {
            pnlMoveset.SaveMoveset();
        }

        private void btnLoadCmnMoveset_Click(object sender, EventArgs e)
        {
            if (btnLoadMoveset.Text == "Load")
            {

            }
            else
            {

            }
        }

        private void btnSaveCmnMoveset_Click(object sender, EventArgs e)
        {

        }

        private void btnLoadSound_Click(object sender, EventArgs e)
        {
            if (btnLoadSound.Text == "Load")
            {

            }
            else
            {

            }
        }

        private void btnLoadCmnEffects_Click(object sender, EventArgs e)
        {
            if (btnLoadCmnEffects.Text == "Load")
            {

            }
            else
            {

            }
        }

        private void btnLoadEffects_Click(object sender, EventArgs e)
        {
            if (btnLoadEffects.Text == "Load")
            {

            }
            else
            {

            }
        }
    }

    public class TransparentPanel : Panel
    {
        public TransparentPanel() { this.SetStyle(ControlStyles.UserPaint, true); }

        bool _transparent = true;

        protected override CreateParams CreateParams
        {
            get
            {
                if (_transparent)
                {
                    CreateParams createParams = base.CreateParams;
                    createParams.ExStyle |= 0x00000020; // WS_EX_TRANSPARENT
                    return createParams;
                }
                else return base.CreateParams;
            }
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == (int)0x84)
                m.Result = (IntPtr)(-1);
            else
                base.WndProc(ref m);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            if (_transparent)
            {
                // Do not paint background.
            }
            else
            {
                base.OnPaintBackground(e);
            }
        }
    }
}
