using Banking.Data.Context;
using Banking.Domain.Models;
using Banking.Services.Interfaces;
using Serilog;

namespace Banking.Services.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly BankingDbContext _context;
        private readonly IAccountService _accountService;
        private readonly ILoanService _loanService;
        private readonly IInvestmentService _investmentService;
        private readonly IScheduledPaymentService _scheduledPaymentService;
        private readonly IUserService _userService;
        private readonly INotificationService _notificationService;
        private readonly ILogger _logger;

        public DashboardService(BankingDbContext context, IAccountService accountService, ILoanService loanService, IInvestmentService investmentService, IScheduledPaymentService scheduledPaymentService,
            IUserService userService, INotificationService notificationService, ILogger logger)
        {
            _context = context;
            _accountService = accountService;
            _loanService = loanService;
            _investmentService = investmentService;
            _notificationService = notificationService;
            _userService = userService;
            _scheduledPaymentService = scheduledPaymentService;
            _logger = logger;
        }

        public async Task<Dashboard> GetUserDashboard(int userId)
        {
            try
            {
                var dashboard = new Dashboard
                {
                    User = await _context.Users.FindAsync(userId) ?? new User(),
                    AccountOverview = await _accountService.GetAccountOverview(userId),
                    ActiveLoans = await _loanService.GetUserLoans(userId),
                    Investments = await _investmentService.GetUserInvestments(userId),
                    UpcomingPayments = await _scheduledPaymentService.GetSchedulePayments(userId),
                    LastLogin = await _userService.GetLastLogin(userId),
                    UnreadNotifications = await _notificationService.GetUserUnreadNotifications(userId)
                };

                return dashboard;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error fetching dashboard data for user {UserId}", userId);
                throw new Exception("Error fetching dashboard data.", ex);
            }
        }
    }
}
