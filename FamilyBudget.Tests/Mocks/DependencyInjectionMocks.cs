using FamilyBudget.Models;
using Microsoft.AspNetCore.Identity;
using Moq;
using Microsoft.Extensions.Configuration;

public static class DependencyInjectionMocks
{
    public const string VALID_USER = "validUser";
    public const string VALID_PASSWORD = "validPassword";

    public static Mock<UserManager<User>> GetUserManagerMock()
    {
        var userManagerMock = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
        userManagerMock.Setup(u => u.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync((User)null);
        userManagerMock.Setup(u => u.FindByNameAsync(VALID_USER))
            .ReturnsAsync(new User { Id = 1, UserName = VALID_USER });
        userManagerMock.Setup(u => u.CheckPasswordAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(false);
        userManagerMock.Setup(u => u.CheckPasswordAsync(It.IsAny<User>(), VALID_PASSWORD))
            .ReturnsAsync(true);
        return userManagerMock;
    }

    public static Mock<IConfiguration> GetConfigurationForJWTMock()
    {
        var configurationMock = new Mock<IConfiguration>();
        configurationMock.Setup(c => c["JWT:Secret"]).Returns("UXH8XxQQp2NQ7enhG0wMdqqcG8uIn1bU");
        configurationMock.Setup(c => c["JWT:ValidIssuer"]).Returns("Issuer");
        configurationMock.Setup(c => c["JWT:ValidAudience"]).Returns("Audience");
        return configurationMock;
    }
}