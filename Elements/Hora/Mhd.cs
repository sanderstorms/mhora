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

using System.IO;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using Mhora.Database.Settings;

namespace Mhora.Elements.Hora;

/// <summary>
///     Class deals with reading the Jhd file specification
///     used by Jagannatha Hora
/// </summary>
public class Mhd : IFileToHoraInfo
{
	private readonly string _fname;

	public Mhd(string fileName)
	{
		_fname = fileName;
	}

	public HoraInfo ToHoraInfo()
	{
		try
		{
			var        hi = new HoraInfo();
			FileStream sOut;
			sOut = new FileStream(_fname, FileMode.Open, FileAccess.Read);
			var formatter = new BinaryFormatter();
			formatter.AssemblyFormat = FormatterAssemblyStyle.Simple;
			hi                       = (HoraInfo) formatter.Deserialize(sOut);
			sOut.Close();
			return hi;
		}
		catch
		{
			MessageBox.Show("Unable to read file");
			return new HoraInfo();
		}
	}

	public void ToFile(HoraInfo hi)
	{
		var sOut      = new FileStream(_fname, FileMode.OpenOrCreate, FileAccess.Write);
		var formatter = new BinaryFormatter();
		formatter.Serialize(sOut, hi);
		sOut.Close();
	}
}