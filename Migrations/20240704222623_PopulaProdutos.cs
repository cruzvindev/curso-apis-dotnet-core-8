using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APICatalogo.Migrations
{
    /// <inheritdoc />
    public partial class PopulaProdutos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Insert into Produtos(Nome, Descricao, Preco, ImagemUrl, Estoque, DataCadastro, CategoriaId) Values ('Coca-Cola Diet', 'Refrigerante de Cola 350ml', 5.45, 'cocacola.jpg', 50, now(), 1)");
            migrationBuilder.Sql("Insert into Produtos(Nome, Descricao, Preco, ImagemUrl, Estoque, DataCadastro, CategoriaId) Values ('Sanduíche Natural', 'Sanduíche com frango e salada', 12.00, 'sanduiche_natural.jpg', 30, now(), 2)");
            migrationBuilder.Sql("Insert into Produtos(Nome, Descricao, Preco, ImagemUrl, Estoque, DataCadastro, CategoriaId) Values ('Bolo de Chocolate', 'Delicioso bolo de chocolate', 25.00, 'bolo_chocolate.jpg', 20, now(), 3)");
            migrationBuilder.Sql("Insert into Produtos(Nome, Descricao, Preco, ImagemUrl, Estoque, DataCadastro, CategoriaId) Values ('Salada Caesar', 'Salada com alface, frango e molho Caesar', 15.00, 'salada_caesar.jpg', 15, now(), 4)");
            migrationBuilder.Sql("Insert into Produtos(Nome, Descricao, Preco, ImagemUrl, Estoque, DataCadastro, CategoriaId) Values ('Lasanha de Carne', 'Lasanha recheada com carne moída e queijo', 35.00, 'lasanha_carne.jpg', 10, now(), 5)");
            migrationBuilder.Sql("Insert into Produtos(Nome, Descricao, Preco, ImagemUrl, Estoque, DataCadastro, CategoriaId) Values ('Filé Mignon', 'Filé mignon grelhado com molho', 50.00, 'file_mignon.jpg', 8, now(), 6)");
            migrationBuilder.Sql("Insert into Produtos(Nome, Descricao, Preco, ImagemUrl, Estoque, DataCadastro, CategoriaId) Values ('Camarão ao Alho e Óleo', 'Camarões fritos ao alho e óleo', 40.00, 'camarao.jpg', 12, now(), 7)");
            migrationBuilder.Sql("Insert into Produtos(Nome, Descricao, Preco, ImagemUrl, Estoque, DataCadastro, CategoriaId) Values ('Sopa de Legumes', 'Sopa quente de legumes variados', 10.00, 'sopa_legumes.jpg', 25, now(), 8)");
            migrationBuilder.Sql("Insert into Produtos(Nome, Descricao, Preco, ImagemUrl, Estoque, DataCadastro, CategoriaId) Values ('Batata Frita', 'Porção de batata frita crocante', 8.00, 'batata_frita.jpg', 40, now(), 9)");
            migrationBuilder.Sql("Insert into Produtos(Nome, Descricao, Preco, ImagemUrl, Estoque, DataCadastro, CategoriaId) Values ('Hambúrguer Vegetariano', 'Hambúrguer feito com grão-de-bico e vegetais', 15.00, 'hamburguer_vegetariano.jpg', 22, now(), 10)");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Delete from Produtos");
        }
    }
}
