namespace FamilyBudget.DTOs
{
    public class BudgetResponse
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public float Balance { get; set; }
        public required UserOutputModel Admin { get; set; }
        public ICollection<UserOutputModel> Members { get; set; } = new List<UserOutputModel>();
    }
}