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

        //private void RememberMeRow_Tapped(object sender, EventArgs e)
        //{
        //    //toggle remember me
        //    if (chkboxRememberMe.IsChecked == false)
        //    {
        //        chkboxRememberMe.IsChecked = true;
        //    }
        //    else
        //    {
        //        chkboxRememberMe.IsChecked = false;
        //    }
        //}

        private async void Login_Clicked(object sender, EventArgs e)
        {
            //if (chkboxRememberMe.IsChecked == true)
            //{
                
            //}

            try
            {
                actInd.IsRunning = true;
                await MasterModel.DAL.SignInUser(entryEmail.Text, entryPassword.Text);
                //MasterModel.currentUser.email = entryEmail.Text;
                MasterModel.currentUser = await MasterModel.DAL.GetUserData();
                ClearForm();
                Navigation.InsertPageBefore(new MainPage(), this);
                await Navigation.PopAsync();
                actInd.IsRunning = false;
                //MasterController.currentUser.rememberMe = chkboxRememberMe.IsChecked;
                //await MasterModel.DAL.UpdateUser();
            }
            catch (Exception exception)
            {
                actInd.IsRunning = false;
                //string reason = MasterModel.DAL.SimplifyException(exception.Message);
                await DisplayAlert("Error", exception.Message, "OK");
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

        //private void chkboxRememberMe_CheckedChanged(object sender, CheckedChangedEventArgs e)
        //{
        //    RememberMeRow_Tapped(sender, e);
        //}
    }
}