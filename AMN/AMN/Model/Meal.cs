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

            double _totalEnergy = 0, _totalProtein = 0, _totalCarb = 0, _totalFat = 0;
            foreach (var foodItem in items)
            {
                _totalEnergy += foodItem.energyKcal;
                _totalProtein += foodItem.protein;
                _totalCarb += foodItem.carbs;
                _totalFat += foodItem.fat;
            }

            totalEnergy = "Energy: " + _totalEnergy.ToString("0.00");
            totalProtein = "Protein: " + _totalProtein.ToString("0.00");
            totalCarbs = "Carbs: " + _totalCarb.ToString("0.00");
            totalFat = "Fat: " + _totalFat.ToString("0.00");
        }
    }
}
