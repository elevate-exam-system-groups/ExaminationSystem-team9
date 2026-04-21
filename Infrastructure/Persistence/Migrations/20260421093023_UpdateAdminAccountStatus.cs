using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExaminationSystem.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAdminAccountStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("84e9be3e-d03d-457c-aa82-32cd9a6cc17f"),
                columns: new[] { "AccountStatus", "CreatedAt", "PasswordHash" },
                values: new object[] { 2, new DateTime(2026, 4, 21, 9, 30, 23, 415, DateTimeKind.Utc).AddTicks(5673), "AQAAAAIAAYagAAAAEGSJThz9A/3hf66JvWt2cQY72n/86/Z94zUqjD0y0exnyuOEIQuLVgczDtZSZm3qxw==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("84e9be3e-d03d-457c-aa82-32cd9a6cc17f"),
                columns: new[] { "AccountStatus", "CreatedAt", "PasswordHash" },
                values: new object[] { 1, new DateTime(2026, 4, 21, 8, 45, 24, 689, DateTimeKind.Utc).AddTicks(15), "AQAAAAIAAYagAAAAENoozm3PE1tKQfNEvqI7H9HsE5tBzuPLYBGqZySi+nnabLo1VIUP6MH7JjtElif1eQ==" });
        }
    }
}
