using Microsoft.EntityFrameworkCore.Migrations;

namespace MedicalInformationSystem.Migrations
{
    public partial class DeleteBloodTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Blood_BloodId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Blood");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_BloodId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "BloodId",
                table: "AspNetUsers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BloodId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Blood",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blood", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_BloodId",
                table: "AspNetUsers",
                column: "BloodId",
                unique: true,
                filter: "[BloodId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Blood_BloodId",
                table: "AspNetUsers",
                column: "BloodId",
                principalTable: "Blood",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
