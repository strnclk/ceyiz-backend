using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ceyiz.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserId1",
                table: "CeyizUserSettings",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CeyizUserSettings_UserId1",
                table: "CeyizUserSettings",
                column: "UserId1",
                unique: true,
                filter: "[UserId1] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_CeyizUserSettings_CeyizUsers_UserId1",
                table: "CeyizUserSettings",
                column: "UserId1",
                principalTable: "CeyizUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CeyizUserSettings_CeyizUsers_UserId1",
                table: "CeyizUserSettings");

            migrationBuilder.DropIndex(
                name: "IX_CeyizUserSettings_UserId1",
                table: "CeyizUserSettings");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "CeyizUserSettings");
        }
    }
}
