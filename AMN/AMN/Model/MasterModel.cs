using AMN.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace AMN.Controller
{
    public static class MasterModel
    {
        //I ended up developing too much using this class
        //I'm wondering if it is bad practice doing something like this, I'm starting to think it is.
        //I experimented using it less, after in-scope tasks are complete, I may look to re-design code
        //to either use local db first or just push/pull from firebase instead of this.
        public static DataAccessLayer DAL = new DataAccessLayer();
        public static LoginController loginC = new LoginController();
        public static Validator vd = new Validator();
        public static Person currentUser = new Person();
        public static APIController apiC = new APIController();
        public static FoodResult currentFoodResult;
        public static Meal tempMeal = new Meal();
        public static MacroNutrients dailyGoal = new MacroNutrients();
        public static MacroNutrients dailyProgress = new MacroNutrients();
        public static MealCollection userMeals = new MealCollection();
    }
}
