using System;
using System.Collections.Generic;
using System.Text;
using BCrypt;
using BCrypt.Net;
using System.IO;

namespace AMN.Controller
{
    public class LoginController
    {
        public string UpdateFooter(string user)
        {
            return $"Logged in as {user}.";
        }

        public string SecurePw(string pw)
        {
            //*********************
            return BCrypt.Net.BCrypt.HashPassword(pw, 12);
        }

        public async void RememberMe()
        {
   
        }
    }
}
