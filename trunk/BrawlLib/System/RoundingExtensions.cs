using System;

namespace System
{
    public static class RoundingExtensions
    {
        public static byte RUp(this byte value, int factor)
        {
            if (factor <= 0) return value;
            return (byte)((value + (factor - 1)) - ((value + (factor - 1)) % factor));
        }
        public static byte RDown(this byte value, int factor)
        {
            if (factor <= 0) return value;
            return (byte)(value - value % factor);
        }

        public static UInt16 RUp(this UInt16 value, int factor)
        {
            if (factor <= 0) return value;
            return (ushort)((value + (factor - 1)) - ((value + (factor - 1)) % factor));
        }

        public static UInt16 RDown(this UInt16 value, int factor)
        {
            if (factor <= 0) return value;
            return (ushort)(value - value % factor);
        }

        public static UInt32 RUp(this UInt32 value, int factor)
        {
            if (factor <= 0) return value;
            return (uint)((value + (factor - 1)) - ((value + (factor - 1)) % factor));
        }

        public static UInt32 RDown(this UInt32 value, int factor)
        {
            if (factor <= 0) return value;
            return (uint)(value - value % factor);
        }

        public static Int16 RUp(this Int16 value, int factor)
        {
            if (factor <= 0) return value;
            return (short)((value + (factor - 1)) - ((value + (factor - 1)) % factor));
        }
        public static Int16 RDown(this Int16 value, int factor)
        {
            if (factor <= 0) return value;
            return (short)(value - value % factor);
        }

        public static Int32 RUp(this Int32 value, int factor)
        {
            if (factor <= 0) return value;
            return (int)((value + (factor - 1)) - ((value + (factor - 1)) % factor));
        }

        public static Int32 RDown(this Int32 value, int factor)
        {
            if (factor <= 0) return value;
            return (int)(value - value % factor);
        }

        public static Int64 RUp(this Int64 value, int factor)
        {
            if (factor <= 0) return value;
            return (long)((value + (factor - 1)) - (value + (factor - 1)) % factor);
        }

        public static Int64 RDown(this Int64 value, int factor)
        {
            if (factor <= 0) return value;
            return (long)(value - value % factor);
        }
    }
}