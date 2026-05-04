using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResolveBridge.Application.Common;
using ResolveBridge.Application.Dtos;
using ResolveBridge.Domain.Enums;

namespace ResolveBridge.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class InstitutionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public InstitutionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(InstitutionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<InstitutionDto>> GetInstitution(Guid id)
    {
        return Ok(new InstitutionDto());
    }

    [HttpGet("{id:guid}/users")]
    [Authorize(Roles = "SuperAdmin,InstitutionAdmin")]
    [ProducesResponseType(typeof(PagedResult<InstitutionUserDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResult<InstitutionUserDto>>> GetInstitutionUsers(
        Guid id, [FromQuery] PagedRequest request)
    {
        return Ok(new PagedResult<InstitutionUserDto>());
    }
}
