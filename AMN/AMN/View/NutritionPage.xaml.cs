using AMN.Controller;
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
    public partial class NutritionPage : ContentPage
    {
        public NutritionPage()
        {
            InitializeComponent();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            MasterController.apiC.Query(entryQuery.Text);
            var resultInfo = MasterController.apiC.queryResult;
            lblTest.Text = "Top Result:\n" +
                "\n" +
                $"{resultInfo.ingredients[0].parsed[0].food}\n";
            foreach (var nutrient in resultInfo.ingredients[0].parsed[0].nutrients.)
            {
                lblTest.Text += $"{nutrient.nutrientName}:\t\t{nutrient.nutrientNumber}{nutrient.unitName}\n";
            }
        }
    }
}