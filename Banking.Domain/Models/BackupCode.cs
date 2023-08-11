namespace Banking.Domain.Models
{
    public class BackupCode
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Code { get; set; }
        public bool Used { get; set; }
    }

}
