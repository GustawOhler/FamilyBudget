using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FamilyBudget.Migrations
{
    /// <inheritdoc />
    public partial class Addedtitletobalancechanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "BudgetChanges",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "BudgetChanges");
        }
    }
}
