using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace FamilyBudget.Models
{
    public class User : IdentityUser<int>
    {
        public ICollection<Budget> ParticipatingBudgets { get; set; } = new List<Budget>();
    }
}