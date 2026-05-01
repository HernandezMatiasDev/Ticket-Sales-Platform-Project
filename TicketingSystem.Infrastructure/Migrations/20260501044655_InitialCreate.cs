using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TicketingSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AUDIT_LOG",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    Action = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EntityType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EntityId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Details = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AUDIT_LOG", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    EventDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Venue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RESERVATION",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    SeatId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ReservedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RESERVATION", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sectors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    EventId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sectors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sectors_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Seats",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Number = table.Column<int>(type: "int", nullable: false),
                    Row = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    SectorId = table.Column<int>(type: "int", nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Seats_Sectors_SectorId",
                        column: x => x.SectorId,
                        principalTable: "Sectors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                columns: new[] { "Id", "Number", "Row", "SectorId", "Status", "Version" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-000000000001"), 1, "V", 1, 0, 1 },
                    { new Guid("11111111-1111-1111-1111-000000000002"), 2, "V", 1, 0, 1 },
                    { new Guid("11111111-1111-1111-1111-000000000003"), 3, "V", 1, 0, 1 },
                    { new Guid("11111111-1111-1111-1111-000000000004"), 4, "V", 1, 0, 1 },
                    { new Guid("11111111-1111-1111-1111-000000000005"), 5, "V", 1, 0, 1 },
                    { new Guid("11111111-1111-1111-1111-000000000006"), 6, "V", 1, 0, 1 },
                    { new Guid("11111111-1111-1111-1111-000000000007"), 7, "V", 1, 0, 1 },
                    { new Guid("11111111-1111-1111-1111-000000000008"), 8, "V", 1, 0, 1 },
                    { new Guid("11111111-1111-1111-1111-000000000009"), 9, "V", 1, 0, 1 },
                    { new Guid("11111111-1111-1111-1111-000000000010"), 10, "V", 1, 0, 1 },
                    { new Guid("11111111-1111-1111-1111-000000000011"), 11, "V", 1, 0, 1 },
                    { new Guid("11111111-1111-1111-1111-000000000012"), 12, "V", 1, 0, 1 },
                    { new Guid("11111111-1111-1111-1111-000000000013"), 13, "V", 1, 0, 1 },
                    { new Guid("11111111-1111-1111-1111-000000000014"), 14, "V", 1, 0, 1 },
                    { new Guid("11111111-1111-1111-1111-000000000015"), 15, "V", 1, 0, 1 },
                    { new Guid("11111111-1111-1111-1111-000000000016"), 16, "V", 1, 0, 1 },
                    { new Guid("11111111-1111-1111-1111-000000000017"), 17, "V", 1, 0, 1 },
                    { new Guid("11111111-1111-1111-1111-000000000018"), 18, "V", 1, 0, 1 },
                    { new Guid("11111111-1111-1111-1111-000000000019"), 19, "V", 1, 0, 1 },
                    { new Guid("11111111-1111-1111-1111-000000000020"), 20, "V", 1, 0, 1 },
                    { new Guid("11111111-1111-1111-1111-000000000021"), 21, "V", 1, 0, 1 },
                    { new Guid("11111111-1111-1111-1111-000000000022"), 22, "V", 1, 0, 1 },
                    { new Guid("11111111-1111-1111-1111-000000000023"), 23, "V", 1, 0, 1 },
                    { new Guid("11111111-1111-1111-1111-000000000024"), 24, "V", 1, 0, 1 },
                    { new Guid("11111111-1111-1111-1111-000000000025"), 25, "V", 1, 0, 1 },
                    { new Guid("11111111-1111-1111-1111-000000000026"), 26, "V", 1, 0, 1 },
                    { new Guid("11111111-1111-1111-1111-000000000027"), 27, "V", 1, 0, 1 },
                    { new Guid("11111111-1111-1111-1111-000000000028"), 28, "V", 1, 0, 1 },
                    { new Guid("11111111-1111-1111-1111-000000000029"), 29, "V", 1, 0, 1 },
                    { new Guid("11111111-1111-1111-1111-000000000030"), 30, "V", 1, 0, 1 },
                    { new Guid("11111111-1111-1111-1111-000000000031"), 31, "V", 1, 0, 1 },
                    { new Guid("11111111-1111-1111-1111-000000000032"), 32, "V", 1, 0, 1 },
                    { new Guid("11111111-1111-1111-1111-000000000033"), 33, "V", 1, 0, 1 },
                    { new Guid("11111111-1111-1111-1111-000000000034"), 34, "V", 1, 0, 1 },
                    { new Guid("11111111-1111-1111-1111-000000000035"), 35, "V", 1, 0, 1 },
                    { new Guid("11111111-1111-1111-1111-000000000036"), 36, "V", 1, 0, 1 },
                    { new Guid("11111111-1111-1111-1111-000000000037"), 37, "V", 1, 0, 1 },
                    { new Guid("11111111-1111-1111-1111-000000000038"), 38, "V", 1, 0, 1 },
                    { new Guid("11111111-1111-1111-1111-000000000039"), 39, "V", 1, 0, 1 },
                    { new Guid("11111111-1111-1111-1111-000000000040"), 40, "V", 1, 0, 1 },
                    { new Guid("11111111-1111-1111-1111-000000000041"), 41, "V", 1, 0, 1 },
                    { new Guid("11111111-1111-1111-1111-000000000042"), 42, "V", 1, 0, 1 },
                    { new Guid("11111111-1111-1111-1111-000000000043"), 43, "V", 1, 0, 1 },
                    { new Guid("11111111-1111-1111-1111-000000000044"), 44, "V", 1, 0, 1 },
                    { new Guid("11111111-1111-1111-1111-000000000045"), 45, "V", 1, 0, 1 },
                    { new Guid("11111111-1111-1111-1111-000000000046"), 46, "V", 1, 0, 1 },
                    { new Guid("11111111-1111-1111-1111-000000000047"), 47, "V", 1, 0, 1 },
                    { new Guid("11111111-1111-1111-1111-000000000048"), 48, "V", 1, 0, 1 },
                    { new Guid("11111111-1111-1111-1111-000000000049"), 49, "V", 1, 0, 1 },
                    { new Guid("11111111-1111-1111-1111-000000000050"), 50, "V", 1, 0, 1 },
                    { new Guid("22222222-2222-2222-2222-000000000001"), 1, "G", 2, 0, 1 },
                    { new Guid("22222222-2222-2222-2222-000000000002"), 2, "G", 2, 0, 1 },
                    { new Guid("22222222-2222-2222-2222-000000000003"), 3, "G", 2, 0, 1 },
                    { new Guid("22222222-2222-2222-2222-000000000004"), 4, "G", 2, 0, 1 },
                    { new Guid("22222222-2222-2222-2222-000000000005"), 5, "G", 2, 0, 1 },
                    { new Guid("22222222-2222-2222-2222-000000000006"), 6, "G", 2, 0, 1 },
                    { new Guid("22222222-2222-2222-2222-000000000007"), 7, "G", 2, 0, 1 },
                    { new Guid("22222222-2222-2222-2222-000000000008"), 8, "G", 2, 0, 1 },
                    { new Guid("22222222-2222-2222-2222-000000000009"), 9, "G", 2, 0, 1 },
                    { new Guid("22222222-2222-2222-2222-000000000010"), 10, "G", 2, 0, 1 },
                    { new Guid("22222222-2222-2222-2222-000000000011"), 11, "G", 2, 0, 1 },
                    { new Guid("22222222-2222-2222-2222-000000000012"), 12, "G", 2, 0, 1 },
                    { new Guid("22222222-2222-2222-2222-000000000013"), 13, "G", 2, 0, 1 },
                    { new Guid("22222222-2222-2222-2222-000000000014"), 14, "G", 2, 0, 1 },
                    { new Guid("22222222-2222-2222-2222-000000000015"), 15, "G", 2, 0, 1 },
                    { new Guid("22222222-2222-2222-2222-000000000016"), 16, "G", 2, 0, 1 },
                    { new Guid("22222222-2222-2222-2222-000000000017"), 17, "G", 2, 0, 1 },
                    { new Guid("22222222-2222-2222-2222-000000000018"), 18, "G", 2, 0, 1 },
                    { new Guid("22222222-2222-2222-2222-000000000019"), 19, "G", 2, 0, 1 },
                    { new Guid("22222222-2222-2222-2222-000000000020"), 20, "G", 2, 0, 1 },
                    { new Guid("22222222-2222-2222-2222-000000000021"), 21, "G", 2, 0, 1 },
                    { new Guid("22222222-2222-2222-2222-000000000022"), 22, "G", 2, 0, 1 },
                    { new Guid("22222222-2222-2222-2222-000000000023"), 23, "G", 2, 0, 1 },
                    { new Guid("22222222-2222-2222-2222-000000000024"), 24, "G", 2, 0, 1 },
                    { new Guid("22222222-2222-2222-2222-000000000025"), 25, "G", 2, 0, 1 },
                    { new Guid("22222222-2222-2222-2222-000000000026"), 26, "G", 2, 0, 1 },
                    { new Guid("22222222-2222-2222-2222-000000000027"), 27, "G", 2, 0, 1 },
                    { new Guid("22222222-2222-2222-2222-000000000028"), 28, "G", 2, 0, 1 },
                    { new Guid("22222222-2222-2222-2222-000000000029"), 29, "G", 2, 0, 1 },
                    { new Guid("22222222-2222-2222-2222-000000000030"), 30, "G", 2, 0, 1 },
                    { new Guid("22222222-2222-2222-2222-000000000031"), 31, "G", 2, 0, 1 },
                    { new Guid("22222222-2222-2222-2222-000000000032"), 32, "G", 2, 0, 1 },
                    { new Guid("22222222-2222-2222-2222-000000000033"), 33, "G", 2, 0, 1 },
                    { new Guid("22222222-2222-2222-2222-000000000034"), 34, "G", 2, 0, 1 },
                    { new Guid("22222222-2222-2222-2222-000000000035"), 35, "G", 2, 0, 1 },
                    { new Guid("22222222-2222-2222-2222-000000000036"), 36, "G", 2, 0, 1 },
                    { new Guid("22222222-2222-2222-2222-000000000037"), 37, "G", 2, 0, 1 },
                    { new Guid("22222222-2222-2222-2222-000000000038"), 38, "G", 2, 0, 1 },
                    { new Guid("22222222-2222-2222-2222-000000000039"), 39, "G", 2, 0, 1 },
                    { new Guid("22222222-2222-2222-2222-000000000040"), 40, "G", 2, 0, 1 },
                    { new Guid("22222222-2222-2222-2222-000000000041"), 41, "G", 2, 0, 1 },
                    { new Guid("22222222-2222-2222-2222-000000000042"), 42, "G", 2, 0, 1 },
                    { new Guid("22222222-2222-2222-2222-000000000043"), 43, "G", 2, 0, 1 },
                    { new Guid("22222222-2222-2222-2222-000000000044"), 44, "G", 2, 0, 1 },
                    { new Guid("22222222-2222-2222-2222-000000000045"), 45, "G", 2, 0, 1 },
                    { new Guid("22222222-2222-2222-2222-000000000046"), 46, "G", 2, 0, 1 },
                    { new Guid("22222222-2222-2222-2222-000000000047"), 47, "G", 2, 0, 1 },
                    { new Guid("22222222-2222-2222-2222-000000000048"), 48, "G", 2, 0, 1 },
                    { new Guid("22222222-2222-2222-2222-000000000049"), 49, "G", 2, 0, 1 },
                    { new Guid("22222222-2222-2222-2222-000000000050"), 50, "G", 2, 0, 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Seats_SectorId",
                table: "Seats",
                column: "SectorId");

            migrationBuilder.CreateIndex(
                name: "IX_Sectors_EventId",
                table: "Sectors",
                column: "EventId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "AUDIT_LOG");

            migrationBuilder.DropTable(
                name: "RESERVATION");

            migrationBuilder.DropTable(
                name: "Seats");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Sectors");

            migrationBuilder.DropTable(
                name: "Events");
        }
    }
}
