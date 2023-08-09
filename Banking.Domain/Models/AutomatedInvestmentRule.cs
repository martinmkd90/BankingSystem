using Banking.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Domain.Models
{
    public class AutomatedInvestmentRule
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public TriggerType TriggerType { get; set; }
        public double TriggerValue { get; set; }
        public double InvestmentAmount { get; set; }
        public string InvestmentInstrument { get; set; }
        public string InvestmentInstrumentType { get; set; }
        public bool IsActive { get; set; }
    }
}
