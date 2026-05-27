using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Interview_Test.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdUniqueIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Users_UserId",
                table: "Users",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_UserId",
                table: "Users");
        }
    }
}
