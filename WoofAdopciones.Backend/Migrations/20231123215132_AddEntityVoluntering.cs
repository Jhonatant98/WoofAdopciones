using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WoofAdopciones.Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddEntityVoluntering : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Stock",
                table: "Pets");

            migrationBuilder.AddColumn<bool>(
                name: "state",
                table: "Pets",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "RequestVolunteering",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CityId = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequestStauts = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestVolunteering", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RequestVolunteering_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RequestVolunteering_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RequestVolunteering_CityId",
                table: "RequestVolunteering",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestVolunteering_UserId",
                table: "RequestVolunteering",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RequestVolunteering");

            migrationBuilder.DropColumn(
                name: "state",
                table: "Pets");

            migrationBuilder.AddColumn<float>(
                name: "Stock",
                table: "Pets",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }
    }
}
