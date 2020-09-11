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
            resultName = MasterModel.apiC.queryResult.ingredients[0].parsed[0].foodMatch;
            resultKcal = MasterModel.apiC.queryResult.ingredients[0].parsed[0].nutrients.ENERC_KCAL.quantity;
            resultCarb = MasterModel.apiC.queryResult.ingredients[0].parsed[0].nutrients.CHOCDF.quantity;
            resultProtein = MasterModel.apiC.queryResult.ingredients[0].parsed[0].nutrients.PROCNT.quantity;
            resultFat = MasterModel.apiC.queryResult.ingredients[0].parsed[0].nutrients.FAT.quantity;
            resultServing = MasterModel.apiC.queryResult.ingredients[0].parsed[0].weight;
        }
    }
}
