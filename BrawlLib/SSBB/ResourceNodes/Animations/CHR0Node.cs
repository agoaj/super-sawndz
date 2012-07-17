using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.SSBBTypes;
using System.ComponentModel;
using System.IO;
using BrawlLib.IO;
using BrawlLib.Wii.Animations;
using System.Windows.Forms;
using BrawlBox;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class CHR0Node : BRESEntryNode
    {
        internal BRESCommonHeader* Header { get { return (BRESCommonHeader*)WorkingUncompressed.Address; } }
        internal CHR0v4_3* Header4_3 { get { return (CHR0v4_3*)WorkingUncompressed.Address; } }
        internal CHR0v5* Header5 { get { return (CHR0v5*)WorkingUncompressed.Address; } }

        public override ResourceType ResourceType { get { return ResourceType.CHR0; } }

        [Browsable(false)]
        public override int tFrameCount { get { return FrameCount; } set { FrameCount = value; } }
        [Browsable(false)]
        public override bool tLoop { get { return Loop; } set { Loop = value; } }

        internal int _numFrames = 1;
        internal int _stringoffset, _dataoffset, _loop;
        internal int _version;

        public int ConversionBias = 0;
        public int startUpVersion = 0;

        [Category("Animation Data")]
        public int Version
        {
            get { return _version; }
            set
            {
                if (_version == value)
                    return;

                if (value == startUpVersion)
                    ConversionBias = 0;
                else if (startUpVersion == 4 && value == 5)
                    ConversionBias = 1;
                else if (startUpVersion == 5 && value == 4)
                    ConversionBias = -1;

                _version = value;
                SignalPropertyChange();
            }
        }
        [Category("Animation Data")]
        public int FrameCount
        {
            get { return _numFrames + (startUpVersion == 5 ? 1 : 0); }
            set
            {
                int bias = (startUpVersion == 5 ? 1 : 0);
                if ((_numFrames == value - bias) || (value - bias < (1 - bias)))
                    return;

                _numFrames = value - bias;
                
                foreach (CHR0EntryNode n in Children)
                    n.SetSize(FrameCount);

                SignalPropertyChange();
            }
        }
        [Category("Animation Data")]
        public bool Loop { get { return _loop != 0; } set { _loop = (ushort)(value ? 1 : 0); SignalPropertyChange(); } }
        
        public CHR0EntryNode CreateEntry() { return CreateEntry(null); }
        public CHR0EntryNode CreateEntry(string name)
        {
            CHR0EntryNode n = new CHR0EntryNode();
            n._numFrames = _numFrames;
            n._name = this.FindName(name);
            AddChild(n);
            return n;
        }

        public void InsertKeyframe(int index)
        {
            FrameCount++;
            foreach (CHR0EntryNode c in Children)
                c.Keyframes.Insert(KeyFrameMode.All, index);
        }
        public void DeleteKeyframe(int index)
        {
            foreach (CHR0EntryNode c in Children)
                c.Keyframes.Delete(KeyFrameMode.All, index);
            FrameCount--;
        }
        public int num;
        public bool IsPorted = false;
        protected override bool OnInitialize()
        {
            base.OnInitialize();

            startUpVersion = _version = Header->_version;

            if (_version == 5)
            {
                CHR0v5* header = Header5;
                _numFrames = header->_numFrames;
                _loop = header->_loop;

                _dataoffset = header->_dataOffset;
                _stringoffset = header->_stringOffset;

                if (_name == null) 
                    if (Header5->ResourceString != null)
                        _name = Header5->ResourceString;
                    else
                        _name = "anim" + Index;

                return Header5->Group->_numEntries > 0;
            }
            else
            {
                CHR0v4_3* header = Header4_3;
                _numFrames = header->_numFrames;
                _loop = header->_loop;
                _dataoffset = header->_dataOffset;
                _stringoffset = header->_stringOffset;

                if (_name == null)
                    if (Header4_3->ResourceString != null)
                        _name = Header4_3->ResourceString;
                    else
                        _name = "anim" + Index;

                return Header4_3->Group->_numEntries > 0;
            }
        }

        protected override void OnPopulate()
        {
            ResourceGroup* group = Header4_3->Group;
            for (int i = 0; i < group->_numEntries; i++)
                new CHR0EntryNode().Initialize(this, new DataSource(group->First[i].DataAddress, 0));
        }

        public void MergeWith(CHR0Node external)
        {
            if (external.FrameCount != FrameCount && MessageBox.Show(null, "Frame counts are not equal; the shorter animation will end early. Do you still wish to continue?", "", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            if (external.FrameCount > FrameCount)
                FrameCount = external.FrameCount;

            foreach (CHR0EntryNode _extTarget in external.Children)
            {
                CHR0EntryNode node = null;
                KeyframeEntry kfe = null;

                CHR0EntryNode entry = new CHR0EntryNode() { Name = _extTarget.Name };
                entry._numFrames = _extTarget.FrameCount;

                //Apply all external keyframes to current entry.
                for (int x = 0; x < _extTarget.FrameCount; x++)
                    for (int i = 0x10; i < 0x19; i++)
                        if ((kfe = _extTarget.GetKeyframe((KeyFrameMode)i, x)) != null)
                            entry.Keyframes.SetFrameValue((KeyFrameMode)i, x, kfe._value)._tangent = kfe._tangent;

                if ((node = FindChild(_extTarget.Name, false) as CHR0EntryNode) == null)
                    AddChild(entry, true);
                else
                {
                    DialogResult result = MessageBox.Show(null, "A bone entry with the name " + _extTarget.Name + " already exists.\nDo you want to rename this entry?\nOtherwise, you will have the option to merge the keyframes.", "Rename Entry?", MessageBoxButtons.YesNoCancel);
                    if (result == DialogResult.Yes)
                    {
                    Top:
                        RenameDialog d = new RenameDialog();
                        if (d.ShowDialog(null, entry) == DialogResult.OK)
                        {
                            if (entry.Name != _extTarget.Name)
                                AddChild(entry, true);
                            else
                            {
                                MessageBox.Show("The name wasn't changed!");
                                goto Top;
                            }
                        }
                    }
                    else if (result == DialogResult.No)
                    {
                        result = MessageBox.Show(null, "Do you want to merge the keyframes of the entries?", "Merge Keyframes?", MessageBoxButtons.YesNoCancel);
                        if (result == DialogResult.Yes)
                        {
                            KeyframeEntry kfe2 = null;

                            if (_extTarget.FrameCount > node.FrameCount)
                                node._numFrames = _extTarget.FrameCount;

                            //Merge all external keyframes with the current entry.
                            for (int x = 0; x < _extTarget.FrameCount; x++)
                                for (int i = 0x10; i < 0x19; i++)
                                    if ((kfe = _extTarget.GetKeyframe((KeyFrameMode)i, x)) != null)
                                        if ((kfe2 = node.GetKeyframe((KeyFrameMode)i, x)) == null)
                                            node.SetKeyframe((KeyFrameMode)i, x, kfe._value);
                                        else
                                        {
                                            result = MessageBox.Show(null, "A keyframe at frame " + x + " already exists.\nOld value: " + kfe2._value + "\nNew value:" + kfe._value + "\nReplace the old value with the new one?", "Replace Keyframe?", MessageBoxButtons.YesNoCancel);
                                            if (result == DialogResult.Yes)
                                                node.SetKeyframe((KeyFrameMode)i, x, kfe._value);
                                            else if (result == DialogResult.Cancel)
                                            {
                                                Restore();
                                                return;
                                            }
                                        }
                        }
                        else if (result == DialogResult.Cancel)
                        {
                            Restore();
                            return;
                        }
                    }
                    else if (result == DialogResult.Cancel)
                    {
                        Restore();
                        return;
                    }
                }
            }
        }

        public void Append(CHR0Node external)
        {
            KeyframeEntry kfe;

            int oldCount = FrameCount;
            FrameCount += external.FrameCount;

            foreach (CHR0EntryNode _target in external.Children)
            {
                CHR0EntryNode node = null;
                if ((node = (CHR0EntryNode)FindChild(_target.Name, false)) == null)
                {
                    CHR0EntryNode newNode = new CHR0EntryNode() { Name = _target.Name };
                    newNode._numFrames = _target.FrameCount + oldCount;
                    for (int x = 0; x < _target.FrameCount; x++)
                        for (int i = 0x10; i < 0x19; i++)
                            if ((kfe = _target.GetKeyframe((KeyFrameMode)i, x)) != null)
                                newNode.Keyframes.SetFrameValue((KeyFrameMode)i, x, kfe._value)._tangent = kfe._tangent;
                    AddChild(newNode);
                }
                else
                {
                    node._numFrames += oldCount;
                    for (int x = 0; x < _target.FrameCount; x++)
                        for (int i = 0x10; i < 0x19; i++)
                            if ((kfe = _target.GetKeyframe((KeyFrameMode)i, x)) != null)
                                node.Keyframes.SetFrameValue((KeyFrameMode)i, x, kfe._value)._tangent = kfe._tangent;
                }
            }
        }

        internal void Port(MDL0Node _targetModel, MDL0Node _extModel)
        {
            MDL0BoneNode extBone;
            MDL0BoneNode bone;
            KeyframeEntry kfe;
            float difference = 0;
            foreach (CHR0EntryNode _target in Children)
            {
                extBone = (MDL0BoneNode)_extModel.FindChild(_target.Name, true); //Get external model bone
                bone = (MDL0BoneNode)_targetModel.FindChild(_target.Name, true); //Get target model bone

                for (int x = 0; x < _target.FrameCount; x++) //Loop thru each frame
                    for (int i = 0x13; i < 0x19; i++) //Loop thru trans
                        if ((kfe = _target.GetKeyframe((KeyFrameMode)i, x)) != null) //Check for a keyframe
                        {
                            if (bone != null && extBone != null)
                                switch (i)
                                {
                                    //Translations
                                    case 0x16: //Trans X
                                        if (Math.Round(kfe._value, 4) == Math.Round(extBone._bindState.Translate._x, 4))
                                            kfe._value = bone._bindState.Translate._x;
                                        else if (bone._bindState.Translate._x < extBone._bindState.Translate._x)
                                            kfe._value -= extBone._bindState.Translate._x - bone._bindState.Translate._x;
                                        else if (bone._bindState.Translate._x > extBone._bindState.Translate._x)
                                            kfe._value += bone._bindState.Translate._x - extBone._bindState.Translate._x;
                                        break;
                                    case 0x17: //Trans Y
                                        if (Math.Round(kfe._value, 4) == Math.Round(extBone._bindState.Translate._y, 4))
                                            kfe._value = bone._bindState.Translate._y;
                                        else if (bone._bindState.Translate._y < extBone._bindState.Translate._y)
                                            kfe._value -= extBone._bindState.Translate._y - bone._bindState.Translate._y;
                                        else if (bone._bindState.Translate._y > extBone._bindState.Translate._y)
                                            kfe._value += bone._bindState.Translate._y - extBone._bindState.Translate._y;
                                        break;
                                    case 0x18: //Trans Z
                                        if (Math.Round(kfe._value, 4) == Math.Round(extBone._bindState.Translate._z, 4))
                                            kfe._value = bone._bindState.Translate._z;
                                        else if (bone._bindState.Translate._z < extBone._bindState.Translate._z)
                                            kfe._value -= extBone._bindState.Translate._z - bone._bindState.Translate._z;
                                        else if (bone._bindState.Translate._z > extBone._bindState.Translate._z)
                                            kfe._value += bone._bindState.Translate._z - extBone._bindState.Translate._z;
                                        break;

                                    //Rotations
                                    case 0x13: //Rot X
                                        difference = bone._bindState.Rotate._x - extBone._bindState.Rotate._x;
                                        kfe._value += difference;
                                        if (difference != 0)
                                            FixChildren(bone, 0);
                                        break;
                                    case 0x14: //Rot Y
                                        difference = bone._bindState.Rotate._y - extBone._bindState.Rotate._y;
                                        kfe._value += difference;
                                        if (difference != 0)
                                            FixChildren(bone, 1);
                                        break;
                                    case 0x15: //Rot Z
                                        difference = bone._bindState.Rotate._z - extBone._bindState.Rotate._z;
                                        kfe._value += difference;
                                        if (difference != 0)
                                            FixChildren(bone, 2);
                                        break;
                                }
                            if (kfe._value == float.NaN || kfe._value == float.PositiveInfinity || kfe._value == float.NegativeInfinity)
                            { 
                                kfe.Remove();
                                _target.Keyframes._keyCounts[i]--;
                            }
                        }
            }
            _changed = true;
            IsPorted = true;
        }

        private void FixChildren(MDL0BoneNode node, int axis)
        {
            KeyframeEntry kfe;
            foreach (MDL0BoneNode b in node.Children)
            {
                CHR0EntryNode _target = (CHR0EntryNode)FindChild(b.Name, true);
                if (_target != null)
                switch (axis)
                {
                    case 0: //X, correct Y and Z
                        for (int l = 0; l < _target.FrameCount; l++)
                            for (int g = 0x13; g < 0x16; g++)
                                if (g != 0x13)
                                    if ((kfe = _target.GetKeyframe((KeyFrameMode)g, l)) != null)
                                        kfe._value *= -1;
                        break;
                    case 1: //Y, correct X and Z
                        for (int l = 0; l < _target.FrameCount; l++)
                            for (int g = 0x13; g < 0x16; g++)
                                if (g != 0x14)
                                    if ((kfe = _target.GetKeyframe((KeyFrameMode)g, l)) != null)
                                        kfe._value *= -1;
                        break;
                    case 2: //Z, correct X and Y
                        for (int l = 0; l < _target.FrameCount; l++)
                            for (int g = 0x13; g < 0x16; g++)
                                if (g != 0x15)
                                    if ((kfe = _target.GetKeyframe((KeyFrameMode)g, l)) != null)
                                        kfe._value *= -1;
                        break;
                }
                FixChildren(b, axis);
            }
        }

        internal override void GetStrings(StringTable table)
        {
            table.Add(Name);
            foreach (CHR0EntryNode n in Children)
                table.Add(n.Name);
        }

        protected override int OnCalculateSize(bool force)
        {
            int size = (_version == 5 ? CHR0v5.Size : CHR0v4_3.Size) + 0x18 + (Children.Count * 0x10);
            foreach (CHR0EntryNode n in Children)
                size += n.CalculateSize(true);
            return size;
        }

        public static CHR0Node FromFile(string path)
        {
            //string ext = Path.GetExtension(path);
            if (path.EndsWith(".chr0", StringComparison.OrdinalIgnoreCase))
                return NodeFactory.FromFile(null, path) as CHR0Node;
            if (path.EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
                return CHR0TextImporter.Convert(path);
            //if (path.EndsWith(".bvh", StringComparison.OrdinalIgnoreCase))
            //    return BVH.Import(path);
            //if (path.EndsWith(".vmd", StringComparison.OrdinalIgnoreCase))
            //    return PMDModel.ImportVMD(path);

            throw new NotSupportedException("The file extension specified is not of a supported animation type.");
        }

        protected internal override void OnRebuild(VoidPtr address, int length, bool force)
        {
            ResourceGroup* group;
            if (_version == 5)
            {
                CHR0v5* header = (CHR0v5*)address;
                *header = new CHR0v5(_version, length, _numFrames - ConversionBias, Children.Count, _loop);
                group = header->Group;
            }
            else
            {
                CHR0v4_3* header = (CHR0v4_3*)address;
                *header = new CHR0v4_3(_version, length, _numFrames - ConversionBias, Children.Count, _loop);
                group = header->Group;
            }

            *group = new ResourceGroup(Children.Count);

            VoidPtr entryAddress = group->EndAddress;
            VoidPtr dataAddress = entryAddress;

            foreach (CHR0EntryNode n in Children)
                dataAddress += n._entryLen;

            ResourceEntry* rEntry = group->First;
            foreach (CHR0EntryNode n in Children)
            {
                (rEntry++)->_dataOffset = (int)entryAddress - (int)group;

                n._dataAddr = dataAddress;
                n.Rebuild(entryAddress, n._entryLen, true);
                entryAddress += n._entryLen;
                dataAddress += n._dataLen;
            }
        }

        protected internal override void PostProcess(VoidPtr bresAddress, VoidPtr dataAddress, int dataLength, StringTable stringTable)
        {
            base.PostProcess(bresAddress, dataAddress, dataLength, stringTable);

            ResourceGroup* group;
            if (_version == 5)
            {
                CHR0v5* header = (CHR0v5*)dataAddress;
                header->_stringOffset = (int)stringTable[Name] + 4 - (int)dataAddress;
                group = header->Group;
            }
            else
            {
                CHR0v4_3* header = (CHR0v4_3*)dataAddress;
                header->_stringOffset = (int)stringTable[Name] + 4 - (int)dataAddress;
                group = header->Group;
            }

            group->_first = new ResourceEntry(0xFFFF, 0, 0, 0, 0);
            ResourceEntry* rEntry = group->First;

            int index = 1;
            foreach (CHR0EntryNode n in Children)
            {
                dataAddress = (VoidPtr)group + (rEntry++)->_dataOffset;
                ResourceEntry.Build(group, index++, dataAddress, (BRESString*)stringTable[n.Name]);
                n.PostProcess(dataAddress, stringTable);
            }
        }

        internal static ResourceNode TryParse(DataSource source) { return ((BRESCommonHeader*)source.Address)->_tag == CHR0v4_3.Tag ? new CHR0Node() : null; }
    }

    public unsafe class CHR0EntryNode : ResourceNode
    {
        internal CHR0Entry* Header { get { return (CHR0Entry*)WorkingUncompressed.Address; } }
        public override ResourceType ResourceType { get { return ResourceType.CHR0Entry; } }

        internal int _numFrames;
        [Browsable(false)]
        public int FrameCount { get { return _numFrames; } }

        internal KeyframeCollection _keyframes;
        [Browsable(false)]
        public KeyframeCollection Keyframes 
        { 
            get 
            {
                if (_keyframes == null)
                {
                    if (Header != null)
                        _keyframes = AnimationConverter.DecodeCHR0Keyframes(Header, FrameCount);
                    else
                        _keyframes = new KeyframeCollection(FrameCount);
                }
                return _keyframes;
            } 
        }

#if DEBUG
        public AnimationCode Code { get { if (Header != null) return Header->Code; else return 0; } }
#endif

        internal int _dataLen;
        internal int _entryLen;
        internal VoidPtr _dataAddr;
        protected override int OnCalculateSize(bool force)
        {
            //Keyframes.Clean();
            _dataLen = AnimationConverter.CalculateCHR0Size(Keyframes, out _entryLen);
            return _dataLen + _entryLen;
        }

        protected override bool OnInitialize()
        {
            _keyframes = null;

            if (_parent is CHR0Node)
                _numFrames = ((CHR0Node)_parent).FrameCount;

            if ((_name == null) && (Header->_stringOffset != 0))
                _name = Header->ResourceString;

            return false;
        }

        public override unsafe void Export(string outPath)
        {
            StringTable table = new StringTable();
            table.Add(_name);

            int dataLen = OnCalculateSize(true);
            int totalLen = dataLen + table.GetTotalSize();

            using (FileStream stream = new FileStream(outPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None, 8, FileOptions.RandomAccess))
            {
                stream.SetLength(totalLen);
                using (FileMap map = FileMap.FromStream(stream))
                {
                    AnimationConverter.EncodeCHR0Keyframes(Keyframes, map.Address, map.Address + _entryLen);
                    table.WriteTable(map.Address + dataLen);
                    PostProcess(map.Address, table);
                }
            }
        }

        protected internal override void OnRebuild(VoidPtr address, int length, bool force)
        {
            AnimationConverter.EncodeCHR0Keyframes(_keyframes, address, _dataAddr);
        }

        protected internal virtual void PostProcess(VoidPtr dataAddress, StringTable stringTable)
        {
            CHR0Entry* header = (CHR0Entry*)dataAddress;
            header->ResourceStringAddress = stringTable[Name] + 4;
        }

        internal void SetSize(int count)
        {
            if (_keyframes != null)
                Keyframes.FrameLimit = count;

            _numFrames = count;
            SignalPropertyChange();
        }
        public KeyframeEntry GetKeyframe(KeyFrameMode mode, int index) { return Keyframes.GetKeyframe(mode, index); }
        public void SetKeyframe(KeyFrameMode mode, int index, float value)
        {
            KeyframeEntry k = Keyframes.SetFrameValue(mode, index, value);
            k.GenerateTangent();
            k._prev.GenerateTangent();
            k._next.GenerateTangent();

            SignalPropertyChange();
        }
        public void SetKeyframe(int index, AnimationFrame frame)
        {
            float* v = (float*)&frame;
            for (int i = 0x10; i < 0x19; i++)
                SetKeyframe((KeyFrameMode)i, index, *v++);
        }

        public void SetKeyframeOnlyTrans(int index, AnimationFrame frame)
        {
            float* v = (float*)&frame.Translation;
            for (int i = 0x16; i < 0x19; i++)
                SetKeyframe((KeyFrameMode)i, index, *v++);
        }

        public void SetKeyframeOnlyRot(int index, AnimationFrame frame)
        {
            float* v = (float*)&frame.Rotation;
            for (int i = 0x13; i < 0x16; i++)
                SetKeyframe((KeyFrameMode)i, index, *v++);
        }

        public void SetKeyframeOnlyScale(int index, AnimationFrame frame)
        {
            float* v = (float*)&frame.Scale;
            for (int i = 0x10; i < 0x13; i++)
                SetKeyframe((KeyFrameMode)i, index, *v++);
        }

        public void SetKeyframeOnlyTrans(int index, Vector3 trans)
        {
            float* v = (float*)&trans;
            for (int i = 0x16; i < 0x19; i++)
                SetKeyframe((KeyFrameMode)i, index, *v++);
        }

        public void SetKeyframeOnlyRot(int index, Vector3 rot)
        {
            float* v = (float*)&rot;
            for (int i = 0x13; i < 0x16; i++)
                SetKeyframe((KeyFrameMode)i, index, *v++);
        }

        public void SetKeyframeOnlyScale(int index, Vector3 scale)
        {
            float* v = (float*)&scale;
            for (int i = 0x10; i < 0x13; i++)
                SetKeyframe((KeyFrameMode)i, index, *v++);
        }

        public void RemoveKeyframe(KeyFrameMode mode, int index)
        {
            KeyframeEntry k = Keyframes.Remove(mode, index);
            if (k != null)
            {
                k._prev.GenerateTangent();
                k._next.GenerateTangent();
                SignalPropertyChange();
            }
        }

        public void RemoveKeyframe(int index)
        {
            for (int i = 0x10; i < 0x19; i++)
                RemoveKeyframe((KeyFrameMode)i, index);
        }

        public void RemoveKeyframeOnlyTrans(int index)
        {
            for (int i = 0x16; i < 0x19; i++)
                RemoveKeyframe((KeyFrameMode)i, index);
        }

        public void RemoveKeyframeOnlyRot(int index)
        {
            for (int i = 0x13; i < 0x16; i++)
                RemoveKeyframe((KeyFrameMode)i, index);
        }

        public void RemoveKeyframeOnlyScale(int index)
        {
            for (int i = 0x10; i < 0x13; i++)
                RemoveKeyframe((KeyFrameMode)i, index);
        }

        public AnimationFrame GetAnimFrame(int index)
        {
            return Keyframes.GetFullFrame(index);
        }
    }
}
