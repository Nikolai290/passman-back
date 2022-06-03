using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace passman_back.Infrastructure.Data.Migrations
{
    public partial class added_usergroups : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_blocked",
                table: "users",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "user_groups",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_groups", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user_group_directory_relations",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_group_id = table.Column<long>(type: "bigint", nullable: true),
                    directory_id = table.Column<long>(type: "bigint", nullable: true),
                    permission = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_group_directory_relations", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_group_directory_relations_directories_directory_id",
                        column: x => x.directory_id,
                        principalTable: "directories",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_user_group_directory_relations_user_groups_user_group_id",
                        column: x => x.user_group_id,
                        principalTable: "user_groups",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "user_user_group",
                columns: table => new
                {
                    user_groups_id = table.Column<long>(type: "bigint", nullable: false),
                    users_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_user_group", x => new { x.user_groups_id, x.users_id });
                    table.ForeignKey(
                        name: "fk_user_user_group_user_groups_user_groups_id",
                        column: x => x.user_groups_id,
                        principalTable: "user_groups",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_user_group_users_users_id",
                        column: x => x.users_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_user_group_directory_relations_directory_id",
                table: "user_group_directory_relations",
                column: "directory_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_group_directory_relations_user_group_id",
                table: "user_group_directory_relations",
                column: "user_group_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_user_group_users_id",
                table: "user_user_group",
                column: "users_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "user_group_directory_relations");

            migrationBuilder.DropTable(
                name: "user_user_group");

            migrationBuilder.DropTable(
                name: "user_groups");

            migrationBuilder.DropColumn(
                name: "is_blocked",
                table: "users");
        }
    }
}
