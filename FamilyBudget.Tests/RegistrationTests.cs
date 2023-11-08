using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using FamilyBudget.Backend.DTOs;
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

public class RegistrationTests
{
    [Fact]
    public async Task Register_ValidUser_ReturnsSuccess()
    {
        // Arrange
        var userModel = new AuthorizationRequest
        {
            UserName = "OtherUser",
            Password = DependencyInjectionMocks.VALID_PASSWORD
        };

        var userManagerMock = DependencyInjectionMocks.GetUserManagerMock();
        userManagerMock.Setup(u => u.CreateAsync(It.IsAny<User>(), userModel.Password))
            .ReturnsAsync(IdentityResult.Success);

        var controller = new AuthenticationController(userManagerMock.Object, DependencyInjectionMocks.GetConfigurationForJWTMock().Object);

        // Act
        var result = await controller.Register(userModel);

        // Assert
        var response = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(result);
        Assert.NotNull(response.Value);
    }
}