using AMN.Controller;
using System;
using System.Collections.Generic;
using System.Text;

namespace AMN.Model
{
    public class Meal
    {
        public int index;
        public int mealId;
        public string userLocalId;
        public string mealName;
        public List<FoodItem> items;

        public Meal()
        {
            index = -1;
            mealId = 0;
            items = new List<FoodItem>();
        }
    }
}
