using System.ComponentModel.DataAnnotations;

namespace FamilyBudgetDomain.Models
{
    public class Category: IEntityBase
    {
        [Key]
        public int Id { get; init; }
        public required string Name { get; set; }
    }
}