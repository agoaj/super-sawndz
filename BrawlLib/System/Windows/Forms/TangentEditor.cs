using System;
using System.Windows.Forms;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.Wii.Animations;

namespace System.Windows.Forms
{
    public class TangentEditor : Form
    {
        public float tan;

        public TangentEditor() { InitializeComponent(); }

        public DialogResult ShowDialog(IWin32Window owner, float tan)
        {
            this.tan = tan;
            value.Text = tan.ToString();
            return base.ShowDialog(owner);
        }

        private unsafe void btnOkay_Click(object sender, EventArgs e)
        {
            float.TryParse(value.Text, out tan);
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e) { DialogResult = DialogResult.Cancel; Close(); }

        #region Designer

        private TextBox value;
        private Button btnCancel;
        private Label label1;
        private Button btnOkay;

        private void InitializeComponent()
        {
            this.value = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOkay = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // value
            // 
            this.value.HideSelection = false;
            this.value.Location = new System.Drawing.Point(72, 9);
            this.value.Name = "value";
            this.value.Size = new System.Drawing.Size(107, 20);
            this.value.TabIndex = 0;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(98, 38);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOkay
            // 
            this.btnOkay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOkay.Location = new System.Drawing.Point(17, 38);
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
            this.label1.Location = new System.Drawing.Point(13, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Tangent: ";
            // 
            // TangentEditor
            // 
            this.AcceptButton = this.btnOkay;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(185, 69);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnOkay);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.value);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "TangentEditor";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Edit All Keyframes";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

    }
}
