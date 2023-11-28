namespace FamilyBudgetApplication.BalanceChangeOperations.DTOs
{
    public class BalanceChangesForBudgetInput
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? RequestingUserName { get; set; }
        public int BudgetId { get; set; }
    }
}