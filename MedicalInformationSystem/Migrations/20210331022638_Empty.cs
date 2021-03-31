using Microsoft.EntityFrameworkCore.Migrations;

namespace MedicalInformationSystem.Migrations
{
    public partial class Empty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sensitivity_MedicalHistory_MedicalHistoryId",
                table: "Sensitivity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Sensitivity",
                table: "Sensitivity");

            migrationBuilder.RenameTable(
                name: "Sensitivity",
                newName: "Sensitivities");

            migrationBuilder.RenameIndex(
                name: "IX_Sensitivity_MedicalHistoryId",
                table: "Sensitivities",
                newName: "IX_Sensitivities_MedicalHistoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sensitivities",
                table: "Sensitivities",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Sensitivities_MedicalHistory_MedicalHistoryId",
                table: "Sensitivities",
                column: "MedicalHistoryId",
                principalTable: "MedicalHistory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sensitivities_MedicalHistory_MedicalHistoryId",
                table: "Sensitivities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Sensitivities",
                table: "Sensitivities");

            migrationBuilder.RenameTable(
                name: "Sensitivities",
                newName: "Sensitivity");

            migrationBuilder.RenameIndex(
                name: "IX_Sensitivities_MedicalHistoryId",
                table: "Sensitivity",
                newName: "IX_Sensitivity_MedicalHistoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sensitivity",
                table: "Sensitivity",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Sensitivity_MedicalHistory_MedicalHistoryId",
                table: "Sensitivity",
                column: "MedicalHistoryId",
                principalTable: "MedicalHistory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
