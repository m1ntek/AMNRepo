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

            //await MasterModel.DAL.SaveSelectedExerciseLoadoutAsync(CurrentLoadout);
            await GetSelectedExerciseLoadout();
            Refresh();
        }

        private void Refresh()
        {
            BindingContext = null;
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
            await Navigation.PushAsync(new SelectExerciseLoadout());
        }
    }
}