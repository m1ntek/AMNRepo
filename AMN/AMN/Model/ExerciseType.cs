using System;
using System.Collections.Generic;
using System.Text;

namespace AMN.Model
{
    public class ExerciseType
    {
        public string Name { get; set; }
        public List<string> Reps { get; set; }

        public ExerciseType()
        {
            Reps = new List<string>();
        }
    }
}
