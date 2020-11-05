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
            //GetUserData();
            //UpdateFooter();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await UpdateFooter();
        }

        //private async Task GetUserData()
        //{
        //    MasterModel.currentUser = await MasterModel.DAL.GetUserDataAsync();
        //    UpdateFooter();
        //}

        private async void Nutrition_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NutritionPageV2());
        }

        private async void Exercises_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ExercisePage());
        }

        private async void Progress_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewSession());
        }

        private async void Signup_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SignupPage());
        }

        private async void Login_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LoginPage());
        }

        private async Task UpdateFooter()
        {
            loggedIn.IsVisible = true;
            string userEmail = await MasterModel.DAL.GetEmail();
            lblLoggedIn.Text = MasterModel.loginC.UpdateFooter(userEmail);
        }
    }
}
