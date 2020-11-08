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
            lvSavedMeals.ItemsSource = savedMeals;
            actInd.IsVisible = false;
        }

        private async Task GetMeals()
        {
            try
            {
                savedMeals = await MasterModel.DAL.GetSavedMealsAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async void Meal_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            MasterModel.tempMeal = savedMeals[e.ItemIndex];
            MasterModel.tempMeal.index = e.ItemIndex;
            await Navigation.PushAsync(new AddMealPageV2());
        }

        private void MealDelete_Clicked(object sender, EventArgs e)
        {
            var mealItem = (Meal)sender;
            savedMeals.RemoveAt(mealItem.index);
            lvSavedMeals.ItemsSource = savedMeals;
        }

        private async void NewMeal_Clicked(object sender, EventArgs e)
        {
            MasterModel.tempMeal = new Meal();
            await Navigation.PushAsync(new AddMealPageV2());
        }
    }
}