using Banking.Domain.Enums;

namespace Banking.Domain.Models
{   
    public class ChatbotResponse
    {
        public string ResponseMessage { get; set; }
        public ChatbotActionType ActionType { get; set; }
        public string ActionLink { get; set; } // This can be a URL or an app-specific deep link.
        public string AdditionalInfo { get; set; } // Any other relevant information or context.
    }
}
