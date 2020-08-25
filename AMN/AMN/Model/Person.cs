using System;
using System.Collections.Generic;
using System.Security;
using System.Text;

namespace AMN.Model
{
    public class Person
    {
        public string email;
        public string passwordHash;
        public bool rememberMe;

        public Person()
        {
            email = "";
            passwordHash = "";
            rememberMe = false;
        }
    }
}
