using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExaminationSystem.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SeedingAdminData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "AspNetUsers",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "AccountStatus", "ConcurrencyStamp", "CreatedAt", "Email", "EmailConfirmed", "FullName", "IsDeleted", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UpdatedAt", "UserName" },
                values: new object[] { new Guid("84e9be3e-d03d-457c-aa82-32cd9a6cc17f"), 0, 1, "EB8550F122F348CE921A3030029B27B7", new DateTime(2026, 4, 21, 8, 45, 24, 689, DateTimeKind.Utc).AddTicks(15), "admin@examination.com", true, "Admin Exam", false, false, null, "ADMIN@EXAMINATION.COM", "ADMIN@EXAMINATION.COM", "AQAAAAIAAYagAAAAENoozm3PE1tKQfNEvqI7H9HsE5tBzuPLYBGqZySi+nnabLo1VIUP6MH7JjtElif1eQ==", null, false, "EB8550F122F348CE921A3030029B27B7", false, null, "admin@examination.com" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("84e9be3e-d03d-457c-aa82-32cd9a6cc17f"));

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);
        }
    }
}
