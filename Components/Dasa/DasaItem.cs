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

using System.Windows.Forms;
using Mhora.Database.Settings;
using Mhora.Definitions;
using Mhora.Elements.Dasas;
using Mhora.Util;

namespace Mhora.Components.Dasa;

/// <summary>
///     Specifies a DasaItem which can be used by any of the Dasa Systems.
///     Hence it includes _both_ a Graha and zodiacHouse in order to be used
///     by systems which Graha dasas and Rasi bhukti's and vice-versa. The logic
///     should be checked carefully
/// </summary>
public class DasaItem : ListViewItem
{
	public DasaEntry Entry;
	public string    EventDesc;

	public DasaItem(DasaEntry entry)
	{
		Entry = entry;
	}

	public DasaItem(Body graha, double startUt, double dasaLength, int level, string shortDesc)
	{
		Construct(new DasaEntry(graha, startUt, dasaLength, level, shortDesc));
	}

	public DasaItem(ZodiacHouse zodiacHouse, double startUt, double dasaLength, int level, string shortDesc)
	{
		Construct(new DasaEntry(zodiacHouse, startUt, dasaLength, level, shortDesc));
	}

	public void PopulateListViewItemMembers(ToDate td, IDasa id)
	{
		UseItemStyleForSubItems = false;

		//this.Text = entry.shortDesc;
		Font      = MhoraGlobalOptions.Instance.GeneralFont;
		ForeColor = MhoraGlobalOptions.Instance.DasaPeriodColor;
		var m          = td.AddYears(Entry.StartUt);
		var m2         = td.AddYears(Entry.StartUt + Entry.DasaLength);
		var sDateRange = m + " - " + m2;
		for (var i = 1; i < Entry.Level; i++)
		{
			sDateRange = " " + sDateRange;
		}

		SubItems.Add(sDateRange);
		Text                  = Entry.DasaName + id.EntryDescription(Entry, m, m2);
		SubItems[1].Font      = MhoraGlobalOptions.Instance.FixedWidthFont;
		SubItems[1].ForeColor = MhoraGlobalOptions.Instance.DasaDateColor;
	}

	private void Construct(DasaEntry entry)
	{
		Entry = entry;
	}
}