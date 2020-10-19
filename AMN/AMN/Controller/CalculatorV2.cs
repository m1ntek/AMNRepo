using AMN.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AMN.Controller
{
    public class CalculatorV2
    {
        private static Meal thisMeal;
        public static async Task<AMN.Model.Meal> MacroTotals(AMN.Model.Meal meal)
        {
            thisMeal = meal;
            //List<Task> calcTasks = new List<Task>();
            for (int i = 0; i < thisMeal.items.Count; i++)
            {
                await CalcTotals(i);
            }
            ///await Task.WhenAll(calcTasks);
            return thisMeal;
        }

        private async static Task CalcTotals(int i)
        {
            thisMeal.totalEnergy += thisMeal.items[i].energyKcal;
            thisMeal.totalProtein += thisMeal.items[i].protein;
            thisMeal.totalCarbs += thisMeal.items[i].carbs;
            thisMeal.totalFat += thisMeal.items[i].fat;
        }
    }
}
