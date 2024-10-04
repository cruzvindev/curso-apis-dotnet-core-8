using APICatalogo.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace ApiCatalogoUnitTests.UnitTests;

public class DeleteProdutoUnitTests : IClassFixture<ProdutosUnitTestController>
{
    private readonly ProdutosController _controller;

    public DeleteProdutoUnitTests(ProdutosUnitTestController controller)
    {
        _controller = new ProdutosController(controller.repository, controller.mapper);
    }

    [Fact]
    public async Task DeleteProdutoById_Return_OkResult()
    {
        var prodId = 2;

        var result = await _controller.Delete(prodId);

        result.Should().NotBeNull();
        result.Result.Should().BeOfType<OkObjectResult>();
    }
    
    [Fact]
    public async Task DeleteProdutoById_Return_NotFound()
    {
        var prodId = 999;

        var result = await _controller.Delete(prodId);

        result.Should().NotBeNull();
        result.Result.Should().BeOfType<NotFoundObjectResult>();
    }
}