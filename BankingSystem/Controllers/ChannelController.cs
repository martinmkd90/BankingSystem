using Microsoft.AspNetCore.Mvc;
using Banking.Data.Context;
using Microsoft.EntityFrameworkCore;
using Banking.Domain.Models;

namespace Banking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChannelController : ControllerBase
    {
        private readonly BankingDbContext _context;

        public ChannelController(BankingDbContext context)
        {
            _context = context;
        }

        // GET: api/channels
        [HttpGet]
        public IActionResult GetAllChannels()
        {
            var channels = _context.Channels.ToList();
            return Ok(channels);
        }

        // GET: api/channels/{id}
        [HttpGet("{id}")]
        public IActionResult GetChannelById(int id)
        {
            var channel = _context.Channels.Find(id);
            if (channel == null)
            {
                return NotFound();
            }
            return Ok(channel);
        }

        // POST: api/channels
        [HttpPost]
        public IActionResult CreateChannel(ProductChannel channel)
        {
            _context.Channels.Add(channel);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetChannelById), new { id = channel.ProductChannelID }, channel);
        }

        // PUT: api/channels/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateChannel(int id, ProductChannel channel)
        {
            if (id != channel.ProductChannelID)
            {
                return BadRequest();
            }

            _context.Entry(channel).State = EntityState.Modified;
            _context.SaveChanges();
            return NoContent();
        }

        // DELETE: api/channels/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteChannel(int id)
        {
            var channel = _context.Channels.Find(id);
            if (channel == null)
            {
                return NotFound();
            }

            _context.Channels.Remove(channel);
            _context.SaveChanges();
            return NoContent();
        }

    }
}
