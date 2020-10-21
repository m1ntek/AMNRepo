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
    public partial class EditGoalsPage : ContentPage
    {
        MacroNutrients dailyGoal;
        public EditGoalsPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await GetUserDataAsync();
            await SetEntriesAsync();
        }

        private async Task GetUserDataAsync()
        {
            MasterModel.currentUser = await MasterModel.DAL.GetUserDataAsync();
            dailyGoal = MasterModel.currentUser.DailyGoal;
        }

        private async Task SetEntriesAsync()
        {
            entryEnergy.Text = dailyGoal.energyKcal.ToString();
            entryProtein.Text = dailyGoal.protein.ToString();
            entryCarbs.Text = dailyGoal.carbs.ToString();
            entryFat.Text = dailyGoal.fat.ToString();
        }

        private async Task ConvertAndSaveMultipleToDoubleAsync()
        {
            dailyGoal.energyKcal = Convert.ToDouble(entryEnergy.Text);
            dailyGoal.protein = Convert.ToDouble(entryProtein.Text);
            dailyGoal.carbs = Convert.ToDouble(entryCarbs.Text);
            dailyGoal.fat = Convert.ToDouble(entryFat.Text);
        }

        private async void Save_Clicked(object sender, EventArgs e)
        {
            await ConvertAndSaveMultipleToDoubleAsync();
            MasterModel.currentUser.DailyGoal = dailyGoal;
            await MasterModel.DAL.SaveUserDataAsync(MasterModel.currentUser);
            
            await Navigation.PopAsync();
        }

        //convenience logic, move to next entry after filling previous entry.
        private void entryEnergy_Unfocused(object sender, FocusEventArgs e)
        {
            entryProtein.Focus();
        }

        private void entryProtein_Unfocused(object sender, FocusEventArgs e)
        {
            entryCarbs.Focus();
        }

        private void entryCarbs_Unfocused(object sender, FocusEventArgs e)
        {
            entryFat.Focus();
        }
    }
}