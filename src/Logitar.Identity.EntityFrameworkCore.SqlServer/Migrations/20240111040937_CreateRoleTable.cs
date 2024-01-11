﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Logitar.Identity.EntityFrameworkCore.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class CreateRoleTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    UniqueName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UniqueNameNormalized = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_Roles", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "UserIdentifiers",
                columns: table => new
                {
                    UserIdentifierId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Key = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserIdentifiers", x => x.UserIdentifierId);
                    table.ForeignKey(
                        name: "FK_UserIdentifiers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Roles_AggregateId",
                table: "Roles",
                column: "AggregateId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Roles_CreatedBy",
                table: "Roles",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_CreatedOn",
                table: "Roles",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_DisplayName",
                table: "Roles",
                column: "DisplayName");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_TenantId",
                table: "Roles",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_TenantId_UniqueNameNormalized",
                table: "Roles",
                columns: new[] { "TenantId", "UniqueNameNormalized" },
                unique: true,
                filter: "[TenantId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_UniqueName",
                table: "Roles",
                column: "UniqueName");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_UpdatedBy",
                table: "Roles",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_UpdatedOn",
                table: "Roles",
                column: "UpdatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Version",
                table: "Roles",
                column: "Version");

            migrationBuilder.CreateIndex(
                name: "IX_UserIdentifiers_Key",
                table: "UserIdentifiers",
                column: "Key");

            migrationBuilder.CreateIndex(
                name: "IX_UserIdentifiers_TenantId_Key_Value",
                table: "UserIdentifiers",
                columns: new[] { "TenantId", "Key", "Value" },
                unique: true,
                filter: "[TenantId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UserIdentifiers_UserId_Key",
                table: "UserIdentifiers",
                columns: new[] { "UserId", "Key" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserIdentifiers_Value",
                table: "UserIdentifiers",
                column: "Value");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "UserIdentifiers");
        }
    }
}
