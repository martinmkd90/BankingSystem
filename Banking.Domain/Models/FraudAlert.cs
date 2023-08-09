using Banking.Domain.Enums;

namespace Banking.Domain.Models
{
    public class FraudAlert
    {
        public int Id { get; set; }
        public int TransactionId { get; set; }
        public string Reason { get; set; }
        public DateTime DetectedOn { get; set; }
        public FraudAlertStatus Status { get; set; }
        public string ResolvedBy { get; set; } // Employee ID or name who resolved the alert
        public DateTime? ResolvedOn { get; set; }

        // Navigation property
        public Transaction Transaction { get; set; }
    }

    public class FraudAlertResolutionRequest
    {
        public string ResolvedBy { get; set; }
        public FraudAlertStatus Status { get; set; }
    }
}
