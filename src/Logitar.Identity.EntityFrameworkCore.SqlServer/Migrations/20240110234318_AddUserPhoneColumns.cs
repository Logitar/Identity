using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Logitar.Identity.EntityFrameworkCore.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class AddUserPhoneColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPhoneVerified",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PhoneCountryCode",
                table: "Users",
                type: "nvarchar(2)",
                maxLength: 2,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneE164Formatted",
                table: "Users",
                type: "nvarchar(37)",
                maxLength: 37,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneExtension",
                table: "Users",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Users",
                type: "nvarchar(17)",
                maxLength: 17,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneVerifiedBy",
                table: "Users",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PhoneVerifiedOn",
                table: "Users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_IsPhoneVerified",
                table: "Users",
                column: "IsPhoneVerified");

            migrationBuilder.CreateIndex(
                name: "IX_Users_PhoneCountryCode",
                table: "Users",
                column: "PhoneCountryCode");

            migrationBuilder.CreateIndex(
                name: "IX_Users_PhoneE164Formatted",
                table: "Users",
                column: "PhoneE164Formatted");

            migrationBuilder.CreateIndex(
                name: "IX_Users_PhoneExtension",
                table: "Users",
                column: "PhoneExtension");

            migrationBuilder.CreateIndex(
                name: "IX_Users_PhoneNumber",
                table: "Users",
                column: "PhoneNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Users_PhoneVerifiedBy",
                table: "Users",
                column: "PhoneVerifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Users_PhoneVerifiedOn",
                table: "Users",
                column: "PhoneVerifiedOn");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_IsPhoneVerified",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_PhoneCountryCode",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_PhoneE164Formatted",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_PhoneExtension",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_PhoneNumber",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_PhoneVerifiedBy",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_PhoneVerifiedOn",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsPhoneVerified",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PhoneCountryCode",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PhoneE164Formatted",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PhoneExtension",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PhoneVerifiedBy",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PhoneVerifiedOn",
                table: "Users");
        }
    }
}
