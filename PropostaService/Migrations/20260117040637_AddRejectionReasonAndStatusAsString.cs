using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PropostaService.Migrations
{
    /// <inheritdoc />
    public partial class AddRejectionReasonAndStatusAsString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "status",
                table: "proposal",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<string>(
                name: "rejection_reason",
                table: "proposal",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "rejection_reason",
                table: "proposal");

            migrationBuilder.AlterColumn<int>(
                name: "status",
                table: "proposal",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
