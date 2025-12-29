using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearningJourney.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUncessaryCurrentTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop FKs that reference the old AppointmentStatusId (we keep the lookup table until mapping finishes)
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_AppointmentStatuses_AppointmentStatusId",
                schema: "CleanArch",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_ScheduleSlots_ScheduleSlotId",
                schema: "CleanArch",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Hospitals_Cities_CityId",
                schema: "CleanArch",
                table: "Hospitals");

            migrationBuilder.DropForeignKey(
                name: "FK_Hospitals_Owners_OwnerId",
                schema: "CleanArch",
                table: "Hospitals");

            // 1) Create new int-based AppointmentStatuses table (temporary name)
            migrationBuilder.CreateTable(
                name: "AppointmentStatuses_New",
                schema: "CleanArch",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppointmentStatuses_New", x => x.Id);
                });

            // Seed the new statuses with the enum values (adjust names if needed)
            migrationBuilder.Sql(@"
                INSERT INTO [CleanArch].[AppointmentStatuses_New] (Id, Name) VALUES
                (1, 'Pending'),
                (2, 'Confirmed'),
                (3, 'Cancelled'),
                (4, 'Completed');
            ");

            // 2) Add new temporary int column to Appointments
            migrationBuilder.AddColumn<int>(
                name: "AppointmentStatusId_New",
                schema: "CleanArch",
                table: "Appointments",
                type: "int",
                nullable: true);

            // 3) Populate AppointmentStatusId_New using JOIN on Name (if old lookup exists),
            // otherwise leave NULLs to be set to default below.
            migrationBuilder.Sql(@"
                IF OBJECT_ID(N'CleanArch.AppointmentStatuses', N'U') IS NOT NULL
                BEGIN
                    UPDATE a
                    SET a.AppointmentStatusId_New = snew.Id
                    FROM [CleanArch].[Appointments] a
                    INNER JOIN [CleanArch].[AppointmentStatuses] sold ON a.AppointmentStatusId = sold.Id
                    INNER JOIN [CleanArch].[AppointmentStatuses_New] snew ON snew.Name = sold.Name;
                END
            ");

            // 4) For safety, set any remaining NULLs to a sensible default (Pending = 1).
            migrationBuilder.Sql(@"
                UPDATE [CleanArch].[Appointments] SET AppointmentStatusId_New = 1 WHERE AppointmentStatusId_New IS NULL;
            ");

            // Drop indexes that referenced the old AppointmentStatusId column (if they exist)
            // Existing migration intended to drop these indexes; keep that behavior.
            migrationBuilder.DropIndex(
                name: "IX_Appointments_AppointmentStatusId",
                schema: "CleanArch",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_ScheduleSlotId",
                schema: "CleanArch",
                table: "Appointments");

            // 6) Rename the new column to a temp name (prepare to drop old GUID column safely)
            migrationBuilder.RenameColumn(
                name: "AppointmentStatusId_New",
                schema: "CleanArch",
                table: "Appointments",
                newName: "AppointmentStatusId_Temp"
            );

            // 7) Drop the old GUID column now that values are copied to the temp int column
            migrationBuilder.DropColumn(
                name: "AppointmentStatusId",
                schema: "CleanArch",
                table: "Appointments"
            );

            // 8) Rename temp column to final column name
            migrationBuilder.RenameColumn(
                name: "AppointmentStatusId_Temp",
                schema: "CleanArch",
                table: "Appointments",
                newName: "AppointmentStatusId"
            );

            // Recreate index on the new int column
            migrationBuilder.CreateIndex(
                name: "IX_Appointments_AppointmentStatusId",
                schema: "CleanArch",
                table: "Appointments",
                column: "AppointmentStatusId");

            // 9) Drop the old AppointmentStatuses table (if it still exists) and put the new table in place
            migrationBuilder.Sql(@"
                IF OBJECT_ID(N'CleanArch.AppointmentStatuses', N'U') IS NOT NULL
                BEGIN
                    DROP TABLE [CleanArch].[AppointmentStatuses];
                END
            ");

            // Rename new lookup table into final name
            migrationBuilder.RenameTable(
                name: "AppointmentStatuses_New",
                schema: "CleanArch",
                newName: "AppointmentStatuses",
                newSchema: "CleanArch");

            // 10) Re-create FK from Appointments to the new int AppointmentStatuses.Id
            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_AppointmentStatuses_AppointmentStatusId",
                schema: "CleanArch",
                table: "Appointments",
                column: "AppointmentStatusId",
                principalSchema: "CleanArch",
                principalTable: "AppointmentStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            // Continue with the rest of your original destructive cleanup
            migrationBuilder.DropTable(
                name: "Attachments",
                schema: "CleanArch");

            migrationBuilder.DropTable(
                name: "Cities",
                schema: "CleanArch");

            migrationBuilder.DropTable(
                name: "Comments",
                schema: "CleanArch");

            migrationBuilder.DropTable(
                name: "Owners",
                schema: "CleanArch");

            migrationBuilder.DropTable(
                name: "ScheduleSlots",
                schema: "CleanArch");

            migrationBuilder.DropIndex(
                name: "IX_Hospitals_CityId",
                schema: "CleanArch",
                table: "Hospitals");

            migrationBuilder.DropIndex(
                name: "IX_Hospitals_OwnerId",
                schema: "CleanArch",
                table: "Hospitals");

            migrationBuilder.DropColumn(
                name: "CityId",
                schema: "CleanArch",
                table: "Hospitals");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                schema: "CleanArch",
                table: "Hospitals");

            migrationBuilder.DropColumn(
                name: "SpecialtyId",
                schema: "CleanArch",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "ScheduleSlotId",
                schema: "CleanArch",
                table: "Appointments");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CityId",
                schema: "CleanArch",
                table: "Hospitals",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "OwnerId",
                schema: "CleanArch",
                table: "Hospitals",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SpecialtyId",
                schema: "CleanArch",
                table: "Doctors",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<Guid>(
                name: "AppointmentStatusId",
                schema: "CleanArch",
                table: "Appointments",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<Guid>(
                name: "ScheduleSlotId",
                schema: "CleanArch",
                table: "Appointments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "AppointmentStatuses",
                schema: "CleanArch",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppointmentStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Attachments",
                schema: "CleanArch",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AppointmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Attachments_Appointments_AppointmentId",
                        column: x => x.AppointmentId,
                        principalSchema: "CleanArch",
                        principalTable: "Appointments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cities",
                schema: "CleanArch",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                schema: "CleanArch",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AppointmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_Appointments_AppointmentId",
                        column: x => x.AppointmentId,
                        principalSchema: "CleanArch",
                        principalTable: "Appointments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Owners",
                schema: "CleanArch",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Owners", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ScheduleSlots",
                schema: "CleanArch",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DoctorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsBooked = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleSlots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScheduleSlots_Doctors_DoctorId",
                        column: x => x.DoctorId,
                        principalSchema: "CleanArch",
                        principalTable: "Doctors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Hospitals_CityId",
                schema: "CleanArch",
                table: "Hospitals",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Hospitals_OwnerId",
                schema: "CleanArch",
                table: "Hospitals",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_AppointmentStatusId",
                schema: "CleanArch",
                table: "Appointments",
                column: "AppointmentStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_ScheduleSlotId",
                schema: "CleanArch",
                table: "Appointments",
                column: "ScheduleSlotId");

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_AppointmentId",
                schema: "CleanArch",
                table: "Attachments",
                column: "AppointmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_AppointmentId",
                schema: "CleanArch",
                table: "Comments",
                column: "AppointmentId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleSlots_DoctorId",
                schema: "CleanArch",
                table: "ScheduleSlots",
                column: "DoctorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_AppointmentStatuses_AppointmentStatusId",
                schema: "CleanArch",
                table: "Appointments",
                column: "AppointmentStatusId",
                principalSchema: "CleanArch",
                principalTable: "AppointmentStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_ScheduleSlots_ScheduleSlotId",
                schema: "CleanArch",
                table: "Appointments",
                column: "ScheduleSlotId",
                principalSchema: "CleanArch",
                principalTable: "ScheduleSlots",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Hospitals_Cities_CityId",
                schema: "CleanArch",
                table: "Hospitals",
                column: "CityId",
                principalSchema: "CleanArch",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Hospitals_Owners_OwnerId",
                schema: "CleanArch",
                table: "Hospitals",
                column: "OwnerId",
                principalSchema: "CleanArch",
                principalTable: "Owners",
                principalColumn: "Id");
        }
    }
}