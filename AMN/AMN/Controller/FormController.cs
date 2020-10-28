using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AMN.Controller
{
    public class FormController
    {
        public static Label NewGridLabel(int col, int row, string lblText)
        {
            Label lbl = new Label();
            Grid.SetColumn(lbl, col);
            Grid.SetRow(lbl, row);
            lbl.Text = lblText;
            lbl.VerticalOptions = LayoutOptions.Center;

            return lbl;
        }

        public static Entry NewGridRepEntry(int col, int row, string placeholder)
        {
            Entry entry = new Entry();
            Grid.SetColumn(entry, col);
            Grid.SetRow(entry, row);
            entry.Placeholder = placeholder;
            entry.Keyboard = Keyboard.Telephone;

            return entry;
        }
            
    }
}
