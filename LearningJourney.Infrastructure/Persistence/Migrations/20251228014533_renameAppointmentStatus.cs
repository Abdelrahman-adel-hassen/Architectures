using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearningJourney.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class renameAppointmentStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop FK and index that reference AppointmentStatusId before dropping the column
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_AppointmentStatuses_AppointmentStatusId",
                schema: "CleanArch",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_AppointmentStatusId",
                schema: "CleanArch",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "AppointmentStatusId",
                schema: "CleanArch",
                table: "Appointments");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                schema: "CleanArch",
                table: "Appointments",
                type: "int",
                nullable: false,
                defaultValue: 1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                schema: "CleanArch",
                table: "Appointments");

            migrationBuilder.AddColumn<int>(
                name: "AppointmentStatusId",
                schema: "CleanArch",
                table: "Appointments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            // Recreate index and FK as they existed before
            migrationBuilder.CreateIndex(
                name: "IX_Appointments_AppointmentStatusId",
                schema: "CleanArch",
                table: "Appointments",
                column: "AppointmentStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_AppointmentStatuses_AppointmentStatusId",
                schema: "CleanArch",
                table: "Appointments",
                column: "AppointmentStatusId",
                principalSchema: "CleanArch",
                principalTable: "AppointmentStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}