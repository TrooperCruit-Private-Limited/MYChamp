using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MYChamp.Migrations
{
    /// <inheritdoc />
    public partial class leaveDetaildatatype : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LeaveDetails_LeaveApplications_LeaveApplicationId",
                table: "LeaveDetails");

            migrationBuilder.DropIndex(
                name: "IX_LeaveDetails_LeaveApplicationId",
                table: "LeaveDetails");

            migrationBuilder.AlterColumn<string>(
                name: "LeaveApplicationId",
                table: "LeaveDetails",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "LeaveApplicationId1",
                table: "LeaveDetails",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LeaveDetails_LeaveApplicationId1",
                table: "LeaveDetails",
                column: "LeaveApplicationId1");

            migrationBuilder.AddForeignKey(
                name: "FK_LeaveDetails_LeaveApplications_LeaveApplicationId1",
                table: "LeaveDetails",
                column: "LeaveApplicationId1",
                principalTable: "LeaveApplications",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LeaveDetails_LeaveApplications_LeaveApplicationId1",
                table: "LeaveDetails");

            migrationBuilder.DropIndex(
                name: "IX_LeaveDetails_LeaveApplicationId1",
                table: "LeaveDetails");

            migrationBuilder.DropColumn(
                name: "LeaveApplicationId1",
                table: "LeaveDetails");

            migrationBuilder.AlterColumn<int>(
                name: "LeaveApplicationId",
                table: "LeaveDetails",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveDetails_LeaveApplicationId",
                table: "LeaveDetails",
                column: "LeaveApplicationId");

            migrationBuilder.AddForeignKey(
                name: "FK_LeaveDetails_LeaveApplications_LeaveApplicationId",
                table: "LeaveDetails",
                column: "LeaveApplicationId",
                principalTable: "LeaveApplications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
