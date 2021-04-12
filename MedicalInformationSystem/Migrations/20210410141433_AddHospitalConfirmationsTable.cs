using Microsoft.EntityFrameworkCore.Migrations;

namespace MedicalInformationSystem.Migrations
{
    public partial class AddHospitalConfirmationsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HospitalConfirmationId",
                table: "HospitalFiles",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "HospitalConfirmations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HospitalConfirmations", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HospitalFiles_HospitalConfirmationId",
                table: "HospitalFiles",
                column: "HospitalConfirmationId");

            migrationBuilder.AddForeignKey(
                name: "FK_HospitalFiles_HospitalConfirmations_HospitalConfirmationId",
                table: "HospitalFiles",
                column: "HospitalConfirmationId",
                principalTable: "HospitalConfirmations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HospitalFiles_HospitalConfirmations_HospitalConfirmationId",
                table: "HospitalFiles");

            migrationBuilder.DropTable(
                name: "HospitalConfirmations");

            migrationBuilder.DropIndex(
                name: "IX_HospitalFiles_HospitalConfirmationId",
                table: "HospitalFiles");

            migrationBuilder.DropColumn(
                name: "HospitalConfirmationId",
                table: "HospitalFiles");
        }
    }
}
