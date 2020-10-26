using System;
using System.Collections.Generic;
using System.Text;

namespace AMN.Model
{
    public class ExerciseLoadout
    {
        public string Name { get; set; }
        public int Sets { get; set; }
        public List<Exercise> Exercises { get; set; }

        public ExerciseLoadout()
        {
            Exercises = new List<Exercise>();
        }
    }
}
