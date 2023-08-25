using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DemoPractical.Migrations
{
    /// <inheritdoc />
    public partial class v4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MoviesCasts",
                columns: table => new
                {
                    MovieName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Actor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Actress = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MoviesCasts", x => x.MovieName);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MoviesCasts");
        }
    }
}
