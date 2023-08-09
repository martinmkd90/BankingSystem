using Banking.Data.Context;
using Banking.Services.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Banking.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StockMarketController : ControllerBase
    {
        private readonly StockMarketService _stockMarketService;
        private readonly BankingDbContext _context;

        public StockMarketController(StockMarketService stockMarketService, BankingDbContext context)
        {
            _stockMarketService = stockMarketService;
            _context = context;
        }

        [HttpGet("current/{symbol}")]
        public async Task<IActionResult> GetCurrentPrice(string symbol)
        {
            var price = await _stockMarketService.GetCurrentStockPrice(symbol);
            return Ok(price);
        }

        [HttpGet("history/{symbol}")]
        public IActionResult GetHistoricalData(string symbol)
        {
            var data = _context.StockPriceHistories.Where(s => s.Symbol == symbol).OrderBy(s => s.Date).ToList();
            return Ok(data);
        }
    }

}
