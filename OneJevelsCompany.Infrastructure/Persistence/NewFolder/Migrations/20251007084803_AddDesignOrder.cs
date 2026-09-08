using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OneJevelsCompany.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddDesignOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "UnitPriceEstimate",
                table: "DesignOrders",
                type: "decimal(14,2)",
                precision: 14,
                scale: 2,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "LengthCm",
                table: "DesignOrders",
                type: "decimal(6,2)",
                precision: 6,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<string>(
                name: "AdminNotes",
                table: "DesignOrders",
                type: "nvarchar(4000)",
                maxLength: 4000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DesignName",
                table: "DesignOrders",
                type: "nvarchar(120)",
                maxLength: 120,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PreviewBeads",
                table: "DesignOrders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PreviewDataUrl",
                table: "DesignOrders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DesignOrders_CreatedUtc",
                table: "DesignOrders",
                column: "CreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_DesignOrders_Status",
                table: "DesignOrders",
                column: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_DesignOrders_CreatedUtc",
                table: "DesignOrders");

            migrationBuilder.DropIndex(
                name: "IX_DesignOrders_Status",
                table: "DesignOrders");

            migrationBuilder.DropColumn(
                name: "AdminNotes",
                table: "DesignOrders");

            migrationBuilder.DropColumn(
                name: "DesignName",
                table: "DesignOrders");

            migrationBuilder.DropColumn(
                name: "PreviewBeads",
                table: "DesignOrders");

            migrationBuilder.DropColumn(
                name: "PreviewDataUrl",
                table: "DesignOrders");

            migrationBuilder.AlterColumn<decimal>(
                name: "UnitPriceEstimate",
                table: "DesignOrders",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(14,2)",
                oldPrecision: 14,
                oldScale: 2,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "LengthCm",
                table: "DesignOrders",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(6,2)",
                oldPrecision: 6,
                oldScale: 2);
        }
    }
}
