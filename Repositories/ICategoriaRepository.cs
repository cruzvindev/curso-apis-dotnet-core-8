using APICatalogo.Models;
using APICatalogo.Pagination;
using X.PagedList;

namespace APICatalogo.Repositories;

public interface ICategoriaRepository : IRepository<Categoria>
{
    Task<X.PagedList.IPagedList<Categoria>> GetCategoriasAsync(CategoriasParameters categoriasParameters);
    Task<X.PagedList.IPagedList<Categoria>> GetCategoriasFiltroNomeAsync(CategoriasFiltroNome categoriasParameters);
}