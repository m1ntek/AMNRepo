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
        Person currentUser;
        List<Meal> meals;
        public NutritionPage()
        {
            InitializeComponent();
        }

        private void RefreshPage()
        {
            var refreshedPage = new NutritionPage();
            Navigation.InsertPageBefore(refreshedPage, this);
            Navigation.PopAsync();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            //MasterModel.dailyGoal = new MacroNutrients(); //to fix an empty object bug?
            currentUser = await MasterModel.DAL.GetUserDataAsync();
            meals = currentUser.Meals;
            try
            {
                if(MasterModel.currentUser.Meals.Count > 0)
                {
                    //switch status views
                    noMeals.IsVisible = false;
                    mealsFound.IsVisible = true;
                    btnMealsFound.IsVisible = true;
                    LoadMeals();
                }              
            }
            catch (Exception ex)
            {
                //do nothing for the moment.
                await DisplayAlert("Error", ex.Message, "OK");
            }

            try
            {
                //MasterModel.dailyGoal = await MasterModel.DAL.GetGoalV2Async();
            }
            catch (Exception)
            {
                //do nothing for the moment.
            }
            UpdateGoalText();
        }

        private void LoadMeals()
        {
            //prevent appendage
            mealsFound.Children.Clear();

            for (int i = 0; i < meals.Count; i++)
            {
                Grid mealGrid = new Grid();
                mealGrid.ColumnDefinitions.Add(new ColumnDefinition());
                mealGrid.ColumnDefinitions.Add(new ColumnDefinition());
                mealGrid.ColumnDefinitions.Add(new ColumnDefinition());
                mealGrid.RowDefinitions.Add(new RowDefinition());
                mealGrid.RowDefinitions.Add(new RowDefinition());

                mealGrid.RowDefinitions[1].Height = new GridLength(0.5, GridUnitType.Star);

                mealGrid.BackgroundColor = Color.FromHex("2196F3");
                mealGrid.Padding = new Thickness(10, 10);

                //save index number per foodItem... To fix a bug
                meals[i].index = i;

                Meal meal = meals[i];

                Label lblName = new Label();
                lblName = CardCreator.CreateLabelTemplate(lblName, LayoutOptions.Start, 0, 1, 3, meal.mealName);
                lblName.TextColor = Color.White;

                mealGrid.Children.Add(lblName);

                StackLayout stack = new StackLayout();
                stack.Orientation = StackOrientation.Horizontal;
                Grid.SetColumnSpan(stack, 3);
                stack.HorizontalOptions = LayoutOptions.End;

                Button btnEdit = new Button()
                {
                    Text = "Edit",
                    FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Button)),
                    BackgroundColor = Color.DarkGoldenrod,
                    TextColor = Color.White,
                    HeightRequest = 35,
                    WidthRequest = 50,
                    CornerRadius = 5
                };

                Button btnDelete = new Button()
                {
                    Text = "Del",
                    FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Button)),
                    BackgroundColor = Color.DarkRed,
                    TextColor = Color.White,
                    HeightRequest = 35,
                    WidthRequest = 50,
                    CornerRadius = 5
                };

                btnEdit.Clicked += async (sender, args) =>
                {
                    MasterModel.tempMeal = meals[meal.index];
                    await Navigation.PushAsync(new AddMealPage());
                };

                btnDelete.Clicked += async (sender, args) =>
                {
                    //meals.RemoveAt(meal.index);
                    MasterModel.currentUser.Meals.RemoveAt(meal.index);
                    await MasterModel.DAL.SaveMealV2();
                    RefreshPage();
                };

                stack.Children.Add(btnEdit);
                stack.Children.Add(btnDelete);

                mealGrid.Children.Add(stack);

                //mealsFound.Children.Add(mealGrid);
                mealsFound.Children.Insert(0, mealGrid);
            }
        }

        private async void AddMeal_Clicked(object sender, EventArgs e)
        {
            actInd.IsRunning = true;

            if(MasterModel.tempMeal.items.Count > 0)
            {
                MasterModel.tempMeal = new Meal();
            }

            await Navigation.PushAsync(new AddMealPage());
            actInd.IsRunning = false;
        }

        private async void SetGoal_Clicked(object sender, EventArgs e)
        {
            MasterModel.dailyGoal.energyKcal = Convert.ToDouble(entryEnergyGoal.Text);
            MasterModel.dailyGoal.carbs = Convert.ToDouble(entryCarbGoal.Text);
            MasterModel.dailyGoal.fat = Convert.ToDouble(entryFatGoal.Text);
            MasterModel.dailyGoal.protein = Convert.ToDouble(entryProteinGoal.Text);
            UpdateGoalText();

            //MasterModel.DAL.SaveGoalAsync();
            DisplayAlert("Success", "Goal set!", "OK");
        }

        private void UpdateGoalText()
        {
            entryEnergyGoal.Text = MasterModel.dailyGoal.energyKcal.ToString();
            entryCarbGoal.Text = MasterModel.dailyGoal.carbs.ToString();
            entryFatGoal.Text = MasterModel.dailyGoal.fat.ToString();
            entryProteinGoal.Text = MasterModel.dailyGoal.protein.ToString();
        }

        private void entryEnergyGoal_Focused(object sender, FocusEventArgs e)
        {
            entryEnergyGoal.Text = "";
        }

        private void entryCarbGoal_Focused(object sender, FocusEventArgs e)
        {
            entryCarbGoal.Text = "";
        }

        private void entryFatGoal_Focused(object sender, FocusEventArgs e)
        {
            entryFatGoal.Text = "";
        }

        private void entryProteinGoal_Focused(object sender, FocusEventArgs e)
        {
            entryProteinGoal.Text = "";
        }

        //method

        private void entryEnergyGoal_Unfocused(object sender, FocusEventArgs e)
        {
            if (string.IsNullOrEmpty(entryEnergyGoal.Text) == true)
            {
                entryEnergyGoal.Text = MasterModel.dailyGoal.energyKcal.ToString();
            }
            else
            {
                MasterModel.dailyGoal.energyKcal = Convert.ToDouble(entryEnergyGoal.Text);
            }

            entryProteinGoal.Focus();
        }

        private void entryCarbGoal_Unfocused(object sender, FocusEventArgs e)
        {
            if (string.IsNullOrEmpty(entryCarbGoal.Text) == true)
            {
                entryCarbGoal.Text = MasterModel.dailyGoal.carbs.ToString();
            }
            else
            {
                MasterModel.dailyGoal.carbs = Convert.ToDouble(entryCarbGoal.Text);
            }

            entryFatGoal.Focus();
        }

        private void entryFatGoal_Unfocused(object sender, FocusEventArgs e)
        {
            if (string.IsNullOrEmpty(entryFatGoal.Text) == true)
            {
                entryFatGoal.Text = MasterModel.dailyGoal.fat.ToString();
            }
            else
            {
                MasterModel.dailyGoal.fat = Convert.ToDouble(entryFatGoal.Text);
            }
        }

        private void entryProteinGoal_Unfocused(object sender, FocusEventArgs e)
        {
            if (string.IsNullOrEmpty(entryProteinGoal.Text) == true)
            {
                entryProteinGoal.Text = MasterModel.dailyGoal.protein.ToString();
            }
            else
            {
                MasterModel.dailyGoal.protein = Convert.ToDouble(entryProteinGoal.Text);
            }

            entryCarbGoal.Focus();
        }

        private void mealsFound_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }
    }
}