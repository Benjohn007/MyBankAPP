using System;
using System.Runtime.Intrinsics.Arm;
using System.Collections.Generic;
using MyBankAPP.Domain.Interfaces;
using System.Threading;
using MyBankAPP.Utility;
using MyBankAPP.Domain.Enums;
using MyBankAPP.Entity;
using MyBankAPP.Domain.Entity;

namespace MyBankAPP.App
{
    internal class BankAPP : IUserLogin, IUserAccount, ITransaction
    {
        private List<UserAccount> UserAccountList;
        private UserAccount selectedAccount;
        private List<Transaction> _listOfTransaction;
        private const decimal minimumKeptAmount = 500;

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

            _listOfTransaction = new List<Transaction>();  
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
                    Deposit();
                    break;

                case (int)MenuApp.MakeWithdrawal:
                    MakeWithdrawal();
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
            Console.WriteLine("\nOnly multipls of 500 and 1000 are allowed\n");
            var transaction_amt = Validator.Convert<int>($"amount {AppScreen.cur}");

            // simulate counting

            Console.WriteLine("\nChecking and Counting bank notes");
            Utility.Utility.PrintDotAnimation();
            Console.WriteLine("");


            //some functions
            if (transaction_amt <= 0)
            {
                Console.WriteLine("amount needs to be greater than zeros.", false);
                return;
            }

            if (transaction_amt % 500 != 0)
            {
                Console.WriteLine($"Enter deposit amount in multiples of 500 or 100.", false);
                return;
            }

            if (PreviewBankNotesCount(transaction_amt) == false)
            {
                Console.WriteLine("You have cancelled your action.", false);
                return;
            }

            //bind transaction details to transaction object
            InsertTransaction(selectedAccount.Id, TransactionType.Deposit, transaction_amt, "");

            //update account balance
            selectedAccount.AccountBalance += transaction_amt;

            //print success message
            Utility.Utility.PrintMessage($"Your deposit of {Utility.Utility.FormatAmount(transaction_amt)} was " +
                $"successful.", true);
        }

        public void MakeWithdrawal()
        {
            var transaction_amt = 0;
            int selectedAmount = AppScreen.SelectAmount();

            if (selectedAmount == -1)
            {
                selectedAmount = AppScreen.SelectAmount();
            }
            else if (selectedAmount != 0)
            {
                transaction_amt = selectedAmount;
            }
            else
            {
                transaction_amt = Validator.Convert<int>($"amount {AppScreen.cur}");
            }

            //input validation
            if (transaction_amt <= 0)
            {
                Utility.Utility.PrintMessage("Amount must be greater than zero. try again", false);
                return;
            }
            if (transaction_amt % 500 != 0)
            {
                Utility.Utility.PrintMessage("You can only withdraw in multiples of 500 and 100. try again", false);
                return;

            }

            //Businee login
            if (transaction_amt > selectedAccount.AccountBalance)
            {
                Utility.Utility.PrintMessage($"Withdrawal failed.Your balance is too lower to withdraw " +
                    $"{Utility.Utility.FormatAmount(transaction_amt)}", false);
                return;
            }
            if ((selectedAccount.AccountBalance - transaction_amt) < minimumKeptAmount)
            {
                Utility.Utility.PrintMessage($"Withdrawal failed. your account need to have " +
                    $"minimum of {Utility.Utility.FormatAmount(minimumKeptAmount)}", false);
            }

            //Bind withdraw details with transaction object
            InsertTransaction(selectedAccount.Id, TransactionType.Withdrawal, -transaction_amt, "");

            //update account balance
            selectedAccount.AccountBalance -= transaction_amt;

            //success message
            Utility.Utility.PrintMessage($"Your have successfully withdrawn {Utility.Utility.FormatAmount(selectedAmount)}.", true);
        }
        private bool PreviewBankNotesCount(int amount)
        {
            int thousandNotesCount = amount / 1000;
            int FivehundredNotesCount = (amount % 1000) / 500;
            Console.WriteLine("\nSummary");
            Console.WriteLine("----------");
            Console.WriteLine($"{AppScreen.cur}1000 X {thousandNotesCount} = {1000 * thousandNotesCount}");
            Console.WriteLine($"{AppScreen.cur}500 X {FivehundredNotesCount} = {500 * FivehundredNotesCount}");
            Console.WriteLine($"Total amount: {Utility.Utility.FormatAmount(amount)}\n\n");

            int opt = Validator.Convert<int>("1 to Confirm : ");
            return opt.Equals(1);


        }

        public void InsertTransaction(long _UserBankAcountID, TransactionType _tranType, decimal _tranAmount, string _desc)
        {
            // create a new transaction object
            var transaction = new Transaction()
            {
                TransactionId = Utility.Utility.GetTransactionId(),
                UserBankAccountId = _UserBankAcountID,
                TransactionDate = DateTime.Now,
                TransactionType = _tranType,
                TransactionAmount = _tranAmount,
                Description = _desc
            };

            // add tramsaction object

            _listOfTransaction.Add(transaction);
        }

        public void viewTransaction()
        {
            throw new NotImplementedException();
        }
    }
}
