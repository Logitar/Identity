using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Logitar.Identity.EntityFrameworkCore.PostgreSQL.Migrations
{
    /// <inheritdoc />
    public partial class Release_3_0_0 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Identity");

            migrationBuilder.CreateTable(
                name: "Actors",
                schema: "Identity",
                columns: table => new
                {
                    ActorId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Id = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Type = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DisplayName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    EmailAddress = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    PictureUrl = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Actors", x => x.ActorId);
                });

            migrationBuilder.CreateTable(
                name: "ApiKeys",
                schema: "Identity",
                columns: table => new
                {
                    ApiKeyId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    EntityId = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    SecretHash = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    DisplayName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    ExpiresOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    AuthenticatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CustomAttributes = table.Column<string>(type: "text", nullable: true),
                    StreamId = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiKeys", x => x.ApiKeyId);
                });

            migrationBuilder.CreateTable(
                name: "CustomAttributes",
                schema: "Identity",
                columns: table => new
                {
                    CustomAttributeId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EntityType = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    EntityId = table.Column<int>(type: "integer", nullable: false),
                    Key = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Value = table.Column<string>(type: "text", nullable: false),
                    ValueShortened = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomAttributes", x => x.CustomAttributeId);
                });

            migrationBuilder.CreateTable(
                name: "OneTimePasswords",
                schema: "Identity",
                columns: table => new
                {
                    OneTimePasswordId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    EntityId = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    PasswordHash = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    ExpiresOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    MaximumAttempts = table.Column<int>(type: "integer", nullable: true),
                    AttemptCount = table.Column<int>(type: "integer", nullable: false),
                    HasValidationSucceeded = table.Column<bool>(type: "boolean", nullable: false),
                    CustomAttributes = table.Column<string>(type: "text", nullable: true),
                    StreamId = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OneTimePasswords", x => x.OneTimePasswordId);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                schema: "Identity",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    EntityId = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    UniqueName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    UniqueNameNormalized = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    DisplayName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    CustomAttributes = table.Column<string>(type: "text", nullable: true),
                    StreamId = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "TokenBlacklist",
                schema: "Identity",
                columns: table => new
                {
                    BlacklistedTokenId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TokenId = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    ExpiresOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TokenBlacklist", x => x.BlacklistedTokenId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "Identity",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    EntityId = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    UniqueName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    UniqueNameNormalized = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    PasswordHash = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    PasswordChangedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    PasswordChangedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    HasPassword = table.Column<bool>(type: "boolean", nullable: false),
                    DisabledBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    DisabledOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDisabled = table.Column<bool>(type: "boolean", nullable: false),
                    AddressStreet = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    AddressLocality = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    AddressPostalCode = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    AddressRegion = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    AddressCountry = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    AddressFormatted = table.Column<string>(type: "character varying(1279)", maxLength: 1279, nullable: true),
                    AddressVerifiedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    AddressVerifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsAddressVerified = table.Column<bool>(type: "boolean", nullable: false),
                    EmailAddress = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    EmailAddressNormalized = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    EmailVerifiedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    EmailVerifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsEmailVerified = table.Column<bool>(type: "boolean", nullable: false),
                    PhoneCountryCode = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: true),
                    PhoneNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    PhoneExtension = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    PhoneE164Formatted = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    PhoneVerifiedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    PhoneVerifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsPhoneVerified = table.Column<bool>(type: "boolean", nullable: false),
                    IsConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    FirstName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    MiddleName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    LastName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    FullName = table.Column<string>(type: "character varying(767)", maxLength: 767, nullable: true),
                    Nickname = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Birthdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Gender = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Locale = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: true),
                    TimeZone = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    Picture = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    Profile = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    Website = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    AuthenticatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CustomAttributes = table.Column<string>(type: "text", nullable: true),
                    StreamId = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "ApiKeyRoles",
                schema: "Identity",
                columns: table => new
                {
                    ApiKeyId = table.Column<int>(type: "integer", nullable: false),
                    RoleId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiKeyRoles", x => new { x.ApiKeyId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_ApiKeyRoles_ApiKeys_ApiKeyId",
                        column: x => x.ApiKeyId,
                        principalSchema: "Identity",
                        principalTable: "ApiKeys",
                        principalColumn: "ApiKeyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApiKeyRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "Identity",
                        principalTable: "Roles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sessions",
                schema: "Identity",
                columns: table => new
                {
                    SessionId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    EntityId = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    SecretHash = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    IsPersistent = table.Column<bool>(type: "boolean", nullable: false),
                    SignedOutBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    SignedOutOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CustomAttributes = table.Column<string>(type: "text", nullable: true),
                    StreamId = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sessions", x => x.SessionId);
                    table.ForeignKey(
                        name: "FK_Sessions_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "Identity",
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserIdentifiers",
                schema: "Identity",
                columns: table => new
                {
                    UserIdentifierId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    TenantId = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Key = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Value = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserIdentifiers", x => x.UserIdentifierId);
                    table.ForeignKey(
                        name: "FK_UserIdentifiers_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "Identity",
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                schema: "Identity",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    RoleId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "Identity",
                        principalTable: "Roles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "Identity",
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Actors_DisplayName",
                schema: "Identity",
                table: "Actors",
                column: "DisplayName");

            migrationBuilder.CreateIndex(
                name: "IX_Actors_EmailAddress",
                schema: "Identity",
                table: "Actors",
                column: "EmailAddress");

            migrationBuilder.CreateIndex(
                name: "IX_Actors_Id",
                schema: "Identity",
                table: "Actors",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Actors_IsDeleted",
                schema: "Identity",
                table: "Actors",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Actors_Type",
                schema: "Identity",
                table: "Actors",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_ApiKeyRoles_RoleId",
                schema: "Identity",
                table: "ApiKeyRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_ApiKeys_AuthenticatedOn",
                schema: "Identity",
                table: "ApiKeys",
                column: "AuthenticatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_ApiKeys_CreatedBy",
                schema: "Identity",
                table: "ApiKeys",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ApiKeys_CreatedOn",
                schema: "Identity",
                table: "ApiKeys",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_ApiKeys_DisplayName",
                schema: "Identity",
                table: "ApiKeys",
                column: "DisplayName");

            migrationBuilder.CreateIndex(
                name: "IX_ApiKeys_EntityId",
                schema: "Identity",
                table: "ApiKeys",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_ApiKeys_ExpiresOn",
                schema: "Identity",
                table: "ApiKeys",
                column: "ExpiresOn");

            migrationBuilder.CreateIndex(
                name: "IX_ApiKeys_StreamId",
                schema: "Identity",
                table: "ApiKeys",
                column: "StreamId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApiKeys_TenantId_EntityId",
                schema: "Identity",
                table: "ApiKeys",
                columns: new[] { "TenantId", "EntityId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApiKeys_UpdatedBy",
                schema: "Identity",
                table: "ApiKeys",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ApiKeys_UpdatedOn",
                schema: "Identity",
                table: "ApiKeys",
                column: "UpdatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_ApiKeys_Version",
                schema: "Identity",
                table: "ApiKeys",
                column: "Version");

            migrationBuilder.CreateIndex(
                name: "IX_CustomAttributes_EntityType_EntityId",
                schema: "Identity",
                table: "CustomAttributes",
                columns: new[] { "EntityType", "EntityId" });

            migrationBuilder.CreateIndex(
                name: "IX_CustomAttributes_EntityType_EntityId_Key",
                schema: "Identity",
                table: "CustomAttributes",
                columns: new[] { "EntityType", "EntityId", "Key" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomAttributes_Key",
                schema: "Identity",
                table: "CustomAttributes",
                column: "Key");

            migrationBuilder.CreateIndex(
                name: "IX_CustomAttributes_ValueShortened",
                schema: "Identity",
                table: "CustomAttributes",
                column: "ValueShortened");

            migrationBuilder.CreateIndex(
                name: "IX_OneTimePasswords_AttemptCount",
                schema: "Identity",
                table: "OneTimePasswords",
                column: "AttemptCount");

            migrationBuilder.CreateIndex(
                name: "IX_OneTimePasswords_CreatedBy",
                schema: "Identity",
                table: "OneTimePasswords",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_OneTimePasswords_CreatedOn",
                schema: "Identity",
                table: "OneTimePasswords",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_OneTimePasswords_EntityId",
                schema: "Identity",
                table: "OneTimePasswords",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_OneTimePasswords_ExpiresOn",
                schema: "Identity",
                table: "OneTimePasswords",
                column: "ExpiresOn");

            migrationBuilder.CreateIndex(
                name: "IX_OneTimePasswords_HasValidationSucceeded",
                schema: "Identity",
                table: "OneTimePasswords",
                column: "HasValidationSucceeded");

            migrationBuilder.CreateIndex(
                name: "IX_OneTimePasswords_MaximumAttempts",
                schema: "Identity",
                table: "OneTimePasswords",
                column: "MaximumAttempts");

            migrationBuilder.CreateIndex(
                name: "IX_OneTimePasswords_StreamId",
                schema: "Identity",
                table: "OneTimePasswords",
                column: "StreamId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OneTimePasswords_TenantId_EntityId",
                schema: "Identity",
                table: "OneTimePasswords",
                columns: new[] { "TenantId", "EntityId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OneTimePasswords_UpdatedBy",
                schema: "Identity",
                table: "OneTimePasswords",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_OneTimePasswords_UpdatedOn",
                schema: "Identity",
                table: "OneTimePasswords",
                column: "UpdatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_OneTimePasswords_Version",
                schema: "Identity",
                table: "OneTimePasswords",
                column: "Version");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_CreatedBy",
                schema: "Identity",
                table: "Roles",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_CreatedOn",
                schema: "Identity",
                table: "Roles",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_DisplayName",
                schema: "Identity",
                table: "Roles",
                column: "DisplayName");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_EntityId",
                schema: "Identity",
                table: "Roles",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_StreamId",
                schema: "Identity",
                table: "Roles",
                column: "StreamId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Roles_TenantId_EntityId",
                schema: "Identity",
                table: "Roles",
                columns: new[] { "TenantId", "EntityId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Roles_TenantId_UniqueNameNormalized",
                schema: "Identity",
                table: "Roles",
                columns: new[] { "TenantId", "UniqueNameNormalized" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Roles_UniqueName",
                schema: "Identity",
                table: "Roles",
                column: "UniqueName");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_UpdatedBy",
                schema: "Identity",
                table: "Roles",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_UpdatedOn",
                schema: "Identity",
                table: "Roles",
                column: "UpdatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Version",
                schema: "Identity",
                table: "Roles",
                column: "Version");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_CreatedBy",
                schema: "Identity",
                table: "Sessions",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_CreatedOn",
                schema: "Identity",
                table: "Sessions",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_EntityId",
                schema: "Identity",
                table: "Sessions",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_IsActive",
                schema: "Identity",
                table: "Sessions",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_IsPersistent",
                schema: "Identity",
                table: "Sessions",
                column: "IsPersistent");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_SignedOutBy",
                schema: "Identity",
                table: "Sessions",
                column: "SignedOutBy");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_SignedOutOn",
                schema: "Identity",
                table: "Sessions",
                column: "SignedOutOn");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_StreamId",
                schema: "Identity",
                table: "Sessions",
                column: "StreamId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_TenantId_EntityId",
                schema: "Identity",
                table: "Sessions",
                columns: new[] { "TenantId", "EntityId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_UpdatedBy",
                schema: "Identity",
                table: "Sessions",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_UpdatedOn",
                schema: "Identity",
                table: "Sessions",
                column: "UpdatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_UserId",
                schema: "Identity",
                table: "Sessions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_Version",
                schema: "Identity",
                table: "Sessions",
                column: "Version");

            migrationBuilder.CreateIndex(
                name: "IX_TokenBlacklist_ExpiresOn",
                schema: "Identity",
                table: "TokenBlacklist",
                column: "ExpiresOn");

            migrationBuilder.CreateIndex(
                name: "IX_TokenBlacklist_TokenId",
                schema: "Identity",
                table: "TokenBlacklist",
                column: "TokenId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserIdentifiers_Key",
                schema: "Identity",
                table: "UserIdentifiers",
                column: "Key");

            migrationBuilder.CreateIndex(
                name: "IX_UserIdentifiers_TenantId_Key_Value",
                schema: "Identity",
                table: "UserIdentifiers",
                columns: new[] { "TenantId", "Key", "Value" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserIdentifiers_UserId_Key",
                schema: "Identity",
                table: "UserIdentifiers",
                columns: new[] { "UserId", "Key" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserIdentifiers_Value",
                schema: "Identity",
                table: "UserIdentifiers",
                column: "Value");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                schema: "Identity",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_AddressCountry",
                schema: "Identity",
                table: "Users",
                column: "AddressCountry");

            migrationBuilder.CreateIndex(
                name: "IX_Users_AddressFormatted",
                schema: "Identity",
                table: "Users",
                column: "AddressFormatted");

            migrationBuilder.CreateIndex(
                name: "IX_Users_AddressLocality",
                schema: "Identity",
                table: "Users",
                column: "AddressLocality");

            migrationBuilder.CreateIndex(
                name: "IX_Users_AddressPostalCode",
                schema: "Identity",
                table: "Users",
                column: "AddressPostalCode");

            migrationBuilder.CreateIndex(
                name: "IX_Users_AddressRegion",
                schema: "Identity",
                table: "Users",
                column: "AddressRegion");

            migrationBuilder.CreateIndex(
                name: "IX_Users_AddressStreet",
                schema: "Identity",
                table: "Users",
                column: "AddressStreet");

            migrationBuilder.CreateIndex(
                name: "IX_Users_AddressVerifiedBy",
                schema: "Identity",
                table: "Users",
                column: "AddressVerifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Users_AddressVerifiedOn",
                schema: "Identity",
                table: "Users",
                column: "AddressVerifiedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Users_AuthenticatedOn",
                schema: "Identity",
                table: "Users",
                column: "AuthenticatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Birthdate",
                schema: "Identity",
                table: "Users",
                column: "Birthdate");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CreatedBy",
                schema: "Identity",
                table: "Users",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CreatedOn",
                schema: "Identity",
                table: "Users",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Users_DisabledBy",
                schema: "Identity",
                table: "Users",
                column: "DisabledBy");

            migrationBuilder.CreateIndex(
                name: "IX_Users_DisabledOn",
                schema: "Identity",
                table: "Users",
                column: "DisabledOn");

            migrationBuilder.CreateIndex(
                name: "IX_Users_EmailAddress",
                schema: "Identity",
                table: "Users",
                column: "EmailAddress");

            migrationBuilder.CreateIndex(
                name: "IX_Users_EmailVerifiedBy",
                schema: "Identity",
                table: "Users",
                column: "EmailVerifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Users_EmailVerifiedOn",
                schema: "Identity",
                table: "Users",
                column: "EmailVerifiedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Users_EntityId",
                schema: "Identity",
                table: "Users",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_FirstName",
                schema: "Identity",
                table: "Users",
                column: "FirstName");

            migrationBuilder.CreateIndex(
                name: "IX_Users_FullName",
                schema: "Identity",
                table: "Users",
                column: "FullName");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Gender",
                schema: "Identity",
                table: "Users",
                column: "Gender");

            migrationBuilder.CreateIndex(
                name: "IX_Users_HasPassword",
                schema: "Identity",
                table: "Users",
                column: "HasPassword");

            migrationBuilder.CreateIndex(
                name: "IX_Users_IsAddressVerified",
                schema: "Identity",
                table: "Users",
                column: "IsAddressVerified");

            migrationBuilder.CreateIndex(
                name: "IX_Users_IsConfirmed",
                schema: "Identity",
                table: "Users",
                column: "IsConfirmed");

            migrationBuilder.CreateIndex(
                name: "IX_Users_IsDisabled",
                schema: "Identity",
                table: "Users",
                column: "IsDisabled");

            migrationBuilder.CreateIndex(
                name: "IX_Users_IsEmailVerified",
                schema: "Identity",
                table: "Users",
                column: "IsEmailVerified");

            migrationBuilder.CreateIndex(
                name: "IX_Users_IsPhoneVerified",
                schema: "Identity",
                table: "Users",
                column: "IsPhoneVerified");

            migrationBuilder.CreateIndex(
                name: "IX_Users_LastName",
                schema: "Identity",
                table: "Users",
                column: "LastName");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Locale",
                schema: "Identity",
                table: "Users",
                column: "Locale");

            migrationBuilder.CreateIndex(
                name: "IX_Users_MiddleName",
                schema: "Identity",
                table: "Users",
                column: "MiddleName");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Nickname",
                schema: "Identity",
                table: "Users",
                column: "Nickname");

            migrationBuilder.CreateIndex(
                name: "IX_Users_PasswordChangedBy",
                schema: "Identity",
                table: "Users",
                column: "PasswordChangedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Users_PasswordChangedOn",
                schema: "Identity",
                table: "Users",
                column: "PasswordChangedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Users_PhoneCountryCode",
                schema: "Identity",
                table: "Users",
                column: "PhoneCountryCode");

            migrationBuilder.CreateIndex(
                name: "IX_Users_PhoneE164Formatted",
                schema: "Identity",
                table: "Users",
                column: "PhoneE164Formatted");

            migrationBuilder.CreateIndex(
                name: "IX_Users_PhoneExtension",
                schema: "Identity",
                table: "Users",
                column: "PhoneExtension");

            migrationBuilder.CreateIndex(
                name: "IX_Users_PhoneNumber",
                schema: "Identity",
                table: "Users",
                column: "PhoneNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Users_PhoneVerifiedBy",
                schema: "Identity",
                table: "Users",
                column: "PhoneVerifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Users_PhoneVerifiedOn",
                schema: "Identity",
                table: "Users",
                column: "PhoneVerifiedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Users_StreamId",
                schema: "Identity",
                table: "Users",
                column: "StreamId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_TenantId_EmailAddressNormalized",
                schema: "Identity",
                table: "Users",
                columns: new[] { "TenantId", "EmailAddressNormalized" });

            migrationBuilder.CreateIndex(
                name: "IX_Users_TenantId_EntityId",
                schema: "Identity",
                table: "Users",
                columns: new[] { "TenantId", "EntityId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_TenantId_UniqueNameNormalized",
                schema: "Identity",
                table: "Users",
                columns: new[] { "TenantId", "UniqueNameNormalized" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_TimeZone",
                schema: "Identity",
                table: "Users",
                column: "TimeZone");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UniqueName",
                schema: "Identity",
                table: "Users",
                column: "UniqueName");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UpdatedBy",
                schema: "Identity",
                table: "Users",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UpdatedOn",
                schema: "Identity",
                table: "Users",
                column: "UpdatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Version",
                schema: "Identity",
                table: "Users",
                column: "Version");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Actors",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "ApiKeyRoles",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "CustomAttributes",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "OneTimePasswords",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "Sessions",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "TokenBlacklist",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "UserIdentifiers",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "UserRoles",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "ApiKeys",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "Roles",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "Identity");
        }
    }
}
