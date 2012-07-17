using System;
using System.Windows.Forms;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.Wii.Animations;
using System.Collections.Generic;

namespace System.Windows.Forms
{
    public class ModelSwitcher : Form
    {
        private List<MDL0Node> _models;
        private CheckBox Delete;
        private CheckBox hide;
        private ModelEditControl form;

        public ModelSwitcher() { InitializeComponent(); }

        public DialogResult ShowDialog(ModelEditControl owner, List<MDL0Node> models)
        {
            _models = models;
            form = owner;
            foreach (MDL0Node m in _models)
                if (m != null) 
                    model.Items.Add(m);
            if (_models.Count != 0 && form.TargetModel != null)
                model.SelectedIndex = form._targetModels.IndexOf(form.TargetModel);
            if (form.hide)
                hide.Checked = true;
            return base.ShowDialog(owner);
        }

        private unsafe void btnOkay_Click(object sender, EventArgs e)
        {
            if (_models.Count != 0)
                if (!Delete.Checked)
                {
                    form.resetcam = false;
                    form.hide = hide.Checked;
                    form.TargetModel = (MDL0Node)model.SelectedItem;
                    for (int i = 0; i < form._targetModels.Count; i++)
                        if (form.hide && form._targetModels[i] != null)
                            form.modelPanel1.RemoveTarget(form._targetModels[i]);
                        else
                            if (form._targetModels[i] != null)
                                form.modelPanel1.AddTarget(form._targetModels[i]);
                }
                else
                {
                    form._targetModels.Remove((MDL0Node)model.SelectedItem);
                    form.modelPanel1.RemoveTarget((MDL0Node)model.SelectedItem);
                    form.modelPanel1.Invalidate();
                }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e) { DialogResult = DialogResult.Cancel; Close(); }

        #region Designer

        private Button btnCancel;
        private Label label1;
        private ComboBox model;
        private Button btnOkay;

        private void InitializeComponent()
        {
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOkay = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.model = new System.Windows.Forms.ComboBox();
            this.Delete = new System.Windows.Forms.CheckBox();
            this.hide = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(172, 85);
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
            this.btnOkay.Location = new System.Drawing.Point(91, 85);
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
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Target Model:";
            // 
            // model
            // 
            this.model.FormattingEnabled = true;
            this.model.Location = new System.Drawing.Point(91, 12);
            this.model.Name = "model";
            this.model.Size = new System.Drawing.Size(156, 21);
            this.model.TabIndex = 4;
            this.model.SelectedIndexChanged += new System.EventHandler(this.model_SelectedIndexChanged);
            // 
            // Delete
            // 
            this.Delete.AutoSize = true;
            this.Delete.Location = new System.Drawing.Point(15, 39);
            this.Delete.Name = "Delete";
            this.Delete.Size = new System.Drawing.Size(183, 17);
            this.Delete.TabIndex = 5;
            this.Delete.Text = "Delete this model from the scene.";
            this.Delete.UseVisualStyleBackColor = true;
            // 
            // hide
            // 
            this.hide.AutoSize = true;
            this.hide.Location = new System.Drawing.Point(15, 62);
            this.hide.Name = "hide";
            this.hide.Size = new System.Drawing.Size(127, 17);
            this.hide.TabIndex = 6;
            this.hide.Text = "Hide all other models.";
            this.hide.UseVisualStyleBackColor = true;
            // 
            // ModelSwitcher
            // 
            this.AcceptButton = this.btnOkay;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(259, 120);
            this.Controls.Add(this.hide);
            this.Controls.Add(this.Delete);
            this.Controls.Add(this.model);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnOkay);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ModelSwitcher";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Switch Target Model";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private void model_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
