using AMN.Controller;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace AMN.Model
{
    public class Meal
    {
        public int index;
        public int mealId;
        public string userLocalId;
        //public string mealName;
        public string mealName { get; set; }
        //public List<FoodItem> items;
        public List<FoodItem> items { get; set; }
        public string totalEnergy { get; set; }
        public string totalProtein { get; set; }
        public string totalCarbs { get; set; }
        public string totalFat { get; set; }

        public Meal()
        {
            index = -1;
            mealId = 0;
            items = new List<FoodItem>();
        }
    }
}
