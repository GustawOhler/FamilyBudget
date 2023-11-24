using System.ComponentModel.DataAnnotations;
using FamilyBudgetDomain.Enums;

namespace FamilyBudgetDomain.Models
{
    public class BalanceChange: IEntityBase
    {
        [Key]
        public int Id { get; init; }
        public string? Name { get; set; }
        public float Amount { get; set; }
        public required int BudgetId { get; set; }
        public Budget? Budget { get; set; }
        public DateTime DateOfChange { get; set; }
        public Category? Category { get; set; }
        public BalanceChangeType Type { get; set; }
    }
}