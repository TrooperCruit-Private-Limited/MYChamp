using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MYChamp.Migrations
{
    /// <inheritdoc />
    public partial class leaveDetailAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Session",
                table: "LeaveApplications");

            migrationBuilder.CreateTable(
                name: "LeaveDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LeaveApplicationId = table.Column<int>(type: "integer", nullable: false),
                    LeaveDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LeaveType = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaveDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LeaveDetails_LeaveApplications_LeaveApplicationId",
                        column: x => x.LeaveApplicationId,
                        principalTable: "LeaveApplications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LeaveDetails_LeaveApplicationId",
                table: "LeaveDetails",
                column: "LeaveApplicationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LeaveDetails");

            migrationBuilder.AddColumn<string>(
                name: "Session",
                table: "LeaveApplications",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
