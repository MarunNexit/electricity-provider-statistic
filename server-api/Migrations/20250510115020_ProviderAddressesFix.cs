using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace electricity_provider_server_api.Migrations
{
    /// <inheritdoc />
    public partial class ProviderAddressesFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Region",
                table: "UserAddress",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Region",
                table: "ProviderAddress",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Region",
                table: "UserAddress");

            migrationBuilder.DropColumn(
                name: "Region",
                table: "ProviderAddress");
        }
    }
}
