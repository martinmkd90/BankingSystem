namespace Banking.API.Controllers
{
    public class TokenSettings
    {
        public string ApiKey { get; set; } = string.Empty;
        public int TokenExpiryMinutes { get; set; }
    }
}