using FamilyBudgetApplication.Auth;
using FamilyBudgetDomain.Models;

namespace FamilyBudget.Application.Tests.Auth
{
    public class AuthorizationVerifierTests
    {
        private readonly AuthorizationVerifier _authorizationVerifier;
        public AuthorizationVerifierTests()
        {
            _authorizationVerifier = new AuthorizationVerifier();
        }

        [Fact]
        public void IsAuthorizedForUser_CorrectId_ReturnsTrue()
        {
            // Arrange
            var validUser = ReadyObjects.Users.ValidUser;
            int validId = ReadyObjects.Users.VALID_USER_ID;


            // Act
            bool result = _authorizationVerifier.IsAuthorizedForUser(validUser, validId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsAuthorizedForUser_IncorrectId_ReturnsFalse()
        {
            // Arrange
            var validUser = ReadyObjects.Users.ValidUser;
            int invalidId = 99;


            // Act
            bool result = _authorizationVerifier.IsAuthorizedForUser(validUser, invalidId);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsAuthorizedForUser_IncorrectUsername_ReturnsFalse()
        {
            // Arrange
            var validUser = ReadyObjects.Users.ValidUser;
            string invalidUsername = "Invalid";


            // Act
            bool result = _authorizationVerifier.IsAuthorizedForUser(validUser, invalidUsername);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsAuthorizedForUser_CorrectUsername_ReturnsTrue()
        {
            // Arrange
            var validUser = ReadyObjects.Users.ValidUser;
            string validUsername = ReadyObjects.Users.VALID_USERNAME;


            // Act
            bool result = _authorizationVerifier.IsAuthorizedForUser(validUser, validUsername);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsBudgetMember_UserIsNotMember_ReturnsFalse()
        {
            // Arrange
            var user = ReadyObjects.Users.ValidUser;
            var differentUser = new User();
            var budget = new Budget()
            {
                Name = "Test",
                Members = new List<User>() { differentUser }
            };

            // Act
            bool result = _authorizationVerifier.IsBudgetMember(user, budget);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsBudgetMember_UserIsMember_ReturnsTrue()
        {
            // Arrange
            var user = ReadyObjects.Users.ValidUser;
            var budget = new Budget()
            {
                Name = "Test",
                Admin = user,
                Members = new List<User>() { user }
            };

            // Act
            bool result = _authorizationVerifier.IsBudgetMember(user, budget);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsAdmin_UserIsNotAdmin_ReturnsFalse()
        {
            // Arrange
            var user = ReadyObjects.Users.ValidUser;
            var differentUser = new User();
            var budget = new Budget()
            {
                Name = "Test",
                Admin = differentUser,
                Members = new List<User>() { differentUser, user }
            };

            // Act
            bool result = _authorizationVerifier.IsAdmin(user, budget);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsAdmin_UserIsAdmin_ReturnsTrue()
        {
            // Arrange
            var user = ReadyObjects.Users.ValidUser;
            var budget = new Budget()
            {
                Name = "Test",
                Admin = user,
                Members = new List<User>() { user }
            };

            // Act
            bool result = _authorizationVerifier.IsAdmin(user, budget);

            // Assert
            Assert.True(result);
        }
    }
}