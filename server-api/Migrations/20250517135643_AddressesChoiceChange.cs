using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace electricity_provider_server_api.Migrations
{
    /// <inheritdoc />
    public partial class AddressesChoiceChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserProviderChoices_Users_UserId",
                table: "UserProviderChoices");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "UserProviderChoices",
                newName: "UserAddressId");

            migrationBuilder.RenameIndex(
                name: "IX_UserProviderChoices_UserId",
                table: "UserProviderChoices",
                newName: "IX_UserProviderChoices_UserAddressId");

            migrationBuilder.AddColumn<string>(
                name: "Postcode",
                table: "UserAddress",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Postcode",
                table: "ProviderAddress",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProviderRatings_ProviderId",
                table: "ProviderRatings",
                column: "ProviderId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProviderRatings_Providers_ProviderId",
                table: "ProviderRatings",
                column: "ProviderId",
                principalTable: "Providers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserProviderChoices_UserAddress_UserAddressId",
                table: "UserProviderChoices",
                column: "UserAddressId",
                principalTable: "UserAddress",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProviderRatings_Providers_ProviderId",
                table: "ProviderRatings");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProviderChoices_UserAddress_UserAddressId",
                table: "UserProviderChoices");

            migrationBuilder.DropIndex(
                name: "IX_ProviderRatings_ProviderId",
                table: "ProviderRatings");

            migrationBuilder.DropColumn(
                name: "Postcode",
                table: "UserAddress");

            migrationBuilder.DropColumn(
                name: "Postcode",
                table: "ProviderAddress");

            migrationBuilder.RenameColumn(
                name: "UserAddressId",
                table: "UserProviderChoices",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserProviderChoices_UserAddressId",
                table: "UserProviderChoices",
                newName: "IX_UserProviderChoices_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserProviderChoices_Users_UserId",
                table: "UserProviderChoices",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
