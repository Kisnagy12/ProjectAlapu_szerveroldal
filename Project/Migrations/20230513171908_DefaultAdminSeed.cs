using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project.Migrations
{
    /// <inheritdoc />
    public partial class DefaultAdminSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ApplicationUser",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "RefreshToken", "RefreshTokenExpiryTime", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "02174cf0–9412–4cfe-afbf-59f706d72cf6", 0, "05032bc6-0a8a-4867-ae10-9a67eeb50d1b", "", false, false, null, null, "ADMIN", "AQAAAAEAACcQAAAAEPsmRgUEow/LIap64iPMu5KLfe8h7XRyY9lJCqIxmCPKBW2GoyZMBfe68k2HTGyadA==", null, false, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "465c2331-d5ae-4b0f-a131-e20f05349243", false, "admin" });

            migrationBuilder.InsertData(
                table: "IdentityUserRole",
                columns: new[] { "Id", "RoleId", "UserId" },
                values: new object[] { "b268a4bb-79b1-467a-957c-1fc2401e087e", "8ff2a9f2-5738-4097-80a3-6e364161263d", "02174cf0–9412–4cfe-afbf-59f706d72cf6" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ApplicationUser",
                keyColumn: "Id",
                keyValue: "02174cf0–9412–4cfe-afbf-59f706d72cf6");

            migrationBuilder.DeleteData(
                table: "IdentityUserRole",
                keyColumn: "Id",
                keyValue: "b268a4bb-79b1-467a-957c-1fc2401e087e");
        }
    }
}
