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
    public partial class SelectExerciseLoadout : ContentPage
    {
        public List<ExerciseLoadout> ELoadouts { get; set; }
        public SelectExerciseLoadout()
        {
            InitializeComponent();
            ELoadouts = new List<ExerciseLoadout>();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            ELoadouts = await MasterModel.DAL.GetExerciseLoadoutsAsync();
            Refresh();
        }

        private void Refresh()
        {
            BindingContext = null;
            BindingContext = this;
        }

        private async void Loadout_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            bool addConfirmed = await DisplayAlert(
                ELoadouts[e.ItemIndex].Name,
                "Select this loadout?",
                "Yes",
                "No");

            if (addConfirmed == true)
            {
                await MasterModel.DAL.SaveSelectedExerciseLoadoutAsync(ELoadouts[e.ItemIndex]);
                await Navigation.PopAsync();
            }
        }

        private async void Done_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}