using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderTask.Migrations
{
    /// <inheritdoc />
    public partial class Updat01 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GovernorateId",
                table: "Cities",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cities_GovernorateId",
                table: "Cities",
                column: "GovernorateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cities_governorates_GovernorateId",
                table: "Cities",
                column: "GovernorateId",
                principalTable: "governorates",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cities_governorates_GovernorateId",
                table: "Cities");

            migrationBuilder.DropIndex(
                name: "IX_Cities_GovernorateId",
                table: "Cities");

            migrationBuilder.DropColumn(
                name: "GovernorateId",
                table: "Cities");
        }
    }
}
