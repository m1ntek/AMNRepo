using System;
using System.Collections.Generic;
using System.Text;
using BCrypt;
using BCrypt.Net;
using System.IO;
using Xamarin.Essentials;

namespace AMN.Controller
{
    //This class was initially planned to do more things...
    //Now it just handles the footer.
    public class LoginController
    {
        public string UpdateFooter(string user)
        {
            return $"Logged in as {user}.";
        }
    }
}
