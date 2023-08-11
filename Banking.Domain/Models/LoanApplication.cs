using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Domain.Models
{
    public class LoanApplication
    {
        public int LoanApplicationID { get; set; }
        public int UserID { get; set; }
        public User User { get; set; } = new User(); // Navigation property
        public int ProductID { get; set; }
        public Product Product { get; set; } = new Product(); // Navigation property
        public string ApplicationStatus { get; set; }
        public DateTime AppliedDate { get; set; } = new DateTime();
        public DateTime? ApprovedDate { get; set; }
        public DateTime? RejectedDate { get; set; }
        public string ReasonForRejection { get; set; }
    }
}
