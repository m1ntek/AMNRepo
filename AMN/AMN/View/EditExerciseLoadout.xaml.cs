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
    public partial class EditExerciseLoadout : ContentPage
    {
        public ExerciseLoadout ELoadout { get; set; }
        public ExerciseLoadout SelectedELoadout { get; set; }
        public string Key { get; set; }
        public EditExerciseLoadout(string key)
        {
            InitializeComponent();
            Key = key;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            ELoadout = await MasterModel.DAL.GetExerciseLoadoutExerciseAsync(Key);
            SelectedELoadout = await MasterModel.DAL.GetSelectedExerciseLoadoutAsync();
            ELoadout.Key = Key;
            Refresh();
        }

        private void Refresh()
        {
            BindingContext = null;
            BindingContext = this;
        }

        private async void AddExercises_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddExercisesToSelectedLoadout(Key));
        }

        private async void Save_Clicked(object sender, EventArgs e)
        {
            if (await Validation() == false)
                return;

            //Update the selected loadout if it is the same as this one.
            if(SelectedELoadout.Key == Key)
            {
                await MasterModel.DAL.SaveSelectedExerciseLoadoutAsync(ELoadout);
            }

            await MasterModel.DAL.SaveExerciseLoadoutAsync(ELoadout, Key);
            await Navigation.PopAsync();
        }

        private async Task<bool> Validation()
        {
            if (MasterModel.vd.FormEntriesValid(new string[] { ELoadout.Name, ELoadout.Sets.ToString() }) == false)
            {
                await DisplayAlert("Error", MasterModel.vd.error, "OK");
                return false;
            }

            if (ELoadout.Exercises.Count == 0)
            {
                await DisplayAlert("Error", "Please have at least one exercise in your loadout.", "OK");
                return false;
            }

            return true;
        }

        private void Del_Clicked(object sender, EventArgs e)
        {
            //Find index of click event
            var button = (Button)sender;
            var exercise = (Exercise)button.CommandParameter;
            var index = ELoadout.Exercises.IndexOf(exercise);

            ELoadout.Exercises.RemoveAt(index);
            Refresh();
        }
    }
}