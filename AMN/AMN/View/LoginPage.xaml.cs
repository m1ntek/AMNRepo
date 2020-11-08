using AMN.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AMN
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            //Set top bar colour
            ((NavigationPage)Application.Current.MainPage).BarBackgroundColor = Color.DarkRed;
            ((NavigationPage)Application.Current.MainPage).BarTextColor = Color.White;
        }

        private async void Login_Clicked(object sender, EventArgs e)
        {
            try
            {
                actInd.IsRunning = true;
                await MasterModel.DAL.SignInUserAsync(entryEmail.Text, entryPassword.Text);
                ClearForm();
                Navigation.InsertPageBefore(new MainPage(), this);
                await Navigation.PopAsync();
                actInd.IsRunning = false;
            }
            catch (Exception exception)
            {
                actInd.IsRunning = false;
                await DisplayAlert("Error", MasterModel.DAL.SimplifyException(exception.Message), "OK");
            }
        }

        private void ClearForm()
        {
            entryEmail.Text = "";
            entryPassword.Text = "";
        }

        private async void Signup_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SignupPage());
        }

        private void entryEmail_Unfocused(object sender, FocusEventArgs e)
        {
            entryPassword.Focus();
        }

        private void entryPassword_Unfocused(object sender, FocusEventArgs e)
        {
            if(string.IsNullOrEmpty(entryEmail.Text) == false && string.IsNullOrEmpty(entryPassword.Text) == false)
            {
                Login_Clicked(sender, e);
            }
        }
    }
}