using System;
using FamilyBudgetDomain.Exceptions;
using FamilyBudgetApplication.BalanceChangeOperations.DTOs;
using FamilyBudgetApplication.BudgetOperations.DTOs;
using FamilyBudgetApplication.Interfaces;
using FamilyBudgetDomain.Enums;
using FamilyBudgetDomain.Models;
using FamilyBudgetInfrastructure.Database.Specifications;
using Infrastructure.Database.Repositories;
using Microsoft.AspNetCore.Identity;

namespace FamilyBudgetApplication.BalanceChangeOperations
{
    public class BalanceChangeManager : IBalanceChangeManager
    {
        private readonly IRepository<Budget> _budgetRepository;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<BalanceChange> _balanceChangeRepository;
        private readonly UserManager<User> _userManager;
        private readonly IAuthorizationVerifier _authorizationVerifier;

        public BalanceChangeManager(IRepository<Budget> budgetRepository, UserManager<User> userManager, IAuthorizationVerifier authorizationVerifier, IRepository<Category> categoryRepository, IRepository<BalanceChange> balanceChangeRepository)
        {
            _budgetRepository = budgetRepository;
            _userManager = userManager;
            _authorizationVerifier = authorizationVerifier;
            _categoryRepository = categoryRepository;
            _balanceChangeRepository = balanceChangeRepository;
        }

        public async Task<BalanceChange> CreateBalanceChange(NewBalanceChangeInput input)
        {
            var authUser = await _userManager.FindByNameAsync(input.RequestingUserName);
            var budget = await _budgetRepository.GetSingleOrDefault(new BudgetSpecification(input.BudgetId));

            if (!_authorizationVerifier.CheckAuthorizationForBudget(authUser, budget))
            {
                throw new AuthorizationException("User is not allowed to manage this budget");
            }

            if (input.Type == BalanceChangeType.Income && input.Amount < 0 || input.Type == BalanceChangeType.Expense && input.Amount > 0)
            {
                throw new ValidationException("Amount of balance change is different than its type implies");
            }

            var balanceChange = new BalanceChange
            {
                DateOfChange = input.DateOfChange,
                Name = input.Name,
                Type = input.Type,
                BudgetId = input.BudgetId,
                Amount = input.Amount,
                Budget = budget,
                Category = await _categoryRepository.GetSingleOrDefault(new CategorySpecification(input.CategoryId))
            };
            await _balanceChangeRepository.Add(balanceChange);

            budget.Balance += input.Amount;
            await _budgetRepository.Edit(budget);
            return balanceChange;
        }
    }
}
