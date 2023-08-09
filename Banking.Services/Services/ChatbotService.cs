using Banking.Domain.Models;
using Banking.Services.Interfaces;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Services.Services
{
    public class ChatbotService : IChatbotService
    {
        private readonly IOptions<DialogflowSettings> _settings;

        public ChatbotService(IOptions<DialogflowSettings> settings)
        {
            _settings = settings;
        }

        public async Task<string> HandleUserQuery(string query, string sessionId)
        {
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_settings.Value.ApiKey}");

            var requestPayload = new
            {
                queryInput = new
                {
                    text = new
                    {
                        text = query,
                        languageCode = "en"
                    }
                }
            };

            var endpoint = _settings.Value.Endpoint.Replace("{session_id}", sessionId);
            var response = await httpClient.PostAsync(endpoint, new StringContent(JsonConvert.SerializeObject(requestPayload), Encoding.UTF8, "application/json"));
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var dialogflowResponse = JsonConvert.DeserializeObject<dynamic>(jsonResponse);

            return dialogflowResponse.queryResult.fulfillmentText;
        }
    }


}
