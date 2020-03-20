namespace RPM.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class RemoveRentalRenferenceFromHome : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Homes_Rentals_RentalId",
                table: "Homes");

            migrationBuilder.DropIndex(
                name: "IX_Homes_RentalId",
                table: "Homes");

            migrationBuilder.DropColumn(
                name: "RentalId",
                table: "Homes");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RentalId",
                table: "Homes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Homes_RentalId",
                table: "Homes",
                column: "RentalId");

            migrationBuilder.AddForeignKey(
                name: "FK_Homes_Rentals_RentalId",
                table: "Homes",
                column: "RentalId",
                principalTable: "Rentals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
