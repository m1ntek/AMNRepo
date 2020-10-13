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
        public List<Meal> Meals;
        public List<Meal> DaysMeals;

        public Person()
        {
            email = "";
            rememberMe = false;
            DailyGoal = new MacroNutrients();
            Meals = new List<Meal>();
        }
    }
}
