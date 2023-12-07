using FamilyBudgetDomain.Models;
using Microsoft.AspNetCore.Identity;
using Moq;
using Microsoft.Extensions.Configuration;
using FamilyBudget.Application.Tests.ReadyObjects;

namespace FamilyBudget.Application.Tests.Mocks
{
    public static class DependencyInjectionMocks
    {
        public static Mock<UserManager<User>> GetUserManagerMock()
        {
            var userManagerMock = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
            userManagerMock.Setup(u => u.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync((User)null);
            userManagerMock.Setup(u => u.FindByNameAsync(Users.VALID_USERNAME))
                .ReturnsAsync(Users.ValidUser);
            userManagerMock.Setup(u => u.CheckPasswordAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(false);
            userManagerMock.Setup(u => u.CheckPasswordAsync(It.IsAny<User>(), Users.VALID_PASSWORD))
                .ReturnsAsync(true);
            return userManagerMock;
        }

        public static Mock<IConfiguration> GetConfigurationForJWTMock()
        {
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(c => c["JWT_Secret"]).Returns("UXH8XxQQp2NQ7enhG0wMdqqcG8uIn1bU");
            configurationMock.Setup(c => c["JWT:ValidIssuer"]).Returns("Issuer");
            configurationMock.Setup(c => c["JWT:ValidAudience"]).Returns("Audience");
            return configurationMock;
        }
    }
}