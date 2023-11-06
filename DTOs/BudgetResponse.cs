namespace FamilyBudget.DTOs
{
    public class BudgetResponse
    {
        public int Id { get; set; }
        public float Balance { get; set; }
        public UserOutputModel Admin { get; set; }
        public ICollection<UserOutputModel> Members { get; set; } = new List<UserOutputModel>();
        public ICollection<BalanceChangeResponse> BalanceChanges { get; } = new List<BalanceChangeResponse>();
    }
}