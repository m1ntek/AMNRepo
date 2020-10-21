﻿using AMN.Controller;
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
    public partial class LoadoutAddSavedMealsPage : ContentPage
    {
        public List<Meal> SavedMeals { get; set; }
        public LoadoutAddSavedMealsPage()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            actInd.IsVisible = true;
            await GetMeals();
            lvSavedMeals.ItemsSource = SavedMeals;
            actInd.IsVisible = false;
        }

        private async Task GetMeals()
        {
            try
            {
                MasterModel.currentUser = await MasterModel.DAL.GetUserDataAsync();
                SavedMeals = MasterModel.currentUser.Meals;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async void lvSavedMeals_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            SavedMeals[e.ItemIndex].index = e.ItemIndex;
            SavedMeals[e.ItemIndex] = await CalculatorV2.MacroTotalsAsync(SavedMeals[e.ItemIndex]);

            bool addMeal = await DisplayAlert(
                SavedMeals[e.ItemIndex].mealName,
                "Add this meal?\n\n" +
                "Contains food items:\n\n" +
                MealItemSummary(e.ItemIndex) +
                string.Format("Total Energy: {0:0.00} kcal", SavedMeals[e.ItemIndex].totalEnergy) + "\n" +
                string.Format("Total Protein: {0:0.00} g", SavedMeals[e.ItemIndex].totalProtein) + "\n" +
                string.Format("Total Carbs: {0:0.00} g", SavedMeals[e.ItemIndex].totalCarbs) + "\n" +
                string.Format("Total Fat: {0:0.00} g", SavedMeals[e.ItemIndex].totalFat) + "\n", "Yes", "No");

            if (addMeal == true)
            {
                MasterModel.currentUser.TempLoadoutMeals.Add(SavedMeals[e.ItemIndex]);
                await MasterModel.DAL.SaveUserDataAsync(MasterModel.currentUser);
                //newLoadoutMeals.Add(MasterModel.tempMeal);
                //lvLoadoutMeals.ItemsSource = newLoadoutMeals;
            }
        }
        //private void SetMealTotals(int index)
        //{
        //    //prevents continuous appending of text
        //    SavedMeals[index].totalEnergy = "Energy: ";
        //    SavedMeals[index].totalProtein = "Protein: ";
        //    SavedMeals[index].totalCarbs = "Carbs: ";
        //    SavedMeals[index].totalFat = "Fat: ";
        //}

        private string MealItemSummary(int index)
        {
            string itemSummary = null;
            foreach (var item in SavedMeals[index].items)
            {
                itemSummary += item.name + "\n";
            }
            itemSummary += "\n";
            return itemSummary;
        }

        private async void AddNewMeal_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LoadoutAddMealPage());
        }
    }
}