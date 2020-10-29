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
        public Exercise Exercise { get; set; }
        private string repeatableLblText;
        private int defaultMaxIndex;
        public NewExercise()
        {
            InitializeComponent();
            Exercise = new Exercise();
            defaultMaxIndex = gridForm.Children.Count - 1;
            repeatableLblText = "Reps:";
        }

        private void entryName_Unfocused(object sender, FocusEventArgs e)
        {
            entryType.Focus();
        }

        private async void NewType_Clicked(object sender, EventArgs e)
        {
            if (MasterModel.vd.FormEntriesValid(new string[] { entryRep.Text }) == true)
            {
                List<Rep> tempReps = RepController.SaveRepsFromEntries(gridForm, repeatableLblText);
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
            FormController.ResetEntries(new List<Entry> { entryType, entryRep });
            Grid.SetRow(btnAddRep, Grid.GetRow(entryRep));
            gridForm = FormController.DeleteGridElements(gridForm, gridForm.Children.Count - 1, defaultMaxIndex);
        }
        private void ResetFormOld()
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

            ++currentRow;

            gridForm.Children.Add(FormController.NewGridLabel(0, currentRow, repeatableLblText));
            gridForm.Children.Add(FormController.NewGridRepEntry(1, currentRow, null));

            //Move button down with the new row
            Grid.SetRow(btnAddRep, currentRow);

            //Focus on the new entry
            gridForm.Children[gridForm.Children.Count - 1].Focus();
        }

        private async Task SaveData()
        {
            Exercise.Name = entryName.Text;

            try
            {
                await MasterModel.DAL.SaveNewExerciseAsync(Exercise);
                await Navigation.PopAsync();
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
                    if(MasterModel.vd.FormEntriesValid(FormController.GetAllEntries(gridForm))==true)
                    {
                        Exercise.Types.Add(new ExerciseType { Name = entryType.Text, Reps = RepController.SaveRepsFromEntries(gridForm, repeatableLblText) });
                    }
                    await SaveData();
                    return;
                }
            }

            if (MasterModel.vd.FormEntriesValid(FormController.GetAllEntries(gridForm)) == false)
            {
                await DisplayAlert("Error", MasterModel.vd.error, "OK");
                return;
            }

            Exercise.Types.Add(new ExerciseType { Name = entryType.Text, Reps = RepController.SaveRepsFromEntries(gridForm, repeatableLblText) });
            await SaveData();
        }

        private void entryType_Unfocused(object sender, FocusEventArgs e)
        {
            entryRep.Focus();
        }

        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {

        }
    }
}