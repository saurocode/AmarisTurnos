using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Amaris.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddServiceEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ServiceId",
                table: "Turn",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 250, nullable: false),
                    Active = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Services",
                columns: new[] { "Id", "Active", "Description", "Name" },
                values: new object[,]
                {
                    { 1, true, "Operaciones de caja general", "Ventanilla / Caja" },
                    { 2, true, "Asesoría para clientes comerciales", "Asesoría Comercial" },
                    { 3, true, "Tarjetas, créditos, cuentas, CDT", "Asesoría en Productos" },
                    { 4, true, "Atención preferencial", "Atención Prioritaria" },
                    { 5, true, "Pagos de servicios y retiros", "Pagos / Retiros" },
                    { 6, true, "Consultas e información general", "Información General" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Turn_ServiceId",
                table: "Turn",
                column: "ServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Turn_Services_ServiceId",
                table: "Turn",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Turn_Services_ServiceId",
                table: "Turn");

            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.DropIndex(
                name: "IX_Turn_ServiceId",
                table: "Turn");

            migrationBuilder.DropColumn(
                name: "ServiceId",
                table: "Turn");
        }
    }
}
