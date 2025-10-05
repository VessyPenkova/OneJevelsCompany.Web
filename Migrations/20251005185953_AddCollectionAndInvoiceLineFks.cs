using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OneJevelsCompany.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddCollectionAndInvoiceLineFks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceLines_Components_ComponentId",
                table: "InvoiceLines");

            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceLines_Jewels_JewelId",
                table: "InvoiceLines");

            migrationBuilder.AddColumn<int>(
                name: "CollectionId",
                table: "OrderItems",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CollectionId",
                table: "InvoiceLines",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Collections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    BasePrice = table.Column<decimal>(type: "decimal(14,2)", precision: 14, scale: 2, nullable: false),
                    QuantityOnHand = table.Column<int>(type: "int", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Collections", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceLines_CollectionId",
                table: "InvoiceLines",
                column: "CollectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceLines_Collections_CollectionId",
                table: "InvoiceLines",
                column: "CollectionId",
                principalTable: "Collections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceLines_Components_ComponentId",
                table: "InvoiceLines",
                column: "ComponentId",
                principalTable: "Components",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceLines_Jewels_JewelId",
                table: "InvoiceLines",
                column: "JewelId",
                principalTable: "Jewels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceLines_Collections_CollectionId",
                table: "InvoiceLines");

            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceLines_Components_ComponentId",
                table: "InvoiceLines");

            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceLines_Jewels_JewelId",
                table: "InvoiceLines");

            migrationBuilder.DropTable(
                name: "Collections");

            migrationBuilder.DropIndex(
                name: "IX_InvoiceLines_CollectionId",
                table: "InvoiceLines");

            migrationBuilder.DropColumn(
                name: "CollectionId",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "CollectionId",
                table: "InvoiceLines");

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceLines_Components_ComponentId",
                table: "InvoiceLines",
                column: "ComponentId",
                principalTable: "Components",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceLines_Jewels_JewelId",
                table: "InvoiceLines",
                column: "JewelId",
                principalTable: "Jewels",
                principalColumn: "Id");
        }
    }
}
