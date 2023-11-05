using System.ComponentModel.DataAnnotations;
using Azure.Identity;

namespace FamilyBudget.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public required string Username { get; set; }
        public required string PasswordHash { get; set; }
    }
}