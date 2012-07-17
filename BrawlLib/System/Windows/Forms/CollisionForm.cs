using System;
using BrawlLib.SSBB.ResourceNodes;

namespace System.Windows.Forms
{
    public class CollisionForm : Form
    {
        #region Designer

        private CollisionEditor collisionEditor1;
    
        private void InitializeComponent()
        {
            this.collisionEditor1 = new System.Windows.Forms.CollisionEditor();
            this.SuspendLayout();
            // 
            // collisionEditor1
            // 
            this.collisionEditor1.BackColor = System.Drawing.Color.Lavender;
            this.collisionEditor1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.collisionEditor1.Location = new System.Drawing.Point(0, 0);
            this.collisionEditor1.Name = "collisionEditor1";
            this.collisionEditor1.Size = new System.Drawing.Size(562, 338);
            this.collisionEditor1.TabIndex = 0;
            // 
            // CollisionForm
            // 
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Controls.Add(this.collisionEditor1);
            this.MinimizeBox = false;
            this.Name = "CollisionForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Collision Editor";
            this.ResumeLayout(false);

        }

        #endregion

        CollisionNode _node;

        public CollisionForm() { InitializeComponent(); }

        public DialogResult ShowDialog(IWin32Window owner, CollisionNode node)
        {
            _node = node;
            try { return ShowDialog(owner); }
            finally { _node.SignalPropertyChange(); _node = null; }
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            collisionEditor1.TargetNode = _node;
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
            collisionEditor1.TargetNode = null;
        }
    }
}
