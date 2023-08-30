using Banking.Domain.Models;
using Banking.Services.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Banking.Domain.Mappings;
using Microsoft.Extensions.Logging;

namespace Banking.Data.Context
{
    public class BankingDbContext : DbContext
    {
        private readonly ILogger<BankingDbContext> _logger;
        public BankingDbContext(DbContextOptions<BankingDbContext> options, ILogger<BankingDbContext> logger): base(options)
        {
            _logger = logger;
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountTypes> AccountTypes { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Transfer> Transfers { get; set; }
        public DbSet<Loan> Loans { get; set; }
        public DbSet<LoanTransaction> LoanTransactions { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<ScheduledPayment> ScheduledPayments { get; set; }
        public DbSet<AccountStatement> AccountStatements { get; set; }
        public DbSet<AuditRecord> AuditRecords { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Investment> Investments { get; set; }
        public DbSet<InvestmentHistory> InvestmentHistories { get; set; }
        public DbSet<InterestRate> InterestRates { get; set; }
        public DbSet<StockPriceHistory> StockPriceHistories { get; set; }
        public DbSet<ExchangeRateHistory> ExchangeRateHistories { get; set; }
        public DbSet<SupportMessage> SupportMessages { get; set; }
        public DbSet<SupportTicketRequest> SupportTicketRequests { get; set; }
        public DbSet<SupportTicketResponse> SupportTicketResponses { get; set; }
        public DbSet<ExternalAccount> ExternalAccounts { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<UserRiskProfile> UserRiskProfiles { get; set; }
        public DbSet<Budget> Budgets { get; set; }
        public DbSet<SavingsGoal> SavingsGoals { get; set; }
        public DbSet<AutomatedInvestmentRule> AutomatedInvestmentRules { get; set; }
        public DbSet<FraudAlert> FraudAlerts { get; set; }
        public DbSet<FinancialInsight> FinancialInsights { get; set; }
        public DbSet<OverdraftHistory> OverdraftHistories { get; set; }
        public DbSet<AccountOverview> AccountOverviews { get; set; }
        public DbSet<MfaCode> MfaCodes { get; set; }
        public DbSet<BackupCode> BackupCodes { get; set; }
        public DbSet<UserSession> UserSessions { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductChannel> Channels { get; set; }
        public DbSet<Segment> Segments { get; set; }
        public DbSet<UserSegmentMapping> UserSegmentMappings { get; set; }
        public DbSet<ProductChannelMapping> ProductChannelMappings { get; set; }
        public DbSet<ProductSegmentMapping> ProductSegmentMappings { get; set; }
        public DbSet<LoanApplication> LoanApplications { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RolePermission>()
                .HasKey(rp => new { rp.RoleId, rp.PermissionId });  // Composite primary key

            modelBuilder.Entity<RolePermission>()
                .HasOne(rp => rp.Role)
                .WithMany(r => r.RolePermissions)
                .HasForeignKey(rp => rp.RoleId);

            modelBuilder.Entity<RolePermission>()
                .HasOne(rp => rp.Permission)
                .WithMany(p => p.RolePermissions)
                .HasForeignKey(rp => rp.PermissionId);

            modelBuilder.Entity<Transfer>()
                .HasOne(t => t.FromAccount)
                .WithMany(a => a.OutgoingTransfers)
                .HasForeignKey(t => t.FromAccountId)
                .OnDelete(DeleteBehavior.NoAction);  // Cascade on delete for FromAccountId

            modelBuilder.Entity<Transfer>()
                .HasOne(t => t.ToAccount)
                .WithMany(a => a.IncomingTransfers)
                .HasForeignKey(t => t.ToAccountId)
                .OnDelete(DeleteBehavior.NoAction);   // No cascade on delete for ToAccountId

            modelBuilder.Entity<User>().Ignore(u => u.Account);

            modelBuilder.Entity<UserProfile>()
                .HasKey(u => u.UserId);

            modelBuilder.Entity<AccountOverview>()
                .HasOne(ao => ao.Account)
                .WithOne(a => a.AccountOverview)
                .HasForeignKey<AccountOverview>(ao => ao.AccountId);

            modelBuilder.Entity<MfaCode>()
                .HasKey(mc => mc.UserId);

            modelBuilder.Entity<User>()
                .Property(e => e.PreviousPasswordHashes)
                .HasConversion(
                    v => string.Join(',', v),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList())
                .Metadata.SetValueComparer(new ValueComparer<List<string>>(
                    (c1, c2) => c1.SequenceEqual(c2),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToList()));

            modelBuilder.Entity<UserSegmentMapping>()
                .HasKey(usm => usm.UserSegmentMappingID);

            modelBuilder.Entity<ProductSegmentMapping>()
                .HasKey(psm => new { psm.ProductID, psm.SegmentID });

            modelBuilder.Entity<ProductChannelMapping>()
                .HasKey(pcm => new { pcm.ProductID, pcm.ChannelID });

            modelBuilder.Entity<LoanApplication>()
                .HasKey(pcm => new { pcm.LoanApplicationID, pcm.UserID });

            modelBuilder.Entity<User>()
                .Property(u => u.Id).ValueGeneratedOnAdd();
        }

        public override int SaveChanges()
        {
            _logger.LogInformation("Saving changes to the database");
            return base.SaveChanges();
        }
    }
}
