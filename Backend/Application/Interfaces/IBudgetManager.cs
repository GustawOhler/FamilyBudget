using FamilyBudgetApplication.BudgetOperations.DTOs;
using FamilyBudgetDomain.Models;

namespace FamilyBudgetApplication.Interfaces
{
    public interface IBudgetManager
    {
        public Task<Budget> CreateBudget(NewBudgetInput input);
        public Task<Budget> AddBudgetMembers(NewBudgetMembersInput input);
    }
}