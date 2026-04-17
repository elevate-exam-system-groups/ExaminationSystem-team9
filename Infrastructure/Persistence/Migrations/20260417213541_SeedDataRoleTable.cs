using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ExaminationSystem.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SeedDataRoleTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "IsDefault", "IsDeleted", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("b19e6450-8a87-4c06-bdec-e398d286dae4"), "41B9439FEFF04EE08BA56A4E7952138F", true, false, "Student", "STUDENT" },
                    { new Guid("c9347073-4adb-4301-b52b-40a89bffbea8"), "2B0CE196CB8646D4991E2A068C8BA717", false, false, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("b19e6450-8a87-4c06-bdec-e398d286dae4"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("c9347073-4adb-4301-b52b-40a89bffbea8"));
        }
    }
}
