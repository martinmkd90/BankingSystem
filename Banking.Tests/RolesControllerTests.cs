using Banking.API.Controllers;
using Banking.Domain.Models;
using Banking.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Banking.Tests
{
    public class RolesControllerTests
    {
        [Fact]
        public void AssignPermissionToRole_ValidIds_ReturnsOk()
        {
            // Arrange
            var mockRoleService = new Mock<IRoleService>();
            var controller = new RolesController(mockRoleService.Object);
            int validRoleId = 1;
            int validPermissionId = 2;

            mockRoleService.Setup(service => service.AssignPermissionToRole(validRoleId, validPermissionId));

            // Act
            var result = controller.AssignPermissionToRole(validRoleId, validPermissionId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }
    }
}
