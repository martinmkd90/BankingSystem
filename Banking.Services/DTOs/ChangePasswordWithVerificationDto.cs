namespace Banking.Services.DTOs
{
    public class ChangePasswordWithVerificationDto
    {
        public int UserId { get; set; }
        public string NewPassword { get; set; } = string.Empty;
        public string VerificationCode { get; set; } = string.Empty;
    }

}
