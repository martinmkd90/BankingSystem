using Banking.Data.Context;
using Banking.Domain.Enums;
using Banking.Domain.Models;
using Banking.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Banking.Services.Services
{
    internal class InvestmentRecommendationService : IInvestmentRecommendationService
    {
        private readonly BankingDbContext _context;

        public InvestmentRecommendationService(BankingDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Investment> GetRecommendations(int userId)
        {
            var userProfile = _context.UserRiskProfiles.SingleOrDefault(up => up.UserId == userId);
            if (userProfile == null)
            {
                // Handle the case where the user's risk profile isn't set
                return Enumerable.Empty<Investment>();
            }

            // Complex algorithm to fetch suitable investments based on user's profile
            var recommendedInvestments = _context.Investments.AsQueryable();

            if (userProfile.RiskTolerance == RiskType.Low)
            {
                recommendedInvestments = recommendedInvestments.Where(i => i.InstrumentType == "Bond");
            }
            else if (userProfile.RiskTolerance == RiskType.Medium)
            {
                recommendedInvestments = recommendedInvestments.Where(i => i.InstrumentType != "Cryptocurrency"); // Example: Avoiding high-risk crypto for medium risk users
            }

            if (userProfile.Age < 30 && userProfile.ExistingInvestmentsValue < 50000)
            {
                recommendedInvestments = recommendedInvestments.Where(i => i.InstrumentName.Contains("Growth")); // Younger users with less investments might prefer growth stocks
            }

            // If the user's investment goal is short-term and they have a high-risk tolerance, recommend tech stocks or emerging markets
            if (userProfile.Goal == InvestmentGoal.ShortTerm && userProfile.RiskTolerance == RiskType.High)
            {
                recommendedInvestments = recommendedInvestments.Where(i => i.InstrumentType == "Stock" && (i.InstrumentName.Contains("Tech") || i.InstrumentName.Contains("Emerging Markets")));
            }

            // If the user's annual income is high and they have a medium risk tolerance, recommend diversified mutual funds
            if (userProfile.AnnualIncome > 100000 && userProfile.RiskTolerance == RiskType.Medium)
            {
                recommendedInvestments = recommendedInvestments.Where(i => i.InstrumentType == "Mutual Fund" && i.InstrumentName.Contains("Diversified"));
            }

            // For older users nearing retirement, recommend more stable, income-generating investments like dividend stocks or bonds
            if (userProfile.Age > 50)
            {
                recommendedInvestments = recommendedInvestments.Where(i => i.InstrumentType == "Bond" || i.InstrumentName.Contains("Dividend"));
            }

            // For users with a long-term goal and low-risk tolerance, recommend index funds or government bonds
            if (userProfile.Goal == InvestmentGoal.LongTerm && userProfile.RiskTolerance == RiskType.Low)
            {
                recommendedInvestments = recommendedInvestments.Where(i => i.InstrumentType == "Index Fund" || i.InstrumentType == "Government Bond");
            }

            // If the user has a significant amount in existing investments, recommend adding some gold or commodities to diversify
            if (userProfile.ExistingInvestmentsValue > 200000)
            {
                recommendedInvestments = recommendedInvestments.Where(i => i.InstrumentType == "Commodity" || i.InstrumentName.Contains("Gold"));
            }

            // ... potentially more conditions ...

            return recommendedInvestments.AsNoTracking().ToList();
        }
    }
}
