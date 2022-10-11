using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBankAPP.Utility
{
    public static class Validator
    {
        public static T Convert<T>(string prompt)
        {
            bool Valid = false;
            string UserInput;

            while (!Valid)
            {
                UserInput = Utility.GetUserInput(prompt);

                try
                {
                    var converter = TypeDescriptor.GetConverter(typeof(T));
                    if (converter != null)
                    {
                        return (T)converter.ConvertFromString(UserInput);
                    }
                    else
                    {
                        return default;
                    }
                }
                catch
                {
                    Utility.PrintMessage("Invalid input, Try again", false);
                }
            }
            return default;
        }
    }
}
