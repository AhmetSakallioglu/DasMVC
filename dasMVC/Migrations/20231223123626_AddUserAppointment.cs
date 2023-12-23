using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dasMVC.Migrations
{
    public partial class AddUserAppointment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserAppointments",
                columns: table => new
                {
                    UserAppointmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AppointmentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAppointments", x => x.UserAppointmentId);
                    table.ForeignKey(
                        name: "FK_UserAppointments_Appointments_AppointmentId",
                        column: x => x.AppointmentId,
                        principalTable: "Appointments",
                        principalColumn: "AppointmentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserAppointments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserAppointments_AppointmentId",
                table: "UserAppointments",
                column: "AppointmentId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAppointments_UserId",
                table: "UserAppointments",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserAppointments");
        }
    }
}
