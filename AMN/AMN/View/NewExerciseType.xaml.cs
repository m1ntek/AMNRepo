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

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            _Exercise = await MasterModel.DAL.GetSelectedExerciseAsync(Key);
            _Exercise.Key = Key; //Save the key to exercise as well
            _ExerciseType = new ExerciseType();
        }

        private async void btnAddRep_Clicked(object sender, EventArgs e)
        {
            gridForm = await FormController.AddRepClicked(
                gridForm,
                btnAddRep,
                repeatableLblText,
                _ExerciseType);
        }

        private async void Save_Clicked(object sender, EventArgs e)
        {
            _Exercise.Types.Add(_ExerciseType);
            await MasterModel.DAL.SaveSelectedExerciseAsync(_Exercise);
            await Navigation.PopAsync();
        }
    }
}