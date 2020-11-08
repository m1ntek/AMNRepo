using System;
using System.Collections.Generic;
using System.Text;

namespace AMN.Model
{
    public class ExerciseType
    {
        public int Index { get; set; }
        public string Name { get; set; }
        public string Summary { get; set; }
        public List<Rep> Reps { get; set; }
        public string ExerciseKey { get; set; }
        public bool IsComparable { get; set; }

        public ExerciseType()
        {
            Name = "Reg";
            Reps = new List<Rep>();
            IsComparable = true;
        }
    }
}
