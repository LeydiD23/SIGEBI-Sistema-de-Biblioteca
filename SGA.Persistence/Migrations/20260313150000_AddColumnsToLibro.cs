using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SGA.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnsToLibro : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Editorial",
                table: "Libros",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Estado",
                table: "Libros",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaAdquisicion",
                table: "Libros",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ISBN",
                table: "Libros",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StockDisponible",
                table: "Libros",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Ubicacion",
                table: "Libros",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Editorial",
                table: "Libros");

            migrationBuilder.DropColumn(
                name: "Estado",
                table: "Libros");

            migrationBuilder.DropColumn(
                name: "FechaAdquisicion",
                table: "Libros");

            migrationBuilder.DropColumn(
                name: "ISBN",
                table: "Libros");

            migrationBuilder.DropColumn(
                name: "StockDisponible",
                table: "Libros");

            migrationBuilder.DropColumn(
                name: "Ubicacion",
                table: "Libros");
        }
    }
}
