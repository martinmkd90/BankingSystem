using Banking.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Services.Interfaces
{
    public interface INotificationService
    {
        void CreateNotification(int userId, string message);
        IEnumerable<Notification> GetUserNotifications(int userId);
        Task<IEnumerable<Notification>> GetUserUnreadNotifications(int userId);
        void MarkAsRead(int notificationId);
        void SendNotification(int userId, string message);
        void SendNotificationToSupport(string message);
    }
}
