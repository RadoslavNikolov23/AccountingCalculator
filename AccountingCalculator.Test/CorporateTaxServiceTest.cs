using AccountingCalculator.DataModels;
using AccountingCalculator.Services;
using AccountingCalculator.Services.Contracts;
using static AccountingCalculator.Common.CorporateComponents;

namespace AccountingCalculator.Test
{
    [TestFixture]
    public class CorporateTaxServiceTest
    {
        private ICorporateTaxService serviceCorporateTax;

        [SetUp]
        public void Setup()
        {
            serviceCorporateTax = new CorporateTaxService();
        }

        [Test]
        public void Calculate_ShouldReturnCorrectTax_ForPositiveProfit()
        {
            CorporateTaxResult result = serviceCorporateTax.Calculate(1000m, 400m);

            Assert.That(result.Revenue, Is.EqualTo(1000m));
            Assert.That(result.Expenses, Is.EqualTo(400m));
            Assert.That(result.Profit, Is.EqualTo(600m));
            Assert.That(result.TaxBase, Is.EqualTo(600m));
            Assert.That(result.TaxRate, Is.EqualTo(CorporateTaxRate));
            Assert.That(result.TaxOwed, Is.EqualTo(Math.Round(600m * CorporateTaxRate, 2)));
            Assert.That(result.NetProfit, Is.EqualTo(600m - Math.Round(600m * CorporateTaxRate, 2)));
            Assert.That(result.IsAtLoss, Is.False);
            Assert.That(result.IsMicroEnterprise, Is.False);
        }

        [Test]
        public void Calculate_ShouldReturnZeroTax_ForZeroOrNegativeProfit()
        {
            CorporateTaxResult result = serviceCorporateTax.Calculate(500m, 600m);

            Assert.That(result.Profit, Is.EqualTo(-100m));
            Assert.That(result.TaxBase, Is.EqualTo(0m));
            Assert.That(result.TaxOwed, Is.EqualTo(0m));
            Assert.That(result.NetProfit, Is.EqualTo(-100m));
            Assert.That(result.IsAtLoss, Is.True);
        }

        [Test]
        public void Calculate_ShouldUseMicroEnterpriseRate_WhenFlagIsTrue()
        {
            CorporateTaxResult result = serviceCorporateTax.Calculate(1000m, 400m, true);

            Assert.That(result.TaxRate, Is.EqualTo(MicroEnterpriseTaxRate));
            Assert.That(result.TaxOwed, Is.EqualTo(Math.Round(600m * MicroEnterpriseTaxRate, 2)));
            Assert.That(result.IsMicroEnterprise, Is.True);
        }

        [TestCase(500)]
        public void CalculateDividendTax_ShouldReturnCorrectValues(decimal netProfit)
        {
            DividendResult result = serviceCorporateTax.CalculateDividendTax(netProfit);

            Assert.That(result.GrossDividend, Is.EqualTo(netProfit));
            Assert.That(result.DividendTax, Is.EqualTo(Math.Round(netProfit * DividendTaxRate, 2)));
            Assert.That(result.NetDividend, Is.EqualTo(netProfit - Math.Round(netProfit * DividendTaxRate, 2)));
        }

        [TestCase(2000,500)]
        public void CalculateEffectiveTax_ShouldReturnCorrectEffectiveTaxRate(decimal revenue, decimal expenses)
        {
            EffectiveTaxResult result = serviceCorporateTax.CalculateEffectiveTax(revenue, expenses);

            decimal expectedProfit = revenue - expenses;
            decimal expectedCorporateTax = Math.Round(expectedProfit * CorporateTaxRate, 2);
            decimal expectedNetProfit = expectedProfit - expectedCorporateTax;
            decimal expectedDividendTax = Math.Round(expectedNetProfit * DividendTaxRate, 2);
            decimal expectedTotalTax = expectedCorporateTax + expectedDividendTax;
            decimal expectedEffectiveTaxRate = Math.Round(expectedTotalTax / expectedProfit * 100, 2);

            Assert.That(result.TotalTaxPaid, Is.EqualTo(expectedTotalTax));
            Assert.That(result.EffectiveTaxRate, Is.EqualTo(expectedEffectiveTaxRate));
            Assert.That(result.OwnerReceives, Is.EqualTo(expectedNetProfit - expectedDividendTax));
        }

        [TestCase(100,200)]
        public void CalculateEffectiveTax_ShouldHandleLosses(decimal revenue, decimal expenses)
        {
            EffectiveTaxResult result = serviceCorporateTax.CalculateEffectiveTax(revenue, expenses);

            Assert.That(result.TotalTaxPaid, Is.EqualTo(-5m));
            Assert.That(result.EffectiveTaxRate, Is.EqualTo(0m));
            Assert.That(result.OwnerReceives, Is.EqualTo(-revenue - result.TotalTaxPaid));
        }
    }
}
