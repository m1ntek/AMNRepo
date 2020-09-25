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
    public partial class NutritionPage : ContentPage
    {
        public NutritionPage()
        {
            InitializeComponent();
        }

        private void RefreshPage()
        {
            var refreshedPage = new NutritionPage(); Navigation.InsertPageBefore(refreshedPage, this); Navigation.PopAsync();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            //MasterModel.dailyGoal = new MacroNutrients(); //to fix an empty object bug?
            try
            {
                await MasterModel.DAL.GetUserData();
            }
            catch (Exception)
            {
                //do nothing for the moment.
            }

            try
            {
                MasterModel.dailyGoal = await MasterModel.DAL.GetGoalV2();
            }
            catch (Exception)
            {
                //do nothing for the moment.
            }
            UpdateText();
        }

        private async void AddMeal_Clicked(object sender, EventArgs e)
        {
            actInd.IsRunning = true;

            if(MasterModel.tempMeal.items.Count > 0)
            {
                MasterModel.tempMeal = new Meal();
            }

            await Navigation.PushAsync(new AddMealPage());
            actInd.IsRunning = false;
        }

        private async void SetGoal_Clicked(object sender, EventArgs e)
        {
            MasterModel.dailyGoal.energyKcal = Convert.ToDouble(entryEnergyGoal.Text);
            MasterModel.dailyGoal.carbs = Convert.ToDouble(entryCarbGoal.Text);
            MasterModel.dailyGoal.fat = Convert.ToDouble(entryFatGoal.Text);
            MasterModel.dailyGoal.protein = Convert.ToDouble(entryProteinGoal.Text);
            UpdateText();

            MasterModel.DAL.SaveGoal();
            DisplayAlert("Success", "Goal set!", "OK");
        }

        private void UpdateText()
        {
            entryEnergyGoal.Text = MasterModel.dailyGoal.energyKcal.ToString();
            entryCarbGoal.Text = MasterModel.dailyGoal.carbs.ToString();
            entryFatGoal.Text = MasterModel.dailyGoal.fat.ToString();
            entryProteinGoal.Text = MasterModel.dailyGoal.protein.ToString();
        }

        private void entryEnergyGoal_Focused(object sender, FocusEventArgs e)
        {
            entryEnergyGoal.Text = "";
        }

        private void entryCarbGoal_Focused(object sender, FocusEventArgs e)
        {
            entryCarbGoal.Text = "";
        }

        private void entryFatGoal_Focused(object sender, FocusEventArgs e)
        {
            entryFatGoal.Text = "";
        }

        private void entryProteinGoal_Focused(object sender, FocusEventArgs e)
        {
            entryProteinGoal.Text = "";
        }

        //method

        private void entryEnergyGoal_Unfocused(object sender, FocusEventArgs e)
        {
            if (string.IsNullOrEmpty(entryEnergyGoal.Text) == true)
            {
                entryEnergyGoal.Text = MasterModel.dailyGoal.energyKcal.ToString();
            }
            else
            {
                MasterModel.dailyGoal.energyKcal = Convert.ToDouble(entryEnergyGoal.Text);
            }

            entryProteinGoal.Focus();
        }

        private void entryCarbGoal_Unfocused(object sender, FocusEventArgs e)
        {
            if (string.IsNullOrEmpty(entryCarbGoal.Text) == true)
            {
                entryCarbGoal.Text = MasterModel.dailyGoal.carbs.ToString();
            }
            else
            {
                MasterModel.dailyGoal.carbs = Convert.ToDouble(entryCarbGoal.Text);
            }

            entryFatGoal.Focus();
        }

        private void entryFatGoal_Unfocused(object sender, FocusEventArgs e)
        {
            if (string.IsNullOrEmpty(entryFatGoal.Text) == true)
            {
                entryFatGoal.Text = MasterModel.dailyGoal.fat.ToString();
            }
            else
            {
                MasterModel.dailyGoal.fat = Convert.ToDouble(entryFatGoal.Text);
            }
        }

        private void entryProteinGoal_Unfocused(object sender, FocusEventArgs e)
        {
            if (string.IsNullOrEmpty(entryProteinGoal.Text) == true)
            {
                entryProteinGoal.Text = MasterModel.dailyGoal.protein.ToString();
            }
            else
            {
                MasterModel.dailyGoal.protein = Convert.ToDouble(entryProteinGoal.Text);
            }

            entryCarbGoal.Focus();
        }
    }
}