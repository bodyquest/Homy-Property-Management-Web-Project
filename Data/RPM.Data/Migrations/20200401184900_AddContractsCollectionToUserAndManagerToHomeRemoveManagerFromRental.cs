using Microsoft.EntityFrameworkCore.Migrations;

namespace RPM.Data.Migrations
{
    public partial class AddContractsCollectionToUserAndManagerToHomeRemoveManagerFromRental : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rentals_AspNetUsers_ManagerId",
                table: "Rentals");

            migrationBuilder.DropIndex(
                name: "IX_Rentals_ManagerId",
                table: "Rentals");

            migrationBuilder.DropIndex(
                name: "IX_Contracts_RentalId",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "ManagerId",
                table: "Rentals");

            migrationBuilder.AddColumn<string>(
                name: "RentalId",
                table: "Rentals",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ManagerId",
                table: "Homes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ManagerId",
                table: "Contracts",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Homes_ManagerId",
                table: "Homes",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_ManagerId",
                table: "Contracts",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_RentalId",
                table: "Contracts",
                column: "RentalId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_AspNetUsers_ManagerId",
                table: "Contracts",
                column: "ManagerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Homes_AspNetUsers_ManagerId",
                table: "Homes",
                column: "ManagerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_AspNetUsers_ManagerId",
                table: "Contracts");

            migrationBuilder.DropForeignKey(
                name: "FK_Homes_AspNetUsers_ManagerId",
                table: "Homes");

            migrationBuilder.DropIndex(
                name: "IX_Homes_ManagerId",
                table: "Homes");

            migrationBuilder.DropIndex(
                name: "IX_Contracts_ManagerId",
                table: "Contracts");

            migrationBuilder.DropIndex(
                name: "IX_Contracts_RentalId",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "RentalId",
                table: "Rentals");

            migrationBuilder.DropColumn(
                name: "ManagerId",
                table: "Homes");

            migrationBuilder.DropColumn(
                name: "ManagerId",
                table: "Contracts");

            migrationBuilder.AddColumn<string>(
                name: "ManagerId",
                table: "Rentals",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rentals_ManagerId",
                table: "Rentals",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_RentalId",
                table: "Contracts",
                column: "RentalId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rentals_AspNetUsers_ManagerId",
                table: "Rentals",
                column: "ManagerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
