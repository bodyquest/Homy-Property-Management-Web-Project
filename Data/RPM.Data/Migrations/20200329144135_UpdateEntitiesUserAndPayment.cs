using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RPM.Data.Migrations
{
    public partial class UpdateEntitiesUserAndPayment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "TransactionDate",
                table: "Payments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StripeConnectedAccountId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StripePublishableKey",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StripeRefreshToken",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransactionDate",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "StripeConnectedAccountId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "StripePublishableKey",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "StripeRefreshToken",
                table: "AspNetUsers");
        }
    }
}
