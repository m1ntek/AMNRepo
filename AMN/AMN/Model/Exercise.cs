using System;
using System.Collections.Generic;
using System.Text;

namespace AMN.Model
{
    public class Exercise
    {
        public string Key { get; set; }
        public string Name { get; set; }
        //public List<Workout> WorkoutTypes { get; set; }
        public List<ExerciseType> Types { get; set; }

        public Exercise()
        {
            //WorkoutTypes = new List<Workout>();
            Types = new List<ExerciseType>();
        }
    }
}