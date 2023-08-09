using Banking.Data.Context;
using Banking.Domain.Models;
using Banking.Domain.Models;
using Banking.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Banking.Services.Services
{
    public class RoleService : IRoleService
    {
        private readonly BankingDbContext _context;

        public RoleService(BankingDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Role> GetAllRoles()
        {
            return _context.Roles.ToList();
        }

        public Role GetRoleById(int roleId)
        {
            return _context.Roles.Find(roleId);
        }

        public void AddRole(Role role)
        {
            _context.Roles.Add(role);
            _context.SaveChanges();
        }

        public void UpdateRole(Role role)
        {
            _context.Roles.Update(role);
            _context.SaveChanges();
        }

        public void DeleteRole(int roleId)
        {
            var role = _context.Roles.Find(roleId);
            if (role != null)
            {
                _context.Roles.Remove(role);
                _context.SaveChanges();
            }
        }

        public void AssignRoleToUser(int userId, int roleId)
        {
            var userRole = new UserRole { Id = userId, RoleId = roleId };
            _context.UserRoles.Add(userRole);
            _context.SaveChanges();
        }
        public void AssignPermissionToRole(int roleId, int permissionId)
        {
            var rolePermission = new RolePermission { RoleId = roleId, PermissionId = permissionId };
            _context.RolePermissions.Add(rolePermission);
            _context.SaveChanges();
        }

        public void RemoveRoleFromUser(int userId, int roleId)
        {
            var userRole = _context.UserRoles.FirstOrDefault(ur => ur.Id == userId && ur.RoleId == roleId);
            if (userRole != null)
            {
                _context.UserRoles.Remove(userRole);
                _context.SaveChanges();
            }
        }
    }
}
