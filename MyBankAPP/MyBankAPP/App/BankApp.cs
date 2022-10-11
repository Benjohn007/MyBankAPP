using System;
using System.Runtime.Intrinsics.Arm;
using System.Collections.Generic;
using MyBankAPP.Domain.Interfaces;
using System.Threading;
using MyBankAPP.Utility;
using MyBankAPP.Domain.Enums;
using MyBankAPP.Entity;
using System.Transactions;

namespace MyBankAPP.App
{
    internal class BankAPP : IUserLogin, IUserAccount, ITransaction
    {
        private List<UserAccount> UserAccountList;
        private UserAccount selectedAccount;

        public void Run()
        {
            AppScreen.Welcome();
            CheckUserCardNumberAndPassword();
            AppScreen.WelcomeCustomer(selectedAccount.FullName);
            Utility.Utility.PressEnterToContinue();
            AppScreen.DisplayMenu();
            MenuOption();
        }

        public void InitilizeData()
        {
            UserAccountList = new List<UserAccount>()
            {
               // new UserAccount{Id, FullName, AccountNumber,CardNumber, CardPin, AccountBalance, IsLocked}
                new UserAccount{Id=2, FullName = "Akorede Kikelomo", AccountNumber =560987,CardNumber =123111, CardPin=111111, AccountBalance=7000.00m, IsLocked=false},
                new UserAccount{Id=3, FullName = "Benjamin John", AccountNumber =223345,CardNumber =123000, CardPin=000419, AccountBalance=5000.00m, IsLocked=true},
           };
        }


        public void CheckUserCardNumberAndPassword()
        {
            bool isCorrectLogin = false;

            while (isCorrectLogin == false)
            {
                UserAccount inputAccount = AppScreen.UserLoginForm();
                AppScreen.LoginProgress();
                foreach (UserAccount account in UserAccountList)
                {
                    selectedAccount = account;
                    if (inputAccount.CardNumber.Equals(selectedAccount.CardNumber))
                    {
                        selectedAccount.TotalLogin++;
                        // Console.WriteLine("wromhjhghjhg");

                        if (inputAccount.CardPin.Equals(selectedAccount.CardPin))
                        {
                            selectedAccount = account;

                            if (selectedAccount.IsLocked || selectedAccount.TotalLogin > 3)
                            {
                                AppScreen.LoginProgress();
                                //isCorrectLogin = true;                               
                            }
                            else
                            {
                                selectedAccount.TotalLogin = 0;
                                isCorrectLogin = true;
                                // return;
                            }
                        }

                    }
                    if (isCorrectLogin == false)
                    {
                        Utility.Utility.PrintMessage("\nInvalid card number or PIN.", false);
                        selectedAccount.IsLocked = selectedAccount.TotalLogin == 3;
                        if (selectedAccount.IsLocked)
                        {
                            AppScreen.PrintLockScreen();
                        }
                    }
                    Console.Clear();

                }

            }



        }



        private void MenuOption()
        {
            switch (Validator.Convert<int>("an Option: "))
            {
                case (int)MenuApp.CheckBalancce:
                    CheckBalance();
                    break;

                case (int)MenuApp.Deposit:
                    Console.WriteLine("Placing Deposit...");
                    break;

                case (int)MenuApp.MakeWithdrawal:
                    Console.WriteLine("Making Withdrawal...");
                    break;

                case (int)MenuApp.InTransfer:
                    Console.WriteLine("Making Transfer...");
                    break;

                case (int)MenuApp.ViewTransaction:
                    Console.WriteLine("View Transaction...");
                    break;

                case (int)MenuApp.Logout:
                    AppScreen.LogoutProcess();
                    Utility.Utility.PrintMessage("You have Successfully logged out.");
                    Run();
                    break;

                default:
                    Utility.Utility.PrintMessage("Invalid Option.", false);
                    break;
            }
        }

        public void CheckBalance()
        {
            Utility.Utility.PrintMessage($"Your Account is :    {Utility.Utility.FormatAmount(selectedAccount.AccountBalance)}");
        }

        public void Deposit()
        {
            throw new NotImplementedException();
        }

        public void MakeWithdrawal()
        {
            throw new NotImplementedException();
        }

        public void InsertTransaction(long _UserBankAcountID_, TransactionType _tranType_, decimal _tranAmount_, string _desc)
        {
            // create a new transaction object
            var transaction = new Transaction()
            {
                TransactionId = Utility.Utility.GetTransactionId(),
           }
        }

        public void viewTransaction()
        {
            throw new NotImplementedException();
        }
    }
}
