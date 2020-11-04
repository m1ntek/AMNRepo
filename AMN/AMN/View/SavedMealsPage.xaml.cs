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
            //savedMeals.Clear();
            await GetMeals();
            //BindingContext = this;
            lvSavedMeals.ItemsSource = savedMeals;
            actInd.IsVisible = false;
        }

        private async Task GetMeals()
        {
            try
            {
                //MasterModel.currentUser = await MasterModel.DAL.GetUserDataAsync();
                //savedMeals = MasterModel.currentUser.Meals;
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

        private async void MealDelete_Clicked(object sender, EventArgs e)
        {
            var mealItem = (Meal)sender;

            //MasterModel.currentUser.Meals.RemoveAt(mealItem.index);
            //await MasterModel.DAL.SaveUserDataAsync(MasterModel.currentUser);
            //savedMeals = MasterModel.currentUser.Meals;

            savedMeals.RemoveAt(mealItem.index);
            //await MasterModel.DAL.DeleteSelectedMealAsync(savedMeals[mealItem.index].Key);

            lvSavedMeals.ItemsSource = savedMeals;
        }

        private async void NewMeal_Clicked(object sender, EventArgs e)
        {
            MasterModel.tempMeal = new Meal();
            await Navigation.PushAsync(new AddMealPageV2());
        }
    }
}