using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResolveBridge.Application.Dtos;
using ResolveBridge.Application.Interfaces;

namespace ResolveBridge.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize(Roles = "SuperAdmin,DataAnalyst,InstitutionAdmin")]
public class AnalyticsController : ControllerBase
{
    private readonly IAnalyticsService _analyticsService;
    private readonly IResponseFactory _responseFactory;

    public AnalyticsController(IAnalyticsService analyticsService, IResponseFactory responseFactory)
    {
        _analyticsService = analyticsService;
        _responseFactory = responseFactory;
    }

    [HttpGet("summary")]
    public async Task<ActionResult> GetSummary([FromQuery] AnalyticsFilterRequestDto filter)
    {
        var result = await _analyticsService.GetSummaryAsync(filter);
        return Ok(_responseFactory.Success(result));
    }

    [HttpGet("markets")]
    public async Task<ActionResult> GetMarketAnalytics()
    {
        var result = await _analyticsService.GetMarketAnalyticsAsync();
        return Ok(_responseFactory.Success(result));
    }
}
