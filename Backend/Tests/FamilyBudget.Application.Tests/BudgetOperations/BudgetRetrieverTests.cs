using Application.BudgetOperations.DTOs;
using FamilyBudget.Application.Tests.Mocks;
using FamilyBudget.Application.Tests.ReadyObjects;
using FamilyBudgetApplication.BudgetOperations;
using FamilyBudgetApplication.Interfaces;
using FamilyBudgetDomain.Exceptions;
using FamilyBudgetDomain.Interfaces.Database;
using FamilyBudgetDomain.Models;
using FamilyBudgetInfrastructure.Database.Specifications;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace FamilyBudget.Application.Tests.BudgetOperations
{
    public class BudgetRetrieverTests
    {
        private readonly Mock<IRepository<Budget>> _mockBudgetRepo;
        private readonly Mock<IRepository<User>> _mockUserRepo;
        private readonly Mock<IAuthorizationVerifier> _mockAuthVerifier;
        private readonly Mock<UserManager<User>> _mockUserManager;

        public BudgetRetrieverTests()
        {
            _mockBudgetRepo = new Mock<IRepository<Budget>>();
            _mockBudgetRepo.Setup(r => r.GetSingleOrDefault(It.IsAny<BudgetSpecification>())).ReturnsAsync(
                new Budget()
                {
                    Name = "Test",
                    Admin = Users.ValidUser,
                    Members = new List<User> { Users.ValidUser }
                }
            );
            _mockBudgetRepo.Setup(r => r.List(It.IsAny<PagedBudgetsSpecification>())).ReturnsAsync(
                new List<Budget>()
                {
                    new Budget()
                    {
                        Name = "Test",
                        Admin = Users.ValidUser,
                        Members = new List<User> { Users.ValidUser }
                    }
                }
            );
            _mockUserRepo = new Mock<IRepository<User>>();
            _mockUserRepo.Setup(r => r.GetSingleOrDefault(It.IsAny<UserSpecification>())).ReturnsAsync(
                Users.ValidUser
            );
            _mockAuthVerifier = new Mock<IAuthorizationVerifier>();
            _mockUserManager = DependencyInjectionMocks.GetUserManagerMock();
        }

        [Fact]
        public async Task GetBudget_UserDoesNotExist_ThrowsResourceNotFoundException()
        {
            // Arrange
            var retriever = new BudgetRetriever(_mockBudgetRepo.Object, _mockUserRepo.Object, _mockAuthVerifier.Object, _mockUserManager.Object);

            // Act && Assert
            await Assert.ThrowsAsync<ResourceNotFoundException>(
                () => retriever.GetBudget("NonExistingUser", 1));
        }

        [Fact]
        public async Task GetBudget_UserIsNotMemberOfBudget_ThrowsAuthorizationException()
        {
            // Arrange
            _mockAuthVerifier.Setup(av => av.IsBudgetMember(It.IsAny<User>(), It.IsAny<Budget>())).Returns(false);
            var retriever = new BudgetRetriever(_mockBudgetRepo.Object, _mockUserRepo.Object, _mockAuthVerifier.Object, _mockUserManager.Object);

            // Act && Assert
            await Assert.ThrowsAsync<AuthorizationException>(
                () => retriever.GetBudget(Users.VALID_USERNAME, 1));
        }

        [Fact]
        public async Task GetBudget_ValidUser_ReturnsBudget()
        {
            // Arrange
            _mockAuthVerifier.Setup(av => av.IsBudgetMember(It.IsAny<User>(), It.IsAny<Budget>())).Returns(true);
            var retriever = new BudgetRetriever(_mockBudgetRepo.Object, _mockUserRepo.Object, _mockAuthVerifier.Object, _mockUserManager.Object);

            // Act
            var result = await retriever.GetBudget(Users.VALID_USERNAME, 1);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Budget>(result);
        }

        [Fact]
        public async Task GetBudgetsForUser_UserDoesNotExist_ThrowsResourceNotFoundException()
        {
            // Arrange
            _mockUserRepo.Setup(r => r.GetSingleOrDefault(It.IsAny<UserSpecification>())).ReturnsAsync(
                (User)null
            );
            var retriever = new BudgetRetriever(_mockBudgetRepo.Object, _mockUserRepo.Object, _mockAuthVerifier.Object, _mockUserManager.Object);
            var input = new BudgetsForUserInput
            {
                RequestedUserId = 2
            };

            // Act && Assert
            await Assert.ThrowsAsync<ResourceNotFoundException>(
                () => retriever.GetBudgetsForUser(input));
        }

        [Fact]
        public async Task GetBudgetsForUser_UserIsNotAllowed_ThrowsAuthorizationException()
        {
            // Arrange
            var retriever = new BudgetRetriever(_mockBudgetRepo.Object, _mockUserRepo.Object, _mockAuthVerifier.Object, _mockUserManager.Object);
            var input = new BudgetsForUserInput
            {
                RequestedUserId = 1,
                RequestingUserName = "InvalidUser"
            };

            // Act && Assert
            await Assert.ThrowsAsync<AuthorizationException>(
                () => retriever.GetBudgetsForUser(input));
        }

        [Fact]
        public async Task GetBudgetsForUser_ValidInput_ReturnsListOfBudgets()
        {
            // Arrange
            _mockAuthVerifier.Setup(av => av.IsAuthorizedForUser(It.IsAny<User>(), It.IsAny<string>())).Returns(true);
            var retriever = new BudgetRetriever(_mockBudgetRepo.Object, _mockUserRepo.Object, _mockAuthVerifier.Object, _mockUserManager.Object);
            var input = new BudgetsForUserInput
            {
                RequestedUserId = 1,
                RequestingUserName = Users.VALID_USERNAME,
                BudgetName = "",
                PageNumber = 0,
                PageSize = 25
            };

            // Act
            var result = await retriever.GetBudgetsForUser(input);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<Budget>>(result);
        }
    }
}