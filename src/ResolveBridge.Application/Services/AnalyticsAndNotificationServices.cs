using Microsoft.EntityFrameworkCore;
using ResolveBridge.Application.Common;
using ResolveBridge.Application.Dtos;
using ResolveBridge.Application.Interfaces;

namespace ResolveBridge.Application.Services;

public class AnalyticsService : IAnalyticsService
{
    private readonly IApplicationDbContext _context;

    public AnalyticsService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<AnalyticsSummaryDto> GetSummaryAsync(AnalyticsFilterRequestDto filter)
    {
        return new AnalyticsSummaryDto
        {
            TotalUsersCount = await _context.AppUsers.CountAsync(),
            TotalApplications = await _context.Applications.CountAsync(),
            NewUsersCount = await _context.AppUsers.CountAsync(u => u.CreatedAt >= filter.FromDate)
        };
    }

    public async Task<List<InstitutionAnalyticsDto>> GetInstitutionAnalyticsAsync(AnalyticsFilterRequestDto filter)
    {
        return new List<InstitutionAnalyticsDto>();
    }

    public async Task<List<MarketAnalyticsDto>> GetMarketAnalyticsAsync()
    {
        return new List<MarketAnalyticsDto>();
    }
}

public class NotificationService : INotificationService
{
    private readonly IApplicationDbContext _context;

    public NotificationService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<NotificationDto>> GetNotificationsAsync(string userId, NotificationFilterRequestDto filter)
    {
        var items = new List<NotificationDto>();
        return new PagedResult<NotificationDto>(items, 0, filter.PageNumber, filter.PageSize);
    }

    public async Task<bool> MarkAsReadAsync(Guid notificationId)
    {
        return true;
    }

    public async Task<bool> MarkAllAsReadAsync(string userId)
    {
        return true;
    }
}
