using AMN.Controller;
using AMN.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AMN.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddExercisesToSelectedLoadout : ContentPage
    {
        public List<Exercise> SavedExercises { get; set; }
        public string Key { get; set; }
        private ExerciseLoadout selectedLoadout;

        public AddExercisesToSelectedLoadout(string key)
        {
            InitializeComponent();
            Key = key;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            selectedLoadout = await MasterModel.DAL.GetExerciseLoadoutAsync(Key);
            SavedExercises = await MasterModel.DAL.GetSavedExercisesAsync();
            SavedExercises = await RepController.PrepareRepSummariesAsync(SavedExercises);
            Refresh();
        }

        private void Refresh()
        {
            BindingContext = null;
            BindingContext = this;
        }

        private async void Exercise_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            string summary = await PrepareAddExerciseSummary(SavedExercises[e.ItemIndex]);
            bool addConfirmed = await DisplayAlert(
                SavedExercises[e.ItemIndex].Name,
                "Add this exercise?\n\n" +
                summary,
                "Yes",
                "No");

            if (addConfirmed == true)
            {
                selectedLoadout.Exercises.Add(SavedExercises[e.ItemIndex]);
                await MasterModel.DAL.SaveSelectedExerciseLoadoutAsync(selectedLoadout, Key);
            }
        }

        private async Task<string> PrepareAddExerciseSummary(Exercise ex)
        {
            string summary = null;
            ex = await RepController.PrepareRepSummaryAsync(ex);

            foreach (var eType in ex.Types)
            {
                summary += (eType.Summary + "\n");
            }

            return summary;
        }

        private async void Done_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}