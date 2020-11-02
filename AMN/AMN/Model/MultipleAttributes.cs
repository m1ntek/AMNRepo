using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AMN.Model
{
    //A class that holds multiple variables for situations
    //where I want to return multiple variables from a method.
    public class MultipleAttributes
    {
        public Grid grid;
        public ExerciseType type;
        public int num;
        public Button btn;
        public string text;

        //Not completely sure if models can have constructors
        public MultipleAttributes(Grid _grid, ExerciseType _type, int _num, Button _btn, string _text)
        {
            grid = _grid;
            type = _type;
            num = _num;
            btn = _btn;
            text = _text;
        }
    }
}
