using APICatalogo.Models;
using APICatalogo.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers;

[Route("[controller]")]
[ApiController]
public class ProdutosController : ControllerBase
{
    private readonly IRepository<Produto> _repository;
    private readonly IProdutoRepository _produtoRepository;
    public ProdutosController(IRepository<Produto> repository, IProdutoRepository produtoRepository)
    {
        _repository = repository;
        _produtoRepository = produtoRepository;
    }

    [HttpGet("produtos/{id}")]
    public ActionResult<IEnumerable<Produto>> GetProdutosCategoria(int id)
    {
       var produtos =  _produtoRepository.GetProdutosPorCategoria(id);

        if(produtos is null)
            return NotFound();

        return Ok(produtos);
    }

    [HttpGet]
    public ActionResult<IEnumerable<Produto>> Get()
    {
        var produtos = _produtoRepository.GetAll();
        if (produtos is null)
        {
            return NotFound();
        }
        return Ok(produtos);
    }

    [HttpGet("{id}", Name = "ObterProduto")]
    public ActionResult<Produto> Get(int id)
    {
        var produto = _produtoRepository.Get(c => c.ProdutoId == id);
        if (produto is null)
        {
            return NotFound("Produto não encontrado...");
        }
        return Ok(produto);
    }

    [HttpPost]
    public ActionResult Post(Produto produto)
    {
        if (produto is null)
            return BadRequest();

       var novoProduto = _produtoRepository.Create(produto);

        return new CreatedAtRouteResult("ObterProduto",
            new { id = produto.ProdutoId }, produto);
    }

    [HttpPut("{id:int}")]
    public ActionResult Put(int id, Produto produto)
    {
        if (id != produto.ProdutoId)
        {
            return BadRequest();
        }

       var produtoAtualizado = _produtoRepository.Update(produto);

       return Ok(produtoAtualizado);
    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
       var produto = _produtoRepository.Get(p => p.ProdutoId == id);

        if (produto is null)
        {
            return NotFound("Produto não encontrado...");
        }

       var produtoDeletado =  _repository.Delete(produto);
       return Ok(produtoDeletado);
    }
}
