using AccountingCalculator.Services;
using AccountingCalculator.Services.Contracts;
using static AccountingCalculator.Common.VatComponents;

namespace AccountingCalculator.Test
{
    [TestFixture]
    public class VatTaxServiceTest
    {
        private IVatTaxService vatTaxService = null!;

        [SetUp]
        public void Setup()
        {
            vatTaxService = new VatTaxService();
        }

        private static IEnumerable<TestCaseData> VatFromNetTestCases()
        {
            yield return new TestCaseData(100m, StandardVatRate, 20m).SetName("Standard rate (20%)");
            yield return new TestCaseData(100m, ReducedVatRate, 9m).SetName("Reduced rate (9%)");
            yield return new TestCaseData(100m, ZeroVatRate, 0m).SetName("Zero rate (0%)");
            yield return new TestCaseData(100.025m, StandardVatRate, 20.01m).SetName("Rounding with standard rate");
            yield return new TestCaseData(-100m, StandardVatRate, -20m).SetName("Negative amount");
        }

        [TestCaseSource(nameof(VatFromNetTestCases))]
        public void CalculateVatFromNet_ShouldCalculateCorrectly(decimal netAmount, decimal vatRate, decimal expectedVat)
        {
            decimal result = vatTaxService.CalculateVatFromNet(netAmount, vatRate);

            Assert.That(result, Is.EqualTo(expectedVat));
        }

        private static IEnumerable<TestCaseData> TotalFromNetTestCases()
        {
            yield return new TestCaseData(100m, StandardVatRate, 120m).SetName("Standard rate (20%)");
            yield return new TestCaseData(100m, ReducedVatRate, 109m).SetName("Reduced rate (9%)");
            yield return new TestCaseData(100m, ZeroVatRate, 100m).SetName("Zero rate (0%)");
            yield return new TestCaseData(100.025m, StandardVatRate, 120.04m).SetName("Rounding with standard rate");
        }

        [TestCaseSource(nameof(TotalFromNetTestCases))]
        public void CalculateTotalFromNet_ShouldCalculateCorrectly(decimal netAmount, decimal vatRate, decimal expectedTotal)
        {
            decimal result = vatTaxService.CalculateTotalFromNet(netAmount, vatRate);

            Assert.That(result, Is.EqualTo(expectedTotal));
        }

        private static IEnumerable<TestCaseData> ExtractNetFromTotalTestCases()
        {
            yield return new TestCaseData(120m, StandardVatRate, 100m).SetName("Standard rate (20%)");
            yield return new TestCaseData(109m, ReducedVatRate, 100m).SetName("Reduced rate (9%)");
            yield return new TestCaseData(100m, ZeroVatRate, 100m).SetName("Zero rate (0%)");
            yield return new TestCaseData(120.04m, StandardVatRate, 100.04m).SetName("Rounding with standard rate");
        }

        [TestCaseSource(nameof(ExtractNetFromTotalTestCases))]
        public void ExtractNetFromTotal_ShouldCalculateCorrectly(decimal totalAmount, decimal vatRate, decimal expectedNet)
        {
            decimal result = vatTaxService.ExtractNetFromTotal(totalAmount, vatRate);

            Assert.That(result, Is.EqualTo(expectedNet));
        }

        [TestCase(200, 50, 150)]
        [TestCase(100, 150, -50)]
        [TestCase(0, 0, 0)]
        [TestCase(-10, 20, -30)]
        public void CalculateVatOwed_ShouldCalculateCorrectly(decimal salesVat, decimal purchaseVat, decimal expectedVatOwed)
        {
            decimal result = vatTaxService.CalculateVatOwed(salesVat, purchaseVat);

            Assert.That(result, Is.EqualTo(expectedVatOwed));
        }

        [Test]
        public void IsVatCorrect_ShouldReturnTrue_WhenStoredMatchesCalculatedTotalWithinTolerance()
        {
            decimal netAmount = 100m;
            decimal expectedStored = vatTaxService.CalculateTotalFromNet(netAmount, StandardVatRate);

            bool result = vatTaxService.IsVatCorrect(netAmount, expectedStored, StandardVatRate);

            Assert.That(result, Is.True);
        }

        [Test]
        public void IsVatCorrect_ShouldRespectToleranceBoundary()
        {
            decimal netAmount = 100m;
            decimal expectedStored = vatTaxService.CalculateTotalFromNet(netAmount, ReducedVatRate);

            bool exactlyOnBoundary = vatTaxService.IsVatCorrect(netAmount, expectedStored + 0.02m, ReducedVatRate);
            bool outsideBoundary = vatTaxService.IsVatCorrect(netAmount, expectedStored + 0.0201m, ReducedVatRate);

            Assert.That(exactlyOnBoundary, Is.True);
            Assert.That(outsideBoundary, Is.False);
        }

        [Test]
        public void IsVatCorrect_ShouldRevealVatVsTotalMismatch()
        {
            decimal netAmount = 100m;
            decimal storedVatOnly = 20m;

            bool result = vatTaxService.IsVatCorrect(netAmount, storedVatOnly, StandardVatRate);

            Assert.That(result, Is.False);
        }

        [Test]
        public void Rates_ShouldContainConfiguredRates()
        {
            VatTaxService service = new VatTaxService();

            Assert.That(service.Rates, Is.EquivalentTo(new[] { StandardVatRate, ReducedVatRate, ZeroVatRate }));
        }
    }
}
