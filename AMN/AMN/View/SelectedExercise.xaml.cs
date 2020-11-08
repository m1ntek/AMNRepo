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
        public Exercise Exercise { get; set; }
        public SelectedExercise(string key)
        {
            InitializeComponent();
            Exercise = new Exercise();
            Key = key;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            Exercise = await GetExercise();
            Exercise = await RepController.PrepareRepSummaryAsync(Exercise);
            Exercise.Key = Key;
            Refresh();
        }

        private void Refresh()
        {
            BindingContext = null;
            BindingContext = this;
        }

        private Task<Exercise> GetExercise()
        {
            return MasterModel.DAL.GetSelectedExerciseAsync(Key);
        }

        private async void Type_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            await Navigation.PushAsync(new EditExerciseTypeV2(e.ItemIndex, Key));
        }

        private async void Save_Clicked(object sender, EventArgs e)
        {
            Exercise.Name = entryRename.Text;
            await MasterModel.DAL.SaveSelectedExerciseAsync(Exercise);
            await Navigation.PopAsync();
        }

        private async void Delete_Clicked(object sender, EventArgs e)
        {
            await MasterModel.DAL.DeleteSelectedExerciseAsync(Exercise);
            await Navigation.PopAsync();
        }

        private async void NewType_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewExerciseType(Key));
        }
    }
}