using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductCatalogService.Infra.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaCampoInativoMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Inativo",
                table: "Produtos",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Inativo",
                table: "Categorias",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Inativo",
                table: "Produtos");

            migrationBuilder.DropColumn(
                name: "Inativo",
                table: "Categorias");
        }
    }
}
