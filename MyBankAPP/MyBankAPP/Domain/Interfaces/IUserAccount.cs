using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBankAPP.Domain.Interfaces
{
    public interface IUserAccount
    {
        void CheckBalance();

        void Deposit();

        void MakeWithdrawal();
    }
}
