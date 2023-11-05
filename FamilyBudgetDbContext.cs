using FamilyBudget.Models;
using Microsoft.EntityFrameworkCore;

namespace FamilyBudget
{
    public class FamilyBudgetDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public FamilyBudgetDbContext(DbContextOptions<FamilyBudgetDbContext> options) : base(options)
        {
        }
    }
}