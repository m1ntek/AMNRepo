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
    public partial class AddExercisesToLoadout : ContentPage
    {
        public List<Exercise> SavedExercises { get; set; }
        private ExerciseLoadout tempELoadout;
        private List<Task> taskScheduler;
        public AddExercisesToLoadout()
        {
            InitializeComponent();
            SavedExercises = new List<Exercise>();
            tempELoadout = new ExerciseLoadout();
            taskScheduler = new List<Task>();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
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
                tempELoadout.Exercises.Add(SavedExercises[e.ItemIndex]);
                taskScheduler.Add(MasterModel.DAL.SaveNewTempLoadoutExerciseAsync(tempELoadout));
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
            await Task.WhenAll(taskScheduler);
            await Navigation.PopAsync();
        }
    }
}