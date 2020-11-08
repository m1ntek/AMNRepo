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
        private int currentRow;
        private string repeatableLblText;
        public EditExerciseType(int index, string key)
        {
            InitializeComponent();
            Index = index;
            Key = key;
            repeatableLblText = "Rep:";
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            Exercise = await MasterModel.DAL.GetSelectedExerciseAsync(Key);
            Exercise.Key = Key; //Save the key to exercise as well
            SelectedType = Exercise.Types[Index];

            await UpdateFormReps();
            await FillBlankName();

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

        private async Task FillBlankName()
        {
            if(string.IsNullOrEmpty(SelectedType.Name) == true)
            {
                lblHeader.Text = "Exercise Type";
                entryType.Placeholder = "if applicable";
            }
        }

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
        }

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