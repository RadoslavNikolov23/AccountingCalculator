using System;
using System.Collections.Generic;
using System.Text;

namespace AccountingCalculator.DataModels
{
    public class CorporateTaxResult
    {
        public decimal Revenue { get; set; }
        public decimal Expenses { get; set; }
        public decimal Profit { get; set; }
        public decimal TaxBase { get; set; }
        public decimal TaxRate { get; set; }
        public decimal TaxOwed { get; set; }
        public decimal NetProfit { get; set; }
        public bool IsAtLoss { get; set; }
        public bool IsMicroEnterprise { get; set; }
    }
}
