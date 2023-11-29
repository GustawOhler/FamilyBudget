using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FamilyBudgetUI.Migrations
{
    /// <inheritdoc />
    public partial class CategoryBalancechangerelationshipfix : Migration
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
                column: "CategoryId");
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
                column: "CategoryId",
                unique: true,
                filter: "[CategoryId] IS NOT NULL");
        }
    }
}
