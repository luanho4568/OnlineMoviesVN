using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineMoviesVN.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CountryId",
                table: "Movies",
                newName: "Country");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "Movies",
                newName: "Category");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Country",
                table: "Movies",
                newName: "CountryId");

            migrationBuilder.RenameColumn(
                name: "Category",
                table: "Movies",
                newName: "CategoryId");
        }
    }
}
