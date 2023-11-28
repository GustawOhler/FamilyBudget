using Application.BudgetOperations.DTOs;
using FamilyBudgetDomain.Models;

namespace FamilyBudgetApplication.Interfaces
{
    public interface IBudgetRetriever
    {
        public Task<List<Budget>> GetBudgetsForUser(BudgetsForUserInput input);
        public Task<Budget> GetBudget(string username, int budgetId);
    }
}