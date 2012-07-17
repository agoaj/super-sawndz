using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.OpenGL;
using BrawlLib.SSBBTypes;
using System.ComponentModel;
using BrawlLib.SSBB.ResourceNodes;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class MoveDefEventNode : MoveDefEntryNode
    {
        internal FDefEvent* Header { get { return (FDefEvent*)WorkingUncompressed.Address; } }
        internal FDefEventArgument* ArgumentHeader { get { return (FDefEventArgument*)(BaseAddress + Header->_argumentOffset); } }

        internal byte nameSpace, id, numArguments, unk1;
        internal List<FDefEventArgument> arguments = new List<FDefEventArgument>();

        public override ResourceType ResourceType { get { return ResourceType.Event; } }

        protected override int OnCalculateSize(bool force)
        {
            int size = 8;
            _lookupCount = (Children.Count > 0 ? 1 : 0);
            foreach (MoveDefEventParameterNode p in Children)
            {
                size += p.CalculateSize(true);
                _lookupCount += p._lookupCount;
            }
            return size;
        }

        [Browsable(false)]
        public ActionEventInfo EventInfo { get { if (Root.EventDictionary == null) Root.LoadEventDictionary(); if (Root.EventDictionary.ContainsKey(_event)) return Root.EventDictionary[_event]; else return null; } }
        public int _event;

        [Browsable(false)]
        public int EventID 
        {
            get { return _event; }
            set 
            {
                _event = value;
                string ev = MParams.Hex8(_event);
                nameSpace = byte.Parse(ev.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                id = byte.Parse(ev.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
                numArguments = byte.Parse(ev.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
                unk1 = byte.Parse(ev.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
                if (Root.EventDictionary.ContainsKey(_event))
                    Name = Root.EventDictionary[_event]._name;
                else
                    Name = ev;
            } 
        }

        [Browsable(false)]
        public Event EventData
        {
            get
            {
                Event e = new Event();
                e.SetEventEvent(_event);
                e.pParameters = ArgumentOffset;
                int i = 0;
                foreach (ResourceNode r in Children)
                    if (r is MoveDefEventParameterNode)
                    {
                        e.parameters[i]._type = (r as MoveDefEventParameterNode)._type;
                        e.parameters[i++]._data = (r as MoveDefEventParameterNode)._value;
                    }
                return e;
            }
        }

        public string Serialize()
        {
            string s = "";
            s += MParams.Hex8(EventID) + "|";
            foreach (MoveDefEventParameterNode p in Children)
            {
                s += ((int)p._type).ToString() + "\\";
                s += p._value + "|";
            }
            return s;
        }

        public static MoveDefEventNode Deserialize(string s, MoveDefNode node)
        {
            if (String.IsNullOrEmpty(s))
                return null;

            try
            {
                string[] lines = s.Split('|');

                if (lines[0].Length != 8)
                    return null;

                MoveDefEventNode newEv = new MoveDefEventNode() { _parent = node };

                string id = lines[0];
                int idNumber = Convert.ToInt32(id, 16);

                newEv.EventID = idNumber;

                int _event = newEv.EventID;
                ActionEventInfo info = newEv.EventInfo;

                for (int i = 0; i < newEv.numArguments; i++)
                {
                    string[] pLines = lines[i + 1].Split('\\');

                    int type = int.Parse(pLines[0]);
                    int value = int.Parse(pLines[1]);

                    if ((_event == 0x06000D00 || _event == 0x06150F00 || _event == 0x062B0D00) && i == 12)
                        newEv.AddChild(new HitboxFlagsNode(info != null ? info.Params[i] : "Value") { _value = value, val = new HitboxFlags() { data = value } });
                    else if (((_event == 0x06000D00 || _event == 0x06150F00 || _event == 0x062B0D00) && (i == 0 || i == 3 || i == 4)) ||
                        ((_event == 0x11001000 || _event == 0x11010A00 || _event == 0x11020A00) && i == 0))
                        newEv.AddChild(new MoveDefEventValue2HalfNode(info != null ? info.Params[i] : "Value") { _value = value });
                    else if (i == 14 && _event == 0x06150F00)
                        newEv.AddChild(new SpecialHitboxFlagsNode(info != null ? info.Params[i] : "Value") { _value = value, val = new SpecialHitboxFlags() { data = value } });
                    else if ((ArgVarType)(int)type == ArgVarType.Value)
                        newEv.AddChild(new MoveDefEventValueNode(info != null ? info.Params[i] : "Value") { _value = value });
                    else if ((ArgVarType)(int)type == ArgVarType.Scalar)
                        newEv.AddChild(new MoveDefEventScalarNode(info != null ? info.Params[i] : "Scalar") { _value = value });
                    else if ((ArgVarType)(int)type == ArgVarType.Boolean)
                        newEv.AddChild(new MoveDefEventBoolNode(info != null ? info.Params[i] : "Boolean") { _value = value });
                    else if ((ArgVarType)(int)type == ArgVarType.Unknown)
                        newEv.AddChild(new MoveDefEventUnkNode(info != null ? info.Params[i] : "Unknown") { _value = value });
                    else if ((ArgVarType)(int)type == ArgVarType.Requirement)
                    {
                        MoveDefEventRequirementNode r = new MoveDefEventRequirementNode(info != null ? info.Params[i] : "Requirement") { _value = value };
                        newEv.AddChild(r);
                        r.val = r.GetRequirement(r._value);
                    }
                    else if ((ArgVarType)(int)type == ArgVarType.Variable)
                    {
                        MoveDefEventVariableNode v = new MoveDefEventVariableNode(info != null ? info.Params[i] : "Variable") { _value = value };
                        newEv.AddChild(v);
                        v.val = v.ResolveVariable(v._value);
                    }
                    else if ((ArgVarType)(int)type == ArgVarType.Offset)
                        newEv.AddChild(new MoveDefEventOffsetNode(info != null ? info.Params[i] : "Offset") { _value = value });
                }

                newEv._parent = null;
                return newEv;
            }
            catch { return null; }
        }

        [Category("MoveDef Event")]
        public byte NameSpace { get { return nameSpace; } }//set { nameSpace = value; SignalPropertyChange(); } }
        [Category("MoveDef Event")]
        public byte ID { get { return id; } }//set { id = value; SignalPropertyChange(); } }
        [Category("MoveDef Event")]
        public byte NumArguments { get { return numArguments; } }//set { numArguments = value; SignalPropertyChange(); } }
        [Category("MoveDef Event")]
        public byte Unknown { get { return unk1; } set { unk1 = value; SignalPropertyChange(); } }
        [Category("MoveDef Event")]
        public uint ArgumentOffset { get { return argOffset; } }
        public uint argOffset = 0;

        [Category("MoveDef Event Argument")]
        public ArgVarType[] Type { get { var array = from x in arguments select (ArgVarType)(int)x._type; return array.ToArray<ArgVarType>(); } }
        [Category("MoveDef Event Argument")]
        public int[] Value { get { var array = from x in arguments select (int)x._data; return array.ToArray<int>(); } }

        [Browsable(false)]
        public List<FDefEventArgument> Arguments { get { return arguments; } set { arguments = value; } }

        protected override bool OnInitialize()
        {
            base.OnInitialize();

            if ((int)Header == (int)BaseAddress)
                return false;

            argOffset = Header->_argumentOffset;

            nameSpace = Header->_nameSpace;
            id = Header->_id;
            numArguments = Header->_numArguments;
            unk1 = Header->_unk1;

            //Merge values to create ID and match with events to get name
            _event = int.Parse(String.Format("{0:X02}{1:X02}{2:X02}{3:X02}", nameSpace, id, numArguments, unk1), System.Globalization.NumberStyles.HexNumber);
            if (Root.EventDictionary.ContainsKey(_event))
                _name = Root.EventDictionary[_event]._name;
            else
            {
                if (unk1 > 0)
                {
                    int temp = int.Parse(String.Format("{0:X02}{1:X02}{2:X02}{3:X02}", nameSpace, id, numArguments, 0), System.Globalization.NumberStyles.HexNumber);
                    if (Root.EventDictionary.ContainsKey(temp))
                    {
                        _name = Root.EventDictionary[temp]._name + " (Unknown == " + unk1 + ")";
                        _event = temp;
                    }
                }
                else _name = MParams.Hex8(_event);
            }

            if (!Root._events.ContainsKey(_event))
                Root._events.Add(_event, new List<MoveDefEventNode>() { this });
            else
                Root._events[_event].Add(this);

            if (_name == "FADEF00D" || _name == "FADE0D8A" || numArguments == 240)
            {
                //_name = "<null>";
                return false;
            }
            
            for (int i = 0; i < numArguments; i++)
            {
                FDefEventArgument e;
                FDefEventArgument* header = &ArgumentHeader[i];
                arguments.Add(e = *header);

                string param = null;
                if (EventInfo != null && EventInfo.Params != null && EventInfo.Params.Length != 0 && EventInfo.Params.Length > i)
                    param = String.IsNullOrEmpty(EventInfo.Params[i]) ? null : EventInfo.Params[i];

                if ((_event == 0x06000D00 || _event == 0x06150F00 || _event == 0x062B0D00) && i == 12)
                    new HitboxFlagsNode(param).Initialize(this, header, 8);
                else if (((_event == 0x06000D00 || _event == 0x06150F00 || _event == 0x062B0D00) && (i == 0 || i == 3 || i == 4)) ||
                    ((_event == 0x11001000 || _event == 0x11010A00 || _event == 0x11020A00) && i == 0))
                    new MoveDefEventValue2HalfNode(param).Initialize(this, header, 8);
                else if (i == 14 && _event == 0x06150F00)
                    new SpecialHitboxFlagsNode(param).Initialize(this, header, 8);
                else if ((ArgVarType)(int)e._type == ArgVarType.Value)
                {
                    if (EventInfo != null && EventInfo.Enums != null && EventInfo.Enums.ContainsKey(i))
                        new MoveDefEventValueEnumNode(param) { Enums = EventInfo.Enums[i].ToArray() }.Initialize(this, header, 8);
                    else
                        new MoveDefEventValueNode(param).Initialize(this, header, 8);
                }
                else if ((ArgVarType)(int)e._type == ArgVarType.Unknown)
                    new MoveDefEventUnkNode(param).Initialize(this, header, 8);
                else if ((ArgVarType)(int)e._type == ArgVarType.Scalar)
                    new MoveDefEventScalarNode(param).Initialize(this, header, 8);
                else if ((ArgVarType)(int)e._type == ArgVarType.Boolean)
                    new MoveDefEventBoolNode(param).Initialize(this, header, 8);
                else if ((ArgVarType)(int)e._type == ArgVarType.Requirement)
                    new MoveDefEventRequirementNode(param).Initialize(this, header, 8);
                else if ((ArgVarType)(int)e._type == ArgVarType.Variable)
                    new MoveDefEventVariableNode(param).Initialize(this, header, 8);
                else if ((ArgVarType)(int)e._type == ArgVarType.Offset)
                {
                    int offset = -1;
                    MoveDefExternalNode ext;
                    int paramOffset = e._data;

                    if (paramOffset == -1)
                        ext = Root.IsExternal((int)ArgumentOffset + i * 8 + 4);
                    else
                        ext = Root.IsExternal(paramOffset);

                    if (ext == null)
                        offset = e._data;

                    if (offset > 0)
                    {
                        MoveDefActionNode a;
                        int list, index, type;
                        Root.GetLocation(offset, out list, out type, out index);

                        if (list == 4) //Offset not found in existing nodes
                        {
                            Root._subRoutines[offset] = (a = new MoveDefActionNode("SubRoutine" + Root._subRoutineList.Count, false, null));
                            a.Initialize(Root._subRoutineGroup, new DataSource((sbyte*)BaseAddress + offset, 0));
                            a.Populate();
                            a._actionRefs.Add(this);
                        }
                        else
                        {
                            MoveDefActionNode n = Root.GetAction(list, type, index);
                            if (n != null)
                                n._actionRefs.Add(this);
                        }
                    }

                    //Add ID node
                    if (ext != null)
                    {
                        MoveDefEventOffsetNode x = new MoveDefEventOffsetNode(param) { _name = ext.Name, _extNode = ext, _extOverride = true };
                        x.Initialize(this, header, 8);
                        ext._refs.Add(x);
                    }
                    else
                        new MoveDefEventOffsetNode(param).Initialize(this, header, 8);
                }
            }
            return arguments.Count > 0;
        }

        #region Rendering
        public int HitboxID = -1, HitboxSize = 0;

        #region Offensive Collision
        public unsafe void RenderOffensiveCollision(ResourceNode[] bl, GLContext c, Vector3 cam, MParams.DrawStyle style)
        {
            //Coded by Toomai
            //Modified for release v0.67

            if (_event != 0x06000D00) //Offensive Collision
                return;

            Event e = EventData;
            HitboxFlagsNode flags = Children[12] as HitboxFlagsNode;

            int boneindex = (int)e.parameters[0]._data >> 16;
            long size = HitboxSize;
            long angle = e.parameters[2]._data;

            if (boneindex >= 400) // hack to make Kirby and Wario work properly - overalls Wario will not
                boneindex -= 400;

            if (boneindex == 0) // if a hitbox is on TopN, make it follow TransN
            {
                int transindex = 0;
                foreach (MDL0BoneNode bn in bl) // this shouldn't take long; TransN should be within the first 10
                {
                    if (bn.Name.Equals("TransN"))
                        break;
                    transindex++;
                }
                boneindex = transindex;
            }
            MDL0BoneNode b;
            b = bl[boneindex] as MDL0BoneNode;
            Vector3 bonepos = b._frameMatrix.GetPoint();
            Vector3 pos = new Vector3(MParams.UnScalar(e.parameters[6]._data), MParams.UnScalar(e.parameters[7]._data), MParams.UnScalar(e.parameters[8]._data));
            Vector3 bonerot = b._frameMatrix.GetAngles();
            Matrix r = b._frameMatrix.GetRotationMatrix();
            Vector3 bonescl = b.RecursiveScale();
            pos._x /= bonescl._x;
            pos._y /= bonescl._y;
            pos._z /= bonescl._z;
            Vector3 globpos = r.Multiply(pos);
            Matrix m = Matrix.TransformMatrix(new Vector3(1), bonerot, globpos + bonepos);
            Vector3 resultpos = new Vector3(m[12], m[13], m[14]);
            m = Matrix.TransformMatrix(new Vector3(MParams.UnScalar(size)), new Vector3(), resultpos);
            c.glPushMatrix();
            c.glMultMatrix((float*)&m);
            int res = 16;
            double drawangle = 360.0 / res;
            // bubble
            if (style == MParams.DrawStyle.SSB64)
            {
                c.glColor(1.0f, 1.0f, 1.0f, 0.25f);
                c.DrawInvertedCube(new Vector3(0, 0, 0), 1.025f);
                c.glColor(1.0f, 0.0f, 0.0f, 0.5f);
                c.DrawCube(new Vector3(0, 0, 0), 0.975f);
            }
            else
            {
                if (style == MParams.DrawStyle.Melee)
                    c.glColor(1.0f, 0.0f, 0.0f, 0.5f);
                else
                {
                    Vector3 typecolour = MParams.getTypeColour(flags.Type);
                    c.glColor((typecolour._x / 255.0f), (typecolour._y / 225.0f), (typecolour._z / 255.0f), 0.5f);
                }
                GLDisplayList spheres = c.GetSphereList();
                spheres.Call();
            }
            if (style == MParams.DrawStyle.Brawl)
            {
                // angle indicator
                double rangle = angle / 180.0 * Math.PI;
                Vector3 effectcolour = MParams.getEffectColour(flags.Effect);
                c.glColor((effectcolour._x / 255.0f), (effectcolour._y / 225.0f), (effectcolour._z / 255.0f), 0.75f);
                c.glPushMatrix();
                if (angle == 361)
                {
                    m = Matrix.TransformMatrix(new Vector3(0.5f), (globpos + bonepos).LookatAngles(cam) * Maths._rad2degf, new Vector3(0));
                    c.glMultMatrix((float*)&m);
                    c.glBegin(GLPrimitiveType.Quads);
                    for (int i = 0; i < 16; i += 2)
                    {
                        c.glVertex(Math.Cos((i - 1) * Math.PI / 8) * 0.5, Math.Sin((i - 1) * Math.PI / 8) * 0.5, 0);
                        c.glVertex(Math.Cos(i * Math.PI / 8), Math.Sin(i * Math.PI / 8), 0);
                        c.glVertex(Math.Cos((i + 1) * Math.PI / 8) * 0.5, Math.Sin((i + 1) * Math.PI / 8) * 0.5, 0);
                        c.glVertex(0, 0, 0);
                    }
                    c.glEnd();
                }
                else
                {
                    long a = -angle; // otherwise 90 would point down
                    int angleflip = 0;
                    if (resultpos._z < 0)
                        angleflip = 180;
                    m = Matrix.TransformMatrix(new Vector3(1), new Vector3(a, angleflip, 0), new Vector3());
                    c.glMultMatrix((float*)&m);
                    c.glBegin(GLPrimitiveType.Quads);
                    // left face
                    c.glVertex(0.1, 0.1, 0);
                    c.glVertex(0.1, 0.1, 1);
                    c.glVertex(0.1, -0.1, 1);
                    c.glVertex(0.1, -0.1, 0);
                    // right face
                    c.glVertex(-0.1, -0.1, 0);
                    c.glVertex(-0.1, -0.1, 1);
                    c.glVertex(-0.1, 0.1, 1);
                    c.glVertex(-0.1, 0.1, 0);
                    // top face
                    c.glVertex(-0.1, 0.1, 0);
                    c.glVertex(-0.1, 0.1, 1);
                    c.glVertex(0.1, 0.1, 1);
                    c.glVertex(0.1, 0.1, 0);
                    // bottom face
                    c.glVertex(0.1, -0.1, 0);
                    c.glVertex(0.1, -0.1, 1);
                    c.glVertex(-0.1, -0.1, 1);
                    c.glVertex(-0.1, -0.1, 0);
                    // front face
                    c.glVertex(-0.1, -0.1, 1);
                    c.glVertex(0.1, -0.1, 1);
                    c.glVertex(0.1, 0.1, 1);
                    c.glVertex(-0.1, 0.1, 1);
                    // back face
                    c.glVertex(-0.1, 0.1, 0);
                    c.glVertex(0.1, 0.1, 0);
                    c.glVertex(0.1, -0.1, 0);
                    c.glVertex(-0.1, -0.1, 0);
                    c.glEnd();
                }
                c.glPopMatrix();
                // border
                GLDisplayList rings = c.GetRingList();
                for (int i = -5; i <= 5; i++)
                {
                    c.glPushMatrix();
                    m = Matrix.TransformMatrix(new Vector3(1 + 0.0025f * i), (globpos + bonepos).LookatAngles(cam) * Maths._rad2degf, new Vector3());
                    c.glMultMatrix((float*)&m);
                    if (flags.Clang)
                        rings.Call();
                    else
                    {
                        for (double j = 0; j < 360 / (drawangle / 2); j += 2)
                        {
                            double ang1 = (j * (drawangle / 2)) / 180 * Math.PI;
                            double ang2 = ((j + 1) * (drawangle / 2)) / 180 * Math.PI;
                            int q = 0;
                            c.glBegin(GLPrimitiveType.LineStrip);
                            c.glVertex(Math.Cos(ang1), Math.Sin(ang1), 0);
                            c.glVertex(Math.Cos(ang2), Math.Sin(ang2), 0);
                            c.glEnd();
                        }
                    }
                    c.glPopMatrix();
                }
            }
            c.glPopMatrix();
            c.glPopMatrix();
        }

        #endregion

        #region Special Offensive Collision
        public unsafe void RenderSpecialOffensiveCollision(ResourceNode[] bl, GLContext c, Vector3 cam, MParams.DrawStyle style)
        {
            //Coded by Toomai
            //Modified for release v0.67

            if (_event != 0x06150F00) //Special Offensive Collision
                return;

            Event e = EventData;
            HitboxFlagsNode flags = Children[12] as HitboxFlagsNode;
            SpecialHitboxFlagsNode specialFlags = Children[14] as SpecialHitboxFlagsNode;

            int boneindex = (int)e.parameters[0]._data >> 16;
            long size = HitboxSize;
            long angle = e.parameters[2]._data;

            // hack to make Kirby and Wario work properly - overalls Wario will not
            if (boneindex >= 400)
                boneindex -= 400;

            if (boneindex == 0) // if a hitbox is on TopN, make it follow TransN
            {
                int transindex = 0;
                foreach (MDL0BoneNode bn in bl) // this shouldn't take long; TransN should be within the first 10
                {
                    if (bn.Name.Equals("TransN")) break;
                    transindex++;
                }
                boneindex = transindex;
            }
            MDL0BoneNode b;
            b = bl[boneindex] as MDL0BoneNode;
            Vector3 bonepos = b._frameMatrix.GetPoint();
            Vector3 pos = new Vector3(MParams.UnScalar(e.parameters[6]._data), MParams.UnScalar(e.parameters[7]._data), MParams.UnScalar(e.parameters[8]._data));
            Vector3 bonerot = b._frameMatrix.GetAngles();
            Matrix r = b._frameMatrix.GetRotationMatrix();
            Vector3 bonescl = b.RecursiveScale();
            pos._x /= bonescl._x;
            pos._y /= bonescl._y;
            pos._z /= bonescl._z;
            Vector3 globpos = r.Multiply(pos);
            Matrix m = Matrix.TransformMatrix(new Vector3(1), bonerot, globpos + bonepos);
            Vector3 resultpos = new Vector3(m[12], m[13], m[14]);
            m = Matrix.TransformMatrix(new Vector3(MParams.UnScalar(size)), new Vector3(), resultpos);
            c.glPushMatrix();
            c.glMultMatrix((float*)&m);
            int res = 16, stretchres = 10;
            double drawangle = 360.0 / res;
            // bubble
            if (style == MParams.DrawStyle.SSB64)
            {
                c.glColor(1.0f, 1.0f, 1.0f, 0.25f);
                c.DrawInvertedCube(new Vector3(0, 0, 0), 1.025f);
                c.glColor(1.0f, 0.0f, 0.0f, 0.5f);
                c.DrawCube(new Vector3(0, 0, 0), 0.975f);
                if (specialFlags.Stretches)
                {
                    Vector3 reversepos = new Vector3(-globpos._x / MParams.UnScalar(size), -globpos._y / MParams.UnScalar(size), -globpos._z / MParams.UnScalar(size));
                    c.glTranslate(reversepos._x, reversepos._y, reversepos._z);
                    c.glColor(1.0f, 0.0f, 0.0f, 0.5f);
                    c.glBegin(GLPrimitiveType.Lines);
                    c.glVertex(-1, -1, -1); // stretch lines
                    c.glVertex(-1 - reversepos._x, -1 - reversepos._y, -1 - reversepos._z);
                    c.glVertex(-1, -1, 1);
                    c.glVertex(-1 - reversepos._x, -1 - reversepos._y, 1 - reversepos._z);
                    c.glVertex(-1, 1, -1);
                    c.glVertex(-1 - reversepos._x, 1 - reversepos._y, -1 - reversepos._z);
                    c.glVertex(-1, 1, 1);
                    c.glVertex(-1 - reversepos._x, 1 - reversepos._y, 1 - reversepos._z);
                    c.glVertex(1, -1, -1);
                    c.glVertex(1 - reversepos._x, -1 - reversepos._y, -1 - reversepos._z);
                    c.glVertex(1, -1, 1);
                    c.glVertex(1 - reversepos._x, -1 - reversepos._y, 1 - reversepos._z);
                    c.glVertex(1, 1, -1);
                    c.glVertex(1 - reversepos._x, 1 - reversepos._y, -1 - reversepos._z);
                    c.glVertex(1, 1, 1);
                    c.glVertex(1 - reversepos._x, 1 - reversepos._y, 1 - reversepos._z);
                    c.glEnd();
                    c.glBegin(GLPrimitiveType.LineLoop); // root box
                    c.glVertex(-1, -1, -1);
                    c.glVertex(-1, -1, 1);
                    c.glVertex(-1, 1, 1);
                    c.glVertex(-1, 1, -1);
                    c.glEnd();
                    c.glBegin(GLPrimitiveType.LineLoop);
                    c.glVertex(1, -1, -1);
                    c.glVertex(1, -1, 1);
                    c.glVertex(1, 1, 1);
                    c.glVertex(1, 1, -1);
                    c.glEnd();
                    c.glBegin(GLPrimitiveType.Lines);
                    c.glVertex(-1, -1, -1);
                    c.glVertex(1, -1, -1);
                    c.glVertex(-1, -1, 1);
                    c.glVertex(1, -1, 1);
                    c.glVertex(-1, 1, -1);
                    c.glVertex(1, 1, -1);
                    c.glVertex(-1, 1, 1);
                    c.glVertex(1, 1, 1);
                    c.glEnd();
                    c.glTranslate(-reversepos._x, -reversepos._y, -reversepos._z);
                }
            }
            else
            {
                if (style == MParams.DrawStyle.Melee)
                    c.glColor(1.0f, 0.0f, 0.0f, 0.5f);
                else
                {
                    Vector3 typecolour = MParams.getTypeColour(flags.Type);
                    c.glColor((typecolour._x / 255.0f), (typecolour._y / 225.0f), (typecolour._z / 255.0f), 0.5f);
                }
                GLDisplayList spheres = c.GetSphereList();
                spheres.Call();
                if (specialFlags.Stretches)
                {
                    c.glPushMatrix();
                    m = Matrix.TransformMatrix(new Vector3(1), bonerot, new Vector3());
                    c.glMultMatrix((float*)&m);
                    Vector3 reversepos = new Vector3(-pos._x / MParams.UnScalar(size), -pos._y / MParams.UnScalar(size), -pos._z / MParams.UnScalar(size));
                    if (style == MParams.DrawStyle.Melee)
                        c.glColor(1.0f, 0.0f, 0.0f, 0.5f);
                    else
                    {
                        Vector3 effectcolour = MParams.getEffectColour(flags.Effect);
                        c.glColor((effectcolour._x / 255.0f), (effectcolour._y / 225.0f), (effectcolour._z / 255.0f), 0.5f);
                    }
                    c.glTranslate(reversepos._x, reversepos._y, reversepos._z);
                    c.glBegin(GLPrimitiveType.Lines); // stretch lines
                    c.glVertex(1, 0, 0);
                    c.glVertex(1 - reversepos._x, 0 - reversepos._y, 0 - reversepos._z);
                    c.glVertex(-1, 0, 0);
                    c.glVertex(-1 - reversepos._x, 0 - reversepos._y, 0 - reversepos._z);
                    c.glVertex(0, 1, 0);
                    c.glVertex(0 - reversepos._x, 1 - reversepos._y, 0 - reversepos._z);
                    c.glVertex(0, -1, 0);
                    c.glVertex(0 - reversepos._x, -1 - reversepos._y, 0 - reversepos._z);
                    c.glVertex(0, 0, 1);
                    c.glVertex(0 - reversepos._x, 0 - reversepos._y, 1 - reversepos._z);
                    c.glVertex(0, 0, -1);
                    c.glVertex(0 - reversepos._x, 0 - reversepos._y, -1 - reversepos._z);
                    c.glEnd();
                    if (style == MParams.DrawStyle.Melee)
                        c.glColor(1.0f, 0.0f, 0.0f, 0.25f);
                    else
                    {
                        Vector3 typecolour = MParams.getTypeColour(flags.Type);
                        c.glColor((typecolour._x / 255.0f), (typecolour._y / 225.0f), (typecolour._z / 255.0f), 0.25f);
                    }
                    spheres.Call(); // root sphere
                    c.glTranslate(-reversepos._x, -reversepos._y, -reversepos._z);
                    c.glPopMatrix();
                }
            }
            if (style == MParams.DrawStyle.Brawl)
            {
                // angle indicator
                double rangle = angle / 180.0 * Math.PI;
                Vector3 effectcolour = MParams.getEffectColour(flags.Effect);
                c.glColor((effectcolour._x / 255.0f), (effectcolour._y / 225.0f), (effectcolour._z / 255.0f), 0.75f);
                c.glPushMatrix();
                if (angle == 361)
                {
                    m = Matrix.TransformMatrix(new Vector3(0.5f), (globpos + bonepos).LookatAngles(cam) * Maths._rad2degf, new Vector3(0));
                    c.glMultMatrix((float*)&m);
                    c.glBegin(GLPrimitiveType.Quads);
                    for (int i = 0; i < 16; i += 2)
                    {
                        c.glVertex(Math.Cos((i - 1) * Math.PI / 8) * 0.5, Math.Sin((i - 1) * Math.PI / 8) * 0.5, 0);
                        c.glVertex(Math.Cos(i * Math.PI / 8), Math.Sin(i * Math.PI / 8), 0);
                        c.glVertex(Math.Cos((i + 1) * Math.PI / 8) * 0.5, Math.Sin((i + 1) * Math.PI / 8) * 0.5, 0);
                        c.glVertex(0, 0, 0);
                    }
                    c.glEnd();
                }
                else
                {
                    long a = -angle; // otherwise 90 would point down
                    int angleflip = 0;
                    if (resultpos._z < 0)
                        angleflip = 180;
                    m = Matrix.TransformMatrix(new Vector3(1), new Vector3(a, angleflip, 0), new Vector3());
                    c.glMultMatrix((float*)&m);
                    c.glBegin(GLPrimitiveType.Quads);
                    // left face
                    c.glVertex(0.1, 0.1, 0);
                    c.glVertex(0.1, 0.1, 1);
                    c.glVertex(0.1, -0.1, 1);
                    c.glVertex(0.1, -0.1, 0);
                    // right face
                    c.glVertex(-0.1, -0.1, 0);
                    c.glVertex(-0.1, -0.1, 1);
                    c.glVertex(-0.1, 0.1, 1);
                    c.glVertex(-0.1, 0.1, 0);
                    // top face
                    c.glVertex(-0.1, 0.1, 0);
                    c.glVertex(-0.1, 0.1, 1);
                    c.glVertex(0.1, 0.1, 1);
                    c.glVertex(0.1, 0.1, 0);
                    // bottom face
                    c.glVertex(0.1, -0.1, 0);
                    c.glVertex(0.1, -0.1, 1);
                    c.glVertex(-0.1, -0.1, 1);
                    c.glVertex(-0.1, -0.1, 0);
                    // front face
                    c.glVertex(-0.1, -0.1, 1);
                    c.glVertex(0.1, -0.1, 1);
                    c.glVertex(0.1, 0.1, 1);
                    c.glVertex(-0.1, 0.1, 1);
                    // back face
                    c.glVertex(-0.1, 0.1, 0);
                    c.glVertex(0.1, 0.1, 0);
                    c.glVertex(0.1, -0.1, 0);
                    c.glVertex(-0.1, -0.1, 0);
                    c.glEnd();
                }
                c.glPopMatrix();
                // border
                GLDisplayList rings = c.GetRingList();
                for (int i = -5; i <= 5; i++)
                {
                    c.glPushMatrix();
                    m = Matrix.TransformMatrix(new Vector3(1 + 0.0025f * i), (globpos + bonepos).LookatAngles(cam) * Maths._rad2degf, new Vector3());
                    c.glMultMatrix((float*)&m);
                    if (flags.Clang)
                        rings.Call();
                    else
                    {
                        for (double j = 0; j < 360 / (drawangle / 2); j += 2)
                        {
                            double ang1 = (j * (drawangle / 2)) / 180 * Math.PI;
                            double ang2 = ((j + 1) * (drawangle / 2)) / 180 * Math.PI;
                            int q = 0;
                            c.glBegin(GLPrimitiveType.LineStrip);
                            c.glVertex(Math.Cos(ang1), Math.Sin(ang1), 0);
                            c.glVertex(Math.Cos(ang2), Math.Sin(ang2), 0);
                            c.glEnd();
                        }
                    }
                    c.glPopMatrix();
                }
            }
            c.glPopMatrix();
            c.glPopMatrix();
        }
        #endregion

        #region Catch Collision
        public unsafe void RenderCatchCollision(ResourceNode[] bl, GLContext c, Vector3 cam, MParams.DrawStyle style)
        {
            //Coded by Toomai
            //Modified for release v0.67

            if (_event != 0x060A0800 && _event != 0x060A0900 && _event != 0x060A0A00)
                return;

            Event e = EventData;

            int boneindex = (int)e.parameters[1]._data;
            long size = HitboxSize;

            if (boneindex >= 400) // hack to make Kirby and Wario work properly - overalls Wario will not
                boneindex -= 400;
            if (boneindex == 0) // if a hitbox is on TopN, make it follow TransN
            {
                int transindex = 0;
                foreach (MDL0BoneNode bn in bl) // this shouldn't take long; TransN should be within the first 10
                {
                    if (bn.Name.Equals("TransN"))
                        break;
                    transindex++;
                }
                boneindex = transindex;
            }
            MDL0BoneNode b = bl[boneindex] as MDL0BoneNode;
            Vector3 bonepos = b._frameMatrix.GetPoint();
            Vector3 pos = new Vector3(MParams.UnScalar(e.parameters[3]._data), MParams.UnScalar(e.parameters[4]._data), MParams.UnScalar(e.parameters[5]._data));
            Vector3 bonerot = b._frameMatrix.GetAngles();
            Matrix r = b._frameMatrix.GetRotationMatrix();
            Vector3 bonescl = b.RecursiveScale();
            pos._x /= bonescl._x;
            pos._y /= bonescl._y;
            pos._z /= bonescl._z;
            Vector3 globpos = r.Multiply(pos);
            Matrix m = Matrix.TransformMatrix(new Vector3(1), bonerot, globpos + bonepos);
            Vector3 resultpos = new Vector3(m[12], m[13], m[14]);
            m = Matrix.TransformMatrix(new Vector3(MParams.UnScalar(size)), new Vector3(), resultpos);
            c.glPushMatrix();
            c.glMultMatrix((float*)&m);
            int res = 16;
            double drawangle = 360.0 / res;
            // bubble
            if (style == MParams.DrawStyle.SSB64)
            {
                c.glColor(1.0f, 1.0f, 1.0f, 0.25f);
                c.DrawInvertedCube(new Vector3(0, 0, 0), 1.025f);
                c.glColor(1.0f, 0.0f, 0.0f, 0.5f);
                c.DrawCube(new Vector3(0, 0, 0), 0.975f);
            }
            else
            {
                Vector3 typecolour = MParams.getTypeColour(MParams.HitboxType.Throwing);
                c.glColor((typecolour._x / 255.0f), (typecolour._y / 225.0f), (typecolour._z / 255.0f), 0.375f);
                GLDisplayList spheres = c.GetSphereList();
                spheres.Call();
            }
            c.glPopMatrix();
        }
        #endregion

        #region General Collision
        //public unsafe virtual void RenderGeneralCollision(List<MDL0BoneNode> bl, GLContext c, Vector3 cam, DrawStyle style)
        //{
        //    MDL0BoneNode b = bl[0];
        //    Vector3 bonepos = b._frameMatrix.GetPoint();
        //    Vector3 pos = new Vector3(intToScalar(getXPos()), intToScalar(getYPos()), intToScalar(getZPos()));
        //    Vector3 bonerot = b._frameMatrix.GetAngles();
        //    Matrix r = b._frameMatrix.GetRotationMatrix();
        //    Vector3 globpos = r.Multiply(pos);
        //    Matrix m = Matrix.TransformMatrix(new Vector3(1), bonerot, globpos + bonepos);
        //    Vector3 result = new Vector3(m[12], m[13], m[14]);
        //    m = Matrix.TransformMatrix(new Vector3(intToScalar(getSize())), new Vector3(), result);
        //    c.glPushMatrix();
        //    c.glMultMatrix((float*)&m);
        //    int res = 16;
        //    double drawangle = 360.0 / res;
        //    // bubble
        //    Vector3 typecolour = new Vector3(0x7f, 0x7f, 0x7f);
        //    c.glColor((typecolour._x / 255.0f), (typecolour._y / 225.0f), (typecolour._z / 255.0f), 0.375f);
        //    if (style == DrawStyle.SSB64)
        //    {
        //        c.glColor(1.0f, 1.0f, 1.0f, 0.25f);
        //        c.DrawInvertedCube(new Vector3(0, 0, 0), 1.025f);
        //        c.glColor(0.5f, 0.5f, 0.5f, 0.5f);
        //        c.DrawCube(new Vector3(0, 0, 0), 0.975f);
        //    }
        //    else
        //    {
        //        GLDisplayList spheres = c.GetSphereList();
        //        spheres.Call();
        //    }
        //    c.glPopMatrix();
        //}
        #endregion

        #endregion

        #region Events
        public enum EventArg : int //Credit goes to Xiggah for converting the PSA events list to an enum form
        {
            Synchronous_Timer = 0x00010100, //Pause the current flow of events until the set time is reached. Synchronous timers count down when they are reached in the code.
            No_Event = 0x00020000, //No Event.
            Asynchronous_Timer = 0x00020100, //Pause the current flow of events until the set time is reached. Asynchronous Timers start counting from the beginning of the animation.
            Set_Loop = 0x00040100, //Set a loop for X iterations. 
            Execute_Loop = 0x00050000, //Execute the the previously set loop.
            Sub_Routine = 0x00070100, //Enter the event routine specefied and return after ending.
            Return = 0x00080000, //Return from a Subroutine.
            Goto = 0x00090100, //Goto the event location specified and execute.
            If_01 = 0x000A0100, //Start an If block until an Else or an EndIf is reached.
            If_02 = 0x000A0200, //Start an If block until an Else or an EndIf is reached.
            If_03 = 0x000A0400, //Start an If block until an Else or an EndIf is reached.
            Else = 0x000E0000, //Insert an Else block inside an If block.
            And_Comparison = 0x000B0400, //Seems to be an "And" to an If statement.
            Else_If_Comparison = 0x000D0400, //Insert an Else If block inside of an If block.
            End_If = 0x000F0000, //End an If block.
            Switch = 0x00100200, //Begin a multiple case Switch block.
            Case = 0x00110100, //Handler for if the variable in the switch statement equals the specified value.
            Default_Case = 0x00120000, //The case chosen if none of the others are executed.
            End_Switch = 0x00130000, //End a Switch block.
            Loop_Rest = 0x01010000, //Briefly return execution back to the system to prevent crashes during infinite loops.
            Change_Action_01 = 0x02000300, //Change the current action upon the specified requirement being met. (the requirement does not have to be met at the time this code is executed - it can be used anytime after execution.)
            Change_Action_02 = 0x02010200, //Change the current action upon the specified requirement being met. (the requirement does not have to be met at the time this code is executed - it can be used anytime after execution.)
            Change_Action_03 = 0x02010300, //Change the current action upon the specified requirement being met. (the requirement does not have to be met at the time this code is executed - it can be used anytime after execution.)
            Change_Action_04 = 0x02010500, //Change the current action upon the specified requirement being met. (the requirement does not have to be met at the time this code is executed - it can be used anytime after execution.)
            Additional_Requirement_01 = 0x02040100, //Add an additional requirement to the preceeding Change Action statement.
            Additional_Requirement_02 = 0x02040200, //Add an additional requirement to the preceeding Change Action statement.
            Additional_Requirement_03 = 0x02040400, //Add an additional requirement to the preceeding Change Action statement.
            Disable_Other_Status_ID = 0x02060100, //Seems to enable the given Status ID and disable all others.
            Change_SubAction_01 = 0x04000100, //Change the current sub action.
            Change_SubAction_02 = 0x04000200, //Change the current sub action upon completing the specified requirement.
            Reverse_Direction = 0x05000000, //Reverse the direction the character is facing after the animation ends.
            Offensive_Collision = 0x06000D00, //Generate an offensive collision bubble with the specified parameters.
            Terminate_Collisions = 0x06040000, //Remove all currently present collision bubbles
            Body_Collision = 0x06050100, //Change how the character's own collision bubbles act. (primarily used to invoke invulnerability)
            Bone_Collision = 0x06080200, //Sets specific bones to a type of body collision.
            Undo_Bone_Collision = 0x06060100, //Sets bones to their normal collision type.
            Catch_Collision = 0x060A0800, //Generate a grabbing collision bubble with the specified parameters
            Terminate_Catch_Collisions = 0x060D0000, //Remove all currently present grab collision bubbles
            Throw_Attack_Collision = 0x060E1100, //Generate a damage collision bubble used while throwing an opponent.
            Throw_Collision = 0x060F0500, //Generate a damage collision bubble used upon releasing an opponent.
            Special_Offensive_Collision = 0x06150F00, //Generate an offensive collision bubble - is able to achieve unique effects.
            Defensive_Collision = 0x06170300, //Generate a defensive collision bubble that nullifies close combat attacks.
            Defensive_Collison_Operation = 0x06180300, //Undefined: Appears to affect defensive collisions.
            Weapon_Collision = 0x061B0500, //Generate a weapon collision bubble with the specified parameters.
            Thrown_Collision = 0x062B0D00, //Generate a damage collision bubble surrounding the character when thrown. (collision bubble doesn't belong to the character but rather the thrower)
            Sound_Effect_01 = 0x0A000100, //Play a specified sound effect.
            Sound_Effect_02 = 0x0A010100, //Play a specified sound effect.
            Sound_Effect_03 = 0x0A020100, //Play a specified sound effect.
            Victory_Operation = 0x0A050100, //Undefined: Is used during victory poses.
            Other_Sound_Effect_01 = 0x0A090100, //Play a specified sound effect - the values are different from normal sound effects.
            Other_Sound_Effect_02 = 0x0A0A0100, //Play a specified sound effect - the values are different from normal sound effects.
            Stop_Sound_Effect = 0x0A030100, //Stops the specified sound effect immediately.
            Terminate_Instance = 0x0C050000, //Causes the acting instance to terminate(if possible). Will load secondary instance if available.
            Low_Voice_Clip = 0x0C0B0000, //Play a random voice clip from the selection of low voice clips.
            Damage_Voice_Clip = 0x0C190000, //Play a random voice clip from the selection of damage voice clips.
            Ottotto_Voice_Clip = 0x0C1D0000, //Play the voice clip for ottotto
            Frame_Speed_Modifier = 0x04070100, //Dictates the frame speed of the subaction.  Example: setting to 2 makes the animation and timers occur twice as fast.
            Time_Manipulation = 0x0C230200, //Change the speed of time for various parts of the environment. Scalar=time stop, value=slow time. If you use a Scalar in parameter 0, then use parameter 1 to a value set how long time stops. if you use a value in parameter 0, use parameter 1 to set a value how long time slows. using a scalar in parameter 0 and 1 will stop time forever.
            Set_Air_Or_Ground = 0x0E000100, //Specify whether the character is on or off the ground.
            Set_Aerial_Or_Onstage_State = 0x08000100, //Determines the character's state relative to the stage.
            Generate_Article = 0x10000100, //Generate a pre-made prop effect from the prop library. 
            Remove_Article = 0x10030100, //Removes an article.
            Article_Visibility = 0x10050200, //Makes an article visible or invisible.
            Generate_Prop_Effect = 0x100A, //Generate a prop effect with the specified parameters.Needs Discovery.
            Graphic_Effect = 0x11001000, //Generate a generic graphical effect with the specified parameters.
            External_Graphic_Effect_01 = 0x11010A00, //Generate a graphical effect from an external file. (usually the Ef_<character> file)
            External_Graphic_Effect_02 = 0x11020A00, //Generate a graphical effect from an external file. (usually the Ef_<character> file)
            Screen_Tint = 0x11170700, //Tint the screen to the specified color.
            Graphic_Effect_01 = 0x111A1000, //Generate a generic graphical effect with the specified parameters.
            Graphic_Effect_02 = 0x111B1000, //Generate a generic graphical effect with the specified parameters.
            Sword_Glow = 0x11031400, //Creates glow of sword.(Only usable when the proper effects are loaded by their respective characters)
            Terminate_Sword_Glow = 0x11050100, //Eliminates sword glow graphics when set to 1.  May have unknown applications.
            Aesthetic_Wind_Effect = 0x14070A00, //Moves nearby movable model parts (capes, hair, etc) with a wind specified by the parameters.
            Basic_Variable_Set = 0x12000200, //Set a basic variable to the specified value.(Can be used to change modes via damage check and other advanced techniques)
            Basic_Variable_Add = 0x12010200, //Add a specified value to a basic variable.
            Basic_Variable_Subtract = 0x12020200, //Subtract a specified value from a basic variable.
            Float_Variable_Set = 0x12060200, //Set a floating point variable to the specified value.
            Float_Variable_Add = 0x12070200, //Add a specified value to a float variable. (This code is usually used for graphics and as a timer, If you want a character to have a certain effect [be strong, etc] to last for a limited time, then you will use float variable add =+1, along with float variable set=0, the action that you put the code that sets the float varable to =0 will be the subaction to activate the timer or graphic. You will also have to use Float Variable Set [##]=true, and once the timer is set at )
            Float_Variable_Subtract = 0x12080200, //Subtract a specified value from a float variable.
            Bit_Variable_Set = 0x120A0100, //Set a bit variable to true.(Can be used to change modes via taunts and other advanced techniques)
            Bit_Variable_Clear = 0x120B0100, //Set a bit variable to false.(clears Bit variable set)
            Camera_Closeup = 0x1A040500, //Zoom the camera on the character.
            Normal_Camera = 0x1A080000, //Return the camera to its normal setting.
            Pickup_Item_01 = 0x1F000100, //Cause the character to recieve the closest item in range.
            Pickup_Item_02 = 0x1F000200, //Cause the character to recieve the closest item in range.
            Throw_Item_01 = 0x1F010300, //Cause the character to throw the currently held item.
            Drop_Item = 0x1F020000, //Cause the character to drop any currently held item.
            Consume_Item = 0x1F030100, //Cause the character to consume the currently held item.
            Item_Property = 0x1F040200, //Modify a property of the currently held item.
            Rocket_Operation = 0x1F070100, //Undefined: Is used when firing a cracker launcher.
            Generate_Item = 0x1F080100, //Generate an item in the character's hand.
            Weapon_Operation = 0x1F0C0100, //Undefined: Is used in projectile weapon and battering weapon operations.
            Throw_Item_02 = 0x1F0E0500, //Cause the character to throw the currently held item.
            Item_Visibility = 0x1F090100, //Determines visibilty of the currently held item.
            Fire_Weapon = 0x1F050000, //Fires a shot from the currently held item.  (May have other unknown applications)
            Fire_Projectile = 0x1F060100, //Fires a projectile of the specified degree of power.
            Terminate_Flash_Effect = 0x21000000, //Terminate all currently active flash effects.
            Flash_Overlay_Effect = 0x21010400, //Generate a flash overlay effect over the character with the specified colors and opacity.
            Change_Flash_Overlay_Color = 0x21020500, //Change the color of the current flash overlay effect.
            Flash_Light_Effect = 0x21050600, //Generate a flash lighting effect over the character with the specified colors, opacity and angle.
            Undefined = 0x21070500, //Appears to do nothing - is related to the flash effects.
            Allow_Interrupt = 0x64000000, //Allow the current action to be interrupted by another action.
            Allow_Specific_Interrupt = 0x020A0100, //Allows interruption only by specific commands. See parameters for list of possible interrupts.
            Screenshake = 0x1A000100, //Shakes the screen.
            Visibility = 0x0B020100, //Shows whether you're visible or not.
            Rumble = 0x07070200, //Controls the rumble on the controller.
            Character_Momentum = 0x0E080400, //Controls the movement velocity of a moving character.
            Add_Or_Subtract_Momentum = 0x0E010200, //Adds or subtracts speed to the char's current momentum.
            Prevent_Certain_Movements = 0x0E060100, //When set to 2, sideways movement is disallowed.  When set to 1, upward momentum seems to be enhanced.
            Undue_Prevent_Certain_Movements = 0x0E070100, //This should be self-explanatory. 
            Prevent_Vertical_Movement = 0x0E020100, //When set to 1, gravity is disabled and all momentum is set to 0.
            Tag_Display = 0x0C250100, //Disables or enables tag display for the current subaction.
            Super_Or_Heavy_Armor = 0x1E000200, //Begins super armor or heavy armor.  Set both parameters to 0 to end the armor.
            Add_Or_Subtract_Damage = 0x1E030100, //Adds or subtracts the specified amount of damage from the character's current percentage.
            Change_Hitbox_Damage = 0x06010200, //Changes a specific hitbox's damage to the new amount.  Only guaranteed to work on Offensive Collisions.
            Delete_Hitbox = 0x06030100, //Deletes a hitbox of the specified ID.  Only guaranteed to work on Offensive Collisions.
            Model_Changer = 0x0B000200, //Changes the char's model in certain preset ways.  (Examples: sheathe sword, retreat into shell, etc.)
            Model_Changer_2 = 0x0B010200, //Seemingly identical to Model Changer; any differences have yet to be discovered.
            Model_Event_1 = 0x10040100, //This affects an article/model action.(This only works with characters who have articles in their texture or vertext file. For example, mario, ike, pit, etc)
            Model_Event_2 = 0x10040200, //This affects an article/model action.(This only works with characters who have articles in their texture or vertext file. For example, mario, ike, pit, etc)
            Model_Event_3 = 0x10040300, //This affects an article/model action.( This only works with characters who have articles in their texture or vertext file. For example, mario, ike, pit, etc)
            Model_Event_Put_Away = 0x14040100, //Puts away an article/model that was summoned. (This only works with characters who have articles in their texture or vertext file. For example, mario, ike, pit, etc)
            Rumble_Loop = 0x070B0200, //Creates a rumble loop on the controller. 
            Slope_Event_01 = 0x18000100, //Affects a characters posture. (keeps legs pointing twords the ground and can affect mouth and eyes)
            Slope_Event_02 = 0x18010200, //Affects a characters posture. (keeps legs pointing twords the ground and can affect mouth and eyes)
            Article_Event_01 = 0x10000200, //Summons a model found in a FitCharacter.pac file. FOr example, to summon pit's arrow, set it to 1.
            Article_Event_02 = 0x10010100, //Makes the article preform an animation. For example, to make pit shoot his arrow, set it to 1.
            Flow_03 = 0x00030000, //Undefined.
            And = 0x000B0100, //Seems to be an "and" to an If statement.
            And_Value = 0x000B0200, //Seems to be an "and" to an If statement.
            And_Unknown = 0x000B0300, //Seems to be an "and" to an If statement.
            Or = 0x000C0100, //Insert an alternate requirement to fall back on if the above requirement(s) are not met.
            Or_Value = 0x000C0200, //Insert an alternate requirement to fall back on if the above requirement(s) are not met.
            Or_Unknown = 0x000C0300, //Insert an alternate requirement to fall back on if the above requirement(s) are not met.
            Or_Comparison = 0x000C0400, //Insert an alternate requirement to fall back on if the above requirement(s) are not met.
            Else_If_Value = 0x000D0200, //Insert an Else If block inside of an If block.
            Else_If_Unknown = 0x000D0300, //Insert an Else If block inside of an If block.
            Flow_18 = 0x00180000, //Appears within Case statements.
            Change_Action_Status_Value = 0x02000400, //Change the current action upon the specified requirement being met. (the requirement does not have to be met at the time this ID is executed - it can be used anytime after execution.)
            Change_Action_Status_Unknown = 0x02000500, //Change the current action upon the specified requirement being met. (the requirement does not have to be met at the time this ID is executed - it can be used anytime after execution.)
            Change_Action_Status_Comparison = 0x02000600, //Change the current action upon the specified requirement being met. (the requirement does not have to be met at the time this ID is executed - it can be used anytime after execution.)
            Disable_Action_Status_ID = 0x02080100, //Disables the Action associated with the given Status ID.
            Register_Interrupt = 0x02090200, //Appears to register the specified Status ID to the specified Interrupt ID.
            Prevent_Specific_Interrupt = 0x020B0100, //Closes the specific interruption window. Must be set to the same thing as the allow specific interrupt that you wish to cancel.
            Clear_Prevent_Interrupt = 0x020C0100, //Possibly clears a previously created interrupt.
            Subactions_02 = 0x04020100, //Set this requirement to the character's orientation in space: air, gound, or undefined.
            Subactions_02_Value = 0x04020200, //Set this requirement to the character's orientation in space: air, gound, or undefined.
            Subactions_02_Unknown = 0x04020300, //Set this requirement to the character's orientation in space: air, gound, or undefined.
            Subactions_02_Compare = 0x04020400, //Set this requirement to the character's orientation in space: air, gound, or undefined.
            Subactions_03 = 0x04030100, //Subactions.
            Subactions_03_Value = 0x04030200, //Subactions with values.
            Subactions_03_Unknown = 0x04030300, //Subactions, unknown.
            Subactions_03_Compare = 0x04030400, //Subactions compare.
            Subactions_06 = 0x04060100, //unknown.
            Subactions_0A = 0x040A0100, //unknown.
            Subactions_0B = 0x040B0100, //unknown.
            Subactions_0C = 0x040C0100, //unknown.
            Subactions_0D = 0x040D0100, //unknown.
            Subactions_14 = 0x04140100, //unknown.
            Subactions_18 = 0x04180100, //unknown.
            Posture_01 = 0x05010000, //Undefined.
            Posture_02 = 0x05020000, //Undefined.
            Posture_03 = 0x05030000, //Undefined.
            Posture_04 = 0x05040000, //Undefined.
            Posture_07 = 0x05070300, //unknown.
            Posture_0D = 0x050D0100, //unknown.
            Change_Hitbox_Size = 0x06020200, //Untested: Could possibly change a specific hitbox's size to the new amount. Only guaranteed to work on Offensive Collisions if at all.
            Delete_Catch_Collision = 0x060C0100, //Deletes the catch collision with the specified ID.
            Uninteractive_Collision = 0x06101100, //Generates an oblivious hitbox only used to detect collision with other characters.
            Bump_Collision = 0x062C0F00, //Possibly the "bump" collisions that occur when a character in hitstun collides with another body.
            Collisions_2D = 0x062D0000, //Undefined.
            Clear_Buffer = 0x07000000, //Possibly clears the controller buffer.
            Controller_01 = 0x07010000, //Undefined.
            Controller_02 = 0x07020000, //Undefined.
            Controller_06 = 0x07060100, //Undefined.
            Controller_0C = 0x070C0000, //Undefined.
            Edge_Interaction_01 = 0x08010100, //unknown.
            Edge_Interaction_02 = 0x08020100, //unknown.
            Edge_Interaction_04 = 0x08040100, //unknown.
            Edge_Interaction_07 = 0x08070000, //Undefined.
            Module09_00 = 0x09000100, //unknown.
            Character_Specific_01 = 0x0C010000, //Undefined.
            Character_Specific_04 = 0x0C040000, //Undefined.
            Enter_Final_Smash_State = 0x0C060000, //Allows use of Final Smash locked articles, variables, etc. Highly unstable.
            Exit_Final_Smash_State = 0x0C070000, //Undefined.
            Terminate_Self = 0x0C080000, //Used by certain article instances to remove themselves.
            Allow_Or_Disallow_Ledgegrab = 0x0C090100, //Allow or disallow grabbing ledges during the current subaction.
            Character_Specific_0A = 0x0C0A0100, //unknown.
            Character_Specific_13 = 0x0C130000, //Undefined.
            Character_Specific_16 = 0x0C160000, //Undefined.
            Character_Specific_17 = 0x0C170100, //Undefined. Often appears before event.
            Character_Specific_17_Variable = 0x0C170200, //Undefined.
            Character_Specific_1A = 0x0C1A0200, //unknown.
            Character_Specific_1B = 0x0C1B0100, //unknown.
            Character_Specific_1C = 0x0C1C0200, //unknown.
            Character_Specific_1C_Boolean = 0x0C1C0300, //unknown.
            Eating_Voice_Clip = 0x0C1F0000, //Play a random voice clip from the selection of eating voice clips.
            Character_Specific_20 = 0x0C200200, //unknown.
            Character_Specific_24 = 0x0C240100, //unknown.
            Character_Specific_26 = 0x0C260100, //unknown.
            Character_Specific_27 = 0x0C270000, //Undefined. Often appears within Switch statements.
            Character_Specific_29 = 0x0C290000, //Undefined.
            Character_Specific_2B = 0x0C2B0000, //Undefined.
            Concurrent_Infinite_Loop = 0x0D000200, //Runs a subroutine once per frame for the current action.
            Terminate_Concurrent_Infinite_Loop = 0x0D010100, //Seems to stop the execution of a loop created with{{Evt
            Link_03 = 0x0F030200, //unknown.
            Graphics_18 = 0x11180200, //unknown.
            Basic_Variable_Increment = 0x12030100, //Variable++.
            Basic_Variable_Decrement = 0x12040100, //Variable--.
            Float_Variable_Multiply = 0x120F0200, //Multiply a specified value with a float variable.
            Physics_01 = 0x17010000, //Undefined.
            Physics_05 = 0x17050000, //Undefined.
            Module19_01 = 0x19010000, //Undefined.
            Set_Camera_Boundaries = 0x1A030400, //Changes the camera boundaries of your character. Does not reset the camera boundaries; rather, it adds to them. Reverts to normal when the animation ends.
            Detach_Or_Attach_Camera_Close = 0x1A060100, //Causes the camera to follow or stop following a character.
            Camera_09 = 0x1A090000, //Undefined.
            Camera_0A = 0x1A0A0100, //unknown.
            Force_Camera_Control = 0x1A0B0000, //Appears to override any other settings in favor of the character's preference.
            Camera_0C = 0x1A0C0000, //Undefined.
            Obliterate_Held_Item = 0x1F0A0000, //Deletes the item that the character is holding.
            Turn_00 = 0x20000200, //unknown.
            Cancel_01 = 0x64010000, //Undefined.
            Cancel_02 = 0x64020000, //Undefined.
            Aesthetic_Wind_05 = 0x14050100, //Affects wind.
            Article_Visibility_2 = 0x10070200, //Does the same thing as Article Visibility but seems to be less reluctant to work properly.
            Effect_ID = 0x111d0100, //Undefined. 
            Item_11 = 0x1F110100, //Undefined.
            Collisions_13 = 0x0613000, //Undefined.
            Character_Specific_Samus = 0x18030200, //Used in samus.
            Morph_Model_Event = 0x1F0F0100, //If false model will appear else if true model will disappear.(Its found in sonic, bowser, samus, etc) 
            Go_to_Loop_Rest_02 = 0x01020000, //Often follows event.
            Go_to_Loop_Rest_01 = 0x01000000, //Appears to work like event.
            Graphic_Model_Specf = 0x0E0B0200, //Appears to control posture graphics. 
            Character_Spef_GFX_01 = 0x11150300, //Appears to control posture graphics. 
            Character_Spef_GFX_02 = 0x18010300, //Appears to control posture graphics. 
            Physics_Normalize = 0x17000000, //Returns to normal physics.
            Article_Event = 0x10080200, //Article Animation.
            ONLY_Article_Event = 0x10020100, //Article Animaion. 
            Catch_Attack_Collision = 0x060A0900, //Attack Collisions. 
        }
        #endregion
    }
}
