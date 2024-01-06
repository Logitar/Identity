using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Logitar.Identity.EntityFrameworkCore.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class AddedSessionSigningOut : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SignedOutBy",
                table: "Sessions",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SignedOutOn",
                table: "Sessions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_SignedOutBy",
                table: "Sessions",
                column: "SignedOutBy");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_SignedOutOn",
                table: "Sessions",
                column: "SignedOutOn");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Sessions_SignedOutBy",
                table: "Sessions");

            migrationBuilder.DropIndex(
                name: "IX_Sessions_SignedOutOn",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "SignedOutBy",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "SignedOutOn",
                table: "Sessions");
        }
    }
}
