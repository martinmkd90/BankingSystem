using Banking.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Banking.Services.Interfaces
{
    public interface ILoginService
    {
        User Authenticate(string username, string password);
        bool IsAccountLocked(string username);
        void IncrementFailedLoginAttempt(string username);
        void ResetFailedLoginAttempt(string username);
        void RecordFailedLoginAttempt(string username);
    }
}