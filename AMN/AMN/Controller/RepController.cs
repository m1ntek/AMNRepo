using AMN.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AMN.Controller
{
    public class RepController
    {
        public static List<Rep> SaveRepsFromEntries(Grid gridForm, string repeatableLblText)
        {
            List<Rep> reps = new List<Rep>();
            for (int i = 0; i < gridForm.Children.Count; i++)
            {
                if (gridForm.Children[i].GetType() == typeof(Label))
                {
                    //Have to save to variable first; can't access Label properties straight from the children reference
                    Label tempLbl = (Label)gridForm.Children[i];
                    if (tempLbl.Text == repeatableLblText)
                    {
                        //Have to save to variable first; can't access Entry properties straight from the children reference
                        Entry tempEntry = (Entry)gridForm.Children[i + 1];
                        if (MasterModel.vd.FormEntriesValid(new string[] { tempEntry.Text }) == false)
                        {
                            return null;
                        }
                        reps.Add(new Rep { Amount = tempEntry.Text });
                    }
                }
            }
            return reps;
        }

        public static async Task<List<Exercise>> PrepareRepSummariesAsync(List<Exercise> exercises)
        {
            foreach (var exercise in exercises)
            {
                foreach (var eTypes in exercise.Types)
                {
                    //Reset summary text
                    eTypes.Summary = null;

                    //If there is a type name, append it.
                    if (string.IsNullOrEmpty(eTypes.Name) == false)
                    {
                        eTypes.Summary += $"{eTypes.Name}: ";
                    }

                    //for loop here as I need to work with the index.
                    for (int i = 0; i < eTypes.Reps.Count; i++)
                    {
                        //Logic to append ">" between rep values as per client request
                        if (i == 0)
                        {
                            //Don't append ">" to the first value.
                            eTypes.Summary += eTypes.Reps[i].Amount;
                        }
                        else
                        {
                            eTypes.Summary += $">{eTypes.Reps[i].Amount}";
                        }
                    }
                }
            }

            return exercises;
        }

        public static async Task<Exercise> PrepareRepSummaryAsync(Exercise exercise)
        {
            foreach (var eTypes in exercise.Types)
            {
                //Reset summary text
                eTypes.Summary = null;

                //If there is a type name, append it.
                if (string.IsNullOrEmpty(eTypes.Name) == false)
                {
                    eTypes.Summary += $"{eTypes.Name}: ";
                }

                //for loop here as I need to work with the index.
                for (int i = 0; i < eTypes.Reps.Count; i++)
                {
                    //Logic to append ">" between rep values as per client request
                    if (i == 0)
                    {
                        //Don't append ">" to the first value.
                        eTypes.Summary += eTypes.Reps[i].Amount;
                    }
                    else
                    {
                        eTypes.Summary += $">{eTypes.Reps[i].Amount}";
                    }
                }
            }

            return exercise;
        }
    }
}
