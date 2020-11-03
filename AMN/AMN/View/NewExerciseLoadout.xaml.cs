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
    public partial class NewExerciseLoadout : ContentPage
    {
        public ExerciseLoadout ELoadout { get; set; }
        public NewExerciseLoadout()
        {
            InitializeComponent();
            ELoadout = new ExerciseLoadout();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            ELoadout = await MasterModel.DAL.GetTempExerciseLoadoutAsync();
        }

        private async void AddExercises_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddExercisesToLoadout());
        }

        private async void Save_Clicked(object sender, EventArgs e)
        {
            await MasterModel.DAL.SaveNewExerciseLoadoutAsync(ELoadout);
            await Navigation.PopAsync();
        }

        private async void Exercise_ItemTapped(object sender, ItemTappedEventArgs e)
        {

        }
    }
}