using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Logitar.Identity.EntityFrameworkCore.PostgreSQL.Migrations
{
    /// <inheritdoc />
    public partial class CreateUserTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RealmId = table.Column<int>(type: "integer", nullable: false),
                    Username = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    UsernameNormalized = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    PasswordHash = table.Column<string>(type: "character varying(65535)", maxLength: 65535, nullable: true),
                    PasswordChangedById = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    PasswordChangedBy = table.Column<string>(type: "jsonb", nullable: true),
                    PasswordChangedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    HasPassword = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DisabledById = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    DisabledBy = table.Column<string>(type: "jsonb", nullable: true),
                    DisabledOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDisabled = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    SignedInOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    AddressLine1 = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    AddressLine2 = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    AddressLocality = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    AddressPostalCode = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    AddressCountry = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    AddressRegion = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    AddressFormatted = table.Column<string>(type: "character varying(65535)", maxLength: 65535, nullable: true),
                    AddressVerifiedById = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    AddressVerifiedBy = table.Column<string>(type: "jsonb", nullable: true),
                    AddressVerifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsAddressVerified = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    EmailAddress = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    EmailAddressNormalized = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    EmailVerifiedById = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    EmailVerifiedBy = table.Column<string>(type: "jsonb", nullable: true),
                    EmailVerifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsEmailVerified = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    PhoneCountryCode = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    PhoneNumber = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    PhoneExtension = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    PhoneE164Formatted = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    PhoneVerifiedById = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    PhoneVerifiedBy = table.Column<string>(type: "jsonb", nullable: true),
                    PhoneVerifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsPhoneVerified = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    IsConfirmed = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    FirstName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    MiddleName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    LastName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    FullName = table.Column<string>(type: "character varying(65535)", maxLength: 65535, nullable: true),
                    Nickname = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Birthdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Gender = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Locale = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    TimeZone = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Picture = table.Column<string>(type: "character varying(65535)", maxLength: 65535, nullable: true),
                    Profile = table.Column<string>(type: "character varying(65535)", maxLength: 65535, nullable: true),
                    Website = table.Column<string>(type: "character varying(65535)", maxLength: 65535, nullable: true),
                    CustomAttributes = table.Column<string>(type: "jsonb", nullable: true),
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
                    table.PrimaryKey("PK_Users", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Users_Realms_RealmId",
                        column: x => x.RealmId,
                        principalTable: "Realms",
                        principalColumn: "RealmId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ExternalIdentifiers",
                columns: table => new
                {
                    ExternalIdentifierId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RealmId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    Key = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Value = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    ValueNormalized = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    CreatedById = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false, defaultValue: "SYSTEM"),
                    CreatedBy = table.Column<string>(type: "jsonb", nullable: false, defaultValue: "{\"Type\":\"System\",\"IsDeleted\":false,\"DisplayName\":\"System\",\"Email\":null,\"Picture\":null}"),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedById = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    UpdatedBy = table.Column<string>(type: "jsonb", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExternalIdentifiers", x => x.ExternalIdentifierId);
                    table.ForeignKey(
                        name: "FK_ExternalIdentifiers_Realms_RealmId",
                        column: x => x.RealmId,
                        principalTable: "Realms",
                        principalColumn: "RealmId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExternalIdentifiers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    RoleId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.RoleId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExternalIdentifiers_CreatedById",
                table: "ExternalIdentifiers",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalIdentifiers_RealmId_Key_ValueNormalized",
                table: "ExternalIdentifiers",
                columns: new[] { "RealmId", "Key", "ValueNormalized" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExternalIdentifiers_UpdatedById",
                table: "ExternalIdentifiers",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalIdentifiers_UserId_Key",
                table: "ExternalIdentifiers",
                columns: new[] { "UserId", "Key" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_UserId",
                table: "UserRoles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_AddressFormatted",
                table: "Users",
                column: "AddressFormatted");

            migrationBuilder.CreateIndex(
                name: "IX_Users_AddressVerifiedById",
                table: "Users",
                column: "AddressVerifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Users_AggregateId",
                table: "Users",
                column: "AggregateId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_CreatedById",
                table: "Users",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CreatedOn",
                table: "Users",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Users_DisabledById",
                table: "Users",
                column: "DisabledById");

            migrationBuilder.CreateIndex(
                name: "IX_Users_DisabledOn",
                table: "Users",
                column: "DisabledOn");

            migrationBuilder.CreateIndex(
                name: "IX_Users_EmailAddress",
                table: "Users",
                column: "EmailAddress");

            migrationBuilder.CreateIndex(
                name: "IX_Users_EmailVerifiedById",
                table: "Users",
                column: "EmailVerifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Users_FirstName",
                table: "Users",
                column: "FirstName");

            migrationBuilder.CreateIndex(
                name: "IX_Users_FullName",
                table: "Users",
                column: "FullName");

            migrationBuilder.CreateIndex(
                name: "IX_Users_IsConfirmed",
                table: "Users",
                column: "IsConfirmed");

            migrationBuilder.CreateIndex(
                name: "IX_Users_IsDisabled",
                table: "Users",
                column: "IsDisabled");

            migrationBuilder.CreateIndex(
                name: "IX_Users_LastName",
                table: "Users",
                column: "LastName");

            migrationBuilder.CreateIndex(
                name: "IX_Users_MiddleName",
                table: "Users",
                column: "MiddleName");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Nickname",
                table: "Users",
                column: "Nickname");

            migrationBuilder.CreateIndex(
                name: "IX_Users_PasswordChangedById",
                table: "Users",
                column: "PasswordChangedById");

            migrationBuilder.CreateIndex(
                name: "IX_Users_PasswordChangedOn",
                table: "Users",
                column: "PasswordChangedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Users_PhoneE164Formatted",
                table: "Users",
                column: "PhoneE164Formatted");

            migrationBuilder.CreateIndex(
                name: "IX_Users_PhoneVerifiedById",
                table: "Users",
                column: "PhoneVerifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RealmId_EmailAddressNormalized",
                table: "Users",
                columns: new[] { "RealmId", "EmailAddressNormalized" });

            migrationBuilder.CreateIndex(
                name: "IX_Users_RealmId_UsernameNormalized",
                table: "Users",
                columns: new[] { "RealmId", "UsernameNormalized" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_SignedInOn",
                table: "Users",
                column: "SignedInOn");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UpdatedById",
                table: "Users",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UpdatedOn",
                table: "Users",
                column: "UpdatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExternalIdentifiers");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
