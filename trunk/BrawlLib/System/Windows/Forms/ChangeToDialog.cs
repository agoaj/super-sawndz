using System;
using System.Windows.Forms;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.Wii.Animations;

namespace System.Windows.Forms
{
    public class ChangeToDialog : Form
    {
        private ResourceNode _node;
        private TextBox ScaleX;
        private TextBox ScaleY;
        private TextBox ScaleZ;
        private Label label3;
        private Label label4;
        private Label label5;
        private bool _rename;

        public ChangeToDialog() { InitializeComponent(); }

        public DialogResult ShowDialog(IWin32Window owner, ResourceNode node, bool rename)
        {
            _rename = rename;
            if (_rename)
            {
                Text = "Rename Nodes";
                newName.MaxLength = 255;
                Height = 160;
            }
            else
            {
                Text = "Scale Animations";
                label2.Visible = false;
                newName.Visible = false;
                label1.Text = "Scale all bones with the name:";
                label3.Visible = label4.Visible = label5.Visible = true;
                ScaleX.Visible = ScaleY.Visible = ScaleZ.Visible = true;
            }
            oldName.MaxLength = 255;
            _node = node;
            try { return base.ShowDialog(owner); }
            finally { _node = null; }
        }

        private unsafe void btnOkay_Click(object sender, EventArgs e)
        {
            if (_rename)
            {
                string oldname = oldName.Text;
                string newname = newName.Text;

                if (oldname == null)
                    return;
                if (newname != null)
                    ChangeNames(oldname, newname, _node);
                else
                {
                    MessageBox.Show(this, "Name cannot be null!", "What the...");
                    return;
                }
            }
            else
            {
                string name = oldName.Text;
                float x = Convert.ToSingle(ScaleX.Text);
                float y = Convert.ToSingle(ScaleY.Text);
                float z = Convert.ToSingle(ScaleZ.Text);
                if (x == float.NaN)
                    x = 0;
                if (y == float.NaN)
                    y = 0;
                if (z == float.NaN)
                    z = 0;
                Vector3 scale = new Vector3(x, y, z);
                if (name != null)
                    ChangeScale(name, _node, scale);
            }
            DialogResult = DialogResult.OK;
            Close();
        }

        public void ChangeNames(string oldname, string newname, ResourceNode parent)
        {
            //if (parent != null)
            //{
            //    //No duplicates
            //    foreach (ResourceNode c in parent.Children)
            //    {
            //        if ((c.Name == newname) && (c.GetType() == parent.GetType()) && (c != _node))
            //        {
            //            MessageBox.Show(this, "A resource with that name already exists!", "What the...");
            //            return;
            //        }
            //    }
            //}
            foreach (ResourceNode r in parent.Children)
            {
                if (r.Name == oldname)
                    if (!(r is MDL0GroupNode))
                        r.Name = newname;
                ChangeNames(oldname, newname, r);
            }
        }

        public void ChangeScale(string name, ResourceNode parent, Vector3 scale)
        {
            int numFrames;
            CHR0EntryNode entry;
            foreach (ResourceNode r in parent.Children)
            {
                if (r is CHR0Node)
                    if (r.FindChild(name, false) == null)
                    {
                        CHR0EntryNode c = new CHR0EntryNode();
                        c._numFrames = (r as CHR0Node).FrameCount;
                        c.Name = name;
                        r.Children.Add(c);
                    }
                if (r.Name == name)
                {
                    if (r is CHR0EntryNode)
                    {
                        entry = r as CHR0EntryNode;
                        numFrames = entry.FrameCount;
                        for (int x = 0; x < numFrames; x++)
                        {
                            for (int i = 0x10; i < 0x13; i++)
                            {
                                if (entry.GetKeyframe((KeyFrameMode)i, x) != null)
                                    entry.RemoveKeyframe((KeyFrameMode)i, x);
                            }
                        }
                        entry.SetKeyframeOnlyScale(0, scale);
                    }
                }
                ChangeScale(name, r, scale);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e) { DialogResult = DialogResult.Cancel; Close(); }

        #region Designer

        private TextBox oldName;
        private Button btnCancel;
        private TextBox newName;
        private Label label1;
        private Label label2;
        private Button btnOkay;

        private void InitializeComponent()
        {
            this.oldName = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOkay = new System.Windows.Forms.Button();
            this.newName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.ScaleX = new System.Windows.Forms.TextBox();
            this.ScaleY = new System.Windows.Forms.TextBox();
            this.ScaleZ = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // oldName
            // 
            this.oldName.HideSelection = false;
            this.oldName.Location = new System.Drawing.Point(12, 28);
            this.oldName.Name = "oldName";
            this.oldName.Size = new System.Drawing.Size(210, 20);
            this.oldName.TabIndex = 0;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(147, 140);
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
            this.btnOkay.Location = new System.Drawing.Point(66, 140);
            this.btnOkay.Name = "btnOkay";
            this.btnOkay.Size = new System.Drawing.Size(75, 23);
            this.btnOkay.TabIndex = 1;
            this.btnOkay.Text = "&Okay";
            this.btnOkay.UseVisualStyleBackColor = true;
            this.btnOkay.Click += new System.EventHandler(this.btnOkay_Click);
            // 
            // newName
            // 
            this.newName.HideSelection = false;
            this.newName.Location = new System.Drawing.Point(12, 74);
            this.newName.Name = "newName";
            this.newName.Size = new System.Drawing.Size(210, 20);
            this.newName.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(161, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Change all nodes with the name:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(23, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "To:";
            // 
            // ScaleX
            // 
            this.ScaleX.Location = new System.Drawing.Point(66, 55);
            this.ScaleX.Name = "ScaleX";
            this.ScaleX.Size = new System.Drawing.Size(156, 20);
            this.ScaleX.TabIndex = 6;
            this.ScaleX.Visible = false;
            // 
            // ScaleY
            // 
            this.ScaleY.Location = new System.Drawing.Point(66, 81);
            this.ScaleY.Name = "ScaleY";
            this.ScaleY.Size = new System.Drawing.Size(156, 20);
            this.ScaleY.TabIndex = 7;
            this.ScaleY.Visible = false;
            // 
            // ScaleZ
            // 
            this.ScaleZ.Location = new System.Drawing.Point(66, 107);
            this.ScaleZ.Name = "ScaleZ";
            this.ScaleZ.Size = new System.Drawing.Size(156, 20);
            this.ScaleZ.TabIndex = 8;
            this.ScaleZ.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 58);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Scale X:";
            this.label3.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 84);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Scale Y:";
            this.label4.Visible = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 110);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Scale Z:";
            this.label5.Visible = false;
            // 
            // ChangeToDialog
            // 
            this.AcceptButton = this.btnOkay;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(234, 175);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.ScaleZ);
            this.Controls.Add(this.ScaleY);
            this.Controls.Add(this.ScaleX);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.newName);
            this.Controls.Add(this.btnOkay);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.oldName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ChangeToDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Rename Nodes";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion
    }
}
