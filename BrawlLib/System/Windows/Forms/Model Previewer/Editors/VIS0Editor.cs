using System;
using System.ComponentModel;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.SSBBTypes;

namespace System.Windows.Forms
{
    public class VIS0Editor : UserControl
    {
        #region Designer
        private void InitializeComponent()
        {
            this.visEditor1 = new System.Windows.Forms.VisEditor();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.Constant = new System.Windows.Forms.CheckBox();
            this.eEnabled = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // visEditor1
            // 
            this.visEditor1.Dock = System.Windows.Forms.DockStyle.Left;
            this.visEditor1.Location = new System.Drawing.Point(207, 4);
            this.visEditor1.Name = "visEditor1";
            this.visEditor1.Size = new System.Drawing.Size(296, 104);
            this.visEditor1.TabIndex = 0;
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(204, 4);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 104);
            this.splitter1.TabIndex = 2;
            this.splitter1.TabStop = false;
            // 
            // listBox1
            // 
            this.listBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(0, 24);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(200, 80);
            this.listBox1.TabIndex = 3;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.listBox1);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(4, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 104);
            this.panel1.TabIndex = 4;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.Constant);
            this.panel2.Controls.Add(this.eEnabled);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(200, 24);
            this.panel2.TabIndex = 4;
            // 
            // Constant
            // 
            this.Constant.AutoSize = true;
            this.Constant.Location = new System.Drawing.Point(76, 4);
            this.Constant.Name = "Constant";
            this.Constant.Size = new System.Drawing.Size(68, 17);
            this.Constant.TabIndex = 1;
            this.Constant.Text = "Constant";
            this.Constant.UseVisualStyleBackColor = true;
            this.Constant.CheckedChanged += new System.EventHandler(this.Constant_CheckedChanged);
            // 
            // eEnabled
            // 
            this.eEnabled.AutoSize = true;
            this.eEnabled.Location = new System.Drawing.Point(4, 4);
            this.eEnabled.Name = "eEnabled";
            this.eEnabled.Size = new System.Drawing.Size(65, 17);
            this.eEnabled.TabIndex = 0;
            this.eEnabled.Text = "Enabled";
            this.eEnabled.UseVisualStyleBackColor = true;
            this.eEnabled.CheckedChanged += new System.EventHandler(this.Enabled_CheckedChanged);
            // 
            // VIS0Editor
            // 
            this.Controls.Add(this.visEditor1);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.panel1);
            this.Name = "VIS0Editor";
            this.Padding = new System.Windows.Forms.Padding(4);
            this.Size = new System.Drawing.Size(507, 112);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public VisEditor visEditor1;
        private Splitter splitter1;
        private ListBox listBox1;
        private Panel panel1;
        private Panel panel2;
        private CheckBox Constant;
        private CheckBox eEnabled;

        public ModelEditControl _mainWindow;

        public VIS0Editor() { InitializeComponent(); visEditor1._mainWindow = this; }

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
        public VIS0Node SelectedAnimation
        {
            get { return _mainWindow._vis0; }
            set { _mainWindow.SelectedVIS0 = value; }
        }

        public void UpdateAnimation()
        {
            listBox1.Items.Clear();
            listBox1.BeginUpdate();
            if (_mainWindow._vis0 != null)
            foreach (VIS0EntryNode n in _mainWindow._vis0.Children)
                listBox1.Items.Add(n);

            listBox1.EndUpdate();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            VIS0EntryNode n = visEditor1.TargetNode = listBox1.Items[listBox1.SelectedIndex] as VIS0EntryNode;
            Constant.Checked = n._flags.HasFlag(VIS0Flags.Constant);
            eEnabled.Checked = n._flags.HasFlag(VIS0Flags.Enabled);
        }

        public void UpdateEntry()
        {
            visEditor1.listBox1.BeginUpdate();
            visEditor1.listBox1.Items.Clear();

            if (visEditor1.TargetNode != null && visEditor1.TargetNode._entryCount > -1)
            {
                for (int i = 0; i < visEditor1.TargetNode._entryCount; i++)
                    visEditor1.listBox1.Items.Add(visEditor1.TargetNode.GetEntry(i));
            }

            visEditor1.listBox1.EndUpdate();
        }

        public void EntryChanged()
        {
            _mainWindow.ReadVIS0();
        }

        private void Enabled_CheckedChanged(object sender, EventArgs e)
        {
            if (visEditor1.TargetNode != null)
            {
                if (Constant.Checked)
                    visEditor1.TargetNode.MakeConstant(eEnabled.Checked);
                else
                    eEnabled.Checked = false;
                UpdateEntry();
            }
        }

        private void Constant_CheckedChanged(object sender, EventArgs e)
        {
            if (visEditor1.TargetNode != null)
            {
                if (Constant.Checked)
                    visEditor1.TargetNode.MakeConstant(eEnabled.Checked);
                else
                {
                    visEditor1.TargetNode.MakeAnimated();
                    eEnabled.Checked = false;
                }
                UpdateEntry();
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool EnableTransformEdit
        {
            get { return _mainWindow._enableTransform; }
            set { panel1.Enabled = (_mainWindow.EnableTransformEdit = value) && (SelectedAnimation != null); }
        }
    }
}