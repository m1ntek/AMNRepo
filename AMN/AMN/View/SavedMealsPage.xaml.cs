using AMN.Controller;
using AMN.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AMN.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SavedMealsPage : ContentPage
    {
        public List<Meal> savedMeals { get; set; }
        public ICommand DeleteClickedCommand { get; set; }

        public SavedMealsPage()
        {
            InitializeComponent();
            savedMeals = new List<Meal>();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            actInd.IsVisible = true;
            await GetMeals();
            BindingContext = this;
            actInd.IsVisible = false;
        }

        private async Task GetMeals()
        {
            try
            {
                //savedMeals = await MasterModel.DAL.GetSavedMeals();
                MasterModel.currentUser = await MasterModel.DAL.GetUserData();
                savedMeals = MasterModel.currentUser.Meals;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async void AddMeal_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddMealPage());
        }

        private void Delete_Clicked(object sender, EventArgs e)
        {

        }

        private void Edit_Clicked(object sender, EventArgs e)
        {

        }
    }
}