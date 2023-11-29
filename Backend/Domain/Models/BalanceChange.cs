using System.ComponentModel.DataAnnotations;
using FamilyBudgetDomain.Enums;

namespace FamilyBudgetDomain.Models
{
    public class BalanceChange : IEntityBase, IValidatableObject
    {
        [Key]
        public int Id { get; init; }
        public string? Name { get; set; }
        [Required]
        public float Amount { get; set; }
        [Required]
        public int BudgetId { get; set; }
        public Budget? Budget { get; set; }
        [Required]
        public DateTime DateOfChange { get; set; }
        public Category? Category { get; set; }
        [Required]
        public BalanceChangeType Type { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Type == BalanceChangeType.Income && Amount < 0 || Type == BalanceChangeType.Expense && Amount > 0)
            {
                yield return new ValidationResult("Amount of balance change is different than its type implies", new[] { nameof(Type), nameof(Amount) });
            }
        }
    }
}