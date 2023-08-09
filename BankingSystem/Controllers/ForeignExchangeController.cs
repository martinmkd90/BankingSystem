using Banking.Services.Services;
using Microsoft.AspNetCore.Mvc;

namespace Banking.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ForeignExchangeController : ControllerBase
    {
        private readonly ForeignExchangeService _foreignExchangeService;
        private readonly ILogger _logger;

        public ForeignExchangeController(ForeignExchangeService foreignExchangeService)
        {
            _foreignExchangeService = foreignExchangeService;
        }

        [HttpGet("{fromCurrency}/{toCurrency}")]
        public IActionResult GetExchangeRate(string fromCurrency, string toCurrency)
        {
            var rate = _foreignExchangeService.GetLatestExchangeRate(fromCurrency, toCurrency);
            return Ok(rate);
        }

        [HttpPost("store-exchange-rates")]
        public async Task<IActionResult> StoreExchangeRates([FromBody] List<string> currencyPairs)
        {
            try
            {
                await _foreignExchangeService.StoreExchangeRates(currencyPairs);
                return Ok("Exchange rates stored successfully.");
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                _logger.LogError(ex, "Failed to fetch exchange rates for pairs: {Pairs}", currencyPairs);
                return BadRequest("An error occurred while storing exchange rates.");
            }
        }
    }

}
