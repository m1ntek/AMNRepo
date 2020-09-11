using AMN.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace AMN.Controller
{
    public static class MasterModel
    {
        public static DataAccessLayer DAL = new DataAccessLayer();
        public static LoginController loginC = new LoginController();
        public static Validator vd = new Validator();
        public static Person currentUser = new Person();
        public static APIController apiC = new APIController();
        public static FoodResult currentFoodResult;
        public static Meal tempMeal = new Meal();
    }
}
