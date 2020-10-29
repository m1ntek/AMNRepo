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

        public static Button NewGridButton(int col, int row, string btnText)
        {
            Button btn = new Button();
            Grid.SetColumn(btn, col);
            Grid.SetRow(btn, row);
            btn.Text = btnText;

            return btn;
        }

        public static string[] GetAllEntries(Grid gridForm)
        {
            //Have to jerry rig this to be a list first and then back to array,
            //if time permits, I'll change the string array into a string list
            //for the validator, don't know why I did string array to begin with...
            List<string> entries = new List<string>();

            for (int i = 0; i < gridForm.Children.Count; i++)
            {
                if (gridForm.Children[i].GetType() == typeof(Label))
                {
                    Label tempLbl = (Label)gridForm.Children[i];
                    if (tempLbl.Text == "Name:" || tempLbl.Text == "Reps:")
                    {
                        Entry tempEntry = (Entry)gridForm.Children[i + 1];
                        entries.Add(tempEntry.Text);
                    }
                }
            }

            //Conversion
            string[] entriesArray = new string[entries.Count];
            for (int i = 0; i < entries.Count; i++)
            {
                entriesArray[i] = entries[i];
            }
            return entriesArray;
        }

        public static List<Entry> ResetEntries(List<Entry> entries)
        {
            foreach (var item in entries)
            {
                item.Text = "";
            }

            return entries;
        }

        public static Grid DeleteGridElements(Grid gridForm, int fromNum, int untilNum)
        {
            int i = fromNum;
            while(i>untilNum)
            {
                gridForm.Children.RemoveAt(i);
                --i;
            }

            return gridForm;
        }

    }
}
