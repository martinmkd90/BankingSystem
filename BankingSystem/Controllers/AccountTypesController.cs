using Banking.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Banking.API.Controllers
{
    [ApiController]
    [Route("api/account-types")]
    public class AccountTypesController : ControllerBase
    {
        private readonly IAccountTypeService _accountTypeService;

        public AccountTypesController(IAccountTypeService accountTypeService)
        {
            _accountTypeService = accountTypeService;
        }

        [HttpGet]
        public IActionResult GetAllAccountTypes()
        {
            return Ok(_accountTypeService.GetAllAccountTypes());
        }

        [HttpGet("{id}")]
        public IActionResult GetAccountType(int id)
        {
            var accountType = _accountTypeService.GetAccountType(id);
            if (accountType == null)
                return NotFound();

            return Ok(accountType);
        }
    }

}
