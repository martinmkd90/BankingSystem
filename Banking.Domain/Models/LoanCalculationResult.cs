using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Domain.Models
{
    public class LoanCalculationResult
    {
        public double MonthlyPayment { get; set; }  // Monthly payment amount
        public double TotalPayment { get; set; }  // Total payment over the loan term
        public double TotalInterest { get; set; }  // Total interest paid over the loan term
        public List<AmortizationEntry> AmortizationSchedule { get; set; }  // Monthly breakdown of payments
    }

    public class AmortizationEntry
    {
        public int MonthNumber { get; set; }  // Month number in the loan term
        public double PrincipalPaid { get; set; }  // Principal amount paid in the month
        public double InterestPaid { get; set; }  // Interest amount paid in the month
        public double RemainingBalance { get; set; }  // Remaining loan balance after the month's payment
    }

}
