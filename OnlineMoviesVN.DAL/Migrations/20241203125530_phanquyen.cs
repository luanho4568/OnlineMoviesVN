using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineMoviesVN.DAL.Migrations
{
    /// <inheritdoc />
    public partial class phanquyen : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UsersPermissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GroupName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    Hidden = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersPermissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UsersPermissionsGranteds",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserRole = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoleLevel = table.Column<int>(type: "int", nullable: true),
                    PermissionCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PermissionName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersPermissionsGranteds", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UsersPermissions");

            migrationBuilder.DropTable(
                name: "UsersPermissionsGranteds");
        }
    }
}
