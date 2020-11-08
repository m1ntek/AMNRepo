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
            //had to implement this to fix a bug... made it a little messy
            try
            {
                var apiQueryResult = MasterModel.apiC.queryResult.ingredients[0].parsed[0];

                resultName = apiQueryResult.foodMatch;
                resultKcal = apiQueryResult.nutrients.ENERC_KCAL.quantity;
                resultCarb = apiQueryResult.nutrients.CHOCDF.quantity;
                resultProtein = apiQueryResult.nutrients.PROCNT.quantity;
                resultFat = apiQueryResult.nutrients.FAT.quantity;
                resultServing = apiQueryResult.weight;
            }
            catch (Exception)
            {
                resultName = "";
                resultKcal = resultCarb = resultProtein = resultFat = resultServing = 0;
            }
        }
    }
}
