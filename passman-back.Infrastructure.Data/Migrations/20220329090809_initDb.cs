using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace passman_back.Infrastructure.Data.Migrations
{
    public partial class initDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "directories",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: true),
                    parent_id = table.Column<long>(type: "bigint", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_directories", x => x.id);
                    table.ForeignKey(
                        name: "fk_directories_directories_parent_id",
                        column: x => x.parent_id,
                        principalTable: "directories",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "passcards",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    url = table.Column<string>(type: "text", nullable: true),
                    login = table.Column<string>(type: "text", nullable: true),
                    password = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_passcards", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "directory_passcards_relations",
                columns: table => new
                {
                    parents_id = table.Column<long>(type: "bigint", nullable: false),
                    passcards_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_directory_passcards_relations", x => new { x.parents_id, x.passcards_id });
                    table.ForeignKey(
                        name: "fk_directory_passcards_relations_directories_parents_id",
                        column: x => x.parents_id,
                        principalTable: "directories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_directory_passcards_relations_passcards_passcards_id",
                        column: x => x.passcards_id,
                        principalTable: "passcards",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_directories_parent_id",
                table: "directories",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "ix_directory_passcards_relations_passcards_id",
                table: "directory_passcards_relations",
                column: "passcards_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "directory_passcards_relations");

            migrationBuilder.DropTable(
                name: "directories");

            migrationBuilder.DropTable(
                name: "passcards");
        }
    }
}
