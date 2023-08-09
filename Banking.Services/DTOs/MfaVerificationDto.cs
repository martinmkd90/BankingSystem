
namespace Banking.Services.DTOs
{
    public class MfaVerificationDto
    {
        public int UserId { get; set; }
        public string Code { get; set; } = string.Empty;
    }
}
