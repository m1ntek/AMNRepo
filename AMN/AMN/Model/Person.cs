using System;
using System.Collections.Generic;
using System.Security;
using System.Text;

namespace AMN.Model
{
    public class Person
    {
        public string email;
        public bool rememberMe;

        public Person()
        {
            email = "";
            rememberMe = false;
        }
    }
}
