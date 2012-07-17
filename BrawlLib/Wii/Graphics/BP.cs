using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace BrawlLib.Wii.Graphics
{
    [StructLayout( LayoutKind.Sequential, Pack=1)]
    public unsafe struct BPCommand
    {
        public BPCommand(bool enabled)
        {
            Reg = (byte)(enabled ? 0x61 : 0);
            Mem = BPMemory.BPMEM_GENMODE;
            Data = (Int24)0;
        }

        public byte Reg; //0x61
        public BPMemory Mem;
        public Int24 Data;
    }
    
    //Not reversed, can be used directly
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct AlphaFunction
    {
        public static readonly AlphaFunction Default = new AlphaFunction() { dat = 0x3F };
        //0000 0000 0000 0000 1111 1111   ref0
        //0000 0000 1111 1111 0000 0000   ref1
        //0000 0111 0000 0000 0000 0000   comp0
        //0011 1000 0000 0000 0000 0000   comp1
        //1100 0000 0000 0000 0000 0000   logic

        public byte dat;
        public byte ref1;
        public byte ref0;

        public AlphaCompare Comp0 { get { return (AlphaCompare)(dat & 7); } set { dat = (byte)((dat & 0xF8) | ((int)value & 7)); } }
        public AlphaCompare Comp1 { get { return (AlphaCompare)((dat >> 3) & 7); } set { dat = (byte)((dat & 0xC7) | (((int)value & 7) << 3)); } }
        public AlphaOp Logic { get { return (AlphaOp)((dat >> 6) & 3); } set { dat = (byte)((dat & 0x3F) | (((int)value & 3) << 6)); } }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ZMode
    {
        public static readonly ZMode Default = new ZMode() { data = 0x17 };

        public byte pad0, pad1, data;

        public bool EnableDepthTest { get { return (data & 1) != 0; } set { data = (byte)((data & 0xFE) | (value ? 1 : 0)); } }
        public bool EnableDepthUpdate { get { return (data & 0x10) != 0; } set { data = (byte)((data & 0xEF) | (value ? 0x10 : 0));} }
        public GXCompare DepthFunction { get { return (GXCompare)((data >> 1) & 7); } set { data = (byte)((data & 0xF1) | ((int)value << 1)); } }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct BlendMode
    {
        public static readonly BlendMode Default = new BlendMode() { dat1 = 0xA0, dat2 = 0x34 };

        //0000 0000 0000 0001    EnableBlend
        //0000 0000 0000 0010    EnableLogic
        //0000 0000 0000 0100    EnableDither
        //0000 0000 0000 1000    UpdateColor
        //0000 0000 0001 0000    UpdateAlpha
        //0000 0000 1110 0000    DstFactor
        //0000 0111 0000 0000    SrcFactor
        //0000 1000 0000 0000    Subtract
        //1111 0000 0000 0000    LogicOp

        public byte pad, dat2, dat1;

        public bool EnableBlend { get { return (dat1 & 1) != 0; } set { dat1 = (byte)((dat1 & 0xFE) | (value ? 1 : 0)); } }
        public bool EnableLogicOp { get { return (dat1 & 2) != 0; } set { dat1 = (byte)((dat1 & 0xFD) | (value ? 2 : 0)); } }
        public bool EnableDither { get { return (dat1 & 4) != 0; } set { dat1 = (byte)((dat1 & 0xFB) | (value ? 4 : 0)); } }
        public bool EnableColorUpdate { get { return (dat1 & 8) != 0; } set { dat1 = (byte)((dat1 & 0xF7) | (value ? 8 : 0)); } }
        public bool EnableAlphaUpdate { get { return (dat1 & 0x10) != 0; } set { dat1 = (byte)((dat1 & 0xEF) | (value ? 0x10 : 0)); } }
        public BlendFactor DstFactor { get { return (BlendFactor)(dat1 >> 5); } set { dat1 = (byte)((dat1 & 0x1F) | ((int)value << 5)); } }
        public BlendFactor SrcFactor { get { return (BlendFactor)(dat2 & 7); } set { dat2 = (byte)((dat2 & 0xF8) | (int)value); } }
        public bool Subtract { get { return (dat2 & 8) != 0; } set { dat2 = (byte)((dat2 & 0xF7) | (value ? 8 : 0)); } }
        public GXLogicOp LogicOp { get { return (GXLogicOp)(dat2 >> 4); } set { dat2 = (byte)((dat2 & 0xF) | ((int)value << 4)); } }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ColorEnv
    {
        //0000 0000 0000 0000 0000 1111   SelD
        //0000 0000 0000 0000 1111 0000   SelC
        //0000 0000 0000 1111 0000 0000   SelB
        //0000 0000 1111 0000 0000 0000   SelA
        //0000 0011 0000 0000 0000 0000   Bias
        //0000 0100 0000 0000 0000 0000   Sub
        //0000 1000 0000 0000 0000 0000   Clamp
        //0011 0000 0000 0000 0000 0000   Shift
        //1100 0000 0000 0000 0000 0000   Dest

        public byte _dat0, _dat1, _dat2;

        public int SelD { get { return (int)(_dat2 & 0xF); } set { _dat2 = (byte)((_dat2 & 0xF0) | ((int)value & 0xF)); } }
        public int SelC { get { return (int)((_dat2 >> 4) & 0xF); } set { _dat2 = (byte)((_dat2 & 0xF) | ((int)value & 0xF) << 4); } }
        public int SelB { get { return (int)(_dat1 & 0xF); } set { _dat1 = (byte)((_dat1 & 0xF0) | ((int)value & 0xF)); } }
        public int SelA { get { return (int)((_dat1 >> 4) & 0xF); } set { _dat1 = (byte)((_dat1 & 0xF) | ((int)value & 0xF) << 4); } }
        public int Bias { get { return (int)(_dat0 & 3); } set { _dat0 = (byte)((_dat0 & 0xFC) | ((int)value & 3)); } }

        public bool Sub { get { return ((_dat0 >> 2) & 1) != 0; } set { _dat0 = (byte)((_dat0 & (0xFB)) | (value ? 4 : 0)); } }
        public bool Clamp { get { return ((_dat0 >> 3) & 1) != 0; } set { _dat0 = (byte)((_dat0 & (0xF7)) | (value ? 8 : 0)); } }

        public int Shift { get { return (int)((_dat0 >> 4) & 3); } set { _dat0 = (byte)((_dat0 & 0x30) | (((int)value & 3) << 4)); } }
        public int Dest { get { return (int)((_dat0 >> 6) & 3); } set { _dat0 = (byte)((_dat0 & 0x3F) | (((int)value & 3) << 6)); } }

        public ColorEnv(byte dat0, byte dat1, byte dat2)
        { _dat0 = dat0; _dat1 = dat1; _dat2 = dat2; }

        public ColorEnv(int value)
        {
            _dat2 = (byte)((value) & 0xFF);
            _dat1 = (byte)((value >> 8) & 0xFF);
            _dat0 = (byte)((value >> 16) & 0xFF);
        }

        public static int Shiftv(int seld, int selc, int selb, int sela, int bias, int sub, int clamp, int shift, int dest)
        {
            return (seld) |
            ((selc) << 4) |
            ((selb) << 8) |
            ((sela) << 12) |
            ((bias) << 16) |
            ((sub) << 18) |
            ((clamp) << 19) |
            ((shift) << 20) |
            ((dest) << 22);
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct AlphaEnv
    {
        //0000 0000 0000 0000 0000 0011   RSwap
        //0000 0000 0000 0000 0000 1100   TSwap
        //0000 0000 0000 0000 0111 0000   SelD
        //0000 0000 0000 0011 1000 0000   SelC
        //0000 0000 0001 1100 0000 0000   SelB
        //0000 0000 1110 0000 0000 0000   SelA
        //0000 0011 0000 0000 0000 0000   Bias
        //0000 0100 0000 0000 0000 0000   Sub
        //0000 1000 0000 0000 0000 0000   Clamp
        //0011 0000 0000 0000 0000 0000   Shift
        //1100 0000 0000 0000 0000 0000   Dest

        public byte _dat0, _dat1, _dat2;

        public int RSwap { get { return (int)(_dat2 & 3); } set { _dat2 = (byte)((_dat2 & 0xFC) | (value & 3)); } }
        public int TSwap { get { return (int)((_dat2 >> 2) & 3); } set { _dat2 = (byte)((_dat2 & 0xF3) | (value & 0xC)); } }
        public int SelD { get { return (int)((_dat2 >> 4) & 7); } set { _dat2 = (byte)((_dat2 & 0x8F) | (value & 0x70)); } }
        public int SelC { get { return (int)((((_dat1 << 1) & 6) | (_dat2 >> 7) & 1) & 7); } set { _dat1 = (byte)((_dat1 & 0xFC) | (value & 3)); _dat2 = (byte)((_dat2 & 0xFE) | ((int)value & 1)); } }
        public int SelB { get { return (int)((_dat1 >> 2) & 7); } set { _dat1 = (byte)((_dat1 & 0xF0) | (value & 0xF)); } }
        public int SelA { get { return (int)((_dat1 >> 5) & 7); } set { _dat1 = (byte)((_dat1 & 0x1F) | (value & 0xE0)); } }
        public int Bias { get { return (int)(_dat0 & 3); } set { _dat0 = (byte)((_dat0 & 0xFC) | (value & 3)); } }
        public bool Sub { get { return (_dat0 & 4) != 0; } set { _dat0 = (byte)((_dat0 & (0xFB)) | (value ? 4 : 0)); } }
        public bool Clamp { get { return (_dat0 & 8) != 0; } set { _dat0 = (byte)((_dat0 & (0xF7)) | (value ? 8 : 0)); } }
        public int Shift { get { return (int)((_dat0 >> 4) & 3); } set { _dat0 = (byte)((_dat0 & 0x30) | ((value & 3) << 4)); } }
        public int Dest { get { return (int)((_dat0 >> 6) & 3); } set { _dat0 = (byte)((_dat0 & 0x3F) | ((value & 3) << 6)); } }
    
        public AlphaEnv(byte dat0, byte dat1, byte dat2)
        { _dat0 = dat0; _dat1 = dat1; _dat2 = dat2; }

        public AlphaEnv(int value)
        {
            _dat2 = (byte)((value) & 0xFF);
            _dat1 = (byte)((value >> 8) & 0xFF);
            _dat0 = (byte)((value >> 16) & 0xFF);
        }

        public static int Shiftv(int rswap, int tswap, int seld, int selc, int selb, int sela, int bias, int sub, int clamp, int shift, int dest)
        {
            return (rswap) |
            ((tswap) << 2) |
            ((seld) << 4) |
            ((selc )<< 7) |
            ((selb )<< 10) |
            ((sela) << 13) |
            ((bias) << 16) |
            ((sub) << 18) |
            ((clamp) << 19) |
            ((shift) << 20) |
            ((dest) << 22);
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct RAS1_IRef //For indirect textures
    {
        //0000 0000 0000 0000 0000 0111   BI0
        //0000 0000 0000 0000 0011 1000   BC0
        //0000 0000 0000 0001 1100 0000   BI1
        //0000 0000 0000 1110 0000 0000   BC1
        //0000 0000 0111 0000 0000 0000   BI2
        //0000 0011 1000 0000 0000 0000   BC2
        //0001 1100 0000 0000 0000 0000   BI3
        //1110 0000 0000 0000 0000 0000   BC3

        public Int24 data;
        
        public int TexMap0 { get { return (int)data & 7; } }
        public int TexCoord0 { get { return ((int)data >> 3) & 7; } }
        public int TexMap1 { get { return ((int)data >> 6) & 7; } }
        public int TexCoord1 { get { return ((int)data >> 9) & 7; } }
        public int TexMap2 { get { return ((int)data >> 12) & 7; } }
        public int TexCoord2 { get { return ((int)data >> 15) & 7; } }
        public int TexMap3 { get { return ((int)data >> 18) & 7; } }
        public int TexCoord3 { get { return ((int)data >> 21) & 7; } }
        
        public RAS1_IRef(byte dat0, byte dat1, byte dat2) { data._dat0 = dat0; data._dat1 = dat1; data._dat2 = dat2; }
        public RAS1_IRef(int value) { data = (Int24)value; }
        public RAS1_IRef(Int24 value) { data = value; }
        
        public static int Shift(int bi0, int bc0, int bi1, int bc1, int bi2, int bc2, int bi3, int bc3)
        {
            return (bi0) |
            ((bc0) << 3) |
            ((bi1) << 6) |
            ((bc1) << 9) |
            ((bi2) << 12) |
            ((bc2) << 15) |
            ((bi3) << 18) |
            ((bc3) << 21);
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct RAS1_TRef //For direct textures
    {
        //0000 0000 0000 0000 0000 0111   TI0
        //0000 0000 0000 0000 0011 1000   TC0
        //0000 0000 0000 0000 0100 0000   TE0
        //0000 0000 0000 0011 1000 0000   CC0
        //0000 0000 0000 1100 0000 0000   PAD0
        //0000 0000 0111 0000 0000 0000   TI1
        //0000 0011 1000 0000 0000 0000   TC1
        //0000 0100 0000 0000 0000 0000   TE1
        //0011 1000 0000 0000 0000 0000   CC1
        //1100 0000 0000 0000 0000 0000   PAD1

        public byte _dat0, _dat1, _dat2;

        public int TI0 { get { return (int)(_dat2 & 7); } set { _dat2 = (byte)((_dat2 & 0xF8) | (value & 7)); } }
        public int TC0 { get { return (int)((_dat2 >> 3) & 7); } set { _dat2 = (byte)((_dat2 & 0xC7) | ((value & 7) << 3)); } }
        public bool TE0 { get { return (_dat2 >> 6) != 0; } set { _dat2 = (byte)((_dat2 & (0xBF)) | ((value ? 1 : 0) << 6)); } }
        public int CC0 { get { return (int)(((_dat1 << 1 & 6) | (_dat2 >> 7 & 1)) & 7); } set { _dat1 = (byte)((_dat1 & 0xFC) | (value & 3)); _dat2 = (byte)((_dat2 & 0xFE) | ((int)value & 1)); } }
        public int Pad0 { get { return (int)((_dat1 >> 2) & 3); } set { _dat1 = (byte)((_dat1 & 0xF3) | ((value & 3) << 2)); } }
        public int TI1 { get { return (int)((_dat1 >> 4) & 7); } set { _dat1 = (byte)((_dat1 & 0x8F) | ((value & 7) << 4)); } }
        public int TC1 { get { return (int)(((_dat0 << 1 & 6) | (_dat1 >> 7 & 1))); } set { _dat0 = (byte)((_dat0 & 0xFC) | (value & 3)); _dat1 = (byte)((_dat1 & 0xFE) | ((int)value & 1)); } }
        public bool TE1 { get { return (_dat0 & 4) != 0; } set { _dat0 = (byte)((_dat0 & (0xFB)) | (value ? 4 : 0)); } }
        public int CC1 { get { return (int)((_dat0 >> 3) & 7); } set { _dat0 = (byte)((_dat0 & 0xC7) | ((value & 7) << 3)); } }
        public int Pad1 { get { return (int)((_dat0 >> 6) & 3); } set { _dat0 = (byte)((_dat0 & 0x3F) | ((value & 3) << 6)); } }

        public RAS1_TRef(byte dat0, byte dat1, byte dat2)
        { _dat0 = dat0; _dat1 = dat1; _dat2 = dat2; }

        public RAS1_TRef(int value)
        {
            _dat2 = (byte)((value) & 0xFF);
            _dat1 = (byte)((value >> 8) & 0xFF);
            _dat0 = (byte)((value >> 16) & 0xFF);
        }

        public static int Shift(int ti0, int tc0, int te0, int cc0, int ti1, int tc1, int te1, int cc1)
        {
            return (ti0) |
            ((tc0) << 3) |
            ((te0) << 6) |
            ((cc0) << 7) |
            ((ti1) << 12) |
            ((tc1) << 15) |
            ((te1) << 18) |
            ((cc1) << 19);
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct KSel
    {
        //0000 0000 0000 0000 0000 0011   XRB - Swap Mode Only
        //0000 0000 0000 0000 0000 1100   XGA - Swap Mode Only
        //0000 0000 0000 0001 1111 0000   KCSEL0 - Selection Mode Only
        //0000 0000 0011 1110 0000 0000   KASEL0 - Selection Mode Only
        //0000 0111 1100 0000 0000 0000   KCSEL1 - Selection Mode Only
        //1111 1000 0000 0000 0000 0000   KASEL1 - Selection Mode Only

        public byte _dat0, _dat1, _dat2;

        public int XRB { get { return (int)(_dat2 & 3); } set { _dat2 = (byte)((_dat2 & 0xFC) | (value & 3)); } }
        public int XGA { get { return (int)((_dat2 >> 2) & 3); } set { _dat2 = (byte)((_dat2 & 0xF3) | (value & 0xC)); } }

        public int KCSEL0 { get { return (int)((((_dat2 >> 4) & 0xF) | (_dat1 << 4)) & 0x1F); } set { } }
        public int KASEL0 { get { return (int)((_dat1 >> 1) & 0x1F); } set { } }

        public int KCSEL1 { get { return (int)((((_dat1 >> 6) & 3) | (_dat0 << 2)) & 0x1F); } set { } }
        public int KASEL1 { get { return (int)((_dat0 >> 3) & 0x1F); } set { } }

        public KSel(byte dat0, byte dat1, byte dat2)
        { _dat0 = dat0; _dat1 = dat1; _dat2 = dat2; }

        public KSel(int value)
        {
            _dat2 = (byte)((value) & 0xFF);
            _dat1 = (byte)((value >> 8) & 0xFF);
            _dat0 = (byte)((value >> 16) & 0xFF);
        }
        
        public static int Shift(int xrb, int xga, int kcsel0, int kasel0, int kcsel1, int kasel1)
        {
            return (xrb) | (xga << 2) | 
                (kcsel0 << 4) |
                (kasel0 << 9) |
                (kcsel1 << 14) |
                (kasel1 << 19);
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct CMD
    {
        //0000 0000 0000 0000 0000 0011   BT
        //0000 0000 0000 0000 0000 1100   Format
        //0000 0000 0000 0000 0111 0000   Bias
        //0000 0000 0000 0001 1000 0000   BS
        //0000 0000 0001 1110 0000 0000   M
        //0000 0000 1110 0000 0000 0000   SW
        //0000 0111 0000 0000 0000 0000   TW
        //0000 1000 0000 0000 0000 0000   LB
        //0001 0000 0000 0000 0000 0000   FB
        //1110 0000 0000 0000 0000 0000   Pad

        public byte _dat0, _dat1, _dat2;
        
        public int BT { get { return (int)((_dat2) & 3); } }
        public int Format { get { return (int)((_dat2 >> 2) & 3); } }
        public int Bias { get { return (int)((_dat2 >> 4) & 7); } }
        public int BS { get { return (int)((((_dat1 << 1) & 2) | (_dat2 >> 7) & 1) & 3); } }
        public int M { get { return (int)((_dat1 >> 1) & 0xF); } }
        public int SW { get { return (int)((_dat1 >> 5) & 7); } }
        public int TW { get { return (int)(_dat0 & 7); } }
        public bool LB { get { return ((_dat0 >> 3) & 1) != 0; } }
        public bool FB { get { return ((_dat0 >> 4) & 1) != 0; } }
        public int Pad { get { return (int)((_dat0 >> 5) & 7); } }

        public CMD(byte dat0, byte dat1, byte dat2)
        { _dat0 = dat0; _dat1 = dat1; _dat2 = dat2; }

        public CMD(int value)
        {
            _dat2 = (byte)((value) & 0xFF);
            _dat1 = (byte)((value >> 8) & 0xFF);
            _dat0 = (byte)((value >> 16) & 0xFF);
        }

        public static int Shift(int bt, int fmt, int bias, int bs, int m, int sw, int tw, int lb, int fb)
        {
            return (bt) |
            ((fmt) << 2) |
            ((bias) << 4) |
            ((bs) << 7) |
            ((m) << 9) |
            ((sw) << 13) |
            ((tw) << 16) |
            ((lb) << 19) |
            ((fb) << 20);
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct RAS1_SS
    {
        //0000 0000 0000 0000 0000 1111   SS0
        //0000 0000 0000 0000 1111 0000   TS0
        //0000 0000 0000 1111 0000 0000   SS1
        //0000 0000 1111 0000 0000 0000   TS1

        public byte _pad, _dat1, _dat2;

        public IndTexScale S_Scale0 { get { return (IndTexScale)((_dat2) & 0xF); } set { _dat2 = (byte)((_dat2 & 0xF0) | ((int)value & 0xF)); } }
        public IndTexScale T_Scale0 { get { return (IndTexScale)((_dat2 >> 4) & 0xF); } set { _dat2 = (byte)((_dat2 & 0xF0) | ((int)value & 0xF)); } }
        public IndTexScale S_Scale1 { get { return (IndTexScale)((_dat1) & 0xF); } set { _dat1 = (byte)((_dat1 & 0xF0) | ((int)value & 0xF)); } }
        public IndTexScale T_Scale1 { get { return (IndTexScale)((_dat1 >> 4) & 0xF); } set { _dat1 = (byte)((_dat1 & 0xF0) | ((int)value & 0xF)); } }
        
        public RAS1_SS(byte dat1, byte dat2)
        { _pad = 0; _dat1 = dat1; _dat2 = dat2; }

        public RAS1_SS(int value)
        {
            _dat2 = (byte)((value) & 0xFF);
            _dat1 = (byte)((value >> 8) & 0xFF);
            _pad = 0;
        }
    }

    public enum RegType
    {
        //TEV register type field
        TEV_COLOR_REG = 0,
        TEV_KONSTANT_REG = 1
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ColorReg
    {
        public static readonly ColorReg Konstant = new ColorReg() { _dat0 = 0x80 };

        //0000 0000 0000 1111 1111 1111 Red (Lo) / Blue (Hi) - A
        //0111 1111 1111 0000 0000 0000 Alpha (Lo) /Green (Hi) - B
        //1000 0000 0000 0000 0000 0000 Register Type

        public byte _dat0, _dat1, _dat2;

        public short A { get { return (short)(((short)(_dat1 << 13) >> 5) | _dat2); } set { _dat1 = (byte)((_dat1 & 0xF8) | ((value >> 8) & 0x7)); _dat2 = (byte)(value & 0xFF); } }
        public short B { get { return (short)(((short)(_dat0 << 9) >> 5) | (_dat1 >> 4)); } set { _dat0 = (byte)((_dat0 & 0x80) | ((value >> 4) & 0x7F)); _dat1 = (byte)((_dat1 & 0xF) | (value << 4)); } }
        public RegType Type { get { return (RegType)((_dat0 & 0x80) != 0 ? 1 : 0); } set { _dat0 = (byte)((_dat0 & 0x7F) | ((int)value == 1 ? 0x80 : 0)); } }

        public override string ToString()
        {
            return String.Format("A:{0}, B:{1}, Type:{2}", A, B, Type);
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ConstantAlpha
    {
        public static readonly ConstantAlpha Default = new ConstantAlpha();

        public byte Pad, Enable, Value;
    }

    public enum BPMemory : byte
    {
        BPMEM_GENMODE = 0x00,

        BPMEM_DISPLAYCOPYFILER0 = 0x01,
        BPMEM_DISPLAYCOPYFILER1 = 0x02,
        BPMEM_DISPLAYCOPYFILER2 = 0x03,
        BPMEM_DISPLAYCOPYFILER3 = 0x04,
        BPMEM_DISPLAYCOPYFILER4 = 0x05,

        BPMEM_IND_MTXA0 = 0x06,
        BPMEM_IND_MTXB0 = 0x07,
        BPMEM_IND_MTXC0 = 0x08,
        BPMEM_IND_MTXA1 = 0x09,
        BPMEM_IND_MTXB1 = 0x0A,
        BPMEM_IND_MTXC1 = 0x0B,
        BPMEM_IND_MTXA2 = 0x0C,
        BPMEM_IND_MTXB2 = 0x0D,
        BPMEM_IND_MTXC2 = 0x0E,
        BPMEM_IND_IMASK = 0x0F,

        BPMEM_IND_CMD0 = 0x10,
        BPMEM_IND_CMD1 = 0x11,
        BPMEM_IND_CMD2 = 0x12,
        BPMEM_IND_CMD3 = 0x13,
        BPMEM_IND_CMD4 = 0x14,
        BPMEM_IND_CMD5 = 0x15,
        BPMEM_IND_CMD6 = 0x16,
        BPMEM_IND_CMD7 = 0x17,
        BPMEM_IND_CMD8 = 0x18,
        BPMEM_IND_CMD9 = 0x19,
        BPMEM_IND_CMDA = 0x1A,
        BPMEM_IND_CMDB = 0x1B,
        BPMEM_IND_CMDC = 0x1C,
        BPMEM_IND_CMDD = 0x1D,
        BPMEM_IND_CMDE = 0x1E,
        BPMEM_IND_CMDF = 0x1F,

        BPMEM_SCISSORTL = 0x20,
        BPMEM_SCISSORBR = 0x21,
        BPMEM_LINEPTWIDTH = 0x22,
        BPMEM_PERF0_TRI = 0x23,
        BPMEM_PERF0_QUAD = 0x24,

        BPMEM_RAS1_SS0 = 0x25,
        BPMEM_RAS1_SS1 = 0x26,
        BPMEM_IREF = 0x27,

        BPMEM_TREF0 = 0x28,
        BPMEM_TREF1 = 0x29,
        BPMEM_TREF2 = 0x2A,
        BPMEM_TREF3 = 0x2B,
        BPMEM_TREF4 = 0x2C,
        BPMEM_TREF5 = 0x2D,
        BPMEM_TREF6 = 0x2E,
        BPMEM_TREF7 = 0x2F,
        
        BPMEM_SU_SSIZE0 = 0x30,
        BPMEM_SU_TSIZE0 = 0x31,
        BPMEM_SU_SSIZE1 = 0x32,
        BPMEM_SU_TSIZE1 = 0x33,
        BPMEM_SU_SSIZE2 = 0x34,
        BPMEM_SU_TSIZE2 = 0x35,
        BPMEM_SU_SSIZE3 = 0x36,
        BPMEM_SU_TSIZE3 = 0x37,
        BPMEM_SU_SSIZE4 = 0x38,
        BPMEM_SU_TSIZE4 = 0x39,
        BPMEM_SU_SSIZE5 = 0x3A,
        BPMEM_SU_TSIZE5 = 0x3B,
        BPMEM_SU_SSIZE6 = 0x3C,
        BPMEM_SU_TSIZE6 = 0x3D,
        BPMEM_SU_SSIZE7 = 0x3E,
        BPMEM_SU_TSIZE7 = 0x3F,

        BPMEM_ZMODE = 0x40,
        BPMEM_BLENDMODE = 0x41,
        BPMEM_CONSTANTALPHA = 0x42,
        BPMEM_ZCOMPARE = 0x43,
        BPMEM_FIELDMASK = 0x44,
        BPMEM_SETDRAWDONE = 0x45,
        BPMEM_BUSCLOCK0 = 0x46,
        BPMEM_PE_TOKEN_ID = 0x47,
        BPMEM_PE_TOKEN_INT_ID = 0x48,

        BPMEM_EFB_TL = 0x49,
        BPMEM_EFB_BR = 0x4A,
        BPMEM_EFB_ADDR = 0x4B,

        BPMEM_MIPMAP_STRIDE = 0x4D,
        BPMEM_COPYYSCALE = 0x4E,

        BPMEM_CLEAR_AR = 0x4F,
        BPMEM_CLEAR_GB = 0x50,
        BPMEM_CLEAR_Z = 0x51,

        BPMEM_TRIGGER_EFB_COPY = 0x52,
        BPMEM_COPYFILTER0 = 0x53,
        BPMEM_COPYFILTER1 = 0x54,
        BPMEM_CLEARBBOX1 = 0x55,
        BPMEM_CLEARBBOX2 = 0x56,

        BPMEM_UNKNOWN_57 = 0x57,
        
        BPMEM_REVBITS = 0x58,
        BPMEM_SCISSOROFFSET = 0x59,

        BPMEM_UNKNOWN_60 = 0x60,
        BPMEM_UNKNOWN_61 = 0x61,
        BPMEM_UNKNOWN_62 = 0x62,

        BPMEM_TEXMODESYNC = 0x63,
        BPMEM_LOADTLUT0 = 0x64,
        BPMEM_LOADTLUT1 = 0x65,
        BPMEM_TEXINVALIDATE = 0x66,
        BPMEM_PERF1 = 0x67,
        BPMEM_FIELDMODE = 0x68,
        BPMEM_BUSCLOCK1 = 0x69,

        BPMEM_TX_SETMODE0_A = 0x80,
        BPMEM_TX_SETMODE0_B = 0x81,
        BPMEM_TX_SETMODE0_C = 0x82,
        BPMEM_TX_SETMODE0_D = 0x83,
        
        BPMEM_TX_SETMODE1_A = 0x84,
        BPMEM_TX_SETMODE1_B = 0x85,
        BPMEM_TX_SETMODE1_C = 0x86,
        BPMEM_TX_SETMODE1_D = 0x87,

        BPMEM_TX_SETIMAGE0_A = 0x88,
        BPMEM_TX_SETIMAGE0_B = 0x89,
        BPMEM_TX_SETIMAGE0_C = 0x8A,
        BPMEM_TX_SETIMAGE0_D = 0x8B,
        
        BPMEM_TX_SETIMAGE1_A = 0x8C,
        BPMEM_TX_SETIMAGE1_B = 0x8D,
        BPMEM_TX_SETIMAGE1_C = 0x8E,
        BPMEM_TX_SETIMAGE1_D = 0x8F,

        BPMEM_TX_SETIMAGE2_A = 0x90,
        BPMEM_TX_SETIMAGE2_B = 0x91,
        BPMEM_TX_SETIMAGE2_C = 0x92,
        BPMEM_TX_SETIMAGE2_D = 0x93,

        BPMEM_TX_SETIMAGE3_A = 0x94,
        BPMEM_TX_SETIMAGE3_B = 0x95,
        BPMEM_TX_SETIMAGE3_C = 0x96,
        BPMEM_TX_SETIMAGE3_D = 0x97,

        BPMEM_TX_SETTLUT_A = 0x98,
        BPMEM_TX_SETTLUT_B = 0x99,
        BPMEM_TX_SETTLUT_C = 0x9A,
        BPMEM_TX_SETTLUT_D = 0x9B,

        BPMEM_TX_SETMODE0_4_A = 0xA0,
        BPMEM_TX_SETMODE0_4_B = 0xA1,
        BPMEM_TX_SETMODE0_4_C = 0xA2,
        BPMEM_TX_SETMODE0_4_D = 0xA3,

        BPMEM_TX_SETMODE1_4_A = 0xA4,
        BPMEM_TX_SETMODE1_4_B = 0xA5,
        BPMEM_TX_SETMODE1_4_C = 0xA6,
        BPMEM_TX_SETMODE1_4_D = 0xA7,

        BPMEM_TX_SETIMAGE0_4_A = 0xA8,
        BPMEM_TX_SETIMAGE0_4_B = 0xA9,
        BPMEM_TX_SETIMAGE0_4_C = 0xAA,
        BPMEM_TX_SETIMAGE0_4_D = 0xAB,

        BPMEM_TX_SETIMAGE1_4_A = 0xAC,
        BPMEM_TX_SETIMAGE1_4_B = 0xAD,
        BPMEM_TX_SETIMAGE1_4_C = 0xAE,
        BPMEM_TX_SETIMAGE1_4_D = 0xAF,

        BPMEM_TX_SETIMAGE2_4_A = 0xB0,
        BPMEM_TX_SETIMAGE2_4_B = 0xB1,
        BPMEM_TX_SETIMAGE2_4_C = 0xB2,
        BPMEM_TX_SETIMAGE2_4_D = 0xB3,

        BPMEM_TX_SETIMAGE3_4_A = 0xB4,
        BPMEM_TX_SETIMAGE3_4_B = 0xB5,
        BPMEM_TX_SETIMAGE3_4_C = 0xB6,
        BPMEM_TX_SETIMAGE3_4_D = 0xB7,

        BPMEM_TX_SETLUT_4_A = 0xB8,
        BPMEM_TX_SETLUT_4_B = 0xB9,
        BPMEM_TX_SETLUT_4_C = 0xBA,
        BPMEM_TX_SETLUT_4_D = 0xBB,

        BPMEM_UNKNOWN_BC = 0xBC,
        BPMEM_UNKNOWN_BB = 0xBB,
        BPMEM_UNKNOWN_BD = 0xBD,
        BPMEM_UNKNOWN_BE = 0xBE,
        BPMEM_UNKNOWN_BF = 0xBF,

        BPMEM_TEV_COLOR_ENV_0 = 0xC0,
        BPMEM_TEV_ALPHA_ENV_0 = 0xC1,
        BPMEM_TEV_COLOR_ENV_1 = 0xC2,
        BPMEM_TEV_ALPHA_ENV_1 = 0xC3,
        BPMEM_TEV_COLOR_ENV_2 = 0xC4,
        BPMEM_TEV_ALPHA_ENV_2 = 0xC5,
        BPMEM_TEV_COLOR_ENV_3 = 0xC6,
        BPMEM_TEV_ALPHA_ENV_3 = 0xC7,
        BPMEM_TEV_COLOR_ENV_4 = 0xC8,
        BPMEM_TEV_ALPHA_ENV_4 = 0xC9,
        BPMEM_TEV_COLOR_ENV_5 = 0xCA,
        BPMEM_TEV_ALPHA_ENV_5 = 0xCB,
        BPMEM_TEV_COLOR_ENV_6 = 0xCC,
        BPMEM_TEV_ALPHA_ENV_6 = 0xCD,
        BPMEM_TEV_COLOR_ENV_7 = 0xCE,
        BPMEM_TEV_ALPHA_ENV_7 = 0xCF,
        BPMEM_TEV_COLOR_ENV_8 = 0xD0,
        BPMEM_TEV_ALPHA_ENV_8 = 0xD1,
        BPMEM_TEV_COLOR_ENV_9 = 0xD2,
        BPMEM_TEV_ALPHA_ENV_9 = 0xD3,
        BPMEM_TEV_COLOR_ENV_A = 0xD4,
        BPMEM_TEV_ALPHA_ENV_A = 0xD5,
        BPMEM_TEV_COLOR_ENV_B = 0xD6,
        BPMEM_TEV_ALPHA_ENV_B = 0xD7,
        BPMEM_TEV_COLOR_ENV_C = 0xD8,
        BPMEM_TEV_ALPHA_ENV_C = 0xD9,
        BPMEM_TEV_COLOR_ENV_D = 0xDA,
        BPMEM_TEV_ALPHA_ENV_D = 0xDB,
        BPMEM_TEV_COLOR_ENV_E = 0xDC,
        BPMEM_TEV_ALPHA_ENV_E = 0xDD,
        BPMEM_TEV_COLOR_ENV_F = 0xDE,
        BPMEM_TEV_ALPHA_ENV_F = 0xDF,

        BPMEM_TEV_REGISTER_L_0 = 0xE0,
        BPMEM_TEV_REGISTER_H_0 = 0xE1,
        BPMEM_TEV_REGISTER_L_1 = 0xE2,
        BPMEM_TEV_REGISTER_H_1 = 0xE3,
        BPMEM_TEV_REGISTER_L_2 = 0xE4,
        BPMEM_TEV_REGISTER_H_2 = 0xE5,
        BPMEM_TEV_REGISTER_L_3 = 0xE6,
        BPMEM_TEV_REGISTER_H_3 = 0xE7,
        
        BPMEM_TEV_FOG_RANGE = 0xE8,
        BPMEM_TEV_FOG_PARAM_0 = 0xEE,
        BPMEM_TEV_FOG_B_MAGNITUDE = 0xEF,
        BPMEM_TEV_FOG_B_EXPONENT = 0xF0,
        BPMEM_TEV_FOG_PARAM_3 = 0xF1,
        BPMEM_TEV_FOG_COLOR = 0xF2,

        BPMEM_ALPHACOMPARE = 0xF3,
        BPMEM_BIAS = 0xF4,
        BPMEM_ZTEX2 = 0xF5,

        BPMEM_TEV_KSEL0 = 0xF6,
        BPMEM_TEV_KSEL1 = 0xF7,
        BPMEM_TEV_KSEL2 = 0xF8,
        BPMEM_TEV_KSEL3 = 0xF9,
        BPMEM_TEV_KSEL4 = 0xFA,
        BPMEM_TEV_KSEL5 = 0xFB,
        BPMEM_TEV_KSEL6 = 0xFC,
        BPMEM_TEV_KSEL7 = 0xFD,

        BPMEM_BP_MASK = 0xFE
    }
}
