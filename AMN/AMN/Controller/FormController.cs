using AMN.Model;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
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

        public static int GetRepIndex(object sender, ExerciseType exType)
        {
            var button = (Button)sender;
            var rep = (Rep)button.CommandParameter;
            return exType.Reps.IndexOf(rep);
        }

        public static async Task<MultipleAttributes> LoadReps(MultipleAttributes ma)
        {
            //Clarify the attributes
            Grid gridForm = ma.grid;
            int currentRow = Grid.GetRow(gridForm.Children[0]);
            ExerciseType selectedType = ma.type;
            string repeatableLblText = ma.text;
            Button btnAddRep = ma.btn;
            
            for (int i = 0; i < selectedType.Reps.Count; i++)
            {
                selectedType.Reps[i].Index = i;
                ++currentRow;

                gridForm.Children.Add(NewGridLabel(0, currentRow, repeatableLblText));

                //Fill in each rep entry
                Entry tempEntry = NewGridRepEntry(1, currentRow, null);
                tempEntry.Text = selectedType.Reps[i].Amount;
                gridForm.Children.Add(tempEntry);

                gridForm = await AddDeleteButton(currentRow, gridForm, selectedType, btnAddRep);

                //Move add button down with the new row
                Grid.SetRow(btnAddRep, currentRow);
            }

            //Save changes back to class and return it
            ma.grid = gridForm;
            ma.num = currentRow;
            ma.type = selectedType;
            ma.text = repeatableLblText;
            ma.btn = btnAddRep;

            return ma;
        }

        private static async Task<Grid> AddDeleteButton(
            int currentRow,
            Grid gridForm,
            ExerciseType SelectedType,
            Button btnAddRep)
        {
            //Add a delete button
            Button delete = NewGridButton(3, currentRow, "Del");
            gridForm.Children.Add(delete); //Prevent messing with last indices

            //Click event
            delete.Clicked += async (sender, args) =>
            {
                var lastChild = gridForm.Children[gridForm.Children.Count - 1];

                int deleteRow = Grid.GetRow((Button)sender); //Find grid row from clicked button
                int deleteIndex = gridForm.Children.IndexOf((Button)sender); //Find index of button in the grid
                int detectedRepIndex = deleteRow - 1; //Formula to find rep index based on delete button row.

                //Delete the grid elements
                gridForm.Children.RemoveAt(deleteIndex); //Rep del button
                gridForm.Children.RemoveAt(deleteIndex - 1); //Rep entry
                gridForm.Children.RemoveAt(deleteIndex - 2); //Rep label

                //Delete the rep itself
                try
                {
                    SelectedType.Reps.RemoveAt(detectedRepIndex);
                }
                catch (Exception)
                {
                    //Do nothing
                }

                //Decrement currentRow
                --currentRow;

                //Move add button
                Grid.SetRow(btnAddRep, currentRow);

                //Check delete button
                gridForm = await ManageDeleteButton(gridForm);
            };

            return gridForm;
        }

        public static async Task<Grid> ManageDeleteButton(Grid gridForm)
        {
            var lastChild = gridForm.Children[gridForm.Children.Count - 1];
            if (Grid.GetRow(lastChild) == 1)
            {
                //Hide delete button
                gridForm.Children[gridForm.Children.Count - 1].IsVisible = false;
            }

            return gridForm;
        }

        public static async Task<Grid> AddRepClicked(
            Grid gridForm,
            Button btnAddRep,
            string repeatableLblText,
            ExerciseType selectedType)
        {
            //Grab the last entry in grid and cast it as Entry type.
            Entry tempEntry = (Entry)gridForm.Children[gridForm.Children.Count - 2];

            if (MasterModel.vd.FormEntriesValid(new string[] { tempEntry.Text }) == false)
                return null;

            int currentRow = Grid.GetRow(btnAddRep);

            ++currentRow;

            gridForm.Children.Add(NewGridLabel(0, currentRow, repeatableLblText));
            gridForm.Children.Add(NewGridRepEntry(1, currentRow, null));

            //Move button down with the new row
            Grid.SetRow(btnAddRep, currentRow);

            //Focus on the new entry
            gridForm.Children[gridForm.Children.Count - 1].Focus();

            //Add another delete button
            gridForm = await AddDeleteButton(currentRow, gridForm, selectedType, btnAddRep);

            //Return updated grid
            return gridForm;
        }
    }
}
