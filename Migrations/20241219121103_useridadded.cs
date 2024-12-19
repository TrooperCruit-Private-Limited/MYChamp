using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MYChamp.Migrations
{
    /// <inheritdoc />
    public partial class useridadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserID",
                table: "LeaveApplications",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserID",
                table: "LeaveApplications");
        }
    }
}
