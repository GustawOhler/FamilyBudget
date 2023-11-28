using System;
using FamilyBudgetApplication.BalanceChangeOperations.DTOs;
using FamilyBudgetDomain.Models;

namespace FamilyBudgetApplication.Interfaces
{
    public interface IBalanceChangesRetriever
    {
        public Task<List<BalanceChange>> GetBalanceChangesForBudget(BalanceChangesForBudgetInput input);
    }
}
