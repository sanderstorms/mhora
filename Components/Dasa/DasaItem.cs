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
using Mhora.Elements;
using Mhora.Util;

namespace Mhora.Components.Dasa;

/// <summary>
///     Specifies a DasaItem which can be used by any of the Dasa Systems.
///     Hence it includes _both_ a graha and zodiacHouse in order to be used
///     by systems which Graha dasas and Rasi bhukti's and vice-versa. The logic
///     should be checked carefully
/// </summary>
public class DasaItem : ListViewItem
{
	public DasaEntry entry;
	public string    EventDesc;

	public DasaItem(DasaEntry _entry)
	{
		entry = _entry;
	}

	public DasaItem(Elements.Body.Name _graha, double _startUT, double _dasaLength, int _level, string _shortDesc)
	{
		Construct(new DasaEntry(_graha, _startUT, _dasaLength, _level, _shortDesc));
	}

	public DasaItem(ZodiacHouse.Name _zodiacHouse, double _startUT, double _dasaLength, int _level, string _shortDesc)
	{
		Construct(new DasaEntry(_zodiacHouse, _startUT, _dasaLength, _level, _shortDesc));
	}

	public void populateListViewItemMembers(ToDate td, IDasa id)
	{
		UseItemStyleForSubItems = false;

		//this.Text = entry.shortDesc;
		Font      = MhoraGlobalOptions.Instance.GeneralFont;
		ForeColor = MhoraGlobalOptions.Instance.DasaPeriodColor;
		var m          = td.AddYears(entry.startUT);
		var m2         = td.AddYears(entry.startUT + entry.dasaLength);
		var sDateRange = m + " - " + m2;
		for (var i = 1; i < entry.level; i++)
		{
			sDateRange = " " + sDateRange;
		}

		SubItems.Add(sDateRange);
		Text                  = entry.shortDesc + id.EntryDescription(entry, m, m2);
		SubItems[1].Font      = MhoraGlobalOptions.Instance.FixedWidthFont;
		SubItems[1].ForeColor = MhoraGlobalOptions.Instance.DasaDateColor;
	}

	private void Construct(DasaEntry _entry)
	{
		entry = _entry;
	}
}