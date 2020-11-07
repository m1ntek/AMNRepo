using System;
using System.Collections.Generic;
using System.Text;

namespace AMN.Model
{
    public class Rep
    {
        public System.Drawing.Color amountColor { get; set; }
        public System.Drawing.Color weightColor { get; set; }
        public string ExerciseKey { get; set; }
        public int ExerciseTypeIndex { get; set; }
        public int Index { get; set; }
        public string Amount { get; set; }
        public string AmountDifference { get; set; }
        public double Weight { get; set; }
        public string WeightDifference { get; set; }
    }
}
