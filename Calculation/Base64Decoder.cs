/******
Copyright (C) 2005 Ajit Krishnan (http://www.mudgala.com)

This program is free software; you can redistribute it and/or
modify it under the terms of the GNU General Public License
as published by the Free Software Foundation; either version 2
of the License, or (at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program; if not, write to the Free Software
Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
******/

namespace Mhora.Calculation
{
    /// <summary>
    ///     Summary description for Base64Decoder.
    /// </summary>
    public class Base64Decoder
    {
        private readonly int    blockCount;
        private readonly int    length;
        private readonly int    length2;
        private          int    length3;
        private readonly int    paddingCount;
        private readonly char[] source;

        public Base64Decoder(char[] input)
        {
            var temp = 0;
            source = input;
            length = input.Length;

            //find how many padding are there
            for (var x = 0; x < 2; x++)
            {
                if (input[length - x - 1] == '=')
                {
                    temp++;
                }
            }

            paddingCount = temp;
            //calculate the blockCount;
            //assuming all whitespace and carriage returns/newline were removed.
            blockCount = length     / 4;
            length2    = blockCount * 3;
        }

        public byte[] GetDecoded()
        {
            var buffer  = new byte[length];  //first conversion result
            var buffer2 = new byte[length2]; //decoded array with padding

            for (var x = 0; x < length; x++)
            {
                buffer[x] = char2sixbit(source[x]);
            }

            byte b,
                 b1,
                 b2,
                 b3;
            byte temp1,
                 temp2,
                 temp3,
                 temp4;

            for (var x = 0; x < blockCount; x++)
            {
                temp1 = buffer[x * 4];
                temp2 = buffer[x * 4 + 1];
                temp3 = buffer[x * 4 + 2];
                temp4 = buffer[x * 4 + 3];

                b  =  (byte)(temp1        << 2);
                b1 =  (byte)((temp2 & 48) >> 4);
                b1 += b;

                b  =  (byte)((temp2 & 15) << 4);
                b2 =  (byte)((temp3 & 60) >> 2);
                b2 += b;

                b  =  (byte)((temp3 & 3) << 6);
                b3 =  temp4;
                b3 += b;

                buffer2[x * 3]     = b1;
                buffer2[x * 3 + 1] = b2;
                buffer2[x * 3 + 2] = b3;
            }

            //remove paddings
            length3 = length2 - paddingCount;
            var result = new byte[length3];

            for (var x = 0; x < length3; x++)
            {
                result[x] = buffer2[x];
            }

            return result;
        }

        private byte char2sixbit(char c)
        {
            var lookupTable = new char[64]
            {
                'A',
                'B',
                'C',
                'D',
                'E',
                'F',
                'G',
                'H',
                'I',
                'J',
                'K',
                'L',
                'M',
                'N',
                'O',
                'P',
                'Q',
                'R',
                'S',
                'T',
                'U',
                'V',
                'W',
                'X',
                'Y',
                'Z',
                'a',
                'b',
                'c',
                'd',
                'e',
                'f',
                'g',
                'h',
                'i',
                'j',
                'k',
                'l',
                'm',
                'n',
                'o',
                'p',
                'q',
                'r',
                's',
                't',
                'u',
                'v',
                'w',
                'x',
                'y',
                'z',
                '0',
                '1',
                '2',
                '3',
                '4',
                '5',
                '6',
                '7',
                '8',
                '9',
                '+',
                '/'
            };
            if (c == '=')
            {
                return 0;
            }

            for (var x = 0; x < 64; x++)
            {
                if (lookupTable[x] == c)
                {
                    return (byte)x;
                }
            }

            //should not reach here
            return 0;
        }
    }
}