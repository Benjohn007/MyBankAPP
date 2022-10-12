using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBankAPP.Domain.Entity
{
    public class InternalTransfer
    {
        public decimal TransferAmount { get; set; }
        public long RecieoentBankAccountNumber { get; set; }
        public string RecieoentBankAccountName { get; set; }
    }
}
