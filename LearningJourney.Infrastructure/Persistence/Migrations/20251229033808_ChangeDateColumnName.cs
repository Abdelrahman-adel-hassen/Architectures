using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearningJourney.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ChangeDateColumnName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AppointmentDate",
                schema: "CleanArch",
                table: "Appointments",
                newName: "Date");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Date",
                schema: "CleanArch",
                table: "Appointments",
                newName: "AppointmentDate");
        }
    }
}
