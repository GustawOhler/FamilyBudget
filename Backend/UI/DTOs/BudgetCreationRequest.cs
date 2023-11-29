namespace FamilyBudgetUI.DTOs
{
    public class BudgetCreationRequest
    {
        public required string Name { get; set; }
        public float Balance { get; set; }
    }
}