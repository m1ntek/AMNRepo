using AMN.Model;
using System;
using System.Collections.Generic;
using System.Text;
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
                    Label tempLbl = (Label)gridForm.Children[i];
                    if (tempLbl.Text == repeatableLblText)
                    {

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
    }
}
