using System;

namespace System.Windows.Forms
{
    public class SCN0Editor : UserControl
    {
        #region Designer
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // SCN0Editor
            // 
            this.Name = "SCN0Editor";
            this.Size = new System.Drawing.Size(553, 48);
            this.ResumeLayout(false);

        }

        #endregion

        public ModelEditControl _mainWindow;

        public SCN0Editor() { InitializeComponent(); }
    }
}
