namespace RPM.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddOwnerToHome : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Homes_Countries_CountryId",
                table: "Homes");

            migrationBuilder.DropForeignKey(
                name: "FK_Homes_AspNetUsers_ManagerId",
                table: "Homes");

            migrationBuilder.DropIndex(
                name: "IX_Homes_CountryId",
                table: "Homes");

            migrationBuilder.DropIndex(
                name: "IX_Homes_ManagerId",
                table: "Homes");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "Homes");

            migrationBuilder.DropColumn(
                name: "ManagerId",
                table: "Homes");

            migrationBuilder.AlterColumn<int>(
                name: "CityId",
                table: "Homes",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "Homes",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Homes_OwnerId",
                table: "Homes",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Homes_AspNetUsers_OwnerId",
                table: "Homes",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Homes_AspNetUsers_OwnerId",
                table: "Homes");

            migrationBuilder.DropIndex(
                name: "IX_Homes_OwnerId",
                table: "Homes");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Homes");

            migrationBuilder.AlterColumn<int>(
                name: "CityId",
                table: "Homes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "CountryId",
                table: "Homes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ManagerId",
                table: "Homes",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Homes_CountryId",
                table: "Homes",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Homes_ManagerId",
                table: "Homes",
                column: "ManagerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Homes_Countries_CountryId",
                table: "Homes",
                column: "CountryId",
                principalTable: "Countries",
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
    }
}
