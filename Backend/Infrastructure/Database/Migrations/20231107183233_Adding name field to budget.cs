using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FamilyBudgetUI.Migrations
{
    /// <inheritdoc />
    public partial class Addingnamefieldtobudget : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                table: "BudgetChanges",
                newName: "Name");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Budgets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Budgets");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "BudgetChanges",
                newName: "Title");
        }
    }
}
