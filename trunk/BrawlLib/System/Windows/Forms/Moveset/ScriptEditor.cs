using System;
using BrawlLib.SSBB.ResourceNodes;
using System.ComponentModel;
using System.Drawing;
using System.Collections.Generic;
using System.IO;

namespace System.Windows.Forms
{
    public class ScriptEditor : UserControl
    {
        #region Designer
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ScriptEditor));
            this.panel1 = new System.Windows.Forms.Panel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnAdd = new System.Windows.Forms.ToolStripButton();
            this.btnRemove = new System.Windows.Forms.ToolStripButton();
            this.btnModify = new System.Windows.Forms.ToolStripButton();
            this.btnUp = new System.Windows.Forms.ToolStripButton();
            this.btnDown = new System.Windows.Forms.ToolStripButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.btnCopy = new System.Windows.Forms.ToolStripButton();
            this.btnCut = new System.Windows.Forms.ToolStripButton();
            this.btnPaste = new System.Windows.Forms.ToolStripButton();
            this.btnCopyText = new System.Windows.Forms.ToolStripButton();
            this.EventList = new System.Windows.Forms.ListBox();
            this.description = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.toolStrip1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(1, 190);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(324, 30);
            this.panel1.TabIndex = 2;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnAdd,
            this.btnRemove,
            this.btnModify,
            this.btnUp,
            this.btnDown});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(324, 30);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnAdd
            // 
            this.btnAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnAdd.Image = ((System.Drawing.Image)(resources.GetObject("btnAdd.Image")));
            this.btnAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(33, 27);
            this.btnAdd.Text = "Add";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnRemove.Image = ((System.Drawing.Image)(resources.GetObject("btnRemove.Image")));
            this.btnRemove.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(54, 27);
            this.btnRemove.Text = "Remove";
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnModify
            // 
            this.btnModify.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnModify.Image = ((System.Drawing.Image)(resources.GetObject("btnModify.Image")));
            this.btnModify.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnModify.Name = "btnModify";
            this.btnModify.Size = new System.Drawing.Size(49, 27);
            this.btnModify.Text = "Modify";
            this.btnModify.Click += new System.EventHandler(this.btnModify_Click);
            // 
            // btnUp
            // 
            this.btnUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnUp.Image = ((System.Drawing.Image)(resources.GetObject("btnUp.Image")));
            this.btnUp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(23, 27);
            this.btnUp.Text = "▲";
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // btnDown
            // 
            this.btnDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnDown.Image = ((System.Drawing.Image)(resources.GetObject("btnDown.Image")));
            this.btnDown.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(23, 27);
            this.btnDown.Text = "▼";
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.toolStrip2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(1, 1);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(324, 23);
            this.panel2.TabIndex = 15;
            // 
            // toolStrip2
            // 
            this.toolStrip2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStrip2.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnCopy,
            this.btnCut,
            this.btnPaste,
            this.btnCopyText});
            this.toolStrip2.Location = new System.Drawing.Point(0, 0);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(324, 23);
            this.toolStrip2.TabIndex = 1;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // btnCopy
            // 
            this.btnCopy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnCopy.Image = ((System.Drawing.Image)(resources.GetObject("btnCopy.Image")));
            this.btnCopy.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(39, 20);
            this.btnCopy.Text = "Copy";
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // btnCut
            // 
            this.btnCut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnCut.Image = ((System.Drawing.Image)(resources.GetObject("btnCut.Image")));
            this.btnCut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCut.Name = "btnCut";
            this.btnCut.Size = new System.Drawing.Size(30, 20);
            this.btnCut.Text = "Cut";
            this.btnCut.Click += new System.EventHandler(this.btnCut_Click);
            // 
            // btnPaste
            // 
            this.btnPaste.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnPaste.Image = ((System.Drawing.Image)(resources.GetObject("btnPaste.Image")));
            this.btnPaste.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPaste.Name = "btnPaste";
            this.btnPaste.Size = new System.Drawing.Size(39, 20);
            this.btnPaste.Text = "Paste";
            this.btnPaste.Click += new System.EventHandler(this.btnPaste_Click);
            // 
            // btnCopyText
            // 
            this.btnCopyText.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnCopyText.Image = ((System.Drawing.Image)(resources.GetObject("btnCopyText.Image")));
            this.btnCopyText.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCopyText.Name = "btnCopyText";
            this.btnCopyText.Size = new System.Drawing.Size(64, 20);
            this.btnCopyText.Text = "Copy Text";
            this.btnCopyText.Click += new System.EventHandler(this.btnCopyText_Click);
            // 
            // EventList
            // 
            this.EventList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EventList.FormattingEnabled = true;
            this.EventList.HorizontalScrollbar = true;
            this.EventList.Location = new System.Drawing.Point(1, 24);
            this.EventList.Name = "EventList";
            this.EventList.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.EventList.Size = new System.Drawing.Size(324, 166);
            this.EventList.TabIndex = 16;
            this.EventList.SelectedIndexChanged += new System.EventHandler(this.EventList_SelectedIndexChanged);
            this.EventList.DoubleClick += new System.EventHandler(this.EventList_DoubleClick);
            // 
            // description
            // 
            this.description.BackColor = System.Drawing.SystemColors.Control;
            this.description.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.description.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.description.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.description.Location = new System.Drawing.Point(1, 220);
            this.description.Name = "description";
            this.description.Size = new System.Drawing.Size(324, 63);
            this.description.TabIndex = 59;
            // 
            // ScriptEditor
            // 
            this.Controls.Add(this.EventList);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.description);
            this.Name = "ScriptEditor";
            this.Padding = new System.Windows.Forms.Padding(1);
            this.Size = new System.Drawing.Size(326, 284);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Panel panel1;

        public ModelMovesetPanel form;

        private MoveDefNode _mDef;
        private Panel panel2;
        public ListBox EventList;

        public string[] iRequirements = new string[0];
        public string[] iAirGroundStats = new string[0];
        public string[] iCollisionStats = new string[0];
        
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MoveDefNode MoveDef
        {
            get { return _mDef; }
            set { _mDef = value; }
        }

        private MoveDefActionNode _targetNode;
        private Label description;
        private ToolStrip toolStrip1;
        private ToolStripButton btnDown;
        private ToolStripButton btnAdd;
        private ToolStripButton btnRemove;
        private ToolStripButton btnModify;
        private ToolStripButton btnUp;
        private ToolStrip toolStrip2;
        private ToolStripButton btnCopy;
        private ToolStripButton btnCut;
        private ToolStripButton btnPaste;
        private ToolStripButton btnCopyText;
    
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MoveDefActionNode TargetNode
        {
            get { return _targetNode; }
            set { _targetNode = value; TargetChanged(); }
        }

        public bool called = false;
        public ScriptEditor() { InitializeComponent(); }
        public ScriptEditor(ModelMovesetPanel owner)
        {
            InitializeComponent();
            form = owner; 
        }

        private void TargetChanged()
        {
            if (TargetNode != null)
            {
                if (!called)
                {
                    called = true;
                    iRequirements = _targetNode.Root.iRequirements;
                    iAirGroundStats = _targetNode.Root.iAirGroundStats;
                    iCollisionStats = _targetNode.Root.iCollisionStats;
                }
                MoveDef = TargetNode.Root;
                //Offset.Text = TargetNode.HexOffset;
                MakeScript();

                description.Text = "No Description Available.";
            }
            else
            {
                MoveDef = null;
                EventList.Items.Clear();
                //Offset.Text = "";
            }
        }

        public void MakeScript()
        {
            if (TargetNode == null)
                return;

            string[] script = new string[TargetNode.Children.Count];
            int tabs = 0;

            for (int i = 0; i < TargetNode.Children.Count; i++)
            {
                MoveDefEventNode node = TargetNode.Children[i] as MoveDefEventNode;
                string arg = node._name;

                //Format the event and its parameters into a readable script.
                script[i] = ResolveEventSyntax(GetEventSyntax(node._event), node.EventData);
                if (script[i] == "") script[i] = GetDefaultSyntax(node.EventData);

                //Add tabs to the script in correspondence to the code Params.
                tabs -= MParams.TabDownEvents(node._event);
                for (int i2 = 0; i2 < tabs; i2++) script[i] = "\t" + script[i];
                tabs += MParams.TabUpEvents(node._event);
            }
            EventList.Items.Clear();
            EventList.Items.AddRange(script);
        }

        //  Return the event syntax corresponding to the event id passed
        public string GetEventSyntax(int id)
        {
            if (TargetNode.Root.EventDictionary.ContainsKey(id))
                return TargetNode.Root.EventDictionary[id]._syntax;
            
            return "";
        }

        //Return the parameters contained in the keyword's parameter list.
        public string[] GetParameters(string strParams, Event eventData)
        {
            string[] parameters = new string[0];
            char chrFound = '\0';
            int paramEnd = -1;
            int index = 0;
            int loc = 0;

            //Search for a ',' or a ')' and return the preceeding string.
            do
            {
                paramEnd = MParams.FindFirstOfIgnoreNest(strParams, loc, new char[] { ',', ')' }, ref chrFound);
                if (paramEnd == -1) paramEnd = strParams.Length;

                Array.Resize<string>(ref parameters, index + 1);
                parameters[index] = strParams.Substring(loc, paramEnd - loc);
                parameters[index] = MParams.ClearWhiteSpace(parameters[index]);

                loc = paramEnd + 1;
                index++;
            } while (chrFound != ')' && chrFound != '\0');

            //Check each parameter for keywords and resolve if they are present.
            for (int i = 0; i < parameters.Length; i++)
                if (parameters[i] != "") parameters[i] = ResolveEventSyntax(parameters[i], eventData);

            return parameters;
        }

        //Return the collision status corresponding to the value passed.
        public string GetCollisionStatus(long value)
        {
            if (value > iCollisionStats.Length)
                return "Undefined(" + value + ")";

            return iCollisionStats[value];
        }

        //Return the air or ground status corresponding to the value passed.
        public string GetAirGroundStatus(long value)
        {
            if (value > iAirGroundStats.Length)
                return "Undefined(" + value + ")";

            return iAirGroundStats[value];
        }

        //Return the collision status corresponding to the value passed.
        public string GetEnum(int paramIndex, long value, Event eventData)
        {
            if (TargetNode.Root.EventDictionary.ContainsKey(eventData.eventEvent))
            {
                Dictionary<int, List<string>> Params = TargetNode.Root.EventDictionary[eventData.eventEvent].Enums;
                if (Params.ContainsKey(paramIndex))
                {
                    List<string> values = Params[paramIndex];
                    if (values != null && values.Count > value)
                        return values[(int)value];
                }
            }
            return "Undefined(" + value + ")";
        }

        //Return the string result from the passed keyword and its parameters.
        public string ResolveKeyword(string keyword, string[] Params, Event eventData)
        {
            switch (keyword)
            {
                case "\\value":
                    try { return ResolveParamTypes(eventData)[int.Parse(Params[0])]; }
                    catch { return "Value-" + Params[0]; }
                case "\\type":
                    try { return eventData.parameters[int.Parse(Params[0])]._type.ToString(); }
                    catch { return "Type-" + Params[0]; }
                case "\\if":
                    bool compare = false;
                    try
                    {
                        switch (Params[1])
                        {
                            case "==": compare = int.Parse(Params[0]) == int.Parse(Params[2]); break;
                            case "!=": compare = int.Parse(Params[0]) != int.Parse(Params[2]); break;
                            case ">=": compare = int.Parse(Params[0]) >= int.Parse(Params[2]); break;
                            case "<=": compare = int.Parse(Params[0]) <= int.Parse(Params[2]); break;
                            case ">": compare = int.Parse(Params[0]) > int.Parse(Params[2]); break;
                            case "<": compare = int.Parse(Params[0]) < int.Parse(Params[2]); break;
                        }
                    }
                    finally { }
                    if (compare)
                        return Params[3];
                    else
                        return Params[4];
                case "\\bone":
                    try
                    {
                        int id = MParams.UnHex(Params[0]);
                        if (id >= 400)
                            id -= 400;
                        if (_targetNode.Model != null && _targetNode.Model._linker.BoneCache != null && _targetNode.Model._linker.BoneCache.Length > id && id >= 0)
                            return _targetNode.Model._linker.BoneCache[id].Name;
                        else return id.ToString();
                    }
                    catch { return int.Parse(Params[0]).ToString(); }
                case "\\unhex":
                    try { return MParams.UnHex(Params[0]).ToString(); }
                    catch { return Params[0]; }
                case "\\hex":
                    try { return MParams.Hex(int.Parse(Params[0])); }
                    catch { return Params[0]; }
                case "\\hex8":
                    try { return MParams.Hex8(int.Parse(Params[0])); }
                    catch { return Params[0]; }
                case "\\half1":
                    return MParams.WordH(Params[0], 0);
                case "\\half2":
                    return MParams.WordH(Params[0], 1);
                case "\\byte1":
                    return MParams.WordB(Params[0], 0);
                case "\\byte2":
                    return MParams.WordB(Params[0], 1);
                case "\\byte3":
                    return MParams.WordB(Params[0], 2);
                case "\\byte4":
                    return MParams.WordB(Params[0], 3);
                case "\\collision":
                    try { return GetCollisionStatus(MParams.UnHex(Params[0])); }
                    catch { return Params[0]; }
                case "\\airground":
                    try { return GetAirGroundStatus(MParams.UnHex(Params[0])); }
                    catch { return Params[0]; }
                case "\\enum":
                    try { return GetEnum(int.Parse(Params[1]), MParams.UnHex(Params[0]), eventData); }
                    catch { return "Undefined(" + Params[1] + ")"; }
                case "\\cmpsign":
                    try { return MParams.GetComparisonSign(MParams.UnHex(Params[0])); }
                    catch { return Params[0]; }
                case "\\name":
                    return GetEventInfo(eventData.eventEvent)._name;
                case "\\white":
                    return " ";
                default:
                    return "";
            }
        }
        //Return a string of the parameter in the format corresponding to it's type.
        public string[] ResolveParamTypes(Event eventData)
        {
            string[] p = new string[eventData.parameters.Length];

            for (int i = 0; i < p.Length; i++)
            {
                switch ((int)eventData.parameters[i]._type)
                {
                    case 0: p[i] = GetValue(eventData.eventEvent, i, eventData.parameters[i]._data); break;
                    case 1: p[i] = MParams.UnScalar(eventData.parameters[i]._data).ToString(); break;
                    case 2: p[i] = ResolvePointer(eventData.pParameters + i * 8 + 4, eventData.parameters[i]); break;
                    case 3: p[i] = (eventData.parameters[i]._data != 0 ? "true" : "false"); break;
                    case 4: p[i] = MParams.Hex(eventData.parameters[i]._data); break;
                    case 5: p[i] = ResolveVariable(eventData.parameters[i]._data); break;
                    case 6: p[i] = GetRequirement(eventData.parameters[i]._data); break;
                }
            }
            return p;
        }
        //Return the name of the external pointer corresponding to the address if 
        //one is available, otherwise return the string of the value passed.
        public string ResolvePointer(long pointer, Param parameter)
        {
            MoveDefExternalNode ext;
            if ((ext = _targetNode.Root.IsExternal((int)pointer)) != null || (ext = _targetNode.Root.IsExternal((int)parameter._data)) != null)
                return "External: " + ext.Name;

            int list, type, index;
            _targetNode.Root.GetLocation((int)parameter._data, out list, out type, out index);

            if (list == 4)
                return "0x" + MParams.Hex(parameter._data);
            else
            {
                string name = "", t = "", grp = "";
                switch (list)
                {
                    case 0:
                        grp = "Actions";
                        name = _targetNode.Root._actions.Children[index].Name;
                        switch (type)
                        {
                            case 0: t = "Entry"; break;
                            case 1: t = "Exit"; break;
                        }
                        break;
                    case 1:
                        grp = "SubActions";
                        name = _targetNode.Root._subActions.Children[index].Name;
                        switch (type)
                        {
                            case 0: t = "Main"; break;
                            case 1: t = "GFX"; break;
                            case 2: t = "SFX"; break;
                            case 3: t = "Other"; break;
                        }
                        break;
                    case 2:
                        grp = "SubRoutines";
                        name = _targetNode.Root._subRoutineList[index].Name;
                        break;
                    case 3:
                        grp = "References";
                        name = _targetNode.Root.references.Children[index].Name;
                        break;
                }
                
                return name + (list >= 2 ? "" : " - " + t) + " in the " + grp + " list";
            }
        }

        //Return the full name of the variable corresponding to the value passed.
        public string ResolveVariable(long value)
        {
            string variableName = "";
            long variableMemType = (value & 0xF0000000) / 0x10000000;
            long variableType = (value & 0xF000000) / 0x1000000;
            long variableNumber = (value & 0xFF);
            if (variableMemType == 0) variableName = "IC-";
            if (variableMemType == 1) variableName = "LA-";
            if (variableMemType == 2) variableName = "RA-";
            if (variableType == 0) variableName += "Basic";
            if (variableType == 1) variableName += "Float";
            if (variableType == 2) variableName += "Bit";
            variableName += "[" + variableNumber + "]";

            return variableName;
        }

        public string GetValue(long eventId, int index, long value)
        {
            string s = null;
            switch (eventId)
            {
                case 0x04000100:
                case 0x04000200:
                    if (index == 0)
                        if (TargetNode.Parent != null && TargetNode.Parent.Parent != null && TargetNode.Parent.Parent.Name.StartsWith("Article"))
                        {
                            ResourceNode sa = TargetNode.Parent.Parent.FindChild("SubActions", false);
                            if (sa != null)
                                return sa.Children[(int)value].Name;
                        }
                        else if (value < TargetNode.Root._subActions.Children.Count && value >= 0)
                            return TargetNode.Root._subActions.Children[(int)value].Name;
                        else return ((int)value).ToString();
                    break;
                //case 0x02010200:
                //case 0x02010300:
                //case 0x02010500:
                //    if (index == 0)
                //        if (TargetNode.Parent != null && TargetNode.Parent.Parent != null && TargetNode.Parent.Parent.Name.StartsWith("Article"))
                //        {
                //            ResourceNode sa = TargetNode.Parent.Parent.FindChild("Actions", false);
                //            if (sa != null)
                //                return sa.Children[(int)value].Name;
                //        }
                //        else if (value < TargetNode.Root._actions.Children.Count)
                //            return TargetNode.Root._actions.Children[(int)value].Name;
                //    break;
            }
            return s == null ? MParams.Hex((int)value) : s;
        }

        //Return the requirement corresponding to the value passed.
        public string GetRequirement(long value)
        {
            bool not = (value & 0x80000000) == 0x80000000;
            long requirement = value & 0xFF;

            if (requirement > iRequirements.Length)
                return MParams.Hex(requirement);

            if (not == true)
                return "Not " + iRequirements[requirement];

            return iRequirements[requirement];
        }
        public ActionEventInfo GetEventInfo(long id)
        {
            if (MoveDef.EventDictionary == null)
                MoveDef.LoadEventDictionary();

            if (MoveDef.EventDictionary.ContainsKey(id))
                return MoveDef.EventDictionary[id];

            return new ActionEventInfo(id, id.ToString("X"), "No Description Available.", null, null);
        }

        //Return the event name followed by each parameter paired with its type.
        public string GetDefaultSyntax(Event eventData)
        {
            string script = GetEventInfo(eventData.eventEvent)._name + (eventData.lParameters > 0 ? ": " : "");
            for (int i = 0; i < eventData.lParameters; i++)
            {
                script += eventData.parameters[i]._type + "-";
                switch ((int)eventData.parameters[i]._type)
                {
                    case 0: script += GetValue(eventData.eventEvent, i, eventData.parameters[i]._data); break;
                    case 1: script += MParams.UnScalar(eventData.parameters[i]._data).ToString(); break;
                    case 2: script += ResolvePointer(eventData.pParameters + i * 8 + 4, eventData.parameters[i]); break;
                    case 3: script += (eventData.parameters[i]._data != 0 ? "true" : "false"); break;
                    case 4: script += MParams.Hex(eventData.parameters[i]._data); break;
                    case 5: script += ResolveVariable(eventData.parameters[i]._data); break;
                    case 6: script += GetRequirement(eventData.parameters[i]._data); break;
                }
                if (i != eventData.lParameters)
                    script += ", ";
            }
            return script;
        }

        //Return the passed syntax with all keywords replaced with their proper values.
        public string ResolveEventSyntax(string syntax, Event eventData)
        {
            while (true)
            {
                string keyword = "";
                string keyResult = "";
                string strParams = "";
                string[] kParams;

                int keyBegin = MParams.FindFirst(syntax, 0, '\\');
                if (keyBegin == -1) break;

                int keyEnd = MParams.FindFirst(syntax, keyBegin, '(');
                if (keyEnd == -1) keyEnd = syntax.Length;

                int paramsBegin = keyEnd + 1;

                int paramsEnd = MParams.FindFirstIgnoreNest(syntax, paramsBegin, ')');
                if (paramsEnd == -1) paramsEnd = syntax.Length;

                keyword = syntax.Substring(keyBegin, keyEnd - keyBegin);

                strParams = syntax.Substring(paramsBegin, paramsEnd - paramsBegin);
                kParams = GetParameters(strParams, eventData);

                keyResult = ResolveKeyword(keyword, kParams, eventData);

                syntax = MParams.DelSubstring(syntax, keyBegin, (paramsEnd + 1) - keyBegin);
                syntax = MParams.InsString(syntax, keyResult, keyBegin);
            }

            return syntax;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            int highest = EventList.Items.Count;
            if (EventList.SelectedIndex != -1)
                highest = EventList.SelectedIndices[EventList.SelectedIndices.Count - 1];

            MoveDefEventNode d = new MoveDefEventNode();
            if (highest == EventList.Items.Count)
                TargetNode.AddChild(d);
            else
                TargetNode.InsertChild(d, true, highest + 1);
            d.EventID = 0x00020000;
            MakeScript();

            if (EventList.Items.Count > highest + 1)
                EventList.SelectedIndex = highest + 1;
            else
                EventList.SelectedIndex = EventList.Items.Count - 1;
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            if (EventList.SelectedIndex != -1)
            if (form != null)
            {
                form.eventModifier1._oldSelectedObject = form.SelectedObject;
                form.SelectedObject = TargetNode.Children[EventList.SelectedIndex] as MoveDefEventNode;
            }
            else
            {
                FormModifyEvent p = new FormModifyEvent();
                p.eventModifier1.origEvent = TargetNode.Children[EventList.SelectedIndex] as MoveDefEventNode;
                p.eventModifier1.Setup(null);
                if (p.ShowDialog() == DialogResult.OK)
                    MakeScript();
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            int[] indices = new int[EventList.SelectedIndices.Count];
            EventList.SelectedIndices.CopyTo(indices, 0);
            for (int i = 0; i < indices.Length; i++)
                TargetNode.Children[indices[0]].Remove();
            MakeScript();
            if (TargetNode != null && indices.Length == 1)
                foreach (int i in indices)
                    if (i - indices.Length >= 0)
                        EventList.SetSelected(i - indices.Length, true);
                    else if (EventList.Items.Count > 0)
                        EventList.SetSelected(0, true);
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            int lowest = -1;
            if (EventList.SelectedIndex != -1)
                lowest = EventList.SelectedIndices[0];
            int[] indices = new int[EventList.SelectedIndices.Count];
            EventList.SelectedIndices.CopyTo(indices, 0);
            if (lowest != -1 && lowest != 0)
            {
                for (int i = 0; i < EventList.SelectedIndices.Count; i++)
                    TargetNode.Children[EventList.SelectedIndices[i]].doMoveUp(false);
                MakeScript();
                if (TargetNode != null)
                    foreach (int i in indices)
                        EventList.SetSelected(i - 1, true);
            }
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            int highest = -1;
            if (EventList.SelectedIndex != -1)
                highest = EventList.SelectedIndices[EventList.SelectedIndices.Count - 1];
            int[] indices = new int[EventList.SelectedIndices.Count];
            EventList.SelectedIndices.CopyTo(indices, 0);
            if (highest != -1 && highest != EventList.Items.Count - 1)
            {
                for (int i = EventList.SelectedIndices.Count - 1; i >= 0; i--)
                    TargetNode.Children[EventList.SelectedIndices[i]].doMoveDown(false);
                MakeScript();
                if (TargetNode != null)
                    foreach (int i in indices)
                        EventList.SetSelected(i + 1, true);
            }
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            string s = "";
            foreach (int i in EventList.SelectedIndices)
            {
                s += (TargetNode.Children[i] as MoveDefEventNode).Serialize();
                s += "/";
            }
            if (!String.IsNullOrEmpty(s))
                Clipboard.SetText(s);
        }

        private void btnPaste_Click(object sender, EventArgs e)
        {
            List<int> indices = new List<int>();

            int highest = EventList.Items.Count;
            if (EventList.SelectedIndex != -1)
                highest = EventList.SelectedIndices[EventList.SelectedIndices.Count - 1];

            string s = Clipboard.GetText();

            try
            {
                string[] Events = s.Split('/');
                foreach (string x in Events)
                {
                    MoveDefEventNode y = MoveDefEventNode.Deserialize(x, TargetNode.Root);
                    if (y != null)
                    {
                        if (highest == TargetNode.Children.Count)
                            TargetNode.AddChild(y);
                        else
                            TargetNode.InsertChild(y, true, highest + 1);
                        indices.Add(y.Index);
                        highest++;
                    }
                }
            }
            finally
            {
                MakeScript(); 
                if (TargetNode != null)
                    foreach (int i in indices)
                        EventList.SetSelected(i, true);
            }
        }

        private void EventList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (EventList.SelectedIndex >= 0)
            {
                ActionEventInfo info = (TargetNode.Children[EventList.SelectedIndex] as MoveDefEventNode).EventInfo;
                if (info != null && !String.IsNullOrEmpty(info._description))
                    description.Text = info._description;
                else
                    description.Text = "No Description Available.";
            }
        }

        private void EventList_DoubleClick(object sender, EventArgs e)
        {
            btnModify_Click(sender, e);
        }

        private void btnCopyText_Click(object sender, EventArgs e)
        {
            string s = "";
            foreach (int i in EventList.SelectedIndices)
            {
                s += EventList.Items[i].ToString();
                s += Environment.NewLine;
            }
            Clipboard.SetText(s);
        }

        private void btnCut_Click(object sender, EventArgs e)
        {
            btnCopy_Click(sender, e);
            btnRemove_Click(sender, e);
        }
    }
}
