namespace FamilyBudgetApplication.BudgetOperations.DTOs
{
    public class NewBudgetInput
    {
        public required string Name { get; set; }
        public float Balance { get; set; }
        public string AdminUsername { get; set; }
    }
}