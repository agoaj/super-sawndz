using System;
using System.Windows.Forms;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.Wii.Animations;

namespace System.Windows.Forms
{
    public class ChangeToDialog2 : Form
    {
        private ResourceNode _node;
        private CHR0EntryNode _copyNode = null;
        private TextBox ScaleX;
        private TextBox ScaleY;
        private TextBox ScaleZ;
        private Label label3;
        private Label label4;
        private Label label5;
        private TextBox RotX;
        private TextBox RotY;
        private TextBox RotZ;
        private TextBox TransX;
        private TextBox TransY;
        private TextBox TransZ;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label label9;
        private Label label10;
        private CheckBox ReplaceScale;
        private CheckBox AddScale;
        private CheckBox SubtractScale;
        private CheckBox SubtractRot;
        private CheckBox AddRot;
        private CheckBox ReplaceRot;
        private CheckBox SubtractTrans;
        private CheckBox AddTrans;
        private CheckBox ReplaceTrans;
        private CheckBox copyKeyframes;
        private TextBox textBox1;
        private CheckBox ChangeVersion;
        private ComboBox Version;
        private CheckBox Port;
        private CheckBox noLoop;
        private Label label11;

        public ChangeToDialog2() { InitializeComponent(); }

        public DialogResult ShowDialog(IWin32Window owner, ResourceNode node)
        {
            name.MaxLength = 255;
            _node = node;
            Version.Items.Add("Version 4");
            Version.Items.Add("Version 5");
            Version.SelectedIndex = 0;
            try { return base.ShowDialog(owner); }
            finally { _node = null; }
        }

        private unsafe void btnOkay_Click(object sender, EventArgs e)
        {
            float x, y, z;
            string _name = name.Text;
            MDL0Node model = new MDL0Node();
            MDL0Node _targetModel = new MDL0Node();
            bool disableLoop = false;

            if (noLoop.Checked)
                disableLoop = true;
             
            if (Port.Checked)
            {
                MessageBox.Show("Please open the model you want to port the animations to.\nThen open the model the animations work normally for.");
                OpenFileDialog dlgOpen = new OpenFileDialog();
                OpenFileDialog dlgOpen2 = new OpenFileDialog();
                dlgOpen.Filter = dlgOpen2.Filter = "MDL0 Raw Model (*.mdl0)|*.mdl0";
                dlgOpen.Title = "Select the model to port the animations to...";
                dlgOpen2.Title = "Select the model the animations are for...";
                if (dlgOpen.ShowDialog() == DialogResult.OK)
                {
                    _targetModel = (MDL0Node)NodeFactory.FromFile(null, dlgOpen.FileName);
                    if (dlgOpen2.ShowDialog() == DialogResult.OK)
                        model = (MDL0Node)NodeFactory.FromFile(null, dlgOpen2.FileName);
                }
            }

            try { x = Convert.ToSingle(ScaleX.Text); }
            catch { x = 0; }
            try { y = Convert.ToSingle(ScaleY.Text); }
            catch { y = 0; }
            try { z = Convert.ToSingle(ScaleZ.Text); }
            catch { z = 0; }

            Vector3 scale = new Vector3(x, y, z);

            try { x = Convert.ToSingle(RotX.Text); }
            catch { x = 0; }
            try { y = Convert.ToSingle(RotY.Text); }
            catch { y = 0; }
            try { z = Convert.ToSingle(RotZ.Text); }
            catch { z = 0; }

            Vector3 rot = new Vector3(x, y, z);

            try { x = Convert.ToSingle(TransX.Text); }
            catch { x = 0; }
            try { y = Convert.ToSingle(TransY.Text); }
            catch { y = 0; }
            try { z = Convert.ToSingle(TransZ.Text); }
            catch { z = 0; }

            Vector3 trans = new Vector3(x, y, z);

            if (_name != null)
                ChangeNode(_name, _node, scale, rot, trans, _targetModel, model, disableLoop);
            
            DialogResult = DialogResult.OK;
            Close();
        }

        public unsafe void ChangeNode(string _name, ResourceNode parent, Vector3 scale, Vector3 rot, Vector3 trans, MDL0Node _targetModel, MDL0Node model, bool disLoop)
        {
            int numFrames;
            float* v;
            bool hasKeyframe = false;
            CHR0EntryNode entry;
            KeyframeEntry kfe;
            AnimationFrame anim = new AnimationFrame();
            foreach (ResourceNode r in parent.Children)
            {
                if (r is CHR0Node)
                {
                    if (Port.Checked && _targetModel != null && model != null)
                        ((CHR0Node)r).Port(_targetModel, model);

                    if (Version.Enabled)
                        if (Version.SelectedIndex == 0)
                            ((CHR0Node)r).Version = 4;
                        else if (Version.SelectedIndex == 1)
                            ((CHR0Node)r).Version = 5;

                    if (disLoop)
                        ((CHR0Node)r).Loop = false;

                    _copyNode = r.FindChild(textBox1.Text, false) as CHR0EntryNode;

                    if (r.FindChild(_name, false) == null)
                    {
                        if (_name != null && _name != String.Empty)
                        {
                            CHR0EntryNode c = new CHR0EntryNode();
                            c._numFrames = (r as CHR0Node).FrameCount;
                            c.Name = _name;

                            if (_copyNode != null)
                                for (int x = 0; x < _copyNode._numFrames; x++)
                                    for (int i = 0x10; i < 0x19; i++)
                                        if ((kfe = _copyNode.GetKeyframe((KeyFrameMode)i, x)) != null)
                                            c.SetKeyframe((KeyFrameMode)i, x, kfe._value);

                            r.Children.Add(c);
                            r._changed = true;
                        }
                    }
                }
                if (r.Name == _name)
                {
                    if (r is CHR0EntryNode)
                    {
                        entry = r as CHR0EntryNode;
                        numFrames = entry.FrameCount;
                        if (ReplaceScale.Checked)
                        {
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
                        if (ReplaceRot.Checked)
                        {
                            for (int x = 0; x < numFrames; x++)
                            {
                                for (int i = 0x13; i < 0x16; i++)
                                {
                                    if (entry.GetKeyframe((KeyFrameMode)i, x) != null)
                                        entry.RemoveKeyframe((KeyFrameMode)i, x);
                                }
                            }
                            entry.SetKeyframeOnlyRot(0, rot);
                        }
                        if (ReplaceTrans.Checked)
                        {
                            for (int x = 0; x < numFrames; x++)
                            {
                                for (int i = 0x16; i < 0x19; i++)
                                {
                                    if (entry.GetKeyframe((KeyFrameMode)i, x) != null)
                                        entry.RemoveKeyframe((KeyFrameMode)i, x);
                                }
                            }
                            entry.SetKeyframeOnlyTrans(0, trans);
                        }
                        hasKeyframe = false;
                        if (AddScale.Checked)
                        {
                            for (int x = 0; x < numFrames; x++)
                            {
                                for (int i = 0x10; i < 0x13; i++)
                                {
                                    if ((kfe = entry.GetKeyframe((KeyFrameMode)i, x)) != null)
                                    {
                                        switch (i)
                                        {
                                            case 0x10:
                                                kfe._value += scale._x;
                                                break;
                                            case 0x11:
                                                kfe._value += scale._y;
                                                break;
                                            case 0x12:
                                                kfe._value += scale._z;
                                                break;
                                        }
                                        hasKeyframe = true;
                                    }
                                }
                            }
                            if (!hasKeyframe)
                            {
                                anim = entry.GetAnimFrame(0);
                                Vector3 newScale = anim.Scale;
                                scale._x += newScale._x;
                                scale._y += newScale._y;
                                scale._z += newScale._z;
                                entry.SetKeyframeOnlyScale(0, scale);
                            }
                        }
                        hasKeyframe = false;
                        if (AddRot.Checked)
                        {
                            for (int x = 0; x < numFrames; x++)
                            {
                                for (int i = 0x13; i < 0x16; i++)
                                {
                                    if ((kfe = entry.GetKeyframe((KeyFrameMode)i, x)) != null)
                                    {
                                        switch (i)
                                        {
                                            case 0x13:
                                                kfe._value += rot._x;
                                                break;
                                            case 0x14:
                                                kfe._value += rot._y;
                                                break;
                                            case 0x15:
                                                kfe._value += rot._z;
                                                break;
                                        }
                                        hasKeyframe = true;
                                    }
                                }
                            }
                            if (!hasKeyframe)
                            {
                                anim = entry.GetAnimFrame(0);
                                Vector3 newRot = anim.Rotation;
                                rot._x += newRot._x;
                                rot._y += newRot._y;
                                rot._z += newRot._z;
                                entry.SetKeyframeOnlyRot(0, rot);
                            }
                        }
                        hasKeyframe = false;
                        if (AddTrans.Checked)
                        {
                            for (int x = 0; x < numFrames; x++)
                            {
                                for (int i = 0x16; i < 0x19; i++)
                                {
                                    if ((kfe = entry.GetKeyframe((KeyFrameMode)i, x)) != null)
                                    {
                                        switch (i)
                                        {
                                            case 0x16:
                                            kfe._value += trans._x;
                                            break;
                                            case 0x17:
                                            kfe._value += trans._y;
                                            break;
                                            case 0x18:
                                            kfe._value += trans._z;
                                            break;
                                        }
                                        hasKeyframe = true;
                                    }
                                }
                            }
                            if (!hasKeyframe)
                            {
                                anim = entry.GetAnimFrame(0);
                                Vector3 newTrans = anim.Translation;
                                trans._x += newTrans._x;
                                trans._y += newTrans._y;
                                trans._z += newTrans._z;
                                entry.SetKeyframeOnlyTrans(0, trans);
                            }
                        }
                        hasKeyframe = false;
                        if (SubtractScale.Checked)
                        {
                            for (int x = 0; x < numFrames; x++)
                            {
                                for (int i = 0x10; i < 0x13; i++)
                                {
                                    if ((kfe = entry.GetKeyframe((KeyFrameMode)i, x)) != null)
                                    {
                                        switch (i)
                                        {
                                            case 0x10:
                                                kfe._value -= scale._x;
                                                break;
                                            case 0x11:
                                                kfe._value -= scale._y;
                                                break;
                                            case 0x12:
                                                kfe._value -= scale._z;
                                                break;
                                        }
                                        hasKeyframe = true;
                                    }
                                }
                            }
                            if (!hasKeyframe)
                            {
                                anim = entry.GetAnimFrame(0);
                                Vector3 newScale = anim.Scale;
                                scale._x = newScale._x - scale._x;
                                scale._y = newScale._y - scale._y;
                                scale._z = newScale._z - scale._z;
                                entry.SetKeyframeOnlyScale(0, scale);
                            }
                        }
                        hasKeyframe = false;
                        if (SubtractRot.Checked)
                        {
                            for (int x = 0; x < numFrames; x++)
                            {
                                for (int i = 0x13; i < 0x16; i++)
                                {
                                    if ((kfe = entry.GetKeyframe((KeyFrameMode)i, x)) != null)
                                    {
                                        switch (i)
                                        {
                                            case 0x13:
                                                kfe._value -= rot._x;
                                                break;
                                            case 0x14:
                                                kfe._value -= rot._y;
                                                break;
                                            case 0x15:
                                                kfe._value -= rot._z;
                                                break;
                                        }
                                        hasKeyframe = true;
                                    }
                                }
                            }
                            if (!hasKeyframe)
                            {
                                anim = entry.GetAnimFrame(0);
                                Vector3 newRot = anim.Rotation;
                                rot._x = newRot._x - rot._x;
                                rot._y = newRot._y - rot._y;
                                rot._z = newRot._z - rot._z;
                                entry.SetKeyframeOnlyRot(0, rot);
                            }
                        }
                        hasKeyframe = false;
                        if (SubtractTrans.Checked)
                        {
                            for (int x = 0; x < numFrames; x++)
                            {
                                for (int i = 0x16; i < 0x19; i++)
                                {
                                    if ((kfe = entry.GetKeyframe((KeyFrameMode)i, x)) != null)
                                    {
                                        switch (i)
                                        {
                                            case 0x16:
                                                kfe._value -= trans._x;
                                                break;
                                            case 0x17:
                                                kfe._value -= trans._y;
                                                break;
                                            case 0x18:
                                                kfe._value -= trans._z;
                                                break;
                                        }
                                        hasKeyframe = true;
                                    }
                                }
                            }
                            if (!hasKeyframe)
                            {
                                anim = entry.GetAnimFrame(0);
                                Vector3 newTrans = anim.Translation;
                                trans._x = newTrans._x - trans._x;
                                trans._y = newTrans._y - trans._y;
                                trans._z = newTrans._z - trans._z;
                                entry.SetKeyframeOnlyTrans(0, trans);
                            }
                        }
                    }
                    r._changed = true;
                }
                ChangeNode(_name, r, scale, rot, trans, _targetModel, model, disLoop);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e) { DialogResult = DialogResult.Cancel; Close(); }

        #region Designer

        private TextBox name;
        private Button btnCancel;
        private Label label1;
        private Button btnOkay;

        private void InitializeComponent()
        {
            this.name = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOkay = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.ScaleX = new System.Windows.Forms.TextBox();
            this.ScaleY = new System.Windows.Forms.TextBox();
            this.ScaleZ = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.RotX = new System.Windows.Forms.TextBox();
            this.RotY = new System.Windows.Forms.TextBox();
            this.RotZ = new System.Windows.Forms.TextBox();
            this.TransX = new System.Windows.Forms.TextBox();
            this.TransY = new System.Windows.Forms.TextBox();
            this.TransZ = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.ReplaceScale = new System.Windows.Forms.CheckBox();
            this.AddScale = new System.Windows.Forms.CheckBox();
            this.SubtractScale = new System.Windows.Forms.CheckBox();
            this.SubtractRot = new System.Windows.Forms.CheckBox();
            this.AddRot = new System.Windows.Forms.CheckBox();
            this.ReplaceRot = new System.Windows.Forms.CheckBox();
            this.SubtractTrans = new System.Windows.Forms.CheckBox();
            this.AddTrans = new System.Windows.Forms.CheckBox();
            this.ReplaceTrans = new System.Windows.Forms.CheckBox();
            this.copyKeyframes = new System.Windows.Forms.CheckBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.ChangeVersion = new System.Windows.Forms.CheckBox();
            this.Version = new System.Windows.Forms.ComboBox();
            this.Port = new System.Windows.Forms.CheckBox();
            this.noLoop = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // name
            // 
            this.name.HideSelection = false;
            this.name.Location = new System.Drawing.Point(12, 28);
            this.name.Name = "name";
            this.name.Size = new System.Drawing.Size(210, 20);
            this.name.TabIndex = 0;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(372, 231);
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
            this.btnOkay.Location = new System.Drawing.Point(291, 231);
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
            this.label1.Location = new System.Drawing.Point(13, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(190, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Change all bone entries with the name:";
            // 
            // ScaleX
            // 
            this.ScaleX.Enabled = false;
            this.ScaleX.Location = new System.Drawing.Point(82, 76);
            this.ScaleX.Name = "ScaleX";
            this.ScaleX.Size = new System.Drawing.Size(140, 20);
            this.ScaleX.TabIndex = 6;
            // 
            // ScaleY
            // 
            this.ScaleY.Enabled = false;
            this.ScaleY.Location = new System.Drawing.Point(82, 102);
            this.ScaleY.Name = "ScaleY";
            this.ScaleY.Size = new System.Drawing.Size(140, 20);
            this.ScaleY.TabIndex = 7;
            // 
            // ScaleZ
            // 
            this.ScaleZ.Enabled = false;
            this.ScaleZ.Location = new System.Drawing.Point(82, 128);
            this.ScaleZ.Name = "ScaleZ";
            this.ScaleZ.Size = new System.Drawing.Size(140, 20);
            this.ScaleZ.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 79);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Scale X:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 105);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Scale Y:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 131);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Scale Z:";
            // 
            // RotX
            // 
            this.RotX.Enabled = false;
            this.RotX.Location = new System.Drawing.Point(82, 178);
            this.RotX.Name = "RotX";
            this.RotX.Size = new System.Drawing.Size(140, 20);
            this.RotX.TabIndex = 12;
            // 
            // RotY
            // 
            this.RotY.Enabled = false;
            this.RotY.Location = new System.Drawing.Point(82, 205);
            this.RotY.Name = "RotY";
            this.RotY.Size = new System.Drawing.Size(140, 20);
            this.RotY.TabIndex = 13;
            // 
            // RotZ
            // 
            this.RotZ.Enabled = false;
            this.RotZ.Location = new System.Drawing.Point(82, 232);
            this.RotZ.Name = "RotZ";
            this.RotZ.Size = new System.Drawing.Size(140, 20);
            this.RotZ.TabIndex = 14;
            // 
            // TransX
            // 
            this.TransX.Enabled = false;
            this.TransX.Location = new System.Drawing.Point(306, 76);
            this.TransX.Name = "TransX";
            this.TransX.Size = new System.Drawing.Size(140, 20);
            this.TransX.TabIndex = 15;
            // 
            // TransY
            // 
            this.TransY.Enabled = false;
            this.TransY.Location = new System.Drawing.Point(306, 102);
            this.TransY.Name = "TransY";
            this.TransY.Size = new System.Drawing.Size(140, 20);
            this.TransY.TabIndex = 16;
            // 
            // TransZ
            // 
            this.TransZ.Enabled = false;
            this.TransZ.Location = new System.Drawing.Point(306, 128);
            this.TransZ.Name = "TransZ";
            this.TransZ.Size = new System.Drawing.Size(140, 20);
            this.TransZ.TabIndex = 17;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 181);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(52, 13);
            this.label6.TabIndex = 18;
            this.label6.Text = "Rotate X:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 208);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(52, 13);
            this.label7.TabIndex = 19;
            this.label7.Text = "Rotate Y:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 235);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(52, 13);
            this.label8.TabIndex = 20;
            this.label8.Text = "Rotate Z:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(236, 80);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(64, 13);
            this.label9.TabIndex = 21;
            this.label9.Text = "Translate X:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(236, 107);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(64, 13);
            this.label10.TabIndex = 22;
            this.label10.Text = "Translate Y:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(236, 134);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(64, 13);
            this.label11.TabIndex = 23;
            this.label11.Text = "Translate Z:";
            // 
            // ReplaceScale
            // 
            this.ReplaceScale.AutoSize = true;
            this.ReplaceScale.Location = new System.Drawing.Point(12, 54);
            this.ReplaceScale.Name = "ReplaceScale";
            this.ReplaceScale.Size = new System.Drawing.Size(66, 17);
            this.ReplaceScale.TabIndex = 24;
            this.ReplaceScale.Text = "Replace";
            this.ReplaceScale.UseVisualStyleBackColor = true;
            this.ReplaceScale.CheckedChanged += new System.EventHandler(this.ReplaceScale_CheckedChanged);
            // 
            // AddScale
            // 
            this.AddScale.AutoSize = true;
            this.AddScale.Location = new System.Drawing.Point(82, 54);
            this.AddScale.Name = "AddScale";
            this.AddScale.Size = new System.Drawing.Size(45, 17);
            this.AddScale.TabIndex = 25;
            this.AddScale.Text = "Add";
            this.AddScale.UseVisualStyleBackColor = true;
            this.AddScale.CheckedChanged += new System.EventHandler(this.AddScale_CheckedChanged);
            // 
            // SubtractScale
            // 
            this.SubtractScale.AutoSize = true;
            this.SubtractScale.Location = new System.Drawing.Point(133, 53);
            this.SubtractScale.Name = "SubtractScale";
            this.SubtractScale.Size = new System.Drawing.Size(66, 17);
            this.SubtractScale.TabIndex = 26;
            this.SubtractScale.Text = "Subtract";
            this.SubtractScale.UseVisualStyleBackColor = true;
            this.SubtractScale.CheckedChanged += new System.EventHandler(this.SubtractScale_CheckedChanged);
            // 
            // SubtractRot
            // 
            this.SubtractRot.AutoSize = true;
            this.SubtractRot.Location = new System.Drawing.Point(133, 155);
            this.SubtractRot.Name = "SubtractRot";
            this.SubtractRot.Size = new System.Drawing.Size(66, 17);
            this.SubtractRot.TabIndex = 29;
            this.SubtractRot.Text = "Subtract";
            this.SubtractRot.UseVisualStyleBackColor = true;
            this.SubtractRot.CheckedChanged += new System.EventHandler(this.SubtractRot_CheckedChanged);
            // 
            // AddRot
            // 
            this.AddRot.AutoSize = true;
            this.AddRot.Location = new System.Drawing.Point(82, 156);
            this.AddRot.Name = "AddRot";
            this.AddRot.Size = new System.Drawing.Size(45, 17);
            this.AddRot.TabIndex = 28;
            this.AddRot.Text = "Add";
            this.AddRot.UseVisualStyleBackColor = true;
            this.AddRot.CheckedChanged += new System.EventHandler(this.AddRot_CheckedChanged);
            // 
            // ReplaceRot
            // 
            this.ReplaceRot.AutoSize = true;
            this.ReplaceRot.Location = new System.Drawing.Point(12, 156);
            this.ReplaceRot.Name = "ReplaceRot";
            this.ReplaceRot.Size = new System.Drawing.Size(66, 17);
            this.ReplaceRot.TabIndex = 27;
            this.ReplaceRot.Text = "Replace";
            this.ReplaceRot.UseVisualStyleBackColor = true;
            this.ReplaceRot.CheckedChanged += new System.EventHandler(this.ReplaceRot_CheckedChanged);
            // 
            // SubtractTrans
            // 
            this.SubtractTrans.AutoSize = true;
            this.SubtractTrans.Location = new System.Drawing.Point(357, 53);
            this.SubtractTrans.Name = "SubtractTrans";
            this.SubtractTrans.Size = new System.Drawing.Size(66, 17);
            this.SubtractTrans.TabIndex = 32;
            this.SubtractTrans.Text = "Subtract";
            this.SubtractTrans.UseVisualStyleBackColor = true;
            this.SubtractTrans.CheckedChanged += new System.EventHandler(this.SubtractTrans_CheckedChanged);
            // 
            // AddTrans
            // 
            this.AddTrans.AutoSize = true;
            this.AddTrans.Location = new System.Drawing.Point(306, 54);
            this.AddTrans.Name = "AddTrans";
            this.AddTrans.Size = new System.Drawing.Size(45, 17);
            this.AddTrans.TabIndex = 31;
            this.AddTrans.Text = "Add";
            this.AddTrans.UseVisualStyleBackColor = true;
            this.AddTrans.CheckedChanged += new System.EventHandler(this.AddTrans_CheckedChanged);
            // 
            // ReplaceTrans
            // 
            this.ReplaceTrans.AutoSize = true;
            this.ReplaceTrans.Location = new System.Drawing.Point(236, 54);
            this.ReplaceTrans.Name = "ReplaceTrans";
            this.ReplaceTrans.Size = new System.Drawing.Size(66, 17);
            this.ReplaceTrans.TabIndex = 30;
            this.ReplaceTrans.Text = "Replace";
            this.ReplaceTrans.UseVisualStyleBackColor = true;
            this.ReplaceTrans.CheckedChanged += new System.EventHandler(this.ReplaceTrans_CheckedChanged);
            // 
            // copyKeyframes
            // 
            this.copyKeyframes.AutoSize = true;
            this.copyKeyframes.Location = new System.Drawing.Point(236, 9);
            this.copyKeyframes.Name = "copyKeyframes";
            this.copyKeyframes.Size = new System.Drawing.Size(127, 17);
            this.copyKeyframes.TabIndex = 33;
            this.copyKeyframes.Text = "Copy keyframes from:";
            this.copyKeyframes.UseVisualStyleBackColor = true;
            this.copyKeyframes.CheckedChanged += new System.EventHandler(this.copyKeyframes_CheckedChanged);
            // 
            // textBox1
            // 
            this.textBox1.Enabled = false;
            this.textBox1.Location = new System.Drawing.Point(236, 28);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(210, 20);
            this.textBox1.TabIndex = 34;
            // 
            // ChangeVersion
            // 
            this.ChangeVersion.AutoSize = true;
            this.ChangeVersion.Location = new System.Drawing.Point(236, 156);
            this.ChangeVersion.Name = "ChangeVersion";
            this.ChangeVersion.Size = new System.Drawing.Size(148, 17);
            this.ChangeVersion.TabIndex = 35;
            this.ChangeVersion.Text = "Change animation version";
            this.ChangeVersion.UseVisualStyleBackColor = true;
            this.ChangeVersion.CheckedChanged += new System.EventHandler(this.ChangeVersion_CheckedChanged);
            // 
            // Version
            // 
            this.Version.Enabled = false;
            this.Version.FormattingEnabled = true;
            this.Version.Location = new System.Drawing.Point(236, 178);
            this.Version.Name = "Version";
            this.Version.Size = new System.Drawing.Size(210, 21);
            this.Version.TabIndex = 36;
            // 
            // Port
            // 
            this.Port.AutoSize = true;
            this.Port.Location = new System.Drawing.Point(236, 207);
            this.Port.Name = "Port";
            this.Port.Size = new System.Drawing.Size(111, 17);
            this.Port.TabIndex = 37;
            this.Port.Text = "Port all animations";
            this.Port.UseVisualStyleBackColor = true;
            // 
            // noLoop
            // 
            this.noLoop.AutoSize = true;
            this.noLoop.Location = new System.Drawing.Point(362, 207);
            this.noLoop.Name = "noLoop";
            this.noLoop.Size = new System.Drawing.Size(84, 17);
            this.noLoop.TabIndex = 38;
            this.noLoop.Text = "Disable loop";
            this.noLoop.UseVisualStyleBackColor = true;
            // 
            // ChangeToDialog2
            // 
            this.AcceptButton = this.btnOkay;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(459, 266);
            this.Controls.Add(this.noLoop);
            this.Controls.Add(this.Port);
            this.Controls.Add(this.Version);
            this.Controls.Add(this.ChangeVersion);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.copyKeyframes);
            this.Controls.Add(this.SubtractTrans);
            this.Controls.Add(this.AddTrans);
            this.Controls.Add(this.ReplaceTrans);
            this.Controls.Add(this.SubtractRot);
            this.Controls.Add(this.AddRot);
            this.Controls.Add(this.ReplaceRot);
            this.Controls.Add(this.SubtractScale);
            this.Controls.Add(this.AddScale);
            this.Controls.Add(this.ReplaceScale);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.TransZ);
            this.Controls.Add(this.TransY);
            this.Controls.Add(this.TransX);
            this.Controls.Add(this.RotZ);
            this.Controls.Add(this.RotY);
            this.Controls.Add(this.RotX);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.ScaleZ);
            this.Controls.Add(this.ScaleY);
            this.Controls.Add(this.ScaleX);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnOkay);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.name);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ChangeToDialog2";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Edit All Bone Entries";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        #region Check Switches
        private void ReplaceScale_CheckedChanged(object sender, EventArgs e)
        {
            AddScale.Enabled = SubtractScale.Enabled = !ReplaceScale.Checked;
            ScaleX.Enabled = ScaleY.Enabled = ScaleZ.Enabled = ReplaceScale.Checked;
        }

        private void AddScale_CheckedChanged(object sender, EventArgs e)
        {
            ReplaceScale.Enabled = SubtractScale.Enabled = !AddScale.Checked;
            ScaleX.Enabled = ScaleY.Enabled = ScaleZ.Enabled = AddScale.Checked;
        }

        private void SubtractScale_CheckedChanged(object sender, EventArgs e)
        {
            AddScale.Enabled = ReplaceScale.Enabled = !SubtractScale.Checked;
            ScaleX.Enabled = ScaleY.Enabled = ScaleZ.Enabled = SubtractScale.Checked;
        }

        private void ReplaceRot_CheckedChanged(object sender, EventArgs e)
        {
            AddRot.Enabled = SubtractRot.Enabled = !ReplaceRot.Checked;
            RotX.Enabled = RotY.Enabled = RotZ.Enabled = ReplaceRot.Checked;
        }

        private void AddRot_CheckedChanged(object sender, EventArgs e)
        {
            ReplaceRot.Enabled = SubtractRot.Enabled = !AddRot.Checked;
            RotX.Enabled = RotY.Enabled = RotZ.Enabled = AddRot.Checked;
        }

        private void SubtractRot_CheckedChanged(object sender, EventArgs e)
        {
            AddRot.Enabled = ReplaceRot.Enabled = !SubtractRot.Checked;
            RotX.Enabled = RotY.Enabled = RotZ.Enabled = SubtractRot.Checked;
        }

        private void ReplaceTrans_CheckedChanged(object sender, EventArgs e)
        {
            AddTrans.Enabled = SubtractTrans.Enabled = !ReplaceTrans.Checked;
            TransX.Enabled = TransY.Enabled = TransZ.Enabled = ReplaceTrans.Checked;
        }

        private void AddTrans_CheckedChanged(object sender, EventArgs e)
        {
            ReplaceTrans.Enabled = SubtractTrans.Enabled = !AddTrans.Checked;
            TransX.Enabled = TransY.Enabled = TransZ.Enabled = AddTrans.Checked;
        }

        private void SubtractTrans_CheckedChanged(object sender, EventArgs e)
        {
            AddTrans.Enabled = ReplaceTrans.Enabled = !SubtractTrans.Checked;
            TransX.Enabled = TransY.Enabled = TransZ.Enabled = SubtractTrans.Checked;
        }
        #endregion

        private void copyKeyframes_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Enabled = copyKeyframes.Checked;
        }

        private void ChangeVersion_CheckedChanged(object sender, EventArgs e)
        {
            Version.Enabled = ChangeVersion.Checked;
        }
    }
}
