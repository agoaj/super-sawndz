using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using BrawlLib.SSBBTypes;

namespace BrawlLib.Wii.Compression
{
    [global::System.Serializable]
    public class InvalidCompressionException : Exception
    {
        public InvalidCompressionException() { }
        public InvalidCompressionException(string message) : base(message) { }
        public InvalidCompressionException(string message, Exception inner) : base(message, inner) { }
        protected InvalidCompressionException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }

    public static unsafe class Compressor
    {
        public static bool IsDataCompressed(VoidPtr addr, int length)
        {
            if (*(uint*)addr == YAZ0.Tag)
                return true;
            else
            {
                CompressionHeader* cmpr = (CompressionHeader*)addr;

                if (cmpr->ExpandedSize < length)
                    return false;

                switch (cmpr->Algorithm)
                {
                    case CompressionType.LZ77: return true;
                    default: return false;
                }
            }
        }
        public static void Expand(CompressionHeader* header, VoidPtr dstAddr, int dstLen)
        {
            switch (header->Algorithm)
            {
                case CompressionType.LZ77: { LZ77.Expand(header, dstAddr, dstLen); break; }
                case CompressionType.Huffman:
                case CompressionType.RunLength:
                case CompressionType.Differential:
                default:
                    throw new InvalidCompressionException("Unknown compression type.");
            }
        }
        public static void Expand(YAZ0* header, VoidPtr dstAddr, int dstLen)
        {
            RunLength.ExpandYAZ0(header, dstAddr, dstLen);
        }
        internal static unsafe void Compact(CompressionType type, VoidPtr srcAddr, int srcLen, Stream outStream)
        {
            switch (type)
            {
                case CompressionType.LZ77: { LZ77.Compact(srcAddr, srcLen, outStream, null); break; }
            }
        }

        internal static unsafe void CompactYAZ0(VoidPtr srcAddr, int srcLen, Stream outStream)
        {
            RunLength.CompactYAZ0(srcAddr, srcLen, outStream, null);
        }
    }
}
