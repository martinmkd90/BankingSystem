using Moq;
using Banking.API.Controllers;
using Banking.Domain.Models;
using Banking.Services.DTOs;
using Banking.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Banking.Tests
{
    public class AccountControllerTests
    {
        [Fact]
        public void Deposit_ValidAmount_ReturnsSuccessResponse()
        {
            // Arrange
            var mockAccountService = new Mock<IAccountService>();
            var controller = new AccountsController(mockAccountService.Object);
            int validAccountId = 1;
            double depositAmount = 500;

            mockAccountService.Setup(service => service.Deposit(validAccountId, depositAmount))
                              .Returns(true); // Assuming the deposit operation is successful

            // Act
            var result = controller.Deposit(validAccountId, depositAmount);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            // If you return a message from the Deposit action, you can further assert that message here.
        }

    }
}
