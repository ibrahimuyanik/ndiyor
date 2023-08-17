using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    public partial class mig_test_cloud_add : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Slug",
                table: "Newses");

            migrationBuilder.AddColumn<string>(
                name: "SavedFileName",
                table: "Tests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SavedUrl",
                table: "Tests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NewsID",
                table: "Images",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Images_NewsID",
                table: "Images",
                column: "NewsID");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Newses_NewsID",
                table: "Images",
                column: "NewsID",
                principalTable: "Newses",
                principalColumn: "NewsID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Newses_NewsID",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Images_NewsID",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "SavedFileName",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "SavedUrl",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "NewsID",
                table: "Images");

            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "Newses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
