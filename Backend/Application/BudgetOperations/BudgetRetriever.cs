using System;
using FamilyBudgetDomain.Exceptions;
using FamilyBudgetApplication.Interfaces;
using FamilyBudgetDomain.Models;
using FamilyBudgetInfrastructure.Database.Specifications;
using Infrastructure.Database.Repositories;
using Application.BudgetOperations.DTOs;
using Microsoft.AspNetCore.Identity;

namespace FamilyBudgetApplication.BudgetOperations
{
    public class BudgetRetriever : IBudgetRetriever
    {
        private readonly IRepository<Budget> _budgetRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IAuthorizationVerifier _authorizationVerifier;
        private readonly UserManager<User> _userManager;
        public BudgetRetriever(IRepository<Budget> budgetRepository, IRepository<User> userRepository, IAuthorizationVerifier authorizationVerifier, UserManager<User> userManager)
        {
            _budgetRepository = budgetRepository;
            _userRepository = userRepository;
            _authorizationVerifier = authorizationVerifier;
            _userManager = userManager;
        }

        public async Task<Budget> GetBudget(string username, int budgetId)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                throw new ResourceNotFoundException("Requested user does not exist");
            }

            var budget = await _budgetRepository.GetSingleOrDefault(new BudgetSpecification(budgetId));

            if (!_authorizationVerifier.CheckAuthorizationForBudget(user, budget))
            {
                throw new AuthorizationException("User is not allowed for this resource");
            }

            return budget;
        }

        public async Task<List<Budget>> GetBudgetsForUser(BudgetsForUserInput input)
        {
            var requestedUser = await _userRepository.GetSingleOrDefault(new UserSpecification(input.RequestedUserId));
            if (requestedUser == null)
            {
                throw new ResourceNotFoundException("Requested user does not exist");
            }
            if (!_authorizationVerifier.CheckAuthorizationForUser(requestedUser, input.RequestingUserName))
            {
                throw new AuthorizationException("User is not allowed for this resource");
            }
            var specification = new PagedBudgetsSpecification(requestedUser, input.BudgetName, input.PageNumber, input.PageSize);
            var budgets = await _budgetRepository.List(specification);
            return budgets.ToList();
        }
    }
}
