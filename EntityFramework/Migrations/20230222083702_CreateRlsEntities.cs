using EntityFrameworkRls.Helpers;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EntityFrameworkRls.Migrations
{
    /// <inheritdoc />
    public partial class CreateRlsEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(MigrationHelper.BuildCreateSecurityPolicyScript().ToString());
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(MigrationHelper.BuildDropSecurityPolicyScript().ToString());
        }
    }
}
