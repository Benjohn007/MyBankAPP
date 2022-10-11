using MyBankAPP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBankAPP.Domain.Entity
{
    public class Transaction
    {
        public long TransactionId { get; set; }
        public long UserBankAccountId { get; set; }
        public long transaction { get; set; }
        public DateTime TransactionDate { get; set; }
        public TransactionType TransactionType { get; set; }
        public string Description { get; set; }
        public decimal TransactionAmount { get; set; }
    }
}
