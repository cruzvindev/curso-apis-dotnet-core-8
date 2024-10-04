using System.Linq.Expressions;
using APICatalogo.Controllers;
using APICatalogo.DTOs;
using APICatalogo.Models;
using APICatalogo.Repositories;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace ApiCatalogoUnitTests.UnitTests;

public class GetProdutoUnitTests : IClassFixture<ProdutosUnitTestController>
{
    private readonly ProdutosController _controller;
    private readonly Mock<ProdutoRepository> _mockRepository;

    public GetProdutoUnitTests(ProdutosUnitTestController controller)
    {
        _controller = new ProdutosController(controller.repository, controller.mapper);
        _mockRepository = new Mock<ProdutoRepository>();
    }

    [Fact]
    public async Task GetProdutoById_OKResult()
    {
        var produtoId = 2;

        var data = await _controller.Get(produtoId);

        /* xUnit
        var okResult = Assert.IsType<OkObjectResult>(data.Result);
        Assert.Equal(200, okResult.StatusCode);
        */
        
        //FluentAssertion
        data.Result.Should().BeOfType<OkObjectResult>()
            .Which.StatusCode.Should().Be(200);
    }

    [Fact]
    public async Task GetProdutoyId_Return_NotFound()
    {
        var prodId = 999;

        var data = await _controller.Get(prodId);

        data.Result.Should().BeOfType<NotFoundObjectResult>().Which.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task GetProdutos_Return_ListOfProdutoDTO()
    {
        var data = await _controller.Get();

        data.Result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeAssignableTo<IEnumerable<ProdutoDTO>>().And.NotBeNull();
    }
    
}