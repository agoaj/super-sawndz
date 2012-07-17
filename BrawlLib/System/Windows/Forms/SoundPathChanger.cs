using System;

namespace System.Windows.Forms
{
    class SoundPathChanger : Form
    {
        #region Designer

        private TextBox txtPath;
        private Button btnOkay;
        private Button btnCancel;
        private Label label1;
    
        private void InitializeComponent()
        {
            this.txtPath = new System.Windows.Forms.TextBox();
            this.btnOkay = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtPath
            // 
            this.txtPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPath.Location = new System.Drawing.Point(12, 34);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(367, 20);
            this.txtPath.TabIndex = 0;
            // 
            // btnOkay
            // 
            this.btnOkay.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnOkay.Location = new System.Drawing.Point(117, 65);
            this.btnOkay.Name = "btnOkay";
            this.btnOkay.Size = new System.Drawing.Size(75, 23);
            this.btnOkay.TabIndex = 1;
            this.btnOkay.Text = "Okay";
            this.btnOkay.UseVisualStyleBackColor = true;
            this.btnOkay.Click += new System.EventHandler(this.btnOkay_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnCancel.Location = new System.Drawing.Point(198, 65);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(367, 21);
            this.label1.TabIndex = 3;
            this.label1.Text = "*Changing the path on an internal file will remove it from the RSAR*";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SoundPathChanger
            // 
            this.ClientSize = new System.Drawing.Size(391, 100);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOkay);
            this.Controls.Add(this.txtPath);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "SoundPathChanger";
            this.Text = "File Path";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private string _filePath = "";
        public string FilePath { get { return _filePath; } set { _filePath = value; } }

        public SoundPathChanger() { InitializeComponent(); }

        protected override void OnShown(EventArgs e)
        {
            txtPath.Text = _filePath;
            base.OnShown(e);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnOkay_Click(object sender, EventArgs e)
        {
            _filePath = txtPath.Text;
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
