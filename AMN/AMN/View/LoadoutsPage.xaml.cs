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
    public partial class LoadoutsPage : ContentPage
    {
        public List<Loadout> userLoadouts { get; set; }
        public LoadoutsPage()
        {
            InitializeComponent();
            userLoadouts = new List<Loadout>();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            //actInd.IsVisible = true;
            await GetLoadouts();
            lvLoadouts.ItemsSource = userLoadouts;
            //actInd.IsVisible = false;
        }

        private async Task GetLoadouts()
        {
            try
            {
                //MasterModel.currentUser = await MasterModel.DAL.GetUserDataAsync();
                //userLoadouts = MasterModel.currentUser.Loadouts;
                userLoadouts = await MasterModel.DAL.GetLoadoutsAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async void lvLoadouts_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            List<Meal> tempLoadoutMeals = userLoadouts[e.ItemIndex].Meals;
            //string userLoadoutKey = userLoadouts[e.ItemIndex].Key;
            await MasterModel.DAL.ResetTempLoadoutMealsAsync();
            foreach (var meal in tempLoadoutMeals)
            {
                await MasterModel.DAL.SaveNewTempLoadoutMealAsync(meal);
            }
            await MasterModel.DAL.SaveUserDataAsync(MasterModel.currentUser);
            await Navigation.PushAsync(new LoadoutMeals(userLoadouts[e.ItemIndex].LoadoutName, e.ItemIndex));
        }

        private async void NewLoadout_Clicked(object sender, EventArgs e)
        {
            //MasterModel.currentUser.TempLoadoutMeals = new List<Meal>();
            await MasterModel.DAL.ResetTempLoadoutMealsAsync();

            //await MasterModel.DAL.SaveUserDataAsync(MasterModel.currentUser);
            await Navigation.PushAsync(new LoadoutMeals("New Loadout"));
        }
    }
}