using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Domain.Models
{
    public class LoanCalculationRequest
    {
        public int UserId { get; set; }  // User for whom the loan details are being calculated
        public double PrincipalAmount { get; set; }  // The principal amount of the loan being requested
        public double AnnualInterestRate { get; set; }  // The interest rate for the loan
        public int LoanTermInYears { get; set; }  // Duration of the loan in years
        public double DownPayment { get; set; }  // Initial down payment, if any
        public bool IsSecuredLoan { get; set; }  // If the loan is secured against an asset like a house or car
        public string LoanPurpose { get; set; }  // e.g., Home, Car, Education, Personal
    }

}
