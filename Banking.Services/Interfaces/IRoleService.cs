using Banking.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Services.Interfaces
{
    public interface IRoleService
    {
        IEnumerable<Role> GetAllRoles();
        Role GetRoleById(int roleId);
        void AddRole(Role role);
        void UpdateRole(Role role);
        void DeleteRole(int roleId);
        void AssignRoleToUser(int userId, int roleId);
        void RemoveRoleFromUser(int userId, int roleId);
        void AssignPermissionToRole(int roleId, int permissionId);
    }
}
