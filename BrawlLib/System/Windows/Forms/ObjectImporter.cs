using System;
using System.Windows.Forms;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.Wii.Animations;
using System.Collections.Generic;
using BrawlLib.Modeling;
using BrawlLib.Wii.Models;

namespace System.Windows.Forms
{
    public class ObjectImporter : Form
    {
        MDL0Node _internalModel;
        private Label label1;
        private ComboBox comboBox1;
        private Label label2;
        private ComboBox comboBox2;
        MDL0Node _externalModel;
        MDL0PolygonNode node;
        IMatrixNode _baseInf;
        private ComboBox comboBox3;
        private CheckBox checkBox1;
        MDL0BoneNode _parent;
        private Label label3;
        bool _mergeModels = false;
        
        public ObjectImporter() { InitializeComponent(); }

        public DialogResult ShowDialog(MDL0Node internalModel, MDL0Node externalModel)
        {
            _internalModel = internalModel;
            _externalModel = externalModel;

            comboBox1.Items.AddRange(_externalModel.FindChild("Objects", true).Children.ToArray());
            comboBox2.Items.AddRange(_internalModel._linker.BoneCache);
            comboBox3.Items.Add("Add as child");
            //comboBox3.Items.Add("Replace");
            //comboBox3.Items.Add("Merge children");
            comboBox1.SelectedIndex = comboBox2.SelectedIndex = comboBox3.SelectedIndex = 0;

            return base.ShowDialog(null);
        }

        private void MergeChildren(MDL0BoneNode main, MDL0BoneNode ext, ResourceNode res)
        {
            bool found = false;
            if (res is MDL0PolygonNode)
            {
                MDL0PolygonNode poly = res as MDL0PolygonNode;
                foreach (Vertex3 v in poly._manager._vertices)
                    if (v._influence == ext)
                    {
                        v._influence = main;
                        main.ReferenceCount++;
                    }
            }
            else if (res is MDL0Node)
            {
                MDL0Node mdl = res as MDL0Node;
                foreach (MDL0PolygonNode poly in mdl.FindChild("Objects", true).Children)
                    foreach (Vertex3 v in poly._manager._vertices)
                        if (v._influence == ext)
                        {
                            v._influence = main;
                            main.ReferenceCount++;
                        }
            }
            foreach (MDL0BoneNode b2 in ext.Children)
            {
                foreach (MDL0BoneNode b1 in main.Children)
                {
                    if (b1.Name == b2.Name)
                    {
                        found = true;
                        MergeChildren(b1, b2, res);
                        break;
                    }
                }
                if (!found)
                {
                    if (b2.ReferenceCount > 0)
                        main.InsertChild(b2, true, b2.Index);
                }
                else
                    found = false;
            }
        }

        private void ImportObject(MDL0PolygonNode node)
        {
            MDL0PolygonNode newNode = node.Clone();
            if (node._vertexNode != null)
            {
                _internalModel.VertexGroup.AddChild(node._vertexNode);
                (newNode._vertexNode = (MDL0VertexNode)_internalModel.VertexGroup.Children[_internalModel._vertList.Count - 1])._polygons.Add(newNode);
            }
            if (node.NormalNode != null)
            {
                _internalModel.NormalGroup.AddChild(node._normalNode);
                (newNode._normalNode = (MDL0NormalNode)_internalModel.NormalGroup.Children[_internalModel._normList.Count - 1])._polygons.Add(newNode);
            }
            for (int i = 0; i < 8; i++)
                if (node.UVNodes[i] != null)
                {
                    _internalModel.UVGroup.AddChild(node.UVNodes[i]);
                    newNode._uvSet[i] = (MDL0UVNode)_internalModel.UVGroup.Children[_internalModel._uvList.Count - 1];
                    newNode._uvSet[i].Name = "#" + (_internalModel._uvList.Count - 1);
                    newNode._uvSet[i]._polygons.Add(newNode);
                }

            for (int i = 0; i < 2; i++)
                if (node._colorSet[i] != null)
                {
                    _internalModel.ColorGroup.AddChild(node._colorSet[i]);
                    //(newNode._colorSet[i] = (MDL0ColorNode)_internalModel.ColorGroup.Children[_internalModel._colorList.Count - 1])._polygons.Add(newNode);
                }

            _internalModel._matGroup.AddChild(node._material);
            newNode.MaterialNode = (MDL0MaterialNode)_internalModel.MaterialGroup.Children[_internalModel._matList.Count - 1];

            _internalModel._shadGroup.AddChild(node._material._shader);
            newNode._material.ShaderNode = (MDL0ShaderNode)_internalModel.ShaderGroup.Children[_internalModel._shadList.Count - 1];

            foreach (MDL0MaterialRefNode r in newNode._material.Children)
            {
                if (r._texture != null)
                    (r._texture = _internalModel.FindOrCreateTexture(r.TextureNode.Name))._references.Add(r);

                if (r._palette != null)
                    (r._palette = _internalModel.FindOrCreatePalette(r.PaletteNode.Name))._references.Add(r);
            }

            if (node.Weighted)
            {
                foreach (Vertex3 vert in node._manager._vertices)
                    if (vert._influence != null && vert._influence is Influence)
                        vert._influence = _internalModel._influences.AddOrCreateInf((Influence)vert._influence);
            }
            else if (node.SingleBindInf != null && node.SingleBindInf is Influence)
                node.SingleBindInf = _internalModel._influences.AddOrCreateInf((Influence)node.SingleBindInf);

            newNode.RecalcIndices();
            newNode._bone = (MDL0BoneNode)_internalModel.BoneGroup.Children[0];
            newNode._manager = node._manager;
            newNode.Name = "polygon" + (_internalModel._polyList.Count);
            newNode.SignalPropertyChange();
            _internalModel._polyGroup.AddChild(newNode);
            newNode.Rebuild(true);
        }

        private unsafe void btnOkay_Click(object sender, EventArgs e)
        {
            switch (comboBox3.SelectedIndex)
            {
                case 0:
                    _parent.AddChild((ResourceNode)_baseInf);
                    break;
                case 1:
                    _parent.Parent.AddChild((ResourceNode)_baseInf);
                    _parent.Remove();
                    break;
                case 2:
                    MergeChildren(_parent, (MDL0BoneNode)_baseInf, _mergeModels ? (ResourceNode)_externalModel : (ResourceNode)node);
                    break;
            }

            if (_mergeModels)
                foreach (MDL0PolygonNode poly in _externalModel.FindChild("Objects", true).Children)
                    ImportObject(poly);
            else ImportObject(node);

            _internalModel.SignalPropertyChange();

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e) { DialogResult = DialogResult.Cancel; Close(); }

        private void getBaseInfluence()
        {
            ResourceNode[] boneCache = _externalModel._linker.BoneCache;
            if ((node = (MDL0PolygonNode)comboBox1.SelectedItem).Weighted)
            {
                int least = int.MaxValue;
                foreach (IMatrixNode inf in node.Influences)
                    if (inf is MDL0BoneNode && ((MDL0BoneNode)inf).BoneIndex < least)
                        least = ((MDL0BoneNode)inf).BoneIndex;

                if (least != int.MaxValue)
                {
                    MDL0BoneNode temp = (MDL0BoneNode)boneCache[least];
                    _baseInf = (IMatrixNode)temp.Parent;
                }
            }
            else _baseInf = node.SingleBindInf;

            if (_baseInf is Influence)
            {
                label2.Hide();
                comboBox2.Hide();
            }
            else if (_baseInf is MDL0BoneNode)
            {
                label2.Show();
                comboBox2.Show();
            }

            switch (comboBox3.SelectedIndex)
            {
                case 0:
                    label2.Text = "Add base bone \"" + _baseInf + "\" to the children of: ";
                    break;
                case 1:
                    label2.Text = "With base bone \"" + _baseInf + "\", replace: ";
                    break;
                case 2:
                    label2.Text = "Merge base bone \"" + _baseInf + "\" with: ";
                    break;
            }

            comboBox2.Location = new Drawing.Point(label2.Size.Width + 20, 36);
            //Width = comboBox2.Location.X + 140;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == null)
                return;

            getBaseInfluence();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == null)
                return;

            _parent = (MDL0BoneNode)comboBox2.SelectedItem;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            _mergeModels = checkBox1.Checked;
            if (_mergeModels)
            {
                label1.Hide();
                comboBox1.Hide();
                _baseInf = (IMatrixNode)_externalModel._linker.BoneCache[0];
                switch (comboBox3.SelectedIndex)
                {
                    case 0:
                        label2.Text = "Add base bone \"" + _baseInf + "\" to the children of: ";
                        break;
                    case 1:
                        label2.Text = "With base bone \"" + _baseInf + "\", replace: ";
                        break;
                    case 2:
                        label2.Text = "Merge base bone \"" + _baseInf + "\" with: ";
                        break;
                }
            }
            else
            {
                label1.Show();
                comboBox1.Show();
                getBaseInfluence();
            }
        }

        #region Designer

        private Button btnCancel;
        private Button btnOkay;

        private void InitializeComponent()
        {
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOkay = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.comboBox3 = new System.Windows.Forms.ComboBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(355, 61);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOkay
            // 
            this.btnOkay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOkay.Location = new System.Drawing.Point(274, 61);
            this.btnOkay.Name = "btnOkay";
            this.btnOkay.Size = new System.Drawing.Size(75, 23);
            this.btnOkay.TabIndex = 1;
            this.btnOkay.Text = "&Okay";
            this.btnOkay.UseVisualStyleBackColor = true;
            this.btnOkay.Click += new System.EventHandler(this.btnOkay_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(140, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Import:";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(181, 10);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(127, 21);
            this.comboBox1.TabIndex = 4;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "(no option selected)";
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(118, 36);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(121, 21);
            this.comboBox2.TabIndex = 6;
            this.comboBox2.SelectedIndexChanged += new System.EventHandler(this.comboBox2_SelectedIndexChanged);
            // 
            // comboBox3
            // 
            this.comboBox3.FormattingEnabled = true;
            this.comboBox3.Location = new System.Drawing.Point(100, 63);
            this.comboBox3.Name = "comboBox3";
            this.comboBox3.Size = new System.Drawing.Size(121, 21);
            this.comboBox3.TabIndex = 7;
            this.comboBox3.SelectedIndexChanged += new System.EventHandler(this.comboBox3_SelectedIndexChanged);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(12, 12);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(116, 17);
            this.checkBox1.TabIndex = 8;
            this.checkBox1.Text = "Merge both models";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 66);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Base bone tree:";
            // 
            // ObjectImporter
            // 
            this.AcceptButton = this.btnOkay;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(442, 96);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.comboBox3);
            this.Controls.Add(this.comboBox2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnOkay);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ObjectImporter";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Import Object";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox3.SelectedIndex)
            {
                case 0:
                    label2.Text = "Add base bone \"" + _baseInf + "\" to the children of: ";
                    break;
                case 1:
                    label2.Text = "With base bone \"" + _baseInf + "\", replace: ";
                    break;
                case 2:
                    label2.Text = "Merge base bone \"" + _baseInf + "\" with: ";
                    break;
            }
        }
    }
}
