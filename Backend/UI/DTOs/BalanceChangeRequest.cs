using FamilyBudgetDomain.Enums;

namespace FamilyBudget.DTOs
{
    public class BalanceChangeRequest
    {
        public float Amount { get; set; }
        public string? Name { get; set; }
        public int CategoryId { get; set; }
        public BalanceChangeType Type { get; set; }
        public DateTime DateOfChange { get; set; }
    }
}
