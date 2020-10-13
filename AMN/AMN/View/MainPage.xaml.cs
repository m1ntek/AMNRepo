using AMN.Controller;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.IO;
using AMN.View;

namespace AMN
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            GetUserData();
        }

        private async Task GetUserData()
        {
            MasterModel.currentUser = await MasterModel.DAL.GetUserData();
            UpdateFooter();
        }

        private async void Nutrition_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NutritionPageV2());
        }

        private void Exercises_Clicked(object sender, EventArgs e)
        {

        }

        private void Progress_Clicked(object sender, EventArgs e)
        {

        }

        private async void Signup_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SignupPage());
        }

        private async void Login_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LoginPage());
        }

        private void UpdateFooter()
        {
            loggedIn.IsVisible = true;
            lblLoggedIn.Text = MasterModel.loginC.UpdateFooter(MasterModel.currentUser.email);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            UpdateFooter();
        }
    }
}
