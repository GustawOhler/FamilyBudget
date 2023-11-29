using System;
using FamilyBudgetDomain.Enums;
using FamilyBudgetDomain.Models;

namespace FamilyBudgetUI.DTOs
{
    public class BalanceChangeResponse
    {
        public int Id { get; set; }
        public float Amount { get; set; }
        public string Name { get; set; } = "";
        public int BudgetId { get; set; }
        public Category? Category { get; set; }
        public BalanceChangeType Type { get; set; }
        public DateTime DateOfChange { get; set; }
    }
}
