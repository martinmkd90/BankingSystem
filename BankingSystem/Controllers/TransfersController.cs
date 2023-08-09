using Banking.Services.DTOs;
using Banking.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Banking.API.Controllers
{
    [ApiController]
    [Route("api/transfers")]
    public class TransfersController : ControllerBase
    {
        private readonly ITransferService _transferService;

        public TransfersController(ITransferService transferService)
        {
            _transferService = transferService;
        }

        [HttpPost]
        public async Task<IActionResult> MakeTransfer([FromBody] TransferDto transferDto)
        {
            try
            {
                var result = _transferService.MakeTransfer(transferDto.FromAccountId, transferDto.ToAccountId, transferDto.Amount);
                if (result != null)
                    return Ok(new { Message = "Transfer successful." });
                else
                    return BadRequest(new { Message = "Transfer failed." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("{accountId}")]
        public IActionResult GetTransfersForAccount(int accountId)
        {
            return Ok(_transferService.GetTransfersForAccount(accountId));
        }
    }
}
