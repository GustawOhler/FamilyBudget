using FamilyBudget.Application.Tests.Mocks;
using FamilyBudgetApplication.Auth;
using FamilyBudgetDomain.Exceptions;
using FamilyBudgetDomain.Models;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace FamilyBudget.Application.Tests.Auth
{
    public class SimpleAuthenticatorTests
    {
        private readonly Mock<UserManager<User>> _mockUserManager;

        public SimpleAuthenticatorTests()
        {
            _mockUserManager = DependencyInjectionMocks.GetUserManagerMock();
        }

        [Fact]
        public async Task AuthenticateUser_InvalidUsername_ThrowsAuthenticationException()
        {
            // Arrange
            string invalidUsername = "invalid";
            string password = "anyPassword";
            var simpleAuthenticator = new SimpleAuthenticator(_mockUserManager.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ResourceNotFoundException>(
                () => simpleAuthenticator.AuthenticateUser(invalidUsername, password));
        }

        [Fact]
        public async Task AuthenticateUser_InvalidPassword_ThrowsAuthenticationException()
        {
            // Arrange
            string password = "anyPassword";
            var simpleAuthenticator = new SimpleAuthenticator(_mockUserManager.Object);

            // Act & Assert
            await Assert.ThrowsAsync<AuthenticationException>(
                () => simpleAuthenticator.AuthenticateUser(ReadyObjects.Users.VALID_USERNAME, password));
        }

        [Fact]
        public async Task AuthenticateUser_ValidCredentials_ReturnsUser()
        {
            // Arrange
            var simpleAuthenticator = new SimpleAuthenticator(_mockUserManager.Object);

            // Act
            var result = await simpleAuthenticator.AuthenticateUser(ReadyObjects.Users.VALID_USERNAME, ReadyObjects.Users.VALID_PASSWORD);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<User>(result);
            Assert.Equal(ReadyObjects.Users.VALID_USERNAME, result.UserName);
        }
    }
}