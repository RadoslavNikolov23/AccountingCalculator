using AccountingCalculator.DataModels;

namespace AccountingCalculator.Services.Contracts
{
    public interface ICorporateTaxService
    {
        public CorporateTaxResult Calculate(decimal revenue, decimal expenses, bool isMicroEnterprise = false);

        public DividendResult CalculateDividendTax(decimal netProfit);

        public EffectiveTaxResult CalculateEffectiveTax(decimal revenue, decimal expenses, bool isMicroEnterprise = false);
        
    }
}
