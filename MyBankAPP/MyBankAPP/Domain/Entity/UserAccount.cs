using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBankAPP.Entity
{

    public class UserAccount
    {
        public int Id { get; set; }
        public long CardNumber { get; set; }
        public long AccountNumber { get; set; }
        public int CardPin { get; set; }
        public string FullName { get; set; }
        public decimal AccountBalance { get; set; }
        public int TotalLogin { get; set; }
        public bool IsLocked { get; set; }
    }

    /*List<UserAccount> userAccounts = new List<UserAccount>()
    {

    }*/
}
