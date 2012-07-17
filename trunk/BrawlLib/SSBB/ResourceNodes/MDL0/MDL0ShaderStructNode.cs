using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.SSBBTypes;
using System.ComponentModel;
using BrawlLib.Wii.Graphics;
using BrawlLib.IO;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class MDL0ShaderStructNode : MDL0EntryNode
    {
        //Holds an even and an odd stage.

        internal StageGroup* Header { get { return (StageGroup*)_origSource.Address; } }

        public BPMemory command1, command2, command3, command4, command5, command6, command7, command8, command9;

        public bool HasOddStage 
        { 
            get { return color2 && alpha2 && CMD2; } 
            set 
            {
                if (color2 != value && alpha2 != value && CMD2 != value)
                {
                    color2 = alpha2 = CMD2 = value;
                    Check();
                }
            } 
        }

        public bool color2 = false;
        public bool alpha2 = false;
        public bool CMD2 = false;
        
        [Category("Structure Commands"), Browsable(true)]
        public BPMemory Command1 { get { return command1; } }
        [Category("Structure Commands"), Browsable(true)]
        public BPMemory Command2 { get { return command2; } }
        [Category("Structure Commands"), Browsable(true)]
        public BPMemory Command3 { get { return command3; } }
        [Category("Structure Commands"), Browsable(true)]
        public BPMemory Command4 { get { return command4; } }
        [Category("Structure Commands"), Browsable(true)]
        public BPMemory Command5 { get { return command5; } }
        [Category("Structure Commands"), Browsable(true)]
        public BPMemory Command6 { get { return command6; } }
        [Category("Structure Commands"), Browsable(true)]
        public BPMemory Command7 { get { return command7; } }
        [Category("Structure Commands"), Browsable(true)]
        public BPMemory Command8 { get { return command8; } }
        [Category("Structure Commands"), Browsable(true)]
        public BPMemory Command9 { get { return command9; } }

        private void Check() //2nd commands are either all existent or not.
        {
            if (color2)
            {
                command5 = (BPMemory)((int)command4 + 2);
                InsertChild(new TEVColorEnvNode() { Stg = ((int)command5 - 0xC0) / 2 }, true, 3);
            }
            else
            {
                command5 = BPMemory.BPMEM_GENMODE;
                foreach (TEVNode t in Children)
                    if (t._stage == (((int)command4 + 2) - 0xC0) / 2)
                    {
                        t.Remove();
                        break;
                    }
            }

            if (alpha2)
            {
                command7 = (BPMemory)((int)command6 + 2);
                InsertChild(new TEVAlphaEnvNode() { Stg = ((int)command7 - 0xC1) / 2 }, true, 5);
            }
            else
            {
                command7 = BPMemory.BPMEM_GENMODE;
                foreach (TEVNode t in Children)
                    if (t._stage == (((int)command6 + 2) - 0xC1) / 2 )
                    {
                        t.Remove();
                        break;
                    }
            }

            if (CMD2)
            {
                command9 = (BPMemory)((int)command8 + 1);
                InsertChild(new TEVCMDNode() { Stg = ((int)command9 - 0x10) }, true, 7);
            }
            else
            {
                command9 = BPMemory.BPMEM_GENMODE;
                foreach (TEVNode t in Children)
                    if (t._stage == ((int)command8 + 1) - 0x10)
                    {
                        t.Remove();
                        break;
                    }
            }

            foreach (MDL0ShaderStructNode s in Parent.Children)
                s.RecalcStages();
        }

        public void RecalcStages() //Stages need to be modulated closely
        {
            Name = String.Format("Struct{0}", Index);

            MDL0ShaderStructNode prev = null;
            if (Index != 0)
                prev = Parent.Children[Index - 1] as MDL0ShaderStructNode;

            command1 = BPMemory.BPMEM_BP_MASK;

            if (prev != null)
            {
                if (prev.HasOddStage != true)
                    prev.HasOddStage = true;

                command2 = prev.command2 + 1;
                command3 = prev.command3 + 1;

                command4 = (prev.HasOddStage ? prev.command5 : prev.command4) + 2;
                command6 = (prev.HasOddStage ? prev.command7 : prev.command6) + 2;
                command8 = (prev.HasOddStage ? prev.command9 : prev.command8) + 1;

                command5 = HasOddStage ? command4 + 2 : BPMemory.BPMEM_GENMODE;
                command7 = HasOddStage ? command6 + 2 : BPMemory.BPMEM_GENMODE;
                command9 = HasOddStage ? command8 + 1 : BPMemory.BPMEM_GENMODE;
            }
            else //This is structure 0
            {
                command2 = BPMemory.BPMEM_TEV_KSEL0;
                command3 = BPMemory.BPMEM_TREF0;
                command4 = BPMemory.BPMEM_TEV_COLOR_ENV_0;
                command5 = HasOddStage ? BPMemory.BPMEM_TEV_COLOR_ENV_1 : BPMemory.BPMEM_GENMODE;
                command6 = BPMemory.BPMEM_TEV_ALPHA_ENV_0;
                command7 = HasOddStage ? BPMemory.BPMEM_TEV_ALPHA_ENV_1 : BPMemory.BPMEM_GENMODE;
                command8 = BPMemory.BPMEM_IND_CMD0;
                command9 = HasOddStage ? BPMemory.BPMEM_IND_CMD1 : BPMemory.BPMEM_GENMODE;
            }

            if (Index == Parent.Children.Count - 1)
            {
                //This is the last structure
                if (HasOddStage)
                    ((MDL0ShaderNode)Parent).STGs = (byte)(((int)command5 - 0xC0) / 2 + 1);
                else
                    ((MDL0ShaderNode)Parent).STGs = (byte)(((int)command4 - 0xC0) / 2 + 1);
            }

            SignalPropertyChange();
        }

        public void Default()
        {
            Name = String.Format("Struct{0}", Index);

            color2 = alpha2 = CMD2 = false;

            RecalcStages();

            AddChild(new TEVKSelNode() { Stg = ((int)command2 - 0xF6) });
            AddChild(new TEVTRefNode() { Stg = ((int)command3 - 0x28) });
            AddChild(new TEVColorEnvNode() { Stg = ((int)command4 - 0xC0) / 2 });
            AddChild(new TEVAlphaEnvNode() { Stg = ((int)command6 - 0xC1) / 2 });
            AddChild(new TEVCMDNode() { Stg = ((int)command8 - 0x10) });
        }

        public void DefaultAsMetal(int texIndex)
        {
            Name = String.Format("Struct{0}", Index);

            color2 = alpha2 = CMD2 = true;

            RecalcStages();
            Children.Clear();

            if (Index == 0)
            {
                AddChild(new TEVKSelNode() { Stg = ((int)command2 - 0xF6), RawVal = 0xE338C0 });

                TEVTRefNode t = new TEVTRefNode();
                t.Stg = (int)command3 - 0x28;
                t.Texture0MapID = TexMapID.TexMap0 + texIndex;
                t.Texture1MapID = TexMapID.TexMap7;
                t.TextureCoord0 = TexCoordID.TexCoord0 + texIndex;
                t.TextureCoord1 = TexCoordID.TexCoord7;
                t.Texture0Enabled = true;
                t.Texture1Enabled = false;
                t.ColorChannel0 = 0;
                t.ColorChannel1 = (ColorSelChan)1;
                AddChild(t);

                AddChild(new TEVColorEnvNode() { Stg = ((int)command4 - 0xC0) / 2, RawVal = 0x28F8AF });
                AddChild(new TEVColorEnvNode() { Stg = ((int)command5 - 0xC0) / 2, RawVal = 0x08AFF0 });
                AddChild(new TEVAlphaEnvNode() { Stg = ((int)command6 - 0xC1) / 2, RawVal = 0x08F2F0 });
                AddChild(new TEVAlphaEnvNode() { Stg = ((int)command7 - 0xC1) / 2, RawVal = 0x08FF80 });
                AddChild(new TEVCMDNode() { Stg = ((int)command8 - 0x10), RawVal = 0 });
                AddChild(new TEVCMDNode() { Stg = ((int)command9 - 0x10), RawVal = 0 });
            }
            else if (Index == 1)
            {
                AddChild(new TEVKSelNode() { Stg = ((int)command2 - 0xF6), RawVal = 0xE338D0 });
                AddChild(new TEVTRefNode() { Stg = ((int)command3 - 0x28), RawVal = 0x3BF03F });
                AddChild(new TEVColorEnvNode() { Stg = ((int)command4 - 0xC0) / 2, RawVal = 0x08FEB0 });
                AddChild(new TEVColorEnvNode() { Stg = ((int)command5 - 0xC0) / 2, RawVal = 0x0806EF });
                AddChild(new TEVAlphaEnvNode() { Stg = ((int)command6 - 0xC1) / 2, RawVal = 0x081FF0 });
                AddChild(new TEVAlphaEnvNode() { Stg = ((int)command7 - 0xC1) / 2, RawVal = 0x081FF0 });
                AddChild(new TEVCMDNode() { Stg = ((int)command8 - 0x10), RawVal = 0 });
                AddChild(new TEVCMDNode() { Stg = ((int)command9 - 0x10), RawVal = 0 });
            }
            else
            {
                ResourceNode p = Parent;
                Remove();
                foreach (MDL0ShaderStructNode s in p.Children)
                    s.RecalcStages();
            }
        }

        protected override bool OnInitialize()
        {
            StageGroup* header = Header;
            
            _name = String.Format("Struct{0}", Index);

            command1 = header->mask.Mem;
            command2 = header->ksel.Mem;
            command3 = header->tref.Mem;
            command4 = header->eClrEnv.Mem;
            command5 = header->oClrEnv.Mem;
            command6 = header->eAlpEnv.Mem;
            command7 = header->oAlpEnv.Mem;
            command8 = header->eCMD.Mem;
            command9 = header->oCMD.Mem;

            for (int i = 0xC0; i < 0xDF; i += 2)
                if (Header->oClrEnv.Mem == (BPMemory)i)
                { color2 = true; break; }
            for (int i = 0xC1; i < 0xE0; i += 2)
                if (Header->oAlpEnv.Mem == (BPMemory)i)
                { alpha2 = true; break; }
            for (int i = 0x10; i < 0x20; i++)
                if (Header->oCMD.Mem == (BPMemory)i)
                { CMD2 = true; break; }

            Populate();
            return true;
        }

        protected override void OnPopulate()
        {
            if (Header->ksel.Mem != BPMemory.BPMEM_GENMODE)
                new TEVKSelNode() { _stage = ((int)Header->ksel.Mem - 0xF6) }.Initialize(this, Header->ksel.Data.Address, 3);
            if (Header->tref.Mem != BPMemory.BPMEM_GENMODE)
                new TEVTRefNode() { _stage = ((int)Header->tref.Mem - 0x28) }.Initialize(this, Header->tref.Data.Address, 3);
            if (Header->eClrEnv.Mem != BPMemory.BPMEM_GENMODE)
                new TEVColorEnvNode() { _stage = ((int)Header->eClrEnv.Mem - 0xC0) / 2 }.Initialize(this, Header->eClrEnv.Data.Address, 3);
            if (Header->oClrEnv.Mem != BPMemory.BPMEM_GENMODE)
                new TEVColorEnvNode() { _stage = ((int)Header->oClrEnv.Mem - 0xC0) / 2 }.Initialize(this, Header->oClrEnv.Data.Address, 3);
            if (Header->eAlpEnv.Mem != BPMemory.BPMEM_GENMODE)
                new TEVAlphaEnvNode() { _stage = ((int)Header->eAlpEnv.Mem - 0xC1) / 2 }.Initialize(this, Header->eAlpEnv.Data.Address, 3);
            if (Header->oAlpEnv.Mem != BPMemory.BPMEM_GENMODE)
                new TEVAlphaEnvNode() { _stage = ((int)Header->oAlpEnv.Mem - 0xC1) / 2 }.Initialize(this, Header->oAlpEnv.Data.Address, 3);
            if (Header->eCMD.Mem != BPMemory.BPMEM_GENMODE)
                new TEVCMDNode() { _stage = ((int)Header->eCMD.Mem - 0x10) }.Initialize(this, Header->eCMD.Data.Address, 3);
            if (Header->oCMD.Mem != BPMemory.BPMEM_GENMODE)
                new TEVCMDNode() { _stage = ((int)Header->oCMD.Mem - 0x10) }.Initialize(this, Header->oCMD.Data.Address, 3);
        }

        protected override int OnCalculateSize(bool force)
        {
            return 0x30; //Shader Structures are always 0x30 in length!
        }

        protected internal override void OnRebuild(VoidPtr address, int length, bool force)
        {
            StageGroup* structure = (StageGroup*)address;

            *structure = StageGroup.Default;

            structure->mask.Mem = command1;
            structure->ksel.Mem = command2;
            structure->tref.Mem = command3;
            structure->eClrEnv.Mem = command4;
            structure->oClrEnv.Mem = command5;
            structure->eAlpEnv.Mem = command6;
            structure->oAlpEnv.Mem = command7;
            structure->eCMD.Mem = command8;
            structure->oCMD.Mem = command9;

            structure->mask.Reg = 
            structure->ksel.Reg = 
            structure->tref.Reg = 
            structure->eClrEnv.Reg = 
            structure->eAlpEnv.Reg =
            structure->eCMD.Reg = 0x61;

            if (HasOddStage)
            {
                structure->oClrEnv.Reg =
                structure->oAlpEnv.Reg =
                structure->oCMD.Reg = 0x61;
            }
            else
            {
                structure->oClrEnv.Reg =
                structure->oAlpEnv.Reg =
                structure->oCMD.Reg = 0;
            }

            //Command 1 masks the KSel. It will never change
            structure->mask.Data[0] = 0xFF;
            structure->mask.Data[1] = 0xFF;
            structure->mask.Data[2] = 0xF0;

            foreach (TEVNode r in Children)
            {
                if (r is TEVKSelNode)
                    r.Rebuild(structure->ksel.Data.Address, 3, force);
                if (r is TEVTRefNode)
                    r.Rebuild(structure->tref.Data.Address, 3, force);
                if (r is TEVColorEnvNode)
                    if (r._stage % 2 == 0)
                        r.Rebuild(structure->eClrEnv.Data.Address, 3, force);
                    else
                        r.Rebuild(structure->oClrEnv.Data.Address, 3, force);
                if (r is TEVAlphaEnvNode)
                    if (r._stage % 2 == 0)
                        r.Rebuild(structure->eAlpEnv.Data.Address, 3, force);
                    else
                        r.Rebuild(structure->oAlpEnv.Data.Address, 3, force);
                if (r is TEVCMDNode)
                    if (r._stage % 2 == 0)
                        r.Rebuild(structure->eCMD.Data.Address, 3, force);
                    else
                        r.Rebuild(structure->oCMD.Data.Address, 3, force);
            }
        }

        internal override void GetStrings(StringTable table)
        {
            //We DO NOT want to add the name to the string table!
        }
    }
}
