using System;
using System.Runtime.InteropServices;
using System.IO;
using System.Windows.Forms;
using BrawlLib.SSBBTypes;

namespace BrawlLib.Wii.Compression
{
    public unsafe class LZ77 : IDisposable
    {
        public const int WindowMask = 0xFFF;
        public const int WindowLength = 4096; //12 bits - 1, 1 - 4096
        public const int PatternLength = 18; //4 bits + 3, 3 - 18
        public const int MinMatch = 3;

        VoidPtr _dataAddr;

        ushort* _Next;
        ushort* _First;
        ushort* _Last;

        int _wIndex;
        int _wLength;

        private LZ77()
        {
            _dataAddr = Marshal.AllocHGlobal((0x1000 + 0x10000 + 0x10000) * 2);

            _Next = (ushort*)_dataAddr;
            _First = _Next + WindowLength;
            _Last = _First + 0x10000;

        }

        ~LZ77() { Dispose(); }
        public void Dispose()
        {
            if (_dataAddr) { Marshal.FreeHGlobal(_dataAddr); _dataAddr = 0; }
            GC.SuppressFinalize(this);
        }

        public int Compress(VoidPtr srcAddr, int srcLen, Stream outStream, IProgressTracker progress)
        {
            int dstLen = 4, bitCount;
            byte control;

            byte* sPtr = (byte*)srcAddr;//, ceil = sPtr + srcLen;
            int matchLength, matchOffset = 0;

            //Initialize
            Memory.Fill(_First, 0x40000, 0xFF);
            _wIndex = _wLength = 0;

            //Write header
            CompressionHeader header = new CompressionHeader();
            header.Algorithm = CompressionType.LZ77;
            header.ExpandedSize = (int)srcLen;
            outStream.Write(&header, 4);

            byte[] blockBuffer = new byte[17];
            int dInd;
            int lastUpdate = srcLen;
            int remaining = srcLen;

            if (progress != null)
                progress.Begin(0, remaining, 0);

            while(remaining > 0)
            {
                dInd = 1;
                //dPtr = blockBuffer + 1;
                for (bitCount = 0, control = 0; (bitCount < 8) && (remaining > 0); bitCount++)
                {
                    control <<= 1;
                    if ((matchLength = FindPattern(sPtr, remaining, ref matchOffset)) != 0)
                    {
                        control |= 1;
                        blockBuffer[dInd++] = (byte)(((matchLength - 3) << 4) | ((matchOffset - 1) >> 8));
                        blockBuffer[dInd++] = (byte)(matchOffset - 1);
                        //*dPtr++ = (byte)(((matchLength - 3) << 4) | ((matchOffset - 1) >> 8));
                        //*dPtr++ = (byte)(matchOffset - 1);

                        //Consume(sPtr, matchLength);
                        //sPtr += matchLength;
                    }
                    else
                    {
                        matchLength = 1;
                        //Consume(sPtr, 1);
                        blockBuffer[dInd++] = *sPtr;
                    }
                    Consume(sPtr, matchLength, remaining);
                    sPtr += matchLength;
                    remaining -= matchLength;
                }
                //Left-align bits
                control <<= 8 - bitCount;

                //Write buffer
                blockBuffer[0] = control;
                outStream.Write(blockBuffer, 0, dInd);
                dstLen += dInd;
                //*blockBuffer = control;
                //outStream.Write(blockBuffer, (uint)(dPtr - blockBuffer));
                //dstLen += (int)(dPtr - blockBuffer);

                if (progress != null)
                    if ((lastUpdate - remaining) > 0x4000)
                    {
                        lastUpdate = remaining;
                        progress.Update(srcLen - remaining);
                    }
            }

            //if (progress != null)
            //    progress.Update(srcLen);

            //while ((dstLen & 3) != 0)
            //{
            //    outStream.WriteByte(0);
            //    dstLen++;
            //}
            outStream.Flush();

            if (progress != null)
                progress.Finish();

            return dstLen;
        }
        
        private ushort MakeHash(byte* ptr)
        {
            return (ushort)((ptr[0] << 6) ^ (ptr[1] << 3) ^ ptr[2]);
        }

        private int FindPattern(byte* sPtr, int length, ref int matchOffset)
        {
            if (length < MinMatch) return 0;
            length = Math.Min(length, PatternLength);

            byte* mPtr;
            int bestLen = MinMatch - 1, bestOffset = 0, index;
            for (int offset = _First[MakeHash(sPtr)]; offset != 0xFFFF; offset = _Next[offset])
            {
                if (offset < _wIndex) mPtr = sPtr - _wIndex + offset;
                else mPtr = sPtr - _wLength - _wIndex + offset;

                if (sPtr - mPtr < 2) break;

                for (index = bestLen + 1; (--index >= 0) && (mPtr[index] == sPtr[index]); ) ;
                if (index >= 0) continue;
                for (index = bestLen; (++index < length) && (mPtr[index] == sPtr[index]); ) ;

                bestOffset = (int)(sPtr - mPtr);
                if ((bestLen = index) == length) break;
            }

            if (bestLen < MinMatch) return 0;

            matchOffset = bestOffset;
            return bestLen;
        }
        private void Consume(byte* ptr, int length, int remaining)
        {
            int last, inOffset, inVal, outVal;
            for (int i = Math.Min(length, remaining - 2); i-- > 0;)
            {
                if (_wLength == WindowLength)
                {
                    //Remove node
                    outVal = MakeHash(ptr - WindowLength);
                    if ((_First[outVal] = _Next[_First[outVal]]) == 0xFFFF)
                        _Last[outVal] = 0xFFFF;
                    inOffset = _wIndex++;
                    _wIndex &= WindowMask;
                }
                else
                    inOffset = _wLength++;

                inVal = MakeHash(ptr++);
                if ((last = _Last[inVal]) == 0xFFFF)
                    _First[inVal] = (ushort)inOffset;
                else
                    _Next[last] = (ushort)inOffset;

                _Last[inVal] = (ushort)inOffset;
                _Next[inOffset] = 0xFFFF;
            }
        }

        public static int Compact(VoidPtr srcAddr, int srcLen, Stream outStream, string name)
        {
            using (LZ77 lz = new LZ77())
            using (ProgressWindow prog = new ProgressWindow(null, "LZ77", String.Format("Compressing {0}, please wait...", name), false))
                return lz.Compress(srcAddr, srcLen, outStream, prog);
        }

        public static void Expand(CompressionHeader* header, VoidPtr dstAddress, int dstLen)
        {
            if ((header->Algorithm != CompressionType.LZ77) || (header->Parameter != 0))
                throw new InvalidCompressionException("Compression header does not match LZ77 format.");

            for (byte* srcPtr = (byte*)header->Data, dstPtr = (byte*)dstAddress, ceiling = dstPtr + dstLen; dstPtr < ceiling; )
                for (byte control = *srcPtr++, bit = 8; (bit-- != 0) && (dstPtr != ceiling); )
                    if ((control & (1 << bit)) == 0)
                        *dstPtr++ = *srcPtr++;
                    else
                        for (int num = (*srcPtr >> 4) + 3, offset = (((*srcPtr++ & 0xF) << 8) | *srcPtr++) + 2; (dstPtr != ceiling) && (num-- > 0); *dstPtr++ = dstPtr[-offset]) ;
        }
    }
}
