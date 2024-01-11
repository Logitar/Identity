using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Logitar.Identity.EntityFrameworkCore.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class CreateCustomAttributeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomAttributes",
                columns: table => new
                {
                    CustomAttributeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntityType = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    EntityId = table.Column<int>(type: "int", nullable: false),
                    Key = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ValueShortened = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomAttributes", x => x.CustomAttributeId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomAttributes_EntityType_EntityId",
                table: "CustomAttributes",
                columns: new[] { "EntityType", "EntityId" });

            migrationBuilder.CreateIndex(
                name: "IX_CustomAttributes_EntityType_EntityId_Key",
                table: "CustomAttributes",
                columns: new[] { "EntityType", "EntityId", "Key" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomAttributes_Key",
                table: "CustomAttributes",
                column: "Key");

            migrationBuilder.CreateIndex(
                name: "IX_CustomAttributes_ValueShortened",
                table: "CustomAttributes",
                column: "ValueShortened");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomAttributes");
        }
    }
}
