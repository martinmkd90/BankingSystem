namespace Banking.API.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using global::Banking.Data.Context;
    using Microsoft.AspNetCore.Builder;
    using static System.Net.Mime.MediaTypeNames;
    using global::Banking.Domain.Models;
    using Microsoft.EntityFrameworkCore;

    namespace Banking.API.Controllers
    {
        [Route("api/[controller]")]
        [ApiController]
        public class LoanApplicationsController : ControllerBase
        {
            private readonly BankingDbContext _context;

            public LoanApplicationsController(BankingDbContext context)
            {
                _context = context;
            }

            [HttpPost("applications")]
            public IActionResult CreateApplication([FromBody] LoanApplication application)
            {
                _context.LoanApplications.Add(application);
                _context.SaveChanges();
                return CreatedAtAction(nameof(GetApplication), new { id = application.LoanApplicationID }, application);
            }

            [HttpGet("applications/{id}")]
            public IActionResult GetApplication(int id)
            {
                var application = _context.LoanApplications.Find(id);
                if (application == null)
                {
                    return NotFound();
                }
                return Ok(application);
            }

            [HttpPut("applications/{id}")]
            public IActionResult UpdateApplication(int id, [FromBody] LoanApplication application)
            {
                if (id != application.LoanApplicationID)
                {
                    return BadRequest();
                }
                _context.Entry(application).State = EntityState.Modified;
                _context.SaveChanges();
                return NoContent();
            }

            [HttpDelete("applications/{id}")]
            public IActionResult DeleteApplication(int id)
            {
                var application = _context.LoanApplications.Find(id);
                if (application == null)
                {
                    return NotFound();
                }
                _context.LoanApplications.Remove(application);
                _context.SaveChanges();
                return NoContent();
            }
        }
    }

}
