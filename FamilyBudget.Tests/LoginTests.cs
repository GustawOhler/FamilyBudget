using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using FamilyBudget.Controllers;
using FamilyBudget.DTOs;
using FamilyBudget.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Moq;
using Xunit;

namespace FamilyBudget.Tests;

public class LoginTests
{
    const string VALID_USER = "validUser";
    const string VALID_PASSWORD = "validPassword";

    private AuthenticationController SetupTests()
    {
        var userManagerMock = new Mock<UserManager<User>>();
        userManagerMock.Setup(u => u.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync(new User { UserName = VALID_USER });
        userManagerMock.Setup(u => u.CheckPasswordAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(true);

        var configurationMock = new Mock<IConfiguration>();
        configurationMock.Setup(c => c["JWT:Secret"]).Returns("Secret");
        configurationMock.Setup(c => c["JWT:ValidIssuer"]).Returns("Issuer");
        configurationMock.Setup(c => c["JWT:ValidAudience"]).Returns("Audience");

        return new AuthenticationController(userManagerMock.Object, configurationMock.Object);
    }

    [Fact]
    public async Task Login_ValidUser_ReturnsToken()
    {
        // Arrange
        var userModel = new AuthorizationRequest
        {
            UserName = VALID_USER,
            Password = VALID_PASSWORD
        };
        var controller = SetupTests();

        // Act
        var result = await controller.Login(userModel);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var tokenData = okResult.Value as dynamic;
        Assert.NotNull(tokenData?.token);
        Assert.IsType<string>(tokenData?.token);
        Assert.IsType<DateTime>(tokenData?.expiration);
        Assert.IsType<int>(tokenData?.id);
    }
}