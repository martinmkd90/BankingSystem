using Banking.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Services.DTOs
{
    public class LoanApplicationDto
    {
        public int UserId { get; set; }
        public double AmountRequested { get; set; }
        public LoanType LoanType { get; set; }
        public int DurationInMonths { get; set; }
    }

    public class LoanRepaymentDto
    {
        public int LoanId { get; set; }
        public double AmountPaid { get; set; }
    }

}
