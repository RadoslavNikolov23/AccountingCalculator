namespace AccountingCalculator.Calculations
{
    public class SalaryCalculator
    {
        //----------------------------------------------
        //----Осигуровки-удръжки от работника/служител-----

        //Държавното обществено осигуряване (ДОО) пенсия
        public const decimal EmployeeDOO = 0.1058m;

        //ДЗПО универсален пенсионен фонд
        public const decimal EmployeeDZPO = 0.0330m;

        //Здравноосигурителна вноска
        public const decimal EmployeeZO = 0.0320m;

        //Осигуровки общо сума
        public static decimal EmployeeTotalContributions = EmployeeDOO + EmployeeDZPO + EmployeeZO;
        //---------------------------------------------------



        //----------------------------------------------
        //-----Осигуровки-удръжки от работодателя-------

        //Държавното обществено осигуряване (ДОО) пенсия
        public const decimal EmployerDOO = 0.1292m;

        //ДЗПО универсален пенсионен фонд
        public const decimal EmployerDZPO = 0.0570m;

        //Здравноосигурителна вноска
        public const decimal EmployerZO = 0.0480m;

        //Средна ставка за трудова злополука и професионална болест
        public const decimal EmployerAccident = 0.0040m;

        //Осигуровки общо сума
        public const decimal EmployerTotalContributions = EmployerDOO + EmployerDZPO + EmployerZO + EmployerAccident;
        //----------------------------------------------


        //----------------------------------------------

        //ДОД - Данък общ доход 10% от брутната заплата
        public const decimal IncomeTaxRate = 0.10m;

        //----------------------------------------------


        //----------------------------------------------

        //Осигурителни прагове за 2026 година
        public const decimal MinInsurableIncome = 933m;
        public const decimal MaxInsurableIncome = 3750m;

        //----------------------------------------------

        public static SalaryBreakdown Calculate(decimal grossSalary)
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
}
