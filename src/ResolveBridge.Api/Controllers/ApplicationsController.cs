using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResolveBridge.Application.Common;
using ResolveBridge.Application.Dtos;
using ResolveBridge.Application.Interfaces;
using System.Security.Claims;

namespace ResolveBridge.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class ApplicationsController : ControllerBase
{
    private readonly IApplicationService _applicationService;
    private readonly IResponseFactory _responseFactory;

    public ApplicationsController(IApplicationService applicationService, IResponseFactory responseFactory)
    {
        _applicationService = applicationService;
        _responseFactory = responseFactory;
    }

    [HttpGet("{id:guid}")]
    [Authorize(Roles = "SuperAdmin,InstitutionAdmin,InstitutionAgent,Customer")]
    public async Task<ActionResult> GetApplication(Guid id)
    {
        var result = await _applicationService.GetApplicationByIdAsync(id);
        return Ok(_responseFactory.Success(result));
    }

    [HttpGet("my-applications")]
    public async Task<ActionResult> GetMyApplications([FromQuery] PagedRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        var result = await _applicationService.GetUserApplicationsAsync(userId, request);
        return Ok(_responseFactory.Success(result));
    }

    [HttpPost]
    public async Task<ActionResult> CreateApplication([FromBody] CreateApplicationRequestDto request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        var result = await _applicationService.CreateApplicationAsync(userId, request);
        return Ok(result);
    }

    [HttpPost("{id:guid}/submit")]
    public async Task<ActionResult> SubmitApplication(Guid id)
    {
        var result = await _applicationService.SubmitApplicationAsync(id);
        return Ok(result);
    }
}
