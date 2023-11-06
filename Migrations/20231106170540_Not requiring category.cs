using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FamilyBudget.Migrations
{
    /// <inheritdoc />
    public partial class Notrequiringcategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BudgetChanges_CategoryId",
                table: "BudgetChanges");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetChanges_CategoryId",
                table: "BudgetChanges",
                column: "CategoryId",
                unique: true,
                filter: "[CategoryId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BudgetChanges_CategoryId",
                table: "BudgetChanges");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetChanges_CategoryId",
                table: "BudgetChanges",
                column: "CategoryId");
        }
    }
}
