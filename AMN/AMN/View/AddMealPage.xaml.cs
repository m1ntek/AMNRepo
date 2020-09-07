using AMN.Controller;
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
    public partial class AddMealPage : ContentPage
    {
        public AddMealPage()
        {
            InitializeComponent();
        }

        private void entryName_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(entryName.IsFocused == true)
            {
                try
                {
                    MasterController.apiC.Query(entryName.Text);
                    BindingContext = MasterController.apiC.queryResult.ingredients;
                }
                catch (Exception)
                {

                }
            }   
        }
    }
}