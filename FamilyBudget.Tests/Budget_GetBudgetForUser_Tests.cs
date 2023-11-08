using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using FamilyBudget;
using FamilyBudget.Controllers;
using FamilyBudget.DTOs;
using FamilyBudget.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

public class Budget_GetBudgetForUser_Tests
{
    public Mock<FamilyBudgetDbContext> GetDbContextMock()
    {
        [Fact]
        public async Task GetBudgetsForUser_ValidUser_ReturnsBudgets()
        {
            // Arrange
            var userManagerMock = DependencyInjectionMocks.GetUserManagerMock();
            var dbContextMock = new Mock<FamilyBudgetDbContext>();
            var configurationMock = new Mock<IConfiguration>();
            var mapperMock = new Mock<IMapper>();
            var controller = new BudgetController(userManagerMock.Object, dbContextMock.Object, configurationMock.Object, mapperMock.Object);

            var budgetSearchRequest = new BudgetSearchRequest
            {
                Limit = 10,
                Offset = 0,
                Name = "BudgetName",
            };
            var exampleUser = new User()
            {
                Id = 1,
                UserName = "ValidUser"
            };
            var exampleBudget = new Budget(){
                Id = 1,
                Name="123",
                Admin=exampleUser,
                Members=new List<User>(){exampleUser},
                Balance=5
            };

            dbContextMock.Setup(db => db.Users.SingleOrDefaultAsync())
                .ReturnsAsync(exampleUser);
            dbContextMock.Setup(db => db.Budgets.ToListAsync())
                .ReturnsAsync(exampleBudget);
        }
    }
