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
using Mhora.Components;

namespace Mhora.Hora;

public class UIStringTypeEditor : UITypeEditor
{
    private IWindowsFormsEditorService edSvc;

    public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
    {
        edSvc = (IWindowsFormsEditorService) provider.GetService(typeof(IWindowsFormsEditorService));
        var stringInit = string.Empty;
        if (value is string)
        {
            stringInit = (string) value;
        }

        var le = new LongStringEditor(stringInit);
        le.TitleText = "Event Description";
        edSvc.ShowDialog(le);
        return le.EditorText;
    }

    public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
    {
        return UITypeEditorEditStyle.Modal;
    }
}