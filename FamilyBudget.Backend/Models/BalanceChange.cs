using System.ComponentModel.DataAnnotations;
using FamilyBudget.Common;

namespace FamilyBudget.Models
{
    public class BalanceChange
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public float Amount { get; set; }
        public required int BudgetId { get; set; }
        public Budget? Budget { get; set; }
        public DateTime DateOfChange { get; set; }
        public Category? Category { get; set; }
        public BalanceChangeType Type { get; set; }
    }
}