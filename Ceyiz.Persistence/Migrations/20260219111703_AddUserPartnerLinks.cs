using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ceyiz.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddUserPartnerLinks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CeyizUserPartnerLinks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PartnerUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CeyizUserPartnerLinks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CeyizUserPartnerLinks_CeyizUsers_PartnerUserId",
                        column: x => x.PartnerUserId,
                        principalTable: "CeyizUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CeyizUserPartnerLinks_CeyizUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "CeyizUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CeyizUserPartnerLinks_PartnerUserId",
                table: "CeyizUserPartnerLinks",
                column: "PartnerUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CeyizUserPartnerLinks_UserId_PartnerUserId",
                table: "CeyizUserPartnerLinks",
                columns: new[] { "UserId", "PartnerUserId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CeyizUserPartnerLinks");
        }
    }
}
