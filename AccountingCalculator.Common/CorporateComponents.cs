namespace AccountingCalculator.Common
{
    public static class CorporateComponents
    {
        // Корпоративен данък от печалбата - 10%
        public const decimal CorporateTaxRate = 0.10m;

        //Мирко-предприятия данък за бизнес под 300 000лв. и до 10 служителя - 1%
        public const decimal MicroEnterpriseTaxRate = 0.01m;

        // Данък за дивиденти - 5%
        public const decimal DividendTaxRate = 0.05m;
    }
}
