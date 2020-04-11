using Microsoft.EntityFrameworkCore.Migrations;

namespace RPM.Data.Migrations
{
    public partial class AddHomeToPaymentEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HomeId",
                table: "Payments",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_HomeId",
                table: "Payments",
                column: "HomeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Homes_HomeId",
                table: "Payments",
                column: "HomeId",
                principalTable: "Homes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Homes_HomeId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_HomeId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "HomeId",
                table: "Payments");
        }
    }
}
