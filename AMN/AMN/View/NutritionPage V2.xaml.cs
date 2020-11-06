using AMN.Controller;
using AMN.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AMN.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NutritionPageV2 : ContentPage
    {
        public List<Meal> loadoutMeals { get; set; }
        public MacroNutrients dailyGoal { get; set; }
        public MacroNutrients goalProgress { get; set; }
        public Loadout loadout { get; set; }
        public NutritionPageV2()
        {
            InitializeComponent();
            //loadoutMeals = new List<Meal>();
            dailyGoal = new MacroNutrients();
            goalProgress = new MacroNutrients();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            //loadoutMeals = new List<Meal>();
            await GetUserData();

            if(loadoutMeals != null &&
                dailyGoal != null &&
                goalProgress != null)
            {
                await DisplayLoadoutMeals();
                await DisplayGoalAndProgress();
                await SetGoalRatios();
                await DisplayGoalRatios();
                await SetLoadoutName();
            }
        }

        private async Task SetLoadoutName()
        {
            lblLoadoutHeader.Text = MasterModel.currentUser.SelectedLoadout.LoadoutName;
        }

        private async Task DisplayGoalRatios()
        {
            lblProteinRatio.Text = $"{string.Format("{0:0}%", goalProgress.proteinRatio)}/{string.Format("{0:0}%", dailyGoal.proteinRatio)}";
            lblCarbRatio.Text = $"{string.Format("{0:0}%", goalProgress.carbRatio)}/{string.Format("{0:0}%", dailyGoal.carbRatio)}";
            lblFatRatio.Text = $"{string.Format("{0:0}%", goalProgress.fatRatio)}/{string.Format("{0:0}%", dailyGoal.fatRatio)}";
        }

        private async Task SetGoalRatios()
        {
            dailyGoal = await CalculatorV2.CalcRatiosAsync(dailyGoal);
            goalProgress = await CalculatorV2.CalcRatiosAsync(goalProgress);
        }

        private async Task DisplayGoalAndProgress()
        {
            lblEnergy.Text = $"{string.Format("{0:0}", goalProgress.energyKcal)}/{dailyGoal.energyKcal}";
            lblProtein.Text = $"{string.Format("{0:0}", goalProgress.protein)}/{dailyGoal.protein}";
            lblCarbs.Text = $"{string.Format("{0:0}", goalProgress.carbs)}/{dailyGoal.carbs}";
            lblFat.Text = $"{string.Format("{0:0}", goalProgress.fat)}/{dailyGoal.fat}";
        }

        private async Task GetUserData()
        {
            //MasterModel.currentUser = await MasterModel.DAL.GetUserDataAsync();
            //loadoutMeals = MasterModel.currentUser.SelectedLoadout.Meals;
            //dailyGoal = MasterModel.currentUser.DailyGoal;
            //goalProgress = MasterModel.currentUser.GoalProgress;

            try
            {
                loadout = await MasterModel.DAL.GetSelectedLoadoutAsync();
                loadoutMeals = loadout.Meals;
                dailyGoal = await MasterModel.DAL.GetGoalAsync();
                goalProgress = await MasterModel.DAL.GetGoalProgressAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
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

                Frame frame = new Frame();
                frame.Content = itemGrid;
                frame.BackgroundColor = Color.DarkRed;
                frame.Padding = new Thickness(10, 10);
                frame.CornerRadius = 5;

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
                chkbxEaten.Color = Color.White;

                //check meal status from database and set accordingly.
                if (loadoutMeals[meal.index].isEaten == true)
                {
                    chkbxEaten.IsChecked = true;
                }
                else
                {
                    chkbxEaten.IsChecked = false;
                }

                chkbxEaten.CheckedChanged += async (sender, args) =>
                {
                    if (chkbxEaten.IsChecked == true)
                    {
                        goalProgress.energyKcal += loadoutMeals[meal.index].totalEnergy;
                        goalProgress.protein += loadoutMeals[meal.index].totalProtein;
                        goalProgress.carbs += loadoutMeals[meal.index].totalCarbs;
                        goalProgress.fat += loadoutMeals[meal.index].totalFat;

                        loadoutMeals[meal.index].isEaten = true;
                    }
                    else
                    {
                        goalProgress.energyKcal -= loadoutMeals[meal.index].totalEnergy;
                        goalProgress.protein -= loadoutMeals[meal.index].totalProtein;
                        goalProgress.carbs -= loadoutMeals[meal.index].totalCarbs;
                        goalProgress.fat -= loadoutMeals[meal.index].totalFat;

                        loadoutMeals[meal.index].isEaten = false;
                    }
                    await DisplayGoalAndProgress(); //update
                    await SetGoalRatios();
                    await DisplayGoalRatios();

                    //update db per tick
                    await MasterModel.DAL.SaveGoalAsync(dailyGoal);
                    await MasterModel.DAL.SaveGoalProgressAsync(goalProgress);
                    //await MasterModel.DAL.SaveLoadoutMeals(loadoutMeals);
                    loadout.Meals = loadoutMeals;
                    await MasterModel.DAL.SaveSelectedLoadout(loadout);

                    //await MasterModel.DAL.SaveUserDataAsync(MasterModel.currentUser);


                };

                stack.Children.Add(chkbxEaten);

                itemGrid.Children.Add(stack);

                loadoutMealsStack.Children.Add(frame);
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

        private async void ClearProgress_Clicked(object sender, EventArgs e)
        {
            foreach (var meal in loadoutMeals)
            {
                meal.isEaten = false;
            }

            //MasterModel.currentUser.GoalProgress.energyKcal = 0;
            //MasterModel.currentUser.GoalProgress.protein = 0;
            //MasterModel.currentUser.GoalProgress.carbs = 0;
            //MasterModel.currentUser.GoalProgress.fat = 0;

            goalProgress.energyKcal = 0;
            goalProgress.protein = 0;
            goalProgress.carbs = 0;
            goalProgress.fat = 0;

            //MasterModel.currentUser.SelectedLoadout.Meals = loadoutMeals;
            await MasterModel.DAL.SaveSelectedLoadoutMealsAsync(loadoutMeals);
            //await MasterModel.DAL.SaveUserDataAsync(MasterModel.currentUser);
            await RefreshPageAsync();
        }

        private async Task RefreshPageAsync()
        {
            var refreshedPage = new NutritionPageV2(); Navigation.InsertPageBefore(refreshedPage, this); await Navigation.PopAsync();
        }
    }
}