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
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms.Design;

namespace Mhora.Components.File;

public class UiStringTypeEditor : UITypeEditor
{
	private IWindowsFormsEditorService _edSvc;

	public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
	{
		_edSvc = (IWindowsFormsEditorService) provider.GetService(typeof(IWindowsFormsEditorService));
		var stringInit = string.Empty;
		if (value is string)
		{
			stringInit = (string) value;
		}

		var le = new LongStringEditor(stringInit);
		le.TitleText = "Event Description";
		_edSvc.ShowDialog(le);
		return le.EditorText;
	}

	public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context) => UITypeEditorEditStyle.Modal;
}