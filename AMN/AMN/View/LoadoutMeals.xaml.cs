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
        public List<Meal> newLoadoutMeals { get; set; }
        List<Loadout> loadouts;
        Loadout selectedLoadout;
        private List<Task> tasks;
        public int currentLoadoutIndex { get; set; } = -1;

        public LoadoutMeals(string headerTitle)
        {
            InitializeComponent();
            newLoadoutMeals = new List<Meal>();
            loadouts = new List<Loadout>();
            selectedLoadout = new Loadout();
            lblHeader.Text = headerTitle;
        }
        public LoadoutMeals(string headerTitle, int index)
        {
            InitializeComponent();
            newLoadoutMeals = new List<Meal>();
            lblHeader.Text = headerTitle;
            entryLoadoutName.Text = headerTitle;
            currentLoadoutIndex = index;
        }

        public LoadoutMeals(string headerTitle, int index, List<Task> _tasks)
        {
            InitializeComponent();
            newLoadoutMeals = new List<Meal>();
            lblHeader.Text = headerTitle;
            entryLoadoutName.Text = headerTitle;
            currentLoadoutIndex = index;
            tasks = _tasks;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            await GetMeals();

            //An attempt to improve loading times.
            //Load all meals asynchronously.
            if(tasks != null)
            {
                while (tasks.Count > 0)
                {
                    Task finished = await Task.WhenAny(tasks);

                    Refresh();

                    tasks.Remove(finished);
                }
            }

            Refresh();
        }

        private void Refresh()
        {
            BindingContext = null;
            BindingContext = this;
        }

        private async Task GetMeals()
        {
            try
            {
                newLoadoutMeals = await MasterModel.DAL.GetTempLoadoutMealsAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async void Meal_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            MasterModel.tempMeal.index = e.ItemIndex;

            MasterModel.tempMeal = await CalculatorV2.MacroTotalsAsync(MasterModel.tempMeal);

            bool addMeal = await DisplayAlert(
                MasterModel.tempMeal.mealName,
                "Add this meal?\n\n" +
                MealItemSummary() +
                string.Format("{0:0.00} kcal", MasterModel.tempMeal.totalEnergy) + "\n" +
                string.Format("{0:0.00} g", MasterModel.tempMeal.totalProtein) + "\n" +
                string.Format("{0:0.00} g", MasterModel.tempMeal.totalCarbs) + "\n" +
                string.Format("{0:0.00} g", MasterModel.tempMeal.totalFat) + "\n", "Yes", "No");

            if(addMeal == true)
            {
                await MasterModel.DAL.SaveNewTempLoadoutMealAsync(MasterModel.tempMeal);
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

            await MasterModel.DAL.DeleteMealAsync(mealItem.key);
        }

        private async void AddMeal_Clicked(object sender, EventArgs e)
        {
            MasterModel.tempMeal = new Meal();

            await Navigation.PushAsync(new LoadoutAddSavedMealsPage());
        }

        private async void SaveLoadout_Clicked(object sender, EventArgs e)
        {
            bool isValid = MasterModel.vd.FormEntriesValid(new string[] { entryLoadoutName.Text });
            await MasterModel.DAL.GetIdsAsync();

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
                //new loadout or replace existing loadout logic
                if(currentLoadoutIndex == -1)
                {
                    await MasterModel.DAL.SaveNewLoadout(new Loadout
                    {
                        LoadoutName = entryLoadoutName.Text,
                        Meals = newLoadoutMeals,
                        LoadoutId = MasterModel.DAL.idGen.totalLoadoutIds
                    });
                    MasterModel.DAL.idGen.totalLoadoutIds++;
                }
                else
                {
                    loadouts = await MasterModel.DAL.GetLoadoutsAsync();
                    loadouts[currentLoadoutIndex].LoadoutName = entryLoadoutName.Text;
                    loadouts[currentLoadoutIndex].Meals = newLoadoutMeals;
                    await MasterModel.DAL.SaveLoadoutAsync(loadouts[currentLoadoutIndex], loadouts[currentLoadoutIndex].Key);
                }
                
            }

            //if this loadout is the current selected loadout, update that too.
            try
            {
                selectedLoadout = await MasterModel.DAL.GetSelectedLoadoutAsync();
                if (loadouts[currentLoadoutIndex].LoadoutId == selectedLoadout.LoadoutId)
                {
                    selectedLoadout = loadouts[currentLoadoutIndex];
                    await MasterModel.DAL.SaveSelectedLoadout(selectedLoadout);
                }
            }
            catch (Exception)
            {
                //do nothing
            }
            
            await MasterModel.DAL.SaveIdsAsync();
            
            await Navigation.PopAsync();
        }

        private async void lvLoadoutMeals_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            List<Meal> tempLoadoutMeals = await MasterModel.DAL.GetTempLoadoutMealsAsync();
            MasterModel.tempMeal = tempLoadoutMeals[e.ItemIndex];
            MasterModel.tempMeal.index = e.ItemIndex;
            await Navigation.PushAsync(new LoadoutAddMealPage(currentLoadoutIndex));
        }

        private async void DeleteLoadout_Clicked(object sender, EventArgs e)
        {
            var deleteConfirmed = await DisplayAlert(
                MasterModel.currentUser.Loadouts[currentLoadoutIndex].LoadoutName,
                "Delete this loadout?", "Yes", "No");

            if(deleteConfirmed == true)
            {
                loadouts = await MasterModel.DAL.GetLoadoutsAsync();
                await MasterModel.DAL.DeleteLoadoutAsync(loadouts[currentLoadoutIndex].Key);
                await Navigation.PopAsync();
            }
        }
    }
}