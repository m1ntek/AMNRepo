using AMN.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMN.Controller
{
    public class WeeklyComparisons
    {
        private ExerciseLoadout exSession;
        private ExerciseLoadout lastWeeksSession;
        public WeeklyComparisons(ExerciseLoadout _exSession)
        {
            exSession = _exSession;
            lastWeeksSession = new ExerciseLoadout();
        }

        public async Task<ExerciseLoadout> CompareLastWeeksSession()
        {
            try
            {
                string lastWeeksDate = exSession.StartTime.Subtract(TimeSpan.FromDays(7)).ToString("dd/MM/yyyy");
                lastWeeksSession = await MasterModel.DAL.GetLastWeeksExerciseLoadoutAsync(lastWeeksDate);
                exSession = await SetDifferences();
            }
            catch (Exception) {/* Do nothing */ }

            return exSession;
        }

        public async Task<ExerciseLoadout> SetDifferences()
        {
            //within each exercise
            for (int i = 0; i < exSession.Exercises.Count; ++i)
            {
                //within each type of exercise
                for (int j = 0; j < exSession.Exercises[i].Types.Count; ++j)
                {
                    var exSessionType = exSession.Exercises[i].Types[j];

                    //In case a type was deleted during session log, we wrap the logic in try/catch
                    try
                    {
                        var lastWeekSessionType = lastWeeksSession.Exercises[i].Types[j];

                        //Easier to conceptualise
                        //A log may have more reps than the initial loadout
                        //so they are counted and identified which is larger.
                        //var orderedReps = new List<ExerciseType> { exSessionType, lastWeekSessionType }.OrderByDescending(x => x.Reps.Count);
                        //var highestCountRepsType = orderedReps.First();
                        //var lowestCountRepsType = orderedReps.Last();

                        //If we have uneven comparisons
                        if (exSessionType.Reps.Count != lastWeekSessionType.Reps.Count)
                        {
                            //exSessionType has more reps
                            if (exSessionType.Reps.Count > lastWeekSessionType.Reps.Count)
                            {
                                for (int k = 0; k < exSessionType.Reps.Count; ++k)
                                {
                                    //For when no comparison rep exists
                                    if (k >= lastWeekSessionType.Reps.Count)
                                    {
                                        //Show new rep
                                        exSessionType.Reps[k].AmountDifference = $"+{exSessionType.Reps[k].Amount}";
                                        exSessionType.Reps[k].amountColor = System.Drawing.Color.Green;
                                    }
                                    else
                                    {
                                        SetRepDifferences(i, j, exSessionType, lastWeekSessionType, k);
                                    }
                                }
                            }
                            //lastWeekSessionType has more reps
                            else
                            {
                                for (int k = 0; k < lastWeekSessionType.Reps.Count; ++k)
                                {
                                    //For when no comparison rep exists
                                    if (k >= exSessionType.Reps.Count)
                                    {
                                        //Show new rep
                                        //exSessionType.Reps[k].AmountDifference = $"+ {exSessionType.Reps[k].Amount}";
                                        exSessionType.Reps.Add(new Rep { AmountDifference = $"-{lastWeekSessionType.Reps[k].Amount}" });
                                        
                                        //set colour
                                        exSessionType.Reps[k].amountColor = System.Drawing.Color.Red;
                                    }
                                    else
                                    {
                                        SetRepDifferences(i, j, exSessionType, lastWeekSessionType, k);
                                    }
                                }
                            }

                        }
                        else //rep counts are even so just do normal logic
                        {
                            for (int k = 0; k < exSessionType.Reps.Count; ++k)
                            {
                                SetRepDifferences(i, j, exSessionType, lastWeekSessionType, k);
                            }
                        }
                    }
                    catch (Exception) { /* Do nothing */ }
                }
            }
            return exSession;
        }

        private void SetRepDifferences(int i, int j, ExerciseType exSessionType, ExerciseType lastWeekSessionType, int k)
        {
            string[] repDigits = new string[2], lastWeekRepDigits = new string[2];
            int repValue = 0, lastWeekRepValue = 0;
            string thisRepAmount = exSessionType.Reps[k].Amount;
            string lastWeekRepAmount = lastWeekSessionType.Reps[k].Amount;
            bool thisRepHasSymbols = false, lastWeeksRepHasSymbols = false;

            //Just grab the digits, incase there are symbols.
            foreach (var character in thisRepAmount)
            {
                if (char.IsDigit(character) == false)
                {
                    repDigits = thisRepAmount.Split(character);
                    thisRepHasSymbols = true;
                }
            }
            if (thisRepHasSymbols == true) repValue = Convert.ToInt32(repDigits[0]);
            else repValue = Convert.ToInt32(thisRepAmount);

            //Just grab the digits, incase there are symbols, for last week's session.
            foreach (var character in lastWeekRepAmount)
            {
                if (char.IsDigit(character) == false)
                {
                    lastWeekRepDigits = lastWeekRepAmount.Split(character);
                    lastWeeksRepHasSymbols = true;
                }
            }
            lastWeekRepValue = Convert.ToInt32(lastWeekRepDigits[0]);
            if (lastWeeksRepHasSymbols == true) repValue = Convert.ToInt32(repDigits[0]);
            else lastWeekRepValue = Convert.ToInt32(lastWeekRepAmount);

            //amount difference
            int value = repValue - lastWeekRepValue;

            //set colours based on value
            SetRepColor(exSessionType, k, value);

            if (value > 0) exSessionType.Reps[k].AmountDifference = $"+{value}";
            else exSessionType.Reps[k].AmountDifference = $"{value}";


            //save back
            exSession.Exercises[i].Types[j] = exSessionType;
        }

        private static void SetRepColor(ExerciseType exSessionType, int k, int value)
        {
            if (value > 0) exSessionType.Reps[k].amountColor = System.Drawing.Color.Green;
            if (value < 0) exSessionType.Reps[k].amountColor = System.Drawing.Color.Red;
            if (value == 0) exSessionType.Reps[k].amountColor = System.Drawing.Color.Yellow;
        }
    }
}

