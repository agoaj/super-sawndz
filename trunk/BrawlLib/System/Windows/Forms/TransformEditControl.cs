using System;

namespace System.Windows.Forms
{

    public class TransformEditControl : UserControl
    {

        public TransformEditControl() { InitializeComponent(); }

        #region Designer

        private Label label5;
        private NumericInputBox numScaleZ;
        private NumericInputBox numTransX;
        private NumericInputBox numScaleY;
        private Label label6;
        private NumericInputBox numScaleX;
        private Label label7;
        private NumericInputBox numRotZ;
        private Label label8;
        private NumericInputBox numRotY;
        private Label label9;
        private NumericInputBox numRotX;
        private Label label10;
        private NumericInputBox numTransZ;
        private Label label11;
        private NumericInputBox numTransY;
        private Label label12;
        private Label label13;
    
        private void InitializeComponent()
        {
            this.label5 = new System.Windows.Forms.Label();
            this.numScaleZ = new System.Windows.Forms.NumericInputBox();
            this.numTransX = new System.Windows.Forms.NumericInputBox();
            this.numScaleY = new System.Windows.Forms.NumericInputBox();
            this.label6 = new System.Windows.Forms.Label();
            this.numScaleX = new System.Windows.Forms.NumericInputBox();
            this.label7 = new System.Windows.Forms.Label();
            this.numRotZ = new System.Windows.Forms.NumericInputBox();
            this.label8 = new System.Windows.Forms.Label();
            this.numRotY = new System.Windows.Forms.NumericInputBox();
            this.label9 = new System.Windows.Forms.Label();
            this.numRotX = new System.Windows.Forms.NumericInputBox();
            this.label10 = new System.Windows.Forms.Label();
            this.numTransZ = new System.Windows.Forms.NumericInputBox();
            this.label11 = new System.Windows.Forms.Label();
            this.numTransY = new System.Windows.Forms.NumericInputBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(3, 3);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(74, 20);
            this.label5.TabIndex = 22;
            this.label5.Text = "Translation X:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numScaleZ
            // 
            this.numScaleZ.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.numScaleZ.Location = new System.Drawing.Point(83, 179);
            this.numScaleZ.Name = "numScaleZ";
            this.numScaleZ.Size = new System.Drawing.Size(82, 20);
            this.numScaleZ.TabIndex = 38;
            this.numScaleZ.Text = "0";
            // 
            // numTransX
            // 
            this.numTransX.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.numTransX.Location = new System.Drawing.Point(83, 3);
            this.numTransX.Name = "numTransX";
            this.numTransX.Size = new System.Drawing.Size(82, 20);
            this.numTransX.TabIndex = 21;
            this.numTransX.Text = "0";
            // 
            // numScaleY
            // 
            this.numScaleY.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.numScaleY.Location = new System.Drawing.Point(83, 159);
            this.numScaleY.Name = "numScaleY";
            this.numScaleY.Size = new System.Drawing.Size(82, 20);
            this.numScaleY.TabIndex = 37;
            this.numScaleY.Text = "0";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(3, 23);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(74, 20);
            this.label6.TabIndex = 23;
            this.label6.Text = "Translation Y:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numScaleX
            // 
            this.numScaleX.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.numScaleX.Location = new System.Drawing.Point(83, 139);
            this.numScaleX.Name = "numScaleX";
            this.numScaleX.Size = new System.Drawing.Size(82, 20);
            this.numScaleX.TabIndex = 36;
            this.numScaleX.Text = "0";
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(3, 43);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(74, 20);
            this.label7.TabIndex = 24;
            this.label7.Text = "Translation Z:";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numRotZ
            // 
            this.numRotZ.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.numRotZ.Location = new System.Drawing.Point(83, 111);
            this.numRotZ.Name = "numRotZ";
            this.numRotZ.Size = new System.Drawing.Size(82, 20);
            this.numRotZ.TabIndex = 35;
            this.numRotZ.Text = "0";
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(3, 71);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(74, 20);
            this.label8.TabIndex = 25;
            this.label8.Text = "Rotation X:";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numRotY
            // 
            this.numRotY.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.numRotY.Location = new System.Drawing.Point(83, 91);
            this.numRotY.Name = "numRotY";
            this.numRotY.Size = new System.Drawing.Size(82, 20);
            this.numRotY.TabIndex = 34;
            this.numRotY.Text = "0";
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(3, 91);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(74, 20);
            this.label9.TabIndex = 26;
            this.label9.Text = "Rotation Y:";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numRotX
            // 
            this.numRotX.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.numRotX.Location = new System.Drawing.Point(83, 71);
            this.numRotX.Name = "numRotX";
            this.numRotX.Size = new System.Drawing.Size(82, 20);
            this.numRotX.TabIndex = 33;
            this.numRotX.Text = "0";
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(3, 111);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(74, 20);
            this.label10.TabIndex = 27;
            this.label10.Text = "Rotation Z:";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numTransZ
            // 
            this.numTransZ.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.numTransZ.Location = new System.Drawing.Point(83, 43);
            this.numTransZ.Name = "numTransZ";
            this.numTransZ.Size = new System.Drawing.Size(82, 20);
            this.numTransZ.TabIndex = 32;
            this.numTransZ.Text = "0";
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(3, 139);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(74, 20);
            this.label11.TabIndex = 28;
            this.label11.Text = "Scale X:";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numTransY
            // 
            this.numTransY.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.numTransY.Location = new System.Drawing.Point(83, 23);
            this.numTransY.Name = "numTransY";
            this.numTransY.Size = new System.Drawing.Size(82, 20);
            this.numTransY.TabIndex = 31;
            this.numTransY.Text = "0";
            // 
            // label12
            // 
            this.label12.Location = new System.Drawing.Point(3, 179);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(74, 20);
            this.label12.TabIndex = 29;
            this.label12.Text = "Scale Z:";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label13
            // 
            this.label13.Location = new System.Drawing.Point(3, 159);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(74, 20);
            this.label13.TabIndex = 30;
            this.label13.Text = "Scale Y:";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // TransformEditControl
            // 
            this.Controls.Add(this.label5);
            this.Controls.Add(this.numScaleZ);
            this.Controls.Add(this.numTransX);
            this.Controls.Add(this.numScaleY);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.numScaleX);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.numRotZ);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.numRotY);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.numRotX);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.numTransZ);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.numTransY);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label13);
            this.Name = "TransformEditControl";
            this.Size = new System.Drawing.Size(168, 203);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

    }
}
