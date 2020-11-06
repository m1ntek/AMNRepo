using System;
using System.Collections.Generic;
using System.Text;

namespace AMN.Model
{
    public class ExerciseLoadout
    {
        public string Key { get; set; }
        public string Name { get; set; }
        public int Sets { get; set; }
        public List<Exercise> Exercises { get; set; }
        public DateTime StartTime { get; set; }
        public string DateString { get; set; }
        public DateTime EndTime { get; set; }
        public string StartToEnd { get; set; }

        public ExerciseLoadout()
        {
            Exercises = new List<Exercise>();
        }
    }
}
