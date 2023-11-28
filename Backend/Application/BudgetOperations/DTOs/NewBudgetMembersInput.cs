namespace FamilyBudgetApplication.BudgetOperations.DTOs
{
    public class NewBudgetMembersInput
    {
        public List<int> UserIds { get; set; }
        public int BudgetId { get; set; }
        public string RequestingUserName { get; set; }
    }
}