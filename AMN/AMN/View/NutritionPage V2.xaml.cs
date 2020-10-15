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
    public partial class NutritionPageV2 : ContentPage
    {
        public List<Meal> daysMeals;
        public NutritionPageV2()
        {
            InitializeComponent();
            daysMeals = new List<Meal>();
            GetDaysMeals();
        }

        private async Task GetDaysMeals()
        {
            try
            {
                daysMeals = await MasterModel.DAL.GetDaysMeals();
                BindingContext = this;
            }
            catch (Exception)
            {
                //do nothing for now
            }
        }

        private async void SavedMeals_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SavedMealsPage());
        }

        private async void AddMeals_Clicked(object sender, EventArgs e)
        {

        }

        private async void Loadouts_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LoadoutsPage());
        }
    }
}