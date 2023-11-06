using FamilyBudget.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FamilyBudget
{
    public class FamilyBudgetDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public DbSet<Budget> Budgets { get; set; }
        public DbSet<BalanceChange> BudgetChanges { get; set; }
        public DbSet<Category> Categories { get; set; }
        public FamilyBudgetDbContext(DbContextOptions<FamilyBudgetDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Budget>()
            .HasMany(e => e.Members)
            .WithMany(e => e.ParticipatingBudgets)
            .UsingEntity(
                "BudgetUser",
                l => l.HasOne(typeof(Budget)).WithMany().HasForeignKey("ParticipatingBudgetsId").OnDelete(DeleteBehavior.NoAction),
                r => r.HasOne(typeof(User)).WithMany().HasForeignKey("MembersId").OnDelete(DeleteBehavior.NoAction),
                j => j.HasKey("ParticipatingBudgetsId", "MembersId"));
            modelBuilder.Entity<Budget>().HasOne(e => e.Admin).WithMany().HasForeignKey(e => e.AdminId).IsRequired();

            // modelBuilder.Entity<BalanceChange>().HasOne(bc => bc.Category).WithOne().HasForeignKey<Category>(c => c.Id).IsRequired(false);
        }
    }
}