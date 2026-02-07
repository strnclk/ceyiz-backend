using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ceyiz.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddPartnerId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PartnerId",
                table: "CeyizUsers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PartnerId",
                table: "CeyizTrousseauItems",
                type: "uniqueidentifier",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PartnerId",
                table: "CeyizUsers");

            migrationBuilder.DropColumn(
                name: "PartnerId",
                table: "CeyizTrousseauItems");
        }
    }
}
