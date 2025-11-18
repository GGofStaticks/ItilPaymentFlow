using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ItilPaymentFlow.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Rename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PresaleOffers",
                table: "PresaleOffers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PresaleMeetings",
                table: "PresaleMeetings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PresaleContracts",
                table: "PresaleContracts");

            migrationBuilder.RenameTable(
                name: "PresaleOffers",
                newName: "Offers");

            migrationBuilder.RenameTable(
                name: "PresaleMeetings",
                newName: "Meetings");

            migrationBuilder.RenameTable(
                name: "PresaleContracts",
                newName: "Contracts");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Offers",
                table: "Offers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Meetings",
                table: "Meetings",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Contracts",
                table: "Contracts",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Offers",
                table: "Offers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Meetings",
                table: "Meetings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Contracts",
                table: "Contracts");

            migrationBuilder.RenameTable(
                name: "Offers",
                newName: "PresaleOffers");

            migrationBuilder.RenameTable(
                name: "Meetings",
                newName: "PresaleMeetings");

            migrationBuilder.RenameTable(
                name: "Contracts",
                newName: "PresaleContracts");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PresaleOffers",
                table: "PresaleOffers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PresaleMeetings",
                table: "PresaleMeetings",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PresaleContracts",
                table: "PresaleContracts",
                column: "Id");
        }
    }
}
