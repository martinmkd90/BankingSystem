using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Domain.Enums
{
    public enum TransactionType
    {
        Deposit,
        Withdrawal,
        Debit,
        TransferIn,
        TransferOut,
        InterestCredited,
        ScheduledPayment,
    }
    public enum CardType
    {
        Debit,
        Credit
    }

    public enum AccountType
    {
        Checking,
        Savings
    }
    public enum RecurrenceType
    {
        Daily = 'D',
        Weekly = 'W',
        BiWeekly = 'B',
        Monthly = 'M',
        Quarterly = 'Q',
        Annually = 'A'
    }
    public enum FailReason
    {
        None = 0,
        InsufficientFunds = 1,
        AccountClosed = 2,
        TechnicalError = 3,
    }
    public enum TicketStatus
    {
        Open = 1,
        InProgress = 2,
        Resolved = 3,
        Closed = 4
    }
    public enum InterestType
    {
        Simple,
        Compound
    }

    public enum RateType
    {
        Fixed,
        Variable
    }

    public enum RiskType
    {
        Low = 1,
        Medium = 2,
        High = 3
    }

    public enum InvestmentGoal
    {
        ShortTerm = 1,
        MediumTerm = 2,
        LongTerm = 3
    }

    public enum NotificationType
    {
        TransactionAlert = 1,
        LowBalance = 2,
        InvestmentUpdate =3
    }

    public enum TriggerType
    {
        AccountBalanceThreshold,
        Monthly,
        OnSalaryDay
    }

    public enum FraudAlertStatus
    {
        Pending,
        Resolved,
        FalseAlarm
    }

    public enum ChatbotActionType
    {
        None,
        OpenLink,
        InitiateServiceRequest,
        ShowTransactionDetails,
        ContactSupport
    }

    public enum LoanStatus
    {
        Pending,
        Approved,
        Rejected
    }

    public enum LoanTransactionType
    {
        Disbursement,
        Repayment,
        InterestApplication
    }
}
