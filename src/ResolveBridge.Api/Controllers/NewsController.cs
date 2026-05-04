using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResolveBridge.Application.Dtos;
using ResolveBridge.Application.Common;
using ResolveBridge.Application.Interfaces;

namespace ResolveBridge.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class NewsController : ControllerBase
{
    private readonly INewsService _newsService;
    private readonly IResponseFactory _responseFactory;

    public NewsController(INewsService newsService, IResponseFactory responseFactory)
    {
        _newsService = newsService;
        _responseFactory = responseFactory;
    }

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<List<NewsArticleDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetArticles()
    {
        var result = await _newsService.GetPublishedArticlesAsync();
        return Ok(_responseFactory.Success(result));
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<NewsArticleDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetArticle(Guid id)
    {
        var result = await _newsService.GetArticleByIdAsync(id);
        return Ok(_responseFactory.Success(result));
    }

    [HttpPost]
    [Authorize(Roles = "SuperAdmin")]
    [ProducesResponseType(typeof(ApiResponse<NewsArticleDto>), StatusCodes.Status201Created)]
    public async Task<ActionResult> CreateArticle([FromBody] CreateNewsArticleRequestDto request)
    {
        var result = await _newsService.CreateArticleAsync(request);
        return CreatedAtAction(nameof(GetArticle), new { id = result.Id }, _responseFactory.Success(result));
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "SuperAdmin")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    public async Task<ActionResult> DeleteArticle(Guid id)
    {
        var result = await _newsService.DeleteArticleAsync(id);
        return Ok(_responseFactory.Success(result));
    }
}
