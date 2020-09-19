using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.Net;
using AMN.Model;
using System.Threading.Tasks;

namespace AMN.Controller
{
    public class APIController
    {
        private string key = "0f1491147e391def6399e62f6ef207f2";
        private string id = "9d06552c";
        public FoodJSON.Root queryResult;
        public async void Query(string search)
        {
            //string json = new WebClient().DownloadString($"https://api.nal.usda.gov/fdc/v1/foods/search?api_key={key}&query={ConvertSpaces(search)}");
            string json;
            using (var client = new WebClient())
            {
                  json = await client.DownloadStringTaskAsync(new Uri($"https://api.edamam.com/api/nutrition-data?app_id={id}&app_key={key}&ingr={ConvertSpaces(search)}"));

            }
            queryResult = JsonConvert.DeserializeObject<FoodJSON.Root>(json);
        }

        private string ConvertSpaces(string search)
        {
            string convertedString = null;

            string[] words = search.Split(' ');
            for (int i = 0; i < words.Length; i++)
            {
                if (i == 0)
                {
                    convertedString = $"1%20{words[i]}";
                }
                else
                {
                    convertedString += $"%20{words[i]}";
                }
            }

            return convertedString;
        }
    }
}
