using ResolveBridge.Application.Common;
using ResolveBridge.Domain.Enums;

namespace ResolveBridge.Application.Dtos;

public class ApplicationDto
{
    public Guid Id { get; set; }
    public string ApplicationNumber { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string UserEmail { get; set; } = string.Empty;
    public string UserPhoneNumber { get; set; } = string.Empty;
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public ProductType ProductType { get; set; }
    public Guid InstitutionId { get; set; }
    public string InstitutionName { get; set; } = string.Empty;
    public ApplicationStatus Status { get; set; }
    public decimal RequestedAmount { get; set; }
    public int RequestedTenureMonths { get; set; }
    public string Purpose { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public Dictionary<string, string>? Metadata { get; set; } = new();
    public decimal? ApprovedAmount { get; set; }
    public int? ApprovedTenureMonths { get; set; }
    public decimal? InterestRate { get; set; }
    public decimal? MonthlyPayment { get; set; }
    public decimal? TotalRepayment { get; set; }
    public DateTime? DecisionDate { get; set; }
    public string? DecisionNotes { get; set; }
    public string? ReviewedById { get; set; }
    public string? ReviewedByName { get; set; }
    public DateTime? SubmittedAt { get; set; }
    public DateTime? DisbursedAt { get; set; }
    public decimal? DisbursedAmount { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<ApplicationDocumentDto> Documents { get; set; } = new();
    public List<ApplicationStatusHistoryDto> StatusHistory { get; set; } = new();
}

public class ApplicationSummaryDto
{
    public Guid Id { get; set; }
    public string ApplicationNumber { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public ProductType ProductType { get; set; }
    public string InstitutionName { get; set; } = string.Empty;
    public ApplicationStatus Status { get; set; }
    public decimal RequestedAmount { get; set; }
    public DateTime? SubmittedAt { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateApplicationRequestDto
{
    public Guid ProductId { get; set; }
    public decimal RequestedAmount { get; set; }
    public int RequestedTenureMonths { get; set; }
    public string Purpose { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public Dictionary<string, string>? Details { get; set; } = new();
}

public class SubmitApplicationRequestDto
{
    public Guid ApplicationId { get; set; }
}

public class ApplicationDecisionRequestDto
{
    public Guid ApplicationId { get; set; }
    public bool IsApproved { get; set; }
    public decimal? ApprovedAmount { get; set; }
    public int? ApprovedTenureMonths { get; set; }
    public decimal? InterestRate { get; set; }
    public decimal? MonthlyPayment { get; set; }
    public decimal? TotalRepayment { get; set; }
    public string? Notes { get; set; }
}

public class DisburseLoanRequestDto
{
    public Guid ApplicationId { get; set; }
    public decimal DisbursedAmount { get; set; }
    public string? TransactionReference { get; set; }
    public string? Notes { get; set; }
}

public class ApplicationDocumentDto
{
    public Guid Id { get; set; }
    public string DocumentType { get; set; } = string.Empty;
    public string DocumentUrl { get; set; } = string.Empty;
    public string? OriginalFileName { get; set; }
    public long? FileSize { get; set; }
    public string? MimeType { get; set; }
    public bool IsVerified { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class UploadApplicationDocumentRequestDto
{
    public Guid ApplicationId { get; set; }
    public string DocumentType { get; set; } = string.Empty;
    public string FileBase64 { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public string MimeType { get; set; } = string.Empty;
}

public class ApplicationStatusHistoryDto
{
    public Guid Id { get; set; }
    public string OldStatus { get; set; } = string.Empty;
    public string NewStatus { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public string ChangedById { get; set; } = string.Empty;
    public string ChangedByName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class ApplicationFilterRequestDto : PagedRequest
{
    public string? Status { get; set; }
    public string? ProductType { get; set; }
    public Guid? InstitutionId { get; set; }
    public Guid? UserId { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
}
