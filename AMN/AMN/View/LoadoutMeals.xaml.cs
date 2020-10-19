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
        //public List<Meal> savedMeals { get; set; }
        public List<Meal> newLoadoutMeals { get; set; }
        //public string headerTitle { get; set; }
        public int currentLoadoutIndex { get; set; } = -1;

        public LoadoutMeals(string headerTitle)
        {
            InitializeComponent();
            //savedMeals = new List<Meal>();
            newLoadoutMeals = new List<Meal>();
            lblHeader.Text = headerTitle;
        }
        public LoadoutMeals(string headerTitle, int index)
        {
            InitializeComponent();
            //savedMeals = new List<Meal>();
            newLoadoutMeals = new List<Meal>();
            lblHeader.Text = headerTitle;
            entryLoadoutName.Text = headerTitle;
            currentLoadoutIndex = index;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            actInd.IsVisible = true;
            await GetMeals();
            //lvSavedMeals.ItemsSource = savedMeals;
            lvLoadoutMeals.ItemsSource = newLoadoutMeals;
            actInd.IsVisible = false;
        }

        private async Task GetMeals()
        {
            try
            {
                MasterModel.currentUser = await MasterModel.DAL.GetUserData();
                //savedMeals = MasterModel.currentUser.Meals;
                newLoadoutMeals = MasterModel.currentUser.TempLoadoutMeals;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        //private void SetMealTotals()
        //{
        //    //prevents continuous appending of text
        //    MasterModel.tempMeal.totalEnergy = "Energy: ";
        //    MasterModel.tempMeal.totalProtein = "Protein: ";
        //    MasterModel.tempMeal.totalCarbs = "Carbs: ";
        //    MasterModel.tempMeal.totalFat = "Fat: ";
        //}


        private async void Meal_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            //MasterModel.tempMeal = savedMeals[e.ItemIndex];
            MasterModel.tempMeal.index = e.ItemIndex;
            
            //var macroTotalsTask = CalculatorV2.MacroTotals(MasterModel.tempMeal);

            //SetMealTotals();
            //MasterModel.tempMeal = CalculatorV2.MacroTotals(MasterModel.tempMeal);
            //MasterModel.tempMeal = await macroTotalsTask;
            MasterModel.tempMeal = await CalculatorV2.MacroTotals(MasterModel.tempMeal);

            bool addMeal = await DisplayAlert(
                MasterModel.tempMeal.mealName,
                "Add this meal?\n\n" +
                MealItemSummary() +
                string.Format("{0:0.00} kcal", MasterModel.tempMeal.totalEnergy) + "\n" +
                string.Format("{0:0.00} g", MasterModel.tempMeal.totalProtein) + "\n" +
                string.Format("{0:0.00} g", MasterModel.tempMeal.totalCarbs) + "\n" +
                string.Format("{0:0.00} g", MasterModel.tempMeal.totalFat) + "\n", "Yes", "No");
            //number format not working for some reason

            if(addMeal == true)
            {
                newLoadoutMeals.Add(MasterModel.tempMeal);
                //lvLoadoutMeals.ItemsSource = newLoadoutMeals;
            }
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
            //savedMeals = MasterModel.currentUser.Meals;

            //lvSavedMeals.ItemsSource = savedMeals;
        }

        private async void AddMeal_Clicked(object sender, EventArgs e)
        {
            MasterModel.currentUser.TempLoadoutMeals = newLoadoutMeals;
            MasterModel.tempMeal = new Meal();
            await MasterModel.DAL.SaveUserData(MasterModel.currentUser);
            await Navigation.PushAsync(new LoadoutAddSavedMealsPage());
        }

        private async void SaveLoadout_Clicked(object sender, EventArgs e)
        {
            bool isValid = MasterModel.vd.FormEntries(new string[] { entryLoadoutName.Text });
            await MasterModel.DAL.GetIds();
            newLoadoutMeals = MasterModel.currentUser.TempLoadoutMeals;

            if (isValid == false)
            {
                await DisplayAlert("Error", MasterModel.vd.error, "OK");
                return;
            }
            else if (newLoadoutMeals.Count == 0)
            {
                await DisplayAlert("Error", "The loadout cannot be empty", "OK");
                return;
            }
            else
            {
                //new loadout or replace existing logic
                if(currentLoadoutIndex == -1)
                {
                    MasterModel.currentUser.Loadouts.Add(new Loadout()
                    {
                        LoadoutName = entryLoadoutName.Text,
                        Meals = newLoadoutMeals,
                        LoadoutId = MasterModel.DAL.idGen.totalLoadoutIds
                    });
                    MasterModel.DAL.idGen.totalLoadoutIds++;
                }
                else
                {
                    MasterModel.currentUser.Loadouts[currentLoadoutIndex].LoadoutName = entryLoadoutName.Text;
                    MasterModel.currentUser.Loadouts[currentLoadoutIndex].Meals = newLoadoutMeals;
                }
                
            }

            
            await MasterModel.DAL.SaveIds();

            await MasterModel.DAL.SaveUserData(MasterModel.currentUser);
            await Navigation.PopAsync();
        }

        private async void lvLoadoutMeals_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            MasterModel.tempMeal = MasterModel.currentUser.TempLoadoutMeals[e.ItemIndex];
            MasterModel.tempMeal.index = e.ItemIndex;
            await Navigation.PushAsync(new AddMealPageV2());
        }

        private async void DeleteLoadout_Clicked(object sender, EventArgs e)
        {
            MasterModel.currentUser.Loadouts.RemoveAt(currentLoadoutIndex);
            await MasterModel.DAL.SaveUserData(MasterModel.currentUser);
            await Navigation.PopAsync();
        }
    }
}