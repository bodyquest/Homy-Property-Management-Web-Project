namespace RPM.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddTransactionRequestsAndPaymentsCollectionsToHome : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HomeId",
                table: "TransactionRequests",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TransactionRequests_HomeId",
                table: "TransactionRequests",
                column: "HomeId");

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionRequests_Homes_HomeId",
                table: "TransactionRequests",
                column: "HomeId",
                principalTable: "Homes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransactionRequests_Homes_HomeId",
                table: "TransactionRequests");

            migrationBuilder.DropIndex(
                name: "IX_TransactionRequests_HomeId",
                table: "TransactionRequests");

            migrationBuilder.DropColumn(
                name: "HomeId",
                table: "TransactionRequests");
        }
    }
}
