using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RPM.Data.Migrations
{
    public partial class AddTransactionRequestEntityAndCollectionOfItInRentalsEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TransactionRequests",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    RecipientId = table.Column<string>(nullable: false),
                    SenderId = table.Column<string>(nullable: false),
                    Reason = table.Column<string>(maxLength: 20, nullable: false),
                    Amount = table.Column<decimal>(nullable: false),
                    RentalId = table.Column<int>(nullable: false),
                    IsRecurring = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransactionRequests_AspNetUsers_RecipientId",
                        column: x => x.RecipientId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransactionRequests_Rentals_RentalId",
                        column: x => x.RentalId,
                        principalTable: "Rentals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransactionRequests_AspNetUsers_SenderId",
                        column: x => x.SenderId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TransactionRequests_RecipientId",
                table: "TransactionRequests",
                column: "RecipientId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionRequests_RentalId",
                table: "TransactionRequests",
                column: "RentalId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionRequests_SenderId",
                table: "TransactionRequests",
                column: "SenderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TransactionRequests");
        }
    }
}
