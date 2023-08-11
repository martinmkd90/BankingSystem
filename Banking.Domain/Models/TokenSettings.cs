namespace Banking.API.Controllers
{
    public class TokenSettings
    {
        public string ApiKey { get; set; }
        public int TokenExpiryMinutes { get; set; }
    }
}