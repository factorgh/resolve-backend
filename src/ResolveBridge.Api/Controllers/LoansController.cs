using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResolveBridge.Application.Dtos;
using ResolveBridge.Application.Common;
using ResolveBridge.Application.Interfaces;
using System.Security.Claims;

namespace ResolveBridge.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class LoansController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly IApplicationService _applicationService;
    private readonly IResponseFactory _responseFactory;

    public LoansController(
        IProductService productService, 
        IApplicationService applicationService,
        IResponseFactory responseFactory)
    {
        _productService = productService;
        _applicationService = applicationService;
        _responseFactory = responseFactory;
    }

    [HttpGet("products")]
    [AllowAnonymous]
    public async Task<ActionResult> GetProducts()
    {
        var result = await _productService.SearchProductsAsync(new ProductFilterRequestDto { });
        return Ok(_responseFactory.Success(result));
    }

    [HttpGet("my-loans")]
    [Authorize]
    public async Task<ActionResult> GetMyLoans([FromQuery] PagedRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        var result = await _applicationService.GetUserApplicationsAsync(userId, request, "Loan");
        return Ok(_responseFactory.Success(result));
    }

    [HttpPost("apply")]
    [Authorize]
    public async Task<ActionResult> Apply([FromBody] CreateApplicationRequestDto request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        var result = await _applicationService.CreateApplicationAsync(userId, request);
        return Ok(result);
    }
}
