using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class FileIdAndFetch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Url",
                table: "Videos",
                newName: "FileId");

            migrationBuilder.AddColumn<string>(
                name: "FetchUrl",
                table: "Videos",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FetchUrl",
                table: "Videos");

            migrationBuilder.RenameColumn(
                name: "FileId",
                table: "Videos",
                newName: "Url");
        }
    }
}
