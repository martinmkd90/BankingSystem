using Banking.Domain.Models;
using Banking.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Services.Interfaces
{
    public interface ILoanService
    {
        Loan ApplyForLoan(LoanApplicationDto applicationDto);
        Loan ApproveLoan(int loanId);
        Loan RejectLoan(int loanId);
        bool MakeRepayment(LoanRepaymentDto repaymentDto);  // Update this line
        Task<IEnumerable<Loan>> GetUserLoans(int userId);
    }
}
