using System;
using System.Windows.Forms;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.Wii.Animations;
using System.Collections.Generic;

namespace System.Windows.Forms
{
    public class LightEditor : Form
    {
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private TextBox ax;
        private TextBox px;
        private TextBox dx;
        private TextBox sx;
        private TextBox sy;
        private TextBox dy;
        private TextBox py;
        private TextBox ay;
        private TextBox sz;
        private TextBox dz;
        private TextBox pz;
        private TextBox az;
        private TextBox sw;
        private TextBox dw;
        private TextBox pw;
        private TextBox aw;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private Label label10;
        private Label label9;
        private Label label11;
        private Label label13;
        private Label label12;
        private Label label14;
        private TextBox farZ;
        private TextBox nearZ;
        private TextBox yFov;
        private TextBox zScale;
        private TextBox tScale;
        private TextBox rScale;
        private TextBox dCamZ;
        private TextBox dCamY;
        private TextBox dCamX;
        private Label label15;
        private ModelEditControl form;

        public LightEditor() { InitializeComponent(); }

        public DialogResult ShowDialog(ModelEditControl owner)
        {
            form = owner;

            ax.Text = form.modelPanel1.Ambient._x.ToString();
            ay.Text = form.modelPanel1.Ambient._y.ToString();
            az.Text = form.modelPanel1.Ambient._z.ToString();
            aw.Text = form.modelPanel1.Ambient._w.ToString();

            px.Text = form.modelPanel1.LightPosition._x.ToString();
            py.Text = form.modelPanel1.LightPosition._y.ToString();
            pz.Text = form.modelPanel1.LightPosition._z.ToString();
            pw.Text = form.modelPanel1.LightPosition._w.ToString();

            dx.Text = form.modelPanel1.Diffuse._x.ToString();
            dy.Text = form.modelPanel1.Diffuse._y.ToString();
            dz.Text = form.modelPanel1.Diffuse._z.ToString();
            dw.Text = form.modelPanel1.Diffuse._w.ToString();

            sx.Text = form.modelPanel1.Specular._x.ToString();
            sy.Text = form.modelPanel1.Specular._y.ToString();
            sz.Text = form.modelPanel1.Specular._z.ToString();
            sw.Text = form.modelPanel1.Specular._w.ToString();

            tScale.Text = form.modelPanel1.TranslationScale.ToString();
            rScale.Text = form.modelPanel1.RotationScale.ToString();
            zScale.Text = form.modelPanel1.ZoomScale.ToString();

            yFov.Text = form.modelPanel1._fovY.ToString();
            nearZ.Text = form.modelPanel1._nearZ.ToString();
            farZ.Text = form.modelPanel1._farZ.ToString();

            dCamX.Text = form.modelPanel1._defaultTranslate._x.ToString();
            dCamY.Text = form.modelPanel1._defaultTranslate._y.ToString();
            dCamZ.Text = form.modelPanel1._defaultTranslate._z.ToString();

            return base.ShowDialog(owner);
        }

        private unsafe void btnOkay_Click(object sender, EventArgs e)
        {
            Vector4 Ambient = new Vector4();
            if (!float.TryParse(ax.Text, out Ambient._x))
                Ambient._x = 0.2f;
            if (!float.TryParse(ay.Text, out Ambient._y))
                Ambient._y = 0.2f;
            if (!float.TryParse(az.Text, out Ambient._z))
                Ambient._z = 0.2f;
            if (!float.TryParse(aw.Text, out Ambient._w))
                Ambient._w = 1;
            form.modelPanel1.Ambient = Ambient;

            Vector4 Light = new Vector4();
            if (!float.TryParse(px.Text, out Light._x))
                Light._x = 0;
            if (!float.TryParse(py.Text, out Light._y))
                Light._y = 6;
            if (!float.TryParse(pz.Text, out Light._z))
                Light._z = 6;
            if (!float.TryParse(pw.Text, out Light._w))
                Light._w = 0;
            form.modelPanel1.LightPosition = Light;

            Vector4 Diffuse = new Vector4();
            if (!float.TryParse(dx.Text, out Diffuse._x))
                Diffuse._x = 0.8f;
            if (!float.TryParse(dy.Text, out Diffuse._y))
                Diffuse._y = 0.8f;
            if (!float.TryParse(dz.Text, out Diffuse._z))
                Diffuse._z = 0.8f;
            if (!float.TryParse(dw.Text, out Diffuse._w))
                Diffuse._w = 1;
            form.modelPanel1.Diffuse = Diffuse;

            Vector4 Specular = new Vector4();
            if (!float.TryParse(sx.Text, out Specular._x))
                Specular._x = 0.5f;
            if (!float.TryParse(sy.Text, out Specular._y))
                Specular._y = 0.5f;
            if (!float.TryParse(sz.Text, out Specular._z))
                Specular._z = 0.5f;
            if (!float.TryParse(sw.Text, out Specular._w))
                Specular._w = 1;
            form.modelPanel1.Specular = Specular;

            float val;
            if (!float.TryParse(tScale.Text, out val))
                val = 0.05f;
            form.modelPanel1.TranslationScale = val;
            if (!float.TryParse(rScale.Text, out val))
                val = 0.1f;
            form.modelPanel1.RotationScale = val;
            if (!float.TryParse(zScale.Text, out val))
                val = 2.5f;
            form.modelPanel1.ZoomScale = val;

            if (!float.TryParse(yFov.Text, out val))
                val = 45.0f;
            form.modelPanel1._fovY = val;
            if (!float.TryParse(nearZ.Text, out val))
                val = 1.0f;
            form.modelPanel1._nearZ = val;
            if (!float.TryParse(farZ.Text, out val))
                val = 20000.0f;
            form.modelPanel1._farZ = val;

            form.modelPanel1._projectionChanged = true;

            if (!float.TryParse(dCamX.Text, out val))
                val = 0;
            form.modelPanel1._defaultTranslate._x = val;
            if (!float.TryParse(dCamY.Text, out val))
                val = 0;
            form.modelPanel1._defaultTranslate._y = val;
            if (!float.TryParse(dCamZ.Text, out val))
                val = 0;
            form.modelPanel1._defaultTranslate._z = val;

            form.modelPanel1.Invalidate();

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e) { DialogResult = DialogResult.Cancel; Close(); }

        #region Designer

        private Button btnCancel;
        private Button btnOkay;

        private void InitializeComponent()
        {
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOkay = new System.Windows.Forms.Button();
            this.ax = new System.Windows.Forms.TextBox();
            this.px = new System.Windows.Forms.TextBox();
            this.dx = new System.Windows.Forms.TextBox();
            this.sx = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.sy = new System.Windows.Forms.TextBox();
            this.dy = new System.Windows.Forms.TextBox();
            this.py = new System.Windows.Forms.TextBox();
            this.ay = new System.Windows.Forms.TextBox();
            this.sz = new System.Windows.Forms.TextBox();
            this.dz = new System.Windows.Forms.TextBox();
            this.pz = new System.Windows.Forms.TextBox();
            this.az = new System.Windows.Forms.TextBox();
            this.sw = new System.Windows.Forms.TextBox();
            this.dw = new System.Windows.Forms.TextBox();
            this.pw = new System.Windows.Forms.TextBox();
            this.aw = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dCamZ = new System.Windows.Forms.TextBox();
            this.dCamY = new System.Windows.Forms.TextBox();
            this.dCamX = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.farZ = new System.Windows.Forms.TextBox();
            this.nearZ = new System.Windows.Forms.TextBox();
            this.yFov = new System.Windows.Forms.TextBox();
            this.zScale = new System.Windows.Forms.TextBox();
            this.tScale = new System.Windows.Forms.TextBox();
            this.rScale = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(288, 278);
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
            this.btnOkay.Location = new System.Drawing.Point(207, 278);
            this.btnOkay.Name = "btnOkay";
            this.btnOkay.Size = new System.Drawing.Size(75, 23);
            this.btnOkay.TabIndex = 1;
            this.btnOkay.Text = "&Okay";
            this.btnOkay.UseVisualStyleBackColor = true;
            this.btnOkay.Click += new System.EventHandler(this.btnOkay_Click);
            // 
            // ax
            // 
            this.ax.Location = new System.Drawing.Point(66, 30);
            this.ax.Name = "ax";
            this.ax.Size = new System.Drawing.Size(67, 20);
            this.ax.TabIndex = 3;
            // 
            // px
            // 
            this.px.Location = new System.Drawing.Point(65, 56);
            this.px.Name = "px";
            this.px.Size = new System.Drawing.Size(67, 20);
            this.px.TabIndex = 4;
            // 
            // dx
            // 
            this.dx.Location = new System.Drawing.Point(65, 82);
            this.dx.Name = "dx";
            this.dx.Size = new System.Drawing.Size(67, 20);
            this.dx.TabIndex = 5;
            // 
            // sx
            // 
            this.sx.Location = new System.Drawing.Point(66, 108);
            this.sx.Name = "sx";
            this.sx.Size = new System.Drawing.Size(67, 20);
            this.sx.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Ambient:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Position:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 85);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(43, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Diffuse:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 111);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(52, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Specular:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(63, 11);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(14, 13);
            this.label5.TabIndex = 19;
            this.label5.Text = "X";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(135, 11);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(14, 13);
            this.label6.TabIndex = 20;
            this.label6.Text = "Y";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(209, 11);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(14, 13);
            this.label7.TabIndex = 21;
            this.label7.Text = "Z";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(282, 11);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(18, 13);
            this.label8.TabIndex = 22;
            this.label8.Text = "W";
            // 
            // sy
            // 
            this.sy.Location = new System.Drawing.Point(139, 108);
            this.sy.Name = "sy";
            this.sy.Size = new System.Drawing.Size(67, 20);
            this.sy.TabIndex = 26;
            // 
            // dy
            // 
            this.dy.Location = new System.Drawing.Point(138, 82);
            this.dy.Name = "dy";
            this.dy.Size = new System.Drawing.Size(67, 20);
            this.dy.TabIndex = 25;
            // 
            // py
            // 
            this.py.Location = new System.Drawing.Point(138, 56);
            this.py.Name = "py";
            this.py.Size = new System.Drawing.Size(67, 20);
            this.py.TabIndex = 24;
            // 
            // ay
            // 
            this.ay.Location = new System.Drawing.Point(139, 30);
            this.ay.Name = "ay";
            this.ay.Size = new System.Drawing.Size(67, 20);
            this.ay.TabIndex = 23;
            // 
            // sz
            // 
            this.sz.Location = new System.Drawing.Point(212, 108);
            this.sz.Name = "sz";
            this.sz.Size = new System.Drawing.Size(67, 20);
            this.sz.TabIndex = 30;
            // 
            // dz
            // 
            this.dz.Location = new System.Drawing.Point(211, 82);
            this.dz.Name = "dz";
            this.dz.Size = new System.Drawing.Size(67, 20);
            this.dz.TabIndex = 29;
            // 
            // pz
            // 
            this.pz.Location = new System.Drawing.Point(211, 56);
            this.pz.Name = "pz";
            this.pz.Size = new System.Drawing.Size(67, 20);
            this.pz.TabIndex = 28;
            // 
            // az
            // 
            this.az.Location = new System.Drawing.Point(212, 30);
            this.az.Name = "az";
            this.az.Size = new System.Drawing.Size(67, 20);
            this.az.TabIndex = 27;
            // 
            // sw
            // 
            this.sw.Location = new System.Drawing.Point(285, 108);
            this.sw.Name = "sw";
            this.sw.Size = new System.Drawing.Size(67, 20);
            this.sw.TabIndex = 34;
            // 
            // dw
            // 
            this.dw.Location = new System.Drawing.Point(284, 82);
            this.dw.Name = "dw";
            this.dw.Size = new System.Drawing.Size(67, 20);
            this.dw.TabIndex = 33;
            // 
            // pw
            // 
            this.pw.Location = new System.Drawing.Point(284, 56);
            this.pw.Name = "pw";
            this.pw.Size = new System.Drawing.Size(67, 20);
            this.pw.TabIndex = 32;
            // 
            // aw
            // 
            this.aw.Location = new System.Drawing.Point(285, 30);
            this.aw.Name = "aw";
            this.aw.Size = new System.Drawing.Size(67, 20);
            this.aw.TabIndex = 31;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.sw);
            this.groupBox1.Controls.Add(this.dw);
            this.groupBox1.Controls.Add(this.pw);
            this.groupBox1.Controls.Add(this.aw);
            this.groupBox1.Controls.Add(this.sz);
            this.groupBox1.Controls.Add(this.dz);
            this.groupBox1.Controls.Add(this.pz);
            this.groupBox1.Controls.Add(this.az);
            this.groupBox1.Controls.Add(this.sy);
            this.groupBox1.Controls.Add(this.dy);
            this.groupBox1.Controls.Add(this.py);
            this.groupBox1.Controls.Add(this.ay);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.sx);
            this.groupBox1.Controls.Add(this.dx);
            this.groupBox1.Controls.Add(this.px);
            this.groupBox1.Controls.Add(this.ax);
            this.groupBox1.Location = new System.Drawing.Point(1, 129);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(362, 143);
            this.groupBox1.TabIndex = 35;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Lighting";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dCamZ);
            this.groupBox2.Controls.Add(this.dCamY);
            this.groupBox2.Controls.Add(this.dCamX);
            this.groupBox2.Controls.Add(this.label15);
            this.groupBox2.Controls.Add(this.farZ);
            this.groupBox2.Controls.Add(this.nearZ);
            this.groupBox2.Controls.Add(this.yFov);
            this.groupBox2.Controls.Add(this.zScale);
            this.groupBox2.Controls.Add(this.tScale);
            this.groupBox2.Controls.Add(this.rScale);
            this.groupBox2.Controls.Add(this.label14);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(375, 123);
            this.groupBox2.TabIndex = 36;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Projection";
            // 
            // dCamZ
            // 
            this.dCamZ.Location = new System.Drawing.Point(255, 91);
            this.dCamZ.Name = "dCamZ";
            this.dCamZ.Size = new System.Drawing.Size(66, 20);
            this.dCamZ.TabIndex = 15;
            // 
            // dCamY
            // 
            this.dCamY.Location = new System.Drawing.Point(183, 90);
            this.dCamY.Name = "dCamY";
            this.dCamY.Size = new System.Drawing.Size(66, 20);
            this.dCamY.TabIndex = 14;
            // 
            // dCamX
            // 
            this.dCamX.Location = new System.Drawing.Point(111, 91);
            this.dCamX.Name = "dCamX";
            this.dCamX.Size = new System.Drawing.Size(66, 20);
            this.dCamX.TabIndex = 13;
            // 
            // label15
            // 
            this.label15.Location = new System.Drawing.Point(-3, 88);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(108, 23);
            this.label15.TabIndex = 12;
            this.label15.Text = "Default Camera:";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // farZ
            // 
            this.farZ.Location = new System.Drawing.Point(274, 63);
            this.farZ.Name = "farZ";
            this.farZ.Size = new System.Drawing.Size(66, 20);
            this.farZ.TabIndex = 11;
            // 
            // nearZ
            // 
            this.nearZ.Location = new System.Drawing.Point(274, 41);
            this.nearZ.Name = "nearZ";
            this.nearZ.Size = new System.Drawing.Size(66, 20);
            this.nearZ.TabIndex = 10;
            // 
            // yFov
            // 
            this.yFov.Location = new System.Drawing.Point(274, 19);
            this.yFov.Name = "yFov";
            this.yFov.Size = new System.Drawing.Size(66, 20);
            this.yFov.TabIndex = 9;
            // 
            // zScale
            // 
            this.zScale.Location = new System.Drawing.Point(111, 63);
            this.zScale.Name = "zScale";
            this.zScale.Size = new System.Drawing.Size(66, 20);
            this.zScale.TabIndex = 8;
            // 
            // tScale
            // 
            this.tScale.Location = new System.Drawing.Point(111, 41);
            this.tScale.Name = "tScale";
            this.tScale.Size = new System.Drawing.Size(66, 20);
            this.tScale.TabIndex = 7;
            // 
            // rScale
            // 
            this.rScale.Location = new System.Drawing.Point(111, 19);
            this.rScale.Name = "rScale";
            this.rScale.Size = new System.Drawing.Size(66, 20);
            this.rScale.TabIndex = 6;
            // 
            // label14
            // 
            this.label14.Location = new System.Drawing.Point(183, 66);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(85, 13);
            this.label14.TabIndex = 5;
            this.label14.Text = "Far Z: ";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label13
            // 
            this.label13.Location = new System.Drawing.Point(183, 44);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(85, 13);
            this.label13.TabIndex = 4;
            this.label13.Text = "Near Z: ";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label12
            // 
            this.label12.Location = new System.Drawing.Point(183, 22);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(85, 13);
            this.label12.TabIndex = 3;
            this.label12.Text = "Y Field Of View: ";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(11, 66);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(94, 13);
            this.label11.TabIndex = 2;
            this.label11.Text = "Zoom Scale: ";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(10, 44);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(95, 13);
            this.label10.TabIndex = 1;
            this.label10.Text = "Translation Scale: ";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(13, 22);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(92, 13);
            this.label9.TabIndex = 0;
            this.label9.Text = "Rotation Scale: ";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // LightEditor
            // 
            this.AcceptButton = this.btnOkay;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(375, 313);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnOkay);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "LightEditor";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Viewer Settings";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion
    }
}
