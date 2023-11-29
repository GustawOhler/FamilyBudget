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
using AutoMapper;

namespace FamilyBudgetApplication.BalanceChangeOperations
{
    public class BalanceChangeManager : IBalanceChangeManager
    {
        private readonly IRepository<Budget> _budgetRepository;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<BalanceChange> _balanceChangeRepository;
        private readonly UserManager<User> _userManager;
        private readonly IAuthorizationVerifier _authorizationVerifier;
        private readonly IMapper _mapper;

        public BalanceChangeManager(IRepository<Budget> budgetRepository, UserManager<User> userManager, IAuthorizationVerifier authorizationVerifier, IRepository<Category> categoryRepository, IRepository<BalanceChange> balanceChangeRepository, IMapper mapper)
        {
            _budgetRepository = budgetRepository;
            _userManager = userManager;
            _authorizationVerifier = authorizationVerifier;
            _categoryRepository = categoryRepository;
            _balanceChangeRepository = balanceChangeRepository;
            _mapper = mapper;
        }

        public async Task<BalanceChange> CreateBalanceChange(NewBalanceChangeInput input)
        {
            var authUser = await _userManager.FindByNameAsync(input.RequestingUserName);
            var budget = await _budgetRepository.GetSingleOrDefault(new BudgetSpecification(input.BudgetId));

            if (!_authorizationVerifier.CheckAuthorizationForBudget(authUser, budget))
            {
                throw new AuthorizationException("User is not allowed to manage this budget");
            }


            var balanceChange = _mapper.Map<BalanceChange>(input);
            balanceChange.Budget = budget;
            balanceChange.Category = await _categoryRepository.GetSingleOrDefault(new CategorySpecification(input.CategoryId));
            await _balanceChangeRepository.Add(balanceChange);

            budget.Balance += input.Amount;
            await _budgetRepository.Edit(budget);
            return balanceChange;
        }
    }
}
