using FamilyBudgetDomain.Enums;
using FamilyBudgetDomain.Models;

namespace FamilyBudget.DTOs
{
    public class BalanceChangeOutputModel
    {
        public int Id { get; set; }
        public float Amount { get; set; }
        public string Name { get; set; } = "";
        public Category? Category { get; set; }
        public BalanceChangeType Type { get; set; }
        public DateTime DateOfChange { get; set; }
    }
    
    public class BudgetResponse
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public float Balance { get; set; }
        public required UserOutputModel Admin { get; set; }
        public ICollection<UserOutputModel> Members { get; set; } = new List<UserOutputModel>();
        public ICollection<BalanceChangeOutputModel> balanceChanges {get;set;} = new List<BalanceChangeOutputModel>();
    }
}