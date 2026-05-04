using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResolveBridge.Application.Dtos;
using ResolveBridge.Application.Common;
using ResolveBridge.Application.Interfaces;
using System.Security.Claims;

namespace ResolveBridge.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ICurrentUserService _currentUserService;
    private readonly IResponseFactory _responseFactory;

    public UsersController(
        IUserService userService, 
        ICurrentUserService currentUserService,
        IResponseFactory responseFactory)
    {
        _userService = userService;
        _currentUserService = currentUserService;
        _responseFactory = responseFactory;
    }

    [HttpPut("profile")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    public async Task<ActionResult> UpdateProfile([FromBody] UpdateProfileRequestDto request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        var result = await _userService.UpdateProfileAsync(userId, request);
        return Ok(_responseFactory.Success(result));
    }

    [HttpPost("kyc")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    public async Task<ActionResult> SubmitKyc([FromBody] KycSubmissionRequestDto request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        var result = await _userService.SubmitKycAsync(userId, request);
        return Ok(result);
    }

    [HttpGet("documents")]
    [ProducesResponseType(typeof(List<UserDocumentDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetDocuments()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        var result = await _userService.GetUserDocumentsAsync(userId);
        return Ok(_responseFactory.Success(result));
    }

    [HttpGet("dashboard-metrics")]
    [ProducesResponseType(typeof(ApiResponse<DashboardMetricsDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetDashboardMetrics()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        var result = await _userService.GetDashboardMetricsAsync(userId);
        return Ok(_responseFactory.Success(result));
    }
}
