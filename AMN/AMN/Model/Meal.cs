using AMN.Controller;
using System;
using System.Collections.Generic;
using System.Text;

namespace AMN.Model
{
    public class Meal
    {
        public int mealId;
        public string userLocalId;
        public List<FoodItem> items;

        public Meal()
        {
            mealId = 0;
            items = new List<FoodItem>();
        }
    }
}
