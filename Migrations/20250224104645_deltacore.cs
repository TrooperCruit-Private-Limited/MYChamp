using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MYChamp.Migrations
{
    /// <inheritdoc />
    public partial class deltacore : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmployeeLists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    JobTitle = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Phone = table.Column<string>(type: "text", nullable: false),
                    SlotDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SlotStart = table.Column<TimeSpan>(type: "interval", nullable: false),
                    SlotEnd = table.Column<TimeSpan>(type: "interval", nullable: false),
                    Book = table.Column<bool>(type: "boolean", nullable: false),
                    IsBooked = table.Column<bool>(type: "boolean", nullable: false),
                    IsCanceled = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeLists", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SlotBookingDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmployeeId = table.Column<int>(type: "integer", nullable: false),
                    SlotDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "interval", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "interval", nullable: false),
                    IsBooked = table.Column<bool>(type: "boolean", nullable: false),
                    IsCanceled = table.Column<bool>(type: "boolean", nullable: false),
                    BookedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    BookedByEmail = table.Column<string>(type: "text", nullable: true),
                    BookedByPhone = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsConfirmed = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SlotBookingDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SlotBookingDetails_EmployeeLists_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "EmployeeLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SlotBookingDetails_EmployeeId",
                table: "SlotBookingDetails",
                column: "EmployeeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SlotBookingDetails");

            migrationBuilder.DropTable(
                name: "EmployeeLists");
        }
    }
}
