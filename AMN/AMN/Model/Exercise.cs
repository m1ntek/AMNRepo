using System;
using System.Collections.Generic;
using System.Text;

namespace AMN.Model
{
    public class Exercise
    {
        public string Name { get; set; }
        public List<Workout> WorkoutTypes { get; set; }

        public Exercise()
        {
            WorkoutTypes = new List<Workout>();
        }
    }
}
