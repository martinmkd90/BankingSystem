namespace Banking.Services.DTOs
{
    public class ChangePasswordWithVerificationDto
    {
        public int UserId { get; set; }
        public string NewPassword { get; set; }
        public string VerificationCode { get; set; }
    }

}
