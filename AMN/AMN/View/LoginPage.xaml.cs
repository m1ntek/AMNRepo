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

        private void RememberMeRow_Tapped(object sender, EventArgs e)
        {
            //toggle remember me
            if (chkboxRememberMe.IsChecked == false)
            {
                chkboxRememberMe.IsChecked = true;
            }
            else
            {
                chkboxRememberMe.IsChecked = false;
            }
        }

        private async void Login_Clicked(object sender, EventArgs e)
        {
            if (chkboxRememberMe.IsChecked == true)
            {
                
            }

            try
            {
                await MasterController.DAL.SignInUser(entryEmail.Text, entryPassword.Text);
                MasterController.currentUser.email = entryEmail.Text;
                ClearForm();
                await Navigation.PopToRootAsync();
                MasterController.currentUser.rememberMe = chkboxRememberMe.IsChecked;
                await MasterController.DAL.UpdateUser();
            }
            catch (Exception exception)
            {
                string reason = MasterController.DAL.SimplifyException(exception.Message);
                await DisplayAlert("Error", reason, "OK");
            }
        }

        private void ClearForm()
        {
            entryEmail.Text = "";
            entryPassword.Text = "";
        }

        private void chkboxRememberMe_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            RememberMeRow_Tapped(sender, e);
        }
    }
}