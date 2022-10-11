using MyBankAPP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBankAPP.Domain.Interfaces
{
    internal interface ITransaction
    {
        void InsertTransaction (long _UserBankAcountID_,  TransactionType _tranType_, decimal _tranAmount_, string _desc);
        void viewTransaction();
    }
}
