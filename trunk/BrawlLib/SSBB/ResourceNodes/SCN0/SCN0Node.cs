using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.SSBBTypes;
using System.ComponentModel;
using BrawlLib.Imaging;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class SCN0Node : BRESEntryNode
    {
        internal SCN0* Header { get { return (SCN0*)WorkingUncompressed.Address; } }
        public override ResourceType ResourceType { get { return ResourceType.SCN0; } }

        private int _unk1, _unk2, _unk3, _unk4, _unk5, _unk6, _unk7, _unk8, _unk9, _unk10;

        [Category("Scene Data")]
        public int Version { get { return Header->_header._version; } }//set { _version = value; SignalPropertyChange(); } }
        [Category("Scene Data")]
        public int Unknown1 { get { return _unk1; } set { _unk1 = value; SignalPropertyChange(); } }
        [Category("Scene Data")]
        public int FrameCount { get { return _unk2; } set { _unk2 = value; SignalPropertyChange(); } }
        [Category("Scene Data"), TypeConverter(typeof(Bin8StringConverter))]
        public Bin8 Flags { get { return new Bin8((byte)_unk3); } set { _unk3 = value.data; SignalPropertyChange(); } }
        [Category("Scene Data")]
        public int Loop { get { return _unk4; } set { _unk4 = value; SignalPropertyChange(); } }
        [Category("Scene Data")]
        public int LightSetEntries { get { return _unk5; } }//set { _unk5 = value; SignalPropertyChange(); } }
        [Category("Scene Data")]
        public int AmbLightsEntries { get { return _unk6; } }//set { _unk6 = value; SignalPropertyChange(); } }
        [Category("Scene Data")]
        public int LightsEntries { get { return _unk7; } }//set { _unk7 = value; SignalPropertyChange(); } }
        [Category("Scene Data")]
        public int FogsEntries { get { return _unk8; } }//set { _unk8 = value; SignalPropertyChange(); } }
        [Category("Scene Data")]
        public int CamerasEntries { get { return _unk9; } }//set { _unk9 = value; SignalPropertyChange(); } }
        [Category("Scene Data")]
        public int Unknown3 { get { return _unk10; } set { _unk10 = value; SignalPropertyChange(); } }

        protected override bool OnInitialize()
        {
            base.OnInitialize();

            if ((_name == null) && (Header->_stringOffset != 0))
                _name = Header->ResourceString;

            _unk1 = Header->_unk1;
            _unk2 = Header->_frameCount;
            _unk3 = Header->_unk3;
            _unk4 = Header->_unk4;
            _unk5 = Header->_lightSetCount;
            _unk6 = Header->_ambientCount;
            _unk7 = Header->_lightCount;
            _unk8 = Header->_fogCount;
            _unk9 = Header->_cameraCount;
            _unk10 = Header->_unk10;

            return Header->Group->_numEntries > 0 && Version != 5;
        }

        protected override void OnPopulate()
        {
            ResourceGroup* group = Header->Group;
            SCN0GroupNode g;
            for (int i = 0; i < group->_numEntries; i++)
            {
                string name = group->First[i].GetName();
                (g = new SCN0GroupNode(name)).Initialize(this, new DataSource(group->First[i].DataAddress, 0));
                if (name == "LightSet(NW4R)")
                    for (int x = 0; x < Header->_lightSetCount; x++)
                        new SCN0LightSetNode().Initialize(g, new DataSource(&Header->LightSets[x], SCN0LightSet.Size));
                else if (name == "AmbLights(NW4R)")
                    for (int x = 0; x < Header->_ambientCount; x++)
                        new SCN0AmbientLightNode().Initialize(g, new DataSource(&Header->AmbientLights[x], SCN0AmbientLight.Size));
                else if (name == "Lights(NW4R)")
                    for (int x = 0; x < Header->_lightCount; x++)
                        new SCN0LightNode().Initialize(g, new DataSource(&Header->Lights[x], SCN0Light.Size));
                else if (name == "Fogs(NW4R)")
                    for (int x = 0; x < Header->_fogCount; x++)
                        new SCN0FogNode().Initialize(g, new DataSource(&Header->Fogs[x], SCN0Fog.Size));
                else if (name == "Cameras(NW4R)")
                    for (int x = 0; x < Header->_cameraCount; x++)
                        new SCN0CameraNode().Initialize(g, new DataSource(&Header->Cameras[x], SCN0Camera.Size));
            }
        }

        internal override void GetStrings(StringTable table)
        {
            table.Add(Name);
            foreach (SCN0GroupNode n in Children)
                n.GetStrings(table);
        }

        protected override int OnCalculateSize(bool force)
        {
            int size = SCN0.Size + 0x18 + Children.Count * 0x10;
            foreach (SCN0GroupNode n in Children)
                size += n.CalculateSize(true);
            return size;
        }

        protected internal override void OnRebuild(VoidPtr address, int length, bool force)
        {
            int GroupLen = 0, LightSetLen = 0, AmbLightSetLen = 0, LightLen = 0, FogLen = 0, CameraLen = 0;

            SCN0* header = (SCN0*)address;

            header->_unk1 = _unk1;
            header->_frameCount = (short)_unk2;
            header->_unk3 = (short)_unk3;
            header->_unk4 = _unk4;
            header->_unk10 = (short)_unk10;
            header->_dataOffset = SCN0.Size;

            ResourceGroup* group = header->Group;
            *group = new ResourceGroup(Children.Count);

            GroupLen = group->_totalSize;

            ResourceEntry* entry = group->First;
            VoidPtr groupAddress = group->EndAddress;
            VoidPtr entryAddress = groupAddress;

            foreach (SCN0GroupNode g in Children)
                entryAddress += g._groupLen;

            VoidPtr keyframeAddress = entryAddress;
            foreach (SCN0GroupNode g in Children)
                foreach (SCN0EntryNode e in g.Children)
                {
                    e.scn0Addr = header;
                    keyframeAddress += e._length;
                }

            VoidPtr lightArrayAddress = keyframeAddress;
            foreach (SCN0GroupNode g in Children)
                foreach (SCN0EntryNode e in g.Children)
                    lightArrayAddress += e.keyLen;

            header->_lightSetCount = 0;
            header->_ambientCount = 0;
            header->_lightCount = 0;
            header->_fogCount = 0;
            header->_cameraCount = 0;

            foreach (SCN0GroupNode g in Children)
            {
                if (g._name == "LightSet(NW4R)")
                {
                    LightSetLen = g._entryLen;
                    header->_lightSetCount = (short)g.Children.Count;
                }
                else if (g._name == "AmbLights(NW4R)")
                {
                    AmbLightSetLen = g._entryLen;
                    header->_ambientCount = (short)g.Children.Count;
                }
                else if (g._name == "Lights(NW4R)")
                {
                    LightLen = g._entryLen;
                    header->_lightCount = (short)g.Children.Count;
                }
                else if (g._name == "Fogs(NW4R)")
                {
                    FogLen = g._entryLen;
                    header->_fogCount = (short)g.Children.Count;
                }
                else if (g._name == "Cameras(NW4R)")
                {
                    CameraLen = g._entryLen;
                    header->_cameraCount = (short)g.Children.Count;
                }

                (entry++)->_dataOffset = (int)groupAddress - (int)group;

                g._dataAddr = entryAddress;
                g.keyframeAddress = keyframeAddress;
                g.lightArrayAddress = lightArrayAddress;

                g.Rebuild(groupAddress, g._groupLen, true);

                groupAddress += g._groupLen;
                GroupLen += g._groupLen;
                entryAddress += g._entryLen;
                keyframeAddress += g.keyLen;
                lightArrayAddress += g.lightLen;
            }

            header->Set(GroupLen, LightSetLen, AmbLightSetLen, LightLen, FogLen, CameraLen);
        }

        protected internal override void PostProcess(VoidPtr bresAddress, VoidPtr dataAddress, int dataLength, StringTable stringTable)
        {
            base.PostProcess(bresAddress, dataAddress, dataLength, stringTable);

            SCN0* header = (SCN0*)dataAddress;
            header->ResourceStringAddress = stringTable[Name] + 4;

            ResourceGroup* group = header->Group;
            group->_first = new ResourceEntry(0xFFFF, 0, 0, 0, 0);

            ResourceEntry* rEntry = group->First;

            int index = 1;
            foreach (SCN0GroupNode n in Children)
            {
                dataAddress = (VoidPtr)group + (rEntry++)->_dataOffset;
                ResourceEntry.Build(group, index++, dataAddress, (BRESString*)stringTable[n.Name]);
                n.PostProcess(header, dataAddress, stringTable);
            }
        }

        internal static ResourceNode TryParse(DataSource source) { return ((SCN0*)source.Address)->_header._tag == SCN0.Tag ? new SCN0Node() : null; }
    }
}