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
            BindingContext = null;
            BindingContext = this;
        }

        private async void AddExercises_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddExercisesToLoadout());
        }

        private async void Save_Clicked(object sender, EventArgs e)
        {
            if (await Validation() == false)
                return;

            await MasterModel.DAL.SaveNewExerciseLoadoutAsync(ELoadout);
            await Navigation.PopAsync();
        }

        private async Task<bool> Validation()
        {
            if (MasterModel.vd.FormEntriesValid(new string[] { ELoadout.Name, ELoadout.Sets.ToString() }) == false)
            {
                await DisplayAlert("Error", MasterModel.vd.error, "OK");
                return false;
            }

            if(ELoadout.Exercises.Count== 0)
            {
                await DisplayAlert("Error", "Please have at least one exercise in your loadout.", "OK");
                return false;
            }

            return true;
        }

        private async void Name_Unfocused(object sender, FocusEventArgs e)
        {
            await MasterModel.DAL.SaveNewTempLoadoutExerciseAsync(ELoadout);
        }

        private async void Sets_Unfocused(object sender, FocusEventArgs e)
        {
            await MasterModel.DAL.SaveNewTempLoadoutExerciseAsync(ELoadout);
        }
    }
}