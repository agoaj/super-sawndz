using System;
using BrawlLib.SSBB.ResourceNodes;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;

namespace System.Windows.Forms
{
    public class EventModifier : UserControl
    {
        private ComboBox cboType;
        private ListBox lstParameters;
        private Button btnChangeEvent;
        private Label lblEventId;
        private Label lblEventName;
        private Label lblParamDescription;
        private Button btnCancel;
        private Button btnDone;
        private Label lblName2;
        private PropertyGrid valueGrid;
        private Panel requirementPanel;
        private Panel mainPanel;
        private SplitContainer splitContainer1;
        private Panel typePanel;
        private CheckBox chkNot;
        private Label label1;
        private ComboBox cboRequirement;
        private Panel offsetPanel;
        private Button offsetOkay;
        private ComboBox comboBox1;
        private ComboBox comboBox2;
        private ComboBox comboBox3;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label lblName1;

        #region Designer

        private void InitializeComponent()
        {
            this.cboType = new System.Windows.Forms.ComboBox();
            this.lstParameters = new System.Windows.Forms.ListBox();
            this.btnChangeEvent = new System.Windows.Forms.Button();
            this.lblEventId = new System.Windows.Forms.Label();
            this.lblEventName = new System.Windows.Forms.Label();
            this.lblParamDescription = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnDone = new System.Windows.Forms.Button();
            this.lblName2 = new System.Windows.Forms.Label();
            this.lblName1 = new System.Windows.Forms.Label();
            this.valueGrid = new System.Windows.Forms.PropertyGrid();
            this.requirementPanel = new System.Windows.Forms.Panel();
            this.chkNot = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cboRequirement = new System.Windows.Forms.ComboBox();
            this.offsetPanel = new System.Windows.Forms.Panel();
            this.offsetOkay = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.comboBox3 = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.mainPanel = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.typePanel = new System.Windows.Forms.Panel();
            this.requirementPanel.SuspendLayout();
            this.offsetPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.typePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // cboType
            // 
            this.cboType.FormattingEnabled = true;
            this.cboType.Items.AddRange(new object[] {
            "Value",
            "Scalar",
            "Pointer",
            "Boolean",
            "Unknown",
            "Variable",
            "Requirement"});
            this.cboType.Location = new System.Drawing.Point(46, 0);
            this.cboType.Name = "cboType";
            this.cboType.Size = new System.Drawing.Size(82, 21);
            this.cboType.TabIndex = 63;
            this.cboType.SelectedIndexChanged += new System.EventHandler(this.cboType_SelectedIndexChanged);
            // 
            // lstParameters
            // 
            this.lstParameters.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstParameters.FormattingEnabled = true;
            this.lstParameters.Location = new System.Drawing.Point(0, 21);
            this.lstParameters.Name = "lstParameters";
            this.lstParameters.Size = new System.Drawing.Size(93, 92);
            this.lstParameters.TabIndex = 62;
            this.lstParameters.SelectedIndexChanged += new System.EventHandler(this.lstParameters_SelectedIndexChanged);
            // 
            // btnChangeEvent
            // 
            this.btnChangeEvent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnChangeEvent.Location = new System.Drawing.Point(171, 2);
            this.btnChangeEvent.Name = "btnChangeEvent";
            this.btnChangeEvent.Size = new System.Drawing.Size(56, 23);
            this.btnChangeEvent.TabIndex = 61;
            this.btnChangeEvent.Text = "Change";
            this.btnChangeEvent.UseVisualStyleBackColor = true;
            this.btnChangeEvent.Click += new System.EventHandler(this.btnChangeEvent_Click);
            // 
            // lblEventId
            // 
            this.lblEventId.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblEventId.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblEventId.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblEventId.Location = new System.Drawing.Point(107, 3);
            this.lblEventId.Name = "lblEventId";
            this.lblEventId.Size = new System.Drawing.Size(66, 20);
            this.lblEventId.TabIndex = 60;
            this.lblEventId.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblEventName
            // 
            this.lblEventName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblEventName.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblEventName.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblEventName.Location = new System.Drawing.Point(2, 3);
            this.lblEventName.Name = "lblEventName";
            this.lblEventName.Size = new System.Drawing.Size(105, 20);
            this.lblEventName.TabIndex = 59;
            this.lblEventName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblParamDescription
            // 
            this.lblParamDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblParamDescription.BackColor = System.Drawing.SystemColors.Control;
            this.lblParamDescription.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblParamDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblParamDescription.Location = new System.Drawing.Point(2, 140);
            this.lblParamDescription.Name = "lblParamDescription";
            this.lblParamDescription.Size = new System.Drawing.Size(225, 63);
            this.lblParamDescription.TabIndex = 58;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(169, 206);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(58, 24);
            this.btnCancel.TabIndex = 57;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnDone
            // 
            this.btnDone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDone.Location = new System.Drawing.Point(105, 206);
            this.btnDone.Name = "btnDone";
            this.btnDone.Size = new System.Drawing.Size(58, 24);
            this.btnDone.TabIndex = 56;
            this.btnDone.Text = "Done";
            this.btnDone.UseVisualStyleBackColor = true;
            this.btnDone.Click += new System.EventHandler(this.btnDone_Click);
            // 
            // lblName2
            // 
            this.lblName2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblName2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblName2.Location = new System.Drawing.Point(0, 0);
            this.lblName2.Name = "lblName2";
            this.lblName2.Size = new System.Drawing.Size(45, 21);
            this.lblName2.TabIndex = 55;
            this.lblName2.Text = "Type:";
            this.lblName2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblName1
            // 
            this.lblName1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblName1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblName1.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblName1.Location = new System.Drawing.Point(0, 0);
            this.lblName1.Name = "lblName1";
            this.lblName1.Size = new System.Drawing.Size(93, 21);
            this.lblName1.TabIndex = 54;
            this.lblName1.Text = "Parameter:";
            this.lblName1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // valueGrid
            // 
            this.valueGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.valueGrid.HelpVisible = false;
            this.valueGrid.Location = new System.Drawing.Point(0, 21);
            this.valueGrid.Name = "valueGrid";
            this.valueGrid.Size = new System.Drawing.Size(128, 92);
            this.valueGrid.TabIndex = 8;
            this.valueGrid.ToolbarVisible = false;
            // 
            // requirementPanel
            // 
            this.requirementPanel.Controls.Add(this.chkNot);
            this.requirementPanel.Controls.Add(this.label1);
            this.requirementPanel.Controls.Add(this.cboRequirement);
            this.requirementPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.requirementPanel.Location = new System.Drawing.Point(0, 21);
            this.requirementPanel.Name = "requirementPanel";
            this.requirementPanel.Size = new System.Drawing.Size(128, 92);
            this.requirementPanel.TabIndex = 64;
            // 
            // chkNot
            // 
            this.chkNot.AutoSize = true;
            this.chkNot.Location = new System.Drawing.Point(81, 3);
            this.chkNot.Name = "chkNot";
            this.chkNot.Size = new System.Drawing.Size(43, 17);
            this.chkNot.TabIndex = 65;
            this.chkNot.Text = "Not";
            this.chkNot.UseVisualStyleBackColor = true;
            this.chkNot.CheckedChanged += new System.EventHandler(this.Requirement_Handle);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 21);
            this.label1.TabIndex = 64;
            this.label1.Text = "Requirement:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cboRequirement
            // 
            this.cboRequirement.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cboRequirement.FormattingEnabled = true;
            this.cboRequirement.Location = new System.Drawing.Point(0, 22);
            this.cboRequirement.Name = "cboRequirement";
            this.cboRequirement.Size = new System.Drawing.Size(128, 21);
            this.cboRequirement.TabIndex = 0;
            this.cboRequirement.SelectedIndexChanged += new System.EventHandler(this.Requirement_Handle);
            // 
            // offsetPanel
            // 
            this.offsetPanel.Controls.Add(this.offsetOkay);
            this.offsetPanel.Controls.Add(this.comboBox1);
            this.offsetPanel.Controls.Add(this.comboBox2);
            this.offsetPanel.Controls.Add(this.comboBox3);
            this.offsetPanel.Controls.Add(this.label2);
            this.offsetPanel.Controls.Add(this.label3);
            this.offsetPanel.Controls.Add(this.label4);
            this.offsetPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.offsetPanel.Location = new System.Drawing.Point(0, 21);
            this.offsetPanel.Name = "offsetPanel";
            this.offsetPanel.Size = new System.Drawing.Size(128, 92);
            this.offsetPanel.TabIndex = 66;
            // 
            // offsetOkay
            // 
            this.offsetOkay.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.offsetOkay.Location = new System.Drawing.Point(-1, 69);
            this.offsetOkay.Name = "offsetOkay";
            this.offsetOkay.Size = new System.Drawing.Size(129, 23);
            this.offsetOkay.TabIndex = 13;
            this.offsetOkay.Text = "Okay";
            this.offsetOkay.UseVisualStyleBackColor = true;
            this.offsetOkay.Click += new System.EventHandler(this.offsetOkay_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Actions",
            "SubActions",
            "SubRoutines",
            "External",
            "Null"});
            this.comboBox1.Location = new System.Drawing.Point(46, 3);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(82, 21);
            this.comboBox1.TabIndex = 7;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // comboBox2
            // 
            this.comboBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(46, 24);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(82, 21);
            this.comboBox2.TabIndex = 9;
            // 
            // comboBox3
            // 
            this.comboBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox3.FormattingEnabled = true;
            this.comboBox3.Location = new System.Drawing.Point(46, 45);
            this.comboBox3.Name = "comboBox3";
            this.comboBox3.Size = new System.Drawing.Size(82, 21);
            this.comboBox3.TabIndex = 11;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label2.Location = new System.Drawing.Point(0, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 21);
            this.label2.TabIndex = 8;
            this.label2.Text = "List:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label3.Location = new System.Drawing.Point(0, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 21);
            this.label3.TabIndex = 10;
            this.label3.Text = "Action:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label4.Location = new System.Drawing.Point(0, 45);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(45, 21);
            this.label4.TabIndex = 12;
            this.label4.Text = "Type:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // mainPanel
            // 
            this.mainPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.mainPanel.Location = new System.Drawing.Point(2, 25);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(225, 113);
            this.mainPanel.TabIndex = 0;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(2, 25);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.lstParameters);
            this.splitContainer1.Panel1.Controls.Add(this.lblName1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.offsetPanel);
            this.splitContainer1.Panel2.Controls.Add(this.valueGrid);
            this.splitContainer1.Panel2.Controls.Add(this.requirementPanel);
            this.splitContainer1.Panel2.Controls.Add(this.typePanel);
            this.splitContainer1.Size = new System.Drawing.Size(225, 113);
            this.splitContainer1.SplitterDistance = 93;
            this.splitContainer1.TabIndex = 9;
            // 
            // typePanel
            // 
            this.typePanel.Controls.Add(this.lblName2);
            this.typePanel.Controls.Add(this.cboType);
            this.typePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.typePanel.Location = new System.Drawing.Point(0, 0);
            this.typePanel.Name = "typePanel";
            this.typePanel.Size = new System.Drawing.Size(128, 21);
            this.typePanel.TabIndex = 64;
            // 
            // EventModifier
            // 
            this.AutoSize = true;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.mainPanel);
            this.Controls.Add(this.btnChangeEvent);
            this.Controls.Add(this.lblEventId);
            this.Controls.Add(this.lblEventName);
            this.Controls.Add(this.lblParamDescription);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnDone);
            this.Name = "EventModifier";
            this.Size = new System.Drawing.Size(230, 233);
            this.requirementPanel.ResumeLayout(false);
            this.requirementPanel.PerformLayout();
            this.offsetPanel.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.typePanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public EventModifier() { InitializeComponent(); frmEventList = new FormEventList(); }

        public DialogResult status;
        public MoveDefEventNode origEvent;
        public ModelMovesetPanel p;

        MoveDefEventNode eventData { get { return newEvent; } }
        MoveDefEventNode newEv = null;
        MoveDefEventNode newEvent 
        {
            get 
            {
                if (newEv == null)
                {
                    newEv = new MoveDefEventNode() { _parent = origEvent.Parent };

                    newEv.EventID = origEvent._event;
                    ActionEventInfo info = origEvent.EventInfo;

                    for (int i = 0; i < newEv.numArguments; i++)
                    {
                        long type = (long)(origEvent.Children[i] as MoveDefEventParameterNode)._type;
                        if (i == 12 && (newEv._event == 0x06000D00 || newEv._event == 0x06150F00 || newEv._event == 0x062B0D00))
                        {
                            HitboxFlagsNode h = new HitboxFlagsNode(info != null ? info.Params[i] : "Value") { _value = (origEvent.Children[i] as MoveDefEventParameterNode)._value };
                            h.val.data = h._value;
                            h.GetFlags();
                            newEv.AddChild(h);
                        }
                        else if (((newEv._event == 0x06000D00 || newEv._event == 0x06150F00 || newEv._event == 0x062B0D00) && (i == 0 || i == 3 || i == 4)) ||
                            ((newEv._event == 0x11001000 || newEv._event == 0x11010A00 || newEv._event == 0x11020A00) && i == 0))
                            newEv.AddChild(new MoveDefEventValue2HalfNode(info != null ? info.Params[i] : "Value") { _value = (origEvent.Children[i] as MoveDefEventParameterNode)._value });
                        else if (i == 14 && newEv._event == 0x06150F00)
                        {
                            SpecialHitboxFlagsNode h = new SpecialHitboxFlagsNode(info != null ? info.Params[i] : "Value") { _value = (origEvent.Children[i] as MoveDefEventParameterNode)._value };
                            h.val.data = h._value;
                            h.GetFlags();
                            newEv.AddChild(h);
                        }
                        else if ((ArgVarType)(int)type == ArgVarType.Value)
                            newEv.AddChild(new MoveDefEventValueNode(info != null ? info.Params[i] : "Value") { _value = (origEvent.Children[i] as MoveDefEventParameterNode)._value });
                        else if ((ArgVarType)(int)type == ArgVarType.Scalar)
                            newEv.AddChild(new MoveDefEventScalarNode(info != null ? info.Params[i] : "Scalar") { _value = (origEvent.Children[i] as MoveDefEventParameterNode)._value });
                        else if ((ArgVarType)(int)type == ArgVarType.Boolean)
                            newEv.AddChild(new MoveDefEventBoolNode(info != null ? info.Params[i] : "Boolean") { _value = (origEvent.Children[i] as MoveDefEventParameterNode)._value });
                        else if ((ArgVarType)(int)type == ArgVarType.Unknown)
                            newEv.AddChild(new MoveDefEventUnkNode(info != null ? info.Params[i] : "Unknown") { _value = (origEvent.Children[i] as MoveDefEventParameterNode)._value });
                        else if ((ArgVarType)(int)type == ArgVarType.Requirement)
                        {
                            MoveDefEventRequirementNode r = new MoveDefEventRequirementNode(info != null ? info.Params[i] : "Requirement") { _value = (origEvent.Children[i] as MoveDefEventParameterNode)._value };
                            newEv.AddChild(r);
                            r.val = r.GetRequirement(r._value);
                        }
                        else if ((ArgVarType)(int)type == ArgVarType.Variable)
                        {
                            MoveDefEventVariableNode v = new MoveDefEventVariableNode(info != null ? info.Params[i] : "Variable") { _value = (origEvent.Children[i] as MoveDefEventParameterNode)._value };
                            newEv.AddChild(v);
                            v.val = v.ResolveVariable(v._value);
                        }
                        else if ((ArgVarType)(int)type == ArgVarType.Offset)
                            newEv.AddChild(new MoveDefEventOffsetNode(info != null ? info.Params[i] : "Offset") { _value = (origEvent.Children[i] as MoveDefEventParameterNode)._value });
                    }
                }
                return newEv;
            }
        }

        public MoveDefEventParameterNode param = null;

        public void Setup(ModelMovesetPanel parent)
        {
            p = parent;

            //Setup requirements list.
            if (cboRequirement.Items.Count == 0)
                cboRequirement.Items.AddRange(eventData.Root.iRequirements);

            status = DialogResult.Cancel;
            newEv = null;

            DisplayEvent();
        }

        //Display the event's name, offset and parameters.
        public void DisplayEvent()
        {
            lstParameters.Items.Clear();
            cboType.SelectedIndex = -1;
            cboType.Text = "";
            cboType.Enabled = false;
            lblParamDescription.Text = "No Description Available.";

            valueGrid.Visible = false;
            requirementPanel.Visible = false;
            offsetPanel.Visible = false;

            ActionEventInfo info = null;
            if (eventData.Root.EventDictionary.ContainsKey(eventData._event))
                info = eventData.Root.EventDictionary[eventData._event];

            if (info != null)
                lblEventName.Text = info._name;

            lblEventId.Text = MParams.Hex8(eventData._event);

            foreach (MoveDefEventParameterNode n in eventData.Children)
                if (!String.IsNullOrEmpty(n.Name))
                    lstParameters.Items.Add(n.Name);
        }

        //Display the selected parameter's value, type and description.
        private void DisplayParameter(int index)
        {
            param = eventData.Children[index] as MoveDefEventParameterNode;

            cboType.Enabled = true;
            try { cboType.SelectedIndex = (int)param._type; }
            catch { cboType.SelectedIndex = -1; cboType.Text = "(" + MParams.Hex((int)param._type) + ")"; }
            DisplayInType(param);

            lblParamDescription.Text = param.Description;
        }

        //Display the parameter's value according to its type.
        public void DisplayInType(MoveDefEventParameterNode value)
        {
            if (value is MoveDefEventOffsetNode)
            {
                requirementPanel.Visible = false;
                valueGrid.Visible = false;
                offsetPanel.Visible = true;

                MoveDefEventOffsetNode offset = value as MoveDefEventOffsetNode;

                int list, type, index;
                offset.Root.GetLocation(offset.RawOffset, out list, out type, out index);

                _updating = true;
                comboBox1.SelectedIndex = offset.list = list;
                if (offset.type != -1)
                    comboBox3.SelectedIndex = offset.type = type;
                if (offset.index != -1)
                    comboBox2.SelectedIndex = offset.index = index;
                _updating = false;
            }
            else 
            {
                requirementPanel.Visible = false;
                valueGrid.Visible = true;
                offsetPanel.Visible = false;

                valueGrid.SelectedObject = value;
            }
        }

        public bool _updating = false;

        public FormEventList frmEventList;
        private void btnChangeEvent_Click(object sender, EventArgs e)
        {
            //Pass in the event Event.
            frmEventList.eventEvent = eventData._event;
            frmEventList.p = eventData.Root;
            frmEventList.ShowDialog();

            //Retrieve and setup the new event according to the new event Event.
            if (frmEventList.status == DialogResult.OK)
            {
                newEv = new MoveDefEventNode() { _parent = origEvent.Parent };

                newEvent.EventID = (int)frmEventList.eventEvent;
                ActionEventInfo info = newEvent.EventInfo;

                if (info.Params == null)
                {
                    DisplayEvent();
                    return;
                }

                for (int i = 0; i < newEvent.numArguments; i++)
                {
                    long type = info.GetDfltParameter(i);
                    if (i >= info.Params.Length)
                        continue;

                    if ((newEvent._event == 0x06000D00 || newEvent._event == 0x06150F00 || newEvent._event == 0x062B0D00) && i == 12)
                        newEvent.AddChild(new HitboxFlagsNode(info != null ? info.Params[i] : "Value"));
                    else if (((newEvent._event == 0x06000D00 || newEvent._event == 0x06150F00 || newEvent._event == 0x062B0D00) && (i == 0 || i == 3 || i == 4)) ||
                        ((newEvent._event == 0x11001000 || newEvent._event == 0x11010A00 || newEvent._event == 0x11020A00) && i == 0))
                        newEvent.AddChild(new MoveDefEventValue2HalfNode(info != null ? info.Params[i] : "Value"));
                    else if (i == 14 && newEvent._event == 0x06150F00)
                        newEvent.AddChild(new SpecialHitboxFlagsNode(info != null ? info.Params[i] : "Value"));
                    else if ((ArgVarType)(int)type == ArgVarType.Value)
                        newEvent.AddChild(new MoveDefEventValueNode(info != null ? info.Params[i] : "Value"));
                    else if ((ArgVarType)(int)type == ArgVarType.Scalar)
                        newEvent.AddChild(new MoveDefEventScalarNode(info != null ? info.Params[i] : "Scalar"));
                    else if ((ArgVarType)(int)type == ArgVarType.Boolean)
                        newEvent.AddChild(new MoveDefEventBoolNode(info != null ? info.Params[i] : "Boolean"));
                    else if ((ArgVarType)(int)type == ArgVarType.Unknown)
                        newEvent.AddChild(new MoveDefEventUnkNode(info != null ? info.Params[i] : "Unknown"));
                    else if ((ArgVarType)(int)type == ArgVarType.Requirement)
                        newEvent.AddChild(new MoveDefEventRequirementNode(info != null ? info.Params[i] : "Requirement"));
                    else if ((ArgVarType)(int)type == ArgVarType.Variable)
                        newEvent.AddChild(new MoveDefEventVariableNode(info != null ? info.Params[i] : "Variable"));
                    else if ((ArgVarType)(int)type == ArgVarType.Offset)
                        newEvent.AddChild(new MoveDefEventOffsetNode(info != null ? info.Params[i] : "Offset"));
                }
            }

            DisplayEvent();
        }

        private void lstParameters_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstParameters.SelectedIndex == -1) return;
            int index = lstParameters.SelectedIndex;
            DisplayParameter(index);
        }

        private void cboType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboType.SelectedIndex == -1) return;
            if (lstParameters.SelectedIndex == -1) return;
            int index = lstParameters.SelectedIndex;

            //Change the type to the type selected and update the view window.

            param = eventData.Children[index] as MoveDefEventParameterNode;

            if (param._type != (ArgVarType)cboType.SelectedIndex)
            {
                int ind = param.Index;
                ActionEventInfo info = eventData.EventInfo;
                string name = ((ArgVarType)cboType.SelectedIndex).ToString();
                if (info != null)
                    name = info.Params[ind];

                //int value = 0;

                //MoveDefEventParameterNode p = newEvent.Children[ind] as MoveDefEventParameterNode;
                //if (p is MoveDefEventValueNode || p is MoveDefEventScalarNode || p is MoveDefEventBoolNode)
                //    value = p._value;

                newEvent.Children[ind].Remove();

                ArgVarType t = ((ArgVarType)cboType.SelectedIndex);

                if ((newEvent._event == 0x06000D00 || newEvent._event == 0x06150F00 || newEvent._event == 0x062B0D00) && ind == 12)
                    newEvent.InsertChild(new HitboxFlagsNode(name), true, ind);
                else if (((newEvent._event == 0x06000D00 || newEvent._event == 0x06150F00 || newEvent._event == 0x062B0D00) && (ind == 0 || ind == 3 || ind == 4)) ||
                    ((newEvent._event == 0x11010A00 || newEvent._event == 0x11020A00) && ind == 0))
                    newEvent.InsertChild(new MoveDefEventValue2HalfNode(name), true, ind);
                else if (ind == 14 && newEvent._event == 0x06150F00)
                    newEvent.InsertChild(new SpecialHitboxFlagsNode(name), true, ind);
                else if (t == ArgVarType.Value)
                    newEvent.InsertChild(new MoveDefEventValueNode(name), true, ind);
                else if (t == ArgVarType.Scalar)
                    newEvent.InsertChild(new MoveDefEventScalarNode(name), true, ind);
                else if (t == ArgVarType.Boolean)
                    newEvent.InsertChild(new MoveDefEventBoolNode(name), true, ind);
                else if (t == ArgVarType.Unknown)
                    newEvent.InsertChild(new MoveDefEventUnkNode(name), true, ind);
                else if (t == ArgVarType.Requirement)
                    newEvent.InsertChild(new MoveDefEventRequirementNode(name), true, ind);
                else if (t == ArgVarType.Variable)
                    newEvent.InsertChild(new MoveDefEventVariableNode(name), true, ind);
                else if (t == ArgVarType.Offset)
                    newEvent.InsertChild(new MoveDefEventOffsetNode(name), true, ind);
            }

            DisplayParameter(index);
        }

        private void Requirement_Handle(object sender, EventArgs e)
        {
            if (cboRequirement.SelectedIndex == -1) return;
            if (lstParameters.SelectedIndex == -1) return;
            int index = lstParameters.SelectedIndex;
            long value = cboRequirement.SelectedIndex;
            if (chkNot.Checked) value |= 0x80000000;

            (newEvent.Children[index] as MoveDefEventParameterNode)._value = (int)value;
        }

        public event EventHandler Completed;

        private void btnCancel_Click(object sender, EventArgs e)
        {
            status = DialogResult.Cancel;

            if (p != null)
                p.SelectedObject = _oldSelectedObject;
            else
            if (Completed != null)
                Completed(this, null);
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            if (newEv == null) //No changes were made.
            {
                btnCancel_Click(sender, e);
                return;
            }

            status = DialogResult.OK;
            int index = origEvent.Index;
            MoveDefActionNode action = origEvent.Parent as MoveDefActionNode;
            action.InsertChild(newEvent, true, index);
            origEvent.Remove();

            if (p != null)
            {
                p.SelectedObject = _oldSelectedObject;
                p.scriptEditor1.MakeScript();
            }
            else
            if (Completed != null)
                Completed(this, null);
        }

        public object _oldSelectedObject;

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                comboBox3.Items.Clear();
                comboBox3.Items.Add("Entry");
                comboBox3.Items.Add("Exit");

                comboBox2.Items.Clear();
                comboBox2.Items.AddRange(param.Root._actions.Children.ToArray());
            }
            if (comboBox1.SelectedIndex == 1)
            {
                comboBox3.Items.Clear();
                comboBox3.Items.Add("Main");
                comboBox3.Items.Add("GFX");
                comboBox3.Items.Add("SFX");
                comboBox3.Items.Add("Other");

                comboBox2.Items.Clear();
                comboBox2.Items.AddRange(param.Root._subActions.Children.ToArray());
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
                comboBox2.Items.AddRange(param.Root._subRoutineList.ToArray());
            }
            if (comboBox1.SelectedIndex == 3)
            {
                comboBox2.Items.Clear();
                comboBox2.Items.AddRange(param.Root._external.ToArray());
            }
        }

        private void offsetOkay_Click(object sender, EventArgs e)
        {
            int offset = -1;
            if (comboBox1.SelectedIndex >= 3)
                offset = -1;
            else
                offset = param.Root.GetOffset(comboBox1.SelectedIndex, (comboBox1.SelectedIndex >= 2 ? -1 : comboBox3.SelectedIndex), comboBox2.SelectedIndex);
            param._value = offset;
            (param as MoveDefEventOffsetNode).action = param.Root.GetAction(offset);
            (param as MoveDefEventOffsetNode).list = comboBox1.SelectedIndex;
            (param as MoveDefEventOffsetNode).type = (comboBox1.SelectedIndex >= 2 ? -1 : comboBox3.SelectedIndex);
            (param as MoveDefEventOffsetNode).index = (comboBox1.SelectedIndex == 4 ? -1 : comboBox2.SelectedIndex);
            param.SignalPropertyChange();
        }
    }
}
