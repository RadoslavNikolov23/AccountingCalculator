using System;
using System.Collections.Generic;
using System.Text;

namespace AccountingCalculator.DataModels
{
    public class SalaryBreakdown
    {
        public decimal GrossSalary { get; set; }
        public decimal InsurableIncome { get; set; }

        //Удръжки от работника
        public decimal EmployeeDOO { get; set; }
        public decimal EmployeeDZPO { get; set; }
        public decimal EmployeeZO { get; set; }
        public decimal TotalEmployeeContributions { get; set; }
        public decimal TaxableIncome { get; set; }
        public decimal IncomeTax { get; set; }
        public decimal NetSalary { get; set; }

        //Удръжки от работодателя
        public decimal EmployerDOO { get; set; }
        public decimal EmployerDZPO { get; set; }
        public decimal EmployerZO { get; set; }
        public decimal EmployerAccident { get; set; }
        public decimal TotalEmployerContributions { get; set; }

        //Обща цена за работодателя
        public decimal TotalCostToEmployer { get; set; }
    }
}
