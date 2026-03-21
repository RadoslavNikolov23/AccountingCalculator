using AccountingCalculator.DataModels;
using AccountingCalculator.Services;
using AccountingCalculator.Services.Contracts;
using static AccountingCalculator.Common.SalaryComponents;

namespace AccountingCalculator.Test
{
    [TestFixture]
    public class SalaryCalculateServiceTest
    {
        private ISalaryCalculateService salaryCalculateService = null!;

        private const double minimumInsurableIncome = (double)MinInsurableIncome;
        private const double maximumInsurableIncome = (double)MaxInsurableIncome;

        [SetUp]
        public void Setup()
        {
            salaryCalculateService = new SalaryCalculateService();
        }

        [TestCase(1000)]
        [TestCase(1200)]
        [TestCase(1500)]
        [TestCase(500)]   // Below minimum insurable income
        [TestCase(5000)]  // Above maximum insurable income
        public void Calculate_ShouldKeepInternalTotalsConsistent(decimal grossSalary)
        {
            SalaryBreakdown result = this.salaryCalculateService.Calculate(grossSalary);

            Assert.Multiple(() =>
            {
                Assert.That(result.TotalEmployeeContributions,
                    Is.EqualTo(result.EmployeeDOO + result.EmployeeDZPO + result.EmployeeZO));

                Assert.That(result.TotalEmployerContributions,
                    Is.EqualTo(result.EmployerDOO + result.EmployerDZPO + result.EmployerZO + result.EmployerAccident));

                Assert.That(result.NetSalary,
                    Is.EqualTo(result.GrossSalary - result.TotalEmployeeContributions - result.IncomeTax));

                Assert.That(result.TotalCostToEmployer,
                    Is.EqualTo(result.GrossSalary + result.TotalEmployerContributions));
            });
        }

        [TestCase(500,minimumInsurableIncome)] //Below minimum insurable income
        [TestCase(1000,1000)] //Within range
        [TestCase(5000,maximumInsurableIncome)] //Above maximum insurable income
        public void Calculate_ShouldClampInsurableIncome(decimal grossSalary, decimal expectedInsurableIncome)
        {
            SalaryBreakdown result = this.salaryCalculateService.Calculate(grossSalary);

            Assert.That(result.InsurableIncome, Is.EqualTo(expectedInsurableIncome));
        }

        [TestCase(1000)]
        [TestCase(1234.56)]
        [TestCase(5000)]
        public void Calculate_ShouldApplyIncomeTaxOnFlooredTaxableIncome(decimal grossSalary)
        {
            SalaryBreakdown result = this.salaryCalculateService.Calculate(grossSalary);
            decimal expectedTax = Math.Round(Math.Floor(result.TaxableIncome) * IncomeTaxRate, 2, MidpointRounding.AwayFromZero);

            Assert.That(result.IncomeTax, Is.EqualTo(expectedTax));
        }

        [TestCase(1876.10, 1400.16, 2322.98)]
        [TestCase(2500.00, 1865.70, 3095.50)]
        public void Calculate_ShouldMatchGoldenCases(decimal grossSalary, decimal expectedNetSalary, decimal expectedTotalCostToEmployer)
        {
            SalaryBreakdown result = this.salaryCalculateService.Calculate(grossSalary);

            Assert.Multiple(() =>
            {
                Assert.That(result.GrossSalary, Is.EqualTo(grossSalary));
                Assert.That(result.NetSalary, Is.EqualTo(expectedNetSalary).Within(0.01m));
                Assert.That(result.TotalCostToEmployer, Is.EqualTo(expectedTotalCostToEmployer).Within(0.01m));
            });
        }

    }
}
