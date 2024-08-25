using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ControleNotasFiscais.Migrations
{
    /// <inheritdoc />
    public partial class UpdateNotaFiscalModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Empresa",
                table: "NotasFiscais");

            migrationBuilder.AddColumn<int>(
                name: "EmpresaId",
                table: "NotasFiscais",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_NotasFiscais_EmpresaId",
                table: "NotasFiscais",
                column: "EmpresaId");

            migrationBuilder.AddForeignKey(
                name: "FK_NotasFiscais_Empresas_EmpresaId",
                table: "NotasFiscais",
                column: "EmpresaId",
                principalTable: "Empresas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NotasFiscais_Empresas_EmpresaId",
                table: "NotasFiscais");

            migrationBuilder.DropIndex(
                name: "IX_NotasFiscais_EmpresaId",
                table: "NotasFiscais");

            migrationBuilder.DropColumn(
                name: "EmpresaId",
                table: "NotasFiscais");

            migrationBuilder.AddColumn<string>(
                name: "Empresa",
                table: "NotasFiscais",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
