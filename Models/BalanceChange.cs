using System.ComponentModel.DataAnnotations;
using FamilyBudget.Common;

namespace FamilyBudget.Models
{
    public class BalanceChange
    {
        [Key]
        public int Id { get; set; }
        public string? Title { get; set; }
        public float Amount { get; set; }
        public int BudgetId { get; set; }
        public required Budget Budget { get; set; }
        public Category? Category { get; set; }
        public BalanceChangeType Type { get; set; }
    }
}