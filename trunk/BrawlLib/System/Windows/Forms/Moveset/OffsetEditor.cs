using System;
using BrawlLib.SSBB.ResourceNodes;
using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms
{
    public class OffsetEditor : UserControl
    {
        #region Designer

        private void InitializeComponent()
        {
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBox3 = new System.Windows.Forms.ComboBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Actions",
            "Animations",
            "SubRoutines",
            "External",
            "Null"});
            this.comboBox1.Location = new System.Drawing.Point(49, 3);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 0;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "List:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Action:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // comboBox2
            // 
            this.comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(49, 27);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(121, 21);
            this.comboBox2.TabIndex = 2;
            this.comboBox2.SelectedIndexChanged += new System.EventHandler(this.comboBox2_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(176, 6);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Type:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // comboBox3
            // 
            this.comboBox3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox3.FormattingEnabled = true;
            this.comboBox3.Location = new System.Drawing.Point(216, 3);
            this.comboBox3.Name = "comboBox3";
            this.comboBox3.Size = new System.Drawing.Size(74, 21);
            this.comboBox3.TabIndex = 4;
            this.comboBox3.SelectedIndexChanged += new System.EventHandler(this.comboBox3_SelectedIndexChanged);
            // 
            // richTextBox1
            // 
            this.richTextBox1.BackColor = System.Drawing.SystemColors.Control;
            this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox1.Cursor = System.Windows.Forms.Cursors.Default;
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBox1.ForeColor = System.Drawing.Color.Black;
            this.richTextBox1.Location = new System.Drawing.Point(0, 52);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(296, 53);
            this.richTextBox1.TabIndex = 6;
            this.richTextBox1.Text = "";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.comboBox1);
            this.panel1.Controls.Add(this.comboBox2);
            this.panel1.Controls.Add(this.comboBox3);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(296, 52);
            this.panel1.TabIndex = 7;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(215, 26);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(76, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "Okay";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // OffsetEditor
            // 
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.panel1);
            this.Name = "OffsetEditor";
            this.Size = new System.Drawing.Size(296, 105);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        int index = -2;
        private ComboBox comboBox1;
        private Label label1;
        private Label label2;
        private ComboBox comboBox2;
        private Label label3;
        private ComboBox comboBox3;
        public RichTextBox richTextBox1;
        private Panel panel1;
        private Button button1;

        private MoveDefEventOffsetNode _targetNode;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MoveDefEventOffsetNode TargetNode
        {
            get { return _targetNode; }
            set { _targetNode = value; TargetChanged(); }
        }

        private void TargetChanged()
        {
            if (_targetNode == null)
                return;

            int list, type, index;
            _targetNode.Root.GetLocation(_targetNode.RawOffset == -1 ? _targetNode._offset + 4 : _targetNode.RawOffset, out list, out type, out index);

            _updating = true;
            comboBox1.SelectedIndex = _targetNode.list = list;
            if (_targetNode.type != -1)
                comboBox3.SelectedIndex = _targetNode.type = type;
            if (_targetNode.index != -1 && comboBox2.Items.Count > index)
                comboBox2.SelectedIndex = _targetNode.index = index;

            if (list < 3)
            {
                _targetNode.action = _targetNode.Root.GetAction(list, type, index);
                if (_targetNode.action == null)
                    _targetNode.action = _targetNode.GetAction();
            }
            else
                _targetNode.action = null;

            _updating = false;
            UpdateText();
        }

        private bool _updating = false;

        public OffsetEditor() { InitializeComponent(); }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                comboBox3.Items.Clear();
                comboBox3.Items.Add("Entry");
                comboBox3.Items.Add("Exit");

                comboBox2.Items.Clear();
                comboBox2.Items.AddRange(_targetNode.Root._actions.Children.ToArray());
            }
            if (comboBox1.SelectedIndex == 1)
            {
                comboBox3.Items.Clear();
                comboBox3.Items.Add("Main");
                comboBox3.Items.Add("GFX");
                comboBox3.Items.Add("SFX");
                comboBox3.Items.Add("Other");

                comboBox2.Items.Clear();
                if (TargetNode.Root._subActions != null)
                comboBox2.Items.AddRange(_targetNode.Root._subActions.Children.ToArray());
            }
            if (comboBox1.SelectedIndex >= 2)
                comboBox3.Visible = label3.Visible = false;
            else
                comboBox3.Visible = label3.Visible = true;
            if (comboBox1.SelectedIndex == 4)
                comboBox2.Visible = label2.Visible = false;
            else
                comboBox2.Visible = label2.Visible = true;
            if (comboBox1.SelectedIndex == 2)
            {
                comboBox2.Items.Clear();
                comboBox2.Items.AddRange(_targetNode.Root._subRoutineList.ToArray());
            }
            if (comboBox1.SelectedIndex == 3)
            {
                comboBox2.Items.Clear();
                comboBox2.Items.AddRange(_targetNode.Root._external.ToArray());
            }
            if (!_updating)
                UpdateText();
        }

        private void UpdateText()
        {
            if (comboBox1.SelectedIndex == 4)
                richTextBox1.Text = "Go nowhere.";
            else
                richTextBox1.Text = "Go to " + comboBox2.Text + (comboBox1.SelectedIndex >= 2 ? "" : " - " + comboBox3.Text) + " in the " + comboBox1.Text + " list.";
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_updating)
                UpdateText();
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_updating)
                UpdateText();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int offset = -1;
            if (comboBox1.SelectedIndex >= 3)
                offset = -1;
            else
                offset = _targetNode.Root.GetOffset(comboBox1.SelectedIndex, (comboBox1.SelectedIndex >= 2 ? -1 : comboBox3.SelectedIndex), comboBox2.SelectedIndex);
            _targetNode._value = offset;
            _targetNode.action = _targetNode.Root.GetAction(offset);
            _targetNode.list = comboBox1.SelectedIndex;
            _targetNode.type = (comboBox1.SelectedIndex >= 2 ? -1 : comboBox3.SelectedIndex);
            _targetNode.index = (comboBox1.SelectedIndex == 4 ? -1 : comboBox2.SelectedIndex);
            _targetNode.SignalPropertyChange();
        }
    }
}
