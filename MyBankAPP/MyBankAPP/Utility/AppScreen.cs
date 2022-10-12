using MyBankAPP.Domain.Entity;
using MyBankAPP.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyBankAPP.Utility
{
    public class AppScreen
    {
        internal const string cur = " N ";
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


        internal static int SelectAmount()
        {
            Console.WriteLine("");
            Console.WriteLine(":1.{0}500           5.{0}10,000", cur);
            Console.WriteLine(":2.{0}1000           6.{0}15,000", cur);
            Console.WriteLine(":3.{0}2000           7.{0}20,000", cur);
            Console.WriteLine(":4.{0}5000           8.{0}40,000", cur);
            Console.WriteLine(":0.Other");
            Console.WriteLine("");

            int selectedAmount = Validator.Convert<int>("Options");
            switch (selectedAmount)
            {
                case 1:
                    return 500;
                    break;

                case 2:
                    return 1000;
                    break;
                case 3:
                    return 2000;
                    break;
                case 4:
                    return 5000;
                    break;
                case 5:
                    return 10000;
                    break;
                case 6:
                    return 15000;
                    break;
                case 7:
                    return 20000;
                    break;
                case 8:
                    return 40000;
                    break;
                case 0:
                    return 0;
                    break;
                default:
                    Utility.PrintMessage("Invalid input. try again", false);
                    //SelectAmount();
                    return -1;
                    break;

            }

        }
        internal InternalTransfer InternalTransferForm()
        {
            var interTransfer = new InternalTransfer();
            interTransfer.RecieoentBankAccountNumber = Validator.Convert<long>("recipient's account number: ");
            interTransfer.TransferAmount = Validator.Convert<decimal>($"amount{cur}");
            interTransfer.RecieoentBankAccountName = Utility.GetUserInput("recipient's name:");
            return interTransfer;
        }
    }

}