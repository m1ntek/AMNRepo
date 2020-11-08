using System;
using System.Collections.Generic;
using System.Text;

namespace AMN.Model
{
    public class Exercise
    {
        public string Key { get; set; }
        public string Name { get; set; }
        public List<ExerciseType> Types { get; set; }

        //Could not clarify if constructors to initiate variables
        //were allowed in models.
        public Exercise()
        {
            Types = new List<ExerciseType>();
        }
    }
}