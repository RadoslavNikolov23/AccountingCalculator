using AccountingCalculator.DataModels;

namespace AccountingCalculator.Services.Contracts
{
    public interface ISalaryCalculateService
    {
        public SalaryBreakdown Calculate(decimal grossSalary);
    }
}
