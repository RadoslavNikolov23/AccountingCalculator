using System;
using System.Collections.Generic;
using System.Text;

namespace AccountingCalculator.DataModels
{
    public class EffectiveTaxResult
    {
        public CorporateTaxResult Corporate { get; set; } = default!;
        public DividendResult Dividend { get; set; } = default!;
        public decimal TotalTaxPaid { get; set; }
        public decimal EffectiveTaxRate { get; set; }
        public decimal OwnerReceives { get; set; }
    }
}
