using System;
using FamilyBudget.Common;
using FamilyBudget.Models;

namespace FamilyBudget.DTOs
{
    public class BalanceChangeResponse
    {
        public int Id { get; set; }
        public float Amount { get; set; }
        public string Name { get; set; }
        public required int BudgetId { get; set; }
        public Category? Category { get; set; }
        public BalanceChangeType Type { get; set; }
        public DateTime DateOfChange { get; set; }
    }
}
