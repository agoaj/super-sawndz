using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.SSBBTypes;
using System.ComponentModel;
using BrawlLib.Imaging;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class SCN0FogNode : SCN0EntryNode
    {
        internal SCN0Fog* Data { get { return (SCN0Fog*)WorkingUncompressed.Address; } }

        private int density;
        private SCN0FogFlags flags;

        private List<SCN0Keyframe> starts, ends;
        private List<RGBAPixel> colors;
        
        [Category("Fog")]
        public SCN0FogFlags Flags { get { return flags; } set { flags = value; SignalPropertyChange(); } }
        [Category("Fog")]
        public int Density { get { return density; } set { density = value; SignalPropertyChange(); } }
        [Category("Fog")]
        public List<SCN0Keyframe> StartPoints { get { return starts; } set { starts = value; SignalPropertyChange(); } }
        [Category("Fog")]
        public List<SCN0Keyframe> EndPoints { get { return ends; } set { ends = value; SignalPropertyChange(); } }
        [Category("Fog")]
        public RGBAPixel[] Colors { get { return colors.ToArray(); } set { colors = value.ToList<RGBAPixel>(); SignalPropertyChange(); } }
        
        protected override bool OnInitialize()
        {
            base.OnInitialize();

            starts = new List<SCN0Keyframe>();
            ends = new List<SCN0Keyframe>();
            colors = new List<RGBAPixel>();

            flags = (SCN0FogFlags)Data->_flags;
            density = Data->_density;
            if (Name != "<null>")
            {
                if (flags.HasFlag(SCN0FogFlags.FixedStart))
                    starts.Add(new Vector3(Data->_start, 0, 0));
                else
                {
                    SCN0KeyframeStruct* addr = Data->startKeyframes->Data;
                    for (int i = 0; i < Data->startKeyframes->_numFrames; i++)
                        starts.Add(*addr++);
                }
                if (flags.HasFlag(SCN0FogFlags.FixedEnd))
                    ends.Add(new Vector3(Data->_end, 0, 0));
                else
                {
                    SCN0KeyframeStruct* addr = Data->endKeyframes->Data;
                    for (int i = 0; i < Data->endKeyframes->_numFrames; i++)
                        ends.Add(*addr++);
                }
                if (flags.HasFlag(SCN0FogFlags.FixedColor))
                    colors.Add(Data->_color);
                else
                {
                    RGBAPixel* addr = Data->colorEntries;
                    for (int i = 0; i <= ((SCN0Node)Parent.Parent).FrameCount; i++)
                        colors.Add(*addr++);
                }
            }

            return false;
        }

        protected override int OnCalculateSize(bool force)
        {
            keyLen = 0;
            lightLen = 0;
            if (starts.Count > 1)
                keyLen += 4 + starts.Count * 12;
            if (ends.Count > 1)
                keyLen += 4 + ends.Count * 12;
            if (colors.Count > 1)
                lightLen += 4 * (((SCN0Node)Parent.Parent).FrameCount + 1);
            return SCN0Fog.Size;
        }

        protected internal override void OnRebuild(VoidPtr address, int length, bool force)
        {
            base.OnRebuild(address, length, force);

            SCN0Fog* header = (SCN0Fog*)address;

            if (colors.Count > 1)
            {
                *((bint*)header->_color.Address) = (int)lightAddr - (int)header->_color.Address;
                for (int i = 0; i <= ((SCN0Node)Parent.Parent).FrameCount; i++)
                    if (i < colors.Count)
                        *lightAddr++ = colors[i];
                    else
                        *lightAddr++ = new RGBAPixel();
                flags &= ~SCN0FogFlags.FixedColor;
            }
            else
            {
                flags |= SCN0FogFlags.FixedColor;
                if (colors.Count == 1)
                    header->_color = colors[0];
                else
                    header->_color = new RGBAPixel();
            }
            if (starts.Count > 1)
            {
                *((bint*)header->_start.Address) = (int)keyframeAddr - (int)header->_start.Address;
                ((SCN0KeyframesHeader*)keyframeAddr)->_numFrames = (ushort)starts.Count;
                SCN0KeyframeStruct* addr = ((SCN0KeyframesHeader*)keyframeAddr)->Data;
                for (int i = 0; i < starts.Count; i++)
                    *addr++ = starts[i];
                keyframeAddr += 4 + starts.Count * 12;
                flags &= ~SCN0FogFlags.FixedStart;
            }
            else
            {
                flags |= SCN0FogFlags.FixedStart;
                if (starts.Count == 1)
                    header->_start = starts[0]._value;
                else
                    header->_start = 0;
            }
            if (ends.Count > 1)
            {
                *((bint*)header->_end.Address) = (int)keyframeAddr - (int)header->_end.Address;
                ((SCN0KeyframesHeader*)keyframeAddr)->_numFrames = (ushort)ends.Count;
                SCN0KeyframeStruct* addr = ((SCN0KeyframesHeader*)keyframeAddr)->Data;
                for (int i = 0; i < ends.Count; i++)
                    *addr++ = ends[i];
                keyframeAddr += 4 + ends.Count * 12;
                flags &= ~SCN0FogFlags.FixedEnd;
            }
            else
            {
                flags |= SCN0FogFlags.FixedEnd;
                if (ends.Count == 1)
                    header->_end = ends[0]._value;
                else
                    header->_end = 0;
            }

            header->_flags = (byte)flags;
            header->_density = density;
        }

        protected internal override void PostProcess(VoidPtr scn0Address, VoidPtr dataAddress, StringTable stringTable)
        {
            base.PostProcess(scn0Address, dataAddress, stringTable);
        }
    }
}
