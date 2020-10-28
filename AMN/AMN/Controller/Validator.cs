using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace AMN.Controller
{
    public class Validator
    {
        public string error = null;

        public bool FormEntriesValid(string[] formEntries)
        {
            foreach (var item in formEntries)
            {
                if(string.IsNullOrEmpty(item) == true)
                {
                    error = "Please don't leave any entries blank.";
                    return false;
                }
            }
            return true;
        }

        public bool Signup(string em, string pw1, string pw2)
        {
            if (!string.IsNullOrEmpty(pw1) && !string.IsNullOrEmpty(pw2) && !string.IsNullOrEmpty(em))
            {
                if (pw1.Length > 7 || pw2.Length > 7)
                {
                    if (pw1 == pw2)
                    {
                        for (int i = 0; i < pw1.Length; i++)
                        {
                            if (char.IsDigit(pw1[i]) == true)
                            {
                                for (int j = 0; j < pw1.Length; j++)
                                {
                                    if (char.IsLetter(pw1[j]) == true)
                                    {
                                        return true;
                                    }
                                }
                                error = "The password must contain at least a digit and a letter.";
                                return false;
                            }
                        }
                        error = "The password must contain at least a digit and a letter.";
                        return false;
                    }
                    else
                    {
                        error = "The passwords did not match, please try again.";
                        return false;
                    }
                }
                else
                {
                    error = "Passwords must at least be 8 characters long.";
                }
            }
            else
            {
                error = "Please do not leave any fields blank.";
                return false;
            }
            return false;
        }
    }
}
