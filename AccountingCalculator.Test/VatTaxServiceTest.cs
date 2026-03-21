using AccountingCalculator.Services;
using AccountingCalculator.Services.Contracts;
using static AccountingCalculator.Common.VatComponents;

namespace AccountingCalculator.Test
{
    [TestFixture]
    public class VatTaxServiceTest
    {
        private IVatTaxService vatTaxService = null!;

        private const double standardVatRate = (double)StandardVatRate;
        private const double reducedVatRate = (double)ReducedVatRate;
        private const double zeroVatRate = (double)ZeroVatRate;

        [SetUp]
        public void Setup()
        {
            vatTaxService = new VatTaxService();
        }

        [TestCase(100, standardVatRate, 20)]
        [TestCase(100, reducedVatRate, 9)]
        [TestCase(100, zeroVatRate, 0)]
        [TestCase(100.025, standardVatRate, 20.01)]
        [TestCase(-100, standardVatRate, -20)]
        public void CalculateVatFromNet_ShouldCalculateCorrectly(decimal netAmount, decimal vatRate, decimal expectedVat)
        {
            decimal result = this.vatTaxService.CalculateVatFromNet(netAmount, vatRate);

            Assert.That(result, Is.EqualTo(expectedVat).Within(0.01m));
        }

        [TestCase(100, standardVatRate, 120)]
        [TestCase(100, reducedVatRate, 109)]
        [TestCase(100, zeroVatRate, 100)]
        [TestCase(100.025, standardVatRate, 120.04)]
        public void CalculateTotalFromNet_ShouldCalculateCorrectly(decimal netAmount, decimal vatRate, decimal expectedTotal)
        {
            decimal result = this.vatTaxService.CalculateTotalFromNet(netAmount, vatRate);

            Assert.That(result, Is.EqualTo(expectedTotal).Within(0.01m));
        }

        [TestCase(120, standardVatRate, 100)]
        [TestCase(109, reducedVatRate, 100)]
        [TestCase(100, zeroVatRate, 100)]
        [TestCase(120.04, standardVatRate, 100.04)]
        public void ExtractNetFromTotal_ShouldCalculateCorrectly(decimal totalAmount, decimal vatRate, decimal expectedNet)
        {
            decimal result = this.vatTaxService.ExtractNetFromTotal(totalAmount, vatRate);

            Assert.That(result, Is.EqualTo(expectedNet).Within(0.01m));
        }

        [TestCase(200, 50, 150)]
        [TestCase(100, 150, -50)]
        [TestCase(0, 0, 0)]
        [TestCase(-10, 20, -30)]
        public void CalculateVatOwed_ShouldCalculateCorrectly(decimal salesVat, decimal purchaseVat, decimal expectedVatOwed)
        {
            decimal result = this.vatTaxService.CalculateVatOwed(salesVat, purchaseVat);

            Assert.That(result, Is.EqualTo(expectedVatOwed).Within(0.01m));
        }

        [Test]
        public void IsVatCorrect_ShouldReturnTrue_WhenStoredMatchesCalculatedTotalWithinTolerance()
        {
            decimal netAmount = 100m;
            decimal expectedStored = this.vatTaxService.CalculateTotalFromNet(netAmount, StandardVatRate);

            bool result = this.vatTaxService.IsVatCorrect(netAmount, expectedStored, StandardVatRate);

            Assert.That(result, Is.True);
        }

        [Test]
        public void IsVatCorrect_ShouldRespectToleranceBoundary()
        {
            decimal netAmount = 100m;
            decimal expectedStored = this.vatTaxService.CalculateTotalFromNet(netAmount, ReducedVatRate);

            bool exactlyOnBoundary = this.vatTaxService.IsVatCorrect(netAmount, expectedStored + 0.02m, ReducedVatRate);
            bool outsideBoundary = this.vatTaxService.IsVatCorrect(netAmount, expectedStored + 0.0201m, ReducedVatRate);

            Assert.That(exactlyOnBoundary, Is.True);
            Assert.That(outsideBoundary, Is.False);
        }

        [Test]
        public void IsVatCorrect_ShouldRevealVatVsTotalMismatch()
        {
            decimal netAmount = 100m;
            decimal storedVatOnly = 20m;

            bool result = this.vatTaxService.IsVatCorrect(netAmount, storedVatOnly, StandardVatRate);

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
