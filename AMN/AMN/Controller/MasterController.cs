using AMN.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace AMN.Controller
{
    public static class MasterController
    {
        public static DataAccessLayer DAL = new DataAccessLayer();
        public static LoginController loginC = new LoginController();
        public static Validator vd = new Validator();
        public static Person currentUser = new Person();
    }
}
