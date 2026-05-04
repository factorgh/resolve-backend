using ResolveBridge.Application.Common;

namespace ResolveBridge.Application.Dtos;

public class LoanLifecycleDto
{
    public Guid Id { get; set; }
    public Guid ApplicationId { get; set; }
    public string ApplicationNumber { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public string InstitutionName { get; set; } = string.Empty;
    public decimal PrincipalAmount { get; set; }
    public decimal InterestRate { get; set; }
    public int TenureMonths { get; set; }
    public decimal TotalRepaymentAmount { get; set; }
    public decimal OutstandingBalance { get; set; }
    public decimal AmountPaid { get; set; }
    public string PaymentStatus { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime? NextPaymentDate { get; set; }
    public decimal? NextPaymentAmount { get; set; }
    public int InstallmentsPaid { get; set; }
    public int TotalInstallments { get; set; }
    public int LatePayments { get; set; }
    public decimal? LateFeesAccrued { get; set; }
    public bool IsActive { get; set; }
    public DateTime? ClosedAt { get; set; }
    public string? ClosureReason { get; set; }
    public double ProgressPercentage => TotalRepaymentAmount > 0 
        ? (double)(AmountPaid / TotalRepaymentAmount) * 100 
        : 0;
    public List<PaymentDto> Payments { get; set; } = new();
}

public class PaymentDto
{
    public Guid Id { get; set; }
    public string PaymentReference { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public decimal PrincipalPaid { get; set; }
    public decimal InterestPaid { get; set; }
    public decimal? LateFeePaid { get; set; }
    public DateTime PaymentDate { get; set; }
    public DateTime DueDate { get; set; }
    public bool IsOnTime { get; set; }
    public string? PaymentMethod { get; set; }
    public string? TransactionId { get; set; }
    public string? Notes { get; set; }
}

public class RecordPaymentRequestDto
{
    public Guid LoanLifecycleId { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public string? PaymentMethod { get; set; }
    public string? TransactionId { get; set; }
    public string? Notes { get; set; }
}

public class LoanLifecycleSummaryDto
{
    public Guid Id { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string InstitutionName { get; set; } = string.Empty;
    public decimal PrincipalAmount { get; set; }
    public decimal OutstandingBalance { get; set; }
    public decimal AmountPaid { get; set; }
    public string PaymentStatus { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime? NextPaymentDate { get; set; }
    public decimal? NextPaymentAmount { get; set; }
    public int InstallmentsPaid { get; set; }
    public int TotalInstallments { get; set; }
    public bool IsActive { get; set; }
    public double ProgressPercentage { get; set; }
}

public class LoanLifecycleFilterRequestDto : PagedRequest
{
    public Guid? UserId { get; set; }
    public Guid? InstitutionId { get; set; }
    public string? PaymentStatus { get; set; }
    public bool? IsActive { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
}

public class DashboardSummaryDto
{
    public int TotalActiveLoans { get; set; }
    public decimal TotalOutstandingBalance { get; set; }
    public decimal TotalAmountPaid { get; set; }
    public int UpcomingPaymentsCount { get; set; }
    public int OverduePaymentsCount { get; set; }
    public List<UpcomingPaymentDto> UpcomingPayments { get; set; } = new();
    public List<RecentActivityDto> RecentActivities { get; set; } = new();
}

public class UpcomingPaymentDto
{
    public Guid LoanLifecycleId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string InstitutionName { get; set; } = string.Empty;
    public DateTime DueDate { get; set; }
    public decimal Amount { get; set; }
    public int DaysUntilDue { get; set; }
}

public class RecentActivityDto
{
    public string Type { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public decimal? Amount { get; set; }
}
