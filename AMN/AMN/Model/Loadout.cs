using System;
using System.Collections.Generic;
using System.Text;

namespace AMN.Model
{
    public class Loadout
    {
        public string Key { get; set; }
        public int LoadoutId { get; set; }
        public List<Meal> Meals { get; set; }
        public string LoadoutName { get; set; }
        public double totalEnergy { get; set; }
        public double totalProtein { get; set; }
        public double totalCarbs { get; set; }
        public double totalFat { get; set; }
        public Loadout()
        {
            Meals = new List<Meal>();
        }
    }
}
