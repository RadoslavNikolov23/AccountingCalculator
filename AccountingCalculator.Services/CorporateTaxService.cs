using AccountingCalculator.DataModels;
using AccountingCalculator.Services.Contracts;
using static AccountingCalculator.Common.CorporateComponents;

namespace AccountingCalculator.Services
{
    public class CorporateTaxService: ICorporateTaxService
    {
        public CorporateTaxResult Calculate(decimal revenue, decimal expenses, bool isMicroEnterprise = false)
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
        public DividendResult CalculateDividendTax(decimal netProfit)
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
        public EffectiveTaxResult CalculateEffectiveTax(decimal revenue, decimal expenses, bool isMicroEnterprise = false)
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
    }
}
