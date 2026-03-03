using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TaxCalculator.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitializeTaxDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "tax");

            migrationBuilder.CreateTable(
                name: "TaxBandTypes",
                schema: "tax",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MinRange = table.Column<int>(type: "int", nullable: false),
                    MaxRange = table.Column<int>(type: "int", nullable: false),
                    TaxRate = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxBandTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TaxCalculations",
                schema: "tax",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GrossAnnualSalary = table.Column<double>(type: "float(18)", precision: 18, scale: 2, nullable: false),
                    GrossMonthlySalary = table.Column<double>(type: "float(18)", precision: 18, scale: 2, nullable: false),
                    NetAnnualSalary = table.Column<double>(type: "float(18)", precision: 18, scale: 2, nullable: false),
                    NetMonthlySalary = table.Column<double>(type: "float(18)", precision: 18, scale: 2, nullable: false),
                    AnnualTaxPaid = table.Column<double>(type: "float(18)", precision: 18, scale: 2, nullable: false),
                    MonthlyTaxPaid = table.Column<double>(type: "float(18)", precision: 18, scale: 2, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxCalculations", x => x.Id);
                });

            migrationBuilder.InsertData(
                schema: "tax",
                table: "TaxBandTypes",
                columns: new[] { "Id", "CreatedDate", "MaxRange", "MinRange", "Name", "TaxRate" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 3, 3, 9, 51, 26, 276, DateTimeKind.Utc).AddTicks(9190), 5000, 0, "Tax Band A", 0 },
                    { 2, new DateTime(2026, 3, 3, 9, 51, 26, 276, DateTimeKind.Utc).AddTicks(9222), 20000, 5000, "Tax Band B", 20 },
                    { 3, new DateTime(2026, 3, 3, 9, 51, 26, 276, DateTimeKind.Utc).AddTicks(9225), 2147483647, 20000, "Tax Band C", 40 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaxBandTypes",
                schema: "tax");

            migrationBuilder.DropTable(
                name: "TaxCalculations",
                schema: "tax");
        }
    }
}
