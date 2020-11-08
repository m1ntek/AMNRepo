using AMN.Controller;
using AMN.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Xamarin.Forms;
using Xamarin.Forms.Markup;
using Xamarin.Forms.Xaml;

namespace AMN.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddMealPageV2 : ContentPage
    {
        int currentFoodItemIndex = -1;

        public AddMealPageV2()
        {
            InitializeComponent();

            if(MasterModel.tempMeal.index == -1)
            {
                btnDeleteMeal.IsVisible = false;
            }
        }

        private void UpdateText()
        {
            entryEnergy.Text = MasterModel.currentFoodResult.resultKcal.ToString("0.00");
            entryCarbs.Text = MasterModel.currentFoodResult.resultCarb.ToString("0.00");
            entryFat.Text = MasterModel.currentFoodResult.resultFat.ToString("0.00");
            entryServing.Text = MasterModel.currentFoodResult.resultServing.ToString("0.00");
            entryProtein.Text = MasterModel.currentFoodResult.resultProtein.ToString("0.00");
        }

        //set to 1 gram on default
        private void SetDefaultServing()
        {
            double serving, kcal, carb, fat, protein;

            serving = MasterModel.currentFoodResult.resultServing / MasterModel.currentFoodResult.resultServing;
            kcal = MasterModel.currentFoodResult.resultKcal / MasterModel.currentFoodResult.resultServing;
            carb = MasterModel.currentFoodResult.resultCarb / MasterModel.currentFoodResult.resultServing;
            fat = MasterModel.currentFoodResult.resultFat / MasterModel.currentFoodResult.resultServing;
            protein = MasterModel.currentFoodResult.resultProtein / MasterModel.currentFoodResult.resultServing;

            //separated to resolve a bug
            MasterModel.currentFoodResult.resultServing = serving;
            MasterModel.currentFoodResult.resultKcal = kcal;
            MasterModel.currentFoodResult.resultCarb = carb;
            MasterModel.currentFoodResult.resultFat = fat;
            MasterModel.currentFoodResult.resultProtein = protein;
        }

        private async void entryName_Unfocused(object sender, FocusEventArgs e)
        {
            actName.IsRunning = true;

            if (string.IsNullOrEmpty(entryName.Text) == false)
            {
                try
                {
                    await MasterModel.apiC.Query(entryName.Text);
                    MasterModel.currentFoodResult = new Model.FoodResult();
                    SetDefaultServing();
                    UpdateText();
                }
                catch (Exception)
                {
                    //do nothing
                }
            }

            actName.IsRunning = false;
        }

        private void entryEnergy_Focused(object sender, FocusEventArgs e)
        {
            entryEnergy.Text = "";
        }

        private void entryEnergy_Unfocused(object sender, FocusEventArgs e)
        {
            if (string.IsNullOrEmpty(entryEnergy.Text) == true)
            {
                if(MasterModel.currentFoodResult != null)
                {
                    entryEnergy.Text = MasterModel.currentFoodResult.resultKcal.ToString();
                }
            }
            else
            {
                MasterModel.currentFoodResult.resultKcal = Convert.ToDouble(entryEnergy.Text);
            }
        }

        private void entryCarbs_Focused(object sender, FocusEventArgs e)
        {
            entryCarbs.Text = "";
        }

        private void entryCarbs_Unfocused(object sender, FocusEventArgs e)
        {
            if (string.IsNullOrEmpty(entryCarbs.Text) == true)
            {
                if (MasterModel.currentFoodResult != null)
                {
                    entryCarbs.Text = MasterModel.currentFoodResult.resultCarb.ToString();
                }
            }
            else
            {
                MasterModel.currentFoodResult.resultCarb = Convert.ToDouble(entryCarbs.Text);
            }

            entryFat.Focus();
        }

        private void entryFat_Focused(object sender, FocusEventArgs e)
        {
            entryFat.Text = "";
        }

        private void entryFat_Unfocused(object sender, FocusEventArgs e)
        {
            if (string.IsNullOrEmpty(entryFat.Text) == true)
            {
                if (MasterModel.currentFoodResult != null)
                {
                    entryFat.Text = MasterModel.currentFoodResult.resultFat.ToString();
                }
            }
            else
            {
                MasterModel.currentFoodResult.resultFat = Convert.ToDouble(entryFat.Text);
            }
            entryEnergy.Focus();
        }

        private void entryServing_Focused(object sender, FocusEventArgs e)
        {
            entryServing.Text = "";
        }

        private void entryServing_Unfocused(object sender, FocusEventArgs e)
        {
            if (string.IsNullOrEmpty(entryServing.Text) == true)
            {
                if (MasterModel.currentFoodResult != null)
                {
                    entryServing.Text = MasterModel.currentFoodResult.resultServing.ToString();
                }
            }

            else
            {
                MasterModel.currentFoodResult.resultKcal = MasterModel.currentFoodResult.resultKcal / MasterModel.currentFoodResult.resultServing * Convert.ToDouble(entryServing.Text);
                MasterModel.currentFoodResult.resultCarb = MasterModel.currentFoodResult.resultCarb / MasterModel.currentFoodResult.resultServing * Convert.ToDouble(entryServing.Text);
                MasterModel.currentFoodResult.resultFat = MasterModel.currentFoodResult.resultFat / MasterModel.currentFoodResult.resultServing * Convert.ToDouble(entryServing.Text);
                MasterModel.currentFoodResult.resultProtein = MasterModel.currentFoodResult.resultProtein / MasterModel.currentFoodResult.resultServing * Convert.ToDouble(entryServing.Text);
                MasterModel.currentFoodResult.resultServing = Convert.ToDouble(entryServing.Text);

                UpdateText(); 
            }
        }

        private void CreateNewItem()
        {
            FoodItem item = new FoodItem(entryName.Text, MasterModel.currentFoodResult.resultKcal, MasterModel.currentFoodResult.resultCarb, MasterModel.currentFoodResult.resultFat, MasterModel.currentFoodResult.resultProtein, Convert.ToDouble(entryServing.Text));
            MasterModel.tempMeal.items.Add(item);
        }

        private void LoadFoodItems()
        {
            for (int i = 0; i < MasterModel.tempMeal.items.Count; i++)
            {
                Grid itemGrid = new Grid();
                itemGrid.ColumnDefinitions.Add(new ColumnDefinition());
                itemGrid.ColumnDefinitions.Add(new ColumnDefinition());
                itemGrid.ColumnDefinitions.Add(new ColumnDefinition());
                itemGrid.RowDefinitions.Add(new RowDefinition());
                itemGrid.RowDefinitions.Add(new RowDefinition());
                itemGrid.RowDefinitions.Add(new RowDefinition());
                itemGrid.RowDefinitions.Add(new RowDefinition());

                itemGrid.RowDefinitions[1].Height = new GridLength(0.5, GridUnitType.Star);
                itemGrid.RowDefinitions[2].Height = new GridLength(0.5, GridUnitType.Star);

                Frame frame = new Frame();
                frame.Content = itemGrid;
                frame.BackgroundColor = Color.DarkRed;
                frame.Padding = new Thickness(10, 10);
                frame.CornerRadius = 5;

                //save index number per foodItem... To fix a bug
                MasterModel.tempMeal.items[i].index = i;

                FoodItem item = MasterModel.tempMeal.items[i];

                Label lblName = new Label();
                lblName = CreateLabelTemplate(lblName, LayoutOptions.Start, 0, 0, 1, item.name);
                lblName.TextColor = Color.White;
                Label lblEnergy = new Label();
                lblEnergy = CreateLabelTemplate(lblEnergy, LayoutOptions.Start, 0, 1, 2, $"Energy: {item.energyKcal.ToString("0.00")} kcal");
                lblEnergy.TextColor = Color.White;
                Label lblProtein = new Label();
                lblProtein = CreateLabelTemplate(lblProtein, LayoutOptions.Start, 0, 2, 1, $"P: {item.protein.ToString("0.00")} g");
                lblProtein.TextColor = Color.White;
                Label lblCarbs = new Label();
                lblCarbs = CreateLabelTemplate(lblCarbs, LayoutOptions.Center, 1, 2, 1, $"C: {item.carbs.ToString("0.00")} g");
                lblCarbs.TextColor = Color.White;
                Label lblFat = new Label();
                lblFat = CreateLabelTemplate(lblFat, LayoutOptions.End, 2, 2, 1, $"F: {item.fat.ToString("0.00")} g");
                lblFat.TextColor = Color.White;
                Label lblServing = new Label();
                lblServing = CreateLabelTemplate(lblServing, LayoutOptions.End, 1, 3, 2, $"Serving: {item.serving.ToString("0.00")} g");
                lblServing.TextColor = Color.White;

                itemGrid.Children.Add(lblName);
                itemGrid.Children.Add(lblEnergy);
                itemGrid.Children.Add(lblProtein);
                itemGrid.Children.Add(lblCarbs);
                itemGrid.Children.Add(lblFat);
                itemGrid.Children.Add(lblServing);

                StackLayout stack = new StackLayout();
                stack.Orientation = StackOrientation.Horizontal;
                Grid.SetColumnSpan(stack, 3);
                stack.HorizontalOptions = LayoutOptions.End;

                Button btnEdit = new Button()
                {
                    Text = "Edit",
                    FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Button)),
                    BackgroundColor = Color.Black,
                    BorderColor = Color.White,
                    BorderWidth = 1,
                    TextColor = Color.White,
                    HeightRequest = 35,
                    WidthRequest = 50,
                    CornerRadius = 5,
                    Padding = new Thickness(0, 0),
                    VerticalOptions = LayoutOptions.Center
                };

                Button btnDelete = new Button()
                {
                    Text = "Del",
                    FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Button)),
                    BackgroundColor = Color.Black,
                    BorderColor = Color.White,
                    BorderWidth = 1,
                    TextColor = Color.White,
                    HeightRequest = 35,
                    WidthRequest = 50,
                    CornerRadius = 5,
                    Padding = new Thickness(0, 0),
                    VerticalOptions = LayoutOptions.Center
                };

                btnEdit.Clicked += (sender, args) =>
                {
                    MasterModel.currentFoodResult = new FoodResult();
                    MasterModel.currentFoodResult.resultName = item.name;
                    MasterModel.currentFoodResult.resultKcal = item.energyKcal;
                    MasterModel.currentFoodResult.resultCarb = item.carbs;
                    MasterModel.currentFoodResult.resultFat = item.fat;
                    MasterModel.currentFoodResult.resultProtein = item.protein;
                    MasterModel.currentFoodResult.resultServing = item.serving;

                    UpdateText();

                    //updatetext method doesn't do name
                    entryName.Text = item.name;

                    btnAddFood.IsVisible = false;
                    btnEditFood.IsVisible = true;

                    //update current index to class variable
                    currentFoodItemIndex = item.index;
                };

                btnDelete.Clicked += (sender, args) =>
                {
                    MasterModel.tempMeal.items.RemoveAt(item.index);
                    RefreshPage();
                };

                stack.Children.Add(btnEdit);
                stack.Children.Add(btnDelete);

                itemGrid.Children.Add(stack);

                stackFoodItems.Children.Add(frame);
            }
        }

        private Label CreateLabelTemplate(Label lbl, LayoutOptions horizontalLayout, int gridColumn, int gridRow, int columnSpan, string name)
        {
            lbl.TextColor = Color.Black;
            lbl.VerticalOptions = LayoutOptions.Center;
            lbl.HorizontalOptions = horizontalLayout;
            Grid.SetRow(lbl, gridRow);
            Grid.SetColumn(lbl, gridColumn);
            Grid.SetColumnSpan(lbl, columnSpan);
            lbl.Text = name;
            lbl.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));

            return lbl;
        }

        private async void addFood_Clicked(object sender, EventArgs e)
        {
            bool isValid = MasterModel.vd.FormEntriesValid(new string[]
            {
                entryName.Text,
                entryServing.Text,
                entryProtein.Text,
                entryCarbs.Text,
                entryFat.Text,
                entryEnergy.Text
            });

            if(isValid == true)
            {
                try
                {
                    CreateNewItem();
                    RefreshPage();
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", $"Please try logging in or signing up before adding items.\n\n{ex.Message}", "OK");
                }
            }
            else
            {
                await DisplayAlert("Error", MasterModel.vd.error, "OK");
            }
        }

        private void RefreshPage()
        {
            var refreshedPage = new AddMealPageV2(); Navigation.InsertPageBefore(refreshedPage, this); Navigation.PopAsync();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadMealName();
            LoadFoodItems();
        }

        private void LoadMealName()
        {
            entryMealName.Text = MasterModel.tempMeal.mealName;
        }

        //appears on edit food item clicked
        private void editFood_Clicked(object sender, EventArgs e)
        {
            MasterModel.tempMeal.items[currentFoodItemIndex].name = entryName.Text;
            MasterModel.tempMeal.items[currentFoodItemIndex].energyKcal = Convert.ToDouble(entryEnergy.Text);
            MasterModel.tempMeal.items[currentFoodItemIndex].carbs = Convert.ToDouble(entryCarbs.Text);
            MasterModel.tempMeal.items[currentFoodItemIndex].fat = Convert.ToDouble(entryFat.Text);
            MasterModel.tempMeal.items[currentFoodItemIndex].protein = Convert.ToDouble(entryProtein.Text);
            MasterModel.tempMeal.items[currentFoodItemIndex].serving = Convert.ToDouble(entryServing.Text);

            RefreshPage();
        }

        private async void SaveMeal_Clicked(object sender, EventArgs e)
        {
            actSave.IsRunning = true;
            if(MasterModel.DAL.UserLoggedIn() == true)
            {
                try
                {
                    //If a new meal
                    if(MasterModel.tempMeal.index == -1)
                    {
                        bool isValid = MasterModel.vd.FormEntriesValid(new string[]
                        {
                            entryMealName.Text
                        });

                        if(isValid == false)
                        {
                            actSave.IsRunning = false;
                            await DisplayAlert("Meal Name", MasterModel.vd.error, "OK");
                            return;
                        }

                        if (MasterModel.tempMeal.items.Count == 0)
                        {
                            actSave.IsRunning = false;
                            await DisplayAlert("Error", "The meal must contain at least 1 food item.", "OK");
                            return;
                        }

                        await MasterModel.DAL.SaveNewMealAsync(MasterModel.tempMeal);
                    }

                    //If an existing meal
                    else
                    {
                        MasterModel.currentUser.Meals[MasterModel.tempMeal.index] = MasterModel.tempMeal;
                        await MasterModel.DAL.SaveMealV3Async(MasterModel.tempMeal, MasterModel.tempMeal.key);
                    }

                    actSave.IsRunning = false;
                    await Navigation.PopAsync();
                }
                catch (Exception ex)
                {
                    actSave.IsRunning = false;
                    await DisplayAlert("Error", $"Something went wrong:\n{ex.Message}", "OK");
                }
            }
            else
            {
                actSave.IsRunning = false;
                await DisplayAlert("Not Logged In", "Please login from the main page to save meals", "OK");
            }
        }

        private void entryProtein_Focused(object sender, FocusEventArgs e)
        {
            entryProtein.Text = "";
        }

        private void entryProtein_Unfocused(object sender, FocusEventArgs e)
        {
            if (string.IsNullOrEmpty(entryProtein.Text) == true)
            {
                if (MasterModel.currentFoodResult != null)
                {
                    entryProtein.Text = MasterModel.currentFoodResult.resultProtein.ToString();
                }
            }
            else
            {
                MasterModel.currentFoodResult.resultProtein = Convert.ToDouble(entryProtein.Text);
            }

            entryCarbs.Focus();
        }

        private void entryMealName_Unfocused(object sender, FocusEventArgs e)
        {
            MasterModel.tempMeal.mealName = entryMealName.Text;
        }

        private async void btnDeleteMeal_Clicked(object sender, EventArgs e)
        {
            //confirmation before deleting meal
            bool userConfirmed = await DisplayAlert("Confirmation", "Delete this meal?", "Yes", "No");

            if (userConfirmed == true)
            {
                try
                {
                    await MasterModel.DAL.DeleteMealAsync(MasterModel.tempMeal.key);
                    await Navigation.PopAsync();
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", ex.Message,"OK");
                }
            }
        }
    }
}