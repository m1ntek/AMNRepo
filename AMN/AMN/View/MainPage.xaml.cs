using AMN.Controller;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.IO;

namespace AMN
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void Nutrition_Clicked(object sender, EventArgs e)
        {

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
            if(string.IsNullOrEmpty(MasterController.currentUser.email) != true)
            {
                notLoggedIn.IsVisible = false;
                loggedIn.IsVisible = true;
                lblLoggedIn.Text = MasterController.loginC.UpdateFooter(MasterController.currentUser.email);
            }
            else
            {
                notLoggedIn.IsVisible = true;
                loggedIn.IsVisible = false;
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            UpdateFooter();
        }
    }
}
