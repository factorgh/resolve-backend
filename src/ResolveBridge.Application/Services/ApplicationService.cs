using Microsoft.EntityFrameworkCore;
using ResolveBridge.Application.Common;
using ResolveBridge.Application.Dtos;
using ResolveBridge.Application.Interfaces;
using ResolveBridge.Domain.Entities;
using ResolveBridge.Domain.Enums;

namespace ResolveBridge.Application.Services;

public class ApplicationService : IApplicationService
{
    private readonly IApplicationDbContext _context;
    private readonly IDateTimeService _dateTimeService;

    public ApplicationService(IApplicationDbContext context, IDateTimeService dateTimeService)
    {
        _context = context;
        _dateTimeService = dateTimeService;
    }

    public async Task<ApiResponse<ApplicationDto>> CreateApplicationAsync(string userId, CreateApplicationRequestDto request)
    {
        var application = new Domain.Entities.Application
        {
            Id = Guid.NewGuid(),
            UserId = Guid.Parse(userId),
            ProductId = request.ProductId,
            RequestedAmount = request.RequestedAmount,
            RequestedTenureMonths = request.RequestedTenureMonths,
            Status = ApplicationStatus.Draft,
            CreatedAt = _dateTimeService.UtcNow
        };

        _context.Applications.Add(application);
        await _context.SaveChangesAsync(default);

        return new ApiResponse<ApplicationDto>
        {
            Success = true,
            Data = new ApplicationDto { Id = application.Id, Status = application.Status },
            Message = "Application created successfully"
        };
    }

    public async Task<ApiResponse<bool>> SubmitApplicationAsync(Guid applicationId)
    {
        var application = await _context.Applications.FindAsync(applicationId);
        if (application == null) return new ApiResponse<bool> { Success = false, Message = "Application not found" };

        application.Status = ApplicationStatus.Submitted;
        application.SubmittedAt = _dateTimeService.UtcNow;

        await _context.SaveChangesAsync(default);

        return new ApiResponse<bool> { Success = true, Data = true, Message = "Application submitted successfully" };
    }

    public async Task<ApiResponse<bool>> ProcessDecisionAsync(Guid applicationId, ApplicationDecisionRequestDto request)
    {
        var application = await _context.Applications.FindAsync(applicationId);
        if (application == null) return new ApiResponse<bool> { Success = false, Message = "Application not found" };

        application.Status = request.IsApproved ? ApplicationStatus.Approved : ApplicationStatus.Rejected;
        application.DecisionDate = _dateTimeService.UtcNow;
        application.DecisionNotes = request.Notes;
        
        await _context.SaveChangesAsync(default);

        return new ApiResponse<bool> { Success = true, Data = true, Message = $"Application {application.Status}" };
    }

    public async Task<ApplicationDto> GetApplicationByIdAsync(Guid id)
    {
        var app = await _context.Applications
            .FirstOrDefaultAsync(a => a.Id == id);
            
        if (app == null) return new ApplicationDto();

        return new ApplicationDto
        {
            Id = app.Id,
            Status = app.Status,
            RequestedAmount = app.RequestedAmount,
            CreatedAt = app.CreatedAt
        };
    }

    public async Task<PagedResult<ApplicationSummaryDto>> GetUserApplicationsAsync(string userId, PagedRequest request, string? type = null)
    {
        var userGuid = Guid.Parse(userId);
        var query = _context.Applications
            .Where(a => a.UserId == userGuid);

        var total = await query.CountAsync();
        var items = await query
            .OrderByDescending(a => a.CreatedAt)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(a => new ApplicationSummaryDto
            {
                Id = a.Id,
                Status = a.Status,
                RequestedAmount = a.RequestedAmount,
                CreatedAt = a.CreatedAt
            })
            .ToListAsync();

        return new PagedResult<ApplicationSummaryDto>(items, total, request.PageNumber, request.PageSize);
    }
}
