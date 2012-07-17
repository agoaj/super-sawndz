using System;
using BrawlLib.SSBB.ResourceNodes;

namespace System.Windows.Forms
{
    public class SoundPackControl : UserControl
    {
        #region Designer

        public ListView lstSets;
        private ColumnHeader clmIndex;
        private ColumnHeader clmName;
        private ContextMenuStrip contextMenuStrip1;
        private System.ComponentModel.IContainer components;
        private ToolStripMenuItem mnuExport;
        private ToolStripMenuItem mnuReplace;
        private ToolStripMenuItem mnuPath;
        private ColumnHeader clmPath;

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.clmIndex = new System.Windows.Forms.ColumnHeader();
            this.clmName = new System.Windows.Forms.ColumnHeader();
            this.clmPath = new System.Windows.Forms.ColumnHeader();
            this.lstSets = new System.Windows.Forms.ListView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuExport = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuReplace = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuPath = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // clmIndex
            // 
            this.clmIndex.Text = "Index";
            this.clmIndex.Width = 38;
            // 
            // clmName
            // 
            this.clmName.Text = "Name";
            this.clmName.Width = 40;
            // 
            // clmPath
            // 
            this.clmPath.Text = "Path";
            this.clmPath.Width = 336;
            // 
            // lstSets
            // 
            this.lstSets.AutoArrange = false;
            this.lstSets.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lstSets.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.clmIndex,
            this.clmName,
            this.clmPath});
            this.lstSets.ContextMenuStrip = this.contextMenuStrip1;
            this.lstSets.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstSets.FullRowSelect = true;
            this.lstSets.GridLines = true;
            this.lstSets.HideSelection = false;
            this.lstSets.LabelWrap = false;
            this.lstSets.Location = new System.Drawing.Point(0, 0);
            this.lstSets.MultiSelect = false;
            this.lstSets.Name = "lstSets";
            this.lstSets.Size = new System.Drawing.Size(414, 240);
            this.lstSets.TabIndex = 0;
            this.lstSets.UseCompatibleStateImageBehavior = false;
            this.lstSets.View = System.Windows.Forms.View.Details;
            this.lstSets.SelectedIndexChanged += new System.EventHandler(this.lstSets_SelectedIndexChanged);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuExport,
            this.mnuReplace,
            this.mnuPath});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(153, 92);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // mnuExport
            // 
            this.mnuExport.Name = "mnuExport";
            this.mnuExport.Size = new System.Drawing.Size(152, 22);
            this.mnuExport.Text = "Export";
            this.mnuExport.Click += new System.EventHandler(this.mnuExport_Click);
            // 
            // mnuReplace
            // 
            this.mnuReplace.Name = "mnuReplace";
            this.mnuReplace.Size = new System.Drawing.Size(152, 22);
            this.mnuReplace.Text = "Replace";
            // 
            // mnuPath
            // 
            this.mnuPath.Name = "mnuPath";
            this.mnuPath.Size = new System.Drawing.Size(152, 22);
            this.mnuPath.Text = "Path...";
            this.mnuPath.Click += new System.EventHandler(this.mnuPath_Click);
            // 
            // SoundPackControl
            // 
            this.Controls.Add(this.lstSets);
            this.Name = "SoundPackControl";
            this.Size = new System.Drawing.Size(414, 240);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private RSARNode _targetNode;
        public RSARNode TargetNode
        {
            get { return _targetNode; }
            set { _targetNode = value; NodeChanged(); }
        }

        private SoundPackItem _selectedItem;

        public SoundPackControl() { InitializeComponent(); }

        private void NodeChanged()
        {
            lstSets.BeginUpdate();

            lstSets.Items.Clear();
            if (_targetNode != null)
                foreach (RSARFileNode file in _targetNode.Files)
                    lstSets.Items.Add(new SoundPackItem(file));

            lstSets.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);

            lstSets.EndUpdate();
        }

        private void contextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_selectedItem == null)
                e.Cancel = true;
            else
            {
                if (_selectedItem._node is RSARExtFileNode)
                    mnuExport.Enabled = false;
                else
                    mnuExport.Enabled = true;
            }
        }

        private void mnuPath_Click(object sender, EventArgs e)
        {
            using (SoundPathChanger dlg = new SoundPathChanger())
            {
                dlg.FilePath = _selectedItem._node._extPath;
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    _selectedItem._node.ExtPath = dlg.FilePath;
                    _selectedItem.SubItems[2].Text = dlg.FilePath;
                }
            }
        }

        private void lstSets_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstSets.SelectedIndices.Count == 0)
                _selectedItem = null;
            else
                _selectedItem = lstSets.SelectedItems[0] as SoundPackItem;
        }

        private void mnuExport_Click(object sender, EventArgs e)
        {
            using(SaveFileDialog dlg = new SaveFileDialog())
            {
                dlg.FileName = _selectedItem.SubItems[1].Text;
                if (dlg.ShowDialog(this) == DialogResult.OK)
                    _selectedItem._node.Export(dlg.FileName);
            }
        }
    }

    public class SoundPackItem : ListViewItem
    {
        public RSARFileNode _node;

        public SoundPackItem(RSARFileNode file)
        {
            ImageIndex = (byte)file.ResourceType;

            Text = file.FileNodeIndex.ToString();
            SubItems.Add(file.Name);
            _node = file;

            SubItems.Add(file.ExtPath);
        }
    }
}
