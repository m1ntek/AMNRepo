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
    public partial class NewExerciseType : ContentPage
    {
        public string Key { get; set; }
        public Exercise _Exercise { get; set; }
        public ExerciseType _ExerciseType { get; set; }
        private string repeatableLblText;

        public NewExerciseType(string key)
        {
            InitializeComponent();
            Key = key;
            repeatableLblText = "Rep:";
        }

        private void Refresh()
        {
            BindingContext = null;
            BindingContext = this;
        }

        private void AddRep()
        {
            _ExerciseType.Reps.Add(new Rep());
            Refresh();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            _Exercise = await MasterModel.DAL.GetSelectedExerciseAsync(Key);
            _Exercise.Key = Key; //Save the key to exercise as well
            _ExerciseType = new ExerciseType();
            _ExerciseType.ExerciseKey = Key;
            AddRep();
            BindingContext = this;
        }

        private async void btnAddRep_Clicked(object sender, EventArgs e)
        {
            AddRep();
        }

        private async void Save_Clicked(object sender, EventArgs e)
        {
            _Exercise.Types.Add(_ExerciseType);
            await MasterModel.DAL.SaveSelectedExerciseAsync(_Exercise);
            await Navigation.PopAsync();
        }

        private async void Del_Clicked(object sender, EventArgs e)
        {
            int repIndex = FormController.GetRepIndex(sender, _ExerciseType);

            if (repIndex == 0)
            {
                await DisplayAlert("Error", "Must have at least 1 rep.", "OK");
            }
            else
            {
                _ExerciseType.Reps.RemoveAt(repIndex);
                Refresh();
            }
        }
    }
}