using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.SSBBTypes;
using System.ComponentModel;
using System.IO;
using BrawlLib.IO;
using BrawlLib.Wii.Animations;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.OpenGL;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class MoveDefDataCommonNode : MoveDefEntryNode
    {
        internal CommonMovesetHeader* Header { get { return (CommonMovesetHeader*)WorkingUncompressed.Address; } }

        public List<SpecialOffset> specialOffsets = new List<SpecialOffset>();
        internal uint DataLen;

        [Category("Data Offsets")]
        public int Unk0 { get { return Header->Unknown0; } }
        [Category("Data Offsets")]
        public int Unk1 { get { return Header->Unknown1; } }
        [Category("Data Offsets")]
        public int Unk2 { get { return Header->Unknown2; } }
        [Category("Data Offsets")]
        public int Unk3 { get { return Header->Unknown3; } }
        [Category("Data Offsets")]
        public int Actions1 { get { return Header->ActionsStart; } }
        [Category("Data Offsets")]
        public int Actions2 { get { return Header->Actions2Start; } }
        [Category("Data Offsets")]
        public int Unk6 { get { return Header->Unknown6; } }
        [Category("Data Offsets")]
        public int Unk7 { get { return Header->Unknown7; } }
        [Category("Data Offsets")]
        public int Unk8 { get { return Header->Unknown8; } }
        [Category("Data Offsets")]
        public int Unk9 { get { return Header->Unknown9; } }
        [Category("Data Offsets")]
        public int Unk10 { get { return Header->Unknown10; } }
        [Category("Data Offsets")]
        public int Unk11 { get { return Header->Unknown11; } }
        [Category("Data Offsets")]
        public int Unk12 { get { return Header->Unknown12; } }
        [Category("Data Offsets")]
        public int Unk13 { get { return Header->Unknown13; } }
        [Category("Data Offsets")]
        public int Unk14 { get { return Header->Unknown14; } }
        [Category("Data Offsets")]
        public int Unk15 { get { return Header->Unknown15; } }
        [Category("Data Offsets")]
        public int Unk16 { get { return Header->Unknown16; } }
        [Category("Data Offsets")]
        public int Unk17 { get { return Header->Unknown17; } }
        [Category("Data Offsets")]
        public int Unk18 { get { return Header->Unknown18; } }
        [Category("Data Offsets")]
        public int Unk19 { get { return Header->Unknown19; } }
        [Category("Data Offsets")]
        public int Unk20 { get { return Header->Unknown20; } }
        [Category("Data Offsets")]
        public int Unk21 { get { return Header->Unknown21; } }
        [Category("Data Offsets")]
        public int Unk22 { get { return Header->Unknown22; } }
        [Category("Data Offsets")]
        public int Unk23 { get { return Header->Unknown23; } }
        [Category("Data Offsets")]
        public int Unk24 { get { return Header->Unknown24; } }
        [Category("Data Offsets")]
        public int Unk25 { get { return Header->Unknown25; } }

        [Category("Special Offsets Node")]
        public SpecialOffset[] Offsets { get { return specialOffsets.ToArray(); } }

        public MoveDefDataCommonNode(uint dataLen, string name) { DataLen = dataLen; _name = name; }

        protected override bool OnInitialize()
        {
            base.OnInitialize();
            bint* current = (bint*)Header;
            for (int i = 0; i < 25; i++)
                specialOffsets.Add(new SpecialOffset() { Index = i, Offset = *current++ });
            CalculateDataLen();

            return true;
        }
        public VoidPtr dataHeaderAddr;
        protected override void OnPopulate()
        {
            //unk11
            //unk21 (dual)
            //unk22 
            //list offsets

            #region Populate
            if (ARCNode.SpecialName.Contains(RootNode.Name))
            {
                MoveDefGroupNode g;
                List<int> ActionOffsets;

                MoveDefActionListNode actions = new MoveDefActionListNode() { _name = "Action Scripts", _parent = this };

                bint* actionOffset;

                //Parse offsets first
                for (int i = 4; i < 6; i++)
                {
                    actionOffset = (bint*)(BaseAddress + specialOffsets[i].Offset);
                    ActionOffsets = new List<int>();
                    for (int x = 0; x < specialOffsets[i].Size / 4; x++)
                        ActionOffsets.Add(actionOffset[x]);
                    actions.ActionOffsets.Add(ActionOffsets);
                }

                int r = 0;
                foreach (SpecialOffset s in specialOffsets)
                {
                    if (r != 4 && r != 5)
                        new MoveDefSectionParamNode() { _name = "Unk" + r }.Initialize(this, BaseAddress + s.Offset, 0);
                    //else if (r == 11 || r == 22)
                    //{
                    //    MoveDefListOffsetNode d = new MoveDefListOffsetNode() { _name = "Unk" + r };
                    //    d.Initialize(this, BaseAddress + s.Offset, s.Size);
                    //    int size = Root.GetSize(d.DataOffset);
                    //    for (int x = 0; x < d.Count; x++)
                    //        new MoveDefSectionParamNode().Initialize(d, BaseAddress + d.DataOffset + size * x, size);
                    //}
                    //else if (r == 6 || r == 19 || r == 20)
                    //{
                    //    MoveDefRawDataNode d = new MoveDefRawDataNode("Unk" + r);
                    //    d.Initialize(this, BaseAddress + s.Offset, 0);
                    //    for (int i = 0; i < d.Size; i += 4)
                    //        new MoveDefSectionParamNode() { _name = "Unk" + (i / 4) }.Initialize(d, BaseAddress + *(bint*)(BaseAddress + s.Offset + i), 4);
                    //}
                    r++;
                }

                if (specialOffsets[4].Size != 0 || specialOffsets[5].Size != 0)
                {
                    int count;
                    if (specialOffsets[4].Size == 0)
                        count = specialOffsets[5].Size / 4;
                    else
                        count = specialOffsets[4].Size / 4;

                    //Initialize using first offset so the node is sorted correctly
                    actions.Initialize(this, BaseAddress + specialOffsets[4].Offset, 0);

                    //Set up groups
                    for (int i = 0; i < count; i++)
                        actions.AddChild(new MoveDefActionGroupNode() { _name = "Action" + i }, false);

                    //Add children
                    for (int i = 0; i < 2; i++)
                        if (specialOffsets[i + 4].Size != 0)
                            PopulateActionGroup(actions, actions.ActionOffsets[i], false, i);

                    //Add to children (because the parent was set before initialization)
                    Children.Add(actions);

                    Root._actions = actions;
                }
            }
            #endregion

            SortChildren();
        }

        private void CalculateDataLen()
        {
            List<SpecialOffset> sorted = specialOffsets.OrderBy(x => x.Offset).ToList();
            for (int i = 0; i < sorted.Count; i++)
            {
                if (i < sorted.Count - 1)
                    sorted[i].Size = (int)(sorted[i + 1].Offset - sorted[i].Offset);
                else 
                    sorted[i].Size = (int)(DataLen - sorted[i].Offset);

                //Console.WriteLine(sorted[i].ToString());
            }
        }
        public void PopulateActionGroup(ResourceNode g, List<int> ActionOffsets, bool subactions, int index)
        {
            string innerName = "";
            if (subactions)
                if (index == 0)
                    innerName = "Main";
                else if (index == 1)
                    innerName = "GFX";
                else if (index == 2)
                    innerName = "SFX";
                else if (index == 3)
                    innerName = "Other";
                else return;
            else
                if (index == 0)
                    innerName = "Part 1";
                else if (index == 1)
                    innerName = "Part 2";

            int i = 0;
            foreach (int offset in ActionOffsets)
            {
                //if (i >= g.Children.Count)
                //    if (subactions)
                //        g.Children.Add(new MoveDefSubActionGroupNode() { _name = "Extra" + i, _flags = new AnimationFlags(), _inTransTime = 0, _parent = g });
                //    else
                //        g.Children.Add(new MoveDefGroupNode() { _name = "Extra" + i, _parent = g });

                if (offset > 0)
                    new MoveDefActionNode(innerName, false, g.Children[i]).Initialize(g.Children[i], new DataSource(BaseAddress + offset, 0));
                else
                    g.Children[i].Children.Add(new MoveDefActionNode(innerName, true, g.Children[i]));
                i++;
            }
        }
    }
}