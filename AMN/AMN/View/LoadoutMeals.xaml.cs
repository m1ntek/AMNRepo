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
    public partial class LoadoutMeals : ContentPage
    {
        public List<Meal> savedMeals { get; set; }

        public LoadoutMeals()
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

        private async void Meal_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            MasterModel.tempMeal = savedMeals[e.ItemIndex];
            MasterModel.tempMeal.index = e.ItemIndex;

            bool addMeal = await DisplayAlert(
                MasterModel.tempMeal.mealName,
                "Add this meal?\n\n" +
                MealItemSummary() +
                MasterModel.tempMeal.totalEnergy + "\n" +
                MasterModel.tempMeal.totalProtein + "\n" +
                MasterModel.tempMeal.totalCarbs + "\n" +
                MasterModel.tempMeal.totalFat + "\n", "Yes", "No");


        }

        private string MealItemSummary()
        {
            string itemSummary = null;
            foreach (var item in MasterModel.tempMeal.items)
            {
                itemSummary += item.name + "\n";
            }
            itemSummary += "\n";
            return itemSummary;
        }

        private async void MealDelete_Clicked(object sender, EventArgs e)
        {
            var mealItem = (Meal)sender;

            MasterModel.currentUser.Meals.RemoveAt(mealItem.index);
            await MasterModel.DAL.SaveUserData(MasterModel.currentUser);
            savedMeals = MasterModel.currentUser.Meals;

            lvSavedMeals.ItemsSource = savedMeals;
        }

        private async void AddMeal_Clicked(object sender, EventArgs e)
        {
            MasterModel.tempMeal = new Meal();
            await Navigation.PushAsync(new AddMealPageV2());
        }

        private void SaveLoadout_Clicked(object sender, EventArgs e)
        {

        }
    }
}