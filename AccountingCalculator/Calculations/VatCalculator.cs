namespace AccountingCalculator.Calculations
{
    public class VatCalculator
    {
        public const decimal StandardVatRate = 0.20m;
        public const decimal ReducedVatRate = 9m;
        public const decimal ZeroVatRate = 0m;

        public static readonly List<decimal> Rates = new List<decimal>{ StandardVatRate, ReducedVatRate, ZeroVatRate };
    
        public static decimal CalculateVatFromNet(decimal netAmount, decimal vatRate)
        { 
            return Math.Round(netAmount * vatRate/100, 2, MidpointRounding.AwayFromZero);
        }

        public static decimal CalculateTotalFromNet(decimal netAmount, decimal vatRate)
        {
            return netAmount + CalculateVatFromNet(netAmount, vatRate);
        }

        public static decimal ExtractNetFromTotal(decimal totalAmount, decimal varRate)
        {
            if(ReducedVatRate ==0)
                return totalAmount;

            return Math.Round(totalAmount / (1 + varRate / 100), 2, MidpointRounding.AwayFromZero);
        }

        public static decimal CalculateVatOwed(decimal salesVat, decimal purchaseVat)
        {
            return salesVat-purchaseVat;
        }

        public static bool IsVatCorrect(decimal netAmount, decimal storedVat,
            decimal vatRate, decimal tolerance = 0.02m)
        {
            var expectedTotal = CalculateTotalFromNet(netAmount, vatRate);
            return Math.Round(storedVat - expectedTotal) <= tolerance;
        }

    }
}
