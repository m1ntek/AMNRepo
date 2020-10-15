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
    public partial class LoadoutsPage : ContentPage
    {
        public List<Loadout> userLoadouts { get; set; }
        public LoadoutsPage()
        {
            InitializeComponent();
            userLoadouts = new List<Loadout>();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            actInd.IsVisible = true;
            await GetLoadouts();
            lvLoadouts.ItemsSource = userLoadouts;
            actInd.IsVisible = false;
        }

        private async Task GetLoadouts()
        {
            try
            {
                MasterModel.currentUser = await MasterModel.DAL.GetUserData();
                userLoadouts = MasterModel.currentUser.Loadouts;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private void lvLoadouts_ItemTapped(object sender, ItemTappedEventArgs e)
        {

        }

        private async void NewLoadout_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LoadoutMeals());
        }
    }
}