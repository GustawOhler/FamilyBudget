using FamilyBudget.Backend.DTOs;
using FamilyBudget.Controllers;
using FamilyBudget.DTOs;
using FamilyBudget.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;

namespace FamilyBudget.Tests;

public class LoginTests
{
    [Fact]
    public async Task Login_ValidUser_ReturnsCorrectResponse()
    {
        // Arrange
        var userModel = new AuthorizationRequest
        {
            UserName = DependencyInjectionMocks.VALID_USER,
            Password = DependencyInjectionMocks.VALID_PASSWORD
        };
        var controller = new AuthenticationController(DependencyInjectionMocks.GetUserManagerMock().Object, DependencyInjectionMocks.GetConfigurationForJWTMock().Object);

        // Act
        var result = await controller.Login(userModel);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var tokenData = okResult.Value as AuthorizationResponse;
        Assert.NotNull(okResult.Value);
        Assert.NotNull(tokenData?.Token);
        Assert.IsType<string>(tokenData?.Token);
        Assert.IsType<DateTime>(tokenData?.Expiration);
        Assert.IsType<int>(tokenData?.Id);
    }
}