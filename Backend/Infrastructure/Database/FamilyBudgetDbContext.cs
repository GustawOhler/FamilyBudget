using FamilyBudgetDomain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FamilyBudgetUI
{
    public class FamilyBudgetDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public DbSet<Budget> Budgets { get; set; }
        public DbSet<BalanceChange> BalanceChanges { get; set; }
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

            modelBuilder.Entity<Category>().HasData(new Category[]{
                new Category{Id=1, Name="Wypłata"},
                new Category{Id=2, Name="Zarobek z inwestycji"},
                new Category{Id=3, Name="Cashback"},
                new Category{Id=4, Name="Rachunki"},
                new Category{Id=5, Name="Podatki"},
                new Category{Id=6, Name="Zakupy spożywcze"},
                new Category{Id=7, Name="Płatności w lokalach"},
                new Category{Id=8, Name="Inne"},
            });
        }
    }
}