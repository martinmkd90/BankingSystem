using Moq;
using Banking.API.Controllers;
using Banking.Domain.Models;
using Banking.Services.DTOs;
using Banking.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Banking.Tests
{
    public class LoansControllerTests
    {
        [Fact]
        public void ApplyForLoan_ValidApplication_ReturnsOk()
        {
            // Arrange
            var mockLoanService = new Mock<ILoanService>();
            var controller = new LoansController(mockLoanService.Object);
            var applicationDto = new LoanApplicationDto
            {
                UserId = 1,
                AmountRequested = 1000,
                LoanType = new LoanType { Id = 1, Name = "Personal", InterestRate = 5 },
                DurationInMonths = 12
            };

            mockLoanService.Setup(service => service.ApplyForLoan(applicationDto))
                           .Returns(new Loan { Id = 1, UserId = 1, Amount = 1000 });

            // Act
            var result = controller.ApplyForLoan(applicationDto);

            // Assert
            Assert.NotNull(result);
            // Add more assertions based on your expected result
        }

        [Fact]
        public void RejectLoan_NonExistentLoan_ReturnsNotFound()
        {
            // Arrange
            var mockLoanService = new Mock<ILoanService>();
            var controller = new LoansController(mockLoanService.Object);
            int nonExistentLoanId = 999;

            mockLoanService.Setup(service => service.RejectLoan(nonExistentLoanId))
                           .Returns((Loan)null);

            // Act
            var result = controller.RejectLoan(nonExistentLoanId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void GetUserLoans_ValidUserId_ReturnsLoans()
        {
            // Arrange
            var mockLoanService = new Mock<ILoanService>();
            var controller = new LoansController(mockLoanService.Object);
            int validUserId = 1;

            mockLoanService.Setup(service => service.GetUserLoans(validUserId))
               .Returns(Task.FromResult<IEnumerable<Loan>>(new List<Loan> { new Loan { Id = 1, UserId = 1, Amount = 1000 } }));


            // Act
            var result = controller.GetUserLoans(validUserId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var loans = Assert.IsType<List<Loan>>(okResult.Value);
            Assert.Single(loans);
        }

    }
}
