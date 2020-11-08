using AMN.Model;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AMN.Controller
{
    /* Initially an attempt to refactor logic that creates food items
     * ended up just being a refactored label template creator. */
    public class CardCreator
    {
        public static Label CreateLabelTemplate(Label lbl, LayoutOptions horizontalLayout, int gridColumn, int gridRow, int columnSpan, string name)
        {
            lbl.TextColor = Color.Black;
            lbl.VerticalOptions = LayoutOptions.Center;
            lbl.HorizontalOptions = horizontalLayout;
            Grid.SetRow(lbl, gridRow);
            Grid.SetColumn(lbl, gridColumn);
            Grid.SetColumnSpan(lbl, columnSpan);
            lbl.Text = name;
            lbl.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));

            return lbl;
        }
    }
}
