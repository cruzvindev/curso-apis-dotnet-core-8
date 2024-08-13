using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;

namespace APICatalogo.Repositories;

public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
{
    public CategoriaRepository(AppDbContext context) : base(context)
    {
    }

    public PagedList<Categoria> GetCategorias(CategoriasParameters categoriasParams)
    {
        var categorias = GetAll().OrderBy(p => p.CategoriaId).AsQueryable(); //Queryable tem uma performance melhor para consultas quando estamos utilizando um provedor como o EF Core
        var categoriasOrdenadas = PagedList<Categoria>.ToPagedList(categorias, categoriasParams.PageNumber, categoriasParams.PageSize);

        return categoriasOrdenadas;
    }

    public PagedList<Categoria> GetCategoriasFiltroNome(CategoriasFiltroNome categoriasParameters)
    {
        var categorias = GetAll().AsQueryable();

        if(!string.IsNullOrEmpty(categoriasParameters.Nome))
        {
            categorias = categorias.Where(c => c.Nome.Contains(categoriasParameters.Nome));
        }

        var categoriasFiltradas = PagedList<Categoria>.ToPagedList(categorias, categoriasParameters.PageNumber, categoriasParameters.PageSize);
        return categoriasFiltradas;
    }
}