using Microsoft.EntityFrameworkCore.Migrations;

namespace MedicalInformationSystem.Migrations
{
    public partial class AddHospitalFilesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "File",
                table: "Hospitals");

            migrationBuilder.CreateTable(
                name: "HospitalFiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HospitalId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HospitalFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HospitalFiles_Hospitals_HospitalId",
                        column: x => x.HospitalId,
                        principalTable: "Hospitals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HospitalFiles_HospitalId",
                table: "HospitalFiles",
                column: "HospitalModelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HospitalFiles");

            migrationBuilder.AddColumn<string>(
                name: "File",
                table: "Hospitals",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
