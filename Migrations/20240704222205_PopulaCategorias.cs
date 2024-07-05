using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APICatalogo.Migrations
{
    /// <inheritdoc />
    public partial class PopulaCategorias : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Insert into Categorias(Nome, ImagemUrl) Values ('Bebidas', 'bebidas.jpg')");
            migrationBuilder.Sql("Insert into Categorias(Nome, ImagemUrl) Values ('Lanches', 'lanches.jpg')");
            migrationBuilder.Sql("Insert into Categorias(Nome, ImagemUrl) Values ('Sobremesas', 'sobremesas.jpg')");
            migrationBuilder.Sql("Insert into Categorias(Nome, ImagemUrl) Values ('Saladas', 'saladas.jpg')");
            migrationBuilder.Sql("Insert into Categorias(Nome, ImagemUrl) Values ('Massas', 'massas.jpg')");
            migrationBuilder.Sql("Insert into Categorias(Nome, ImagemUrl) Values ('Carnes', 'carnes.jpg')");
            migrationBuilder.Sql("Insert into Categorias(Nome, ImagemUrl) Values ('Frutos do Mar', 'frutos_do_mar.jpg')");
            migrationBuilder.Sql("Insert into Categorias(Nome, ImagemUrl) Values ('Sopas', 'sopas.jpg')");
            migrationBuilder.Sql("Insert into Categorias(Nome, ImagemUrl) Values ('Aperitivos', 'aperitivos.jpg')");
            migrationBuilder.Sql("Insert into Categorias(Nome, ImagemUrl) Values ('Vegetarianos', 'vegetarianos.jpg')");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Delete from Categorias");
        }
    }
}
