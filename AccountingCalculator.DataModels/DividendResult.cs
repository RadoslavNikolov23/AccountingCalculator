using System;
using System.Collections.Generic;
using System.Text;

namespace AccountingCalculator.DataModels
{
    public class DividendResult
    {
        public decimal GrossDividend { get; set; }
        public decimal DividendTax { get; set; }
        public decimal NetDividend { get; set; }
    }
}
