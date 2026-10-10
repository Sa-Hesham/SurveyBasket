using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SurveyBasket.Api.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addseedIdentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "IsDefault", "IsDeleted", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "C834FFA7-20F0-47D8-9162-5728CA19C6D8", "7701DF37-F279-49AF-B998-9EDDA403D99E", false, false, "Admin", "ADMIN" },
                    { "D731FE31-D180-453E-B8E6-54DB43A5AD6D", "339C3D36-6AB9-4CBA-B360-AC56F2838875", true, false, "Member", "MEMBER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "32257867-308A-42EA-96F2-1100514D3FFD", 0, "A237BB74-ECF5-4043-9E72-87D52DA8EA97", "admin@myApp.com", true, "Admin", "SurveyBasket", false, null, "ADMIN@MYAPP.COM", "ADMIN@MYAPP.COM", "AQAAAAIAAYagAAAAEPDsVsJ5aKcMdWddzu7upKf9NxKRzvYAqnjCOR6QQhMAWBdcCPnL9PXSV4W8ddtGWQ==", null, false, "36D01D3C84544DE3B0A1BDF67E33E5D3", false, "admin@myApp.com" });

            migrationBuilder.InsertData(
                table: "AspNetRoleClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "RoleId" },
                values: new object[,]
                {
                    { 1, "permission", "polls:read", "C834FFA7-20F0-47D8-9162-5728CA19C6D8" },
                    { 2, "permission", "polls:add", "C834FFA7-20F0-47D8-9162-5728CA19C6D8" },
                    { 3, "permission", "polls:update", "C834FFA7-20F0-47D8-9162-5728CA19C6D8" },
                    { 4, "permission", "polls:delete", "C834FFA7-20F0-47D8-9162-5728CA19C6D8" },
                    { 5, "permission", "questions:read", "C834FFA7-20F0-47D8-9162-5728CA19C6D8" },
                    { 6, "permission", "questions:add", "C834FFA7-20F0-47D8-9162-5728CA19C6D8" },
                    { 7, "permission", "questions:update", "C834FFA7-20F0-47D8-9162-5728CA19C6D8" },
                    { 8, "permission", "users:read", "C834FFA7-20F0-47D8-9162-5728CA19C6D8" },
                    { 9, "permission", "users:add", "C834FFA7-20F0-47D8-9162-5728CA19C6D8" },
                    { 10, "permission", "users:update", "C834FFA7-20F0-47D8-9162-5728CA19C6D8" },
                    { 11, "permission", "roles:read", "C834FFA7-20F0-47D8-9162-5728CA19C6D8" },
                    { 12, "permission", "roles:add", "C834FFA7-20F0-47D8-9162-5728CA19C6D8" },
                    { 13, "permission", "roles:update", "C834FFA7-20F0-47D8-9162-5728CA19C6D8" },
                    { 14, "permission", "results:read", "C834FFA7-20F0-47D8-9162-5728CA19C6D8" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "C834FFA7-20F0-47D8-9162-5728CA19C6D8", "32257867-308A-42EA-96F2-1100514D3FFD" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "D731FE31-D180-453E-B8E6-54DB43A5AD6D");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "C834FFA7-20F0-47D8-9162-5728CA19C6D8", "32257867-308A-42EA-96F2-1100514D3FFD" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "C834FFA7-20F0-47D8-9162-5728CA19C6D8");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "32257867-308A-42EA-96F2-1100514D3FFD");
        }
    }
}
