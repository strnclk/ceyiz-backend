using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ceyiz.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddUserSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CeyizUserSettings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TotalBudget = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 75000m),
                    WeddingDate = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PartnerName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CeyizUserSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CeyizUserSettings_CeyizUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "CeyizUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CeyizUserSettings_UserId",
                table: "CeyizUserSettings",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CeyizUserSettings");
        }
    }
}
