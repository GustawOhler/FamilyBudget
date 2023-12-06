using System;
using System.IdentityModel.Tokens.Jwt;
using FamilyBudget.Application.Tests.Mocks;
using FamilyBudgetApplication.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Moq;

namespace FamilyBudget.Application.Tests.Auth
{
    public class JwtTokenGeneratorTests
    {
        private readonly IConfiguration _mockConfiguration;
        private readonly JwtTokenGenerator _tokenGenerator;

        public JwtTokenGeneratorTests()
        {
            _mockConfiguration = DependencyInjectionMocks.GetConfigurationForJWTMock().Object;

            _tokenGenerator = new JwtTokenGenerator(_mockConfiguration);
        }

        [Fact]
        public void GenerateToken_ValidUsername_ReturnsValidJwtToken()
        {
            // Arrange
            string username = "testUser";

            // Act
            var token = _tokenGenerator.GenerateToken(username);

            // Assert
            Assert.NotNull(token);
            Assert.IsType<JwtSecurityToken>(token);
            Assert.Equal(token.Issuer, _mockConfiguration["JWT:ValidIssuer"]);
        }

        [Fact]
        public void TokenToString_ValidJwtToken_ReturnsString()
        {
            // Arrange
            string username = "testUser";
            var token = _tokenGenerator.GenerateToken(username);

            // Act
            var tokenString = _tokenGenerator.TokenToString(token);

            // Assert
            Assert.NotNull(tokenString);
            Assert.IsType<string>(tokenString);
        }

        [Fact]
        public void TokenToString_InvalidTokenType_ThrowsArgumentException()
        {
            // Arrange
            var invalidToken = new Mock<SecurityToken>();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _tokenGenerator.TokenToString(invalidToken.Object));
        }
    }
}
