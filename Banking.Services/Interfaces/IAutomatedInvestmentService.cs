using Banking.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Services.Interfaces
{
    public interface IAutomatedInvestmentService
    {
        void ExecuteRules();
        AutomatedInvestmentRule CreateRule(AutomatedInvestmentRule rule);
        AutomatedInvestmentRule UpdateRule(AutomatedInvestmentRule rule);
        void DeleteRule(int ruleId);
        AutomatedInvestmentRule GetRule(int ruleId);
        IEnumerable<AutomatedInvestmentRule> GetUserRules(int userId);
        
    }
}
