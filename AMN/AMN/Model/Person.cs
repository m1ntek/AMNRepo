using System;
using System.Collections.Generic;
using System.Security;
using System.Text;

namespace AMN.Model
{
    public class Person
    {
        public string email;
        public bool rememberMe;
        public MacroNutrients DailyGoal;
        public MacroNutrients GoalProgress;
        public List<Meal> Meals;
        public Loadout SelectedLoadout;
        public List<Meal> TempLoadoutMeals;
        public List<Loadout> Loadouts;

        public Person()
        {
            email = "";
            rememberMe = false;
            DailyGoal = new MacroNutrients();
            GoalProgress = new MacroNutrients();
            Meals = new List<Meal>();
            Loadouts = new List<Loadout>();
            TempLoadoutMeals = new List<Meal>();
            SelectedLoadout = new Loadout();
        }
    }
}
