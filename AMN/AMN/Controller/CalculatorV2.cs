using AMN.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AMN.Controller
{
    /* Created a lot of async methods in an attempt to
     * process more things on background threads. */
    public class CalculatorV2
    {
        private static Meal thisMeal;
        private static Loadout thisLoadout;
        public static async Task<AMN.Model.Meal> MacroTotalsAsync(AMN.Model.Meal meal)
        {
            thisMeal = meal;
            await ResetTotalsAsync();
            for (int i = 0; i < thisMeal.items.Count; i++)
            {
                await CalcTotalsAsync(i);
            }
            return thisMeal;
        }

        private async static Task ResetTotalsAsync()
        {
            thisMeal.totalEnergy = 0;
            thisMeal.totalProtein = 0;
            thisMeal.totalCarbs = 0;
            thisMeal.totalFat = 0;
        }

        private async static Task CalcTotalsAsync(int i)
        {
            thisMeal.totalEnergy += thisMeal.items[i].energyKcal;
            thisMeal.totalProtein += thisMeal.items[i].protein;
            thisMeal.totalCarbs += thisMeal.items[i].carbs;
            thisMeal.totalFat += thisMeal.items[i].fat;
        }

        public static async Task<Loadout> MacroTotalsLoadoutAsync(Loadout loadout)
        {
            thisLoadout = loadout;
            await ResetTotalsLoadoutAsync();
            for (int i = 0; i < thisLoadout.Meals.Count; i++)
            {
                await CalcTotalsLoadoutAsync(i);
            }
            return thisLoadout;
        }

        private async static Task CalcTotalsLoadoutAsync(int i)
        {
            thisLoadout.totalEnergy += thisLoadout.Meals[i].totalEnergy;
            thisLoadout.totalProtein += thisLoadout.Meals[i].totalProtein;
            thisLoadout.totalCarbs += thisLoadout.Meals[i].totalCarbs;
            thisLoadout.totalFat += thisLoadout.Meals[i].totalFat;
        }

        private async static Task ResetTotalsLoadoutAsync()
        {
            thisLoadout.totalEnergy = 0;
            thisLoadout.totalProtein = 0;
            thisLoadout.totalCarbs = 0;
            thisLoadout.totalFat = 0;
        }

        public async static Task<MacroNutrients> CalcRatiosAsync(MacroNutrients macros)
        {
            double total = macros.protein + macros.carbs + macros.fat;

            macros.proteinRatio = (macros.protein / total) * 100;
            macros.carbRatio = (macros.carbs / total) * 100;
            macros.fatRatio = (macros.fat / total) * 100;

            return macros;
        }
    }
}
