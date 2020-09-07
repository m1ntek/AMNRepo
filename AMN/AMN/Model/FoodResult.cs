using AMN.Controller;
using System;
using System.Collections.Generic;
using System.Text;

namespace AMN.Model
{
    public class FoodResult
    {
        //public string resultName;
        //public double resultKcal;
        //public double resultCarb;
        //public double resultProtein;
        //public double resultFat;
        //public double resultServing;
        public object result;

        public FoodResult()
        {
            result = MasterController.apiC.queryResult.ingredients;

            //resultName = MasterController.apiC.queryResult.ingredients[i].parsed[0].food;
            //resultKcal = MasterController.apiC.queryResult.ingredients[i].parsed[0].nutrients.ENERC_KCAL.quantity;
            //resultCarb = MasterController.apiC.queryResult.ingredients[i].parsed[0].nutrients.CHOCDF.quantity;
            //resultProtein = MasterController.apiC.queryResult.ingredients[i].parsed[0].nutrients.PROCNT.quantity;
            //resultFat = MasterController.apiC.queryResult.ingredients[i].parsed[0].nutrients.FAT.quantity;
            //resultServing = MasterController.apiC.queryResult.ingredients[i].parsed[0].weight;
        }
    }
}
