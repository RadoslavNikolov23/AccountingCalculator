using AccountingCalculator.DataModels;
using AccountingCalculator.Services.Contracts;
using static AccountingCalculator.Common.SalaryComponents;

namespace AccountingCalculator.Services
{
    public class SalaryCalculateService: ISalaryCalculateService
    {
        
        public SalaryBreakdown Calculate(decimal grossSalary)
        {
            //Фиксиране на осигурителния доход между минималния и максималния
            decimal insurableIncome = Math.Min(Math.Max(grossSalary, MinInsurableIncome), MaxInsurableIncome);

            //Изчисляване на осигуровките за работника
            decimal employeeDOO = Math.Round(insurableIncome * EmployeeDOO, 2, MidpointRounding.AwayFromZero);
            decimal employeeDZPO = Math.Round(insurableIncome * EmployeeDZPO, 2, MidpointRounding.AwayFromZero);
            decimal employeeZO = Math.Round(insurableIncome * EmployeeZO, 2, MidpointRounding.AwayFromZero);
            decimal totalEmployeeContributions = employeeDOO + employeeDZPO + employeeZO;

            //Облагаем доход = бруто - социално осигуряване на служителя
            decimal taxableIncome = grossSalary - totalEmployeeContributions;

            //Изчисляване на данък общ доход
            decimal incomeTax = Math.Round(taxableIncome * IncomeTaxRate, 2, MidpointRounding.AwayFromZero);

            //Изчисляване на нетната заплата
            decimal netSalary = grossSalary - totalEmployeeContributions - incomeTax;

            //Изчисляване на осигуровките за работодателя
            decimal employerDOO = Math.Round(insurableIncome * EmployerDOO, 2, MidpointRounding.AwayFromZero);
            decimal employerDZPO = Math.Round(insurableIncome * EmployerDZPO, 2, MidpointRounding.AwayFromZero);
            decimal employerZO = Math.Round(insurableIncome * EmployerZO, 2, MidpointRounding.AwayFromZero);
            decimal employerAccident = Math.Round(insurableIncome * EmployerAccident, 2, MidpointRounding.AwayFromZero);
            decimal totalEmployerContributions = employerDOO + employerDZPO + employerZO + employerAccident;


            //Обща цена за работодателя = бруто + вноски на работодателя
            decimal totalCostToEmployer = grossSalary + totalEmployerContributions;

            return new SalaryBreakdown
            {
                GrossSalary = grossSalary,
                InsurableIncome = insurableIncome,

                //Удръжки от работника
                EmployeeDOO = employeeDOO,
                EmployeeDZPO = employeeDZPO,
                EmployeeZO = employeeZO,
                TotalEmployeeContributions = totalEmployeeContributions,
                TaxableIncome = taxableIncome,
                IncomeTax = incomeTax,
                NetSalary = netSalary,

                //Удръжки от работодателя
                EmployerDOO = employerDOO,
                EmployerDZPO = employerDZPO,
                EmployerZO = employerZO,
                EmployerAccident = employerAccident,
                TotalEmployerContributions = totalEmployerContributions,
                TotalCostToEmployer = totalCostToEmployer
            };
        }
    }
}
