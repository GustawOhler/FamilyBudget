using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace FamilyBudgetDomain.Models
{
    public class User : IdentityUser<int>
    {
        public ICollection<Budget> ParticipatingBudgets { get; set; } = new List<Budget>();
    }
}