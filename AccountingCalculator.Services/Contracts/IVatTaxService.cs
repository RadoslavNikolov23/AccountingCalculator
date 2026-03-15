using System;
using System.Collections.Generic;
using System.Text;

namespace AccountingCalculator.Services.Contracts
{
    public interface IVatTaxService
    {
        public decimal CalculateVatFromNet(decimal netAmount, decimal vatRate);

        public decimal CalculateTotalFromNet(decimal netAmount, decimal vatRate);

        public decimal ExtractNetFromTotal(decimal totalAmount, decimal varRate);

        public decimal CalculateVatOwed(decimal salesVat, decimal purchaseVat);

        public bool IsVatCorrect(decimal netAmount, decimal storedVat, decimal vatRate, decimal tolerance = 0.02m);
    }
}
