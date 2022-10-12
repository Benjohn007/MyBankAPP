using System;
using System.Runtime.Intrinsics.Arm;
using System.Collections.Generic;
using MyBankAPP.Domain.Interfaces;
using System.Threading;
using MyBankAPP.Utility;
using MyBankAPP.Domain.Enums;
using MyBankAPP.Entity;
using MyBankAPP.Domain.Entity;
using System.Linq;
using ConsoleTables;

namespace MyBankAPP.App
{
    internal class BankAPP : IUserLogin, IUserAccount, ITransaction
    {
        private List<UserAccount> UserAccountList;
        private UserAccount selectedAccount;
        private List<Transaction> _listOfTransaction;
        private const decimal minimumKeptAmount = 500;
        private readonly AppScreen screen;

        public BankAPP()
        {
            screen = new AppScreen();
        }

        public void Run()
        {
            AppScreen.Welcome();
            CheckUserCardNumberAndPassword();
            AppScreen.WelcomeCustomer(selectedAccount.FullName);
           
            while (true)
            {
                Utility.Utility.PressEnterToContinue();
                AppScreen.DisplayMenu();
                MenuOption();
            }
         
        }

        public void InitilizeData()
        {
            UserAccountList = new List<UserAccount>()
            {
               // new UserAccount{Id, FullName, AccountNumber,CardNumber, CardPin, AccountBalance, IsLocked}
                new UserAccount{Id=2, FullName = "Akorede Kikelomo", AccountNumber =560987,CardNumber =123111, CardPin=111111, AccountBalance=7000.00m, IsLocked=false},
                new UserAccount{Id=3, FullName = "Benjamin John", AccountNumber =223345,CardNumber =123000, CardPin=000419, AccountBalance=5000.00m, IsLocked=false},
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
                    var internalTransfer = screen.InternalTransferForm();
                    ProcessInternalTransfer(internalTransfer);
                    break;

                case (int)MenuApp.ViewTransaction:
                    viewTransaction();
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
                MakeWithdrawal();
               // selectedAmount = AppScreen.SelectAmount();
               return;
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
           var filteredTransactionList = _listOfTransaction.Where(t => t.UserBankAccountId == selectedAccount.Id).ToList();
            //check if there's a transaction
            if(filteredTransactionList.Count <= 0)
            {
                Utility.Utility.PrintMessage("You have no trAnsaction yet.", true);
            }
            else
            {
                var table = new ConsoleTable("Id", "Transaction Date", "Type", "Descriptions", "Amount" + AppScreen.cur);
                foreach(var tran in filteredTransactionList)
                {
                    table.AddRow(tran.TransactionId,tran.TransactionDate, tran.TransactionType,tran.Description,tran.TransactionAmount);
                }
                table.Options.EnableCount = false;
                table.Write();
                Utility.Utility.PrintMessage($"You have {filteredTransactionList.Count} transaction(s)", true);

            }
        }

        private void ProcessInternalTransfer(InternalTransfer internalTransfer)
        {
            if (internalTransfer.TransferAmount <= 0)
            {
                Utility.Utility.PrintMessage("Amount needs to be greater than zero. Try again.", false);
            }
            // check sender's account balance
            if (internalTransfer.TransferAmount> selectedAccount.AccountBalance)
            {
                Utility.Utility.PrintMessage($"Transfer failed. you do not have enough balance" +
                    $"to transfer {Utility.Utility.FormatAmount(internalTransfer.TransferAmount)} ", false);
                return;
            }
            //check the minimum kept amount 
            if ((selectedAccount.AccountBalance - minimumKeptAmount) < minimumKeptAmount)
            {
                Utility.Utility.PrintMessage($"Transfer failed. Your account needs to habe a minimum of " +
                    $"{Utility.Utility.FormatAmount(minimumKeptAmount)}", false);
                return;
            }
            //check reciever's account number is valid
            var selectedBankAccoubtReciever = (from userAcc in UserAccountList
                                               where userAcc.AccountNumber == internalTransfer.RecieoentBankAccountNumber
                                               select userAcc).FirstOrDefault();

            if (selectedBankAccoubtReciever == null)
            {
                Utility.Utility.PrintMessage("Transfer failed. Reciever bank account number is invalid.",false);
                return;
            }

            //check reciever's name
            if (selectedBankAccoubtReciever.FullName != internalTransfer.RecieoentBankAccountName)
            {
                Utility.Utility.PrintMessage("Transfer failed. Recipient's bank account name does not match.", false);
                return;
            }
            //add transaction to transaction record - sender
            InsertTransaction(selectedAccount.Id, TransactionType.Transfer, -internalTransfer.TransferAmount, "Transfered" +
                $"to {selectedBankAccoubtReciever.AccountNumber}({selectedBankAccoubtReciever.FullName})");
            // update sender's account balance
            selectedAccount.AccountBalance -= internalTransfer.TransferAmount;

            //add transaction record-reciver
            InsertTransaction(selectedBankAccoubtReciever.Id, TransactionType.Transfer, internalTransfer.TransferAmount, "Transfered from" +
                $"{selectedAccount.AccountBalance}({selectedAccount.FullName})");

            //update reciever's account balance
            selectedBankAccoubtReciever.AccountBalance +=internalTransfer.TransferAmount;

            //print success message
            Utility.Utility.PrintMessage($"You have successfully transfered" +
                $"{Utility.Utility.FormatAmount(internalTransfer.TransferAmount)} to" +
                $"{internalTransfer.RecieoentBankAccountName}",true);

        }
    }
}
