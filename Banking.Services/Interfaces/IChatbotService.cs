using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Services.Interfaces
{
    public interface IChatbotService
    {
        Task<string> HandleUserQuery(string query, string sessionId);
    }
}
