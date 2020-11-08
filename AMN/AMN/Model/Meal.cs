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
        public string key;
        public string userLocalId;
        public string mealName { get; set; }
        public List<FoodItem> items { get; set; }
        public double totalEnergy { get; set; }
        public double totalProtein { get; set; }
        public double totalCarbs { get; set; }
        public double totalFat { get; set; }

        public bool isEaten { get; set; }

        public Meal()
        {
            index = -1;
            mealId = 0;
            items = new List<FoodItem>();
            isEaten = false;
        }
    }
}
