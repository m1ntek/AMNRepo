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
    public partial class SelectedExercise : ContentPage
    {
        public string Key { get; set; }
        public int currentRow { get; set; }
        private string repeatableLblText;
        private int defaultMaxIndex;
        public Exercise Exercise { get; set; }
        public SelectedExercise(string key)
        {
            InitializeComponent();
            Exercise = new Exercise();
            Key = key;
            repeatableLblText = "Reps:";
            defaultMaxIndex = -1;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            Exercise = await GetExercise();
            Exercise.Key = Key;
            BindingContext = null;
            BindingContext = this;
        }

        private Task<Exercise> GetExercise()
        {
            return MasterModel.DAL.GetSelectedExerciseAsync(Key);
        }

        private void FillForm()
        {

        }

        private void AddRep_Clicked(object sender, EventArgs e)
        {

        }

        private async void Type_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            await Navigation.PushAsync(new EditExerciseType(e.ItemIndex, Key));
        }

        private async void Save_Clicked(object sender, EventArgs e)
        {
            Exercise.Name = entryRename.Text;
            await MasterModel.DAL.SaveSelectedExerciseAsync(Exercise);
            await Navigation.PopAsync();
        }
    }
}