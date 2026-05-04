using Microsoft.EntityFrameworkCore;
using ResolveBridge.Application.Common;
using ResolveBridge.Application.Dtos;
using ResolveBridge.Application.Interfaces;

namespace ResolveBridge.Application.Services;

public class LoanService : ILoanService
{
    private readonly IApplicationDbContext _context;

    public LoanService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<LoanLifecycleSummaryDto>> GetLoansAsync(LoanLifecycleFilterRequestDto filter)
    {
        return new PagedResult<LoanLifecycleSummaryDto>(new List<LoanLifecycleSummaryDto>(), 0, filter.PageNumber, filter.PageSize);
    }

    public async Task<List<PaymentDto>> GetPaymentsAsync(Guid loanId)
    {
        return new List<PaymentDto>();
    }

    public async Task<PaymentDto> RecordPaymentAsync(RecordPaymentRequestDto request)
    {
        return new PaymentDto { Id = Guid.NewGuid() };
    }

    public async Task<DashboardSummaryDto> GetDashboardSummaryAsync(string userId)
    {
        return new DashboardSummaryDto();
    }
}
