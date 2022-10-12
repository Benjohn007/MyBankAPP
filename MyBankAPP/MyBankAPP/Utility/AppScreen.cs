using MyBankAPP.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyBankAPP.Utility
{
    public static class AppScreen
    {
        internal const string cur = "N ";
        public static void Welcome()
        {
            // clear the console screen
            Console.Clear();

            //set the title of the console window
            Console.Title = "BenTrust Bank";

            // set the test color
            Console.ForegroundColor = ConsoleColor.Green;

            //welcome message
            Console.WriteLine("*********************************Welcome To BenTrust Bank*********************************");

        }

        internal static UserAccount UserLoginForm()
        {
            UserAccount tempUserAccount = new UserAccount();

            tempUserAccount.CardNumber = Validator.Convert<long>("Your card number:");
            tempUserAccount.CardPin = Convert.ToInt32(Utility.GetSecretInput("Enter your card Pin"));
            return tempUserAccount;
        }

        internal static void LoginProgress()
        {
            Console.Write("\nChecking card number and PIN...");
            Utility.PrintDotAnimation();
        }

        internal static void PrintLockScreen()
        {
            Console.Clear();
            Utility.PrintMessage("Your accont is locked. Please go to the nearest " +
                "branch to unlock your account. Thank you.", true);
            Utility.PressEnterToContinue();
            Environment.Exit(1);
        }

        internal static void WelcomeCustomer(string fullName)
        {
            Console.WriteLine($"Welcome back, {fullName}");
        }

        internal static void DisplayMenu()
        {
            Console.Clear();
            Console.WriteLine("----------------My Menu----------------");
            Console.WriteLine(":                                     :");
            Console.WriteLine("1. Account Balance                    :");
            Console.WriteLine("2. Deposit cash                       :");
            Console.WriteLine("3. Withdraw cash                      :");
            Console.WriteLine("4. Transfer                           :");
            Console.WriteLine("5. Transaction                        :");
            Console.WriteLine("6. Logout                             :");

        }

        internal static void LogoutProcess()
        {
            Console.WriteLine("Thank you for Banking with us.");
            Utility.PrintDotAnimation();
            Console.Clear();
        }
    }
}