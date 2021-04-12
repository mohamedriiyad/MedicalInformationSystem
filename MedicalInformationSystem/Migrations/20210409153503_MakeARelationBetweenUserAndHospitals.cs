using Microsoft.EntityFrameworkCore.Migrations;

namespace MedicalInformationSystem.Migrations
{
    public partial class MakeARelationBetweenUserAndHospitals : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Hospitals",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Hospitals_ApplicationUserId",
                table: "Hospitals",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Hospitals_AspNetUsers_ApplicationUserId",
                table: "Hospitals",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Hospitals_AspNetUsers_ApplicationUserId",
                table: "Hospitals");

            migrationBuilder.DropIndex(
                name: "IX_Hospitals_ApplicationUserId",
                table: "Hospitals");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Hospitals");
        }
    }
}
