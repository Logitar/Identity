﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Logitar.Identity.EntityFrameworkCore.PostgreSQL.Migrations
{
    /// <inheritdoc />
    public partial class CreateRealmTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Realms",
                columns: table => new
                {
                    RealmId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UniqueName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    UniqueNameNormalized = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    DisplayName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    DefaultLocale = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Url = table.Column<string>(type: "character varying(65535)", maxLength: 65535, nullable: true),
                    RequireConfirmedAccount = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    RequireUniqueEmail = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    UsernameSettings = table.Column<string>(type: "jsonb", nullable: false),
                    PasswordSettings = table.Column<string>(type: "jsonb", nullable: false),
                    JwtSecret = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    ClaimMappings = table.Column<string>(type: "jsonb", nullable: true),
                    CustomAttributes = table.Column<string>(type: "jsonb", nullable: true),
                    GoogleOAuth2Configuration = table.Column<string>(type: "jsonb", nullable: true),
                    AggregateId = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    CreatedById = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false, defaultValue: "SYSTEM"),
                    CreatedBy = table.Column<string>(type: "jsonb", nullable: false, defaultValue: "{\"Type\":\"System\",\"IsDeleted\":false,\"DisplayName\":\"System\",\"Email\":null,\"Picture\":null}"),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedById = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    UpdatedBy = table.Column<string>(type: "jsonb", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Realms", x => x.RealmId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Realms_AggregateId",
                table: "Realms",
                column: "AggregateId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Realms_CreatedById",
                table: "Realms",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Realms_CreatedOn",
                table: "Realms",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Realms_DisplayName",
                table: "Realms",
                column: "DisplayName");

            migrationBuilder.CreateIndex(
                name: "IX_Realms_UniqueName",
                table: "Realms",
                column: "UniqueName");

            migrationBuilder.CreateIndex(
                name: "IX_Realms_UniqueNameNormalized",
                table: "Realms",
                column: "UniqueNameNormalized",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Realms_UpdatedById",
                table: "Realms",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Realms_UpdatedOn",
                table: "Realms",
                column: "UpdatedOn");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Realms");
        }
    }
}
