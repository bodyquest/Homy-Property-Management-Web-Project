using Microsoft.EntityFrameworkCore.Migrations;

namespace RPM.Data.Migrations
{
    public partial class CreateStripeCheckoutSessionEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StripeCheckoutSessions",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    PaymentId = table.Column<string>(nullable: true),
                    ToStripeAccountId = table.Column<string>(nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StripeCheckoutSessions", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StripeCheckoutSessions");
        }
    }
}
