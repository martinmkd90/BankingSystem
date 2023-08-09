using Banking.Data.Context;
using Banking.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Banking.Services.Services
{
    public class ForeignExchangeService
    {
        private readonly BankingDbContext _context;
        private readonly IOptions<ExchangeApiSettings> _settings;
        private readonly IMemoryCache _cache; // For caching
        private readonly ILogger _logger;

        public ForeignExchangeService(BankingDbContext context, IOptions<ExchangeApiSettings> settings, ILogger<ForeignExchangeService> logger, IMemoryCache cache)
        {
            _context = context;
            _settings = settings;
            _cache = cache;
            _logger = logger;
        }

        public async Task StoreExchangeRates(IEnumerable<string> currencyPairs)
        {
            using var httpClient = new HttpClient();

            foreach (var pair in currencyPairs)
            {
                var fromCurrency = pair.Substring(0, 3);
                var toCurrency = pair.Substring(3, 3);

                try
                {
                    var response = await httpClient.GetStringAsync($"{_settings.Value.Endpoint}?from={fromCurrency}&to={toCurrency}&apikey={_settings.Value.ApiKey}");
                    var rate = JsonConvert.DeserializeObject<double>(response);

                    var record = new ExchangeRateHistory
                    {
                        FromCurrency = fromCurrency,
                        ToCurrency = toCurrency,
                        Date = DateTime.UtcNow,
                        Rate = rate
                    };

                    _context.ExchangeRateHistories.Add(record);
                }
                catch (HttpRequestException ex)
                {
                    _logger.LogError($"Failed to fetch exchange rate: {ex.Message}");
                    throw new Exception("Failed to fetch exchange rate. Please try again later.");
                }
                catch (DbUpdateException ex)
                {
                    _logger.LogError($"Database update failed: {ex.Message}");
                    throw new Exception("Failed to update the database. Please try again later.");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"An error occurred: {ex.Message}");
                    throw new Exception("An unexpected error occurred. Please try again later.");
                }
            }

            _context.SaveChanges();
        }

        public double GetLatestExchangeRate(string fromCurrency, string toCurrency)
        {
            var cacheKey = $"{fromCurrency}-{toCurrency}";

            if (!_cache.TryGetValue(cacheKey, out double rate))
            {
                rate = _context.ExchangeRateHistories
                    .Where(e => e.FromCurrency == fromCurrency && e.ToCurrency == toCurrency)
                    .OrderByDescending(e => e.Date)
                    .FirstOrDefault()?.Rate ?? 0;

                var cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1) // Cache for 1 hour
                };

                _cache.Set(cacheKey, rate, cacheOptions);
            }

            return rate;
        }
    }
}    
