﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Logitar.Identity.EntityFrameworkCore.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class CreateApiKeyTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApiKeys",
                columns: table => new
                {
                    ApiKeyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    SecretHash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpiresOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AuthenticatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CustomAttributes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AggregateId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiKeys", x => x.ApiKeyId);
                });

            migrationBuilder.CreateTable(
                name: "ApiKeyRoles",
                columns: table => new
                {
                    ApiKeyId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiKeyRoles", x => new { x.ApiKeyId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_ApiKeyRoles_ApiKeys_ApiKeyId",
                        column: x => x.ApiKeyId,
                        principalTable: "ApiKeys",
                        principalColumn: "ApiKeyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApiKeyRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApiKeyRoles_RoleId",
                table: "ApiKeyRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_ApiKeys_AggregateId",
                table: "ApiKeys",
                column: "AggregateId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApiKeys_AuthenticatedOn",
                table: "ApiKeys",
                column: "AuthenticatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_ApiKeys_CreatedBy",
                table: "ApiKeys",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ApiKeys_CreatedOn",
                table: "ApiKeys",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_ApiKeys_DisplayName",
                table: "ApiKeys",
                column: "DisplayName");

            migrationBuilder.CreateIndex(
                name: "IX_ApiKeys_ExpiresOn",
                table: "ApiKeys",
                column: "ExpiresOn");

            migrationBuilder.CreateIndex(
                name: "IX_ApiKeys_TenantId",
                table: "ApiKeys",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ApiKeys_UpdatedBy",
                table: "ApiKeys",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ApiKeys_UpdatedOn",
                table: "ApiKeys",
                column: "UpdatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_ApiKeys_Version",
                table: "ApiKeys",
                column: "Version");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApiKeyRoles");

            migrationBuilder.DropTable(
                name: "ApiKeys");
        }
    }
}