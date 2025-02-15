using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MYChamp.Migrations
{
    /// <inheritdoc />
    public partial class Initialsetup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop 'Session' column from LeaveApplications
            migrationBuilder.DropColumn(
                name: "Session",
                table: "LeaveApplications"
            );

            // Create LeaveDetails table
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

            // Add 'IsHalfDay' and 'Session' columns to LeaveApplications
            migrationBuilder.AddColumn<bool>(
                name: "IsHalfDay",
                table: "LeaveApplications",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Session",
                table: "LeaveApplications",
                type: "text",
                nullable: false,
                defaultValue: "");

            // Update data type for RemainingLeaves in Employees
            migrationBuilder.AlterColumn<double>(
                name: "RemainingLeaves",
                table: "Employees",
                type: "double precision",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            // Update data type for NumberOfDays in LeaveApplications
            migrationBuilder.AlterColumn<double>(
                name: "NumberOfDays",
                table: "LeaveApplications",
                type: "double precision",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            // Create Attendence table
            migrationBuilder.CreateTable(
                name: "Attendence",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LeaveType = table.Column<int>(type: "integer", nullable: false),
                    MarkedAttendence = table.Column<int>(type: "integer", nullable: false),
                    UserID = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attendence", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop Attendence table
            migrationBuilder.DropTable(
                name: "Attendence");

            // Revert data type for NumberOfDays in LeaveApplications
            migrationBuilder.AlterColumn<int>(
                name: "NumberOfDays",
                table: "LeaveApplications",
                type: "integer",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision");

            // Revert data type for RemainingLeaves in Employees
            migrationBuilder.AlterColumn<int>(
                name: "RemainingLeaves",
                table: "Employees",
                type: "integer",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision");

            // Drop 'IsHalfDay' and 'Session' columns from LeaveApplications
            migrationBuilder.DropColumn(
                name: "IsHalfDay",
                table: "LeaveApplications");

            migrationBuilder.DropColumn(
                name: "Session",
                table: "LeaveApplications");

            // Drop LeaveDetails table
            migrationBuilder.DropTable(
                name: "LeaveDetails");

            // Add 'Session' column back to LeaveApplications
            migrationBuilder.AddColumn<string>(
                name: "Session",
                table: "LeaveApplications",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
