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

        private void entryName_Unfocused(object sender, FocusEventArgs e)
        {
            actName.IsRunning = true;
            //if (entryName.IsFocused == true)
            //{
            //    try
            //    {
            //        MasterController.apiC.Query(entryName.Text);
            //        BindingContext = MasterController.apiC.queryResult.ingredients;
            //    }
            //    catch (Exception ex)
            //    {
            //        DisplayAlert("Error", ex.Message, "OK");
            //    }
            //}

            try
            {
                MasterController.apiC.Query(entryName.Text);
                BindingContext = MasterController.apiC.queryResult.ingredients;
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", ex.Message, "OK");
            }
            actName.IsRunning = false;
        }
    }
}