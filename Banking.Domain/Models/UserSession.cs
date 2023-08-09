using System.ComponentModel.DataAnnotations;

namespace Banking.Domain.Models
{
    public class UserSession
    {
        [Key]
        public int UserId { get; set; }
        public string Token { get; set; } = string.Empty;
        public string? IPAddress { get; set; }
        public string UserAgent { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
