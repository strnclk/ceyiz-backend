using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ceyiz.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCeyizTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Budgets_Users_UserId",
                table: "Budgets");

            migrationBuilder.DropForeignKey(
                name: "FK_TrousseauItems_Users_UserId",
                table: "TrousseauItems");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProfiles_Users_UserId",
                table: "UserProfiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserProfiles",
                table: "UserProfiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TrousseauItems",
                table: "TrousseauItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Budgets",
                table: "Budgets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Achievements",
                table: "Achievements");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "CeyizUsers");

            migrationBuilder.RenameTable(
                name: "UserProfiles",
                newName: "CeyizUserProfiles");

            migrationBuilder.RenameTable(
                name: "TrousseauItems",
                newName: "CeyizTrousseauItems");

            migrationBuilder.RenameTable(
                name: "Budgets",
                newName: "CeyizBudgets");

            migrationBuilder.RenameTable(
                name: "Achievements",
                newName: "CeyizAchievements");

            migrationBuilder.RenameIndex(
                name: "IX_Users_Email",
                table: "CeyizUsers",
                newName: "IX_CeyizUsers_Email");

            migrationBuilder.RenameIndex(
                name: "IX_UserProfiles_UserId",
                table: "CeyizUserProfiles",
                newName: "IX_CeyizUserProfiles_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_TrousseauItems_UserId",
                table: "CeyizTrousseauItems",
                newName: "IX_CeyizTrousseauItems_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Budgets_UserId",
                table: "CeyizBudgets",
                newName: "IX_CeyizBudgets_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CeyizUsers",
                table: "CeyizUsers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CeyizUserProfiles",
                table: "CeyizUserProfiles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CeyizTrousseauItems",
                table: "CeyizTrousseauItems",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CeyizBudgets",
                table: "CeyizBudgets",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CeyizAchievements",
                table: "CeyizAchievements",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CeyizBudgets_CeyizUsers_UserId",
                table: "CeyizBudgets",
                column: "UserId",
                principalTable: "CeyizUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CeyizTrousseauItems_CeyizUsers_UserId",
                table: "CeyizTrousseauItems",
                column: "UserId",
                principalTable: "CeyizUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CeyizUserProfiles_CeyizUsers_UserId",
                table: "CeyizUserProfiles",
                column: "UserId",
                principalTable: "CeyizUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CeyizBudgets_CeyizUsers_UserId",
                table: "CeyizBudgets");

            migrationBuilder.DropForeignKey(
                name: "FK_CeyizTrousseauItems_CeyizUsers_UserId",
                table: "CeyizTrousseauItems");

            migrationBuilder.DropForeignKey(
                name: "FK_CeyizUserProfiles_CeyizUsers_UserId",
                table: "CeyizUserProfiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CeyizUsers",
                table: "CeyizUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CeyizUserProfiles",
                table: "CeyizUserProfiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CeyizTrousseauItems",
                table: "CeyizTrousseauItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CeyizBudgets",
                table: "CeyizBudgets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CeyizAchievements",
                table: "CeyizAchievements");

            migrationBuilder.RenameTable(
                name: "CeyizUsers",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "CeyizUserProfiles",
                newName: "UserProfiles");

            migrationBuilder.RenameTable(
                name: "CeyizTrousseauItems",
                newName: "TrousseauItems");

            migrationBuilder.RenameTable(
                name: "CeyizBudgets",
                newName: "Budgets");

            migrationBuilder.RenameTable(
                name: "CeyizAchievements",
                newName: "Achievements");

            migrationBuilder.RenameIndex(
                name: "IX_CeyizUsers_Email",
                table: "Users",
                newName: "IX_Users_Email");

            migrationBuilder.RenameIndex(
                name: "IX_CeyizUserProfiles_UserId",
                table: "UserProfiles",
                newName: "IX_UserProfiles_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_CeyizTrousseauItems_UserId",
                table: "TrousseauItems",
                newName: "IX_TrousseauItems_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_CeyizBudgets_UserId",
                table: "Budgets",
                newName: "IX_Budgets_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserProfiles",
                table: "UserProfiles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TrousseauItems",
                table: "TrousseauItems",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Budgets",
                table: "Budgets",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Achievements",
                table: "Achievements",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Budgets_Users_UserId",
                table: "Budgets",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TrousseauItems_Users_UserId",
                table: "TrousseauItems",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfiles_Users_UserId",
                table: "UserProfiles",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
