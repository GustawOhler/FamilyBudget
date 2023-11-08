using System;

namespace FamilyBudget.DTOs
{
    public class BudgetSearchRequest
    {
        public int Limit { get; set; }
        public int Offset { get; set; }
        public string? Name { get; set; }
    }
}