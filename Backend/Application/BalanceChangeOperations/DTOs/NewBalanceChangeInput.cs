using FamilyBudgetDomain.Enums;

namespace FamilyBudgetApplication.BalanceChangeOperations.DTOs
{
    public class NewBalanceChangeInput
    {
        public float Amount { get; set; }
        public string? Name { get; set; }
        public int CategoryId { get; set; }
        public BalanceChangeType Type { get; set; }
        public DateTime DateOfChange { get; set; }
        public int BudgetId { get; set; }
        public string RequestingUserName { get; set; }
    }
}