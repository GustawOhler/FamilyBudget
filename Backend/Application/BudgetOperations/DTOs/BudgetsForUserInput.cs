using System;

namespace Application.BudgetOperations.DTOs
{
    public class BudgetsForUserInput
    {
        public int RequestedUserId { get; set; }
        public string? RequestingUserName { get; set; }
        public string? BudgetName { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
