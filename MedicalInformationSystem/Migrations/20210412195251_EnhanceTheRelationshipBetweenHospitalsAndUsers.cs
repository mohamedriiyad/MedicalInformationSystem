using Microsoft.EntityFrameworkCore.Migrations;

namespace MedicalInformationSystem.Migrations
{
    public partial class EnhanceTheRelationshipBetweenHospitalsAndUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Hospitals_ApplicationUserId",
                table: "Hospitals");

            migrationBuilder.CreateIndex(
                name: "IX_Hospitals_ApplicationUserId",
                table: "Hospitals",
                column: "ApplicationUserId",
                unique: true,
                filter: "[ApplicationUserId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Hospitals_ApplicationUserId",
                table: "Hospitals");

            migrationBuilder.CreateIndex(
                name: "IX_Hospitals_ApplicationUserId",
                table: "Hospitals",
                column: "ApplicationUserId");
        }
    }
}
