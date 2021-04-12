using Microsoft.EntityFrameworkCore.Migrations;

namespace MedicalInformationSystem.Migrations
{
    public partial class EnhanceTheRelationBetweenHospitalFilesAndHospitals : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HospitalFiles_Hospitals_HospitalId",
                table: "HospitalFiles");

            migrationBuilder.DropIndex(
                name: "IX_HospitalFiles_HospitalId",
                table: "HospitalFiles");

            migrationBuilder.AddColumn<int>(
                name: "HospitalModelId",
                table: "HospitalFiles",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_HospitalFiles_HospitalModelId",
                table: "HospitalFiles",
                column: "HospitalModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_HospitalFiles_Hospitals_HospitalModelId",
                table: "HospitalFiles",
                column: "HospitalModelId",
                principalTable: "Hospitals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HospitalFiles_Hospitals_HospitalModelId",
                table: "HospitalFiles");

            migrationBuilder.DropIndex(
                name: "IX_HospitalFiles_HospitalModelId",
                table: "HospitalFiles");

            migrationBuilder.DropColumn(
                name: "HospitalModelId",
                table: "HospitalFiles");

            migrationBuilder.CreateIndex(
                name: "IX_HospitalFiles_HospitalId",
                table: "HospitalFiles",
                column: "HospitalModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_HospitalFiles_Hospitals_HospitalId",
                table: "HospitalFiles",
                column: "HospitalModelId",
                principalTable: "Hospitals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
