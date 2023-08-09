namespace Banking.Domain.Models
{
    public class BackupCode
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Code { get; set; } = string.Empty;
        public bool Used { get; set; }
    }

}
