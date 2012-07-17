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

namespace System.Windows.Forms
{
    class ModelAnimPanel : UserControl
    {
        public delegate void ReferenceEventHandler(ResourceNode node);

        #region Designer

        private GroupBox grpCHRTransform;
        private Button btnPaste;
        private Button btnCopy;
        private Button btnCut;
        private Label label5;
        private NumericInputBox numScaleZ;
        internal NumericInputBox numTransX;
        private NumericInputBox numScaleY;
        private Label label6;
        private NumericInputBox numScaleX;
        private Label label7;
        internal NumericInputBox numRotZ;
        private Label label8;
        internal NumericInputBox numRotY;
        private Label label9;
        internal NumericInputBox numRotX;
        private Label label10;
        internal NumericInputBox numTransZ;
        private Label label11;
        internal NumericInputBox numTransY;
        private Label label12;
        private GroupBox grpExt;
        private TextBox txtExtPath;
        private Button btnOpenClose;
        private Button btnSave;
        private ListView listAnims;
        private ColumnHeader nameColumn;
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
        private Button btnClear;
        private Button btnDelete;
        private Button btnInsert;
        private GroupBox grpTransAll;
        private IContainer components;
        private Button btnClean;
        private Button btnPasteAll;
        private Button btnCopyAll;
        private CheckBox Translate;
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
        private CheckBox Scale;
        private CheckBox Rotate;
        private Button btnSaveAs;
        private GroupBox grpSRTTransform;
        private Button button1;
        private Button button2;
        private Button button3;
        private Label label1;
        internal NumericInputBox numTexTransX;
        private Label label2;
        private NumericInputBox numTexScale;
        private Label label4;
        internal NumericInputBox numTexRot;
        private Label label16;
        internal NumericInputBox numTexTransY;
        private Label label13;

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("Animations", System.Windows.Forms.HorizontalAlignment.Left);
            this.grpCHRTransform = new System.Windows.Forms.GroupBox();
            this.btnPaste = new System.Windows.Forms.Button();
            this.btnCopy = new System.Windows.Forms.Button();
            this.btnCut = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.numScaleZ = new System.Windows.Forms.NumericInputBox();
            this.numTransX = new System.Windows.Forms.NumericInputBox();
            this.numScaleY = new System.Windows.Forms.NumericInputBox();
            this.label6 = new System.Windows.Forms.Label();
            this.numScaleX = new System.Windows.Forms.NumericInputBox();
            this.label7 = new System.Windows.Forms.Label();
            this.numRotZ = new System.Windows.Forms.NumericInputBox();
            this.label8 = new System.Windows.Forms.Label();
            this.numRotY = new System.Windows.Forms.NumericInputBox();
            this.label9 = new System.Windows.Forms.Label();
            this.numRotX = new System.Windows.Forms.NumericInputBox();
            this.label10 = new System.Windows.Forms.Label();
            this.numTransZ = new System.Windows.Forms.NumericInputBox();
            this.label11 = new System.Windows.Forms.Label();
            this.numTransY = new System.Windows.Forms.NumericInputBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnInsert = new System.Windows.Forms.Button();
            this.grpExt = new System.Windows.Forms.GroupBox();
            this.btnSaveAs = new System.Windows.Forms.Button();
            this.txtExtPath = new System.Windows.Forms.TextBox();
            this.btnOpenClose = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.listAnims = new System.Windows.Forms.ListView();
            this.nameColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
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
            this.grpTransAll = new System.Windows.Forms.GroupBox();
            this.Scale = new System.Windows.Forms.CheckBox();
            this.Rotate = new System.Windows.Forms.CheckBox();
            this.Translate = new System.Windows.Forms.CheckBox();
            this.btnClean = new System.Windows.Forms.Button();
            this.btnPasteAll = new System.Windows.Forms.Button();
            this.btnCopyAll = new System.Windows.Forms.Button();
            this.grpSRTTransform = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.numTexTransX = new System.Windows.Forms.NumericInputBox();
            this.label2 = new System.Windows.Forms.Label();
            this.numTexScale = new System.Windows.Forms.NumericInputBox();
            this.label4 = new System.Windows.Forms.Label();
            this.numTexRot = new System.Windows.Forms.NumericInputBox();
            this.label16 = new System.Windows.Forms.Label();
            this.numTexTransY = new System.Windows.Forms.NumericInputBox();
            this.grpCHRTransform.SuspendLayout();
            this.grpExt.SuspendLayout();
            this.ctxAnim.SuspendLayout();
            this.ctxBox.SuspendLayout();
            this.grpTransAll.SuspendLayout();
            this.grpSRTTransform.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpCHRTransform
            // 
            this.grpCHRTransform.Controls.Add(this.btnPaste);
            this.grpCHRTransform.Controls.Add(this.btnCopy);
            this.grpCHRTransform.Controls.Add(this.btnCut);
            this.grpCHRTransform.Controls.Add(this.label5);
            this.grpCHRTransform.Controls.Add(this.numScaleZ);
            this.grpCHRTransform.Controls.Add(this.numTransX);
            this.grpCHRTransform.Controls.Add(this.numScaleY);
            this.grpCHRTransform.Controls.Add(this.label6);
            this.grpCHRTransform.Controls.Add(this.numScaleX);
            this.grpCHRTransform.Controls.Add(this.label7);
            this.grpCHRTransform.Controls.Add(this.numRotZ);
            this.grpCHRTransform.Controls.Add(this.label8);
            this.grpCHRTransform.Controls.Add(this.numRotY);
            this.grpCHRTransform.Controls.Add(this.label9);
            this.grpCHRTransform.Controls.Add(this.numRotX);
            this.grpCHRTransform.Controls.Add(this.label10);
            this.grpCHRTransform.Controls.Add(this.numTransZ);
            this.grpCHRTransform.Controls.Add(this.label11);
            this.grpCHRTransform.Controls.Add(this.numTransY);
            this.grpCHRTransform.Controls.Add(this.label12);
            this.grpCHRTransform.Controls.Add(this.label13);
            this.grpCHRTransform.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.grpCHRTransform.Enabled = false;
            this.grpCHRTransform.Location = new System.Drawing.Point(0, 376);
            this.grpCHRTransform.Name = "grpCHRTransform";
            this.grpCHRTransform.Size = new System.Drawing.Size(173, 239);
            this.grpCHRTransform.TabIndex = 22;
            this.grpCHRTransform.TabStop = false;
            this.grpCHRTransform.Text = "Transform Frame";
            // 
            // btnPaste
            // 
            this.btnPaste.Location = new System.Drawing.Point(116, 215);
            this.btnPaste.Name = "btnPaste";
            this.btnPaste.Size = new System.Drawing.Size(50, 20);
            this.btnPaste.TabIndex = 23;
            this.btnPaste.Text = "Paste";
            this.btnPaste.UseVisualStyleBackColor = true;
            this.btnPaste.Click += new System.EventHandler(this.btnPaste_Click);
            // 
            // btnCopy
            // 
            this.btnCopy.Location = new System.Drawing.Point(62, 215);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(50, 20);
            this.btnCopy.TabIndex = 22;
            this.btnCopy.Text = "Copy";
            this.btnCopy.UseVisualStyleBackColor = true;
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // btnCut
            // 
            this.btnCut.Location = new System.Drawing.Point(8, 215);
            this.btnCut.Name = "btnCut";
            this.btnCut.Size = new System.Drawing.Size(50, 20);
            this.btnCut.TabIndex = 21;
            this.btnCut.Text = "Cut";
            this.btnCut.UseVisualStyleBackColor = true;
            this.btnCut.Click += new System.EventHandler(this.btnCut_Click);
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(6, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(74, 20);
            this.label5.TabIndex = 4;
            this.label5.Text = "Translation X:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label5.MouseDown += new System.Windows.Forms.MouseEventHandler(this.label5_MouseDown);
            // 
            // numScaleZ
            // 
            this.numScaleZ.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.numScaleZ.Location = new System.Drawing.Point(86, 192);
            this.numScaleZ.Name = "numScaleZ";
            this.numScaleZ.Size = new System.Drawing.Size(82, 20);
            this.numScaleZ.TabIndex = 20;
            this.numScaleZ.Text = "0";
            this.numScaleZ.ValueChanged += new System.EventHandler(this.BoxChangedCreateUndo);
            // 
            // numTransX
            // 
            this.numTransX.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.numTransX.Location = new System.Drawing.Point(86, 16);
            this.numTransX.Name = "numTransX";
            this.numTransX.Size = new System.Drawing.Size(82, 20);
            this.numTransX.TabIndex = 3;
            this.numTransX.Text = "0";
            this.numTransX.ValueChanged += new System.EventHandler(this.BoxChangedCreateUndo);
            // 
            // numScaleY
            // 
            this.numScaleY.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.numScaleY.Location = new System.Drawing.Point(86, 172);
            this.numScaleY.Name = "numScaleY";
            this.numScaleY.Size = new System.Drawing.Size(82, 20);
            this.numScaleY.TabIndex = 19;
            this.numScaleY.Text = "0";
            this.numScaleY.ValueChanged += new System.EventHandler(this.BoxChangedCreateUndo);
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(6, 36);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(74, 20);
            this.label6.TabIndex = 5;
            this.label6.Text = "Translation Y:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label6.MouseDown += new System.Windows.Forms.MouseEventHandler(this.label6_MouseDown);
            // 
            // numScaleX
            // 
            this.numScaleX.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.numScaleX.Location = new System.Drawing.Point(86, 152);
            this.numScaleX.Name = "numScaleX";
            this.numScaleX.Size = new System.Drawing.Size(82, 20);
            this.numScaleX.TabIndex = 18;
            this.numScaleX.Text = "0";
            this.numScaleX.ValueChanged += new System.EventHandler(this.BoxChangedCreateUndo);
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(6, 56);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(74, 20);
            this.label7.TabIndex = 6;
            this.label7.Text = "Translation Z:";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label7.MouseDown += new System.Windows.Forms.MouseEventHandler(this.label7_MouseDown);
            // 
            // numRotZ
            // 
            this.numRotZ.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.numRotZ.Location = new System.Drawing.Point(86, 124);
            this.numRotZ.Name = "numRotZ";
            this.numRotZ.Size = new System.Drawing.Size(82, 20);
            this.numRotZ.TabIndex = 17;
            this.numRotZ.Text = "0";
            this.numRotZ.ValueChanged += new System.EventHandler(this.BoxChangedCreateUndo);
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(6, 84);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(74, 20);
            this.label8.TabIndex = 7;
            this.label8.Text = "Rotation X:";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label8.MouseDown += new System.Windows.Forms.MouseEventHandler(this.label8_MouseDown);
            // 
            // numRotY
            // 
            this.numRotY.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.numRotY.Location = new System.Drawing.Point(86, 104);
            this.numRotY.Name = "numRotY";
            this.numRotY.Size = new System.Drawing.Size(82, 20);
            this.numRotY.TabIndex = 16;
            this.numRotY.Text = "0";
            this.numRotY.ValueChanged += new System.EventHandler(this.BoxChangedCreateUndo);
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(6, 104);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(74, 20);
            this.label9.TabIndex = 8;
            this.label9.Text = "Rotation Y:";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label9.MouseDown += new System.Windows.Forms.MouseEventHandler(this.label9_MouseDown);
            // 
            // numRotX
            // 
            this.numRotX.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.numRotX.Location = new System.Drawing.Point(86, 84);
            this.numRotX.Name = "numRotX";
            this.numRotX.Size = new System.Drawing.Size(82, 20);
            this.numRotX.TabIndex = 15;
            this.numRotX.Text = "0";
            this.numRotX.ValueChanged += new System.EventHandler(this.BoxChangedCreateUndo);
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(6, 124);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(74, 20);
            this.label10.TabIndex = 9;
            this.label10.Text = "Rotation Z:";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label10.MouseDown += new System.Windows.Forms.MouseEventHandler(this.label10_MouseDown);
            // 
            // numTransZ
            // 
            this.numTransZ.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.numTransZ.Location = new System.Drawing.Point(86, 56);
            this.numTransZ.Name = "numTransZ";
            this.numTransZ.Size = new System.Drawing.Size(82, 20);
            this.numTransZ.TabIndex = 14;
            this.numTransZ.Text = "0";
            this.numTransZ.ValueChanged += new System.EventHandler(this.BoxChangedCreateUndo);
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(6, 152);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(74, 20);
            this.label11.TabIndex = 10;
            this.label11.Text = "Scale X:";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numTransY
            // 
            this.numTransY.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.numTransY.Location = new System.Drawing.Point(86, 36);
            this.numTransY.Name = "numTransY";
            this.numTransY.Size = new System.Drawing.Size(82, 20);
            this.numTransY.TabIndex = 13;
            this.numTransY.Text = "0";
            this.numTransY.ValueChanged += new System.EventHandler(this.BoxChangedCreateUndo);
            // 
            // label12
            // 
            this.label12.Location = new System.Drawing.Point(6, 192);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(74, 20);
            this.label12.TabIndex = 11;
            this.label12.Text = "Scale Z:";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label13
            // 
            this.label13.Location = new System.Drawing.Point(6, 172);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(74, 20);
            this.label13.TabIndex = 12;
            this.label13.Text = "Scale Y:";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(116, 14);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(50, 20);
            this.btnClear.TabIndex = 26;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(62, 14);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(50, 20);
            this.btnDelete.TabIndex = 25;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnInsert
            // 
            this.btnInsert.Location = new System.Drawing.Point(8, 14);
            this.btnInsert.Name = "btnInsert";
            this.btnInsert.Size = new System.Drawing.Size(50, 20);
            this.btnInsert.TabIndex = 24;
            this.btnInsert.Text = "Insert";
            this.btnInsert.UseVisualStyleBackColor = true;
            this.btnInsert.Click += new System.EventHandler(this.btnInsert_Click);
            // 
            // grpExt
            // 
            this.grpExt.Controls.Add(this.btnSaveAs);
            this.grpExt.Controls.Add(this.txtExtPath);
            this.grpExt.Controls.Add(this.btnOpenClose);
            this.grpExt.Controls.Add(this.btnSave);
            this.grpExt.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpExt.Location = new System.Drawing.Point(0, 0);
            this.grpExt.Name = "grpExt";
            this.grpExt.Padding = new System.Windows.Forms.Padding(6, 4, 6, 3);
            this.grpExt.Size = new System.Drawing.Size(173, 68);
            this.grpExt.TabIndex = 23;
            this.grpExt.TabStop = false;
            this.grpExt.Text = "External Animation File";
            // 
            // btnSaveAs
            // 
            this.btnSaveAs.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnSaveAs.Location = new System.Drawing.Point(112, 42);
            this.btnSaveAs.Margin = new System.Windows.Forms.Padding(2);
            this.btnSaveAs.Name = "btnSaveAs";
            this.btnSaveAs.Size = new System.Drawing.Size(55, 20);
            this.btnSaveAs.TabIndex = 4;
            this.btnSaveAs.Text = "Save As";
            this.btnSaveAs.UseVisualStyleBackColor = true;
            this.btnSaveAs.Click += new System.EventHandler(this.btnSaveAs_Click);
            // 
            // txtExtPath
            // 
            this.txtExtPath.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtExtPath.Location = new System.Drawing.Point(6, 17);
            this.txtExtPath.Name = "txtExtPath";
            this.txtExtPath.ReadOnly = true;
            this.txtExtPath.Size = new System.Drawing.Size(161, 20);
            this.txtExtPath.TabIndex = 3;
            this.txtExtPath.Click += new System.EventHandler(this.txtExtPath_Click);
            // 
            // btnOpenClose
            // 
            this.btnOpenClose.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnOpenClose.Location = new System.Drawing.Point(6, 42);
            this.btnOpenClose.Margin = new System.Windows.Forms.Padding(2);
            this.btnOpenClose.Name = "btnOpenClose";
            this.btnOpenClose.Size = new System.Drawing.Size(48, 20);
            this.btnOpenClose.TabIndex = 0;
            this.btnOpenClose.Text = "Load";
            this.btnOpenClose.UseVisualStyleBackColor = true;
            this.btnOpenClose.Click += new System.EventHandler(this.btnOpenClose_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnSave.Location = new System.Drawing.Point(58, 42);
            this.btnSave.Margin = new System.Windows.Forms.Padding(2);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(50, 20);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // listAnims
            // 
            this.listAnims.AutoArrange = false;
            this.listAnims.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.nameColumn});
            this.listAnims.ContextMenuStrip = this.ctxAnim;
            this.listAnims.Dock = System.Windows.Forms.DockStyle.Fill;
            listViewGroup1.Header = "Animations";
            listViewGroup1.Name = "grpAnims";
            this.listAnims.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1});
            this.listAnims.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.listAnims.HideSelection = false;
            this.listAnims.Location = new System.Drawing.Point(0, 68);
            this.listAnims.MultiSelect = false;
            this.listAnims.Name = "listAnims";
            this.listAnims.Size = new System.Drawing.Size(173, 118);
            this.listAnims.TabIndex = 24;
            this.listAnims.UseCompatibleStateImageBehavior = false;
            this.listAnims.View = System.Windows.Forms.View.Details;
            this.listAnims.SelectedIndexChanged += new System.EventHandler(this.listAnims_SelectedIndexChanged);
            // 
            // nameColumn
            // 
            this.nameColumn.Text = "Name";
            this.nameColumn.Width = 160;
            // 
            // ctxAnim
            // 
            this.ctxAnim.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sourceToolStripMenuItem,
            this.toolStripMenuItem1,
            this.exportToolStripMenuItem,
            this.replaceToolStripMenuItem,
            this.portToolStripMenuItem});
            this.ctxAnim.Name = "ctxAnim";
            this.ctxAnim.Size = new System.Drawing.Size(165, 98);
            this.ctxAnim.Opening += new System.ComponentModel.CancelEventHandler(this.ctxAnim_Opening);
            // 
            // sourceToolStripMenuItem
            // 
            this.sourceToolStripMenuItem.Enabled = false;
            this.sourceToolStripMenuItem.Name = "sourceToolStripMenuItem";
            this.sourceToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.sourceToolStripMenuItem.Text = "Source";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(161, 6);
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.exportToolStripMenuItem.Text = "Export...";
            this.exportToolStripMenuItem.Click += new System.EventHandler(this.exportToolStripMenuItem_Click);
            // 
            // replaceToolStripMenuItem
            // 
            this.replaceToolStripMenuItem.Name = "replaceToolStripMenuItem";
            this.replaceToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.replaceToolStripMenuItem.Text = "Replace...";
            this.replaceToolStripMenuItem.Click += new System.EventHandler(this.replaceToolStripMenuItem_Click);
            // 
            // portToolStripMenuItem
            // 
            this.portToolStripMenuItem.Name = "portToolStripMenuItem";
            this.portToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.portToolStripMenuItem.Text = "Port Animation...";
            this.portToolStripMenuItem.Click += new System.EventHandler(this.portToolStripMenuItem_Click);
            // 
            // ctxBox
            // 
            this.ctxBox.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Source,
            this.toolStripSeparator1,
            this.add,
            this.subtract,
            this.removeAllToolStripMenuItem,
            this.addCustomAmountToolStripMenuItem});
            this.ctxBox.Name = "ctxBox";
            this.ctxBox.Size = new System.Drawing.Size(167, 120);
            this.ctxBox.Opening += new System.ComponentModel.CancelEventHandler(this.ctxBox_Opening);
            // 
            // Source
            // 
            this.Source.Enabled = false;
            this.Source.Name = "Source";
            this.Source.Size = new System.Drawing.Size(166, 22);
            this.Source.Text = "Source";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(163, 6);
            // 
            // add
            // 
            this.add.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem3,
            this.toolStripMenuItem4,
            this.toolStripMenuItem7});
            this.add.Name = "add";
            this.add.Size = new System.Drawing.Size(166, 22);
            this.add.Text = "Add To All";
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(100, 22);
            this.toolStripMenuItem3.Text = "+180";
            this.toolStripMenuItem3.Click += new System.EventHandler(this.toolStripMenuItem3_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(100, 22);
            this.toolStripMenuItem4.Text = "+90";
            this.toolStripMenuItem4.Click += new System.EventHandler(this.toolStripMenuItem4_Click);
            // 
            // toolStripMenuItem7
            // 
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            this.toolStripMenuItem7.Size = new System.Drawing.Size(100, 22);
            this.toolStripMenuItem7.Text = "+45";
            this.toolStripMenuItem7.Click += new System.EventHandler(this.toolStripMenuItem7_Click);
            // 
            // subtract
            // 
            this.subtract.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem5,
            this.toolStripMenuItem6,
            this.toolStripMenuItem8});
            this.subtract.Name = "subtract";
            this.subtract.Size = new System.Drawing.Size(166, 22);
            this.subtract.Text = "Subtract From All";
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(97, 22);
            this.toolStripMenuItem5.Text = "-180";
            this.toolStripMenuItem5.Click += new System.EventHandler(this.toolStripMenuItem5_Click);
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(97, 22);
            this.toolStripMenuItem6.Text = "-90";
            this.toolStripMenuItem6.Click += new System.EventHandler(this.toolStripMenuItem6_Click);
            // 
            // toolStripMenuItem8
            // 
            this.toolStripMenuItem8.Name = "toolStripMenuItem8";
            this.toolStripMenuItem8.Size = new System.Drawing.Size(97, 22);
            this.toolStripMenuItem8.Text = "-45";
            this.toolStripMenuItem8.Click += new System.EventHandler(this.toolStripMenuItem8_Click);
            // 
            // removeAllToolStripMenuItem
            // 
            this.removeAllToolStripMenuItem.Name = "removeAllToolStripMenuItem";
            this.removeAllToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.removeAllToolStripMenuItem.Text = "Remove All";
            this.removeAllToolStripMenuItem.Click += new System.EventHandler(this.removeAllToolStripMenuItem_Click);
            // 
            // addCustomAmountToolStripMenuItem
            // 
            this.addCustomAmountToolStripMenuItem.Name = "addCustomAmountToolStripMenuItem";
            this.addCustomAmountToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.addCustomAmountToolStripMenuItem.Text = "Edit All...";
            this.addCustomAmountToolStripMenuItem.Click += new System.EventHandler(this.addCustomAmountToolStripMenuItem_Click);
            // 
            // grpTransAll
            // 
            this.grpTransAll.Controls.Add(this.Scale);
            this.grpTransAll.Controls.Add(this.Rotate);
            this.grpTransAll.Controls.Add(this.Translate);
            this.grpTransAll.Controls.Add(this.btnClean);
            this.grpTransAll.Controls.Add(this.btnPasteAll);
            this.grpTransAll.Controls.Add(this.btnCopyAll);
            this.grpTransAll.Controls.Add(this.btnClear);
            this.grpTransAll.Controls.Add(this.btnInsert);
            this.grpTransAll.Controls.Add(this.btnDelete);
            this.grpTransAll.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.grpTransAll.Enabled = false;
            this.grpTransAll.Location = new System.Drawing.Point(0, 186);
            this.grpTransAll.Name = "grpTransAll";
            this.grpTransAll.Size = new System.Drawing.Size(173, 87);
            this.grpTransAll.TabIndex = 25;
            this.grpTransAll.TabStop = false;
            this.grpTransAll.Text = "Transform All";
            // 
            // Scale
            // 
            this.Scale.AutoSize = true;
            this.Scale.Checked = true;
            this.Scale.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Scale.Location = new System.Drawing.Point(122, 64);
            this.Scale.Name = "Scale";
            this.Scale.Size = new System.Drawing.Size(53, 17);
            this.Scale.TabIndex = 32;
            this.Scale.Text = "Scale";
            this.Scale.UseVisualStyleBackColor = true;
            this.Scale.CheckedChanged += new System.EventHandler(this.Scale_CheckedChanged);
            // 
            // Rotate
            // 
            this.Rotate.AutoSize = true;
            this.Rotate.Checked = true;
            this.Rotate.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Rotate.Location = new System.Drawing.Point(69, 64);
            this.Rotate.Name = "Rotate";
            this.Rotate.Size = new System.Drawing.Size(58, 17);
            this.Rotate.TabIndex = 31;
            this.Rotate.Text = "Rotate";
            this.Rotate.UseVisualStyleBackColor = true;
            this.Rotate.CheckedChanged += new System.EventHandler(this.Rotate_CheckedChanged);
            // 
            // Translate
            // 
            this.Translate.AutoSize = true;
            this.Translate.Checked = true;
            this.Translate.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Translate.Location = new System.Drawing.Point(4, 64);
            this.Translate.Name = "Translate";
            this.Translate.Size = new System.Drawing.Size(70, 17);
            this.Translate.TabIndex = 30;
            this.Translate.Text = "Translate";
            this.Translate.UseVisualStyleBackColor = true;
            this.Translate.CheckedChanged += new System.EventHandler(this.Translate_CheckedChanged);
            // 
            // btnClean
            // 
            this.btnClean.Location = new System.Drawing.Point(116, 40);
            this.btnClean.Name = "btnClean";
            this.btnClean.Size = new System.Drawing.Size(50, 20);
            this.btnClean.TabIndex = 29;
            this.btnClean.Text = "Clean";
            this.btnClean.UseVisualStyleBackColor = true;
            this.btnClean.Click += new System.EventHandler(this.btnClean_Click);
            // 
            // btnPasteAll
            // 
            this.btnPasteAll.Location = new System.Drawing.Point(62, 40);
            this.btnPasteAll.Name = "btnPasteAll";
            this.btnPasteAll.Size = new System.Drawing.Size(50, 20);
            this.btnPasteAll.TabIndex = 28;
            this.btnPasteAll.Text = "Paste";
            this.btnPasteAll.UseVisualStyleBackColor = true;
            this.btnPasteAll.Click += new System.EventHandler(this.btnPasteAll_Click);
            // 
            // btnCopyAll
            // 
            this.btnCopyAll.Location = new System.Drawing.Point(8, 40);
            this.btnCopyAll.Name = "btnCopyAll";
            this.btnCopyAll.Size = new System.Drawing.Size(50, 20);
            this.btnCopyAll.TabIndex = 27;
            this.btnCopyAll.Text = "Copy";
            this.btnCopyAll.UseVisualStyleBackColor = true;
            this.btnCopyAll.Click += new System.EventHandler(this.btnCopyAll_Click);
            // 
            // grpSRTTransform
            // 
            this.grpSRTTransform.Controls.Add(this.button1);
            this.grpSRTTransform.Controls.Add(this.button2);
            this.grpSRTTransform.Controls.Add(this.button3);
            this.grpSRTTransform.Controls.Add(this.label1);
            this.grpSRTTransform.Controls.Add(this.numTexTransX);
            this.grpSRTTransform.Controls.Add(this.label2);
            this.grpSRTTransform.Controls.Add(this.numTexScale);
            this.grpSRTTransform.Controls.Add(this.label4);
            this.grpSRTTransform.Controls.Add(this.numTexRot);
            this.grpSRTTransform.Controls.Add(this.label16);
            this.grpSRTTransform.Controls.Add(this.numTexTransY);
            this.grpSRTTransform.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.grpSRTTransform.Enabled = false;
            this.grpSRTTransform.Location = new System.Drawing.Point(0, 273);
            this.grpSRTTransform.Name = "grpSRTTransform";
            this.grpSRTTransform.Size = new System.Drawing.Size(173, 103);
            this.grpSRTTransform.TabIndex = 26;
            this.grpSRTTransform.TabStop = false;
            this.grpSRTTransform.Text = "Transform Frame";
            this.grpSRTTransform.Visible = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(116, 215);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(50, 20);
            this.button1.TabIndex = 23;
            this.button1.Text = "Paste";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(62, 215);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(50, 20);
            this.button2.TabIndex = 22;
            this.button2.Text = "Copy";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(8, 215);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(50, 20);
            this.button3.TabIndex = 21;
            this.button3.Text = "Cut";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 20);
            this.label1.TabIndex = 4;
            this.label1.Text = "Translation X:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numTexTransX
            // 
            this.numTexTransX.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.numTexTransX.Location = new System.Drawing.Point(86, 16);
            this.numTexTransX.Name = "numTexTransX";
            this.numTexTransX.Size = new System.Drawing.Size(82, 20);
            this.numTexTransX.TabIndex = 3;
            this.numTexTransX.Text = "0";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(6, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 20);
            this.label2.TabIndex = 5;
            this.label2.Text = "Translation Y:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numTexScale
            // 
            this.numTexScale.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.numTexScale.Location = new System.Drawing.Point(86, 76);
            this.numTexScale.Name = "numTexScale";
            this.numTexScale.Size = new System.Drawing.Size(82, 20);
            this.numTexScale.TabIndex = 18;
            this.numTexScale.Text = "0";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(6, 56);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(74, 20);
            this.label4.TabIndex = 7;
            this.label4.Text = "Rotation:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numTexRot
            // 
            this.numTexRot.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.numTexRot.Location = new System.Drawing.Point(86, 56);
            this.numTexRot.Name = "numTexRot";
            this.numTexRot.Size = new System.Drawing.Size(82, 20);
            this.numTexRot.TabIndex = 15;
            this.numTexRot.Text = "0";
            // 
            // label16
            // 
            this.label16.Location = new System.Drawing.Point(6, 76);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(74, 20);
            this.label16.TabIndex = 10;
            this.label16.Text = "Scale:";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numTexTransY
            // 
            this.numTexTransY.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.numTexTransY.Location = new System.Drawing.Point(86, 36);
            this.numTexTransY.Name = "numTexTransY";
            this.numTexTransY.Size = new System.Drawing.Size(82, 20);
            this.numTexTransY.TabIndex = 13;
            this.numTexTransY.Text = "0";
            // 
            // ModelAnimPanel
            // 
            this.Controls.Add(this.listAnims);
            this.Controls.Add(this.grpTransAll);
            this.Controls.Add(this.grpExt);
            this.Controls.Add(this.grpSRTTransform);
            this.Controls.Add(this.grpCHRTransform);
            this.Name = "ModelAnimPanel";
            this.Size = new System.Drawing.Size(173, 615);
            this.grpCHRTransform.ResumeLayout(false);
            this.grpCHRTransform.PerformLayout();
            this.grpExt.ResumeLayout(false);
            this.grpExt.PerformLayout();
            this.ctxAnim.ResumeLayout(false);
            this.ctxBox.ResumeLayout(false);
            this.grpTransAll.ResumeLayout(false);
            this.grpTransAll.PerformLayout();
            this.grpSRTTransform.ResumeLayout(false);
            this.grpSRTTransform.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public ResourceNode _externalNode;
        private ListViewGroup _CHRGroup = new ListViewGroup("Character Animations");
        internal NumericInputBox[] _transBoxes = new NumericInputBox[9];
        internal NumericInputBox[] _texBoxes = new NumericInputBox[4];
        private AnimationFrame _tempFrame = AnimationFrame.Neutral;

        public event EventHandler CreateUndo;
        public event EventHandler RenderStateChanged;
        public event EventHandler AnimStateChanged;
        public event EventHandler SelectedAnimationChanged;
        public event ReferenceEventHandler ReferenceLoaded;
        public event ReferenceEventHandler ReferenceClosed;

        private object _transformObject = null;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object TransformObject
        {
            get { return _transformObject; }
            set { _transformObject = value; UpdatePropDisplay(); }
        }

        private int _animFrame = 0;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int CurrentFrame
        {
            get { return _animFrame; }
            set { _animFrame = value; UpdateModel(); }
        }

        private MDL0Node _targetModel = null;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MDL0Node TargetModel
        {
            get { return _targetModel; }
            set { _targetModel = value; UpdateReferences(); UpdateModel(); }
        }

        private CHR0Node _selectedAnim;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public CHR0Node SelectedAnimation
        {
            get { return _selectedAnim; }
        }

        private SRT0Node _selectedTexAnim;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SRT0Node SelectedTextureAnimation
        {
            get { return _selectedTexAnim; }
        }

        private bool _enableTransform = true;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool EnableTransformEdit
        {
            get { return _enableTransform; }
            set { grpCHRTransform.Enabled = grpTransAll.Enabled = (_enableTransform = value) && (_transformObject != null); }
        }

        public ModelAnimPanel() 
        { 
            InitializeComponent();
            listAnims.Groups.Add(_CHRGroup);
            _transBoxes[0] = numScaleX; numScaleX.Tag = 0;
            _transBoxes[1] = numScaleY; numScaleY.Tag = 1;
            _transBoxes[2] = numScaleZ; numScaleZ.Tag = 2;
            _transBoxes[3] = numRotX; numRotX.Tag = 3;
            _transBoxes[4] = numRotY; numRotY.Tag = 4;
            _transBoxes[5] = numRotZ; numRotZ.Tag = 5;
            _transBoxes[6] = numTransX; numTransX.Tag = 6;
            _transBoxes[7] = numTransY; numTransY.Tag = 7;
            _transBoxes[8] = numTransZ; numTransZ.Tag = 8;
        }

        public bool CloseReferences()
        {
            return CloseExternal();
        }

        private bool UpdateReferences()
        {
            listAnims.BeginUpdate();
            listAnims.Items.Clear();

            if (_targetModel != null)
                LoadAnims(_targetModel.RootNode);

            int count = listAnims.Items.Count;

            if (_externalNode != null)
                LoadAnims(_externalNode.RootNode);

            if ((_selectedAnim != null) && (listAnims.SelectedItems.Count == 0))
            {
                _selectedAnim = null;
                if (SelectedAnimationChanged != null)
                    SelectedAnimationChanged(this, null);
            }

            listAnims.EndUpdate();

            return count != listAnims.Items.Count;
        }

        private void UpdateModel()
        {
            if (_targetModel == null)
                return;

            if (_selectedAnim != null)
                _targetModel.ApplyCHR(_selectedAnim, _animFrame);
            else
                _targetModel.ApplyCHR(null, 0);

            UpdatePropDisplay();
            if (RenderStateChanged != null)
                RenderStateChanged(this, null);
        }

        private void LoadAnims(ResourceNode node)
        {
            switch (node.ResourceType)
            {
                case ResourceType.ARC:
                case ResourceType.MRG:
                case ResourceType.BRES:
                case ResourceType.BRESGroup:
                    foreach (ResourceNode n in node.Children)
                        LoadAnims(n);
                    break;

                case ResourceType.CHR0:
                    listAnims.Items.Add(new ListViewItem(node.Name, (int)node.ResourceType, _CHRGroup) { Tag = node });
                    break;
            }
        }

        private void UpdatePropDisplay()
        {
            grpTransAll.Enabled = _enableTransform && (_selectedAnim != null);
            btnInsert.Enabled = btnDelete.Enabled = btnClear.Enabled = _animFrame != 0;
            grpCHRTransform.Enabled = _enableTransform && (_transformObject != null);
            for (int i = 0; i < 9; i++)
                ResetBox(i);
        }

        public unsafe void ResetBox2(int index) //Will be for SRT0 editing. Index 0 - 3
        {
            NumericInputBox box = _texBoxes[index];

            if (_transformObject is MDL0TextureNode)
            {
                MDL0BoneNode bone = _transformObject as MDL0BoneNode;
                CHR0EntryNode entry;
                if ((_selectedAnim != null) && (_animFrame > 0) && ((entry = _selectedAnim.FindChild(bone.Name, false) as CHR0EntryNode) != null))
                {
                    KeyframeEntry e = entry.Keyframes.GetKeyframe((KeyFrameMode)index + 0x10, _animFrame - 1);
                    if (e == null)
                    {
                        box.Value = entry.Keyframes[KeyFrameMode.ScaleX + index, _animFrame - 1];
                        box.BackColor = Color.White;
                    }
                    else
                    {
                        box.Value = e._value;
                        box.BackColor = Color.Yellow;
                    }
                }
                else
                {
                    FrameState state = bone._bindState;
                    box.Value = ((float*)&state)[index];
                    box.BackColor = Color.White;
                }
            }
            else
            {
                box.Value = 0;
                box.BackColor = Color.White;
            }
        }

        public unsafe void ResetBox(int index)
        {
            NumericInputBox box = _transBoxes[index];

            if (_transformObject is MDL0BoneNode)
            {
                MDL0BoneNode bone = _transformObject as MDL0BoneNode;
                CHR0EntryNode entry;
                if ((_selectedAnim != null) && (_animFrame > 0) && ((entry = _selectedAnim.FindChild(bone.Name, false) as CHR0EntryNode) != null))
                {
                    KeyframeEntry e = entry.Keyframes.GetKeyframe((KeyFrameMode)index + 0x10, _animFrame - 1);
                    if (e == null)
                    {
                        box.Value = entry.Keyframes[KeyFrameMode.ScaleX + index, _animFrame - 1];
                        box.BackColor = Color.White;
                    }
                    else
                    {
                        box.Value = e._value;
                        box.BackColor = Color.Yellow;
                    }
                }
                else
                {
                    FrameState state = bone._bindState;
                    box.Value = ((float*)&state)[index];
                    box.BackColor = Color.White;
                }
            }
            else
            {
                box.Value = 0;
                box.BackColor = Color.White;
            }
        }

        public unsafe void Undo(SaveState2 save)
        {
            numTransX.Value = save.frameState._translate._x;
            BoxChanged(numTransX, null);
            numTransY.Value = save.frameState._translate._y;
            BoxChanged(numTransY, null);
            numTransZ.Value = save.frameState._translate._z;
            BoxChanged(numTransZ, null);

            numRotX.Value = save.frameState._rotate._x;
            BoxChanged(numRotX, null);
            numRotY.Value = save.frameState._rotate._y;
            BoxChanged(numRotY, null);
            numRotZ.Value = save.frameState._rotate._z;
            BoxChanged(numRotZ, null);

            numScaleX.Value = save.frameState._scale._x;
            BoxChanged(numScaleX, null);
            numScaleY.Value = save.frameState._scale._y;
            BoxChanged(numScaleY, null);
            numScaleZ.Value = save.frameState._scale._z;
            BoxChanged(numScaleZ, null);
        }

        internal unsafe void BoxChangedCreateUndo(object sender, EventArgs e)
        {
            if (CreateUndo != null)
                CreateUndo(sender, null);

            //Only update for input boxes: Methods affecting multiple values call BoxChanged on their own.
            if (sender.GetType() == typeof(NumericInputBox))        
                BoxChanged(sender, null);
        }

        public unsafe void ApplySave(SaveState save)
        {
            _transformObject = save.bone;
            if (save.animation != null)
            {
                CHR0EntryNode entry = null;
                if (save.bone != null)
                    entry = save.animation.FindChild(save.bone.Name, false) as CHR0EntryNode;
                _selectedAnim = save.animation;
                if (save.undo) //Do the opposite of what the booleans say.
                {
                    //Console.WriteLine("Undo");
                    if (save.newEntry)
                        save.animation.RemoveChild(entry);

                    if (save.keyframeRemoved && save.boxIndex != -1)
                        if (save.primarySave)
                            entry.SetKeyframe(KeyFrameMode.ScaleX + save.boxIndex, save.frameIndex - 1, save.oldBoxValues[save.boxIndex]);
                        else
                            entry.SetKeyframe(KeyFrameMode.ScaleX + save.boxIndex, save.frameIndex - 1, save.newBoxValues[save.boxIndex]);

                    if (save.keyframeSet && save.boxIndex != -1)
                    {
                        entry.RemoveKeyframe(KeyFrameMode.ScaleX + save.boxIndex, save.frameIndex - 1);
                        entry.SetKeyframe(KeyFrameMode.ScaleX + save.boxIndex, save.frameIndex - 1, save.oldBoxValues[save.boxIndex]);
                    }

                    if (save.animPorted && save.oldAnimation != null)
                        _targetModel.ApplyCHR(_selectedAnim = save.oldAnimation, _animFrame = save.frameIndex);
                }
                //Follow what the booleans say, the opposite of undo. 
                //This is because undo will already have been called.
                if (save.redo)
                {
                    //Console.WriteLine("Redo");
                    if (save.newEntry)
                    {
                        entry = save.animation.CreateEntry();
                        entry.Name = save.bone.Name;

                        //Set initial values (so they aren't null)
                        FrameState state = save.oldFrameState; //Get the bone's bindstate
                        float* p = (float*)&state;
                        for (int i = 0; i < 3; i++) //Get the scale
                            if (p[i] != 1.0f)
                                entry.SetKeyframe(KeyFrameMode.ScaleX + i, 0, p[i]);
                        for (int i = 3; i < 9; i++) //Get rotation and translation respectively
                            if (p[i] != 0.0f)
                                entry.SetKeyframe(KeyFrameMode.ScaleX + i, 0, p[i]);
                        //Finally, replace the changed value
                        entry.SetKeyframe(KeyFrameMode.ScaleX + save.boxIndex, save.frameIndex - 1, save.newBoxValues[save.boxIndex]);
                    }

                    if (save.keyframeRemoved)
                        entry.RemoveKeyframe(KeyFrameMode.ScaleX + save.boxIndex, save.frameIndex - 1);

                    if (save.keyframeSet)
                        entry.SetKeyframe(KeyFrameMode.ScaleX + save.boxIndex, save.frameIndex - 1, save.newBoxValues[save.boxIndex]);
                }

                if (save.animation != null && !save.animPorted)
                    _targetModel.ApplyCHR(_selectedAnim = save.animation, _animFrame = save.frameIndex);

                if (SelectedAnimationChanged != null)
                    SelectedAnimationChanged(this, null);

                if (save.boxIndex != -1)
                    ResetBox(save.boxIndex);
            }
            else
            {
                if (save.undo)
                    save.bone._bindState = save.oldFrameState;
                if (save.redo)
                    save.bone._bindState = save.newFrameState;
                //save.bone.RecalcBindState();
            }
        }
        public bool _rotating = false;
        //public bool check = false, removed = false;
        internal unsafe void BoxChanged(object sender, EventArgs e)
        {
            if (_transformObject == null)
                return;

            NumericInputBox box = sender as NumericInputBox;
            int index = (int)box.Tag;

            if (_transformObject is MDL0BoneNode)
            {
                MDL0BoneNode bone = _transformObject as MDL0BoneNode;

                //SaveState save = new SaveState();

                //save.bone = bone;
                //save.id = _targetModel.Saves.Count;

                if ((_selectedAnim != null) && (_animFrame > 0))
                {
                    //save.animation = _selectedAnim;

                    //Find bone anim and change transform
                    CHR0EntryNode entry = _selectedAnim.FindChild(bone.Name, false) as CHR0EntryNode;

                    if (entry == null) //Create new bone animation
                    {
                        if (!float.IsNaN(box.Value))
                        {
                            //save.oldBoxValues[index] = box._oldValue;
                            //save.newBoxValues[index] = box.Value;

                            //save.newEntry = true;

                            entry = _selectedAnim.CreateEntry();
                            entry.Name = bone.Name;

                            //Set initial values (so they aren't null)
                            FrameState state = bone._bindState; //Get the bone's bindstate
                            float* p = (float*)&state;
                            for (int i = 0; i < 3; i++) //Get the scale
                                if (p[i] != 1.0f) //Check for default values
                                    entry.SetKeyframe(KeyFrameMode.ScaleX + i, 0, p[i]);
                            for (int i = 3; i < 9; i++) //Get rotation and translation respectively
                                if (p[i] != 0.0f) //Check for default values
                                    entry.SetKeyframe(KeyFrameMode.ScaleX + i, 0, p[i]);
                            //Finally, replace with the changed value
                            entry.SetKeyframe(KeyFrameMode.ScaleX + index, _animFrame - 1, box.Value);

                            //save.keyframeSet = true;
                            //save.frameIndex = _animFrame;
                        }
                    }
                    else //Set to existing chr0 entry 
                    {
                        if (float.IsNaN(box.Value))
                        {
                            entry.RemoveKeyframe(KeyFrameMode.ScaleX + index, _animFrame - 1);

                            //save.keyframeRemoved = true;
                            //save.frameIndex = _animFrame;
                            //save.oldBoxValues[index] = box._oldValue;
                            //save.newBoxValues[index] = box.Value;
                        }
                        else
                        {
                            entry.SetKeyframe(KeyFrameMode.ScaleX + index, _animFrame - 1, box.Value);

                            //save.keyframeSet = true;
                            //save.frameIndex = _animFrame;
                            //save.oldBoxValues[index] = box._oldValue;
                            //save.newBoxValues[index] = box.Value;
                        }
                    }
                }
                else
                {
                    //Change base transform
                    FrameState state = bone._bindState;
                    //save.oldFrameState = bone._bindState;
                    float* p = (float*)&state;
                    p[index] = float.IsNaN(box.Value) ? (index > 2 ? 0.0f : 1.0f) : box.Value;
                    state.CalcTransforms();
                    bone._bindState = state;
                    //bone.RecalcBindState();
                    bone.SignalPropertyChange();
                    //save.newFrameState = bone._bindState;
                    //save.frameIndex = 0;
                }

                //if (_targetModel._primarySave)
                //{
                //    save.primarySave = true;
                //    _targetModel.Saves.Add(save); 
                //    _targetModel._primarySave = false;
                //}
                //else if (_targetModel._canRedo) //Get rid of those old redo saves!
                //{
                //    _targetModel.Saves.RemoveRange(_targetModel._currentSave + 1, _targetModel.Saves.Count - (_targetModel._currentSave + 1));
                //    _targetModel._canRedo = false;
                //}

                //if (!_rotating)
                //{
                //    _targetModel.Saves.Add(save);
                //    _targetModel._saveIndex++;
                //    _targetModel._canUndo = true;
                //}

                _targetModel.ApplyCHR(_selectedAnim, _animFrame);
                ResetBox(index);
                if (RenderStateChanged != null)
                    RenderStateChanged(this, null);
            }
        }

        private bool LoadExternal()
        {
            int count;
            dlgOpen.Filter = "All Compatible Files (*.pac, *.pcs, *.brres, *.chr0, *.mrg)|*.pac;*.pcs;*.brres;*.chr0;*.mrg";
            if (dlgOpen.ShowDialog() == DialogResult.OK)
            {
                ResourceNode node = null;
                listAnims.BeginUpdate();
                try
                {
                    if ((node = NodeFactory.FromFile(null, dlgOpen.FileName)) != null)
                    {
                        if (!CloseExternal())
                            return false;

                        count = listAnims.Items.Count;
                        LoadAnims(node);

                        if (count == listAnims.Items.Count)
                            MessageBox.Show(this, "No animations could be found in external file, closing.", "Error");
                        else
                        {
                            _externalNode = node;
                            node = null;
                            txtExtPath.Text = Path.GetFileName(dlgOpen.FileName);

                            if (ReferenceLoaded != null)
                                ReferenceLoaded(_externalNode);

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
                    listAnims.EndUpdate();
                }
            }
            return false;
        }
        private bool CloseExternal()
        {
            if (_externalNode != null)
            {
                if (_externalNode.IsDirty)
                {
                    DialogResult res = MessageBox.Show(this, "You have made changes to an external file. Would you like to save those changes?", "Closing external file.", MessageBoxButtons.YesNoCancel);
                    if (((res == DialogResult.Yes) && (!SaveExternal(false))) || (res == DialogResult.Cancel))
                        return false;
                }
                if (ReferenceClosed != null)
                    ReferenceClosed(_externalNode);

                _externalNode.Dispose();
                _externalNode = null;
                txtExtPath.Text = "";
                UpdateReferences();
            }
            return true;
        }
        private bool SaveExternal(bool As)
        {
            if ((_externalNode == null) || ((!_externalNode.IsDirty) && !As))
                return true;

            try
            {
                if (As)
                {
                    FolderBrowserDialog dialog = new FolderBrowserDialog();
                    dialog.Description = "Please choose a location to save this file.";
                    dialog.SelectedPath = _externalNode._origPath;
                    if (dialog.ShowDialog() == DialogResult.OK) 
                    {
                        _externalNode.Merge();
                        _externalNode.Export(dialog.SelectedPath + "\\" + Path.GetFileName(_externalNode._origPath));
                    }
                }
                else
                {
                    _externalNode.Merge();
                    _externalNode.Export(_externalNode._origPath);
                }
                return true;
            }
            catch (Exception x) { MessageBox.Show(this, x.ToString()); }
            return false;
        }

        private void btnOpenClose_Click(object sender, EventArgs e) 
        {
            if (btnOpenClose.Text == "Load")
            {
                if (LoadExternal()) 
                    btnOpenClose.Text = "Close";
            }
            else if (btnOpenClose.Text == "Close")
            {
                if (CloseExternal()) 
                    btnOpenClose.Text = "Load";
            }
        }
        private void btnSave_Click(object sender, EventArgs e) { SaveExternal(false); }
        private void btnSaveAs_Click(object sender, EventArgs e) { SaveExternal(true); }
        private void txtExtPath_Click(object sender, EventArgs e) { if (btnOpenClose.Text == "Load") btnOpenClose_Click(null, null); }

        private void listAnims_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listAnims.SelectedItems.Count > 0)
            {
                _selectedAnim = listAnims.SelectedItems[0].Tag as CHR0Node;

                BRESNode group = null;
                if (_selectedAnim != null && (group = _selectedAnim.Parent.Parent as BRESNode) != null)
                    _selectedTexAnim = (SRT0Node)group.FindChildByType(_selectedAnim.Name, true, ResourceType.SRT0);
            }
            else
            {
                _selectedAnim = null;
                _selectedTexAnim = null;
            }

            if (_selectedAnim == null)
                btnInsert.Enabled = btnDelete.Enabled = btnClear.Enabled = false;
            else
                btnInsert.Enabled = btnDelete.Enabled = btnClear.Enabled = true;

            _copyAllIndex = -1;
            //if (_selectedAnim != null)
            //    portToolStripMenuItem.Enabled = !_selectedAnim.IsPorted;
            if (SelectedAnimationChanged != null)
                SelectedAnimationChanged(this, null);
        }

        private unsafe void btnCut_Click(object sender, EventArgs e)
        {
            AnimationFrame frame;
            float* p = (float*)&frame;

            BoxChangedCreateUndo(this, null);

            for (int i = 0; i < 9; i++)
            {
                p[i] = _transBoxes[i].Value;
                _transBoxes[i].Value = float.NaN;
                BoxChanged(_transBoxes[i], null);
            }

            _tempFrame = frame;
        }

        private unsafe void btnCopy_Click(object sender, EventArgs e)
        {
            AnimationFrame frame;
            float* p = (float*)&frame;

            for (int i = 0; i < 9; i++)
                p[i] = _transBoxes[i].Value;

            _tempFrame = frame;
            //Clipboard.SetData("AnimationFrame", frame);
        }

        private unsafe void btnPaste_Click(object sender, EventArgs e)
        {
            //AnimationFrame copyFrame = (AnimationFrame)Clipboard.GetData("AnimationFrame");

            AnimationFrame frame = _tempFrame;
            float* p = (float*)&frame;

            BoxChangedCreateUndo(this, null);

            for (int i = 0; i < 9; i++)
            {
                _transBoxes[i].Value = p[i];
                BoxChanged(_transBoxes[i], null);
            }
        }

        private void ctxAnim_Opening(object sender, CancelEventArgs e)
        {
            if (_selectedAnim == null)
                e.Cancel = true;
            else
            {
                sourceToolStripMenuItem.Text = String.Format("Source: {0}", Path.GetFileName(_selectedAnim.RootNode._origPath));
            }
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_selectedAnim == null)
                return;

            dlgSave.FileName = _selectedAnim.Name;
            dlgSave.Filter = ExportFilters.CHR0;
            if (dlgSave.ShowDialog() == DialogResult.OK)
            {
                _selectedAnim.Export(dlgSave.FileName);
            }
        }

        private void replaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_selectedAnim == null)
                return;

            dlgOpen.Filter = ExportFilters.CHR0;
            if (dlgOpen.ShowDialog() == DialogResult.OK)
            {
                _selectedAnim.Replace(dlgOpen.FileName);

                if (SelectedAnimationChanged != null)
                    SelectedAnimationChanged(this, null);
            }
        }

        private unsafe void portToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_selectedAnim == null || (_selectedAnim.IsPorted && MessageBox.Show("This animation has already been ported!\nDo you still want to continue?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No))
                return;

            MDL0Node model;

            OpenFileDialog dlgOpen = new OpenFileDialog();
            dlgOpen.Filter = "MDL0 Raw Model (*.mdl0)|*.mdl0";
            dlgOpen.Title = "Select the model this animation is for...";

            //SaveState save = new SaveState();
            //save.oldAnimation = _selectedAnim;
            //save.frameIndex = _animFrame;

            if (dlgOpen.ShowDialog() == DialogResult.OK)
                if ((model = (MDL0Node)NodeFactory.FromFile(null, dlgOpen.FileName)) != null)
                    _selectedAnim.Port(_targetModel, model);

            if (SelectedAnimationChanged != null)
                SelectedAnimationChanged(this, null);

            //save.animation = _selectedAnim;

            //_targetModel.Saves.Add(save);
            //save.animPorted = false;
            //_targetModel.Saves.Add(save);
            //_targetModel._saveIndex++;
            //_targetModel._canUndo = true;
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            if ((_selectedAnim == null) || (_animFrame == 0))
                return;

            _selectedAnim.InsertKeyframe(_animFrame - 1);
            if (AnimStateChanged != null)
                AnimStateChanged(this, null);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if ((_selectedAnim == null) || (_animFrame == 0))
                return;

            _selectedAnim.DeleteKeyframe(_animFrame - 1);
            if (AnimStateChanged != null)
                AnimStateChanged(this, null);
        }

        private int _copyAllIndex = -1;

        private static Dictionary<string, AnimationFrame> _copyAllState = new Dictionary<string, AnimationFrame>();

        private void btnCopyAll_Click(object sender, EventArgs e)
        {
            _copyAllState.Clear();

            if (_animFrame == 0)
                foreach (MDL0BoneNode bone in _targetModel.FindChildrenByType("Bones", ResourceType.MDL0Bone))
                    _copyAllState[bone._name] = (AnimationFrame)bone._bindState;
            else
                foreach (CHR0EntryNode entry in _selectedAnim.Children)
                    _copyAllState[entry._name] = entry.GetAnimFrame(_animFrame - 1);
        }

        private void btnPasteAll_Click(object sender, EventArgs e)
        {
            if (_copyAllState.Count == 0)
                return;

            if (_animFrame == 0)
            {
                foreach (MDL0BoneNode bone in _targetModel.FindChildrenByType("Bones", ResourceType.MDL0Bone))
                    if (_copyAllState.ContainsKey(bone._name))
                    {
                        if (Translate.Checked)
                            bone._bindState._translate = _copyAllState[bone._name].Translation;
                        if (Rotate.Checked)
                            bone._bindState._rotate = _copyAllState[bone._name].Rotation;
                        if (Scale.Checked)
                            bone._bindState._scale = _copyAllState[bone._name].Scale;
                        //bone.RecalcBindState();
                        bone.SignalPropertyChange();
                    }
            }
            else
                foreach (CHR0EntryNode entry in _selectedAnim.Children)
                    if (_copyAllState.ContainsKey(entry._name))
                    {
                        if (Translate.Checked)
                            entry.SetKeyframeOnlyTrans(_animFrame - 1, _copyAllState[entry._name]);
                        if (Rotate.Checked)
                            entry.SetKeyframeOnlyRot(_animFrame - 1, _copyAllState[entry._name]);
                        if (Scale.Checked)
                            entry.SetKeyframeOnlyScale(_animFrame - 1, _copyAllState[entry._name]);
                    }

            UpdateModel();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            if (_animFrame == 0)
                return;

            foreach (CHR0EntryNode entry in _selectedAnim.Children)
            {
                if (Translate.Checked)
                    entry.RemoveKeyframeOnlyTrans(_animFrame - 1);
                if (Rotate.Checked)
                    entry.RemoveKeyframeOnlyRot(_animFrame - 1);
                if (Scale.Checked)
                    entry.RemoveKeyframeOnlyScale(_animFrame - 1);
            }

            UpdateModel();
        }

        private void btnClean_Click(object sender, EventArgs e)
        {
            ResourceNode group = _targetModel._boneGroup;
            if (group == null)
                return;

            List<CHR0EntryNode> badNodes = new List<CHR0EntryNode>();
            foreach (CHR0EntryNode entry in _selectedAnim.Children)
            {
                if (group.FindChild(entry._name, true) == null)
                    badNodes.Add(entry);
                else
                    entry.Keyframes.Clean();
            }
            int temp = badNodes.Count;
            foreach (CHR0EntryNode n in badNodes)
            {
                n.Remove();
                n.Dispose();
            }
            MessageBox.Show(temp + " unused nodes removed.");
            UpdatePropDisplay();
        }

        private void ctxBox_Opening(object sender, CancelEventArgs e)
        {
            if (_selectedAnim == null || label8.Enabled == false || label9.Enabled == false || label10.Enabled == false)
                e.Cancel = true;
        }
        public int type = 0;
        private void label8_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == Forms.MouseButtons.Right)
            {
                type = 0x13;
                if (label8.Enabled == true)
                {
                    label8.ContextMenuStrip = ctxBox;
                    Source.Text = label8.Text;
                }
                else
                    label8.ContextMenuStrip = null;
            }
        }

        private void label9_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == Forms.MouseButtons.Right)
            {
                type = 0x14;
                if (label9.Enabled == true)
                {
                    label9.ContextMenuStrip = ctxBox;
                    Source.Text = label9.Text;
                }
                else
                    label9.ContextMenuStrip = null;
            }
        }

        private void label10_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == Forms.MouseButtons.Right)
            {
                type = 0x15;
                if (label10.Enabled == true)
                {
                    label10.ContextMenuStrip = ctxBox;
                    Source.Text = label10.Text;
                }
                else
                    label10.ContextMenuStrip = null;
            }
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            if (_selectedAnim == null || type == 0)
                return;

            KeyframeEntry kfe;
            CHR0EntryNode _target = _selectedAnim.FindChild((_transformObject as MDL0BoneNode).Name, false) as CHR0EntryNode;
            for (int x = 0; x < _target.FrameCount; x++) //Loop thru each frame
                if ((kfe = _target.GetKeyframe((KeyFrameMode)type, x)) != null) //Check for a keyframe
                { kfe._value += 180; }
            ResetBox(type - 0x10);
            UpdateModel();
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            if (_selectedAnim == null || type == 0)
                return;

            KeyframeEntry kfe;
            CHR0EntryNode _target = _selectedAnim.FindChild((_transformObject as MDL0BoneNode).Name, false) as CHR0EntryNode;
            for (int x = 0; x < _target.FrameCount; x++) //Loop thru each frame
                if ((kfe = _target.GetKeyframe((KeyFrameMode)type, x)) != null) //Check for a keyframe
                { kfe._value += 90; }
            ResetBox(type - 0x10);
            UpdateModel();
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            if (_selectedAnim == null || type == 0)
                return;

            KeyframeEntry kfe;
            CHR0EntryNode _target = _selectedAnim.FindChild((_transformObject as MDL0BoneNode).Name, false) as CHR0EntryNode;
            for (int x = 0; x < _target.FrameCount; x++) //Loop thru each frame
                if ((kfe = _target.GetKeyframe((KeyFrameMode)type, x)) != null) //Check for a keyframe
                { kfe._value -= 180; }
            ResetBox(type - 0x10);
            UpdateModel();
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            if (_selectedAnim == null || type == 0)
                return;

            KeyframeEntry kfe;
            CHR0EntryNode _target = _selectedAnim.FindChild((_transformObject as MDL0BoneNode).Name, false) as CHR0EntryNode;
            for (int x = 0; x < _target.FrameCount; x++) //Loop thru each frame
                if ((kfe = _target.GetKeyframe((KeyFrameMode)type, x)) != null) //Check for a keyframe
                { kfe._value -= 90; }
            ResetBox(type - 0x10);
            UpdateModel();
        }

        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            if (_selectedAnim == null || type == 0)
                return;

            KeyframeEntry kfe;
            CHR0EntryNode _target = _selectedAnim.FindChild((_transformObject as MDL0BoneNode).Name, false) as CHR0EntryNode;
            for (int x = 0; x < _target.FrameCount; x++) //Loop thru each frame
                if ((kfe = _target.GetKeyframe((KeyFrameMode)type, x)) != null) //Check for a keyframe
                { kfe._value += 45; }
            ResetBox(type - 0x10);
            UpdateModel();
        }

        private void toolStripMenuItem8_Click(object sender, EventArgs e)
        {
            if (_selectedAnim == null || type == 0)
                return;

            KeyframeEntry kfe;
            CHR0EntryNode _target = _selectedAnim.FindChild((_transformObject as MDL0BoneNode).Name, false) as CHR0EntryNode;
            for (int x = 0; x < _target.FrameCount; x++) //Loop thru each frame
                if ((kfe = _target.GetKeyframe((KeyFrameMode)type, x)) != null) //Check for a keyframe
                { kfe._value -= 45; }
            ResetBox(type - 0x10);
            UpdateModel();
        }

        private void removeAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_selectedAnim == null || type == 0)
                return;

            KeyframeEntry kfe;
            CHR0EntryNode _target = _selectedAnim.FindChild((_transformObject as MDL0BoneNode).Name, false) as CHR0EntryNode;
            for (int x = 0; x < _target.FrameCount; x++) //Loop thru each frame
                if ((kfe = _target.GetKeyframe((KeyFrameMode)type, x)) != null) //Check for a keyframe
                { kfe.Remove(); }
            ResetBox(type - 0x10);
            UpdateModel();
        }

        private void label5_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == Forms.MouseButtons.Right)
            {
                type = 0x16;
                if (label5.Enabled == true)
                {
                    label5.ContextMenuStrip = ctxBox;
                    Source.Text = label5.Text;
                }
                else
                    label5.ContextMenuStrip = null;
            }
        }

        private void label6_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == Forms.MouseButtons.Right)
            {
                type = 0x17;
                if (label6.Enabled == true)
                {
                    label6.ContextMenuStrip = ctxBox;
                    Source.Text = label6.Text;
                }
                else
                    label6.ContextMenuStrip = null;
            }
        }

        private void label7_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == Forms.MouseButtons.Right)
            {
                type = 0x18;
                if (label7.Enabled == true)
                {
                    label7.ContextMenuStrip = ctxBox;
                    Source.Text = label7.Text;
                }
                else
                    label7.ContextMenuStrip = null;
            }
        }

        private void addCustomAmountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_selectedAnim == null || type == 0)
                return;

            EditDialog ed = new EditDialog();
            ed.ShowDialog(null, (KeyFrameMode)type, _selectedAnim.FindChild((_transformObject as MDL0BoneNode).Name, false) as CHR0EntryNode);
            ResetBox(type - 0x10);
            UpdateModel();
        }

        private void Translate_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void Rotate_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void Scale_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
