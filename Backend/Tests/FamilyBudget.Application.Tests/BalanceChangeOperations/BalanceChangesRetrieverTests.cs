using FamilyBudget.Application.Tests.Mocks;
using FamilyBudget.Application.Tests.ReadyObjects;
using FamilyBudgetApplication.BalanceChangeOperations;
using FamilyBudgetApplication.BalanceChangeOperations.DTOs;
using FamilyBudgetApplication.Interfaces;
using FamilyBudgetDomain.Exceptions;
using FamilyBudgetDomain.Interfaces.Database;
using FamilyBudgetDomain.Models;
using FamilyBudgetInfrastructure.Database.Specifications;
using Microsoft.AspNetCore.Identity;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace FamilyBudget.Application.Tests.BalanceChangeOperations
{
    public class BalanceChangesRetrieverTests
    {
        private readonly Mock<IRepository<BalanceChange>> _mockBalanceChangeRepo;
        private readonly Mock<IRepository<Budget>> _mockBudgetRepo;
        private readonly Mock<UserManager<User>> _mockUserManager;
        private readonly Mock<IAuthorizationVerifier> _mockAuthVerifier;

        public BalanceChangesRetrieverTests()
        {
            _mockBalanceChangeRepo = new Mock<IRepository<BalanceChange>>();
            _mockBudgetRepo = new Mock<IRepository<Budget>>();
            _mockUserManager = DependencyInjectionMocks.GetUserManagerMock();
            _mockAuthVerifier = new Mock<IAuthorizationVerifier>();
        }

        [Fact]
        public async Task GetBalanceChangesForBudget_AuthorizedUser_ReturnsBalanceChanges()
        {
            // Arrange
            var input = new BalanceChangesForBudgetInput
            {
                BudgetId = 1,
                PageNumber = 0,
                PageSize = 20,
                RequestingUserName = Users.VALID_USERNAME
            };
            var budget = new Budget
            {
                Name = "Test"
            };
            var balanceChanges = new List<BalanceChange>
            {
                new BalanceChange(),
                new BalanceChange()
            };

            _mockBudgetRepo.Setup(br => br.GetSingleOrDefault(It.IsAny<BudgetSpecification>())).ReturnsAsync(budget);
            _mockAuthVerifier.Setup(av => av.IsBudgetMember(Users.ValidUser, budget)).Returns(true);
            _mockBalanceChangeRepo.Setup(bcr => bcr.List(It.IsAny<PagedBalanceChangesSpecification>())).ReturnsAsync(balanceChanges);

            var balanceChangesRetriever = new BalanceChangesRetriever(_mockBalanceChangeRepo.Object, _mockBudgetRepo.Object, _mockUserManager.Object, _mockAuthVerifier.Object);

            // Act
            var result = await balanceChangesRetriever.GetBalanceChangesForBudget(input);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(balanceChanges, result);
        }

        [Fact]
        public async Task GetBalanceChangesForBudget_UnauthorizedUser_ThrowsAuthorizationException()
        {
            // Arrange
            var input = new BalanceChangesForBudgetInput
            {
                BudgetId = 1,
                PageNumber = 0,
                PageSize = 20,
                RequestingUserName = Users.VALID_USERNAME
            };
            var budget = new Budget
            {
                Name = "Test"
            };
            var balanceChanges = new List<BalanceChange> { };

            _mockBudgetRepo.Setup(br => br.GetSingleOrDefault(It.IsAny<BudgetSpecification>())).ReturnsAsync(budget);
            _mockAuthVerifier.Setup(av => av.IsBudgetMember(Users.ValidUser, budget)).Returns(false);

            var balanceChangesRetriever = new BalanceChangesRetriever(_mockBalanceChangeRepo.Object, _mockBudgetRepo.Object, _mockUserManager.Object, _mockAuthVerifier.Object);

            // Act & Assert
            await Assert.ThrowsAsync<AuthorizationException>(
                () => balanceChangesRetriever.GetBalanceChangesForBudget(input));
        }
    }
}
