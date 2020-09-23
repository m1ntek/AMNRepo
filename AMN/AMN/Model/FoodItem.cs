using AMN.Controller;
using System;
using System.Collections.Generic;
using System.Text;

namespace AMN.Model
{
    public class FoodItem
    {
        public string name, currentLocalId;
        public double energyKcal, carbs, fat, protein, serving;
        public int index;

        public FoodItem(string _name, double _energy, double _carbs, double _fat, double _protein, double _serving)
        {
            name = _name;
            energyKcal = _energy;
            carbs = _carbs;
            fat = _fat;
            protein = _protein;
            serving = _serving;
            currentLocalId = MasterModel.DAL.GetCurrentLocalId();
        }
    }
}
