﻿using System;
using System.Collections.Generic;
using System.Text;

namespace AMN.Model
{
    public class Loadout
    {
        public int LoadoutId { get; set; }
        public List<Meal> Meals { get; set; }
        public string LoadoutName { get; set; }
        public Loadout()
        {
            Meals = new List<Meal>();
        }
    }
}
