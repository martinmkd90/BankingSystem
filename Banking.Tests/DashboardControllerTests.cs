using Banking.API.Controllers;
using Banking.Domain.Models;
using Banking.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Banking.Tests
{
    public class DashboardControllerTests
    {
        private readonly Mock<IDashboardService> _mockDashboardService;
        private readonly DashboardController _controller;

        public DashboardControllerTests()
        {
            _mockDashboardService = new Mock<IDashboardService>();
            _controller = new DashboardController(_mockDashboardService.Object);
        }

        [Fact]
        public async Task GetUserDashboard_ReturnsValidDashboard()
        {
            // Arrange
            int testUserId = 1;
            var expectedDashboard = new Dashboard
            {
                User = new User { Id = testUserId, Username = "testUser" }
                // ... other properties
            };
            _mockDashboardService.Setup(service => service.GetUserDashboard(testUserId))
                .ReturnsAsync(expectedDashboard);

            // Act
            var result = await _controller.GetUserDashboard(testUserId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedDashboard = Assert.IsType<Dashboard>(okResult.Value);
            Assert.Equal(testUserId, returnedDashboard.User.Id);
        }
    }

}
