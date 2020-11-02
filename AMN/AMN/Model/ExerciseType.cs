using System;
using System.Collections.Generic;
using System.Text;

namespace AMN.Model
{
    public class ExerciseType
    {
        public string Name { get; set; }
        public string Summary { get; set; }
        public List<Rep> Reps { get; set; }

        public ExerciseType()
        {
            Reps = new List<Rep>();
        }
    }
}
