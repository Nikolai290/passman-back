using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace passman_back.Infrastructure.Data.Migrations
{
    public partial class favorite_passcards : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "passcard_user",
                columns: table => new
                {
                    favorite_passcards_id = table.Column<long>(type: "bigint", nullable: false),
                    users_favorite_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_passcard_user", x => new { x.favorite_passcards_id, x.users_favorite_id });
                    table.ForeignKey(
                        name: "fk_passcard_user_passcards_favorite_passcards_id",
                        column: x => x.favorite_passcards_id,
                        principalTable: "passcards",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_passcard_user_users_users_favorite_id",
                        column: x => x.users_favorite_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_passcard_user_users_favorite_id",
                table: "passcard_user",
                column: "users_favorite_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "passcard_user");
        }
    }
}
