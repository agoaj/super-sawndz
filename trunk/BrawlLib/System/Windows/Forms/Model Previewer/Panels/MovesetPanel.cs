using System;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.Wii.Animations;
using System.Drawing;
using BrawlLib.Modeling;
using System.IO;
using System.ComponentModel;
using BrawlLib;
using System.Collections.Generic;
using BrawlLib.Wii.Models;
using BrawlLib.SSBBTypes;

namespace System.Windows.Forms
{
    public class ModelMovesetPanel : UserControl
    {
        public delegate void ReferenceEventHandler(ResourceNode node);

        #region Designer

        private GroupBox grpHurtBox;
        private Label OffsetX;
        internal NumericInputBox numOffX;
        private Label OffsetY;
        private NumericInputBox numRadius;
        private Label OffsetZ;
        internal NumericInputBox numStrZ;
        private Label StretchX;
        internal NumericInputBox numStrY;
        private Label StretchY;
        internal NumericInputBox numStrX;
        private Label StretchZ;
        internal NumericInputBox numOffZ;
        private Label BoxRadius;
        internal NumericInputBox numOffY;
        private Label BoxZone;
        private OpenFileDialog dlgOpen;
        private ContextMenuStrip ctxBox;
        private ToolStripMenuItem add;
        private ToolStripMenuItem subtract;
        private ContextMenuStrip ctxAnim;
        private ToolStripMenuItem sourceToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem1;
        private ToolStripMenuItem exportToolStripMenuItem;
        private ToolStripMenuItem replaceToolStripMenuItem;
        private ToolStripMenuItem portToolStripMenuItem;
        private SaveFileDialog dlgSave;
        private IContainer components;
        private ToolStripMenuItem toolStripMenuItem3;
        private ToolStripMenuItem toolStripMenuItem4;
        private ToolStripMenuItem toolStripMenuItem5;
        private ToolStripMenuItem toolStripMenuItem6;
        private ToolStripMenuItem toolStripMenuItem7;
        private ToolStripMenuItem toolStripMenuItem8;
        private ToolStripMenuItem Source;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem removeAllToolStripMenuItem;
        private ToolStripMenuItem addCustomAmountToolStripMenuItem;
        private ComboBox SelectedBone;
        private ComboBox SelectedZone;
        private CheckBox BoxEnabled;
        private NumericInputBox numRegion;
        private Label BoxRegion;
        private Panel ControlPanel;
        private Panel ItemsPanel;
        private TabControl ItemSwitcher;
        private TabPage ActionsTab;
        private TabPage SubActionsTab;
        private TabPage AttributesTab;
        private TabControl AttributesTabGroup;
        private TabPage MainAttrTab;
        private TabPage SSEAttrTab;
        private TabPage CombatTab;
        private TabControl CombatTabGroup;
        private TabPage HurtboxTab;
        private CheckBox checkBox1;
        private ListBox ActionsList;
        private ListBox SubActionsList;
        private AttributeGrid attributeGridMain;
        private AttributeGrid attributeGridSSE;
        public ScriptEditor scriptEditor1;
        private Panel ActionEditor;
        private Splitter splitter1;
        private Panel SubActionFlagsPanel;
        private NumericInputBox inTransTime;
        private CheckBox chkNoOutTrans;
        private Label label1;
        private Button flagsToggle;
        private CheckBox chkMovesChar;
        private CheckBox chkTransOutStart;
        private CheckBox chkUnk;
        private CheckBox chkLoop;
        private CheckBox chkFixedTrans;
        private CheckBox chkFixedRot;
        private CheckBox chkFixedScale;
        private Panel panel2;
        private Button button1;
        private ComboBox comboBox1;
        public CheckedListBox lstHurtboxes;
        public Timer animTimer;
        private Panel ActionFlagsPanel;
        public EventModifier eventModifier1;
        private Label BoxBone;

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.grpHurtBox = new System.Windows.Forms.GroupBox();
            this.numRegion = new System.Windows.Forms.NumericInputBox();
            this.BoxRegion = new System.Windows.Forms.Label();
            this.BoxEnabled = new System.Windows.Forms.CheckBox();
            this.SelectedZone = new System.Windows.Forms.ComboBox();
            this.SelectedBone = new System.Windows.Forms.ComboBox();
            this.OffsetX = new System.Windows.Forms.Label();
            this.numOffX = new System.Windows.Forms.NumericInputBox();
            this.OffsetY = new System.Windows.Forms.Label();
            this.numRadius = new System.Windows.Forms.NumericInputBox();
            this.OffsetZ = new System.Windows.Forms.Label();
            this.numStrZ = new System.Windows.Forms.NumericInputBox();
            this.StretchX = new System.Windows.Forms.Label();
            this.numStrY = new System.Windows.Forms.NumericInputBox();
            this.StretchY = new System.Windows.Forms.Label();
            this.numStrX = new System.Windows.Forms.NumericInputBox();
            this.StretchZ = new System.Windows.Forms.Label();
            this.numOffZ = new System.Windows.Forms.NumericInputBox();
            this.BoxRadius = new System.Windows.Forms.Label();
            this.numOffY = new System.Windows.Forms.NumericInputBox();
            this.BoxZone = new System.Windows.Forms.Label();
            this.BoxBone = new System.Windows.Forms.Label();
            this.ctxAnim = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.sourceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.replaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.portToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxBox = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.Source = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.add = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripMenuItem();
            this.subtract = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem8 = new System.Windows.Forms.ToolStripMenuItem();
            this.removeAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addCustomAmountToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dlgOpen = new System.Windows.Forms.OpenFileDialog();
            this.dlgSave = new System.Windows.Forms.SaveFileDialog();
            this.ControlPanel = new System.Windows.Forms.Panel();
            this.eventModifier1 = new System.Windows.Forms.EventModifier();
            this.ActionEditor = new System.Windows.Forms.Panel();
            this.scriptEditor1 = new System.Windows.Forms.ScriptEditor();
            this.ActionFlagsPanel = new System.Windows.Forms.Panel();
            this.SubActionFlagsPanel = new System.Windows.Forms.Panel();
            this.chkUnk = new System.Windows.Forms.CheckBox();
            this.chkLoop = new System.Windows.Forms.CheckBox();
            this.chkFixedTrans = new System.Windows.Forms.CheckBox();
            this.chkFixedRot = new System.Windows.Forms.CheckBox();
            this.chkFixedScale = new System.Windows.Forms.CheckBox();
            this.chkMovesChar = new System.Windows.Forms.CheckBox();
            this.chkTransOutStart = new System.Windows.Forms.CheckBox();
            this.inTransTime = new System.Windows.Forms.NumericInputBox();
            this.chkNoOutTrans = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.flagsToggle = new System.Windows.Forms.Button();
            this.ItemsPanel = new System.Windows.Forms.Panel();
            this.ItemSwitcher = new System.Windows.Forms.TabControl();
            this.ActionsTab = new System.Windows.Forms.TabPage();
            this.ActionsList = new System.Windows.Forms.ListBox();
            this.SubActionsTab = new System.Windows.Forms.TabPage();
            this.SubActionsList = new System.Windows.Forms.ListBox();
            this.AttributesTab = new System.Windows.Forms.TabPage();
            this.AttributesTabGroup = new System.Windows.Forms.TabControl();
            this.MainAttrTab = new System.Windows.Forms.TabPage();
            this.attributeGridMain = new System.Windows.Forms.AttributeGrid();
            this.SSEAttrTab = new System.Windows.Forms.TabPage();
            this.attributeGridSSE = new System.Windows.Forms.AttributeGrid();
            this.CombatTab = new System.Windows.Forms.TabPage();
            this.CombatTabGroup = new System.Windows.Forms.TabControl();
            this.HurtboxTab = new System.Windows.Forms.TabPage();
            this.lstHurtboxes = new System.Windows.Forms.CheckedListBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.animTimer = new System.Windows.Forms.Timer(this.components);
            this.grpHurtBox.SuspendLayout();
            this.ControlPanel.SuspendLayout();
            this.ActionEditor.SuspendLayout();
            this.SubActionFlagsPanel.SuspendLayout();
            this.panel2.SuspendLayout();
            this.ItemsPanel.SuspendLayout();
            this.ItemSwitcher.SuspendLayout();
            this.ActionsTab.SuspendLayout();
            this.SubActionsTab.SuspendLayout();
            this.AttributesTab.SuspendLayout();
            this.AttributesTabGroup.SuspendLayout();
            this.MainAttrTab.SuspendLayout();
            this.SSEAttrTab.SuspendLayout();
            this.CombatTab.SuspendLayout();
            this.CombatTabGroup.SuspendLayout();
            this.HurtboxTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpHurtBox
            // 
            this.grpHurtBox.Controls.Add(this.numRegion);
            this.grpHurtBox.Controls.Add(this.BoxRegion);
            this.grpHurtBox.Controls.Add(this.BoxEnabled);
            this.grpHurtBox.Controls.Add(this.SelectedZone);
            this.grpHurtBox.Controls.Add(this.SelectedBone);
            this.grpHurtBox.Controls.Add(this.OffsetX);
            this.grpHurtBox.Controls.Add(this.numOffX);
            this.grpHurtBox.Controls.Add(this.OffsetY);
            this.grpHurtBox.Controls.Add(this.numRadius);
            this.grpHurtBox.Controls.Add(this.OffsetZ);
            this.grpHurtBox.Controls.Add(this.numStrZ);
            this.grpHurtBox.Controls.Add(this.StretchX);
            this.grpHurtBox.Controls.Add(this.numStrY);
            this.grpHurtBox.Controls.Add(this.StretchY);
            this.grpHurtBox.Controls.Add(this.numStrX);
            this.grpHurtBox.Controls.Add(this.StretchZ);
            this.grpHurtBox.Controls.Add(this.numOffZ);
            this.grpHurtBox.Controls.Add(this.BoxRadius);
            this.grpHurtBox.Controls.Add(this.numOffY);
            this.grpHurtBox.Controls.Add(this.BoxZone);
            this.grpHurtBox.Controls.Add(this.BoxBone);
            this.grpHurtBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpHurtBox.Location = new System.Drawing.Point(0, 0);
            this.grpHurtBox.Name = "grpHurtBox";
            this.grpHurtBox.Size = new System.Drawing.Size(229, 375);
            this.grpHurtBox.TabIndex = 22;
            this.grpHurtBox.TabStop = false;
            this.grpHurtBox.Text = "Edit Hurtbox";
            this.grpHurtBox.Visible = false;
            // 
            // numRegion
            // 
            this.numRegion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.numRegion.Location = new System.Drawing.Point(69, 171);
            this.numRegion.Name = "numRegion";
            this.numRegion.Size = new System.Drawing.Size(155, 20);
            this.numRegion.TabIndex = 25;
            this.numRegion.Text = "0";
            this.numRegion.TextChanged += new System.EventHandler(this.numRegion_TextChanged);
            // 
            // BoxRegion
            // 
            this.BoxRegion.Location = new System.Drawing.Point(-10, 170);
            this.BoxRegion.Name = "BoxRegion";
            this.BoxRegion.Size = new System.Drawing.Size(74, 20);
            this.BoxRegion.TabIndex = 24;
            this.BoxRegion.Text = "Region:";
            this.BoxRegion.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // BoxEnabled
            // 
            this.BoxEnabled.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BoxEnabled.AutoSize = true;
            this.BoxEnabled.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BoxEnabled.Location = new System.Drawing.Point(158, 13);
            this.BoxEnabled.Name = "BoxEnabled";
            this.BoxEnabled.Size = new System.Drawing.Size(65, 17);
            this.BoxEnabled.TabIndex = 23;
            this.BoxEnabled.Text = "Enabled";
            this.BoxEnabled.UseVisualStyleBackColor = true;
            this.BoxEnabled.CheckedChanged += new System.EventHandler(this.BoxEnabled_CheckedChanged);
            // 
            // SelectedZone
            // 
            this.SelectedZone.FormattingEnabled = true;
            this.SelectedZone.Location = new System.Drawing.Point(70, 232);
            this.SelectedZone.Name = "SelectedZone";
            this.SelectedZone.Size = new System.Drawing.Size(126, 21);
            this.SelectedZone.TabIndex = 22;
            this.SelectedZone.Tag = "";
            this.SelectedZone.SelectedIndexChanged += new System.EventHandler(this.SelectedZone_SelectedIndexChanged);
            // 
            // SelectedBone
            // 
            this.SelectedBone.FormattingEnabled = true;
            this.SelectedBone.Location = new System.Drawing.Point(70, 211);
            this.SelectedBone.Name = "SelectedBone";
            this.SelectedBone.Size = new System.Drawing.Size(126, 21);
            this.SelectedBone.TabIndex = 21;
            this.SelectedBone.Tag = "";
            this.SelectedBone.SelectedIndexChanged += new System.EventHandler(this.SelectedBone_SelectedIndexChanged);
            // 
            // OffsetX
            // 
            this.OffsetX.Location = new System.Drawing.Point(-10, 34);
            this.OffsetX.Name = "OffsetX";
            this.OffsetX.Size = new System.Drawing.Size(74, 20);
            this.OffsetX.TabIndex = 4;
            this.OffsetX.Text = "Offset X:";
            this.OffsetX.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numOffX
            // 
            this.numOffX.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.numOffX.Location = new System.Drawing.Point(69, 35);
            this.numOffX.Name = "numOffX";
            this.numOffX.Size = new System.Drawing.Size(155, 20);
            this.numOffX.TabIndex = 3;
            this.numOffX.Text = "0";
            this.numOffX.ValueChanged += new System.EventHandler(this.BoxChanged);
            this.numOffX.TextChanged += new System.EventHandler(this.numOffX_TextChanged);
            // 
            // OffsetY
            // 
            this.OffsetY.Location = new System.Drawing.Point(-10, 54);
            this.OffsetY.Name = "OffsetY";
            this.OffsetY.Size = new System.Drawing.Size(74, 20);
            this.OffsetY.TabIndex = 5;
            this.OffsetY.Text = "Offset Y:";
            this.OffsetY.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numRadius
            // 
            this.numRadius.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.numRadius.Location = new System.Drawing.Point(70, 191);
            this.numRadius.Name = "numRadius";
            this.numRadius.Size = new System.Drawing.Size(154, 20);
            this.numRadius.TabIndex = 18;
            this.numRadius.Text = "0";
            this.numRadius.ValueChanged += new System.EventHandler(this.BoxChanged);
            this.numRadius.TextChanged += new System.EventHandler(this.numRadius_TextChanged);
            // 
            // OffsetZ
            // 
            this.OffsetZ.Location = new System.Drawing.Point(-11, 74);
            this.OffsetZ.Name = "OffsetZ";
            this.OffsetZ.Size = new System.Drawing.Size(74, 20);
            this.OffsetZ.TabIndex = 6;
            this.OffsetZ.Text = "Offset Z:";
            this.OffsetZ.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numStrZ
            // 
            this.numStrZ.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.numStrZ.Location = new System.Drawing.Point(69, 143);
            this.numStrZ.Name = "numStrZ";
            this.numStrZ.Size = new System.Drawing.Size(155, 20);
            this.numStrZ.TabIndex = 17;
            this.numStrZ.Text = "0";
            this.numStrZ.ValueChanged += new System.EventHandler(this.BoxChanged);
            this.numStrZ.TextChanged += new System.EventHandler(this.numStrZ_TextChanged);
            // 
            // StretchX
            // 
            this.StretchX.Location = new System.Drawing.Point(-10, 102);
            this.StretchX.Name = "StretchX";
            this.StretchX.Size = new System.Drawing.Size(74, 20);
            this.StretchX.TabIndex = 7;
            this.StretchX.Text = "Stretch X:";
            this.StretchX.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numStrY
            // 
            this.numStrY.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.numStrY.Location = new System.Drawing.Point(69, 123);
            this.numStrY.Name = "numStrY";
            this.numStrY.Size = new System.Drawing.Size(155, 20);
            this.numStrY.TabIndex = 16;
            this.numStrY.Text = "0";
            this.numStrY.ValueChanged += new System.EventHandler(this.BoxChanged);
            this.numStrY.TextChanged += new System.EventHandler(this.numStrY_TextChanged);
            // 
            // StretchY
            // 
            this.StretchY.Location = new System.Drawing.Point(-10, 122);
            this.StretchY.Name = "StretchY";
            this.StretchY.Size = new System.Drawing.Size(74, 20);
            this.StretchY.TabIndex = 8;
            this.StretchY.Text = "Stretch Y:";
            this.StretchY.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numStrX
            // 
            this.numStrX.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.numStrX.Location = new System.Drawing.Point(69, 103);
            this.numStrX.Name = "numStrX";
            this.numStrX.Size = new System.Drawing.Size(155, 20);
            this.numStrX.TabIndex = 15;
            this.numStrX.Text = "0";
            this.numStrX.ValueChanged += new System.EventHandler(this.BoxChanged);
            this.numStrX.TextChanged += new System.EventHandler(this.numStrX_TextChanged);
            // 
            // StretchZ
            // 
            this.StretchZ.Location = new System.Drawing.Point(-10, 142);
            this.StretchZ.Name = "StretchZ";
            this.StretchZ.Size = new System.Drawing.Size(74, 20);
            this.StretchZ.TabIndex = 9;
            this.StretchZ.Text = "Stretch Z:";
            this.StretchZ.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numOffZ
            // 
            this.numOffZ.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.numOffZ.Location = new System.Drawing.Point(69, 75);
            this.numOffZ.Name = "numOffZ";
            this.numOffZ.Size = new System.Drawing.Size(155, 20);
            this.numOffZ.TabIndex = 14;
            this.numOffZ.Text = "0";
            this.numOffZ.ValueChanged += new System.EventHandler(this.BoxChanged);
            this.numOffZ.TextChanged += new System.EventHandler(this.numOffZ_TextChanged);
            // 
            // BoxRadius
            // 
            this.BoxRadius.Location = new System.Drawing.Point(-10, 190);
            this.BoxRadius.Name = "BoxRadius";
            this.BoxRadius.Size = new System.Drawing.Size(74, 20);
            this.BoxRadius.TabIndex = 10;
            this.BoxRadius.Text = "Radius:";
            this.BoxRadius.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numOffY
            // 
            this.numOffY.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.numOffY.Location = new System.Drawing.Point(69, 55);
            this.numOffY.Name = "numOffY";
            this.numOffY.Size = new System.Drawing.Size(155, 20);
            this.numOffY.TabIndex = 13;
            this.numOffY.Text = "0";
            this.numOffY.ValueChanged += new System.EventHandler(this.BoxChanged);
            this.numOffY.TextChanged += new System.EventHandler(this.numOffY_TextChanged);
            // 
            // BoxZone
            // 
            this.BoxZone.Location = new System.Drawing.Point(-10, 231);
            this.BoxZone.Name = "BoxZone";
            this.BoxZone.Size = new System.Drawing.Size(74, 20);
            this.BoxZone.TabIndex = 11;
            this.BoxZone.Text = "Zone:";
            this.BoxZone.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // BoxBone
            // 
            this.BoxBone.Location = new System.Drawing.Point(-10, 210);
            this.BoxBone.Name = "BoxBone";
            this.BoxBone.Size = new System.Drawing.Size(74, 20);
            this.BoxBone.TabIndex = 12;
            this.BoxBone.Text = "Bone:";
            this.BoxBone.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ctxAnim
            // 
            this.ctxAnim.Name = "ctxAnim";
            this.ctxAnim.Size = new System.Drawing.Size(61, 4);
            // 
            // sourceToolStripMenuItem
            // 
            this.sourceToolStripMenuItem.Name = "sourceToolStripMenuItem";
            this.sourceToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(6, 6);
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // replaceToolStripMenuItem
            // 
            this.replaceToolStripMenuItem.Name = "replaceToolStripMenuItem";
            this.replaceToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // portToolStripMenuItem
            // 
            this.portToolStripMenuItem.Name = "portToolStripMenuItem";
            this.portToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // ctxBox
            // 
            this.ctxBox.Name = "ctxBox";
            this.ctxBox.Size = new System.Drawing.Size(61, 4);
            // 
            // Source
            // 
            this.Source.Name = "Source";
            this.Source.Size = new System.Drawing.Size(32, 19);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 6);
            // 
            // add
            // 
            this.add.Name = "add";
            this.add.Size = new System.Drawing.Size(32, 19);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(32, 19);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(32, 19);
            // 
            // toolStripMenuItem7
            // 
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            this.toolStripMenuItem7.Size = new System.Drawing.Size(32, 19);
            // 
            // subtract
            // 
            this.subtract.Name = "subtract";
            this.subtract.Size = new System.Drawing.Size(32, 19);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(32, 19);
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(32, 19);
            // 
            // toolStripMenuItem8
            // 
            this.toolStripMenuItem8.Name = "toolStripMenuItem8";
            this.toolStripMenuItem8.Size = new System.Drawing.Size(32, 19);
            // 
            // removeAllToolStripMenuItem
            // 
            this.removeAllToolStripMenuItem.Name = "removeAllToolStripMenuItem";
            this.removeAllToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // addCustomAmountToolStripMenuItem
            // 
            this.addCustomAmountToolStripMenuItem.Name = "addCustomAmountToolStripMenuItem";
            this.addCustomAmountToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // ControlPanel
            // 
            this.ControlPanel.Controls.Add(this.eventModifier1);
            this.ControlPanel.Controls.Add(this.ActionEditor);
            this.ControlPanel.Controls.Add(this.grpHurtBox);
            this.ControlPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ControlPanel.Location = new System.Drawing.Point(0, 240);
            this.ControlPanel.Name = "ControlPanel";
            this.ControlPanel.Size = new System.Drawing.Size(229, 375);
            this.ControlPanel.TabIndex = 26;
            this.ControlPanel.Visible = false;
            // 
            // eventModifier1
            // 
            this.eventModifier1.AutoSize = true;
            this.eventModifier1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.eventModifier1.Location = new System.Drawing.Point(0, 0);
            this.eventModifier1.Name = "eventModifier1";
            this.eventModifier1.Size = new System.Drawing.Size(229, 375);
            this.eventModifier1.TabIndex = 37;
            this.eventModifier1.Visible = false;
            // 
            // ActionEditor
            // 
            this.ActionEditor.Controls.Add(this.scriptEditor1);
            this.ActionEditor.Controls.Add(this.ActionFlagsPanel);
            this.ActionEditor.Controls.Add(this.SubActionFlagsPanel);
            this.ActionEditor.Controls.Add(this.panel2);
            this.ActionEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ActionEditor.Location = new System.Drawing.Point(0, 0);
            this.ActionEditor.Name = "ActionEditor";
            this.ActionEditor.Size = new System.Drawing.Size(229, 375);
            this.ActionEditor.TabIndex = 26;
            this.ActionEditor.Visible = false;
            // 
            // scriptEditor1
            // 
            this.scriptEditor1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scriptEditor1.Location = new System.Drawing.Point(0, 21);
            this.scriptEditor1.Name = "scriptEditor1";
            this.scriptEditor1.Padding = new System.Windows.Forms.Padding(1);
            this.scriptEditor1.Size = new System.Drawing.Size(229, 354);
            this.scriptEditor1.TabIndex = 26;
            // 
            // ActionFlagsPanel
            // 
            this.ActionFlagsPanel.Location = new System.Drawing.Point(0, 168);
            this.ActionFlagsPanel.Margin = new System.Windows.Forms.Padding(0);
            this.ActionFlagsPanel.Name = "ActionFlagsPanel";
            this.ActionFlagsPanel.Size = new System.Drawing.Size(201, 147);
            this.ActionFlagsPanel.TabIndex = 37;
            this.ActionFlagsPanel.Visible = false;
            // 
            // SubActionFlagsPanel
            // 
            this.SubActionFlagsPanel.Controls.Add(this.chkUnk);
            this.SubActionFlagsPanel.Controls.Add(this.chkLoop);
            this.SubActionFlagsPanel.Controls.Add(this.chkFixedTrans);
            this.SubActionFlagsPanel.Controls.Add(this.chkFixedRot);
            this.SubActionFlagsPanel.Controls.Add(this.chkFixedScale);
            this.SubActionFlagsPanel.Controls.Add(this.chkMovesChar);
            this.SubActionFlagsPanel.Controls.Add(this.chkTransOutStart);
            this.SubActionFlagsPanel.Controls.Add(this.inTransTime);
            this.SubActionFlagsPanel.Controls.Add(this.chkNoOutTrans);
            this.SubActionFlagsPanel.Controls.Add(this.label1);
            this.SubActionFlagsPanel.Location = new System.Drawing.Point(0, 21);
            this.SubActionFlagsPanel.Margin = new System.Windows.Forms.Padding(0);
            this.SubActionFlagsPanel.Name = "SubActionFlagsPanel";
            this.SubActionFlagsPanel.Size = new System.Drawing.Size(201, 147);
            this.SubActionFlagsPanel.TabIndex = 27;
            this.SubActionFlagsPanel.Visible = false;
            // 
            // chkUnk
            // 
            this.chkUnk.AutoSize = true;
            this.chkUnk.Location = new System.Drawing.Point(63, 125);
            this.chkUnk.Name = "chkUnk";
            this.chkUnk.Size = new System.Drawing.Size(72, 17);
            this.chkUnk.TabIndex = 36;
            this.chkUnk.Text = "Unknown";
            this.chkUnk.UseVisualStyleBackColor = true;
            // 
            // chkLoop
            // 
            this.chkLoop.AutoSize = true;
            this.chkLoop.Location = new System.Drawing.Point(7, 125);
            this.chkLoop.Name = "chkLoop";
            this.chkLoop.Size = new System.Drawing.Size(50, 17);
            this.chkLoop.TabIndex = 35;
            this.chkLoop.Text = "Loop";
            this.chkLoop.UseVisualStyleBackColor = true;
            // 
            // chkFixedTrans
            // 
            this.chkFixedTrans.AutoSize = true;
            this.chkFixedTrans.Location = new System.Drawing.Point(7, 108);
            this.chkFixedTrans.Name = "chkFixedTrans";
            this.chkFixedTrans.Size = new System.Drawing.Size(106, 17);
            this.chkFixedTrans.TabIndex = 34;
            this.chkFixedTrans.Text = "Fixed Translation";
            this.chkFixedTrans.UseVisualStyleBackColor = true;
            // 
            // chkFixedRot
            // 
            this.chkFixedRot.AutoSize = true;
            this.chkFixedRot.Location = new System.Drawing.Point(7, 91);
            this.chkFixedRot.Name = "chkFixedRot";
            this.chkFixedRot.Size = new System.Drawing.Size(94, 17);
            this.chkFixedRot.TabIndex = 33;
            this.chkFixedRot.Text = "Fixed Rotation";
            this.chkFixedRot.UseVisualStyleBackColor = true;
            // 
            // chkFixedScale
            // 
            this.chkFixedScale.AutoSize = true;
            this.chkFixedScale.Location = new System.Drawing.Point(7, 74);
            this.chkFixedScale.Name = "chkFixedScale";
            this.chkFixedScale.Size = new System.Drawing.Size(81, 17);
            this.chkFixedScale.TabIndex = 32;
            this.chkFixedScale.Text = "Fixed Scale";
            this.chkFixedScale.UseVisualStyleBackColor = true;
            // 
            // chkMovesChar
            // 
            this.chkMovesChar.AutoSize = true;
            this.chkMovesChar.Location = new System.Drawing.Point(7, 57);
            this.chkMovesChar.Name = "chkMovesChar";
            this.chkMovesChar.Size = new System.Drawing.Size(107, 17);
            this.chkMovesChar.TabIndex = 31;
            this.chkMovesChar.Text = "Moves Character";
            this.chkMovesChar.UseVisualStyleBackColor = true;
            // 
            // chkTransOutStart
            // 
            this.chkTransOutStart.AutoSize = true;
            this.chkTransOutStart.Location = new System.Drawing.Point(7, 40);
            this.chkTransOutStart.Name = "chkTransOutStart";
            this.chkTransOutStart.Size = new System.Drawing.Size(143, 17);
            this.chkTransOutStart.TabIndex = 30;
            this.chkTransOutStart.Text = "Transition Out From Start";
            this.chkTransOutStart.UseVisualStyleBackColor = true;
            // 
            // inTransTime
            // 
            this.inTransTime.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.inTransTime.Location = new System.Drawing.Point(107, 4);
            this.inTransTime.Name = "inTransTime";
            this.inTransTime.Size = new System.Drawing.Size(89, 20);
            this.inTransTime.TabIndex = 29;
            this.inTransTime.Text = "0";
            // 
            // chkNoOutTrans
            // 
            this.chkNoOutTrans.AutoSize = true;
            this.chkNoOutTrans.Location = new System.Drawing.Point(7, 24);
            this.chkNoOutTrans.Name = "chkNoOutTrans";
            this.chkNoOutTrans.Size = new System.Drawing.Size(109, 17);
            this.chkNoOutTrans.TabIndex = 2;
            this.chkNoOutTrans.Text = "No Out Transition";
            this.chkNoOutTrans.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "In Translation Time:";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.button1);
            this.panel2.Controls.Add(this.comboBox1);
            this.panel2.Controls.Add(this.flagsToggle);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(229, 21);
            this.panel2.TabIndex = 37;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(101, -1);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(128, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Run Script";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Main",
            "GFX",
            "SFX",
            "Other"});
            this.comboBox1.Location = new System.Drawing.Point(47, 0);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(54, 21);
            this.comboBox1.TabIndex = 1;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // flagsToggle
            // 
            this.flagsToggle.Cursor = System.Windows.Forms.Cursors.Default;
            this.flagsToggle.Location = new System.Drawing.Point(0, -1);
            this.flagsToggle.Name = "flagsToggle";
            this.flagsToggle.Size = new System.Drawing.Size(47, 23);
            this.flagsToggle.TabIndex = 0;
            this.flagsToggle.Text = "Flags";
            this.flagsToggle.UseVisualStyleBackColor = true;
            this.flagsToggle.Click += new System.EventHandler(this.flagsToggle_Click);
            // 
            // ItemsPanel
            // 
            this.ItemsPanel.Controls.Add(this.ItemSwitcher);
            this.ItemsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ItemsPanel.Location = new System.Drawing.Point(0, 0);
            this.ItemsPanel.Name = "ItemsPanel";
            this.ItemsPanel.Size = new System.Drawing.Size(229, 237);
            this.ItemsPanel.TabIndex = 27;
            // 
            // ItemSwitcher
            // 
            this.ItemSwitcher.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.ItemSwitcher.Controls.Add(this.ActionsTab);
            this.ItemSwitcher.Controls.Add(this.SubActionsTab);
            this.ItemSwitcher.Controls.Add(this.AttributesTab);
            this.ItemSwitcher.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ItemSwitcher.Enabled = false;
            this.ItemSwitcher.Location = new System.Drawing.Point(0, 0);
            this.ItemSwitcher.Name = "ItemSwitcher";
            this.ItemSwitcher.Padding = new System.Drawing.Point(0, 0);
            this.ItemSwitcher.SelectedIndex = 0;
            this.ItemSwitcher.Size = new System.Drawing.Size(229, 237);
            this.ItemSwitcher.TabIndex = 0;
            this.ItemSwitcher.SelectedIndexChanged += new System.EventHandler(this.ItemSwitcher_SelectedIndexChanged);
            // 
            // ActionsTab
            // 
            this.ActionsTab.Controls.Add(this.ActionsList);
            this.ActionsTab.Location = new System.Drawing.Point(4, 25);
            this.ActionsTab.Name = "ActionsTab";
            this.ActionsTab.Size = new System.Drawing.Size(221, 208);
            this.ActionsTab.TabIndex = 0;
            this.ActionsTab.Text = "Actions";
            this.ActionsTab.UseVisualStyleBackColor = true;
            // 
            // ActionsList
            // 
            this.ActionsList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ActionsList.FormattingEnabled = true;
            this.ActionsList.Location = new System.Drawing.Point(0, 0);
            this.ActionsList.Name = "ActionsList";
            this.ActionsList.Size = new System.Drawing.Size(221, 208);
            this.ActionsList.TabIndex = 0;
            this.ActionsList.SelectedIndexChanged += new System.EventHandler(this.ActionsList_SelectedIndexChanged);
            // 
            // SubActionsTab
            // 
            this.SubActionsTab.Controls.Add(this.SubActionsList);
            this.SubActionsTab.Location = new System.Drawing.Point(4, 25);
            this.SubActionsTab.Name = "SubActionsTab";
            this.SubActionsTab.Size = new System.Drawing.Size(221, 208);
            this.SubActionsTab.TabIndex = 1;
            this.SubActionsTab.Text = "SubActions";
            this.SubActionsTab.UseVisualStyleBackColor = true;
            // 
            // SubActionsList
            // 
            this.SubActionsList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SubActionsList.FormattingEnabled = true;
            this.SubActionsList.Location = new System.Drawing.Point(0, 0);
            this.SubActionsList.Name = "SubActionsList";
            this.SubActionsList.Size = new System.Drawing.Size(221, 208);
            this.SubActionsList.TabIndex = 1;
            this.SubActionsList.SelectedIndexChanged += new System.EventHandler(this.SubActionsList_SelectedIndexChanged);
            // 
            // AttributesTab
            // 
            this.AttributesTab.Controls.Add(this.AttributesTabGroup);
            this.AttributesTab.Location = new System.Drawing.Point(4, 25);
            this.AttributesTab.Name = "AttributesTab";
            this.AttributesTab.Size = new System.Drawing.Size(221, 208);
            this.AttributesTab.TabIndex = 2;
            this.AttributesTab.Text = "Attributes";
            this.AttributesTab.UseVisualStyleBackColor = true;
            // 
            // AttributesTabGroup
            // 
            this.AttributesTabGroup.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.AttributesTabGroup.Controls.Add(this.MainAttrTab);
            this.AttributesTabGroup.Controls.Add(this.SSEAttrTab);
            this.AttributesTabGroup.Controls.Add(this.CombatTab);
            this.AttributesTabGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AttributesTabGroup.Location = new System.Drawing.Point(0, 0);
            this.AttributesTabGroup.Margin = new System.Windows.Forms.Padding(0);
            this.AttributesTabGroup.Name = "AttributesTabGroup";
            this.AttributesTabGroup.Padding = new System.Drawing.Point(0, 0);
            this.AttributesTabGroup.SelectedIndex = 0;
            this.AttributesTabGroup.Size = new System.Drawing.Size(221, 208);
            this.AttributesTabGroup.TabIndex = 0;
            this.AttributesTabGroup.SelectedIndexChanged += new System.EventHandler(this.AttributesTabGroup_SelectedIndexChanged);
            // 
            // MainAttrTab
            // 
            this.MainAttrTab.Controls.Add(this.attributeGridMain);
            this.MainAttrTab.Location = new System.Drawing.Point(4, 4);
            this.MainAttrTab.Name = "MainAttrTab";
            this.MainAttrTab.Size = new System.Drawing.Size(213, 182);
            this.MainAttrTab.TabIndex = 0;
            this.MainAttrTab.Text = "Main";
            this.MainAttrTab.UseVisualStyleBackColor = true;
            // 
            // attributeGridMain
            // 
            this.attributeGridMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.attributeGridMain.Location = new System.Drawing.Point(0, 0);
            this.attributeGridMain.Margin = new System.Windows.Forms.Padding(0);
            this.attributeGridMain.Name = "attributeGridMain";
            this.attributeGridMain.Size = new System.Drawing.Size(213, 182);
            this.attributeGridMain.TabIndex = 0;
            // 
            // SSEAttrTab
            // 
            this.SSEAttrTab.Controls.Add(this.attributeGridSSE);
            this.SSEAttrTab.Location = new System.Drawing.Point(4, 4);
            this.SSEAttrTab.Name = "SSEAttrTab";
            this.SSEAttrTab.Size = new System.Drawing.Size(213, 182);
            this.SSEAttrTab.TabIndex = 1;
            this.SSEAttrTab.Text = "SSE";
            this.SSEAttrTab.UseVisualStyleBackColor = true;
            // 
            // attributeGridSSE
            // 
            this.attributeGridSSE.Dock = System.Windows.Forms.DockStyle.Fill;
            this.attributeGridSSE.Location = new System.Drawing.Point(0, 0);
            this.attributeGridSSE.Name = "attributeGridSSE";
            this.attributeGridSSE.Size = new System.Drawing.Size(213, 182);
            this.attributeGridSSE.TabIndex = 0;
            // 
            // CombatTab
            // 
            this.CombatTab.Controls.Add(this.CombatTabGroup);
            this.CombatTab.Location = new System.Drawing.Point(4, 4);
            this.CombatTab.Name = "CombatTab";
            this.CombatTab.Size = new System.Drawing.Size(213, 182);
            this.CombatTab.TabIndex = 2;
            this.CombatTab.Text = "Combat";
            this.CombatTab.UseVisualStyleBackColor = true;
            // 
            // CombatTabGroup
            // 
            this.CombatTabGroup.Controls.Add(this.HurtboxTab);
            this.CombatTabGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CombatTabGroup.Location = new System.Drawing.Point(0, 0);
            this.CombatTabGroup.Name = "CombatTabGroup";
            this.CombatTabGroup.SelectedIndex = 0;
            this.CombatTabGroup.Size = new System.Drawing.Size(213, 182);
            this.CombatTabGroup.TabIndex = 0;
            this.CombatTabGroup.SelectedIndexChanged += new System.EventHandler(this.CombatTabGroup_SelectedIndexChanged);
            // 
            // HurtboxTab
            // 
            this.HurtboxTab.Controls.Add(this.lstHurtboxes);
            this.HurtboxTab.Controls.Add(this.checkBox1);
            this.HurtboxTab.Location = new System.Drawing.Point(4, 22);
            this.HurtboxTab.Name = "HurtboxTab";
            this.HurtboxTab.Padding = new System.Windows.Forms.Padding(3);
            this.HurtboxTab.Size = new System.Drawing.Size(205, 156);
            this.HurtboxTab.TabIndex = 0;
            this.HurtboxTab.Text = "Hurtboxes";
            // 
            // lstHurtboxes
            // 
            this.lstHurtboxes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstHurtboxes.Location = new System.Drawing.Point(3, 20);
            this.lstHurtboxes.Name = "lstHurtboxes";
            this.lstHurtboxes.Size = new System.Drawing.Size(199, 133);
            this.lstHurtboxes.TabIndex = 1;
            this.lstHurtboxes.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.lstHurtboxes_ItemCheck);
            this.lstHurtboxes.SelectedIndexChanged += new System.EventHandler(this.lstHurtboxes_SelectedIndexChanged);
            this.lstHurtboxes.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lstHurtboxes_KeyDown);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.BackColor = System.Drawing.SystemColors.Control;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.checkBox1.Location = new System.Drawing.Point(3, 3);
            this.checkBox1.Margin = new System.Windows.Forms.Padding(0);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Padding = new System.Windows.Forms.Padding(1, 0, 0, 0);
            this.checkBox1.Size = new System.Drawing.Size(199, 17);
            this.checkBox1.TabIndex = 0;
            this.checkBox1.Text = "Display All";
            this.checkBox1.UseVisualStyleBackColor = false;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter1.Location = new System.Drawing.Point(0, 237);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(229, 3);
            this.splitter1.TabIndex = 27;
            this.splitter1.TabStop = false;
            // 
            // animTimer
            // 
            this.animTimer.Interval = 16;
            this.animTimer.Tick += new System.EventHandler(this.animTimer_Tick);
            // 
            // ModelMovesetPanel
            // 
            this.Controls.Add(this.ItemsPanel);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.ControlPanel);
            this.Name = "ModelMovesetPanel";
            this.Size = new System.Drawing.Size(229, 615);
            this.grpHurtBox.ResumeLayout(false);
            this.grpHurtBox.PerformLayout();
            this.ControlPanel.ResumeLayout(false);
            this.ControlPanel.PerformLayout();
            this.ActionEditor.ResumeLayout(false);
            this.SubActionFlagsPanel.ResumeLayout(false);
            this.SubActionFlagsPanel.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ItemsPanel.ResumeLayout(false);
            this.ItemSwitcher.ResumeLayout(false);
            this.ActionsTab.ResumeLayout(false);
            this.SubActionsTab.ResumeLayout(false);
            this.AttributesTab.ResumeLayout(false);
            this.AttributesTabGroup.ResumeLayout(false);
            this.MainAttrTab.ResumeLayout(false);
            this.SSEAttrTab.ResumeLayout(false);
            this.CombatTab.ResumeLayout(false);
            this.CombatTabGroup.ResumeLayout(false);
            this.HurtboxTab.ResumeLayout(false);
            this.HurtboxTab.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion

        public ModelEditControl _mainWindow;

        //Variable storage. Order: RA, LA, IC
        public int[][] BasicVars = new int[3][];
        public float[][] FloatVars = new float[3][];
        public bool[][] BitVars = new bool[3][];
       
        public event EventHandler FileChanged;
        public event ReferenceEventHandler ReferenceLoaded;
        public event ReferenceEventHandler ReferenceClosed;
        public event EventHandler EditorChanged;

        private object _transformObject = null;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object TransformObject
        {
            get { return _transformObject; }
            set { _transformObject = value; }
        }

        private object _selectedObject = null;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object SelectedObject
        {
            get { return _selectedObject; }
            set { _selectedObject = value; UpdateCurrentControl(); }
        }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MDL0Node TargetModel
        {
            get { return _mainWindow.TargetModel; }
            set { _mainWindow.TargetModel = value; UpdateModel(); }
        }

        internal NumericInputBox[] _hurtboxBoxes = new NumericInputBox[8];

        public ModelMovesetPanel() 
        {
            InitializeComponent();
            SubActionFlagsPanel.Dock = ActionFlagsPanel.Dock = DockStyle.Top;
            scriptEditor1.form = this;
            //scriptEditor1.label1.Visible = false;
            //scriptEditor1.Offset.Visible = false;
            _hurtboxBoxes[0] = numOffX; numOffX.Tag = 0;
            _hurtboxBoxes[1] = numOffY; numOffY.Tag = 1;
            _hurtboxBoxes[2] = numOffZ; numOffZ.Tag = 2;
            _hurtboxBoxes[3] = numStrX; numStrX.Tag = 3;
            _hurtboxBoxes[4] = numStrY; numStrY.Tag = 4;
            _hurtboxBoxes[5] = numStrZ; numStrZ.Tag = 5;
            _hurtboxBoxes[6] = numRadius; numRadius.Tag = 6;
            _hurtboxBoxes[7] = numRegion; numRegion.Tag = 7;
        }

        public bool CloseReferences()
        {
            return CloseMoveset();
        }

        private void UpdateModel()
        {
            if (TargetModel == null)
                return;

            UpdateCurrentControl();

            _mainWindow.modelPanel1.Invalidate();
        }
        
        Control currentControl = null;
        private void UpdateCurrentControl()
        {
            Control newControl = null;

            if (_selectedObject is MoveDefHurtBoxNode && level1Index == 2 && level2Index == 2 && level3Index == 0)
            {
                MoveDefHurtBoxNode node = (MoveDefHurtBoxNode)_selectedObject;

                newControl = grpHurtBox;

                numOffX.Text = node._offst._x.ToString();
                numOffY.Text = node._offst._y.ToString();
                numOffZ.Text = node._offst._z.ToString();

                numStrX.Text = node._stretch._x.ToString();
                numStrY.Text = node._stretch._y.ToString();
                numStrZ.Text = node._stretch._z.ToString();

                numRadius.Text = node._radius.ToString();
                numRegion.Text = node.Region.ToString();
                SelectedBone.SelectedIndex = node.BoneNode.BoneIndex;
                SelectedZone.SelectedIndex = (int)node.Zone;

                BoxEnabled.Checked = node.Enabled;
            }
            else if (_selectedObject is MoveDefActionGroupNode && level1Index == 0)
            {
                newControl = ActionEditor;
            }
            else if (_selectedObject is MoveDefSubActionGroupNode && level1Index == 1)
            {
                MoveDefSubActionGroupNode grp = _selectedObject as MoveDefSubActionGroupNode;
                newControl = ActionEditor;
                inTransTime.Value = grp._inTransTime;
                chkNoOutTrans.Checked = grp._flags.HasFlag(AnimationFlags.NoOutTransition);
                chkTransOutStart.Checked = grp._flags.HasFlag(AnimationFlags.TransitionOutFromStart);
                chkMovesChar.Checked = grp._flags.HasFlag(AnimationFlags.MovesCharacter);
                chkLoop.Checked = _mainWindow._loop = grp._flags.HasFlag(AnimationFlags.Loop);
                chkUnk.Checked = grp._flags.HasFlag(AnimationFlags.Unknown);
                chkFixedScale.Checked = grp._flags.HasFlag(AnimationFlags.FixedScale);
                chkFixedRot.Checked = grp._flags.HasFlag(AnimationFlags.FixedRotation);
                chkFixedTrans.Checked = grp._flags.HasFlag(AnimationFlags.FixedTranslation);
            }
            else if (_selectedObject is MoveDefEventNode && level1Index < 2)
            {
                newControl = eventModifier1;
                eventModifier1.origEvent = _selectedObject as MoveDefEventNode;
                eventModifier1.Setup(this);
            }

            if (currentControl != newControl)
            {
                if (currentControl != null)
                    currentControl.Visible = false;
                currentControl = newControl;
                if (currentControl != null)
                    currentControl.Visible = true;
            }

            if (ControlPanel.Visible != (currentControl != null))
                ControlPanel.Visible = (currentControl != null);
        }

        internal unsafe void BoxChanged(object sender, EventArgs e)
        {
            
        }

        public unsafe void ResetBox(int index)
        {
            
        }
        
        public MoveDefNode _mainMoveset;
        public MoveDefNode _cmnMoveset;
        public ARCNode _cmnEffects;
        public ARCNode _effects;
        public RSARNode _rsar;

        public void PopulateActions()
        {
            if (_cmnMoveset != null && _cmnMoveset._actions != null)
            {
                ActionsList.BeginUpdate();
                ActionsList.Items.Clear();

                foreach (ResourceNode n in _cmnMoveset._actions.Children)
                    ActionsList.Items.Add(n);

                ActionsList.EndUpdate();
            }

            if (_mainMoveset != null && _mainMoveset._actions != null)
            {
                ActionsList.BeginUpdate();
                ActionsList.Items.Clear();

                foreach (ResourceNode n in _mainMoveset._actions.Children)
                    ActionsList.Items.Add(n);

                ActionsList.EndUpdate();
            }
        }

        public void PopulateSubActions()
        {
            if (_mainMoveset != null && _mainMoveset._subActions != null)
            {
                SubActionsList.BeginUpdate();
                SubActionsList.Items.Clear();

                foreach (ResourceNode n in _mainMoveset._subActions.Children)
                    SubActionsList.Items.Add(n);

                SubActionsList.EndUpdate();
            }
        }

        public bool LoadMoveset()
        {
            dlgOpen.Filter = "Moveset File (*.pac)|*.pac";
            if (dlgOpen.ShowDialog() == DialogResult.OK)
            {
                ResourceNode node = null;
                try
                {
                    if ((node = NodeFactory.FromFile(null, dlgOpen.FileName)) != null)
                    {
                        if (!CloseMoveset())
                            return false;

                        if (node.Children[0] is MoveDefNode)
                            (_mainMoveset = (MoveDefNode)node.Children[0])._model = TargetModel;
                        else
                        {
                            MessageBox.Show(this, "Input file does not contain a moveset.");
                            return false;
                        }

                        _updating = true;

                        node = null;

                        _mainMoveset.Populate();

                        if (_mainMoveset.data.misc.hurtBoxes != null)
                        {
                            lstHurtboxes.BeginUpdate();
                            lstHurtboxes.Items.Clear();

                            foreach (ResourceNode n in _mainMoveset.data.misc.hurtBoxes.Children)
                                lstHurtboxes.Items.Add(n, true);

                            lstHurtboxes.EndUpdate();

                            SelectedBone.Items.AddRange(TargetModel._linker.BoneCache);
                            SelectedZone.Items.AddRange(Enum.GetNames(typeof(HurtBoxZone)));
                        }

                        PopulateActions();
                        PopulateSubActions();

                        if (_mainMoveset.data.attributes != null)
                            attributeGridMain.TargetNode = _mainMoveset.data.attributes;
                        if (_mainMoveset.data.sseAttributes != null)
                            attributeGridSSE.TargetNode = _mainMoveset.data.sseAttributes;

                        comboBox1.SelectedIndex = 0;

                        //if (ReferenceLoaded != null)
                        //    ReferenceLoaded(_externalNode);

                        _updating = false;

                        ItemSwitcher.Enabled = true;

                        if (FileChanged != null)
                            FileChanged(this, null);

                        return true;
                    }
                    else
                        MessageBox.Show(this, "Unable to recognize input file.");
                }
                //catch (Exception x) { MessageBox.Show(this, x.ToString()); _updating = false; }
                finally
                {
                    if (node != null)
                        node.Dispose();

                    _updating = false;
                }
            }
            return false;
        }
        public bool CloseMoveset()
        {
            if (_mainMoveset != null)
            {
                if (_mainMoveset.IsDirty)
                {
                    DialogResult res = MessageBox.Show(this, "You have made changes to an external file. Would you like to save those changes?", "Closing external file.", MessageBoxButtons.YesNoCancel);
                    if (((res == DialogResult.Yes) && (!SaveMoveset())) || (res == DialogResult.Cancel))
                        return false;
                }
                if (ReferenceClosed != null)
                    ReferenceClosed(_mainMoveset);

                _mainMoveset.Dispose();
                _mainMoveset = null;

                if (FileChanged != null)
                    FileChanged(this, null);

                //UpdateReferences();
            }

            ItemSwitcher.SelectedIndex = 0;
            ItemSwitcher.Enabled = false;
            return true;
        }
        public bool SaveMoveset()
        {
            if ((_mainMoveset == null) || (!_mainMoveset.IsDirty))
                return true;

            try
            {
                _mainMoveset.RootNode.Merge();
                _mainMoveset.RootNode.Export(_mainMoveset.RootNode._origPath);
                return true;
            }
            catch (Exception x) { MessageBox.Show(this, x.ToString()); }
            return false;
        }

        private void btnOpen_Click(object sender, EventArgs e) { LoadMoveset(); }
        private void btnSave_Click(object sender, EventArgs e) { SaveMoveset(); }
        private void btnClose_Click(object sender, EventArgs e) { CloseMoveset(); }
        private void txtExtPath_Click(object sender, EventArgs e) { LoadMoveset(); }

        public int _selectedHurtboxIndex = -1;

        public bool _updating = false;
        private void lstHurtboxes_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (!_updating)
            {
                TransformObject = lstHurtboxes.Items[e.Index] as MoveDefHurtBoxNode;

                _mainWindow.modelPanel1.Invalidate();
            }
        }

        private void SelectedBone_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!(SelectedObject is MoveDefHurtBoxNode))
                return;

            ((MoveDefHurtBoxNode)SelectedObject).BoneNode = (MDL0BoneNode)TargetModel._linker.BoneCache[SelectedBone.SelectedIndex];
            
            if (!_updating)
                _mainWindow.modelPanel1.Invalidate();
        }

        private void SelectedZone_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!(SelectedObject is MoveDefHurtBoxNode))
                return;

            ((MoveDefHurtBoxNode)SelectedObject).Zone = (HurtBoxZone)SelectedZone.SelectedIndex;

            if (!_updating)
                _mainWindow.modelPanel1.Invalidate();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (EditorChanged != null)
                EditorChanged(this, null);
        }

        private void lstHurtboxes_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedObject = lstHurtboxes.SelectedItem;
            _selectedHurtboxIndex = lstHurtboxes.SelectedIndex;

            if (!_updating)
                _mainWindow.modelPanel1.Invalidate();
        }

        private void lstHurtboxes_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                lstHurtboxes.SelectedItem = null;
                SelectedObject = null;
                _selectedHurtboxIndex = -1;

                if (!_updating)
                    _mainWindow.modelPanel1.Invalidate();
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (lstHurtboxes.Items.Count == 0)
                return;

            _updating = true;

            lstHurtboxes.BeginUpdate();
            for (int i = 0; i < lstHurtboxes.Items.Count; i++)
                lstHurtboxes.SetItemCheckState(i, checkBox1.CheckState);
            lstHurtboxes.EndUpdate();

            _updating = false;

            _mainWindow.modelPanel1.Invalidate();
        }

        private void numOffX_TextChanged(object sender, EventArgs e)
        {
            if (!(SelectedObject is MoveDefHurtBoxNode))
                return;

            float.TryParse(numOffX.Text, out ((MoveDefHurtBoxNode)SelectedObject)._offst._x);
            ((MoveDefHurtBoxNode)SelectedObject).SignalPropertyChange();

            if (!_updating)
                _mainWindow.modelPanel1.Invalidate();
        }

        private void numOffY_TextChanged(object sender, EventArgs e)
        {
            if (!(SelectedObject is MoveDefHurtBoxNode))
                return;

            float.TryParse(numOffY.Text, out ((MoveDefHurtBoxNode)SelectedObject)._offst._y);
            ((MoveDefHurtBoxNode)SelectedObject).SignalPropertyChange();

            if (!_updating)
                _mainWindow.modelPanel1.Invalidate();
        }

        private void numOffZ_TextChanged(object sender, EventArgs e)
        {
            if (!(SelectedObject is MoveDefHurtBoxNode))
                return;

            float.TryParse(numOffZ.Text, out ((MoveDefHurtBoxNode)SelectedObject)._offst._z);
            ((MoveDefHurtBoxNode)SelectedObject).SignalPropertyChange();

            if (!_updating)
                _mainWindow.modelPanel1.Invalidate();
        }

        private void numStrX_TextChanged(object sender, EventArgs e)
        {
            if (!(SelectedObject is MoveDefHurtBoxNode))
                return;

            float.TryParse(numStrX.Text, out ((MoveDefHurtBoxNode)SelectedObject)._stretch._x);
            ((MoveDefHurtBoxNode)SelectedObject).SignalPropertyChange();

            if (!_updating)
                _mainWindow.modelPanel1.Invalidate();
        }

        private void numStrY_TextChanged(object sender, EventArgs e)
        {
            if (!(SelectedObject is MoveDefHurtBoxNode))
                return;

            float.TryParse(numStrY.Text, out ((MoveDefHurtBoxNode)SelectedObject)._stretch._y);
            ((MoveDefHurtBoxNode)SelectedObject).SignalPropertyChange();

            if (!_updating)
                _mainWindow.modelPanel1.Invalidate();
        }

        private void numStrZ_TextChanged(object sender, EventArgs e)
        {
            if (!(SelectedObject is MoveDefHurtBoxNode))
                return;

            float.TryParse(numStrZ.Text, out ((MoveDefHurtBoxNode)SelectedObject)._stretch._z);
            ((MoveDefHurtBoxNode)SelectedObject).SignalPropertyChange();

            if (!_updating)
                _mainWindow.modelPanel1.Invalidate();
        }

        private void numRegion_TextChanged(object sender, EventArgs e)
        {
            if (!(SelectedObject is MoveDefHurtBoxNode))
                return;

            int r;
            int.TryParse(numRegion.Text, out r);
            ((MoveDefHurtBoxNode)SelectedObject).Region = r;
        }

        private void numRadius_TextChanged(object sender, EventArgs e)
        {
            if (!(SelectedObject is MoveDefHurtBoxNode))
                return;

            float.TryParse(numRadius.Text, out ((MoveDefHurtBoxNode)SelectedObject)._radius);
            ((MoveDefHurtBoxNode)SelectedObject).SignalPropertyChange();

            if (!_updating)
                _mainWindow.modelPanel1.Invalidate();
        }

        private void BoxEnabled_CheckedChanged(object sender, EventArgs e)
        {
            if (!(SelectedObject is MoveDefHurtBoxNode))
                return;

            ((MoveDefHurtBoxNode)SelectedObject).Enabled = BoxEnabled.Checked;
        }

        int level1Index = 0;
        int level2Index = 0;
        int level3Index = 0;

        private void ItemSwitcher_SelectedIndexChanged(object sender, EventArgs e)
        {
            level1Index = ItemSwitcher.SelectedIndex;
            if (level1Index == 0)
            {
                comboBox1.Items.Clear();
                comboBox1.Items.Add("Entry");
                comboBox1.Items.Add("Exit");
                SubActionFlagsPanel.Visible = false;
                comboBox1.SelectedIndex = 0;
                subactions = false;
            }
            else if (level1Index == 1)
            {
                comboBox1.Items.Clear();
                comboBox1.Items.Add("Main");
                comboBox1.Items.Add("GFX");
                comboBox1.Items.Add("SFX");
                comboBox1.Items.Add("Other");
                ActionFlagsPanel.Visible = false;
                comboBox1.SelectedIndex = 0;
                subactions = true;
            }
            UpdateCurrentControl();
        }
        private void AttributesTabGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            level2Index = AttributesTabGroup.SelectedIndex;
            UpdateCurrentControl();
        }
        private void CombatTabGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            level3Index = CombatTabGroup.SelectedIndex;
            UpdateCurrentControl();
        }

        public MoveDefSubActionGroupNode selectedSubActionGrp;
        public MoveDefActionGroupNode selectedActionGrp;
        public List<MoveDefActionNode> selectedActionNodes = new List<MoveDefActionNode>();
        
        private void ActionsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            _selectedObject = selectedActionGrp = ActionsList.SelectedItem as MoveDefActionGroupNode;

            _mainWindow._maxFrame = 1;
            _mainWindow.GetFiles(-1);
            
            comboBox1_SelectedIndexChanged(this, null);
            UpdateCurrentControl();

            foreach (MoveDefActionNode a in selectedActionNodes)
                a.Reset(_mainWindow);

            selectedActionNodes.Clear();
            foreach (MoveDefActionNode a in selectedActionGrp.Children)
                selectedActionNodes.Add(a);

            _animFrame = 0;
            _mainWindow.pnlAssets.listAnims.SelectedIndices.Clear();
        }

        public bool subactions = false; //Actions default
        private void flagsToggle_Click(object sender, EventArgs e)
        {
            if (subactions)
            if (SubActionFlagsPanel.Visible)
                SubActionFlagsPanel.Visible = false;
            else
                SubActionFlagsPanel.Visible = true;
            //else
            //if (ActionFlagsPanel.Visible && !subactions)
            //    ActionFlagsPanel.Visible = false;
            //else
            //    ActionFlagsPanel.Visible = true;
        }

        private void SubActionsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            _selectedObject = selectedSubActionGrp = SubActionsList.SelectedItem as MoveDefSubActionGroupNode;
            comboBox1_SelectedIndexChanged(this, null);
            UpdateCurrentControl();

            foreach (MoveDefActionNode a in selectedActionNodes)
                a.Reset(_mainWindow);

            selectedActionNodes.Clear();
            foreach (MoveDefActionNode a in selectedSubActionGrp.Children)
                selectedActionNodes.Add(a);

            for (int i = 0; i < _mainWindow.pnlAssets.listAnims.Items.Count; i++)
                if (_mainWindow.pnlAssets.listAnims.Items[i].Tag.ToString() == selectedSubActionGrp.Name)
                {
                    _mainWindow.pnlAssets.listAnims.Items[i].Selected = true;
                    break;
                }

            SetFrame(0);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex != -1)
            if (subactions)
                if (selectedSubActionGrp != null)
                    scriptEditor1.TargetNode = selectedSubActionGrp.Children[comboBox1.SelectedIndex] as MoveDefActionNode;
                else { }
            else
                if (selectedActionGrp != null)
                    scriptEditor1.TargetNode = selectedActionGrp.Children[comboBox1.SelectedIndex] as MoveDefActionNode;
                else { }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (animTimer.Enabled)
                StopScript();
            else
                RunScript();
        }

        public bool ActionsIdling
        {
            get
            {
                //bool allEmpty = true;
                //foreach (MoveDefActionNode a in selectedActionNodes)
                //    if (a.Children.Count > 0)
                //        allEmpty = false;
                //if (allEmpty)
                //    return false;
                foreach (MoveDefActionNode a in selectedActionNodes)
                    if (!a._idling)
                        return false;
                return true;
            }
        }

        public bool _playing = false;
        public void RunScript()
        {
            _playing = true;

            if (subactions)
            {
                _mainWindow._playing = true;
                if (ActionsIdling && _animFrame >= _mainWindow._maxFrame - 1) //Reset scripts
                    SetFrame(0);
            }

            //if (!ActionsIdling)
            //{
                animTimer.Start();
                button1.Text = "Stop Script";
            //}
            //else
            //    _playing = false;
        }

        public void StopScript()
        {
            animTimer.Stop();
            _playing = false;
            if (subactions)
                _mainWindow._playing = false;
            button1.Text = "Run Script";
            //_animFrame = -1;
        }
        private void animTimer_Tick(object sender, EventArgs e)
        {
            if (ActionsIdling && subactions && _animFrame >= _mainWindow._maxFrame - 1&& selectedSubActionGrp != null)
            {
                if (_mainWindow._animFrame < _mainWindow._maxFrame)
                    _mainWindow.SetFrame(_mainWindow._animFrame + 1);
                else
                {
                    _animFrame = -1;
                    if (chkLoop.Checked)
                        SetFrame(0);
                    else
                        StopScript();
                }
            }
            else
                SetFrame(_animFrame + 1);
        }
        public int _animFrame = -1;
        public void SetFrame(int index)
        {
            if (index == 0)
                ResetModelVis();

            if (subactions)
                _mainWindow.SetFrame(index + 1);

            if (!_updating)
            {
                if (!_playing)
                {
                    ResetModelVis();
                    for (int i = 0; i < selectedActionNodes.Count; i++)
                    {
                        MoveDefActionNode a = selectedActionNodes[i];

                        //if (a._idling)
                        //    continue;

                        a.SetFrame(index, _mainWindow);
                        if (a == scriptEditor1.TargetNode)
                        {
                            scriptEditor1.EventList.SelectedIndices.Clear();
                            if (a._eventIndex - 1 < scriptEditor1.EventList.Items.Count)
                                scriptEditor1.EventList.SelectedIndex = a._eventIndex - 1;
                        }
                    }
                }
                else
                {
                    if (_animFrame == index - 1)
                        for (int i = 0; i < selectedActionNodes.Count; i++)
                        {
                            MoveDefActionNode a = selectedActionNodes[i];

                            //if (a._idling)
                            //    continue;

                            if (index == 0)
                                a.Reset(_mainWindow);

                            a.FrameAdvance(_mainWindow);
                            if (a == scriptEditor1.TargetNode)
                            {
                                scriptEditor1.EventList.SelectedIndices.Clear();
                                if (a._eventIndex - 1 < scriptEditor1.EventList.Items.Count)
                                    scriptEditor1.EventList.SelectedIndex = a._eventIndex - 1;
                            }
                        }
                    else
                    {
                        ResetModelVis();
                        for (int i = 0; i < selectedActionNodes.Count; i++)
                        {
                            MoveDefActionNode a = selectedActionNodes[i];
                            a.SetFrame(index, _mainWindow);
                            if (a == scriptEditor1.TargetNode)
                            {
                                scriptEditor1.EventList.SelectedIndices.Clear();
                                if (a._eventIndex - 1 < scriptEditor1.EventList.Items.Count)
                                    scriptEditor1.EventList.SelectedIndex = a._eventIndex - 1;
                            }
                        }
                    }
                }
            }
            _animFrame = index;
        }

        public void ResetModelVis()
        {
            foreach (MDL0BoneNode bone in _mainWindow.boneCollisions)
                bone._nodeColor = bone._boneColor = Color.Transparent;
            _mainWindow.boneCollisions = new List<MDL0BoneNode>();

            if (TargetModel != null && TargetModel._polyList != null && _mainMoveset != null)
            {
                MoveDefModelVisibilityNode node = _mainMoveset.data.mdlVisibility;
                if (node.Children.Count != 0)
                {
                    MoveDefModelVisRefNode entry = node.Children[0] as MoveDefModelVisRefNode;
                    foreach (MoveDefBoneSwitchNode Switch in entry.Children)
                    {
                        foreach (MoveDefModelVisGroupNode Group in Switch.Children)
                        {
                            if (Group.Index != Switch.defaultGroup)
                            foreach (MoveDefBoneIndexNode b in Group.Children)
                            {
                                if (b.BoneNode != null)
                                    foreach (MDL0PolygonNode p in b.BoneNode._manPolys)
                                        p._render = false;
                            }
                        }
                    }
                    foreach (MoveDefBoneSwitchNode Switch in entry.Children)
                    {
                        foreach (MoveDefModelVisGroupNode Group in Switch.Children)
                        {
                            if (Group.Index == Switch.defaultGroup)
                            foreach (MoveDefBoneIndexNode b in Group.Children)
                            {
                                if (b.BoneNode != null)
                                    foreach (MDL0PolygonNode p in b.BoneNode._manPolys)
                                        p._render = true;
                            }
                        }
                    }
                }
            }
        }
    }
}
