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

using System.Security.Cryptography;
using System.Text;

namespace Mhora.Components.File;

/// <summary>
///     Summary description for Base64Encoder.
/// </summary>
public class MhoraVerifier
{
	public static bool GoodHash(string key, string hash)
	{
		return GetHash(key) == hash;
	}

	public static string GetHash(string s)
	{
		var utf8        = new UTF8Encoding();
		var hasher      = new MD5CryptoServiceProvider();
		var hashedBytes = new byte[hasher.HashSize / 8];
		hashedBytes = hasher.ComputeHash(utf8.GetBytes(s));
		var b64 = new Base64Encoder(hashedBytes);
		return new string(b64.GetEncoded());
	}
}

public class Base64Encoder
{
	private readonly int    _blockCount;
	private readonly int    _length;
	private readonly int    _length2;
	private readonly int    _paddingCount;
	private readonly byte[] _source;

	public Base64Encoder(byte[] input)
	{
		_source = input;
		_length = input.Length;
		if (_length % 3 == 0)
		{
			_paddingCount = 0;
			_blockCount   = _length / 3;
		}
		else
		{
			_paddingCount = 3 - _length % 3; //need to add padding
			_blockCount   = (_length + _paddingCount) / 3;
		}

		_length2 = _length + _paddingCount; //or blockCount *3
	}

	public char[] GetEncoded()
	{
		var source2 = new byte[_length2];
		//copy data over insert padding
		for (var x = 0; x < _length2; x++)
		{
			if (x < _length)
			{
				source2[x] = _source[x];
			}
			else
			{
				source2[x] = 0;
			}
		}

		byte b1,   b2,    b3;
		byte temp, temp1, temp2, temp3, temp4;
		var  buffer = new byte[_blockCount * 4];
		var  result = new char[_blockCount * 4];
		for (var x = 0; x < _blockCount; x++)
		{
			b1 = source2[x * 3];
			b2 = source2[x * 3 + 1];
			b3 = source2[x * 3 + 2];

			temp1 = (byte) ((b1 & 252) >> 2); //first

			temp  =  (byte) ((b1 & 3)   << 4);
			temp2 =  (byte) ((b2 & 240) >> 4);
			temp2 += temp; //second

			temp  =  (byte) ((b2 & 15)  << 2);
			temp3 =  (byte) ((b3 & 192) >> 6);
			temp3 += temp; //third

			temp4 = (byte) (b3 & 63); //fourth

			buffer[x * 4]     = temp1;
			buffer[x * 4 + 1] = temp2;
			buffer[x * 4 + 2] = temp3;
			buffer[x * 4 + 3] = temp4;
		}

		for (var x = 0; x < _blockCount * 4; x++)
		{
			result[x] = Sixbit2Char(buffer[x]);
		}

		//covert last "A"s to "=", based on paddingCount
		switch (_paddingCount)
		{
			case 0: break;
			case 1:
				result[_blockCount * 4 - 1] = '=';
				break;
			case 2:
				result[_blockCount * 4 - 1] = '=';
				result[_blockCount * 4 - 2] = '=';
				break;
		}

		return result;
	}

	private char Sixbit2Char(byte b)
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

		if (b >= 0 && b <= 63)
		{
			return lookupTable[b];
		}

		//should not happen;
		return ' ';
	}
}

/// <summary>
///     Summary description for Base64Decoder.
/// </summary>
public class Base64Decoder
{
	private readonly int    _blockCount;
	private readonly int    _length;
	private readonly int    _length2;
	private readonly int    _paddingCount;
	private readonly char[] _source;
	private          int    _length3;

	public Base64Decoder(char[] input)
	{
		var temp = 0;
		_source = input;
		_length = input.Length;

		//find how many padding are there
		for (var x = 0; x < 2; x++)
		{
			if (input[_length - x - 1] == '=')
			{
				temp++;
			}
		}

		_paddingCount = temp;
		//calculate the blockCount;
		//assuming all whitespace and carriage returns/newline were removed.
		_blockCount = _length     / 4;
		_length2    = _blockCount * 3;
	}

	public byte[] GetDecoded()
	{
		var buffer  = new byte[_length];  //first conversion result
		var buffer2 = new byte[_length2]; //decoded array with padding

		for (var x = 0; x < _length; x++)
		{
			buffer[x] = Char2Sixbit(_source[x]);
		}

		byte b,     b1,    b2,    b3;
		byte temp1, temp2, temp3, temp4;

		for (var x = 0; x < _blockCount; x++)
		{
			temp1 = buffer[x * 4];
			temp2 = buffer[x * 4 + 1];
			temp3 = buffer[x * 4 + 2];
			temp4 = buffer[x * 4 + 3];

			b  =  (byte) (temp1        << 2);
			b1 =  (byte) ((temp2 & 48) >> 4);
			b1 += b;

			b  =  (byte) ((temp2 & 15) << 4);
			b2 =  (byte) ((temp3 & 60) >> 2);
			b2 += b;

			b  =  (byte) ((temp3 & 3) << 6);
			b3 =  temp4;
			b3 += b;

			buffer2[x * 3]     = b1;
			buffer2[x * 3 + 1] = b2;
			buffer2[x * 3 + 2] = b3;
		}

		//remove paddings
		_length3 = _length2 - _paddingCount;
		var result = new byte[_length3];

		for (var x = 0; x < _length3; x++)
		{
			result[x] = buffer2[x];
		}

		return result;
	}

	private byte Char2Sixbit(char c)
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
				return (byte) x;
			}
		}

		//should not reach here
		return 0;
	}
}