using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MYChamp.Migrations
{
    /// <inheritdoc />
    public partial class addeduserId : Migration
    {

            protected override void Up(MigrationBuilder migrationBuilder)
            {
                migrationBuilder.DropPrimaryKey(
                     name: "PK_Attendence",
                     table: "Attendence");

                // Add new Id column with auto-increment
                migrationBuilder.AddColumn<int>(
                    name: "Id",
                    table: "Attendence",
                    nullable: false,
                    defaultValue: 0)
                    .Annotation("SqlServer:Identity", "1, 1");

                // Add new primary key
                migrationBuilder.AddPrimaryKey(
                    name: "PK_Attendence",
                    table: "Attendence",
                    column: "Id");

                // Add LeaveType column
                migrationBuilder.AddColumn<int>(
                    name: "LeaveType",
                    table: "Attendence",
                    nullable: false,
                    defaultValue: 0);
            }

            /// <inheritdoc />
            protected override void Down(MigrationBuilder migrationBuilder)
            {
                migrationBuilder.DropColumn(
            name: "LeaveType",
            table: "Attendence");

                // Drop the new primary key
                migrationBuilder.DropPrimaryKey(
                    name: "PK_Attendence",
                    table: "Attendence");

                // Remove the Id column
                migrationBuilder.DropColumn(
                    name: "Id",
                    table: "Attendence");

                // Only drop the primary key from MarkedAttendance, without restoring it
                migrationBuilder.AddPrimaryKey(
                    name: "PK_Attendence",
                    table: "Attendence",
                    column: "MarkedAttendance");
            }
        }

    
}
