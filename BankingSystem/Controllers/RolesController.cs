using Banking.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Banking.API.Controllers
{
    [ApiController]
    [Route("api/roles")]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet("{id}")]
        public IActionResult GetRole(int id)
        {
            var role = _roleService.GetRoleById(id);
            if (role == null)
                return NotFound();

            return Ok(role);
        }

        [HttpGet]
        public IActionResult GetAllRoles()
        {
            return Ok(_roleService.GetAllRoles());
        }

        [HttpPost("{roleId}/assign-permission/{permissionId}")]
        public IActionResult AssignPermissionToRole(int roleId, int permissionId)
        {
            try
            {
                _roleService.AssignPermissionToRole(roleId, permissionId);
                return Ok("Permission assigned to role successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to assign permission to role. Error: {ex.Message}");
            }
        }
    }
}
