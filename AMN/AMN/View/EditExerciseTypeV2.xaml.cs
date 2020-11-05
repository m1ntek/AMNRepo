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
    public partial class EditExerciseTypeV2 : ContentPage
    {
        public int Index { get; set; }
        public string Key { get; set; }
        public Exercise Exercise { get; set; }
        public ExerciseType SelectedType { get; set; }
        public string EntryTypePlaceholder { get; set; }
        public string HeaderTitle { get; set; }
        public Entry RepEntry { get; set; }
        public EditExerciseTypeV2(int index, string key)
        {
            InitializeComponent();
            Index = index;
            Key = key;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            Exercise = await MasterModel.DAL.GetSelectedExerciseAsync(Key);
            Exercise.Key = Key;
            SelectedType = Exercise.Types[Index];

            if (string.IsNullOrEmpty(SelectedType.Name) == true)
            {
                HeaderTitle = "Exercise Type";
                EntryTypePlaceholder = "if applicable";
            }
            else HeaderTitle = SelectedType.Name;

            Refresh();
        }

        private void Refresh()
        {
            BindingContext = null;
            BindingContext = this;
        }

        private int GetCurrentIndex(object sender)
        {
            var button = (Button)sender;
            var rep = (Rep)button.CommandParameter;
            return SelectedType.Reps.IndexOf(rep);
        }

        private async void Del_Clicked(object sender, EventArgs e)
        {
            var repIndex = GetCurrentIndex(sender);

            if(repIndex == 0)
            {
                await DisplayAlert("Error", "Must have at least 1 rep.", "OK");
            }
            else
            {
                SelectedType.Reps.RemoveAt(repIndex);
                Refresh();
            }
        }

        private void AddRep_Clicked(object sender, EventArgs e)
        {
            SelectedType.Reps.Add(new Rep());
            Refresh();
        }

        private async void Save_Clicked(object sender, EventArgs e)
        {
            //Prep a string array for validator
            string[] repValues = new string[SelectedType.Reps.Count];
            for (int i = 0; i < SelectedType.Reps.Count; ++i)
            {
                repValues[i] = SelectedType.Reps[i].Amount;
            }

            //Stop if invalid
            if(MasterModel.vd.FormEntriesValid(repValues) == false)
            {
                await DisplayAlert("Error", MasterModel.vd.error, "OK");
                return;
            }


            await MasterModel.DAL.SaveSelectedExerciseAsync(Exercise);
            await Navigation.PopAsync();
        }

        private async void Delete_Clicked(object sender, EventArgs e)
        {
            await MasterModel.DAL.DeleteSelectedExerciseTypeAsync(Exercise, Index);
            await Navigation.PopAsync();
        }
    }
}