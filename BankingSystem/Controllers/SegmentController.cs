using Microsoft.AspNetCore.Mvc;
using global::Banking.Data.Context;
using global::Banking.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Banking.API.Controllers
{
    namespace Banking.API.Controllers
    {
        [Route("api/[controller]")]
        [ApiController]
        public class SegmentsController : ControllerBase
        {
            private readonly BankingDbContext _context;

            public SegmentsController(BankingDbContext context)
            {
                _context = context;
            }

            // GET: api/segments
            [HttpGet]
            public IActionResult GetAllSegments()
            {
                var segments = _context.Segments.ToList();
                return Ok(segments);
            }

            // GET: api/segments/{id}
            [HttpGet("{id}")]
            public IActionResult GetSegmentById(int id)
            {
                var segment = _context.Segments.Find(id);
                if (segment == null)
                {
                    return NotFound();
                }
                return Ok(segment);
            }

            // POST: api/segments
            [HttpPost]
            public IActionResult CreateSegment(Segment segment)
            {
                _context.Segments.Add(segment);
                _context.SaveChanges();
                return CreatedAtAction(nameof(GetSegmentById), new { id = segment.SegmentID }, segment);
            }

            // PUT: api/segments/{id}
            [HttpPut("{id}")]
            public IActionResult UpdateSegment(int id, Segment segment)
            {
                if (id != segment.SegmentID)
                {
                    return BadRequest();
                }

                _context.Entry(segment).State = EntityState.Modified;
                _context.SaveChanges();
                return NoContent();
            }

            // DELETE: api/segments/{id}
            [HttpDelete("{id}")]
            public IActionResult DeleteSegment(int id)
            {
                var segment = _context.Segments.Find(id);
                if (segment == null)
                {
                    return NotFound();
                }

                _context.Segments.Remove(segment);
                _context.SaveChanges();
                return NoContent();
            }

        }
    }

}
