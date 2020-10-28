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
    public partial class EditExercisePage : ContentPage
    {
        public List<Exercise> SavedExercises { get; set; }
        public EditExercisePage()
        {
            InitializeComponent();
            SavedExercises = new List<Exercise>();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            SavedExercises = await GetExercisesAsync();
            RefreshList();
        }

        private void RefreshList()
        {
            BindingContext = null;
            BindingContext = this;
        }

        private async void NewExercise_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewExercise());
        }

        private Task<List<Exercise>> GetExercisesAsync()
        {
            return MasterModel.DAL.GetSavedExercisesAsync();
        }

        private async void Exercise_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            await Navigation.PushAsync(new SelectedExercise(SavedExercises[e.ItemIndex].Key));
        }
    }
}