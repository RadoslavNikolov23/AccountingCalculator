namespace AccountingCalculator.Common
{
    public static class SalaryComponents
    {
        //----Осигуровки-удръжки от работника/служител-----

        //Държавното обществено осигуряване (ДОО) пенсия
        public const decimal EmployeeDOO = 0.1058m;

        //ДЗПО универсален пенсионен фонд
        public const decimal EmployeeDZPO = 0.0330m;

        //Здравноосигурителна вноска
        public const decimal EmployeeZO = 0.0320m;

        //Осигуровки общо сума
        public static decimal EmployeeTotalContributions = EmployeeDOO + EmployeeDZPO + EmployeeZO;



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


        //----ДОД - Данък общ доход 10% от брутната заплата
        public const decimal IncomeTaxRate = 0.10m;

        
        //----Осигурителни прагове за 2026 година
        public const decimal MinInsurableIncome = 933m;
        public const decimal MaxInsurableIncome = 3750m;
    }
}
