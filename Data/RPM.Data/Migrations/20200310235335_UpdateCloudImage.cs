using Microsoft.EntityFrameworkCore.Migrations;

namespace RPM.Data.Migrations
{
    public partial class UpdateCloudImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CloudImages_AspNetUsers_UserId",
                table: "CloudImages");

            migrationBuilder.DropIndex(
                name: "IX_CloudImages_UserId",
                table: "CloudImages");

            migrationBuilder.DropColumn(
                name: "Length",
                table: "CloudImages");

            migrationBuilder.DropColumn(
                name: "PictureThumbnailUrl",
                table: "CloudImages");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "CloudImages");

            migrationBuilder.AddColumn<string>(
                name: "HomeId",
                table: "CloudImages",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CloudImages_HomeId",
                table: "CloudImages",
                column: "HomeId");

            migrationBuilder.AddForeignKey(
                name: "FK_CloudImages_Homes_HomeId",
                table: "CloudImages",
                column: "HomeId",
                principalTable: "Homes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CloudImages_Homes_HomeId",
                table: "CloudImages");

            migrationBuilder.DropIndex(
                name: "IX_CloudImages_HomeId",
                table: "CloudImages");

            migrationBuilder.DropColumn(
                name: "HomeId",
                table: "CloudImages");

            migrationBuilder.AddColumn<long>(
                name: "Length",
                table: "CloudImages",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "PictureThumbnailUrl",
                table: "CloudImages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "CloudImages",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CloudImages_UserId",
                table: "CloudImages",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CloudImages_AspNetUsers_UserId",
                table: "CloudImages",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
