using System.ComponentModel.DataAnnotations;

namespace FamilyBudget.Models
{
    public class Budget
    {
        [Key]
        public int Id { get; set; }
        public required string Name { get; set; }
        public float Balance { get; set; }
        public int AdminId { get; set; }
        public User? Admin { get; set; }
        public ICollection<User> Members { get; set; } = new List<User>();
        public ICollection<BalanceChange> BalanceChanges { get; } = new List<BalanceChange>();
    }
}