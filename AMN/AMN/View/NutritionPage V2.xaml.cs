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
        public List<Meal> loadoutMeals;
        public MacroNutrients dailyGoal;
        public NutritionPageV2()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            loadoutMeals = new List<Meal>();
            await GetUserData();
            await DisplayLoadoutMeals();
        }

        private async Task GetUserData()
        {
            MasterModel.currentUser = await MasterModel.DAL.GetUserDataAsync();
            loadoutMeals = MasterModel.currentUser.SelectedLoadout.Meals;
            dailyGoal = MasterModel.currentUser.DailyGoal;
        }

        private async void SavedMeals_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SavedMealsPage());
        }

        private async void Loadouts_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LoadoutsPage());
        }

        

        private async Task DisplayLoadoutMeals()
        {
            //stop appendage
            loadoutMealsStack.Children.Clear();

            for (int i = 0; i < loadoutMeals.Count; i++)
            {
                Grid itemGrid = new Grid();
                itemGrid.ColumnDefinitions.Add(new ColumnDefinition());
                itemGrid.ColumnDefinitions.Add(new ColumnDefinition());
                itemGrid.ColumnDefinitions.Add(new ColumnDefinition());
                itemGrid.RowDefinitions.Add(new RowDefinition());
                itemGrid.RowDefinitions.Add(new RowDefinition());
                itemGrid.RowDefinitions.Add(new RowDefinition());
                //itemGrid.RowDefinitions.Add(new RowDefinition());

                itemGrid.RowDefinitions[1].Height = new GridLength(0.5, GridUnitType.Star);
                itemGrid.RowDefinitions[2].Height = new GridLength(0.5, GridUnitType.Star);

                itemGrid.BackgroundColor = Color.FromHex("2196F3");
                itemGrid.Padding = new Thickness(10, 10);

                //save index number per foodItem... To fix a bug
                loadoutMeals[i].index = i;

                Meal meal = loadoutMeals[i];

                Label lblName = new Label();
                lblName = CardCreator.CreateLabelTemplate(lblName, LayoutOptions.Start, 0, 0, 1, meal.mealName);
                lblName.TextColor = Color.White;
                Label lblEnergy = new Label();
                lblEnergy = CardCreator.CreateLabelTemplate(lblEnergy, LayoutOptions.Start, 0, 1, 2, string.Format("Energy: {0:0.00} kcal", meal.totalEnergy));
                lblEnergy.TextColor = Color.White;
                Label lblProtein = new Label();
                lblProtein = CardCreator.CreateLabelTemplate(lblProtein, LayoutOptions.Start, 0, 2, 1, string.Format("P: {0:0.00} g", meal.totalProtein));
                lblProtein.TextColor = Color.White;
                Label lblCarbs = new Label();
                lblCarbs = CardCreator.CreateLabelTemplate(lblCarbs, LayoutOptions.Center, 1, 2, 1, string.Format("C: {0:0.00} g", meal.totalCarbs));
                lblCarbs.TextColor = Color.White;
                Label lblFat = new Label();
                lblFat = CardCreator.CreateLabelTemplate(lblFat, LayoutOptions.End, 2, 2, 1, string.Format("F: {0:0.00} g", meal.totalFat));
                lblFat.TextColor = Color.White;

                itemGrid.Children.Add(lblName);
                itemGrid.Children.Add(lblEnergy);
                itemGrid.Children.Add(lblProtein);
                itemGrid.Children.Add(lblCarbs);
                itemGrid.Children.Add(lblFat);

                StackLayout stack = new StackLayout();
                stack.Orientation = StackOrientation.Horizontal;
                Grid.SetColumnSpan(stack, 3);
                stack.HorizontalOptions = LayoutOptions.End;

                CheckBox chkbxEaten = new CheckBox();

                chkbxEaten.CheckedChanged += (sender, args) =>
                {
                    if(chkbxEaten.IsChecked == true)
                    {

                    }
                    else
                    {

                    }
                };

                stack.Children.Add(chkbxEaten);

                itemGrid.Children.Add(stack);

                loadoutMealsStack.Children.Add(itemGrid);
            }
        }

        private async void SelectLoadout_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SelectLoadoutPage());
        }

        private async void EditGoals_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EditGoalsPage());
        }
    }
}