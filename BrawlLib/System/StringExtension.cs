using System;

namespace System
{
    public static class StringExtension
    {
        public static unsafe string TruncateAndFill(this string s, int length, char fillChar)
        {
            char* buffer = stackalloc char[length];

            int i;
            int min = Math.Min(s.Length, length);
            for (i = 0; i < min; i++)
                buffer[i] = s[i];

            while (i < length)
                buffer[i++] = fillChar;

            return new string(buffer, 0, length);
        }
        public static unsafe int IndexOfOccurance(this string s, char c, int index)
        {
            int len = s.Length;
            fixed(char* cPtr = s)
            {
                for (int i = 0, count = 0; i < len; i++)
                    if ((cPtr[i] == c) && (count++ == index))
                        return i;
            }
            return -1;
        }
    }
}
