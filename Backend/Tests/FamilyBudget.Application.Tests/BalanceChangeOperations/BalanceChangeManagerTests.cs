using AutoMapper;
using FamilyBudget.Application.Tests.Mocks;
using FamilyBudget.Application.Tests.ReadyObjects;
using FamilyBudgetApplication.BalanceChangeOperations;
using FamilyBudgetApplication.BalanceChangeOperations.DTOs;
using FamilyBudgetApplication.Interfaces;
using FamilyBudgetDomain.Exceptions;
using FamilyBudgetDomain.Interfaces.Database;
using FamilyBudgetDomain.Models;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace FamilyBudget.Application.Tests.BalanceChangeOperations
{
    public class BalanceChangeManagerTests
    {
        private readonly Mock<IRepository<Budget>> _mockBudgetRepo;
        private readonly Mock<IRepository<Category>> _mockCategoryRepo;
        private readonly Mock<IRepository<BalanceChange>> _mockBalanceChangeRepo;
        private readonly Mock<IRepository<User>> _mockUserRepo;
        private readonly Mock<UserManager<User>> _mockUserManager;
        private readonly Mock<IAuthorizationVerifier> _mockAuthVerifier;
        private readonly Mock<IMapper> _mockMapper;

        public BalanceChangeManagerTests()
        {
            _mockBudgetRepo = new Mock<IRepository<Budget>>();
            _mockCategoryRepo = new Mock<IRepository<Category>>();
            _mockBalanceChangeRepo = new Mock<IRepository<BalanceChange>>();
            _mockUserRepo = new Mock<IRepository<User>>();
            _mockUserManager = DependencyInjectionMocks.GetUserManagerMock();
            _mockAuthVerifier = new Mock<IAuthorizationVerifier>();
            _mockMapper = new Mock<IMapper>();
        }

        public async Task CreateBalanceChange_ValidInput_ReturnsBalanceChange()
        {
            // Arrange
            float balanceChangeAmount = 25;
            var testBudget = new Budget()
            {
                Name = "Test",
                Admin = Users.ValidUser,
                Members = new List<User> { Users.ValidUser },
                Balance = 0
            };
            var newBalanceChange = new BalanceChange()
            {
                Amount = balanceChangeAmount
            };
            var balanceChangeInput = new NewBalanceChangeInput
            {
                BudgetId = 1,
                CategoryId = 1
            };
            var category = new Category
            {
                Name = "Test"
            };

            _mockBudgetRepo.Setup(b => b.GetSingleOrDefault(It.IsAny<ISpecification<Budget>>())).ReturnsAsync(testBudget);
            _mockAuthVerifier.Setup(a => a.IsBudgetMember(It.IsAny<User>(), It.IsAny<Budget>())).Returns(true);
            _mockMapper.Setup(m => m.Map<BalanceChange>(It.IsAny<NewBalanceChangeInput>())).Returns(newBalanceChange);
            _mockCategoryRepo.Setup(c => c.GetSingleOrDefault(It.IsAny<ISpecification<Category>>())).ReturnsAsync(category);

            var manager = new BalanceChangeManager(_mockBudgetRepo.Object, _mockUserManager.Object, _mockAuthVerifier.Object, _mockCategoryRepo.Object, _mockBalanceChangeRepo.Object, _mockMapper.Object);

            //Act
            var balanceChange = await manager.CreateBalanceChange(balanceChangeInput);

            //Assert
            Assert.Same(balanceChange.Budget, testBudget);
            Assert.Same(balanceChange.Category, category);
            Assert.Equal(testBudget.Balance, balanceChangeAmount);
        }

        public async Task CreateBalanceChange_UserIsNotMember_ThrowsException()
        {
            // Arrange
            var testBudget = new Budget()
            {
                Name = "Test",
                Admin = Users.ValidUser,
                Members = new List<User> { Users.ValidUser },
                Balance = 0
            };
            var balanceChangeInput = new NewBalanceChangeInput
            {
                BudgetId = 1,
                CategoryId = 1
            };

            _mockBudgetRepo.Setup(b => b.GetSingleOrDefault(It.IsAny<ISpecification<Budget>>())).ReturnsAsync(testBudget);
            _mockAuthVerifier.Setup(a => a.IsBudgetMember(It.IsAny<User>(), It.IsAny<Budget>())).Returns(false);

            var manager = new BalanceChangeManager(_mockBudgetRepo.Object, _mockUserManager.Object, _mockAuthVerifier.Object, _mockCategoryRepo.Object, _mockBalanceChangeRepo.Object, _mockMapper.Object);

            //Act
            await Assert.ThrowsAsync<AuthorizationException>(() => manager.CreateBalanceChange(balanceChangeInput));
        }
    }
}