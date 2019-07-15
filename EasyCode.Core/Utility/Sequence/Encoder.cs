using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyCode.Core.Utility.Sequence
{
    internal sealed class Encoder
    {
        private static readonly char[] EncodingChars = "0123456789ABCDEFGHIJKLMNOPQRSTUV".ToArray();

        internal static string Encode32(long number, bool withLeadingZero = false)
        {
            char[] output = new char[13];
            int index = 12;
            var b = BitConverter.GetBytes(number);
            do
            {
                output[index--] = EncodingChars[number & 0x1F];
                number >>= 5;
            }
            while (index >= 0 && number != 0);

            var id = new string(output, index + 1, 12 - index);
            return withLeadingZero ? id.PadLeft(13, '0') : id;
        }
    }
}
