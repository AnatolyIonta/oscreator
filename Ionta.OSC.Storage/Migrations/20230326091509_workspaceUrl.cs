using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ionta.OSC.Storage.Migrations
{
    /// <inheritdoc />
    public partial class workspaceUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_CustomPages_Url",
                table: "CustomPages",
                column: "Url",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CustomPages_Url",
                table: "CustomPages");
        }
    }
}
