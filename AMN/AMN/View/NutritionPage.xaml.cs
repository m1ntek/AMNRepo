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
                $"{resultInfo.ingredients[0].parsed[0].food}\n" +
                $"\n" +
                $"{resultInfo.ingredients[0].parsed[0].nutrients.ENERC_KCAL.label}:\t{resultInfo.ingredients[0].parsed[0].nutrients.ENERC_KCAL.quantity}{resultInfo.ingredients[0].parsed[0].nutrients.ENERC_KCAL.unit}\n" +
                $"{resultInfo.ingredients[0].parsed[0].nutrients.PROCNT.label}:\t{resultInfo.ingredients[0].parsed[0].nutrients.PROCNT.quantity}{resultInfo.ingredients[0].parsed[0].nutrients.PROCNT.unit}\n" +
                $"{resultInfo.ingredients[0].parsed[0].nutrients.CA.label}:\t{resultInfo.ingredients[0].parsed[0].nutrients.CA.quantity}{resultInfo.ingredients[0].parsed[0].nutrients.CA.unit}\n" +
                $"{resultInfo.ingredients[0].parsed[0].nutrients.FAT.label}:\t{resultInfo.ingredients[0].parsed[0].nutrients.FAT.quantity}{resultInfo.ingredients[0].parsed[0].nutrients.FAT.unit}";
        }
    }
}