using FamilyBudgetApplication.BudgetOperations.DTOs;
using FamilyBudgetApplication.Interfaces;
using FamilyBudgetDomain.Exceptions;
using FamilyBudgetDomain.Models;
using FamilyBudgetInfrastructure.Database.Specifications;
using Infrastructure.Database.Repositories;
using Microsoft.AspNetCore.Identity;

namespace FamilyBudgetApplication.BudgetOperations
{
    public class BudgetManager : IBudgetManager
    {
        private readonly IRepository<Budget> _budgetRepository;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<BalanceChange> _balanceChangeRepository;
        private readonly IRepository<User> _userRepository;
        private readonly UserManager<User> _userManager;
        private readonly IAuthorizationVerifier _authorizationVerifier;

        public BudgetManager(IRepository<Budget> budgetRepository, UserManager<User> userManager, IAuthorizationVerifier authorizationVerifier, IRepository<Category> categoryRepository, IRepository<BalanceChange> balanceChangeRepository, IRepository<User> userRepository)
        {
            _budgetRepository = budgetRepository;
            _userManager = userManager;
            _authorizationVerifier = authorizationVerifier;
            _categoryRepository = categoryRepository;
            _balanceChangeRepository = balanceChangeRepository;
            _userRepository = userRepository;
        }

        public async Task<Budget> AddBudgetMembers(NewBudgetMembersInput input)
        {
            var authUser = await _userManager.FindByNameAsync(input.RequestingUserName);
            var budget = await _budgetRepository.GetSingleOrDefault(new BudgetSpecification(input.BudgetId));

            if (!_authorizationVerifier.IsAdmin(authUser, budget))
            {
                throw new AuthorizationException("User needs admin rights for this operation");
            }

            var newMembers = await _userRepository.List(new UserSpecification(input.UserIds));

            var concatMembers = budget.Members.ToList();
            concatMembers.AddRange(newMembers);
            budget.Members = concatMembers.Distinct().ToList();

            await _budgetRepository.Edit(budget);
            return budget;
        }

        public async Task<Budget> CreateBudget(NewBudgetInput input)
        {
            var authUser = await _userManager.FindByNameAsync(input.AdminUsername);

            var budget = new Budget()
            {
                Name = input.Name,
                Balance = input.Balance,
                Admin = authUser
            };
            budget.Members.Add(authUser);

            await _budgetRepository.Add(budget);
            return budget;
        }
    }
}