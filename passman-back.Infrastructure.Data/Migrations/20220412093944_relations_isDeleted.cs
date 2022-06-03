using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace passman_back.Infrastructure.Data.Migrations
{
    public partial class relations_isDeleted : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_deleted",
                table: "user_group_directory_relations",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_deleted",
                table: "user_group_directory_relations");
        }
    }
}
