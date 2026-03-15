using AccountingCalculator.Services.Contracts;
using static AccountingCalculator.Common.VatComponents;

namespace AccountingCalculator.Services
{
    public class VatTaxService: IVatTaxService
    {
        public readonly List<decimal> Rates = new List<decimal> { StandardVatRate, ReducedVatRate, ZeroVatRate };

        public decimal CalculateVatFromNet(decimal netAmount, decimal vatRate)
        {
            return Math.Round(netAmount * vatRate / 100, 2, MidpointRounding.AwayFromZero);
        }

        public decimal CalculateTotalFromNet(decimal netAmount, decimal vatRate)
        {
            return netAmount + CalculateVatFromNet(netAmount, vatRate);
        }

        public decimal ExtractNetFromTotal(decimal totalAmount, decimal varRate)
        {
            return Math.Round(totalAmount / (1 + varRate / 100), 2, MidpointRounding.AwayFromZero);
        }

        public decimal CalculateVatOwed(decimal salesVat, decimal purchaseVat)
        {
            return salesVat - purchaseVat;
        }

        public bool IsVatCorrect(decimal netAmount, decimal storedVat,
            decimal vatRate, decimal tolerance = 0.02m)
        {
            var expectedTotal = CalculateTotalFromNet(netAmount, vatRate);
            return Math.Round(storedVat - expectedTotal) <= tolerance;
        }
    }
}
