using Banking.Data.Context;
using Banking.Domain.Models;
using Banking.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Banking.Services.Services
{
    public class NotificationService : INotificationService
    {
        private readonly BankingDbContext _context;
        private readonly IEmailService _emailService;

        public NotificationService(BankingDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public void CreateNotification(int userId, string message)
        {
            var notification = new Notification
            {
                UserId = userId,
                Message = message,
                Date = DateTime.UtcNow,
                IsRead = false
            };

            _context.Notifications.Add(notification);
            _context.SaveChanges();
        }


        public IEnumerable<Notification> GetUserNotifications(int userId)
        {
            return _context.Notifications.Where(n => n.UserId == userId).OrderByDescending(n => n.Date).AsNoTracking().ToList();
        }
        public async Task<IEnumerable<Notification>> GetUserUnreadNotifications(int userId)
        {
            return await _context.Notifications.Where(n => n.UserId == userId && !n.IsRead).OrderByDescending(n => n.Date).AsNoTracking().ToListAsync();
        }

        public void MarkAsRead(int notificationId)
        {
            var notification = _context.Notifications.SingleOrDefault(n => n.Id == notificationId);
            if (notification != null)
            {
                notification.IsRead = true;
                _context.SaveChanges();
            }
        }

        public void SendNotification(int userId, string message)
        {
            // Fetch the user from the database
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                throw new ArgumentException($"No user found with ID {userId}", nameof(userId));
            }

            // Create a new notification
            var notification = new Notification
            {
                User = user,
                Message = message,
                Date = DateTime.UtcNow
            };

            // Add the notification to the database
            _context.Notifications.Add(notification);
            _context.SaveChanges();

            // Optionally, you could also send the notification via email, SMS, etc. here
            // This would likely involve calling another service or method
        }

        public void SendNotificationToSupport(string message)
        {
            var supportEmail = "support@yourbank.com";
            var subject = "New Support Notification";
            _emailService.SendEmail(supportEmail, subject, message);
        }

    }
}
