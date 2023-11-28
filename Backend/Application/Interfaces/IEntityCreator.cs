using FamilyBudgetApplication.BalanceChangeOperations.DTOs;
using FamilyBudgetApplication.BudgetOperations.DTOs;
using FamilyBudgetDomain.Models;

namespace FamilyBudgetApplication.Interfaces
{
    public interface IBalanceChangeManager
    {
        public Task<BalanceChange> CreateBalanceChange(NewBalanceChangeInput input);
    }
}