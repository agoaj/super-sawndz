using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.SSBBTypes;
using System.ComponentModel;
using BrawlLib.Imaging;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class SCN0LightNode : SCN0EntryNode
    {
        internal SCN0Light* Data { get { return (SCN0Light*)WorkingUncompressed.Address; } }

        public override ResourceType ResourceType { get { return ResourceType.Unknown; } }

        internal int _unk1, _unk2, _unk5, _unk6, _unk9;
        internal Bin16 _flags1, _flags2;
        internal float _unk7, _unk8, _unk10, _unk12;
        private List<RGBAPixel> _lighting1, _lighting2;
        private List<SCN0Keyframe> xStarts, yStarts, zStarts, xEnds, yEnds, zEnds;

        [Flags]
        public enum Flags1e : ushort
        {
            U1 = 0x1,
            U2 = 0x2,
            U3 = 0x4,
            EPtXFix = 0x8,
            EPtYFix = 0x10,
            EPtZFix = 0x20,
            FixL1 = 0x40,
            U4 = 0x80,
            SPtXFix = 0x100,
            SPtYFix = 0x200,
            SPtZFix = 0x400,
            U5 = 0x800,
            U6 = 0x1000,
            U7 = 0x2000,
            FixL2 = 0x4000,
            U8 = 0x8000
        }

        [Flags]
        public enum Flags2e : ushort
        {
            NoUnk6_7_8 = 0x1,
            UseUnk10 = 0x2,
            NotProgram = 0x4,
            UseUnk1_11_12 = 0x8,
            U2 = 0x10,
            U3 = 0x20,
            U4 = 0x40,
            U5 = 0x80
        }

        /*
            Flags 2
            0000 0000 0000 0001 - Do not use Unk6 - 8
            0000 0000 0000 0010 - Use Unk10 
            0000 0000 0000 0100 - Is Not Program (or not referenced by all lightsets)
            0000 0000 0000 1000 - Use Unk1, Unk11, Unk12
            0000 0000 0001 0000 - Enabled
            0000 0000 0010 0000 - UseLightSet

            Flags 1
            0000 0000 0000 0111 - Unknown 1 (3)
            0000 0000 0011 1000 - EndPoints fixed in order, bits right to left
            0000 0000 0100 0000 - Fixed Lighting 1
            0000 0000 1000 0000 - Do not use Unk5
            0000 0111 0000 0000 - StartPoints fixed in order, bits right to left
            0011 1000 0000 0000 - Unknown 2 (3) 
            0100 0000 0000 0000 - Fixed Lighting 2
            1000 0000 0000 0000 - Unknown 3
         */

        [Category("Light")]
        public int Unknown1 { get { return _unk1; } set { _unk1 = value; SignalPropertyChange(); } }
        [Category("Light")]
        public int Unknown2 { get { return _unk2; } set { _unk2 = value; SignalPropertyChange(); } }
        [Category("Light"), TypeConverter(typeof(Bin16StringConverter))]
        public Bin16 Flags1 { get { return _flags1; } set { _flags1 = value; SignalPropertyChange(); } }
        [Category("Light"), TypeConverter(typeof(Bin16StringConverter))]
        public Bin16 Flags2 { get { return _flags2; } set { _flags2 = value; SignalPropertyChange(); } }
        [Category("Light")]
        public int Unknown5 { get { return _unk5; } set { _unk5 = value; SignalPropertyChange(); } }
        [Category("Light")]
        public List<SCN0Keyframe> XEndPoints { get { return xEnds; } set { xEnds = value; SignalPropertyChange(); } }
        [Category("Light")]
        public List<SCN0Keyframe> YEndPoints { get { return yEnds; } set { yEnds = value; SignalPropertyChange(); } }
        [Category("Light")]
        public List<SCN0Keyframe> ZEndPoints { get { return zEnds; } set { zEnds = value; SignalPropertyChange(); } }
        [Category("Light")]
        public RGBAPixel[] Lighting { get { return _lighting1.ToArray(); } set { _lighting1 = value.ToList<RGBAPixel>(); SignalPropertyChange(); } }
        [Category("Light")]
        public List<SCN0Keyframe> XStartPoints { get { return xStarts; } set { xStarts = value; SignalPropertyChange(); } }
        [Category("Light")]
        public List<SCN0Keyframe> YStartPoints { get { return yStarts; } set { yStarts = value; SignalPropertyChange(); } }
        [Category("Light")]
        public List<SCN0Keyframe> ZStartPoints { get { return zStarts; } set { zStarts = value; SignalPropertyChange(); } }
        [Category("Light")]
        public int Unknown6 { get { return _unk6; } set { _unk6 = value; SignalPropertyChange(); } }
        [Category("Light")]
        public float Brightness { get { return _unk7; } set { _unk7 = value; SignalPropertyChange(); } }
        [Category("Light")]
        public float Intensity { get { return _unk8; } set { _unk8 = value; SignalPropertyChange(); } }
        [Category("Light")]
        public int Unknown9 { get { return _unk9; } set { _unk9 = value; SignalPropertyChange(); } }
        [Category("Light")]
        public float LightSize { get { return _unk10; } set { _unk10 = value; SignalPropertyChange(); } }
        [Category("Light")]
        public RGBAPixel[] Lighting2 { get { return _lighting2.ToArray(); } set { _lighting2 = value.ToList<RGBAPixel>(); SignalPropertyChange(); } }
        [Category("Light")]
        public float Lighting2ConstantAlpha { get { return _unk12; } set { _unk12 = value; SignalPropertyChange(); } }
        
        protected override bool OnInitialize()
        {
            base.OnInitialize();

            _unk1 = Data->_unk1;
            _unk2 = Data->_unk2;
            _flags1 = new Bin16(Data->_flags1);
            _flags2 = new Bin16(Data->_flags2);
            _unk5 = Data->_unk5;
            _unk6 = Data->_unk6;
            _unk7 = Data->_unk7;
            _unk8 = Data->_unk8;
            _unk9 = Data->_unk9;
            _unk10 = Data->_unk10;
            _unk12 = Data->_unk12;

            _lighting1 = new List<RGBAPixel>();
            _lighting2 = new List<RGBAPixel>();
            xEnds = new List<SCN0Keyframe>();
            yEnds = new List<SCN0Keyframe>();
            zEnds = new List<SCN0Keyframe>();
            xStarts = new List<SCN0Keyframe>();
            yStarts = new List<SCN0Keyframe>();
            zStarts = new List<SCN0Keyframe>();

            if (Flags1[3])
                xEnds.Add(new Vector3(0, 0, Data->_vec1._x));
            else
            {
                if (Name != "<null>")
                {
                    SCN0KeyframesHeader* keysHeader = Data->xEndKeyframes;
                    SCN0KeyframeStruct* addr = keysHeader->Data;
                    for (int i = 0; i < keysHeader->_numFrames; i++)
                        xEnds.Add(*addr++);
                }
            }
            if (Flags1[4])
                yEnds.Add(new Vector3(0, 0, Data->_vec1._y));
            else
            {
                if (Name != "<null>")
                {
                    SCN0KeyframesHeader* keysHeader = Data->yEndKeyframes;
                    SCN0KeyframeStruct* addr = keysHeader->Data;
                    for (int i = 0; i < keysHeader->_numFrames; i++)
                        yEnds.Add(*addr++);
                }
            }
            if (Flags1[5])
                zEnds.Add(new Vector3(0, 0, Data->_vec1._z));
            else
            {
                if (Name != "<null>")
                {
                    SCN0KeyframesHeader* keysHeader = Data->zEndKeyframes;
                    SCN0KeyframeStruct* addr = keysHeader->Data;
                    for (int i = 0; i < keysHeader->_numFrames; i++)
                        zEnds.Add(*addr++);
                }
            }
            if (Flags1[6])
                _lighting1.Add(Data->_lighting1);
            else
            {
                if (Name != "<null>")
                {
                    RGBAPixel* addr = Data->light1Entries;
                    for (int i = 0; i <= ((SCN0Node)Parent.Parent).FrameCount; i++)
                        _lighting1.Add(*addr++);
                }
            }
            if (Flags1[7])
                xStarts.Add(new Vector3(0, 0, Data->_vec2._x));
            else
            {
                if (Name != "<null>")
                {
                    SCN0KeyframesHeader* keysHeader = Data->xStartKeyframes;
                    SCN0KeyframeStruct* addr = keysHeader->Data;
                    for (int i = 0; i < keysHeader->_numFrames; i++)
                        xStarts.Add(*addr++);
                }
            }
            if (Flags1[8])
                yStarts.Add(new Vector3(0, 0, Data->_vec2._y));
            else
            {
                if (Name != "<null>")
                {
                    SCN0KeyframesHeader* keysHeader = Data->yStartKeyframes;
                    SCN0KeyframeStruct* addr = keysHeader->Data;
                    for (int i = 0; i < keysHeader->_numFrames; i++)
                        yStarts.Add(*addr++);
                }
            }
            if (Flags1[9])
                zStarts.Add(new Vector3(0, 0, Data->_vec2._z));
            else
            {
                if (Name != "<null>")
                {
                    SCN0KeyframesHeader* keysHeader = Data->zStartKeyframes;
                    SCN0KeyframeStruct* addr = keysHeader->Data;
                    for (int i = 0; i < keysHeader->_numFrames; i++)
                        zStarts.Add(*addr++);
                }
            }
            if (Flags1[14])
                _lighting2.Add(Data->_lighting2);
            else
            {
                if (Name != "<null>")
                {
                    RGBAPixel* addr = Data->light2Entries;
                    for (int i = 0; i <= ((SCN0Node)Parent.Parent).FrameCount; i++)
                        _lighting2.Add(*addr++);
                }
            }
            return false;
        }

        protected override int OnCalculateSize(bool force)
        {
            lightLen = 0;
            keyLen = 0;
            if (_name != "<null>")
            {
                if (_lighting1.Count > 1)
                    lightLen += 4 * (((SCN0Node)Parent.Parent).FrameCount + 1);
                if (_lighting2.Count > 1)
                    lightLen += 4 * (((SCN0Node)Parent.Parent).FrameCount + 1);
                if (xStarts.Count > 1)
                    keyLen += 4 + xStarts.Count * 12;
                if (yStarts.Count > 1)
                    keyLen += 4 + yStarts.Count * 12;
                if (zStarts.Count > 1)
                    keyLen += 4 + zStarts.Count * 12;
                if (xEnds.Count > 1)
                    keyLen += 4 + xEnds.Count * 12;
                if (yEnds.Count > 1)
                    keyLen += 4 + yEnds.Count * 12;
                if (zEnds.Count > 1)
                    keyLen += 4 + zEnds.Count * 12;
            }
            return SCN0Light.Size;
        }

        protected internal override void OnRebuild(VoidPtr address, int length, bool force)
        {
            SCN0Light* header = (SCN0Light*)address;

            base.OnRebuild(address, length, force);

            if (_name != "<null>")
            {
                header->_unk1 = _unk1;
                header->_unk2 = _unk2;
                header->_unk5 = _unk5;
                header->_unk6 = _unk6;
                header->_unk7 = _unk7;
                header->_unk8 = _unk8;
                header->_unk9 = _unk9;
                header->_unk10 = _unk10;
                header->_unk12 = _unk12;

                if (_lighting1.Count > 1)
                {
                    *((bint*)header->_lighting1.Address) = (int)lightAddr - (int)header->_lighting1.Address;
                    for (int i = 0; i <= ((SCN0Node)Parent.Parent).FrameCount; i++)
                        if (i < _lighting1.Count)
                            *lightAddr++ = _lighting1[i];
                        else
                            *lightAddr++ = new RGBAPixel();
                    _flags1[6] = false;
                }
                else
                {
                    _flags1[6] = true;
                    if (_lighting1.Count == 1)
                        header->_lighting1 = _lighting1[0];
                    else
                        header->_lighting1 = new RGBAPixel();
                }

                if (_lighting2.Count > 1)
                {
                    *((bint*)header->_lighting2.Address) = (int)lightAddr - (int)header->_lighting2.Address;
                    for (int i = 0; i <= ((SCN0Node)Parent.Parent).FrameCount; i++)
                        if (i < _lighting2.Count)
                            *lightAddr++ = _lighting2[i];
                        else
                            *lightAddr++ = new RGBAPixel();
                    _flags1[14] = false;
                }
                else
                {
                    _flags1[14] = true;
                    if (_lighting2.Count == 1)
                        header->_lighting2 = _lighting2[0];
                    else
                        header->_lighting2 = new RGBAPixel();
                }
                if (xEnds.Count > 1)
                {
                    *((bint*)header->_vec1._x.Address) = (int)keyframeAddr - (int)header->_vec1._x.Address;
                    ((SCN0KeyframesHeader*)keyframeAddr)->_numFrames = (ushort)xEnds.Count;
                    SCN0KeyframeStruct* addr = ((SCN0KeyframesHeader*)keyframeAddr)->Data;
                    for (int i = 0; i < xEnds.Count; i++)
                        *addr++ = xEnds[i];
                    keyframeAddr += 4 + xEnds.Count * 12;
                    _flags1[3] = false;
                }
                else
                {
                    _flags1[3] = true;
                    if (xEnds.Count == 1)
                        header->_vec1._x = xEnds[0]._value;
                    else
                        header->_vec1._x = 0;
                }
                if (yEnds.Count > 1)
                {
                    *((bint*)header->_vec1._y.Address) = (int)keyframeAddr - (int)header->_vec1._y.Address;
                    ((SCN0KeyframesHeader*)keyframeAddr)->_numFrames = (ushort)yEnds.Count;
                    SCN0KeyframeStruct* addr = ((SCN0KeyframesHeader*)keyframeAddr)->Data;
                    for (int i = 0; i < yEnds.Count; i++)
                        *addr++ = yEnds[i];
                    keyframeAddr += 4 + yEnds.Count * 12;
                    _flags1[4] = false;
                }
                else
                {
                    _flags1[4] = true;
                    if (yEnds.Count == 1)
                        header->_vec1._y = yEnds[0]._value;
                    else
                        header->_vec1._y = 0;
                }
                if (zEnds.Count > 1)
                {
                    *((bint*)header->_vec1._z.Address) = (int)keyframeAddr - (int)header->_vec1._z.Address;
                    ((SCN0KeyframesHeader*)keyframeAddr)->_numFrames = (ushort)zEnds.Count;
                    SCN0KeyframeStruct* addr = ((SCN0KeyframesHeader*)keyframeAddr)->Data;
                    for (int i = 0; i < zEnds.Count; i++)
                        *addr++ = zEnds[i];
                    keyframeAddr += 4 + zEnds.Count * 12;
                    _flags1[5] = false;
                }
                else
                {
                    _flags1[5] = true;
                    if (zEnds.Count == 1)
                        header->_vec1._z = zEnds[0]._value;
                    else
                        header->_vec1._z = 0;
                }
                if (xStarts.Count > 1)
                {
                    *((bint*)header->_vec2._x.Address) = (int)keyframeAddr - (int)header->_vec2._x.Address;
                    ((SCN0KeyframesHeader*)keyframeAddr)->_numFrames = (ushort)xStarts.Count;
                    SCN0KeyframeStruct* addr = ((SCN0KeyframesHeader*)keyframeAddr)->Data;
                    for (int i = 0; i < xStarts.Count; i++)
                        *addr++ = xStarts[i];
                    keyframeAddr += 4 + xStarts.Count * 12;
                    _flags1[7] = false;
                }
                else
                {
                    _flags1[7] = true;
                    if (xStarts.Count == 1)
                        header->_vec2._x = xStarts[0]._value;
                    else
                        header->_vec2._x = 0;
                }
                if (yStarts.Count > 1)
                {
                    *((bint*)header->_vec2._y.Address) = (int)keyframeAddr - (int)header->_vec2._y.Address;
                    ((SCN0KeyframesHeader*)keyframeAddr)->_numFrames = (ushort)yStarts.Count;
                    SCN0KeyframeStruct* addr = ((SCN0KeyframesHeader*)keyframeAddr)->Data;
                    for (int i = 0; i < yStarts.Count; i++)
                        *addr++ = yStarts[i];
                    keyframeAddr += 4 + yStarts.Count * 12;
                    _flags1[8] = false;
                }
                else
                {
                    _flags1[8] = true;
                    if (yStarts.Count == 1)
                        header->_vec2._y = yStarts[0]._value;
                    else
                        header->_vec2._y = 0;
                }
                if (zStarts.Count > 1)
                {
                    *((bint*)header->_vec2._z.Address) = (int)keyframeAddr - (int)header->_vec2._z.Address;
                    ((SCN0KeyframesHeader*)keyframeAddr)->_numFrames = (ushort)zStarts.Count;
                    SCN0KeyframeStruct* addr = ((SCN0KeyframesHeader*)keyframeAddr)->Data;
                    for (int i = 0; i < zStarts.Count; i++)
                        *addr++ = zStarts[i];
                    keyframeAddr += 4 + zStarts.Count * 12;
                    _flags1[9] = false;
                }
                else
                {
                    _flags1[9] = true;
                    if (zStarts.Count == 1)
                        header->_vec2._z = zStarts[0]._value;
                    else
                        header->_vec2._z = 0;
                }

                header->_flags1 = _flags1.data;
                header->_flags2 = _flags2.data;
            }
        }

        protected internal override void PostProcess(VoidPtr scn0Address, VoidPtr dataAddress, StringTable stringTable)
        {
            base.PostProcess(scn0Address, dataAddress, stringTable);
        }
    }
}
