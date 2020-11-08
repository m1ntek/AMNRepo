using AMN.Model;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AMN.Controller
{
    public class CardCreator
    {
        public StackLayout CreateMealCards(List<Meal> meals, StackLayout masterStack)
        {
            for (int i = 0; i < meals.Count; i++)
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

                itemGrid.BackgroundColor = Color.FromHex("2196F3");
                itemGrid.Padding = new Thickness(10, 10);

                //save index number per foodItem... To fix a bug
                //MasterModel.tempMeal.items[i].index = i;

                //FoodItem item = MasterModel.tempMeal.items[i];

                Label lblName = new Label();
                lblName = CreateLabelTemplate(lblName, LayoutOptions.Start, 0, 0, 1, meals[i].mealName);
                lblName.TextColor = Color.White;

                itemGrid.Children.Add(lblName);

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

                btnEdit.Clicked += (sender, args) =>
                {
                    
                };

                btnDelete.Clicked += (sender, args) =>
                {
                    meals.RemoveAt(i);
                };

                stack.Children.Add(btnEdit);
                stack.Children.Add(btnDelete);

                itemGrid.Children.Add(stack);

                masterStack.Children.Add(itemGrid);
            }

            return masterStack;
        }
        public static Label CreateLabelTemplate(Label lbl, LayoutOptions horizontalLayout, int gridColumn, int gridRow, int columnSpan, string name)
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
    }
}
