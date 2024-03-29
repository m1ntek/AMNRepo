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
    public partial class SelectLoadoutPage : ContentPage
    {
        public List<Loadout> userLoadouts { get; set; }
        private MacroNutrients goalProgress;
        public SelectLoadoutPage()
        {
            InitializeComponent();
            userLoadouts = new List<Loadout>();
            goalProgress = new MacroNutrients();
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
                userLoadouts = await MasterModel.DAL.GetLoadoutsAsync();
                goalProgress = await MasterModel.DAL.GetGoalProgressAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async Task<string> MealSummary(int index)
        {
            string summary = "Select this loadout?\n\n";

            //calculate macro totals for each meal while adding to summary string
            for (int i = 0; i < userLoadouts[index].Meals.Count; i++)
            {
                await CalculatorV2.MacroTotalsAsync(userLoadouts[index].Meals[i]);
                summary += (userLoadouts[index].Meals[i].mealName + "\n");
            }
            summary += "\n";

            //calculate macro totals of whole loadout
            userLoadouts[index] = await CalculatorV2.MacroTotalsLoadoutAsync(userLoadouts[index]);
            summary += "This loadout has:\n";
            summary += string.Format("Energy: {0:0.00} kcal\n", userLoadouts[index].totalEnergy);
            summary += string.Format("Protein: {0:0.00} kcal\n", userLoadouts[index].totalProtein);
            summary += string.Format("Carbs: {0:0.00} kcal\n", userLoadouts[index].totalCarbs);
            summary += string.Format("Fat: {0:0.00} kcal\n", userLoadouts[index].totalFat);

            return summary;
        }

        private async void lvLoadouts_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            bool isConfirmed = await DisplayAlert(
                userLoadouts[e.ItemIndex].LoadoutName,
                await MealSummary(e.ItemIndex),
                "Yes", "No"
                );

            if (isConfirmed == true)
            {
                await MasterModel.DAL.SaveSelectedLoadout(userLoadouts[e.ItemIndex]);
                goalProgress.energyKcal = 0;
                goalProgress.protein = 0;
                goalProgress.carbs = 0;
                goalProgress.fat = 0;
                await MasterModel.DAL.SaveGoalProgressAsync(goalProgress);
                await Navigation.PopAsync();
            }
        }
    }
}