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

using System;
using System.Collections;
using System.Collections.Generic;
using Mhora.Components.DasaControl;
using Mhora.Elements;

namespace Mhora.Dasas;

/// <summary>
///     Interface implemented by all IDasa functions. At the moment the method of
///     implementation for any Level below AntarDasa is assumed to be the same.
/// </summary>
public interface IDasa : IUpdateable
{
	double          ParamAyus();
	List<DasaEntry> Dasa(int cycle);
	List<DasaEntry> AntarDasa(DasaEntry        pdi);
	string    EntryDescription(DasaEntry de, DateTime start, DateTime end);
	string    Description();
	void      DivisionChanged(Division d);
	void      RecalculateOptions();
}