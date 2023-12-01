using FamilyBudgetDomain.Models;
using FamilyBudgetDomain.Exceptions;
using Microsoft.AspNetCore.Identity;
using Moq;
using FamilyBudget.Application.Tests.Mocks;
using Application.Auth;
using System.Diagnostics;

namespace FamilyBudget.Application.Tests.Auth
{
    public class UserRegistrantTests
    {
        private readonly Mock<UserManager<User>> _mockUserManager;

        public UserRegistrantTests()
        {
            _mockUserManager = DependencyInjectionMocks.GetUserManagerMock();
        }
        [Fact]
        public async Task RegisterUser_UserDoesNotExist_CreatesUser()
        {
            // Arrange
            string username = "newUser";
            string password = "newPassword";
            _mockUserManager.Setup(u => u.CreateAsync(It.IsAny<User>(), password))
                .ReturnsAsync(IdentityResult.Success);

            var userRegistrant = new UserRegistrant(_mockUserManager.Object);

            // Act
            var result = await userRegistrant.RegisterUser(username, password);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(username, result.UserName);
        }

        [Fact]
        public async Task RegisterUser_UserAlreadyExists_ThrowsRegistrationException()
        {
            // Arrange
            string alreadyExistingUsername = ReadyObjects.Users.VALID_USERNAME;
            string password = "newPassword";
            var userRegistrant = new UserRegistrant(_mockUserManager.Object);

            // Act & Assert
            await Assert.ThrowsAsync<RegistrationException>(
                () => userRegistrant.RegisterUser(alreadyExistingUsername, "anyPassword"));
        }

        [Fact]
        public async Task RegisterUser_FailureInUserCreation_ThrowsInnerQuietException()
        {
            // Arrange
            string username = "newUser";
            string password = "newPassword";
            var identityErrors = new List<IdentityError> { new IdentityError { Description = "Test error" } };
            _mockUserManager.Setup(x => x.CreateAsync(It.IsAny<User>(), password))
                           .ReturnsAsync(IdentityResult.Failed(identityErrors.ToArray()));

            var userRegistrant = new UserRegistrant(_mockUserManager.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InnerQuietException>(
                () => userRegistrant.RegisterUser(username, password));

            // Additional assert to check if the error messages are passed correctly
            Console.WriteLine(exception.Message);
            Assert.Contains("Test error", exception.Message);
        }
    }
}
