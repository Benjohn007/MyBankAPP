using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyBankAPP.Entity;
using MyBankAPP.Utility;

namespace MyBankAPP.App
{
    public static class Entry
    {

        static void Main(string[] args)
        {

            BankAPP bank = new BankAPP();
            bank.InitilizeData();
            Utility.Utility.PressEnterToContinue();
            bank.Run();
        }

    }
}
