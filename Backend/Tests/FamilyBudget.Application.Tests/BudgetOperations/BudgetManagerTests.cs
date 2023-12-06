using AutoMapper;
using FamilyBudget.Application.Tests.Mocks;
using FamilyBudget.Application.Tests.ReadyObjects;
using FamilyBudgetApplication.BudgetOperations;
using FamilyBudgetApplication.BudgetOperations.DTOs;
using FamilyBudgetApplication.Interfaces;
using FamilyBudgetDomain.Exceptions;
using FamilyBudgetDomain.Interfaces.Database;
using FamilyBudgetDomain.Models;
using FamilyBudgetInfrastructure.Database.Specifications;
using Microsoft.AspNetCore.Identity;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace FamilyBudgetApplication.Tests.BudgetOperations
{
    public class BudgetManagerTests
    {
        private readonly Mock<IRepository<Budget>> _mockBudgetRepo;
        private readonly Mock<IRepository<Category>> _mockCategoryRepo;
        private readonly Mock<IRepository<BalanceChange>> _mockBalanceChangeRepo;
        private readonly Mock<IRepository<User>> _mockUserRepo;
        private readonly Mock<UserManager<User>> _mockUserManager;
        private readonly Mock<IAuthorizationVerifier> _mockAuthVerifier;
        private readonly Mock<IMapper> _mockMapper;

        public BudgetManagerTests()
        {
            _mockBudgetRepo = new Mock<IRepository<Budget>>();
            _mockCategoryRepo = new Mock<IRepository<Category>>();
            _mockBalanceChangeRepo = new Mock<IRepository<BalanceChange>>();
            _mockUserRepo = new Mock<IRepository<User>>();
            _mockUserManager = DependencyInjectionMocks.GetUserManagerMock();
            _mockAuthVerifier = new Mock<IAuthorizationVerifier>();
            _mockMapper = new Mock<IMapper>();
        }

        [Fact]
        public async Task AddBudgetMembers_UserIsAdmin_AddsMembers()
        {
            // Arrange
            var testBudget = new Budget()
            {
                Name = "Test",
                Admin = Users.ValidUser,
                Members = new List<User> { Users.ValidUser }
            };
            var newMember = new User()
            {
                Id = 2,
                UserName = "Test"
            };
            var input = new NewBudgetMembersInput
            {
                BudgetId = 1,
                RequestingUserName = Users.VALID_USERNAME,
                UserIds = new List<int> { 2 }
            };

            _mockBudgetRepo.Setup(b => b.GetSingleOrDefault(It.IsAny<ISpecification<Budget>>())).ReturnsAsync(testBudget);
            _mockUserRepo.Setup(u => u.List(It.IsAny<ISpecification<User>>())).ReturnsAsync(new List<User> { newMember });
            _mockAuthVerifier.Setup(a => a.IsAdmin(It.IsAny<User>(), It.IsAny<Budget>())).Returns(true);

            var budgetManager = new BudgetManager(_mockBudgetRepo.Object, _mockUserManager.Object, _mockAuthVerifier.Object,
                                                    _mockCategoryRepo.Object, _mockBalanceChangeRepo.Object, _mockUserRepo.Object, _mockMapper.Object);

            // Act
            var result = await budgetManager.AddBudgetMembers(input);

            // Assert
            Assert.True(testBudget.Members.Contains(newMember));
        }

        [Fact]
        public async Task AddBudgetMembers_UserIsNotAdmin_ThrowsAuthorizationException()
        {
            // Arrange
            var testBudget = new Budget()
            {
                Name = "Test",
                Admin = Users.ValidUser,
                Members = new List<User> { Users.ValidUser }
            };
            var newMember = new User()
            {
                Id = 2,
                UserName = "Test"
            };
            var input = new NewBudgetMembersInput
            {
                BudgetId = 1,
                RequestingUserName = Users.VALID_USERNAME,
                UserIds = new List<int> { 2 }
            };

            _mockBudgetRepo.Setup(b => b.GetSingleOrDefault(It.IsAny<ISpecification<Budget>>())).ReturnsAsync(testBudget);
            _mockUserRepo.Setup(u => u.List(It.IsAny<ISpecification<User>>())).ReturnsAsync(new List<User> { newMember });
            _mockAuthVerifier.Setup(a => a.IsAdmin(It.IsAny<User>(), It.IsAny<Budget>())).Returns(false);

            var budgetManager = new BudgetManager(_mockBudgetRepo.Object, _mockUserManager.Object, _mockAuthVerifier.Object,
                                                    _mockCategoryRepo.Object, _mockBalanceChangeRepo.Object, _mockUserRepo.Object, _mockMapper.Object);


            // Act & Assert
            await Assert.ThrowsAsync<AuthorizationException>(
                () => budgetManager.AddBudgetMembers(input));
        }

        [Fact]
        public async Task CreateBudget_ValidInput_CreatesBudget()
        {
            // Arrange
            var input = new NewBudgetInput
            {
                Name = "Test",
                AdminUsername = Users.VALID_USERNAME,
                Balance = 10
            };
            var budget = new Budget
            {
                Name = "Test"
            };

            _mockMapper.Setup(m => m.Map<Budget>(It.IsAny<NewBudgetInput>())).Returns(budget);

            var budgetManager = new BudgetManager(_mockBudgetRepo.Object, _mockUserManager.Object, _mockAuthVerifier.Object,
                                                    _mockCategoryRepo.Object, _mockBalanceChangeRepo.Object, _mockUserRepo.Object, _mockMapper.Object);

            // Act
            var result = await budgetManager.CreateBudget(input);

            // Assert
            Assert.IsType<Budget>(result);
            Assert.Same(budget.Admin, Users.ValidUser);
            Assert.True(budget.Members.Contains(Users.ValidUser));
        }
    }
}
