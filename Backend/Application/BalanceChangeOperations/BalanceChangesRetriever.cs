using FamilyBudgetApplication.BalanceChangeOperations.DTOs;
using FamilyBudgetApplication.Interfaces;
using FamilyBudgetDomain.Exceptions;
using FamilyBudgetDomain.Models;
using FamilyBudgetInfrastructure.Database.Specifications;
using FamilyBudgetDomain.Interfaces.Database;
using Microsoft.AspNetCore.Identity;

namespace FamilyBudgetApplication.BalanceChangeOperations
{
    public class BalanceChangesRetriever : IBalanceChangesRetriever
    {
        private readonly UserManager<User> _userManager;
        private readonly IRepository<Budget> _budgetRepository;
        private readonly IRepository<BalanceChange> _balanceChangeRepository;
        private readonly IAuthorizationVerifier _authorizationVerifier;

        public BalanceChangesRetriever(IRepository<BalanceChange> balanceChangeRepository, IRepository<Budget> budgetRepository, UserManager<User> userManager, IAuthorizationVerifier authorizationVerifier)
        {
            _balanceChangeRepository = balanceChangeRepository;
            _budgetRepository = budgetRepository;
            _userManager = userManager;
            _authorizationVerifier = authorizationVerifier;
        }

        public async Task<List<BalanceChange>> GetBalanceChangesForBudget(BalanceChangesForBudgetInput input)
        {
            var authUser = await _userManager.FindByNameAsync(input.RequestingUserName);
            var budget = await _budgetRepository.GetSingleOrDefault(new BudgetSpecification(input.BudgetId));

            if (!_authorizationVerifier.IsBudgetMember(authUser, budget))
            {
                throw new AuthorizationException("User is not one of budget members.");
            }

            var balanceChanges = await _balanceChangeRepository.List(new PagedBalanceChangesSpecification(budget, input.PageNumber, input.PageSize));
            return balanceChanges.ToList();
        }
    }
}