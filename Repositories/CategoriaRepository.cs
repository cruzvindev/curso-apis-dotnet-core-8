using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;
using X.PagedList;

namespace APICatalogo.Repositories;

public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
{
    public CategoriaRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<X.PagedList.IPagedList<Categoria>> GetCategoriasAsync(CategoriasParameters categoriasParams)
    {
        var categorias = await GetAllAsync();
        var categoriasOrdenadas = categorias.OrderBy(p => p.CategoriaId).AsQueryable();
       // var resultado = PagedList<Categoria>.ToPagedList(categoriasOrdenadas, categoriasParams.PageNumber, categoriasParams.PageSize);
       var resultado = await categoriasOrdenadas.ToPagedListAsync(categoriasParams.PageNumber, categoriasParams.PageSize);

        return resultado;
    }

    public async Task<X.PagedList.IPagedList<Categoria>> GetCategoriasFiltroNomeAsync(CategoriasFiltroNome categoriasParameters)
    {
        var categorias = await GetAllAsync();

        if(!string.IsNullOrEmpty(categoriasParameters.Nome))
        {
            categorias = categorias.Where(c => c.Nome.Contains(categoriasParameters.Nome));
        }

        //var categoriasFiltradas = PagedList<Categoria>.ToPagedList(categorias.AsQueryable(), categoriasParameters.PageNumber, categoriasParameters.PageSize);

        var categoriasFiltradas = await categorias.ToPagedListAsync(categoriasParameters.PageNumber, categoriasParameters.PageSize);

        return categoriasFiltradas;
    }
}