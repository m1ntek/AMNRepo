using AMN.Controller;
using AMN.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Markup;
using Xamarin.Forms.Xaml;

namespace AMN.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewExercise : ContentPage
    {
        public int currentRow { get; set; }
        public int currentCol { get; set; }
        public Exercise Exercise { get; set; }
        private string repeatableLblText = "Reps:";
        private int defaultMaxIndex = -1;
        public NewExercise()
        {
            InitializeComponent();
            Exercise = new Exercise();
            defaultMaxIndex = gridForm.Children.Count - 1;
        }

        private void entryName_Unfocused(object sender, FocusEventArgs e)
        {

        }

        private async void NewType_Clicked(object sender, EventArgs e)
        {
            if (MasterModel.vd.FormEntriesValid(new string[] { entryRep.Text }) == true)
            {
                List<string> tempReps = SaveReps();
                Exercise.Types.Add(new ExerciseType
                {
                    Name = entryType.Text,
                    Reps = tempReps
                });
                ResetForm();
                entryType.Focus();
            }
            else
            {
                await DisplayAlert("Error", MasterModel.vd.error, "OK");
            }
        }

        //Reset all except name.
        private void ResetForm()
        {
            entryType.Text = "";
            entryRep.Text = "";
            Grid.SetRow(btnAddRep, Grid.GetRow(entryRep));
            int i = gridForm.Children.Count - 1;
            while (i > defaultMaxIndex)
            {
                gridForm.Children.RemoveAt(i);
                --i;
            }
        }

        private void AddRep_Clicked(object sender, EventArgs e)
        {
            //if first rep is blank then just return.
            if (MasterModel.vd.FormEntriesValid(new string[] { entryRep.Text }) == false)
                return;

            currentRow = Grid.GetRow(btnAddRep);
            //currentCol = Grid.GetColumn(btnAddRep);

            ++currentRow;

            gridForm.Children.Add(FormController.NewGridLabel(0, currentRow, repeatableLblText));
            gridForm.Children.Add(FormController.NewGridRepEntry(1, currentRow, null));

            //Move button down with the new row
            Grid.SetRow(btnAddRep, currentRow);

            //Focus on the new entry
            gridForm.Children[gridForm.Children.Count - 1].Focus();
        }

        private List<string> SaveReps()
        {
            List<string> reps = new List<string>();
            for (int i = 0; i < gridForm.Children.Count; i++)
            {
                if (gridForm.Children[i].GetType() == typeof(Label))
                {
                    Label tempLbl = (Label)gridForm.Children[i];
                    if (tempLbl.Text == repeatableLblText)
                    {

                        Entry tempEntry = (Entry)gridForm.Children[i + 1];
                        if (MasterModel.vd.FormEntriesValid(new string[] { tempEntry.Text }) == false)
                        {
                            DisplayAlert("Error", MasterModel.vd.error, "OK");
                            return null;
                        }
                        reps.Add(tempEntry.Text);
                    }
                }
            }
            return reps;
        }

        //Get all entries but type
        private string[] GetAllEntries()
        {
            //Have to jerry rig this to be a list first and then back to array,
            //if time permits, I'll change the string array into a string list
            //for the validator, don't know why I did string array to begin with...
            List<string> entries = new List<string>();
            //foreach (var item in gridForm.Children)
            //{
            //    if(item.GetType() == typeof(Entry))
            //    {
            //        Entry tempEntry = (Entry)item;
            //        entries.Add(tempEntry.Text);
            //    }
            //}
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


            string[] entriesArray = new string[entries.Count];
            for (int i = 0; i < entries.Count; i++)
            {
                entriesArray[i] = entries[i];
            }
            return entriesArray;
        }

        private async Task SaveData()
        {
            Exercise.Name = entryName.Text;

            try
            {
                await MasterModel.DAL.SaveNewExerciseAsync(Exercise);
                await Navigation.PopAsync();
                //entryName.Text = "";
                //ResetForm();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async void Save_Clicked(object sender, EventArgs e)
        {
            //Ugly band-aid fix for if types already exist but user just
            //wants to save...
            if (MasterModel.vd.FormEntriesValid(new string[] { entryName.Text }) == true)
            {
                if (Exercise.Types.Count > 0)
                {
                    if(MasterModel.vd.FormEntriesValid(GetAllEntries())==true)
                    {
                        Exercise.Types.Add(new ExerciseType { Name = entryType.Text, Reps = SaveReps() });
                    }
                    await SaveData();
                    return;
                }
            }

            if (MasterModel.vd.FormEntriesValid(GetAllEntries()) == false)
            {
                await DisplayAlert("Error", MasterModel.vd.error, "OK");
                return;
            }

            Exercise.Types.Add(new ExerciseType { Name = entryType.Text, Reps = SaveReps() });
            await SaveData();
        }
    }
}