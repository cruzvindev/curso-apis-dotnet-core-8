using APICatalogo.Models;
using APICatalogo.Pagination;
using X.PagedList;

namespace APICatalogo.Repositories;

public interface IProdutoRepository : IRepository<Produto>
{
    Task<IEnumerable<Produto>>GetProdutosPorCategoriaAsync(int id);
    Task<X.PagedList.IPagedList<Produto>> GetProdutosAsync(ProdutosParameters produtos);
    Task<X.PagedList.IPagedList<Produto>> GetProdutosFiltroPrecoAsync(ProdutosFiltroPreco produtosFiltroParams);
}