using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Logitar.Identity.EntityFrameworkCore.PostgreSQL.Migrations
{
    /// <inheritdoc />
    public partial class CreateJwtBlacklistTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JwtBlacklist",
                columns: table => new
                {
                    BlacklistedJwtId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ExpiresOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JwtBlacklist", x => x.BlacklistedJwtId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JwtBlacklist_ExpiresOn",
                table: "JwtBlacklist",
                column: "ExpiresOn");

            migrationBuilder.CreateIndex(
                name: "IX_JwtBlacklist_Id",
                table: "JwtBlacklist",
                column: "Id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JwtBlacklist");
        }
    }
}
