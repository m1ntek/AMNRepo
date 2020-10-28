using AMN.Controller;
using AMN.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
        public ObservableCollection<Exercise> LoadoutExercises { get; set; }
        public ExercisePage()
        {
            InitializeComponent();
            CurrentLoadout = new ExerciseLoadout();
            //BindingContext = this;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await GetSelectedExerciseLoadout();
            BindingContext = this;
        }

        private async Task GetSelectedExerciseLoadout()
        {
            try
            {
                CurrentLoadout = await MasterModel.DAL.GetSelectedExerciseLoadoutAsync();
                LoadoutExercises = new ObservableCollection<Exercise>(CurrentLoadout.Exercises);
            }
            catch (Exception ex)
            {
                //This violates a MVVM rule (ViewModel shouldn't talk to View),
                //but due to time constraint, cannot implement the correct
                //solution in time.
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private void Exercise_ItemTapped(object sender, ItemTappedEventArgs e)
        {

        }
    }
}