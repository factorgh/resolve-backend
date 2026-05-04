using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResolveBridge.Application.Dtos;
using ResolveBridge.Application.Interfaces;
using System.Security.Claims;

namespace ResolveBridge.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class LoanLifecycleController : ControllerBase
{
    private readonly ILoanService _loanService;
    private readonly IResponseFactory _responseFactory;

    public LoanLifecycleController(ILoanService loanService, IResponseFactory responseFactory)
    {
        _loanService = loanService;
        _responseFactory = responseFactory;
    }

    [HttpGet]
    public async Task<ActionResult> GetLoans([FromQuery] LoanLifecycleFilterRequestDto filter)
    {
        var result = await _loanService.GetLoansAsync(filter);
        return Ok(_responseFactory.Success(result));
    }

    [HttpGet("dashboard")]
    public async Task<ActionResult> GetDashboard()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        var result = await _loanService.GetDashboardSummaryAsync(userId);
        return Ok(_responseFactory.Success(result));
    }
}
