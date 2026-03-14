namespace AccountingCalculator.Calculations
{
    public class CorporateTaxCalculator
    {
        // Корпоративен данък от печалбата - 10%
        public const decimal CorporateTaxRate = 0.10m;

        //Мирко-предприятия данък за бизнес под 300 000лв. и до 10 служителя - 1%
        public const decimal MicroEnterpriseTaxRate = 0.01m;

        // Данък за дивиденти - 5%
        public const decimal DividendTaxRate = 0.05m;

        public static CorporateTaxResult Calculate(decimal revenue, decimal expenses, bool isMicroEnterprise = false)
        {
            decimal profit = revenue - expenses;
            decimal taxBase = profit > 0 ? profit : 0;

            decimal rate = isMicroEnterprise ? MicroEnterpriseTaxRate : CorporateTaxRate;
            decimal taxOwed = Math.Round(taxBase * rate, 2, MidpointRounding.AwayFromZero);

            decimal netProfit = profit - taxOwed;

            return new CorporateTaxResult
            {
                Revenue = revenue,
                Expenses = expenses,
                Profit = profit,
                TaxBase = taxBase,
                TaxRate = rate,
                TaxOwed = taxOwed,
                NetProfit = netProfit,
                IsAtLoss = profit < 0,
                IsMicroEnterprise = isMicroEnterprise
            };
        }

        //Данък за дивиденти - изчислява се след заплащане на коорпоративния данък и се прилага върху разпределената печалба
        public static DividendResult CalculateDividendTax(decimal netProfit)
        {
            decimal dividendTax = Math.Round(netProfit * DividendTaxRate, 2, MidpointRounding.AwayFromZero);
            decimal netDividend = netProfit - dividendTax;

            return new DividendResult
            {
                GrossDividend = netProfit,
                DividendTax = dividendTax,
                NetDividend = netDividend
            };
        }


        // Общи данъци - Корпоративен данък + данък върху дивидентите
        // Показва какъв % от печалбата действително задържа собственикът
        public static EffectiveTaxResult CalculateEffectiveTax(decimal revenue, decimal expenses, bool isMicroEnterprise = false)
        {
            CorporateTaxResult corporateTaxResult = Calculate(revenue, expenses, isMicroEnterprise);
            DividendResult dividendResult = CalculateDividendTax(corporateTaxResult.NetProfit);
            decimal totalTax = corporateTaxResult.TaxOwed + dividendResult.DividendTax;
            decimal effectiveTaxRate = corporateTaxResult.Profit > 0 ? Math.Round(totalTax / corporateTaxResult.Profit * 100, 2) : 0;
           
            return new EffectiveTaxResult
            {
                Corporate = corporateTaxResult,
                Dividend = dividendResult,
                TotalTaxPaid = totalTax,
                EffectiveTaxRate = effectiveTaxRate,
                OwnerReceives = dividendResult.NetDividend
            };
        }

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

        public class DividendResult
        {
            public decimal GrossDividend { get; set; }
            public decimal DividendTax { get; set; }
            public decimal NetDividend { get; set; }
        }

        public class EffectiveTaxResult
        {
            public CorporateTaxResult Corporate { get; set; } = default!;
            public DividendResult Dividend { get; set; } = default!;
            public decimal TotalTaxPaid { get; set; }
            public decimal EffectiveTaxRate { get; set; }
            public decimal OwnerReceives { get; set; }
        }
    }
}
