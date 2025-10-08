using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OneJevelsCompany.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddPurchaseNeedsAndComponentMOQ : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MinOrderQty",
                table: "Components",
                type: "int",
                nullable: false,
                defaultValue: 120);

            migrationBuilder.CreateTable(
                name: "PurchaseNeeds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ComponentId = table.Column<int>(type: "int", nullable: false),
                    NeededQty = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    MinOrderQtyUsed = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    SourcesJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    LastUpdatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseNeeds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseNeeds_Components_ComponentId",
                        column: x => x.ComponentId,
                        principalTable: "Components",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseNeeds_ComponentId",
                table: "PurchaseNeeds",
                column: "ComponentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PurchaseNeeds");

            migrationBuilder.DropColumn(
                name: "MinOrderQty",
                table: "Components");
        }
    }
}
