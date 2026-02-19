using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ceyiz.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FixPartnerIdOnTrousseauItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
IF COL_LENGTH('CeyizTrousseauItems', 'PartnerId') IS NULL
BEGIN
    ALTER TABLE [CeyizTrousseauItems] ADD [PartnerId] uniqueidentifier NULL;
END
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
IF COL_LENGTH('CeyizTrousseauItems', 'PartnerId') IS NOT NULL
BEGIN
    ALTER TABLE [CeyizTrousseauItems] DROP COLUMN [PartnerId];
END
");
        }
    }
}
