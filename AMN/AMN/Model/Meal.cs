using System;
using System.Collections.Generic;
using System.Text;

namespace AMN.Model
{
    public class Meal
    {
        public List<FoodItem> items;

        public Meal()
        {
            items = new List<FoodItem>();
        }
    }
}
