using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Services.DTOs
{
    public class ChatbotQueryRequest
    {
        public string Query { get; set; }
        public string SessionId { get; set; }
    }
}
