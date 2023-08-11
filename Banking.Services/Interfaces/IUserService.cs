using Banking.Domain.Models;
using Banking.Services.DTOs;
using Banking.Services.Services;
using Microsoft.AspNetCore.Http;

namespace Banking.Services.Interfaces
{
    public interface IUserService
    {
        User Register(UserDto model);
        User Login(UserDto model);
        string GenerateJwtToken(User user, HttpResponse response);
        Task<DateTime?> GetLastLogin(int userId);
        bool VerifyPassword(string password, string passwordHash);
        public User GetUserById(int userId);
        User GetUserByUsername(string username);
        User GetUserByEmail(string email);
        bool VerifyMfaCode(int userId, string code);
        void SendMfaCodeToUser(User user, string mfaCode);
        List<string> GenerateBackupCodes(int userId, int numberOfCodes);
        string GenerateMfaCode();
        Task RequestPasswordReset(string email);
        Task<bool> ResetPassword(PasswordResetDto model);
        Task<bool> IsPasswordCompromised(string password);
        string HashPassword(string password);
        void UpdateUser(User user);
        Role GetRoleByName(string name);
    }
}
