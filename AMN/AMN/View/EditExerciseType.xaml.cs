using AMN.Controller;
using AMN.Model;
using Newtonsoft.Json.Serialization;
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
    public partial class EditExerciseType : ContentPage
    {
        public int Index { get; set; }
        public string Key { get; set; }
        public Exercise Exercise { get; set; }
        public ExerciseType SelectedType { get; set; }
        private int currentMaxIndex;
        private int currentRow;
        private string repeatableLblText;
        public EditExerciseType(int index, string key)
        {
            InitializeComponent();
            Index = index;
            Key = key;
            currentMaxIndex = gridForm.Children.Count - 1;
            repeatableLblText = "Rep:";
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            Exercise = await MasterModel.DAL.GetSelectedExerciseAsync(Key);
            Exercise.Key = Key; //Save the key to exercise as well
            SelectedType = Exercise.Types[Index];
            //await LoadReps();
            await UpdateFormReps();

            await FillBlankName();
            //await ManageDeleteButton();
            gridForm = await FormController.ManageDeleteButton(gridForm);
            BindingContext = this;
        }

        private async Task UpdateFormReps()
        {
            MultipleAttributes ma = await FormController.LoadReps(new MultipleAttributes(
                gridForm,
                SelectedType,
                currentRow,
                btnAddRep,
                repeatableLblText));

            gridForm = ma.grid;
            SelectedType = ma.type;
            currentRow = ma.num;
            btnAddRep = ma.btn;
            repeatableLblText = ma.text;
        }

        //private async Task ManageDeleteButton()
        //{
        //    var lastChild = gridForm.Children[gridForm.Children.Count - 1];
        //    if (Grid.GetRow(lastChild) == 1)
        //    {
        //        //Hide delete button
        //        gridForm.Children[gridForm.Children.Count - 1].IsVisible = false;
        //    }
        //}

        private async Task FillBlankName()
        {
            if(string.IsNullOrEmpty(SelectedType.Name) == true)
            {
                lblHeader.Text = "Exercise Type";
                entryType.Placeholder = "Unnamed";
            }
        }

        //private async Task LoadReps()
        //{
        //    currentRow = Grid.GetRow(entryType);
        //    for (int i = 0; i < SelectedType.Reps.Count; i++)
        //    {
        //        SelectedType.Reps[i].Index = i;
        //        ++currentRow;

        //        gridForm.Children.Add(FormController.NewGridLabel(0, currentRow, repeatableLblText));

        //        //Fill in each rep entry
        //        Entry tempEntry = FormController.NewGridRepEntry(1, currentRow, null);
        //        tempEntry.Text = SelectedType.Reps[i].Amount;
        //        gridForm.Children.Add(tempEntry);

        //        await AddDeleteButton();

        //        //Move add button down with the new row
        //        Grid.SetRow(btnAddRep, currentRow);
        //    }
        //}

        private async void btnAddRep_Clicked(object sender, EventArgs e)
        {
            gridForm = await FormController.AddRepClicked(
                gridForm,
                btnAddRep,
                repeatableLblText,
                SelectedType);

            //Validation, null is invalid.
            if (gridForm == null)
                return;


            ////Grab the last entry in grid and cast it as Entry type.
            //Entry tempEntry = (Entry)gridForm.Children[gridForm.Children.Count - 2];

            //if (MasterModel.vd.FormEntriesValid(new string[] { tempEntry.Text }) == false)
            //    return;

            //currentRow = Grid.GetRow(btnAddRep);

            //++currentRow;

            //gridForm.Children.Add(FormController.NewGridLabel(0, currentRow, repeatableLblText));
            //gridForm.Children.Add(FormController.NewGridRepEntry(1, currentRow, null));

            ////Move button down with the new row
            //Grid.SetRow(btnAddRep, currentRow);

            ////Focus on the new entry
            //gridForm.Children[gridForm.Children.Count - 1].Focus();

            ////Add another delete button
            //await AddDeleteButton();
        }

        //private async Task AddDeleteButton()
        //{
        //    //Add a delete button
        //    Button delete = FormController.NewGridButton(3, currentRow, "Del");
        //    gridForm.Children.Add(delete); //Prevent messing with last indices

        //    //Click event
        //    delete.Clicked += async (sender, args) =>
        //    {

        //        var lastChild = gridForm.Children[gridForm.Children.Count - 1];

        //        int deleteRow = Grid.GetRow((Button)sender); //Find grid row from clicked button
        //        int deleteIndex = gridForm.Children.IndexOf((Button)sender); //Find index of button in the grid
        //        int detectedRepIndex = deleteRow - 1; //Formula to find rep index based on delete button row.

        //        //Delete the grid elements
        //        gridForm.Children.RemoveAt(deleteIndex); //Rep del button
        //        gridForm.Children.RemoveAt(deleteIndex - 1); //Rep entry
        //        gridForm.Children.RemoveAt(deleteIndex - 2); //Rep label

        //        //Delete the rep itself
        //        try
        //        {
        //            SelectedType.Reps.RemoveAt(detectedRepIndex);
        //        }
        //        catch (Exception)
        //        {
        //            //Do nothing
        //        }

        //        //Decrement currentRow
        //        --currentRow;

        //        //Move add button
        //        Grid.SetRow(btnAddRep, currentRow);

        //        //Check delete button
        //        await ManageDeleteButton();
        //    };
        //}

        private void ResetForm()
        {
            //Move add button to default position
            Grid.SetRow(btnAddRep, 1);

            //Delete extra rep grid elements.
            gridForm = FormController.DeleteGridElements(gridForm, gridForm.Children.Count - 1, 5);

            //Clear entries
            Entry entryRep = (Entry)gridForm.Children[4]; //Grab entryRep from grid
            var resetEntries = FormController.ResetEntries(new List<Entry> { entryType, entryRep });
            entryType = resetEntries[0]; //Save back to grid's entry
            
            //Save back to grid's entry
            gridForm.Children.RemoveAt(4);
            gridForm.Children.Insert(4, resetEntries[1]); 
        }

        private void RefreshPage()
        {
            BindingContext = null;
            BindingContext = this;
        }

        //private async void NewType_Clicked(object sender, EventArgs e)
        //{
        //    if (MasterModel.vd.FormEntriesValid(FormController.GetAllEntries(gridForm)) == true)
        //    {
        //        List<Rep> tempReps = RepController.SaveRepsFromEntries(gridForm, repeatableLblText);
        //        Exercise.Types.Add(new ExerciseType
        //        {
        //            Name = entryType.Text,
        //            Reps = tempReps
        //        });
        //        ResetForm();
        //        await ManageDeleteButton();
        //        entryType.Focus();
        //        Exercise.Types.Add(SelectedType);
        //        SelectedType = new ExerciseType();
        //        RefreshPage();
                
        //    }
        //    else
        //    {
        //        await DisplayAlert("Error", MasterModel.vd.error, "OK");
        //    }
        //}

        private async void Save_Clicked(object sender, EventArgs e)
        {
            //If entries are valid
            if(MasterModel.vd.FormEntriesValid( FormController.GetAllEntries(gridForm)) == true)
            {
                try
                {
                    //Save
                    SelectedType.Reps = RepController.SaveRepsFromEntries(gridForm, repeatableLblText);
                    Exercise.Types[Index] = SelectedType;
                    await MasterModel.DAL.SaveSelectedExerciseAsync(Exercise);
                    await Navigation.PopAsync();
                }
                catch (Exception ex)
                {
                    //Display error
                    await DisplayAlert("Error", ex.Message, "OK");
                    throw;
                }
                
            }
            //Entries are invalid
            else
            {
                //Display error
                await DisplayAlert("Error", MasterModel.vd.error, "OK");
            }
            
        }

        private async void Delete_Clicked(object sender, EventArgs e)
        {
            await MasterModel.DAL.DeleteSelectedExerciseTypeAsync(Exercise, Index);
            await Navigation.PopAsync();
        }
    }
}