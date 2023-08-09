
using System.ComponentModel.DataAnnotations;

namespace Banking.Services.Models
{
    public class MfaCode
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Code { get; set; } = string.Empty;
        public DateTime? ExpirationTime { get; set; }
        public DateTime? CreatedDate { get; private set; }
    }
}
