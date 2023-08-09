using Banking.Data.Context;
using Banking.Domain.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Globalization;

namespace Banking.Services.Services
{
    public class StockMarketService
    {
        private readonly IOptions<AlphaVantageSettings> _settings;
        private readonly BankingDbContext _context;

        public StockMarketService(IOptions<AlphaVantageSettings> settings, BankingDbContext context)
        {
            _settings = settings;
            _context = context;
        }

        public async Task<double> GetCurrentStockPrice(string symbol)
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.GetStringAsync($"{_settings.Value.Endpoint}?function=TIME_SERIES_INTRADAY&symbol={symbol}&interval=5min&apikey={_settings.Value.ApiKey}");
            var data = JsonConvert.DeserializeObject<dynamic>(response);
            var latestTime = data?["Meta Data"]["3. Last Refreshed"].Value;
            var latestPrice = data?["Time Series (5min)"][latestTime]["1. open"].Value;
            return double.Parse(latestPrice, CultureInfo.InvariantCulture);
        }

        public async Task StoreHistoricalData(string symbol)
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.GetStringAsync($"{_settings.Value.Endpoint}?function=TIME_SERIES_DAILY&symbol={symbol}&apikey={_settings.Value.ApiKey}");
            var data = JsonConvert.DeserializeObject<dynamic>(response);
            var timeSeries = data?["Time Series (Daily)"];

            foreach (var date in timeSeries.Properties)
            {
                var record = new StockPriceHistory
                {
                    Symbol = symbol,
                    Date = DateTime.Parse(date.Name),
                    OpenPrice = double.Parse(timeSeries[date.Name]["1. open"].Value, CultureInfo.InvariantCulture),
                    HighPrice = double.Parse(timeSeries[date.Name]["2. high"].Value, CultureInfo.InvariantCulture),
                    LowPrice = double.Parse(timeSeries[date.Name]["3. low"].Value, CultureInfo.InvariantCulture),
                    ClosePrice = double.Parse(timeSeries[date.Name]["4. close"].Value, CultureInfo.InvariantCulture)
                };

                _context.StockPriceHistories.Add(record);
            }

            _context.SaveChanges();
        }

    }
}
