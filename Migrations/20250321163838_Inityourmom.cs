using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GlamoraApi.Migrations
{
    /// <inheritdoc />
    public partial class Inityourmom : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ProductCreatedDate",
                table: "Products",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "ProductDiscount",
                table: "Products",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductCreatedDate",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ProductDiscount",
                table: "Products");
        }
    }
}
