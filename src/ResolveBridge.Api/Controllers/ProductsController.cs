using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResolveBridge.Application.Dtos;
using ResolveBridge.Application.Interfaces;

namespace ResolveBridge.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly IResponseFactory _responseFactory;

    public ProductsController(IProductService productService, IResponseFactory responseFactory)
    {
        _productService = productService;
        _responseFactory = responseFactory;
    }

    [HttpGet("featured")]
    [AllowAnonymous]
    public async Task<ActionResult> GetFeaturedProducts()
    {
        var result = await _productService.GetFeaturedProductsAsync();
        return Ok(_responseFactory.Success(result));
    }

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<ActionResult> GetProduct(Guid id)
    {
        var result = await _productService.GetProductByIdAsync(id);
        if (result == null) return NotFound();
        return Ok(_responseFactory.Success(result));
    }

    [HttpGet("search")]
    [AllowAnonymous]
    public async Task<ActionResult> SearchProducts([FromQuery] ProductFilterRequestDto request)
    {
        var result = await _productService.SearchProductsAsync(request);
        return Ok(_responseFactory.Success(result));
    }

    [HttpGet("recommendations")]
    [Authorize]
    public async Task<ActionResult> GetRecommendations()
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        var result = await _productService.GetRecommendationsAsync(userId);
        return Ok(_responseFactory.Success(result));
    }
}
