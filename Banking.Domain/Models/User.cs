namespace Banking.Domain.Models
{
    public class User
    {
        public User()
        {
               
        }
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Username { get; set; }
        public string PasswordHash { get; set; }
        public int RoleId { get; set; }
        public bool IsActive { get; set; }
        public virtual Role Role { get; set; }
        public virtual Account Account { get; set; }
        public ICollection<Account>? Accounts { get; set; }
        public ICollection<Transaction>? Transactions { get; set; }
        public DateTime? LastLogin { get; set; }
        public int FailedLoginAttempts { get; set; }
        public DateTime? LastMfaAttempt { get; set; }
        public DateTime? LockoutEnd { get; set; }
        public bool IsLocked { get; set; }
        public bool IsMfaEnabled { get; set; }
        public string? Email { get; set; }
        public string? PasswordResetToken { get; set; }
        public DateTime? PasswordResetTokenExpiry { get; set; }
        public DateTime? LastPasswordChangeDate { get; set; }
        public List<string>? PreviousPasswordHashes { get; set; }
    }
}
