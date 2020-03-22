using Microsoft.EntityFrameworkCore.Migrations;

namespace RPM.Data.Migrations
{
    public partial class AddManagerToHomeAddNullableDurationToRental : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Duration",
                table: "Rentals",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ManagerId",
                table: "Rentals",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rentals_ManagerId",
                table: "Rentals",
                column: "ManagerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rentals_AspNetUsers_ManagerId",
                table: "Rentals",
                column: "ManagerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rentals_AspNetUsers_ManagerId",
                table: "Rentals");

            migrationBuilder.DropIndex(
                name: "IX_Rentals_ManagerId",
                table: "Rentals");

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Rentals");

            migrationBuilder.DropColumn(
                name: "ManagerId",
                table: "Rentals");
        }
    }
}
