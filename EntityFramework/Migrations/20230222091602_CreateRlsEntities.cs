using EntityFrameworkRls.Helpers;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EntityFrameworkRls.Migrations
{
    public partial class CreateRlsEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(MigrationHelper.BuildCreateSecurityPolicyScript().ToString());
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(MigrationHelper.BuildDropSecurityPolicyScript().ToString());
        }
    }
}
