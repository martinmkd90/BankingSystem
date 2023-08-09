namespace Banking.Domain.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public int RoleId { get; set; }
        public bool IsActive { get; set; }

        public virtual Role Role { get; set; } = new Role();
        public virtual Account Account { get; set; } = new Account();
        public ICollection<Account>? Accounts { get; set; }
        public ICollection<Transaction>? Transactions { get; set; }
        public DateTime? LastLogin { get; set; }
        public int FailedLoginAttempts { get; set; }
        public DateTime? LastMfaAttempt { get; set; }
        public DateTime? LockoutEnd { get; set; }
        public bool IsLocked { get; set; }
        public bool IsMfaEnabled { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PasswordResetToken { get; set; } = string.Empty;
        public DateTime? PasswordResetTokenExpiry { get; set; } = new DateTime();
        public DateTime LastPasswordChangeDate { get; set; }
        public List<string> PreviousPasswordHashes { get; set; } = new List<string>();
    }
}
