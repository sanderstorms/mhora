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

namespace Mhora.Components.Property;

public class XmlYogaNode
{
	public enum Pos
	{
		SourceRef,
		SourceText,
		SourceItxText,
		MhoraRule,
		Result,
		Other
	}

	public string mhoraRule;     // rule in mhora format
	public string result;        // results
	public string sourceItxText; // source rule (eng / sans)
	public string sourceRef;     // source reference (text:verse)
	public string sourceText;    // source rule (english)
	public string yogaCat;       // yoga category
	public string yogaName;      // short desc of yoga (1-2 words)
}