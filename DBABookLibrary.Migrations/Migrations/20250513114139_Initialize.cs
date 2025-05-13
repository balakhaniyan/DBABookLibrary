using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DBABookLibrary.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class Initialize : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Authors",
                columns: table => new
                {
                    AuthorId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AuthorGuid = table.Column<Guid>(type: "uuid", nullable: false),
                    Name_FirstName = table.Column<string>(type: "character varying(248)", maxLength: 248, nullable: false),
                    Name_LastName = table.Column<string>(type: "character varying(248)", maxLength: 248, nullable: false),
                    Name_NickName = table.Column<string>(type: "character varying(248)", maxLength: 248, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.AuthorId);
                });

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    BookId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BookGuid = table.Column<Guid>(type: "uuid", nullable: false),
                    AuthorId = table.Column<int>(type: "integer", nullable: false),
                    Genre = table.Column<int>(type: "integer", nullable: false),
                    LifeCycle = table.Column<int>(type: "integer", nullable: false),
                    Version = table.Column<int>(type: "integer", nullable: false),
                    PublishedBookYear_BookYearId = table.Column<int>(type: "integer", nullable: false),
                    PublishedBookYear_Year = table.Column<int>(type: "integer", nullable: false),
                    Title_BookTitleId = table.Column<int>(type: "integer", nullable: false),
                    Title_Title = table.Column<string>(type: "character varying(248)", maxLength: 248, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.BookId);
                    table.ForeignKey(
                        name: "FK_Books_Authors_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Authors",
                        principalColumn: "AuthorId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Books_AuthorId",
                table: "Books",
                column: "AuthorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "Authors");
        }
    }
}
