using Microsoft.EntityFrameworkCore.Migrations;

namespace MedicalInformationSystem.Migrations
{
    public partial class MakeBloodIdNullableInUsersTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Blood_BloodId",
                table: "AspNetUsers");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Blood_BloodId",
                table: "AspNetUsers",
                column: "BloodId",
                principalTable: "Blood",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Blood_BloodId",
                table: "AspNetUsers");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Blood_BloodId",
                table: "AspNetUsers",
                column: "BloodId",
                principalTable: "Blood",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
