using AccountingCalculator.DataModels;
using AccountingCalculator.Services;
using AccountingCalculator.Services.Contracts;
using static AccountingCalculator.Common.SalaryComponents;

namespace AccountingCalculator.Test
{
    public class SalaryCalculateServiceTest
    {
        private ISalaryCalculateService salaryCalculateService = null!;

        [SetUp]
        public void Setup()
        {
            salaryCalculateService = new SalaryCalculateService();
        }

        [TestCase(1000)]
        [TestCase(1200)]
        [TestCase(1500)]
        [TestCase(500)]   // below minimum insurable income
        [TestCase(5000)]  // above maximum insurable income
        public void Calculate_ShouldKeepInternalTotalsConsistent(decimal grossSalary)
        {
            SalaryBreakdown result = salaryCalculateService.Calculate(grossSalary);

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

        private static IEnumerable<TestCaseData> InsurableIncomeTestCases()
        {
            yield return new TestCaseData(500m, MinInsurableIncome).SetName("Below minimum insurable income");
            yield return new TestCaseData(1000m, 1000m).SetName("Within range");
            yield return new TestCaseData(5000m, MaxInsurableIncome).SetName("Above maximum insurable income");
        }

        [TestCaseSource(nameof(InsurableIncomeTestCases))]
        public void Calculate_ShouldClampInsurableIncome(decimal grossSalary, decimal expectedInsurableIncome)
        {
            SalaryBreakdown result = salaryCalculateService.Calculate(grossSalary);

            Assert.That(result.InsurableIncome, Is.EqualTo(expectedInsurableIncome));
        }

        [TestCase(1000)]
        [TestCase(1234.56)]
        [TestCase(5000)]
        public void Calculate_ShouldApplyIncomeTaxOnFlooredTaxableIncome(decimal grossSalary)
        {
            SalaryBreakdown result = salaryCalculateService.Calculate(grossSalary);
            decimal expectedTax = Math.Round(Math.Floor(result.TaxableIncome) * IncomeTaxRate, 2, MidpointRounding.AwayFromZero);

            Assert.That(result.IncomeTax, Is.EqualTo(expectedTax));
        }

        [TestCase(1822.99, 1400.00, 2188.13)]
        [TestCase(2000.00, 1535.94, 2400.60)]
        public void Calculate_ShouldMatchGoldenCases(decimal grossSalary, decimal expectedNetSalary, decimal expectedTotalCostToEmployer)
        {
            SalaryBreakdown result = salaryCalculateService.Calculate(grossSalary);

            Assert.Multiple(() =>
            {
                Assert.That(result.GrossSalary, Is.EqualTo(grossSalary));
                Assert.That(result.NetSalary, Is.EqualTo(expectedNetSalary));
                Assert.That(result.TotalCostToEmployer, Is.EqualTo(expectedTotalCostToEmployer));
            });
        }

    }
}
