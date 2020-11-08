using System;
using System.Collections.Generic;
using System.Text;

namespace AMN.Controller
{
    public class Calculator
    {
        public static AMN.Model.Meal MacroTotals(AMN.Model.Meal meal)
        {
            for (int i = 0; i < meal.items.Count; i++)
            {
                meal.totalEnergy += meal.items[i].energyKcal;
                meal.totalProtein += meal.items[i].protein;
                meal.totalCarbs += meal.items[i].carbs;
                meal.totalFat += meal.items[i].fat;
            }
            return meal;
        }
    }
}
