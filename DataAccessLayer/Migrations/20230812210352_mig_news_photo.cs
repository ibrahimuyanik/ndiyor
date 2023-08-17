using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    public partial class mig_news_photo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Newses_NewsID",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Images_NewsID",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "NewsID",
                table: "Images");

            migrationBuilder.AddColumn<string>(
                name: "SavedFileName",
                table: "Newses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SavedUrl",
                table: "Newses",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SavedFileName",
                table: "Newses");

            migrationBuilder.DropColumn(
                name: "SavedUrl",
                table: "Newses");

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
    }
}
