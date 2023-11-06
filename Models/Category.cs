using System.ComponentModel.DataAnnotations;

namespace FamilyBudget.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        public required string Name { get; set; }
    }
}