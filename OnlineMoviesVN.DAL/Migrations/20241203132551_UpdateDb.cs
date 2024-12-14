using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineMoviesVN.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GroupName",
                table: "UsersPermissions");

            migrationBuilder.DropColumn(
                name: "Hidden",
                table: "UsersPermissions");

            migrationBuilder.DropColumn(
                name: "Order",
                table: "UsersPermissions");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "UsersPermissions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GroupName",
                table: "UsersPermissions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Hidden",
                table: "UsersPermissions",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "UsersPermissions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "UsersPermissions",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
