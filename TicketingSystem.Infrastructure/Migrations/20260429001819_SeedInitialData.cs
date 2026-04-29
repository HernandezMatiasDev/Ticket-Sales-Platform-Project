using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TicketingSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedInitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DocumentNumber",
                table: "AspNetUsers");

            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "Id", "EventDate", "Name", "Status", "Venue" },
                values: new object[] { 1, new DateTime(2025, 12, 31, 20, 0, 0, 0, DateTimeKind.Utc), "Concierto de Rock", "Active", "Estadio Municipal" });

            migrationBuilder.InsertData(
                table: "Sectors",
                columns: new[] { "Id", "EventId", "Name", "Price" },
                values: new object[,]
                {
                    { 1, 1, "VIP", 5000m },
                    { 2, 1, "General", 2000m }
                });

            migrationBuilder.InsertData(
                table: "Seats",
                columns: new[] { "Id", "Number", "Row", "SectorId", "Status" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-000000000001"), 1, "V", 1, 0 },
                    { new Guid("11111111-1111-1111-1111-000000000002"), 2, "V", 1, 0 },
                    { new Guid("11111111-1111-1111-1111-000000000003"), 3, "V", 1, 0 },
                    { new Guid("11111111-1111-1111-1111-000000000004"), 4, "V", 1, 0 },
                    { new Guid("11111111-1111-1111-1111-000000000005"), 5, "V", 1, 0 },
                    { new Guid("11111111-1111-1111-1111-000000000006"), 6, "V", 1, 0 },
                    { new Guid("11111111-1111-1111-1111-000000000007"), 7, "V", 1, 0 },
                    { new Guid("11111111-1111-1111-1111-000000000008"), 8, "V", 1, 0 },
                    { new Guid("11111111-1111-1111-1111-000000000009"), 9, "V", 1, 0 },
                    { new Guid("11111111-1111-1111-1111-000000000010"), 10, "V", 1, 0 },
                    { new Guid("11111111-1111-1111-1111-000000000011"), 11, "V", 1, 0 },
                    { new Guid("11111111-1111-1111-1111-000000000012"), 12, "V", 1, 0 },
                    { new Guid("11111111-1111-1111-1111-000000000013"), 13, "V", 1, 0 },
                    { new Guid("11111111-1111-1111-1111-000000000014"), 14, "V", 1, 0 },
                    { new Guid("11111111-1111-1111-1111-000000000015"), 15, "V", 1, 0 },
                    { new Guid("11111111-1111-1111-1111-000000000016"), 16, "V", 1, 0 },
                    { new Guid("11111111-1111-1111-1111-000000000017"), 17, "V", 1, 0 },
                    { new Guid("11111111-1111-1111-1111-000000000018"), 18, "V", 1, 0 },
                    { new Guid("11111111-1111-1111-1111-000000000019"), 19, "V", 1, 0 },
                    { new Guid("11111111-1111-1111-1111-000000000020"), 20, "V", 1, 0 },
                    { new Guid("11111111-1111-1111-1111-000000000021"), 21, "V", 1, 0 },
                    { new Guid("11111111-1111-1111-1111-000000000022"), 22, "V", 1, 0 },
                    { new Guid("11111111-1111-1111-1111-000000000023"), 23, "V", 1, 0 },
                    { new Guid("11111111-1111-1111-1111-000000000024"), 24, "V", 1, 0 },
                    { new Guid("11111111-1111-1111-1111-000000000025"), 25, "V", 1, 0 },
                    { new Guid("11111111-1111-1111-1111-000000000026"), 26, "V", 1, 0 },
                    { new Guid("11111111-1111-1111-1111-000000000027"), 27, "V", 1, 0 },
                    { new Guid("11111111-1111-1111-1111-000000000028"), 28, "V", 1, 0 },
                    { new Guid("11111111-1111-1111-1111-000000000029"), 29, "V", 1, 0 },
                    { new Guid("11111111-1111-1111-1111-000000000030"), 30, "V", 1, 0 },
                    { new Guid("11111111-1111-1111-1111-000000000031"), 31, "V", 1, 0 },
                    { new Guid("11111111-1111-1111-1111-000000000032"), 32, "V", 1, 0 },
                    { new Guid("11111111-1111-1111-1111-000000000033"), 33, "V", 1, 0 },
                    { new Guid("11111111-1111-1111-1111-000000000034"), 34, "V", 1, 0 },
                    { new Guid("11111111-1111-1111-1111-000000000035"), 35, "V", 1, 0 },
                    { new Guid("11111111-1111-1111-1111-000000000036"), 36, "V", 1, 0 },
                    { new Guid("11111111-1111-1111-1111-000000000037"), 37, "V", 1, 0 },
                    { new Guid("11111111-1111-1111-1111-000000000038"), 38, "V", 1, 0 },
                    { new Guid("11111111-1111-1111-1111-000000000039"), 39, "V", 1, 0 },
                    { new Guid("11111111-1111-1111-1111-000000000040"), 40, "V", 1, 0 },
                    { new Guid("11111111-1111-1111-1111-000000000041"), 41, "V", 1, 0 },
                    { new Guid("11111111-1111-1111-1111-000000000042"), 42, "V", 1, 0 },
                    { new Guid("11111111-1111-1111-1111-000000000043"), 43, "V", 1, 0 },
                    { new Guid("11111111-1111-1111-1111-000000000044"), 44, "V", 1, 0 },
                    { new Guid("11111111-1111-1111-1111-000000000045"), 45, "V", 1, 0 },
                    { new Guid("11111111-1111-1111-1111-000000000046"), 46, "V", 1, 0 },
                    { new Guid("11111111-1111-1111-1111-000000000047"), 47, "V", 1, 0 },
                    { new Guid("11111111-1111-1111-1111-000000000048"), 48, "V", 1, 0 },
                    { new Guid("11111111-1111-1111-1111-000000000049"), 49, "V", 1, 0 },
                    { new Guid("11111111-1111-1111-1111-000000000050"), 50, "V", 1, 0 },
                    { new Guid("22222222-2222-2222-2222-000000000001"), 1, "G", 2, 0 },
                    { new Guid("22222222-2222-2222-2222-000000000002"), 2, "G", 2, 0 },
                    { new Guid("22222222-2222-2222-2222-000000000003"), 3, "G", 2, 0 },
                    { new Guid("22222222-2222-2222-2222-000000000004"), 4, "G", 2, 0 },
                    { new Guid("22222222-2222-2222-2222-000000000005"), 5, "G", 2, 0 },
                    { new Guid("22222222-2222-2222-2222-000000000006"), 6, "G", 2, 0 },
                    { new Guid("22222222-2222-2222-2222-000000000007"), 7, "G", 2, 0 },
                    { new Guid("22222222-2222-2222-2222-000000000008"), 8, "G", 2, 0 },
                    { new Guid("22222222-2222-2222-2222-000000000009"), 9, "G", 2, 0 },
                    { new Guid("22222222-2222-2222-2222-000000000010"), 10, "G", 2, 0 },
                    { new Guid("22222222-2222-2222-2222-000000000011"), 11, "G", 2, 0 },
                    { new Guid("22222222-2222-2222-2222-000000000012"), 12, "G", 2, 0 },
                    { new Guid("22222222-2222-2222-2222-000000000013"), 13, "G", 2, 0 },
                    { new Guid("22222222-2222-2222-2222-000000000014"), 14, "G", 2, 0 },
                    { new Guid("22222222-2222-2222-2222-000000000015"), 15, "G", 2, 0 },
                    { new Guid("22222222-2222-2222-2222-000000000016"), 16, "G", 2, 0 },
                    { new Guid("22222222-2222-2222-2222-000000000017"), 17, "G", 2, 0 },
                    { new Guid("22222222-2222-2222-2222-000000000018"), 18, "G", 2, 0 },
                    { new Guid("22222222-2222-2222-2222-000000000019"), 19, "G", 2, 0 },
                    { new Guid("22222222-2222-2222-2222-000000000020"), 20, "G", 2, 0 },
                    { new Guid("22222222-2222-2222-2222-000000000021"), 21, "G", 2, 0 },
                    { new Guid("22222222-2222-2222-2222-000000000022"), 22, "G", 2, 0 },
                    { new Guid("22222222-2222-2222-2222-000000000023"), 23, "G", 2, 0 },
                    { new Guid("22222222-2222-2222-2222-000000000024"), 24, "G", 2, 0 },
                    { new Guid("22222222-2222-2222-2222-000000000025"), 25, "G", 2, 0 },
                    { new Guid("22222222-2222-2222-2222-000000000026"), 26, "G", 2, 0 },
                    { new Guid("22222222-2222-2222-2222-000000000027"), 27, "G", 2, 0 },
                    { new Guid("22222222-2222-2222-2222-000000000028"), 28, "G", 2, 0 },
                    { new Guid("22222222-2222-2222-2222-000000000029"), 29, "G", 2, 0 },
                    { new Guid("22222222-2222-2222-2222-000000000030"), 30, "G", 2, 0 },
                    { new Guid("22222222-2222-2222-2222-000000000031"), 31, "G", 2, 0 },
                    { new Guid("22222222-2222-2222-2222-000000000032"), 32, "G", 2, 0 },
                    { new Guid("22222222-2222-2222-2222-000000000033"), 33, "G", 2, 0 },
                    { new Guid("22222222-2222-2222-2222-000000000034"), 34, "G", 2, 0 },
                    { new Guid("22222222-2222-2222-2222-000000000035"), 35, "G", 2, 0 },
                    { new Guid("22222222-2222-2222-2222-000000000036"), 36, "G", 2, 0 },
                    { new Guid("22222222-2222-2222-2222-000000000037"), 37, "G", 2, 0 },
                    { new Guid("22222222-2222-2222-2222-000000000038"), 38, "G", 2, 0 },
                    { new Guid("22222222-2222-2222-2222-000000000039"), 39, "G", 2, 0 },
                    { new Guid("22222222-2222-2222-2222-000000000040"), 40, "G", 2, 0 },
                    { new Guid("22222222-2222-2222-2222-000000000041"), 41, "G", 2, 0 },
                    { new Guid("22222222-2222-2222-2222-000000000042"), 42, "G", 2, 0 },
                    { new Guid("22222222-2222-2222-2222-000000000043"), 43, "G", 2, 0 },
                    { new Guid("22222222-2222-2222-2222-000000000044"), 44, "G", 2, 0 },
                    { new Guid("22222222-2222-2222-2222-000000000045"), 45, "G", 2, 0 },
                    { new Guid("22222222-2222-2222-2222-000000000046"), 46, "G", 2, 0 },
                    { new Guid("22222222-2222-2222-2222-000000000047"), 47, "G", 2, 0 },
                    { new Guid("22222222-2222-2222-2222-000000000048"), 48, "G", 2, 0 },
                    { new Guid("22222222-2222-2222-2222-000000000049"), 49, "G", 2, 0 },
                    { new Guid("22222222-2222-2222-2222-000000000050"), 50, "G", 2, 0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-000000000001"));

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-000000000002"));

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-000000000003"));

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-000000000004"));

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-000000000005"));

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-000000000006"));

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-000000000007"));

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-000000000008"));

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-000000000009"));

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-000000000010"));

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-000000000011"));

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-000000000012"));

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-000000000013"));

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-000000000014"));

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-000000000015"));

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-000000000016"));

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-000000000017"));

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-000000000018"));

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-000000000019"));

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-000000000020"));

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-000000000021"));

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-000000000022"));

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-000000000023"));

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-000000000024"));

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-000000000025"));

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-000000000026"));

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-000000000027"));

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-000000000028"));

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-000000000029"));

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-000000000030"));

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-000000000031"));

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-000000000032"));

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-000000000033"));

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-000000000034"));

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-000000000035"));

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-000000000036"));

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-000000000037"));

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-000000000038"));

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-000000000039"));

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-000000000040"));

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-000000000041"));

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-000000000042"));

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-000000000043"));

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-000000000044"));

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-000000000045"));

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-000000000046"));

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-000000000047"));

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-000000000048"));

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-000000000049"));

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-000000000050"));

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-000000000001"));

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-000000000002"));

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-000000000003"));

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-000000000004"));

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-000000000005"));

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-000000000006"));

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-000000000007"));

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-000000000008"));

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-000000000009"));

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-000000000010"));

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-000000000011"));

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-000000000012"));

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-000000000013"));

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-000000000014"));

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-000000000015"));

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-000000000016"));

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-000000000017"));

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-000000000018"));

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-000000000019"));

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-000000000020"));

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-000000000021"));

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-000000000022"));

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-000000000023"));

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-000000000024"));

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-000000000025"));

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-000000000026"));

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-000000000027"));

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-000000000028"));

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-000000000029"));

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-000000000030"));

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-000000000031"));

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-000000000032"));

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-000000000033"));

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-000000000034"));

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-000000000035"));

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-000000000036"));

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-000000000037"));

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-000000000038"));

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-000000000039"));

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-000000000040"));

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-000000000041"));

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-000000000042"));

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-000000000043"));

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-000000000044"));

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-000000000045"));

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-000000000046"));

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-000000000047"));

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-000000000048"));

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-000000000049"));

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-000000000050"));

            migrationBuilder.DeleteData(
                table: "Sectors",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Sectors",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.AddColumn<string>(
                name: "DocumentNumber",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
