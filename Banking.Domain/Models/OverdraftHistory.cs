using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Domain.Models
{
    public class OverdraftHistory
    {
        public int Id { get; set; }  // Primary key
        public int AccountId { get; set; }  // Foreign key to the Account
        public DateTime Date { get; set; }  // Date and time when the overdraft occurred
        public double WithdrawalAmount { get; set; }  // The amount that was withdrawn causing the overdraft
        public double EffectiveBalance { get; set; }  // The balance of the account after the withdrawal
        public double OverdraftFeeApplied { get; set; }  // The fee applied due to the overdraft

        // Navigation property
        public Account Account { get; set; }  // Reference to the associated account
    }

}
