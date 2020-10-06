﻿using AMN.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AMN
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignupPage : ContentPage
    {
        public SignupPage()
        {
            InitializeComponent();
        }

        private async void Signup_Clicked(object sender, EventArgs e)
        {
            bool pwValid = MasterModel.vd.Signup(entryEmail.Text, entryPassword1.Text, entryPassword2.Text);

            if(pwValid == true)
            {
                try
                {
                    await MasterModel.DAL.SignUpUser(entryEmail.Text, entryPassword1.Text);
                    await DisplayAlert("Successful", "New account created successfully.", "OK");
                    ClearForm();
                    await MasterModel.DAL.UpdateUser();
                    await Navigation.PopToRootAsync();
                }
                catch (Exception exception)
                {
                    string reason = MasterModel.DAL.SimplifyException(exception.Message);
                    await DisplayAlert("Error", reason, "OK");
                }
            }
            else
            {
                await DisplayAlert("Invalid", MasterModel.vd.error, "OK");
                ClearPasswords();
            }
        }

        private void ClearForm()
        {
            entryEmail.Text = "";
            ClearPasswords();
        }

        private void ClearPasswords()
        {
            entryPassword1.Text = "";
            entryPassword2.Text = "";
        }
    }
}