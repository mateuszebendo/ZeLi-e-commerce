using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductCatalogService.Infra.Migrations
{
    /// <inheritdoc />
    public partial class MudaInativoParaAtivoMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Inativo",
                table: "Categorias",
                newName: "Ativo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Ativo",
                table: "Categorias",
                newName: "Inativo");
        }
    }
}
