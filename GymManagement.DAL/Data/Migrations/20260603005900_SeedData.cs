using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GymManagement.DAL.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Photo",
                table: "Members",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Weight",
                table: "HealthRecords",
                type: "decimal(5,2)",
                precision: 5,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Height",
                table: "HealthRecords",
                type: "decimal(5,2)",
                precision: 5,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.InsertData(
                table: "Members",
                columns: new[] { "Id", "DateOfBirth", "Email", "Gender", "Name", "PhoneNumber", "Photo", "UpdatedAt", "Address_BuildingNumber", "Address_City", "Address_Street" },
                values: new object[,]
                {
                    { 1, new DateOnly(1995, 5, 15), "m.salah@example.com", "Male", "Mohamed Salah", "01000000010", null, null, 10, "Cairo", "El Nile St" },
                    { 2, new DateOnly(1998, 3, 22), "aya.ibrahim@example.com", "Female", "Aya Ibrahim", "01000000011", null, null, 5, "Cairo", "Tahrir Ave" }
                });

            migrationBuilder.InsertData(
                table: "Plans",
                columns: new[] { "Id", "Description", "DurationDays", "IsActive", "Name", "Price", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "Basic monthly plan", 30, true, "Basic", 19.99m, null },
                    { 2, "Standard 3-month plan", 90, true, "Standard", 49.99m, null },
                    { 3, "Yearly premium plan", 365, true, "Premium", 199.99m, null }
                });

            migrationBuilder.InsertData(
                table: "Trainers",
                columns: new[] { "Id", "DateOfBirth", "Email", "Gender", "Name", "PhoneNumber", "Speciality", "UpdatedAt", "Address_BuildingNumber", "Address_City", "Address_Street" },
                values: new object[,]
                {
                    { 1, new DateOnly(1990, 1, 1), "ahmed.ali@example.com", "Male", "Ahmed Ali", "01000000001", "GeneralFitness", null, 10, "Cairo", "El Nile St" },
                    { 2, new DateOnly(1992, 6, 15), "sara.hassan@example.com", "Female", "Sara Hassan", "01000000002", "Yoga", null, 5, "Cairo", "Tahrir Ave" }
                });

            migrationBuilder.InsertData(
                table: "HealthRecords",
                columns: new[] { "Id", "BloodType", "CreatedAt", "Height", "MemberId", "Note", "UpdatedAt", "Weight" },
                values: new object[,]
                {
                    { 1, "Oplus", new DateTime(2026, 6, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 180.5m, 1, "No issues", null, 80.2m },
                    { 2, "Aplus", new DateTime(2025, 5, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 165.0m, 2, "Allergic to nuts", null, 60.0m }
                });

            migrationBuilder.InsertData(
                table: "Memberships",
                columns: new[] { "Id", "StartDate", "EndDate", "MemberId", "PlanId", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 6, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateOnly(2026, 7, 3), 1, 1, null },
                    { 2, new DateTime(2026, 6, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateOnly(2026, 7, 3), 2, 2, null }
                });

            migrationBuilder.InsertData(
                table: "Sessions",
                columns: new[] { "Id", "Capacity", "CategoryId", "CreatedAt", "Description", "EndDate", "StartDate", "TrainerId", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, 15, 1, new DateTime(2026, 6, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "Morning Cardio", new DateTime(2026, 7, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 6, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, null },
                    { 2, 12, 3, new DateTime(2026, 6, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "Evening Yoga", new DateTime(2026, 9, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 6, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "HealthRecords",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "HealthRecords",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Memberships",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Memberships",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Plans",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Plans",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Plans",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Trainers",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Trainers",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.AlterColumn<string>(
                name: "Photo",
                table: "Members",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Weight",
                table: "HealthRecords",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)",
                oldPrecision: 5,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "Height",
                table: "HealthRecords",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)",
                oldPrecision: 5,
                oldScale: 2);
        }
    }
}
