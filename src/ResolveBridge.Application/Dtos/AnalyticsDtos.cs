namespace ResolveBridge.Application.Dtos;

public class AnalyticsSummaryDto
{
    public DateTime PeriodStart { get; set; }
    public DateTime PeriodEnd { get; set; }
    public int TotalApplications { get; set; }
    public int ApprovedApplications { get; set; }
    public int RejectedApplications { get; set; }
    public int PendingApplications { get; set; }
    public decimal TotalLoanAmount { get; set; }
    public decimal TotalDisbursedAmount { get; set; }
    public decimal TotalRepaymentsReceived { get; set; }
    public int ActiveLoansCount { get; set; }
    public int NewUsersCount { get; set; }
    public int TotalUsersCount { get; set; }
}

public class InstitutionAnalyticsDto
{
    public Guid InstitutionId { get; set; }
    public string InstitutionName { get; set; } = string.Empty;
    public int TotalProducts { get; set; }
    public int TotalApplications { get; set; }
    public int ApprovedApplications { get; set; }
    public int RejectedApplications { get; set; }
    public decimal TotalDisbursedAmount { get; set; }
    public decimal TotalRepaymentsReceived { get; set; }
    public decimal OutstandingPortfolio { get; set; }
    public double ApprovalRate { get; set; }
}

public class ProductPerformanceDto
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string ProductType { get; set; } = string.Empty;
    public int TotalApplications { get; set; }
    public int ApprovedApplications { get; set; }
    public decimal TotalApprovedAmount { get; set; }
    public double ConversionRate { get; set; }
}

public class MonthlyTrendDto
{
    public string Month { get; set; } = string.Empty;
    public int ApplicationsCount { get; set; }
    public int ApprovedCount { get; set; }
    public decimal DisbursedAmount { get; set; }
    public decimal RepaymentsReceived { get; set; }
}

public class MarketAnalyticsDto
{
    public string Market { get; set; } = string.Empty;
    public int TotalUsers { get; set; }
    public int TotalApplications { get; set; }
    public decimal TotalLoanVolume { get; set; }
    public int ActiveInstitutions { get; set; }
}

public class AnalyticsFilterRequestDto
{
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public Guid? InstitutionId { get; set; }
    public string? ProductType { get; set; }
    public string? Market { get; set; }
}
