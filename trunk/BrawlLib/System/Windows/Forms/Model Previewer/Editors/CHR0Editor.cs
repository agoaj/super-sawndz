using System;
using BrawlLib.Wii.Animations;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.Modeling;
using System.ComponentModel;
using System.Drawing;
using System.Collections.Generic;
using BrawlLib.SSBBTypes;

namespace System.Windows.Forms
{
    public class CHR0Editor : UserControl
    {
        #region Designer
        private GroupBox grpTransform;
        private Button btnPaste;
        private Button btnCopy;
        private Button btnCut;
        private Label lblTransX;
        private NumericInputBox numScaleZ;
        internal NumericInputBox numTransX;
        private NumericInputBox numScaleY;
        private Label lblTransY;
        private NumericInputBox numScaleX;
        private Label lblTransZ;
        internal NumericInputBox numRotZ;
        private Label lblRotX;
        internal NumericInputBox numRotY;
        private Label lblRotY;
        internal NumericInputBox numRotX;
        private Label lblRotZ;
        internal NumericInputBox numTransZ;
        private Label lblScaleX;
        internal NumericInputBox numTransY;
        private Label lblScaleZ;
        private GroupBox grpTransAll;
        private CheckBox AllScale;
        private CheckBox AllRot;
        private CheckBox AllTrans;
        public Button btnClean;
        public Button btnPasteAll;
        public Button btnCopyAll;
        public Button btnClear;
        public Button btnInsert;
        public Button btnDelete;
        private CheckBox FrameScale;
        private CheckBox FrameRot;
        private CheckBox FrameTrans;
        private ContextMenuStrip ctxBox;
        private System.ComponentModel.IContainer components;
        private ToolStripMenuItem Source;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem add;
        private ToolStripMenuItem toolStripMenuItem3;
        private ToolStripMenuItem toolStripMenuItem4;
        private ToolStripMenuItem toolStripMenuItem7;
        private ToolStripMenuItem subtract;
        private ToolStripMenuItem toolStripMenuItem5;
        private ToolStripMenuItem toolStripMenuItem6;
        private ToolStripMenuItem toolStripMenuItem8;
        private ToolStripMenuItem removeAllToolStripMenuItem;
        private ToolStripMenuItem addCustomAmountToolStripMenuItem;
        private ToolStripMenuItem editRawTangentToolStripMenuItem;
        public Label labelBone;
        private Label lblScaleY;
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.grpTransform = new System.Windows.Forms.GroupBox();
            this.FrameScale = new System.Windows.Forms.CheckBox();
            this.btnPaste = new System.Windows.Forms.Button();
            this.FrameRot = new System.Windows.Forms.CheckBox();
            this.btnCopy = new System.Windows.Forms.Button();
            this.FrameTrans = new System.Windows.Forms.CheckBox();
            this.btnCut = new System.Windows.Forms.Button();
            this.numScaleX = new System.Windows.Forms.NumericInputBox();
            this.numScaleY = new System.Windows.Forms.NumericInputBox();
            this.numScaleZ = new System.Windows.Forms.NumericInputBox();
            this.numRotX = new System.Windows.Forms.NumericInputBox();
            this.numRotY = new System.Windows.Forms.NumericInputBox();
            this.numRotZ = new System.Windows.Forms.NumericInputBox();
            this.numTransX = new System.Windows.Forms.NumericInputBox();
            this.labelBone = new System.Windows.Forms.Label();
            this.numTransY = new System.Windows.Forms.NumericInputBox();
            this.numTransZ = new System.Windows.Forms.NumericInputBox();
            this.lblTransX = new System.Windows.Forms.Label();
            this.lblTransY = new System.Windows.Forms.Label();
            this.lblTransZ = new System.Windows.Forms.Label();
            this.lblRotX = new System.Windows.Forms.Label();
            this.lblRotY = new System.Windows.Forms.Label();
            this.lblRotZ = new System.Windows.Forms.Label();
            this.lblScaleX = new System.Windows.Forms.Label();
            this.lblScaleZ = new System.Windows.Forms.Label();
            this.lblScaleY = new System.Windows.Forms.Label();
            this.grpTransAll = new System.Windows.Forms.GroupBox();
            this.btnClean = new System.Windows.Forms.Button();
            this.btnPasteAll = new System.Windows.Forms.Button();
            this.btnCopyAll = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnInsert = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.AllScale = new System.Windows.Forms.CheckBox();
            this.AllRot = new System.Windows.Forms.CheckBox();
            this.AllTrans = new System.Windows.Forms.CheckBox();
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
            this.editRawTangentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.grpTransform.SuspendLayout();
            this.grpTransAll.SuspendLayout();
            this.ctxBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpTransform
            // 
            this.grpTransform.Controls.Add(this.FrameScale);
            this.grpTransform.Controls.Add(this.btnPaste);
            this.grpTransform.Controls.Add(this.FrameRot);
            this.grpTransform.Controls.Add(this.btnCopy);
            this.grpTransform.Controls.Add(this.FrameTrans);
            this.grpTransform.Controls.Add(this.btnCut);
            this.grpTransform.Controls.Add(this.numScaleX);
            this.grpTransform.Controls.Add(this.numScaleY);
            this.grpTransform.Controls.Add(this.numScaleZ);
            this.grpTransform.Controls.Add(this.numRotX);
            this.grpTransform.Controls.Add(this.numRotY);
            this.grpTransform.Controls.Add(this.numRotZ);
            this.grpTransform.Controls.Add(this.numTransX);
            this.grpTransform.Controls.Add(this.labelBone);
            this.grpTransform.Controls.Add(this.numTransY);
            this.grpTransform.Controls.Add(this.numTransZ);
            this.grpTransform.Controls.Add(this.lblTransX);
            this.grpTransform.Controls.Add(this.lblTransY);
            this.grpTransform.Controls.Add(this.lblTransZ);
            this.grpTransform.Controls.Add(this.lblRotX);
            this.grpTransform.Controls.Add(this.lblRotY);
            this.grpTransform.Controls.Add(this.lblRotZ);
            this.grpTransform.Controls.Add(this.lblScaleX);
            this.grpTransform.Controls.Add(this.lblScaleZ);
            this.grpTransform.Controls.Add(this.lblScaleY);
            this.grpTransform.Dock = System.Windows.Forms.DockStyle.Left;
            this.grpTransform.Enabled = false;
            this.grpTransform.Location = new System.Drawing.Point(0, 0);
            this.grpTransform.Name = "grpTransform";
            this.grpTransform.Size = new System.Drawing.Size(557, 82);
            this.grpTransform.TabIndex = 23;
            this.grpTransform.TabStop = false;
            this.grpTransform.Text = "Transform Frame";
            // 
            // FrameScale
            // 
            this.FrameScale.AutoSize = true;
            this.FrameScale.Checked = true;
            this.FrameScale.CheckState = System.Windows.Forms.CheckState.Checked;
            this.FrameScale.Location = new System.Drawing.Point(500, 54);
            this.FrameScale.Name = "FrameScale";
            this.FrameScale.Size = new System.Drawing.Size(53, 17);
            this.FrameScale.TabIndex = 35;
            this.FrameScale.Text = "Scale";
            this.FrameScale.UseVisualStyleBackColor = true;
            // 
            // btnPaste
            // 
            this.btnPaste.Location = new System.Drawing.Point(446, 54);
            this.btnPaste.Name = "btnPaste";
            this.btnPaste.Size = new System.Drawing.Size(48, 20);
            this.btnPaste.TabIndex = 23;
            this.btnPaste.Text = "Paste";
            this.btnPaste.UseVisualStyleBackColor = true;
            this.btnPaste.Click += new System.EventHandler(this.btnPaste_Click);
            // 
            // FrameRot
            // 
            this.FrameRot.AutoSize = true;
            this.FrameRot.Checked = true;
            this.FrameRot.CheckState = System.Windows.Forms.CheckState.Checked;
            this.FrameRot.Location = new System.Drawing.Point(500, 34);
            this.FrameRot.Name = "FrameRot";
            this.FrameRot.Size = new System.Drawing.Size(43, 17);
            this.FrameRot.TabIndex = 34;
            this.FrameRot.Text = "Rot";
            this.FrameRot.UseVisualStyleBackColor = true;
            // 
            // btnCopy
            // 
            this.btnCopy.Location = new System.Drawing.Point(446, 33);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(48, 20);
            this.btnCopy.TabIndex = 22;
            this.btnCopy.Text = "Copy";
            this.btnCopy.UseVisualStyleBackColor = true;
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // FrameTrans
            // 
            this.FrameTrans.AutoSize = true;
            this.FrameTrans.Checked = true;
            this.FrameTrans.CheckState = System.Windows.Forms.CheckState.Checked;
            this.FrameTrans.Location = new System.Drawing.Point(500, 13);
            this.FrameTrans.Name = "FrameTrans";
            this.FrameTrans.Size = new System.Drawing.Size(53, 17);
            this.FrameTrans.TabIndex = 33;
            this.FrameTrans.Text = "Trans";
            this.FrameTrans.UseVisualStyleBackColor = true;
            // 
            // btnCut
            // 
            this.btnCut.Location = new System.Drawing.Point(446, 12);
            this.btnCut.Name = "btnCut";
            this.btnCut.Size = new System.Drawing.Size(48, 20);
            this.btnCut.TabIndex = 21;
            this.btnCut.Text = "Cut";
            this.btnCut.UseVisualStyleBackColor = true;
            this.btnCut.Click += new System.EventHandler(this.btnCut_Click);
            // 
            // numScaleX
            // 
            this.numScaleX.Location = new System.Drawing.Point(358, 14);
            this.numScaleX.Name = "numScaleX";
            this.numScaleX.Size = new System.Drawing.Size(82, 20);
            this.numScaleX.TabIndex = 18;
            this.numScaleX.Text = "0";
            this.numScaleX.ValueChanged += new System.EventHandler(this.BoxChangedCreateUndo);
            // 
            // numScaleY
            // 
            this.numScaleY.Location = new System.Drawing.Point(358, 34);
            this.numScaleY.Name = "numScaleY";
            this.numScaleY.Size = new System.Drawing.Size(82, 20);
            this.numScaleY.TabIndex = 19;
            this.numScaleY.Text = "0";
            this.numScaleY.ValueChanged += new System.EventHandler(this.BoxChangedCreateUndo);
            // 
            // numScaleZ
            // 
            this.numScaleZ.Location = new System.Drawing.Point(358, 54);
            this.numScaleZ.Name = "numScaleZ";
            this.numScaleZ.Size = new System.Drawing.Size(82, 20);
            this.numScaleZ.TabIndex = 20;
            this.numScaleZ.Text = "0";
            this.numScaleZ.ValueChanged += new System.EventHandler(this.BoxChangedCreateUndo);
            // 
            // numRotX
            // 
            this.numRotX.Location = new System.Drawing.Point(220, 15);
            this.numRotX.Name = "numRotX";
            this.numRotX.Size = new System.Drawing.Size(82, 20);
            this.numRotX.TabIndex = 15;
            this.numRotX.Text = "0";
            this.numRotX.ValueChanged += new System.EventHandler(this.BoxChangedCreateUndo);
            // 
            // numRotY
            // 
            this.numRotY.Location = new System.Drawing.Point(220, 35);
            this.numRotY.Name = "numRotY";
            this.numRotY.Size = new System.Drawing.Size(82, 20);
            this.numRotY.TabIndex = 16;
            this.numRotY.Text = "0";
            this.numRotY.ValueChanged += new System.EventHandler(this.BoxChangedCreateUndo);
            // 
            // numRotZ
            // 
            this.numRotZ.Location = new System.Drawing.Point(220, 55);
            this.numRotZ.Name = "numRotZ";
            this.numRotZ.Size = new System.Drawing.Size(82, 20);
            this.numRotZ.TabIndex = 17;
            this.numRotZ.Text = "0";
            this.numRotZ.ValueChanged += new System.EventHandler(this.BoxChangedCreateUndo);
            // 
            // numTransX
            // 
            this.numTransX.Location = new System.Drawing.Point(76, 16);
            this.numTransX.Name = "numTransX";
            this.numTransX.Size = new System.Drawing.Size(82, 20);
            this.numTransX.TabIndex = 3;
            this.numTransX.Text = "0";
            this.numTransX.ValueChanged += new System.EventHandler(this.BoxChangedCreateUndo);
            // 
            // labelBone
            // 
            this.labelBone.AutoSize = true;
            this.labelBone.Location = new System.Drawing.Point(100, 1);
            this.labelBone.Name = "labelBone";
            this.labelBone.Size = new System.Drawing.Size(0, 13);
            this.labelBone.TabIndex = 36;
            // 
            // numTransY
            // 
            this.numTransY.Location = new System.Drawing.Point(76, 36);
            this.numTransY.Name = "numTransY";
            this.numTransY.Size = new System.Drawing.Size(82, 20);
            this.numTransY.TabIndex = 13;
            this.numTransY.Text = "0";
            this.numTransY.ValueChanged += new System.EventHandler(this.BoxChangedCreateUndo);
            // 
            // numTransZ
            // 
            this.numTransZ.Location = new System.Drawing.Point(76, 56);
            this.numTransZ.Name = "numTransZ";
            this.numTransZ.Size = new System.Drawing.Size(82, 20);
            this.numTransZ.TabIndex = 14;
            this.numTransZ.Text = "0";
            this.numTransZ.ValueChanged += new System.EventHandler(this.BoxChangedCreateUndo);
            // 
            // lblTransX
            // 
            this.lblTransX.Location = new System.Drawing.Point(2, 16);
            this.lblTransX.Name = "lblTransX";
            this.lblTransX.Size = new System.Drawing.Size(74, 20);
            this.lblTransX.TabIndex = 4;
            this.lblTransX.Text = "Translation X:";
            this.lblTransX.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblTransX.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lblTransX_MouseDown);
            // 
            // lblTransY
            // 
            this.lblTransY.Location = new System.Drawing.Point(2, 36);
            this.lblTransY.Name = "lblTransY";
            this.lblTransY.Size = new System.Drawing.Size(74, 20);
            this.lblTransY.TabIndex = 5;
            this.lblTransY.Text = "Translation Y:";
            this.lblTransY.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblTransY.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lblTransY_MouseDown);
            // 
            // lblTransZ
            // 
            this.lblTransZ.Location = new System.Drawing.Point(2, 56);
            this.lblTransZ.Name = "lblTransZ";
            this.lblTransZ.Size = new System.Drawing.Size(74, 20);
            this.lblTransZ.TabIndex = 6;
            this.lblTransZ.Text = "Translation Z:";
            this.lblTransZ.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblTransZ.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lblTransZ_MouseDown);
            // 
            // lblRotX
            // 
            this.lblRotX.Location = new System.Drawing.Point(160, 16);
            this.lblRotX.Name = "lblRotX";
            this.lblRotX.Size = new System.Drawing.Size(60, 20);
            this.lblRotX.TabIndex = 7;
            this.lblRotX.Text = "Rotation X:";
            this.lblRotX.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblRotX.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lblRotX_MouseDown);
            // 
            // lblRotY
            // 
            this.lblRotY.Location = new System.Drawing.Point(160, 36);
            this.lblRotY.Name = "lblRotY";
            this.lblRotY.Size = new System.Drawing.Size(60, 20);
            this.lblRotY.TabIndex = 8;
            this.lblRotY.Text = "Rotation Y:";
            this.lblRotY.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblRotY.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lblRotY_MouseDown);
            // 
            // lblRotZ
            // 
            this.lblRotZ.Location = new System.Drawing.Point(160, 56);
            this.lblRotZ.Name = "lblRotZ";
            this.lblRotZ.Size = new System.Drawing.Size(60, 20);
            this.lblRotZ.TabIndex = 9;
            this.lblRotZ.Text = "Rotation Z:";
            this.lblRotZ.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblRotZ.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lblRotZ_MouseDown);
            // 
            // lblScaleX
            // 
            this.lblScaleX.Location = new System.Drawing.Point(305, 14);
            this.lblScaleX.Name = "lblScaleX";
            this.lblScaleX.Size = new System.Drawing.Size(47, 20);
            this.lblScaleX.TabIndex = 10;
            this.lblScaleX.Text = "Scale X:";
            this.lblScaleX.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblScaleX.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lblScaleX_MouseDown);
            // 
            // lblScaleZ
            // 
            this.lblScaleZ.Location = new System.Drawing.Point(305, 54);
            this.lblScaleZ.Name = "lblScaleZ";
            this.lblScaleZ.Size = new System.Drawing.Size(47, 20);
            this.lblScaleZ.TabIndex = 11;
            this.lblScaleZ.Text = "Scale Z:";
            this.lblScaleZ.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblScaleZ.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lblScaleZ_MouseDown);
            // 
            // lblScaleY
            // 
            this.lblScaleY.Location = new System.Drawing.Point(305, 34);
            this.lblScaleY.Name = "lblScaleY";
            this.lblScaleY.Size = new System.Drawing.Size(47, 20);
            this.lblScaleY.TabIndex = 12;
            this.lblScaleY.Text = "Scale Y:";
            this.lblScaleY.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblScaleY.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lblScaleY_MouseDown);
            // 
            // grpTransAll
            // 
            this.grpTransAll.Controls.Add(this.btnClean);
            this.grpTransAll.Controls.Add(this.btnPasteAll);
            this.grpTransAll.Controls.Add(this.btnCopyAll);
            this.grpTransAll.Controls.Add(this.btnClear);
            this.grpTransAll.Controls.Add(this.btnInsert);
            this.grpTransAll.Controls.Add(this.btnDelete);
            this.grpTransAll.Controls.Add(this.AllScale);
            this.grpTransAll.Controls.Add(this.AllRot);
            this.grpTransAll.Controls.Add(this.AllTrans);
            this.grpTransAll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpTransAll.Enabled = false;
            this.grpTransAll.Location = new System.Drawing.Point(557, 0);
            this.grpTransAll.Name = "grpTransAll";
            this.grpTransAll.Size = new System.Drawing.Size(175, 82);
            this.grpTransAll.TabIndex = 26;
            this.grpTransAll.TabStop = false;
            this.grpTransAll.Text = "Transform All";
            // 
            // btnClean
            // 
            this.btnClean.Location = new System.Drawing.Point(116, 35);
            this.btnClean.Name = "btnClean";
            this.btnClean.Size = new System.Drawing.Size(50, 20);
            this.btnClean.TabIndex = 29;
            this.btnClean.Text = "Clean";
            this.btnClean.UseVisualStyleBackColor = true;
            this.btnClean.Click += new System.EventHandler(this.btnClean_Click);
            // 
            // btnPasteAll
            // 
            this.btnPasteAll.Location = new System.Drawing.Point(60, 35);
            this.btnPasteAll.Name = "btnPasteAll";
            this.btnPasteAll.Size = new System.Drawing.Size(50, 20);
            this.btnPasteAll.TabIndex = 28;
            this.btnPasteAll.Text = "Paste";
            this.btnPasteAll.UseVisualStyleBackColor = true;
            this.btnPasteAll.Click += new System.EventHandler(this.btnPasteAll_Click);
            // 
            // btnCopyAll
            // 
            this.btnCopyAll.Location = new System.Drawing.Point(60, 57);
            this.btnCopyAll.Name = "btnCopyAll";
            this.btnCopyAll.Size = new System.Drawing.Size(50, 20);
            this.btnCopyAll.TabIndex = 27;
            this.btnCopyAll.Text = "Copy";
            this.btnCopyAll.UseVisualStyleBackColor = true;
            this.btnCopyAll.Click += new System.EventHandler(this.btnCopyAll_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(116, 13);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(50, 20);
            this.btnClear.TabIndex = 26;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnInsert
            // 
            this.btnInsert.Location = new System.Drawing.Point(116, 57);
            this.btnInsert.Name = "btnInsert";
            this.btnInsert.Size = new System.Drawing.Size(50, 20);
            this.btnInsert.TabIndex = 24;
            this.btnInsert.Text = "Insert";
            this.btnInsert.UseVisualStyleBackColor = true;
            this.btnInsert.Click += new System.EventHandler(this.btnInsert_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(60, 13);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(50, 20);
            this.btnDelete.TabIndex = 25;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // AllScale
            // 
            this.AllScale.AutoSize = true;
            this.AllScale.Checked = true;
            this.AllScale.CheckState = System.Windows.Forms.CheckState.Checked;
            this.AllScale.Location = new System.Drawing.Point(8, 55);
            this.AllScale.Name = "AllScale";
            this.AllScale.Size = new System.Drawing.Size(53, 17);
            this.AllScale.TabIndex = 32;
            this.AllScale.Text = "Scale";
            this.AllScale.UseVisualStyleBackColor = true;
            this.AllScale.CheckedChanged += new System.EventHandler(this.AllScale_CheckedChanged);
            // 
            // AllRot
            // 
            this.AllRot.AutoSize = true;
            this.AllRot.Checked = true;
            this.AllRot.CheckState = System.Windows.Forms.CheckState.Checked;
            this.AllRot.Location = new System.Drawing.Point(8, 38);
            this.AllRot.Name = "AllRot";
            this.AllRot.Size = new System.Drawing.Size(43, 17);
            this.AllRot.TabIndex = 31;
            this.AllRot.Text = "Rot";
            this.AllRot.UseVisualStyleBackColor = true;
            this.AllRot.CheckedChanged += new System.EventHandler(this.AllRot_CheckedChanged);
            // 
            // AllTrans
            // 
            this.AllTrans.AutoSize = true;
            this.AllTrans.Checked = true;
            this.AllTrans.CheckState = System.Windows.Forms.CheckState.Checked;
            this.AllTrans.Location = new System.Drawing.Point(8, 21);
            this.AllTrans.Name = "AllTrans";
            this.AllTrans.Size = new System.Drawing.Size(53, 17);
            this.AllTrans.TabIndex = 30;
            this.AllTrans.Text = "Trans";
            this.AllTrans.UseVisualStyleBackColor = true;
            this.AllTrans.CheckedChanged += new System.EventHandler(this.AllTrans_CheckedChanged);
            // 
            // ctxBox
            // 
            this.ctxBox.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Source,
            this.toolStripSeparator1,
            this.add,
            this.subtract,
            this.removeAllToolStripMenuItem,
            this.addCustomAmountToolStripMenuItem,
            this.editRawTangentToolStripMenuItem});
            this.ctxBox.Name = "ctxBox";
            this.ctxBox.Size = new System.Drawing.Size(167, 142);
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
            // editRawTangentToolStripMenuItem
            // 
            this.editRawTangentToolStripMenuItem.Name = "editRawTangentToolStripMenuItem";
            this.editRawTangentToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.editRawTangentToolStripMenuItem.Text = "Edit Raw Tangent";
            this.editRawTangentToolStripMenuItem.Click += new System.EventHandler(this.editRawTangentToolStripMenuItem_Click);
            // 
            // CHR0Editor
            // 
            this.Controls.Add(this.grpTransAll);
            this.Controls.Add(this.grpTransform);
            this.Name = "CHR0Editor";
            this.Size = new System.Drawing.Size(732, 82);
            this.grpTransform.ResumeLayout(false);
            this.grpTransform.PerformLayout();
            this.grpTransAll.ResumeLayout(false);
            this.grpTransAll.PerformLayout();
            this.ctxBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public ModelEditControl _mainWindow;

        public event EventHandler CreateUndo;

        internal NumericInputBox[] _transBoxes = new NumericInputBox[9];
        private AnimationFrame _tempFrame = AnimationFrame.Neutral;

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MDL0BoneNode TargetBone { get { return _mainWindow._targetBone; } set { _mainWindow.TargetBone = value; } }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MDL0MaterialRefNode TargetTexRef { get { return _mainWindow._targetTexRef; } set { _mainWindow.TargetTexRef = value; } }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int CurrentFrame
        {
            get { return _mainWindow.CurrentFrame; }
            set { _mainWindow.CurrentFrame = value; }
        }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MDL0Node TargetModel
        {
            get { return _mainWindow.TargetModel; }
            set { _mainWindow.TargetModel = value; }
        }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public CHR0Node SelectedAnimation
        {
            get { return _mainWindow._chr0; }
            set { _mainWindow._chr0 = value; }
        }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool EnableTransformEdit
        {
            get { return _mainWindow._enableTransform; }
            set { grpTransform.Enabled = grpTransAll.Enabled = (_mainWindow.EnableTransformEdit = value) && (TargetBone != null); }
        }

        public CHR0Editor()
        {
            InitializeComponent(); 
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
        public void UpdatePropDisplay()
        {
            grpTransAll.Enabled = EnableTransformEdit && (SelectedAnimation != null);
            btnInsert.Enabled = btnDelete.Enabled = btnClear.Enabled = CurrentFrame != 0;
            grpTransform.Enabled = EnableTransformEdit && (TargetBone != null);
            
            for (int i = 0; i < 9; i++)
                ResetBox(i);
        }
        public unsafe void ResetBox(int index)
        {
            NumericInputBox box = _transBoxes[index];
            MDL0BoneNode bone = TargetBone;
            CHR0EntryNode entry;
            if (TargetBone != null)
            if ((SelectedAnimation != null) && (CurrentFrame > 0) && ((entry = SelectedAnimation.FindChild(bone.Name, false) as CHR0EntryNode) != null))
            {
                KeyframeEntry e = entry.Keyframes.GetKeyframe((KeyFrameMode)index + 0x10, CurrentFrame - 1);
                if (e == null)
                {
                    box.Value = entry.Keyframes[KeyFrameMode.ScaleX + index, CurrentFrame - 1];
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
        public unsafe void Undo(SaveState2 save)
        {
            if (numTransX.Value != save.frameState._translate._x)
            {
                numTransX.Value = save.frameState._translate._x;
                BoxChanged(numTransX, null);
            }
            if (numTransY.Value != save.frameState._translate._y)
            {
                numTransY.Value = save.frameState._translate._y;
                BoxChanged(numTransY, null);
            }
            if (numTransZ.Value != save.frameState._translate._z)
            {
                numTransZ.Value = save.frameState._translate._z;
                BoxChanged(numTransZ, null);
            }

            if (numRotX.Value != save.frameState._rotate._x)
            {
                numRotX.Value = save.frameState._rotate._x;
                BoxChanged(numRotX, null);
            }
            if (numRotY.Value != save.frameState._rotate._y)
            {
                numRotY.Value = save.frameState._rotate._y;
                BoxChanged(numRotY, null);
            }
            if (numRotZ.Value != save.frameState._rotate._z)
            {
                numRotZ.Value = save.frameState._rotate._z;
                BoxChanged(numRotZ, null);
            }

            if (numRotX.Value != save.frameState._rotate._x)
            {
                numRotX.Value = save.frameState._rotate._x;
                BoxChanged(numRotX, null);
            }
            if (numRotY.Value != save.frameState._rotate._y)
            {
                numRotY.Value = save.frameState._rotate._y;
                BoxChanged(numRotY, null);
            }
            if (numRotZ.Value != save.frameState._rotate._z)
            {
                numRotZ.Value = save.frameState._rotate._z;
                BoxChanged(numRotZ, null);
            }
        }
        internal unsafe void BoxChangedCreateUndo(object sender, EventArgs e)
        {
            _mainWindow.CreateUndo(this, null);

            //Only update for input boxes: Methods affecting multiple values call BoxChanged on their own.
            if (sender.GetType() == typeof(NumericInputBox))
                BoxChanged(sender, null);
        }
        internal unsafe void BoxChanged(object sender, EventArgs e)
        {
            if (TargetBone == null)
                return;

            NumericInputBox box = sender as NumericInputBox;
            int index = (int)box.Tag;

            MDL0BoneNode bone = TargetBone;

            if ((SelectedAnimation != null) && (CurrentFrame > 0))
            {
                //Find bone anim and change transform
                CHR0EntryNode entry = SelectedAnimation.FindChild(bone.Name, false) as CHR0EntryNode;

                if (entry == null) //Create new bone animation
                {
                    if (!float.IsNaN(box.Value))
                    {
                        entry = SelectedAnimation.CreateEntry();
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
                        entry.SetKeyframe(KeyFrameMode.ScaleX + index, CurrentFrame - 1, box.Value);
                    }
                }
                else //Set to existing CHR0 entry 
                    if (float.IsNaN(box.Value))
                        entry.RemoveKeyframe(KeyFrameMode.ScaleX + index, CurrentFrame - 1);
                    else
                        entry.SetKeyframe(KeyFrameMode.ScaleX + index, CurrentFrame - 1, box.Value);
            }
            else
            {
                //Change base transform
                FrameState state = bone._bindState;
                float* p = (float*)&state;
                p[index] = float.IsNaN(box.Value) ? (index > 2 ? 0.0f : 1.0f) : box.Value;
                state.CalcTransforms();
                bone._bindState = state;
                //bone.RecalcBindState();
                bone.SignalPropertyChange();
            }

            TargetModel.ApplyCHR(SelectedAnimation, CurrentFrame);

            ResetBox(index);

            _mainWindow.modelPanel1.Invalidate();
        }

        //public unsafe void ApplySave(SaveState save)
        //{
        //    _transformObject = save.bone;
        //    if (save.animation != null)
        //    {
        //        CHR0EntryNode entry = null;
        //        if (save.bone != null)
        //            entry = save.animation.FindChild(save.bone.Name, false) as CHR0EntryNode;
        //        _selectedAnim = save.animation;
        //        if (save.undo) //Do the opposite of what the booleans say.
        //        {
        //            //Console.WriteLine("Undo");
        //            if (save.newEntry)
        //                save.animation.RemoveChild(entry);

        //            if (save.keyframeRemoved && save.boxIndex != -1)
        //                if (save.primarySave)
        //                    entry.SetKeyframe(KeyFrameMode.ScaleX + save.boxIndex, save.frameIndex - 1, save.oldBoxValues[save.boxIndex]);
        //                else
        //                    entry.SetKeyframe(KeyFrameMode.ScaleX + save.boxIndex, save.frameIndex - 1, save.newBoxValues[save.boxIndex]);

        //            if (save.keyframeSet && save.boxIndex != -1)
        //            {
        //                entry.RemoveKeyframe(KeyFrameMode.ScaleX + save.boxIndex, save.frameIndex - 1);
        //                entry.SetKeyframe(KeyFrameMode.ScaleX + save.boxIndex, save.frameIndex - 1, save.oldBoxValues[save.boxIndex]);
        //            }

        //            if (save.animPorted && save.oldAnimation != null)
        //                _targetModel.ApplyCHR(_selectedAnim = save.oldAnimation, _animFrame = save.frameIndex);
        //        }
        //        //Follow what the booleans say, the opposite of undo. 
        //        //This is because undo will already have been called.
        //        if (save.redo)
        //        {
        //            //Console.WriteLine("Redo");
        //            if (save.newEntry)
        //            {
        //                entry = save.animation.CreateEntry();
        //                entry.Name = save.bone.Name;

        //                //Set initial values (so they aren't null)
        //                FrameState state = save.oldFrameState; //Get the bone's bindstate
        //                float* p = (float*)&state;
        //                for (int i = 0; i < 3; i++) //Get the scale
        //                    if (p[i] != 1.0f)
        //                        entry.SetKeyframe(KeyFrameMode.ScaleX + i, 0, p[i]);
        //                for (int i = 3; i < 9; i++) //Get rotation and translation respectively
        //                    if (p[i] != 0.0f)
        //                        entry.SetKeyframe(KeyFrameMode.ScaleX + i, 0, p[i]);
        //                //Finally, replace the changed value
        //                entry.SetKeyframe(KeyFrameMode.ScaleX + save.boxIndex, save.frameIndex - 1, save.newBoxValues[save.boxIndex]);
        //            }

        //            if (save.keyframeRemoved)
        //                entry.RemoveKeyframe(KeyFrameMode.ScaleX + save.boxIndex, save.frameIndex - 1);

        //            if (save.keyframeSet)
        //                entry.SetKeyframe(KeyFrameMode.ScaleX + save.boxIndex, save.frameIndex - 1, save.newBoxValues[save.boxIndex]);
        //        }

        //        if (save.animation != null && !save.animPorted)
        //            _targetModel.ApplyCHR(_selectedAnim = save.animation, _animFrame = save.frameIndex);

        //        if (SelectedAnimationChanged != null)
        //            SelectedAnimationChanged(this, null);

        //        if (save.boxIndex != -1)
        //            ResetBox(save.boxIndex);
        //    }
        //    else
        //    {
        //        if (save.undo)
        //            save.bone._bindState = save.oldFrameState;
        //        if (save.redo)
        //            save.bone._bindState = save.newFrameState;
        //        //save.bone.RecalcBindState();
        //    }
        //}
        //public bool _rotating = false;
        //public bool check = false, removed = false;

        private static Dictionary<string, AnimationFrame> _copyAllState = new Dictionary<string, AnimationFrame>();

        private void btnCopyAll_Click(object sender, EventArgs e)
        {
            _copyAllState.Clear();

            if (CurrentFrame == 0)
                foreach (MDL0BoneNode bone in TargetModel._linker.BoneCache)
                {
                    AnimationFrame frame = (AnimationFrame)bone._bindState;
                    if (!AllTrans.Checked)
                        frame.Translation = new Vector3();
                    if (!AllRot.Checked)
                        frame.Rotation = new Vector3();
                    if (!AllScale.Checked)
                        frame.Scale = new Vector3(1);
                    _copyAllState[bone._name] = frame;
                }
            else
                foreach (CHR0EntryNode entry in SelectedAnimation.Children)
                {
                    AnimationFrame frame = entry.GetAnimFrame(CurrentFrame - 1);
                    if (!AllTrans.Checked)
                        frame.Translation = new Vector3();
                    if (!AllRot.Checked)
                        frame.Rotation = new Vector3();
                    if (!AllScale.Checked)
                        frame.Scale = new Vector3(1);
                    _copyAllState[entry._name] = frame;
                }
        }

        private void btnPasteAll_Click(object sender, EventArgs e)
        {
            if (_copyAllState.Count == 0)
                return;

            if (CurrentFrame == 0)
            {
                foreach (MDL0BoneNode bone in TargetModel._linker.BoneCache)
                    if (_copyAllState.ContainsKey(bone._name))
                    {
                        if (AllTrans.Checked)
                            bone._bindState._translate = _copyAllState[bone._name].Translation;
                        if (AllRot.Checked)
                            bone._bindState._rotate = _copyAllState[bone._name].Rotation;
                        if (AllScale.Checked)
                            bone._bindState._scale = _copyAllState[bone._name].Scale;
                        //bone.RecalcBindState();
                        bone.SignalPropertyChange();
                    }
            }
            else
            {
                foreach (string name in _copyAllState.Keys)
                {
                    CHR0EntryNode entry = null;
                    if ((entry = SelectedAnimation.FindChild(name, false) as CHR0EntryNode) == null)
                    {
                        entry = new CHR0EntryNode() { Name = name };
                        entry._numFrames = SelectedAnimation.FrameCount;
                        SelectedAnimation.AddChild(entry);
                    }

                    if (AllTrans.Checked)
                        entry.SetKeyframeOnlyTrans(CurrentFrame - 1, _copyAllState[entry._name]);
                    if (AllRot.Checked)
                        entry.SetKeyframeOnlyRot(CurrentFrame - 1, _copyAllState[entry._name]);
                    if (AllScale.Checked)
                        entry.SetKeyframeOnlyScale(CurrentFrame - 1, _copyAllState[entry._name]);
                }
            }
            _mainWindow.UpdateModel();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            if (CurrentFrame == 0)
                return;

            foreach (CHR0EntryNode entry in SelectedAnimation.Children)
            {
                if (AllTrans.Checked)
                    entry.RemoveKeyframeOnlyTrans(CurrentFrame - 1);
                if (AllRot.Checked)
                    entry.RemoveKeyframeOnlyRot(CurrentFrame - 1);
                if (AllScale.Checked)
                    entry.RemoveKeyframeOnlyScale(CurrentFrame - 1);
            }

            _mainWindow.UpdateModel();
        }

        private void btnClean_Click(object sender, EventArgs e)
        {
            ResourceNode group = TargetModel._boneGroup;
            if (group == null)
                return;

            List<CHR0EntryNode> badNodes = new List<CHR0EntryNode>();
            foreach (CHR0EntryNode entry in SelectedAnimation.Children)
            {
                if (group.FindChild(entry._name, true) == null)
                    badNodes.Add(entry);
                //else
                //    entry.Keyframes.Clean();
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
            if (SelectedAnimation == null || lblRotX.Enabled == false || lblRotY.Enabled == false || lblRotZ.Enabled == false)
                e.Cancel = true;
        }
        public int type = 0;
        private void lblScaleX_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == Forms.MouseButtons.Right)
            {
                type = 0x10;
                if (lblScaleX.Enabled == true)
                {
                    if (_transBoxes[0].BackColor == Color.Yellow)
                        editRawTangentToolStripMenuItem.Visible = true;
                    else
                        editRawTangentToolStripMenuItem.Visible = false;
                    lblScaleX.ContextMenuStrip = ctxBox;
                    Source.Text = lblScaleX.Text;
                }
                else
                    lblScaleX.ContextMenuStrip = null;
            }
        }

        private void lblScaleY_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == Forms.MouseButtons.Right)
            {
                type = 0x11;
                if (lblScaleY.Enabled == true)
                {
                    if (_transBoxes[1].BackColor == Color.Yellow)
                        editRawTangentToolStripMenuItem.Visible = true;
                    else
                        editRawTangentToolStripMenuItem.Visible = false;
                    lblScaleY.ContextMenuStrip = ctxBox;
                    Source.Text = lblScaleY.Text;
                }
                else
                    lblScaleY.ContextMenuStrip = null;
            }
        }

        private void lblScaleZ_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == Forms.MouseButtons.Right)
            {
                type = 0x12;
                if (lblScaleZ.Enabled == true)
                {
                    if (_transBoxes[2].BackColor == Color.Yellow)
                        editRawTangentToolStripMenuItem.Visible = true;
                    else
                        editRawTangentToolStripMenuItem.Visible = false;
                    lblScaleZ.ContextMenuStrip = ctxBox;
                    Source.Text = lblScaleZ.Text;
                }
                else
                    lblScaleZ.ContextMenuStrip = null;
            }
        }

        private void lblRotX_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == Forms.MouseButtons.Right)
            {
                type = 0x13;
                if (lblRotX.Enabled == true)
                {
                    if (_transBoxes[3].BackColor == Color.Yellow)
                        editRawTangentToolStripMenuItem.Visible = true;
                    else
                        editRawTangentToolStripMenuItem.Visible = false;
                    lblRotX.ContextMenuStrip = ctxBox;
                    Source.Text = lblRotX.Text;
                }
                else
                    lblRotX.ContextMenuStrip = null;
            }
        }

        private void lblRotY_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == Forms.MouseButtons.Right)
            {
                type = 0x14;
                if (lblRotY.Enabled == true)
                {
                    if (_transBoxes[4].BackColor == Color.Yellow)
                        editRawTangentToolStripMenuItem.Visible = true;
                    else
                        editRawTangentToolStripMenuItem.Visible = false;
                    lblRotY.ContextMenuStrip = ctxBox;
                    Source.Text = lblRotY.Text;
                }
                else
                    lblRotY.ContextMenuStrip = null;
            }
        }

        private void lblRotZ_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == Forms.MouseButtons.Right)
            {
                type = 0x15;
                if (lblRotZ.Enabled == true)
                {
                    if (_transBoxes[5].BackColor == Color.Yellow)
                        editRawTangentToolStripMenuItem.Visible = true;
                    else
                        editRawTangentToolStripMenuItem.Visible = false;
                    lblRotZ.ContextMenuStrip = ctxBox;
                    Source.Text = lblRotZ.Text;
                }
                else
                    lblRotZ.ContextMenuStrip = null;
            }
        }

        private void lblTransX_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == Forms.MouseButtons.Right)
            {
                type = 0x16;
                if (lblTransX.Enabled == true)
                {
                    if (_transBoxes[6].BackColor == Color.Yellow)
                        editRawTangentToolStripMenuItem.Visible = true;
                    else
                        editRawTangentToolStripMenuItem.Visible = false;
                    lblTransX.ContextMenuStrip = ctxBox;
                    Source.Text = lblTransX.Text;
                }
                else
                    lblTransX.ContextMenuStrip = null;
            }
        }

        private void lblTransY_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == Forms.MouseButtons.Right)
            {
                type = 0x17;
                if (lblTransY.Enabled == true)
                {
                    if (_transBoxes[7].BackColor == Color.Yellow)
                        editRawTangentToolStripMenuItem.Visible = true;
                    else
                        editRawTangentToolStripMenuItem.Visible = false;
                    lblTransY.ContextMenuStrip = ctxBox;
                    Source.Text = lblTransY.Text;
                }
                else
                    lblTransY.ContextMenuStrip = null;
            }
        }

        private void lblTransZ_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == Forms.MouseButtons.Right)
            {
                type = 0x18;
                if (lblTransZ.Enabled == true)
                {
                    if (_transBoxes[8].BackColor == Color.Yellow)
                        editRawTangentToolStripMenuItem.Visible = true;
                    else
                        editRawTangentToolStripMenuItem.Visible = false;
                    lblTransZ.ContextMenuStrip = ctxBox;
                    Source.Text = lblTransZ.Text;
                }
                else
                    lblTransZ.ContextMenuStrip = null;
            }
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            if (SelectedAnimation == null || type == 0)
                return;

            KeyframeEntry kfe;
            CHR0EntryNode _target = SelectedAnimation.FindChild(TargetBone.Name, false) as CHR0EntryNode;
            if (_target != null)
            {
                for (int x = 0; x < _target.FrameCount; x++) //Loop thru each frame
                    if ((kfe = _target.GetKeyframe((KeyFrameMode)type, x)) != null) //Check for a keyframe
                    { kfe._value += 180; }
                ResetBox(type - 0x10);
                _mainWindow.UpdateModel();
            }
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            if (SelectedAnimation == null || type == 0)
                return;

            KeyframeEntry kfe;
            CHR0EntryNode _target = SelectedAnimation.FindChild(TargetBone.Name, false) as CHR0EntryNode;
            if (_target != null)
            {
                for (int x = 0; x < _target.FrameCount; x++) //Loop thru each frame
                    if ((kfe = _target.GetKeyframe((KeyFrameMode)type, x)) != null) //Check for a keyframe
                    { kfe._value += 90; }
                ResetBox(type - 0x10);
                _mainWindow.UpdateModel();
            }
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            if (SelectedAnimation == null || type == 0)
                return;

            KeyframeEntry kfe;
            CHR0EntryNode _target = SelectedAnimation.FindChild(TargetBone.Name, false) as CHR0EntryNode;
            if (_target != null)
            {
                for (int x = 0; x < _target.FrameCount; x++) //Loop thru each frame
                    if ((kfe = _target.GetKeyframe((KeyFrameMode)type, x)) != null) //Check for a keyframe
                    { kfe._value -= 90; }
                ResetBox(type - 0x10);
                _mainWindow.UpdateModel();
            }
        }

        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            if (SelectedAnimation == null || type == 0)
                return;

            KeyframeEntry kfe;
            CHR0EntryNode _target = SelectedAnimation.FindChild(TargetBone.Name, false) as CHR0EntryNode;
            if (_target != null)
            {
                for (int x = 0; x < _target.FrameCount; x++) //Loop thru each frame
                    if ((kfe = _target.GetKeyframe((KeyFrameMode)type, x)) != null) //Check for a keyframe
                    { kfe._value += 45; }
                ResetBox(type - 0x10);
                _mainWindow.UpdateModel();
            }
        }

        private void toolStripMenuItem8_Click(object sender, EventArgs e)
        {
            if (SelectedAnimation == null || type == 0)
                return;

            KeyframeEntry kfe;
            CHR0EntryNode _target = SelectedAnimation.FindChild(TargetBone.Name, false) as CHR0EntryNode;
            if (_target != null)
            {
                for (int x = 0; x < _target.FrameCount; x++) //Loop thru each frame
                    if ((kfe = _target.GetKeyframe((KeyFrameMode)type, x)) != null) //Check for a keyframe
                    { kfe._value -= 45; }
                ResetBox(type - 0x10);
                _mainWindow.UpdateModel();
            }
        }

        private void removeAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SelectedAnimation == null || type == 0)
                return;

            CHR0EntryNode _target = SelectedAnimation.FindChild(TargetBone.Name, false) as CHR0EntryNode;
            if (_target != null)
            {
                _target.Keyframes._keyRoots[type & 0xF] = new KeyframeEntry(-1, type >= 0x10 && type <= 0x12 ? 1 : 0);
                _target.Keyframes._keyCounts[type & 0xF] = 0;
                _target.SignalPropertyChange();
                ResetBox(type - 0x10);
                _mainWindow.UpdateModel();
            }
        }

        private void addCustomAmountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SelectedAnimation == null || type == 0)
                return;

            EditDialog ed = new EditDialog();
            ed.ShowDialog(null, (KeyFrameMode)type, SelectedAnimation.FindChild(TargetBone.Name, false) as CHR0EntryNode);
            ResetBox(type - 0x10);
            _mainWindow.UpdateModel();
        }

        private unsafe void btnCut_Click(object sender, EventArgs e)
        {
            AnimationFrame frame = new AnimationFrame();
            float* p = (float*)&frame;

            if (FrameScale.Checked) frame.hasSx = frame.hasSy = frame.hasSz = true;
            if (FrameRot.Checked) frame.hasRx = frame.hasRy = frame.hasRz = true;
            if (FrameTrans.Checked) frame.hasTx = frame.hasTy = frame.hasTz = true;

            BoxChangedCreateUndo(this, null);

            for (int i = 0; i < 9; i++)
            {
                if ((!FrameScale.Checked && i < 3))
                    p[i] = 1;
                else if (
                    (FrameScale.Checked && i < 3) || 
                    (FrameRot.Checked && i >= 3 && i < 6) || 
                    (FrameTrans.Checked && i >= 6))
                        p[i] = _transBoxes[i].Value;
                _transBoxes[i].Value = float.NaN;
                BoxChanged(_transBoxes[i], null);
            }

            _tempFrame = frame;
        }

        private unsafe void btnCopy_Click(object sender, EventArgs e)
        {
            AnimationFrame frame = new AnimationFrame();
            float* p = (float*)&frame;

            if (FrameScale.Checked) frame.hasSx = frame.hasSy = frame.hasSz = true;
            if (FrameRot.Checked) frame.hasRx = frame.hasRy = frame.hasRz = true;
            if (FrameTrans.Checked) frame.hasTx = frame.hasTy = frame.hasTz = true;

            for (int i = 0; i < 9; i++)
                if ((!FrameScale.Checked && i < 3))
                    p[i] = 1;
                else if (
                    (FrameScale.Checked && i < 3) ||
                    (FrameRot.Checked && i >= 3 && i < 6) ||
                    (FrameTrans.Checked && i >= 6))
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
                if ((FrameScale.Checked && i < 3) ||
                    (FrameRot.Checked && i >= 3 && i < 6) ||
                    (FrameTrans.Checked && i >= 6))
                    if (_transBoxes[i].Value != p[i])
                        _transBoxes[i].Value = p[i];
                BoxChanged(_transBoxes[i], null);
            }
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            if ((SelectedAnimation == null) || (CurrentFrame == 0))
                return;

            SelectedAnimation.InsertKeyframe(CurrentFrame - 1);
            _mainWindow.CHR0StateChanged(this, null);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if ((SelectedAnimation == null) || (CurrentFrame == 0))
                return;

            SelectedAnimation.DeleteKeyframe(CurrentFrame - 1);
            _mainWindow.CHR0StateChanged(this, null);
        }

        //private void chkLinear_CheckedChanged(object sender, EventArgs e)
        //{
        //    DialogResult r;
        //    if (SelectedAnimation != null)
        //        if (TargetBone != null)
        //        {
        //            if ((r = MessageBox.Show("Do you want to apply linear interpolation to only this bone?\nOtherwise, all bones in the animation will be set to linear.", "", MessageBoxButtons.YesNoCancel)) == DialogResult.Yes)
        //                (SelectedAnimation.FindChild(TargetBone.Name, true) as CHR0EntryNode).Keyframes.LinearRotation = chkLinear.Checked;
        //            else if (r == DialogResult.No)
        //                foreach (CHR0EntryNode n in SelectedAnimation.Children)
        //                    n.Keyframes.LinearRotation = chkLinear.Checked;
        //            else return;
        //        }
        //        else
        //            foreach (CHR0EntryNode n in SelectedAnimation.Children)
        //                n.Keyframes.LinearRotation = chkLinear.Checked;
        //}

        //private void chkLoop_CheckedChanged(object sender, EventArgs e)
        //{
        //    SelectedAnimation.Loop = chkLoop.Checked ? true : false;
        //}

        private void editRawTangentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TangentEditor t = new TangentEditor();
            CHR0EntryNode entry = SelectedAnimation.FindChild(TargetBone.Name, false) as CHR0EntryNode;
            KeyframeEntry kfe = entry.GetKeyframe((KeyFrameMode)type, CurrentFrame - 1);
            if (kfe != null)
            if (t.ShowDialog(this, kfe._tangent) == DialogResult.OK)
                kfe._tangent = t.tan;
        }

        private void AllScale_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void AllRot_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void AllTrans_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
