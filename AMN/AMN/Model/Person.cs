using System;
using System.Collections.Generic;
using System.Security;
using System.Text;

namespace AMN.Model
{
    //Was actually going to name this User but the name had a clash
    //with maybe one of the library's names.
    public class Person
    {
        public string email;
        public bool rememberMe;
        public MacroNutrients DailyGoal;
        public MacroNutrients GoalProgress;
        public List<Meal> Meals;
        public Loadout SelectedLoadout;
        public List<Meal> TempLoadoutMeals;
        public List<Loadout> Loadouts;
        public ExerciseLoadout SelectedExerciseLoadout;
        public List<ExerciseLoadout> ExerciseLoadouts;
        public List<Exercise> SavedExercises;

        public Person()
        {
            email = "";
            rememberMe = false;
            DailyGoal = new MacroNutrients();
            GoalProgress = new MacroNutrients();
            Meals = new List<Meal>();
            Loadouts = new List<Loadout>();
            TempLoadoutMeals = new List<Meal>();
            SelectedLoadout = new Loadout();
            ExerciseLoadouts = new List<ExerciseLoadout>();
            SavedExercises = new List<Exercise>();
            SelectedExerciseLoadout = new ExerciseLoadout();
        }
    }
}
