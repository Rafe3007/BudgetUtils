using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetUtils.Models
{
    public class BankTransaction
    {
        public DateTime? TransactionDate { get; set; }
        public string? BankName { get; set; }
        public string? PaidTo { get; set; }
        public decimal? Amount { get; set; }
    }
}
