using AMN.Controller;
using AMN.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Markup;
using Xamarin.Forms.Xaml;

namespace AMN.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddMealPage : ContentPage
    {
        int currentFoodItemIndex = -1;

        public AddMealPage()
        {
            InitializeComponent();
        }

        private void UpdateText()
        {
            entryEnergy.Text = MasterModel.currentFoodResult.resultKcal.ToString();
            entryCarbs.Text = MasterModel.currentFoodResult.resultCarb.ToString();
            entryFat.Text = MasterModel.currentFoodResult.resultFat.ToString();
            entryServing.Text = MasterModel.currentFoodResult.resultServing.ToString();
        }

        private async Task ActNameOn()
        {
            actName.IsRunning = true;
        }

        private async Task ActNameOff()
        {
            actName.IsRunning = false;
        }

        private async Task QueryAPI()
        {
            MasterModel.apiC.Query(entryName.Text);
        }

        bool isFirstSearch = true;

        private async void entryName_Unfocused(object sender, FocusEventArgs e)
        {
            //actName.IsRunning = true;
            await ActNameOn();

            var queryTask = QueryAPI();

            if (string.IsNullOrEmpty(entryName.Text) == false)
            {
                try
                {
                    await queryTask;
                    MasterModel.currentFoodResult = new Model.FoodResult();
                    UpdateText();
                }
                catch (Exception ex)
                {
                    //do nothing
                }
            }

            await ActNameOff();
                
            
        }

        private void entryEnergy_Focused(object sender, FocusEventArgs e)
        {
            entryEnergy.Text = "";
        }

        private void entryEnergy_Unfocused(object sender, FocusEventArgs e)
        {
            if (string.IsNullOrEmpty(entryEnergy.Text) == true && MasterModel.currentFoodResult != null)
            {
                entryEnergy.Text = MasterModel.currentFoodResult.resultKcal.ToString();
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
            if (string.IsNullOrEmpty(entryCarbs.Text) == true && MasterModel.currentFoodResult != null)
            {
                entryCarbs.Text = MasterModel.currentFoodResult.resultCarb.ToString();
            }
            else
            {
                MasterModel.currentFoodResult.resultCarb = Convert.ToDouble(entryCarbs.Text);
            }
        }

        private void entryFat_Focused(object sender, FocusEventArgs e)
        {
            entryFat.Text = "";
        }

        private void entryFat_Unfocused(object sender, FocusEventArgs e)
        {
            if (string.IsNullOrEmpty(entryFat.Text) == true && MasterModel.currentFoodResult != null)
            {
                entryFat.Text = MasterModel.currentFoodResult.resultFat.ToString();
            }
            else
            {
                MasterModel.currentFoodResult.resultFat = Convert.ToDouble(entryFat.Text);
            }
        }

        private void entryServing_Focused(object sender, FocusEventArgs e)
        {
            entryServing.Text = "";
        }

        private void entryServing_Unfocused(object sender, FocusEventArgs e)
        {
            if (string.IsNullOrEmpty(entryServing.Text) == true && MasterModel.currentFoodResult != null)
            {
                entryServing.Text = MasterModel.currentFoodResult.resultServing.ToString();
            }

            MasterModel.currentFoodResult.resultKcal = MasterModel.currentFoodResult.resultKcal / MasterModel.currentFoodResult.resultServing * Convert.ToDouble(entryServing.Text);
            MasterModel.currentFoodResult.resultCarb = MasterModel.currentFoodResult.resultCarb / MasterModel.currentFoodResult.resultServing * Convert.ToDouble(entryServing.Text);
            MasterModel.currentFoodResult.resultFat = MasterModel.currentFoodResult.resultFat / MasterModel.currentFoodResult.resultServing * Convert.ToDouble(entryServing.Text);
            MasterModel.currentFoodResult.resultServing = Convert.ToDouble(entryServing.Text);

            UpdateText();
        }

        private void CreateNewItem()
        {
            Grid itemGrid = new Grid();
            itemGrid.ColumnDefinitions.Add(new ColumnDefinition());
            itemGrid.ColumnDefinitions.Add(new ColumnDefinition());
            itemGrid.ColumnDefinitions.Add(new ColumnDefinition());
            itemGrid.RowDefinitions.Add(new RowDefinition());
            itemGrid.RowDefinitions.Add(new RowDefinition());
            itemGrid.RowDefinitions.Add(new RowDefinition());

            itemGrid.RowDefinitions[1].Height = new GridLength(0.5, GridUnitType.Star);
            itemGrid.RowDefinitions[2].Height = new GridLength(0.5, GridUnitType.Star);

            FoodItem item = new FoodItem(entryName.Text, MasterModel.currentFoodResult.resultKcal, MasterModel.currentFoodResult.resultCarb, MasterModel.currentFoodResult.resultFat, Convert.ToDouble(entryServing.Text));
            MasterModel.tempMeal.items.Add(item);

            Label lblName = new Label();
            lblName = CreateLabelTemplate(lblName, LayoutOptions.Start, 0, 0, 1, item.name);
            Label lblEnergy = new Label();
            lblEnergy = CreateLabelTemplate(lblEnergy, LayoutOptions.Start, 0, 1, 1, item.energyKcal.ToString());
            Label lblCarbs = new Label();
            lblCarbs = CreateLabelTemplate(lblCarbs, LayoutOptions.Center, 1, 1, 1, item.carbs.ToString());
            Label lblFat = new Label();
            lblFat = CreateLabelTemplate(lblFat, LayoutOptions.End, 2, 1, 1, item.fat.ToString());
            Label lblServing = new Label();
            lblServing = CreateLabelTemplate(lblServing, LayoutOptions.End, 1, 2, 3, item.serving.ToString());

            itemGrid.Children.Add(lblName);
            itemGrid.Children.Add(lblEnergy);
            itemGrid.Children.Add(lblCarbs);
            itemGrid.Children.Add(lblFat);
            itemGrid.Children.Add(lblServing);

            StackLayout stack = new StackLayout();
            stack.Orientation = StackOrientation.Horizontal;
            Grid.SetColumnSpan(stack, 3);
            stack.HorizontalOptions = LayoutOptions.End;

            itemGrid.Children.Add(stack);
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

                itemGrid.RowDefinitions[1].Height = new GridLength(0.5, GridUnitType.Star);
                itemGrid.RowDefinitions[2].Height = new GridLength(0.5, GridUnitType.Star);

                //save index number per foodItem... To fix a bug
                MasterModel.tempMeal.items[i].index = i;

                FoodItem item = MasterModel.tempMeal.items[i];

                Label lblName = new Label();
                lblName = CreateLabelTemplate(lblName, LayoutOptions.Start, 0, 0, 1, item.name);
                Label lblEnergy = new Label();
                lblEnergy = CreateLabelTemplate(lblEnergy, LayoutOptions.Start, 0, 1, 1, $"E: {item.energyKcal}");
                Label lblCarbs = new Label();
                lblCarbs = CreateLabelTemplate(lblCarbs, LayoutOptions.Center, 1, 1, 1, $"C: {item.carbs}");
                Label lblFat = new Label();
                lblFat = CreateLabelTemplate(lblFat, LayoutOptions.End, 2, 1, 1, $"F: {item.fat}");
                Label lblServing = new Label();
                lblServing = CreateLabelTemplate(lblServing, LayoutOptions.End, 1, 2, 2, $"Serving: {item.serving}");

                itemGrid.Children.Add(lblName);
                itemGrid.Children.Add(lblEnergy);
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
                    BackgroundColor = Color.FromHex("2196F3"),
                    TextColor = Color.White,
                    HeightRequest = 35,
                    WidthRequest = 50
                };

                Button btnDelete = new Button()
                {
                    Text = "Del",
                    FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Button)),
                    BackgroundColor = Color.DarkRed,
                    TextColor = Color.White,
                    HeightRequest = 35,
                    WidthRequest = 50
                };

                btnEdit.Clicked += (sender, args) =>
                {
                    MasterModel.currentFoodResult.resultName = item.name;
                    MasterModel.currentFoodResult.resultKcal = item.energyKcal;
                    MasterModel.currentFoodResult.resultCarb = item.carbs;
                    MasterModel.currentFoodResult.resultFat = item.fat;
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

                stackFoodItems.Children.Add(itemGrid);
            }
        }

        private Label CreateLabelTemplate(Label lbl, LayoutOptions horizontalLayout, int gridColumn, int gridRow, int columnSpan, string name)
        {
            lbl.TextColor = Color.Black;
            lbl.VerticalOptions = LayoutOptions.Center;
            lbl.HorizontalOptions = horizontalLayout;
            //lbl.Row(gridRow);
            Grid.SetRow(lbl, gridRow);
            //lbl.Column(gridColumn);
            Grid.SetColumn(lbl, gridColumn);
            //lbl.ColumnSpan(columnSpan);
            Grid.SetColumnSpan(lbl, columnSpan);
            lbl.Text = name;

            return lbl;
        }

        private void addFood_Clicked(object sender, EventArgs e)
        {
            try
            {
                CreateNewItem();
                RefreshPage();
            }
            catch (Exception ex)
            {
                DisplayActionSheet("Error", ex.Message, "OK");
            }

        }

        private void RefreshPage()
        {
            var refreshedPage = new AddMealPage(); Navigation.InsertPageBefore(refreshedPage, this); Navigation.PopAsync();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadFoodItems();
        }

        //appears on edit food item clicked
        private void editFood_Clicked(object sender, EventArgs e)
        {
            MasterModel.tempMeal.items[currentFoodItemIndex].name = entryName.Text;
            MasterModel.tempMeal.items[currentFoodItemIndex].energyKcal = Convert.ToDouble(entryEnergy.Text);
            MasterModel.tempMeal.items[currentFoodItemIndex].carbs = Convert.ToDouble(entryCarbs.Text);
            MasterModel.tempMeal.items[currentFoodItemIndex].fat = Convert.ToDouble(entryFat.Text);
            MasterModel.tempMeal.items[currentFoodItemIndex].serving = Convert.ToDouble(entryServing.Text);

            RefreshPage();
        }

        private void SaveMeal_Clicked(object sender, EventArgs e)
        {

        }
    }
}