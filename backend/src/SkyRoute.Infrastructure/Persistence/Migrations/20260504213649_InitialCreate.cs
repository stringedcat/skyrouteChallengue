using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkyRoute.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    Reference = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Flight_FlightId = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Flight_Provider = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Flight_FlightNumber = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Flight_Origin_Code = table.Column<string>(type: "TEXT", maxLength: 3, nullable: false),
                    Flight_Origin_Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Flight_Origin_City = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Flight_Origin_Country = table.Column<string>(type: "TEXT", maxLength: 2, nullable: false),
                    Flight_Origin_CountryName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Flight_Destination_Code = table.Column<string>(type: "TEXT", maxLength: 3, nullable: false),
                    Flight_Destination_Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Flight_Destination_City = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Flight_Destination_Country = table.Column<string>(type: "TEXT", maxLength: 2, nullable: false),
                    Flight_Destination_CountryName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Flight_DepartureTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Flight_ArrivalTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Flight_CabinClass = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Flight_BaseFare_Amount = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false),
                    Flight_BaseFare_Currency = table.Column<string>(type: "TEXT", maxLength: 3, nullable: false),
                    PricePerPassenger_Amount = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false),
                    PricePerPassenger_Currency = table.Column<string>(type: "TEXT", maxLength: 3, nullable: false),
                    TotalPrice_Amount = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false),
                    TotalPrice_Currency = table.Column<string>(type: "TEXT", maxLength: 3, nullable: false),
                    Status = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.Reference);
                });

            migrationBuilder.CreateTable(
                name: "BookingPassengers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FullName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    DocumentNumber = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    BookingReference = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingPassengers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookingPassengers_Bookings_BookingReference",
                        column: x => x.BookingReference,
                        principalTable: "Bookings",
                        principalColumn: "Reference",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookingPassengers_BookingReference",
                table: "BookingPassengers",
                column: "BookingReference");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookingPassengers");

            migrationBuilder.DropTable(
                name: "Bookings");
        }
    }
}
