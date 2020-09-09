using AMN.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AMN.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddMealPage : ContentPage
    {
        public AddMealPage()
        {
            InitializeComponent();
        }

        private async void entryName_Unfocused(object sender, FocusEventArgs e)
        {
            actName.IsRunning = true;

            if (string.IsNullOrEmpty(entryName.Text) == false)
            {
                try
                {
                    MasterController.apiC.Query(entryName.Text);
                    MasterController.currentFoodResult = new Model.FoodResult();
                    entryEnergy.Text = MasterController.currentFoodResult.resultKcal.ToString();
                    entryCarbs.Text = MasterController.currentFoodResult.resultCarb.ToString();
                    entryFat.Text = MasterController.currentFoodResult.resultFat.ToString();
                    entryServing.Text = MasterController.currentFoodResult.resultServing.ToString();
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", ex.Message, "OK");
                }
            }

            actName.IsRunning = false;
        }

        private void entryEnergy_Focused(object sender, FocusEventArgs e)
        {
            entryEnergy.Text = "";
        }

        private void entryEnergy_Unfocused(object sender, FocusEventArgs e)
        {
            if(string.IsNullOrEmpty(entryEnergy.Text) == true && MasterController.currentFoodResult != null)
            {
                entryEnergy.Text = MasterController.currentFoodResult.resultKcal.ToString();
            }
        }

        private void entryCarbs_Focused(object sender, FocusEventArgs e)
        {
            entryCarbs.Text = "";
        }

        private void entryCarbs_Unfocused(object sender, FocusEventArgs e)
        {
            if (string.IsNullOrEmpty(entryCarbs.Text) == true && MasterController.currentFoodResult != null)
            {
                entryCarbs.Text = MasterController.currentFoodResult.resultCarb.ToString();
            }
        }

        private void entryFat_Focused(object sender, FocusEventArgs e)
        {
            entryFat.Text = "";
        }

        private void entryFat_Unfocused(object sender, FocusEventArgs e)
        {
            if (string.IsNullOrEmpty(entryFat.Text) == true && MasterController.currentFoodResult != null)
            {
                entryFat.Text = MasterController.currentFoodResult.resultFat.ToString();
            }
        }

        private void entryServing_Focused(object sender, FocusEventArgs e)
        {
            entryServing.Text = "";
        }

        private void entryServing_Unfocused(object sender, FocusEventArgs e)
        {
            if (string.IsNullOrEmpty(entryServing.Text) == true && MasterController.currentFoodResult != null)
            {
                entryServing.Text = MasterController.currentFoodResult.resultServing.ToString();
            }
            entryEnergy.Text = (MasterController.currentFoodResult.resultKcal / MasterController.currentFoodResult.resultServing * Convert.ToDouble(entryServing.Text)).ToString();
            entryCarbs.Text = (MasterController.currentFoodResult.resultCarb / MasterController.currentFoodResult.resultServing * Convert.ToDouble(entryServing.Text)).ToString();
            entryFat.Text = (MasterController.currentFoodResult.resultFat / MasterController.currentFoodResult.resultServing * Convert.ToDouble(entryServing.Text)).ToString();
        }
    }
}