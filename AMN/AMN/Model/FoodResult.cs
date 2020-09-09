using AMN.Controller;
using System;
using System.Collections.Generic;
using System.Text;

namespace AMN.Model
{
    public class FoodResult
    {
        public string resultName;
        public double resultKcal;
        public double resultCarb;
        public double resultProtein;
        public double resultFat;
        public double resultServing;

        public FoodResult()
        {
            resultName = MasterController.apiC.queryResult.ingredients[0].parsed[0].foodMatch;
            resultKcal = MasterController.apiC.queryResult.ingredients[0].parsed[0].nutrients.ENERC_KCAL.quantity;
            resultCarb = MasterController.apiC.queryResult.ingredients[0].parsed[0].nutrients.CHOCDF.quantity;
            resultProtein = MasterController.apiC.queryResult.ingredients[0].parsed[0].nutrients.PROCNT.quantity;
            resultFat = MasterController.apiC.queryResult.ingredients[0].parsed[0].nutrients.FAT.quantity;
            resultServing = MasterController.apiC.queryResult.ingredients[0].parsed[0].weight;
        }
    }
}
