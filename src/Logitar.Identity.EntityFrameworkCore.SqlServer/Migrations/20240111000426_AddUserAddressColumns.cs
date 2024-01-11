using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Logitar.Identity.EntityFrameworkCore.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class AddUserAddressColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AddressCountry",
                table: "Users",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AddressFormatted",
                table: "Users",
                type: "nvarchar(1279)",
                maxLength: 1279,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AddressLocality",
                table: "Users",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AddressPostalCode",
                table: "Users",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AddressRegion",
                table: "Users",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AddressStreet",
                table: "Users",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AddressVerifiedBy",
                table: "Users",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "AddressVerifiedOn",
                table: "Users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsAddressVerified",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Users_AddressCountry",
                table: "Users",
                column: "AddressCountry");

            migrationBuilder.CreateIndex(
                name: "IX_Users_AddressFormatted",
                table: "Users",
                column: "AddressFormatted");

            migrationBuilder.CreateIndex(
                name: "IX_Users_AddressLocality",
                table: "Users",
                column: "AddressLocality");

            migrationBuilder.CreateIndex(
                name: "IX_Users_AddressPostalCode",
                table: "Users",
                column: "AddressPostalCode");

            migrationBuilder.CreateIndex(
                name: "IX_Users_AddressRegion",
                table: "Users",
                column: "AddressRegion");

            migrationBuilder.CreateIndex(
                name: "IX_Users_AddressStreet",
                table: "Users",
                column: "AddressStreet");

            migrationBuilder.CreateIndex(
                name: "IX_Users_AddressVerifiedBy",
                table: "Users",
                column: "AddressVerifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Users_AddressVerifiedOn",
                table: "Users",
                column: "AddressVerifiedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Users_IsAddressVerified",
                table: "Users",
                column: "IsAddressVerified");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_AddressCountry",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_AddressFormatted",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_AddressLocality",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_AddressPostalCode",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_AddressRegion",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_AddressStreet",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_AddressVerifiedBy",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_AddressVerifiedOn",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_IsAddressVerified",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AddressCountry",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AddressFormatted",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AddressLocality",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AddressPostalCode",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AddressRegion",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AddressStreet",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AddressVerifiedBy",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AddressVerifiedOn",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsAddressVerified",
                table: "Users");
        }
    }
}
