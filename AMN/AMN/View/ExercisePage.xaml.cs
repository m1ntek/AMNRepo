using AMN.Controller;
using AMN.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AMN.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExercisePage : ContentPage
    {
        public ExerciseLoadout CurrentLoadout { get; set; }
        public ExercisePage()
        {
            InitializeComponent();
            CurrentLoadout = new ExerciseLoadout();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            //Temp data
            //CurrentLoadout.Name = "PUSH I";
            //CurrentLoadout.Sets = 4;

            //Exercise ex = new Exercise();
            //ex.Name = "Flat bench";
            //ExerciseType type = new ExerciseType();
            //type.Name = "HT:";
            //type.Reps.Add("10+");
            //ExerciseType type2 = new ExerciseType();
            //type2.Name = "OT:";
            //type2.Reps.Add("10");
            //type2.Reps.Add(">");
            //type2.Reps.Add("8");
            //type2.Reps.Add(">");
            //type2.Reps.Add("8");
            //type2.Reps.Add(">");
            //type2.Reps.Add("6");
            //ex.Types.Add(type);
            //ex.Types.Add(type2);
            //CurrentLoadout.Exercises.Add(ex);

            //ex = new Exercise();
            //ex.Name = "Standing military press";
            //type2 = new ExerciseType();
            //type2.Reps.Add("12");
            //type2.Reps.Add(">");
            //type2.Reps.Add("10");
            //type2.Reps.Add(">");
            //type2.Reps.Add("8");
            //type2.Reps.Add(">");
            //type2.Reps.Add("8");
            //ex.Types.Add(type2);
            //CurrentLoadout.Exercises.Add(ex);

            await MasterModel.DAL.SaveSelectedExerciseLoadoutAsync(CurrentLoadout);

            await GetSelectedExerciseLoadout();
            BindingContext = this;
        }

        private async Task GetSelectedExerciseLoadout()
        {
            try
            {
                CurrentLoadout = await MasterModel.DAL.GetSelectedExerciseLoadoutAsync();
                //LoadoutExercises = new ObservableCollection<Exercise>(CurrentLoadout.Exercises);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async void EditExercise_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EditExercisePage());
        }

        private async void EditLoadouts_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ExerciseLoadouts());
        }

        private async void SelectLoadout_Clicked(object sender, EventArgs e)
        {

        }
    }
}