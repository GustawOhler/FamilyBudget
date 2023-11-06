using FamilyBudget.Common;

namespace FamilyBudget.DTOs
{
    public class BalanceChangeRequest
    {
        public float Amount { get; set; }
        public string? Title { get; set; }
        public int CategoryId { get; set; }
        public BalanceChangeType Type { get; set; }
    }
}
