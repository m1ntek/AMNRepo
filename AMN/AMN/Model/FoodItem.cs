using System;
using System.Collections.Generic;
using System.Text;

namespace AMN.Model
{
    public class FoodItem
    {
        public string name;
        public double energyKcal, carbs, fat, serving;
        public int index;

        public FoodItem(string _name, double _energy, double _carbs, double _fat, double _serving)
        {
            name = _name;
            energyKcal = _energy;
            carbs = _carbs;
            fat = _fat;
            serving = _serving;
        }
    }
}
