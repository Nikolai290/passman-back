using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace passman_back.Infrastructure.Data.Migrations
{
    public partial class inviteCodes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "invite_codes",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: true),
                    code = table.Column<string>(type: "text", nullable: true),
                    is_stopped = table.Column<bool>(type: "boolean", nullable: false),
                    alive_before = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    role = table.Column<int>(type: "integer", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_invite_codes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "invite_code_user_group",
                columns: table => new
                {
                    invite_codes_id = table.Column<long>(type: "bigint", nullable: false),
                    user_groups_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_invite_code_user_group", x => new { x.invite_codes_id, x.user_groups_id });
                    table.ForeignKey(
                        name: "fk_invite_code_user_group_invite_codes_invite_codes_id",
                        column: x => x.invite_codes_id,
                        principalTable: "invite_codes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_invite_code_user_group_user_groups_user_groups_id",
                        column: x => x.user_groups_id,
                        principalTable: "user_groups",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_invite_code_user_group_user_groups_id",
                table: "invite_code_user_group",
                column: "user_groups_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "invite_code_user_group");

            migrationBuilder.DropTable(
                name: "invite_codes");
        }
    }
}
