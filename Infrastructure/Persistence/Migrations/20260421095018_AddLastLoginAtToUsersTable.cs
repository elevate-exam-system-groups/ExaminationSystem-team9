using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExaminationSystem.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddLastLoginAtToUsersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastLoginAt",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("84e9be3e-d03d-457c-aa82-32cd9a6cc17f"),
                columns: new[] { "CreatedAt", "LastLoginAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 4, 21, 9, 50, 17, 987, DateTimeKind.Utc).AddTicks(9889), null, "AQAAAAIAAYagAAAAEOdwkvr7LaaYaMxQJ9r968d4p8dEgscwkSIzX/sGmsPZXoEaqa9ECmhn/uxEXhJYfw==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastLoginAt",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("84e9be3e-d03d-457c-aa82-32cd9a6cc17f"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 4, 21, 9, 30, 23, 415, DateTimeKind.Utc).AddTicks(5673), "AQAAAAIAAYagAAAAEGSJThz9A/3hf66JvWt2cQY72n/86/Z94zUqjD0y0exnyuOEIQuLVgczDtZSZm3qxw==" });
        }
    }
}
