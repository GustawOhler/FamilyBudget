using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FamilyBudgetUI.Migrations
{
    /// <inheritdoc />
    public partial class Changeofnameoftable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BudgetChanges_Budgets_BudgetId",
                table: "BudgetChanges");

            migrationBuilder.DropForeignKey(
                name: "FK_BudgetChanges_Categories_CategoryId",
                table: "BudgetChanges");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BudgetChanges",
                table: "BudgetChanges");

            migrationBuilder.RenameTable(
                name: "BudgetChanges",
                newName: "BalanceChanges");

            migrationBuilder.RenameIndex(
                name: "IX_BudgetChanges_CategoryId",
                table: "BalanceChanges",
                newName: "IX_BalanceChanges_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_BudgetChanges_BudgetId",
                table: "BalanceChanges",
                newName: "IX_BalanceChanges_BudgetId");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfChange",
                table: "BalanceChanges",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_BalanceChanges",
                table: "BalanceChanges",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BalanceChanges_Budgets_BudgetId",
                table: "BalanceChanges",
                column: "BudgetId",
                principalTable: "Budgets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BalanceChanges_Categories_CategoryId",
                table: "BalanceChanges",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BalanceChanges_Budgets_BudgetId",
                table: "BalanceChanges");

            migrationBuilder.DropForeignKey(
                name: "FK_BalanceChanges_Categories_CategoryId",
                table: "BalanceChanges");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BalanceChanges",
                table: "BalanceChanges");

            migrationBuilder.DropColumn(
                name: "DateOfChange",
                table: "BalanceChanges");

            migrationBuilder.RenameTable(
                name: "BalanceChanges",
                newName: "BudgetChanges");

            migrationBuilder.RenameIndex(
                name: "IX_BalanceChanges_CategoryId",
                table: "BudgetChanges",
                newName: "IX_BudgetChanges_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_BalanceChanges_BudgetId",
                table: "BudgetChanges",
                newName: "IX_BudgetChanges_BudgetId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BudgetChanges",
                table: "BudgetChanges",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetChanges_Budgets_BudgetId",
                table: "BudgetChanges",
                column: "BudgetId",
                principalTable: "Budgets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetChanges_Categories_CategoryId",
                table: "BudgetChanges",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id");
        }
    }
}
